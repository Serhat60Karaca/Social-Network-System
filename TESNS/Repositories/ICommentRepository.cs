using TESNS.Models;

namespace TESNS.Repositories
{
    public interface ICommentRepository
    {

        Task<List<Comment>> GetCommentsByPostId(int postId);
        Task<List<Comment>> GetCommentsByUserId(int userId);
        
        
        bool Add(Comment comment);
        bool Update(Comment comment);
        bool Delete(Comment comment);
    }
}
