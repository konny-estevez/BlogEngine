using Entities;
using System.Security.Claims;

namespace ApplicationCore.Services
{
    public interface IPostsService
    {
        Task<(IEnumerable<Post>, long)> GetNotApprovedPosts(string keyword, int page, int pageSize);
        Task<(IEnumerable<Post>, long)> GetApprovedPosts(string keyword, int page, int pageSize);
        Task<(IEnumerable<Post>, long)> GetPendingPosts(string keyword, int page, int pageSize);
        Task<Post> GetPost(Guid id);
        Task<(Post, string)> CreatePost(Post post);
        Task<Dictionary<int, string>> GetStates();
        Task<(bool, string)> UpdatePost(Post post, IEnumerable<Claim> claims);
        Task<(bool, string)> DeletePost(Guid id);
        Task<(IEnumerable<Post>, long, string)> GetPostsByUser(Guid id, int page, int pageSize);
        Task<(bool, string)> UpdatePostState(Guid id, int state);
        Task<(bool, string)> RejectPost(Guid id, string rejectComment, IEnumerable<Claim> claims);
    }
}
