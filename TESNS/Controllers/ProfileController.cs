﻿using Microsoft.AspNetCore.Identity;
using TESNS.Models.Authentication;
using TESNS.Models;
using Microsoft.AspNetCore.Mvc;
using TESNS.ViewModels;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authorization;
using TESNS.Services;
using TESNS.Repositories;

namespace TESNS.Controllers
{
    public class ProfileController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            ApplicationDbContext context,IPhotoService photoService,IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _photoService = photoService;
            _userRepository = userRepository;
        }
        [HttpGet]
        [Route("Profile/ProfileDetail/{id}")]
        public async Task<IActionResult> ProfileDetail(int id)
        {
            var user = await _userRepository.GetUserById(id);
            //var user = await _userManager.GetUserAsync(User);
            if (user == null) return View();
            var userDetailVM = new UserDetailViewModel()
            {
                //FirstName=user.FirstName,
                //LastName=user.LastName,
                Id = id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Cinsiyet,
                ProfilePhoto= user.ProfilePhoto 
            };
            return View(userDetailVM);
        }
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> EditProfile() 
        {
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return View();
            if (user.ProfilePhoto == null) 
            {
                user.ProfilePhoto = "http://res.cloudinary.com/daw0gahvl/image/upload/v1687924653/upz1ztj9ue8a7thqdzie.jpg";
            }
            var userEditVM = new EditProfileViewModel()
            {
                //FirstName=user.FirstName,
                //LastName= user.LastName,
                
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Gender = user.Cinsiyet,
                ProfilePhotoUrl = user.ProfilePhoto 
            };
            
            return View(userEditVM);
        }
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileVM)
        {
            var currentPhoto = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editProfileVM);
            }
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View("Error");
            }
            if (editProfileVM.ProfilePhoto != null) {
                var result = await _photoService.AddPhotoAsync(editProfileVM.ProfilePhoto);
                if (result.Error != null)
                {
                    ModelState.AddModelError("", "Failed to upload photo");
                }
                if (!string.IsNullOrEmpty(user.ProfilePhoto))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfilePhoto);
                }
                user.ProfilePhoto = result.Url.ToString();
                editProfileVM.ProfilePhotoUrl = user.ProfilePhoto;
                await _userManager.UpdateAsync(user);
            }
            //user.FirstName = editProfileVM.FirstName;
            //user.LastName = editProfileVM.LastName;
            
            user.UserName = editProfileVM.UserName;
            user.PhoneNumber = editProfileVM.PhoneNumber;
            user.BirthDate = editProfileVM.BirthDate;
            user.Cinsiyet = editProfileVM.Gender;
            if (editProfileVM.ProfilePhotoUrl == null) 
            {
                editProfileVM.ProfilePhotoUrl = currentPhoto.ProfilePhoto;
            }
            user.ProfilePhoto = editProfileVM.ProfilePhotoUrl;
            await _userManager.UpdateAsync(user);
            
          return View("EditProfile",editProfileVM);
        }
        public async Task<IActionResult> ShowAllProfiles() 
        {
            var users = await _userRepository.GetAllUsers();

            return View(users);
        }
    }
}
