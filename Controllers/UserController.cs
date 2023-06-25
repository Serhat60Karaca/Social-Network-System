using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TESNS.Models;
using TESNS.Models.Authentication;
using TESNS.ViewModels;
using MailKit.Net.Smtp;
using System.Security.Cryptography;

namespace TESNS.Controllers
{
    public class UserController:Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ApplicationDbContext _context;
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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

            if (user != null)
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
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = appUserViewModel.UserName,
                    Email = appUserViewModel.Email
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Password);
                if (result.Succeeded)
                    return RedirectToAction("Login","User");
                else
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            return View();
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
                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress("SNS", "emirdeveci55@gmail.com");
                MailboxAddress mailboxAddressTo = new MailboxAddress("User", logVM.Email);
                mimeMessage.From.Add(mailboxAddressFrom);
                mimeMessage.To.Add(mailboxAddressTo);
                var bodyBuilder = new BodyBuilder();
                user.PasswordResetToken = CreateRandomToken();
                _context.SaveChanges();
                bodyBuilder.TextBody = "reset password code:" + user.PasswordResetToken;
                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessage.Subject = "Password Reset Code";
                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("emirdeveci55@gmail.com", "yngcadpphqqnjqrs");
                client.Send(mimeMessage);
                client.Disconnect(true);
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
            return RedirectToAction("Login");
        }
    }
}
