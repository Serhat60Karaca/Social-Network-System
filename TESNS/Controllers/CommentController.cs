using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TESNS.Models;
using TESNS.Models.Authentication;
using TESNS.Repositories;
using TESNS.ViewModels;

namespace TESNS.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository commentRepository;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepository, UserManager<AppUser> _userManager)
        {
            this.commentRepository = commentRepository;
            this._userManager = _userManager;
        }


        [HttpPost]
        public async Task<IActionResult> NewComment(NewCommentViweModel commentViweModel)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ViewData["LoginError"] = "You should login to publis a post";
                return RedirectToAction("Login", "User");
            }

            Comment comment = new();
            comment.User = currentUser;
            comment.UserId = currentUser.Id;
            comment.CommentText = commentViweModel.Comment;
            comment.PostId = commentViweModel.PostId;


            commentRepository.Add(comment);


            return RedirectToAction("GetPost","Post",new { id = comment.PostId });
        }


        
    }
}
