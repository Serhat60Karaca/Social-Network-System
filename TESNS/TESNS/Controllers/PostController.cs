using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TESNS.Models.Authentication;
using TESNS.Models;
using TESNS.ViewModels;
using TESNS.Interfaces;

namespace TESNS.Controllers
{
    public class PostController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;

        public PostController(ApplicationDbContext context, UserManager<AppUser> userManager, IPhotoService photoService)
        {
            _context = context;
            _userManager = userManager;
            _photoService = photoService;
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostViewModel postVM)
        {
            var result = await _photoService.AddPhotoAsync(postVM.ImagePath);
            Post newPost = new Post()
            {
                Header = postVM.Header,
                Text = postVM.Text,
                ImagePath = result.Url.ToString(),
                Video = postVM.Video,
                PublishDate = DateTime.Now.ToUniversalTime(),
                EditedDate = DateTime.Now.ToUniversalTime(),
                CommentCount = 0,
                LikeCount = 0
            };
            var currentUser = await _userManager.GetUserAsync(User);
            newPost.UserId = currentUser.Id;
            newPost.CommunityId = 1;
            //newPost.CommunityId = _context.Communities.FirstOrDefault().Id;

            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return RedirectToAction("ListPosts", "Post");
        }

        public IActionResult ListPosts()
        {
            var posts = _context.Posts;
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
    }
}
