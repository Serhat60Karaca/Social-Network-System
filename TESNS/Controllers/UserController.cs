using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TESNS.Models;
using TESNS.Models.Authentication;
using TESNS.ViewModels;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using TESNS.Services;

namespace TESNS.Controllers
{
    public class UserController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ISendEmailService _sendEmailService;
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ApplicationDbContext context,ISendEmailService sendEmailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _sendEmailService = sendEmailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LoginVM)
        {
            if (!ModelState.IsValid) return View(LoginVM);

            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == LoginVM.Email);
            var user = await _userManager.FindByEmailAsync(LoginVM.Email);
            //var user = _context.Users.FirstOrDefault(x => x.Email == LoginVM.Email);

            if (user != null && user.IsEmailConfirmed==true)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, LoginVM.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUserViewModel appUserViewModel)
        {
            var user = await _userManager.FindByEmailAsync(appUserViewModel.Email);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(appUserViewModel);
            }
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    //FirstName = appUserViewModel.FirstName,
                    //LastName = appUserViewModel.LastName,

                    UserName = appUserViewModel.UserName,
                    Email = appUserViewModel.Email,
                    PhoneNumber= appUserViewModel.PhoneNumber,
                    BirthDate = appUserViewModel.BirthDate,
                    Cinsiyet = appUserViewModel.Gender
                };
                if (appUser.ProfilePhoto == null)
                {
                    appUser.ProfilePhoto = "http://res.cloudinary.com/daw0gahvl/image/upload/v1687924653/upz1ztj9ue8a7thqdzie.jpg";
                }
                if (appUser.CoverPhoto == null)
                {
                    appUser.ProfilePhoto = "https://res.cloudinary.com/daw0gahvl/image/upload/v1687712292/samples/balloons.jpg";
                }
                IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Password);
                if (result.Succeeded) { 
                _sendEmailService.SendEmail(appUser.Email, appUser);
                return View("VerifyUser");
                }
                else
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> VerifyUser() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyUser(VerifyUserViewModel verifyVM)
        {
            if (!ModelState.IsValid) return View();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == verifyVM.Code);//tricky,verifying using reset token
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                _context.Update(user);
                _context.SaveChanges();
                return View("Login");
            }
            ViewData["Wrong"] = "Your code is wrong please try again";
            return View(verifyVM.Code);
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(LoginViewModel logVM)
        {
            //var user = _userManager.FirstOrDefault(x => x.Email == logVM.Email);
            var user = await _userManager.FindByEmailAsync(logVM.Email);
            if (user == null) { return RedirectToAction("Login", "User"); }
            if (user != null)
            {
                _sendEmailService.SendEmail(logVM.Email, user);
                return View("ChangePassword");
            }
            return View("ChangePassword");
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changeVM)
        {
            if (!ModelState.IsValid) return View(changeVM);
            var user = _userManager.Users.FirstOrDefault(x => x.PasswordResetToken == changeVM.Code);
            if (user != null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, changeVM.NewPassword);

                ViewData["congrats"] = "changed your password";
                return View("Login");
            }
            ViewData["error"] = "your information is wrong";
            return View(changeVM);
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
