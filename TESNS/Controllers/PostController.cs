using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TESNS.Models.Authentication;
using TESNS.Models;
using TESNS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TESNS.Services;
using TESNS.Repositories;
using System.Reflection.Metadata;
using TESNS.Repositories.Concrete;

namespace TESNS.Controllers
{
    //[Authorize]
    public class PostController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPhotoService _photoService;
        private readonly IUserInteractionRepository _userInteractionRepository;
        private readonly IRecommendationService _recommendationService;
        private readonly ICommunityRepository _communityRepository;

        public PostController(ApplicationDbContext context, UserManager<AppUser> userManager, IPhotoService photoService,SignInManager<AppUser> signInManager, 
            IUserInteractionRepository userInteractionRepository, IRecommendationService recommendationService,ICommunityRepository communityRepository)
        {
            _context = context;
            _userManager = userManager;
            _photoService = photoService;
            _signInManager = signInManager;
            _userInteractionRepository = userInteractionRepository;
            _recommendationService = recommendationService;
            _communityRepository = communityRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetPost(int id)
        {
            Post? post = _context.Posts.Find(id);
            if(post == null)
                return NotFound();
            post.Comments = _context.Comments
                .Where(e => e.PostId == id)
                .ToList();
            foreach(var comment in post.Comments)
            {
                comment.User = _context.Users.Find(comment.UserId);
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                UserInteraction interaction = new UserInteraction()
                {
                    UserId = currentUser.Id,
                    PostId = post.Id,
                    ViewDate = DateTime.Now.ToUniversalTime(),
                };
                _userInteractionRepository.Add(interaction);
            }

            return View(post);
        }

        /*[HttpGet]
        public IActionResult CreatePost()
        {
            CreatePostViewModel createPostViewModel = new CreatePostViewModel();
            //createPostViewModel.CommunityId= id ;
            return PartialView("_SendProduct",createPostViewModel);
            //return PartialView("_SendProduct");
            
        }*/
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostViewModel postVM,int? id)
        {
            var currUser = await _userManager.GetUserAsync(User);
           
            Post newPost = new Post()
            {
                
                Header = postVM.Header,
                Text = postVM.Text,
                //ImagePath = result.Url.ToString(),
                Video = postVM.Video,
                PublishDate = DateTime.Now.ToUniversalTime(),
                EditedDate = DateTime.Now.ToUniversalTime(),
                Categories = postVM.Categories.Replace(" ", "").Split(',').ToList(),
                CommentCount = 0,
                LikeCount = 0
            };
            if (id != null)
            {
                newPost.CommunityId = id;
            }
            else {
                newPost.CommunityId = null;
            }
            if (postVM.ImagePath != null)
            {
                var result = await _photoService.AddPhotoAsync(postVM.ImagePath);
                newPost.ImagePath = result.Url.ToString();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ViewData["LoginError"] = "You should login to publis a post";
                return RedirectToAction("Login", "User");
            }
            newPost.UserId = currentUser.Id;
            foreach(var category in newPost.Categories)
            {
                Category _category = new Category();
                var doesExist = _context.Categories.Where(i => i.Name == category).Any();
                _category.Name = category;
                if (!doesExist)
                {
                    _context.Categories.Add(_category);
                }
            }
            
            //newPost.CommunityId = _context.Communities.FirstOrDefault().Id;

            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetRecommendations()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var recommendations = await _recommendationService.GetRecommendedPostsAsync(currentUser.Id, 10);
            return View(recommendations);
        }


        public async Task<IActionResult> ListPosts()
        {
            var user = await _userManager.GetUserAsync(User);
            var posts = await _context.Posts.Where(x => x.UserId == user.Id).ToListAsync();
            return View(posts);
        }

        /*[HttpGet]
        public IActionResult AddCommunity()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCommunity(Community com)
        {
            Community comi = new Community()
            {
                Logo = com.Logo,
                Description = com.Description,
                InviteLink = com.InviteLink
            };

            _context.Communities.Add(comi);
            _context.SaveChanges();


            return RedirectToAction("CreatePost", "Post");
        }*/

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            if (post.ImagePath != null)
            {
                await _photoService.DeletePhotoAsync(post.ImagePath);
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != post.UserId)
            {
                return Unauthorized();
            }
            

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
