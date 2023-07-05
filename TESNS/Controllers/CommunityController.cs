using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Data;
using TESNS.Migrations;
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
        private readonly ApplicationDbContext _context;
        public CommunityController(ICommunityRepository communityRepository, IPhotoService photoService, UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _communityRepository = communityRepository;
            _photoService = photoService;
            _userManager = userManager;
            _context = context;
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
            var ListComm = _context.Communities.ToList();
            return View("Index", ListComm);
        }
        [HttpGet]
        [Route("Community/CommunityDetail/{id}")]
        public async Task<IActionResult> CommunityDetail(int id)
        {
            var currComm = await _communityRepository.GetCommunityWithId(id);
            var commDetail = new CommunityDetailViewModel()
            {
                Id = id,
                OwnerId = currComm.OwnerId,
                Name = currComm.Name,
                Description = currComm.Description,
                LogoUrl = currComm.Logo,
                //Users = currComm.Users
            };
            
            return View(commDetail);
        }
        [HttpGet]
        public async Task<IActionResult> EditCommunity(int id)
        {
            var currComm = await _communityRepository.GetCommunityWithId(id);
            var editCommunity = new EditCommunityViewModel()
            {
                Id = currComm.Id,
                OwnerId = currComm.OwnerId,
                Name = currComm.Name,
                Description = currComm.Description,
                LogoUrl = currComm.Logo
            };
            return View(editCommunity);
        }
        [HttpPost]
        public async Task<IActionResult> EditCommunity(EditCommunityViewModel editCommunityVM) 
        {
            //if (!ModelState.IsValid) 
            //{
            //   ModelState.AddModelError("", "Model is not valid");
            //}
            var community = await _communityRepository.GetCommunityWithId(editCommunityVM.Id);
            var currLogo = community.Logo;
            if(community.Logo != null) { 
            var result = await _photoService.AddPhotoAsync(editCommunityVM.Logo);
            if (result.Error != null)
            { 
                ModelState.AddModelError("", "Photo couldn't be uploaded");
                return View();
            }
            if (community.Logo != null)
            { 
                _ = await _photoService.DeletePhotoAsync(community.Logo);
            }
                editCommunityVM.LogoUrl = result.Url.ToString();
            }
            
            community.Name = editCommunityVM.Name;
            community.Description = editCommunityVM.Description;
            community.Logo = editCommunityVM.LogoUrl;
            if (community.Logo == null) 
            {
               community.Logo = currLogo;
            }
            _communityRepository.Update(community);
            return View(editCommunityVM);
        }
        [HttpPost]
        public async Task<IActionResult> JoinCommunity(int id)
        {
            var currUser = await _userManager.GetUserAsync(User);
            var currComm = await _communityRepository.GetCommunityWithId(id);
            var commUser = new CommunityUser()
            {
                UserId = currUser.Id,
                CommunityId = currComm.Id,
            };
            var checkUser = _context.CommunityUsers.FirstOrDefault(i => i.CommunityId == currComm.Id && i.UserId == currUser.Id);
            if (checkUser != null)
            {
                ModelState.AddModelError("", "Your are alread joined the community");
            }
            else { 
            _context.CommunityUsers.Add(commUser);
            _context.SaveChanges();
            }
            var comDetail = new CommunityDetailViewModel();
            comDetail.Id = currComm.Id;
            comDetail.Name = currComm.Name;
            comDetail.Description = currComm.Name;
            comDetail.OwnerId = currComm.OwnerId;
            comDetail.LogoUrl = currComm.Logo;
            return View("CommunityDetail",comDetail);
        }
        [HttpPost]
        public async Task<IActionResult> QuitCommunity(int id) 
        {
            var currUser = await _userManager.GetUserAsync(User);
            var currComm = await _communityRepository.GetCommunityWithId(id);
            var removeCommUser = _context.CommunityUsers.FirstOrDefault(i => i.CommunityId == currComm.Id && i.UserId == currUser.Id);
            if (removeCommUser != null) { 
                _context.CommunityUsers.Remove(removeCommUser);
                _context.SaveChanges();
            }
            var comDetail = new CommunityDetailViewModel();
            comDetail.Id = currComm.Id;
            comDetail.Name = currComm.Name;
            comDetail.Description = currComm.Name;
            comDetail.OwnerId = currComm.OwnerId;
            comDetail.LogoUrl = currComm.Logo;
            return View("CommunityDetail", comDetail);
        }
        [HttpPost]
        public async Task<IActionResult> ShowCommunityUsers(int id) 
        {
            var currComm = await _communityRepository.GetCommunityWithId(id);
            var commUsers = _context.CommunityUsers.Where(i => i.CommunityId == currComm.Id).ToList();
            
            return View(commUsers);
        }
       /* [HttpGet]
        public async Task<IActionResult> CreateCommunityPost(int id)
        {
            var post = new CreatePostViewModel();
            post.CommunityId = id;

            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCommunityPost(CreatePostViewModel commPostVM)
        {
            var commPost = new CommunityPost();
            commPost.CommunityId = communityPostVM.CommunityId;
            commPost.PostId = communityPostVM.PostId;
            return View();
        }*/
        //[HttpPost]
        //public async Task<IActionResult> JoinCommunity(Community currComm) 
        //{
        //    var currUser = await _userManager.GetUserAsync(User);
        //    var commUser = new CommunityUser()
        //    {
        //        UserId = currUser.Id,
        //        CommunityId = currComm.Id,
        //    };
        //    _context.CommunityUsers.Add(commUser);
        //    _context.SaveChanges();
        //    //var x = _context.CommunityUsers.FirstOrDefault(i => i.CommunityId == currComm.Id && i.UserId == currUser.Id);

        //    //currComm.Users.Add(x);
        //    //_communityRepository.Update(currComm);
        //    return View();
        //}
    }
}
