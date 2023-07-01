using Microsoft.EntityFrameworkCore;
using TESNS.Models;

namespace TESNS.Repositories.Concrete
{
    public class CommentRepository : ICommentRepository
    {

        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool Add(Comment comment)
        {
            _context.Add(comment);
            Post post = _context.Posts.Find(comment.PostId);
            post.LikeCount++;
            _context.Update(post);
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool Delete(Comment comment)
        {
            _context.Remove(comment);
            Post post = _context.Posts.Find(comment.PostId);
            post.LikeCount--;
            _context.Update(post);
            return _context.SaveChanges() > 0 ? true : false;
        }


        public async Task<List<Comment>> GetCommentsByPostId(int postId)
        {
            var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
            return comments;

        }

        public async Task<List<Comment>> GetCommentsByUserId(int userId)
        {
            var comments = await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
            return comments;
        }

        public bool Update(Comment comment)
        {
            _context.Update(comment);
            return _context.SaveChanges() > 0 ? true : false;
        }
    }
}
