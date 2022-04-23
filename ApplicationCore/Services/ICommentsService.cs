using Entities;

namespace ApplicationCore.Services
{
    public interface ICommentsService
    {
        Task<(IEnumerable<Comment>, long)> GetComments(string keyword, int page, int pageSize);
        Task<Comment> GetComment(Guid id);
        Task<(Comment, string)> CreateComment(Comment comment);
        Task<(bool, string)> UpdateComment(Comment comment);
        Task<(bool, string)> DeleteComment(Guid id);
        Task<(IEnumerable<Comment>, long, string)> GetCommentsByPost(Guid id, int page, int pageSize);
        Task<(IEnumerable<Comment>, long, string)> GetCommentsRejectedByPost(Guid id, int page, int pageSize);
    }
}
