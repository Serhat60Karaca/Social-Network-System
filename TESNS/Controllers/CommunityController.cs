using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using TESNS.Models;
using TESNS.Models.Authentication;
using TESNS.Repositories;
using TESNS.Repositories.Concrete;
using TESNS.Services;
using TESNS.ViewModels;

namespace TESNS.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        public CommunityController(ICommunityRepository communityRepository,IPhotoService photoService,UserManager<AppUser> userManager) 
        {
            _communityRepository = communityRepository;
            _photoService = photoService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var comm = await _communityRepository.GetAllCommunities();
            return View(comm);
        }
        public async Task<IActionResult> MyCommunities() 
        {
            var currUser = await _userManager.GetUserAsync(User);
            var myComm = await _communityRepository.GetCommunitiesWithOwnerId(currUser.Id);
            return View(myComm);
        }
        [HttpGet]
        public IActionResult CreateCommunity()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCommunity(CreateCommunityViewModel createCommunityVM)
        {
            if (!ModelState.IsValid) return View(createCommunityVM);
            var result = await _photoService.AddPhotoAsync(createCommunityVM.Logo);
            createCommunityVM.LogoUrl = result.Url.ToString();

            var user = await _userManager.GetUserAsync(User);
            var newComm = new Community()
            {
                Name = createCommunityVM.Name,
                Logo = createCommunityVM.LogoUrl,
                Description = createCommunityVM.Description,
                CreationDate = DateTime.Now.ToUniversalTime(),
                OwnerId = user.Id
            };
            _communityRepository.Add(newComm);
            return View("Index");
        }
        [HttpGet]
        [Route("Community/CommunityDetail/{id}")]
        public async Task<IActionResult> CommunityDetail(int id)
        {
            var currComm = await _communityRepository.GetCommunityWithId(id);
            var commDetail = new CommunityDetailViewModel()
            {   
                Id = id,
                OwnerId=currComm.OwnerId,
                Name = currComm.Name,
                Description=currComm.Description,
                LogoUrl=currComm.Logo,
                //Users = currComm.Users
            };
            return View(commDetail);
        }
        [HttpGet]
        [Route("Community/EditCommunity/{id}")]
        public async Task<IActionResult> EditCommunity(int id) 
        {
            return View();
        }
    }
}
