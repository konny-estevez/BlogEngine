using Entities;
using ApplicationCore.Repositories;
using Infrastructure.Repository;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ApplicationCore.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _usersService;
        private readonly ICommentsService _commentsService;

        /// <summary>
        /// PostsService class constructor
        /// </summary>
        /// <param name="postsRepository">Posts Repository interface</param>
        /// <param name="unitOfWork">Unit of Work interface</param>
        /// <param name="usersService">Users Service interface</param>
        /// <param name="commentsService">Comments Service interface</param>
        public PostsService(IPostsRepository postsRepository, IUnitOfWork unitOfWork, IUsersService usersService, ICommentsService commentsService)
        {
            _postsRepository = postsRepository ?? throw new ArgumentNullException(nameof(postsRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));  
            _commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
        }

        /// <summary>
        /// Get all posts according to a keyword ordered from the most recent
        /// </summary>
        /// <param name="keyword">Keyword filter</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>IEnumerable of Post and total count</returns>
        public async Task<(IEnumerable<Post>, long)> GetNotApprovedPosts(string keyword, int page, int pageSize)
        {
            var states = new[] { PostStates.New, PostStates.Rejected };
            Expression<Func<Post, bool>> filter = f => states.Contains(f.State);
            if (!string.IsNullOrEmpty(keyword))
                filter = f => states.Contains(f.State) && f.Title.Contains(keyword);
            return await _postsRepository.GetAll(filter, x => x.OrderByDescending(o => o.CreatedDate), null, page, pageSize);
        }

        /// <summary>
        /// Gets all approved posts according to a keyword ordered from the most recent
        /// </summary>
        /// <param name="keyword">Keyword filter</param>
        /// <param name="page">Page size</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>IEnumerable of Post and total count</returns>
        public async Task<(IEnumerable<Post>, long)> GetApprovedPosts(string keyword, int page, int pageSize)
        {
            Expression<Func<Post, bool>> filter = f => f.State == PostStates.Approved;
            if (!string.IsNullOrEmpty(keyword))
                filter = f => f.State == PostStates.Approved && f.Title.Contains(keyword);
            return await _postsRepository.GetAll(filter, x => x.OrderByDescending(o => o.CreatedDate), null, page, pageSize);
        }

        /// <summary>
        /// Gets all pending posts according to a keyword ordered from the most recent
        /// </summary>
        /// <param name="keyword">Keyword filter</param>
        /// <param name="page">Page size</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>IEnumerable of Post and total count</returns>
        public async Task<(IEnumerable<Post>, long)> GetPendingPosts(string keyword, int page, int pageSize)
        {
            Expression<Func<Post, bool>> filter = f => f.State == PostStates.PendingApproval;
            if (!string.IsNullOrEmpty(keyword))
                filter = f => f.State == PostStates.PendingApproval && f.Title.Contains(keyword);
            return await _postsRepository.GetAll(filter, x => x.OrderByDescending(o => o.CreatedDate), null, page, pageSize);
        }

        /// <summary>
        /// Get a Post by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Post</returns>
        public async Task<Post> GetPost(Guid id)
        {
            return await _postsRepository.Get(id);
        }

        /// <summary>
        /// Gets all posts by UserId order by created date descending
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns>IEnumerbale of Post with total count</returns>
        public async Task<(IEnumerable<Post>, long, string)> GetPostsByUser(Guid id, int page, int pageSize)
        {
            var user = await _usersService.GetUser(id);
            if (user != null)
            {
                var result = await _postsRepository.GetAll(f => f.UserId == id.ToString(), x => x.OrderByDescending(o => o.CreatedDate), null, page, pageSize);
                if (result.Item1 != null)
                {
                    return (result.Item1, result.Item2, string.Empty);
                }
                return (null, 0, ErrorMessages.ErrorGettingPosts);
            }
            else
            {
                return (null, 0, ErrorMessages.UserIdNotExists);
            }
        }

        /// <summary>
        /// Creates a new Post
        /// </summary>
        /// <param name="post">New post</param>
        /// <returns>Creation result</returns>
        public async Task<(Post, string)> CreatePost(Post post)
        {
            ArgumentNullException.ThrowIfNull(post);

            post.Id = new Guid();
            try
            {
                post.State = PostStates.New;
                post.CreatedDate = DateTime.Now;
                var user = await _usersService.GetUser(Guid.Parse(post.UserId));
                if (user != null)
                {
                    if (user.Role.Equals(Constants.WriterRole))
                    {
                        var entity = await _postsRepository.Create(post);
                        var result = await _unitOfWork.Commit();
                        if (entity != null)
                            return (entity, string.Empty);
                        return (null, ErrorMessages.ErrorCreatingPost);
                    }
                    return (null, ErrorMessages.UserNotAllowed);
                }
                return (null, ErrorMessages.UserIdNotExists);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(ErrorMessages.DuplicatedRecordPattern))
                {
                    return (null, ErrorMessages.DuplicatedPost);
                }
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing Post
        /// </summary>
        /// <param name="post">Updated post</param>
        /// <returns>Update result</returns>
        public async Task<(bool, string)> UpdatePost(Post post, IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(post);
            try
            {
                var updatedPost = await GetPost(post.Id);
                if (updatedPost == null)
                {
                    return (false, ErrorMessages.PostNotFound);
                }
                else
                {
                    if (claims == null || !claims.Any())
                    {
                        return (false, ErrorMessages.CannotVerifyIdentity);
                    }
                    var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                    if (updatedPost.State == PostStates.Approved)
                    {
                        return (false, ErrorMessages.PostIsApproved);
                    }
                    var user = await _usersService.GetUser(Guid.Parse(updatedPost.UserId));
                    if (user != null)
                    {
                        switch (role)
                        {
                            case "Editor":
                                if (updatedPost.State == PostStates.New)
                                {
                                    return (false, ErrorMessages.CannotEditPost);
                                }
                                break;
                            case "Writer":
                                if (updatedPost.State == PostStates.PendingApproval || updatedPost.State == PostStates.Approved)
                                {
                                    return (false, ErrorMessages.CannotEditPost);
                                }
                                break;
                            default:
                                return (false, ErrorMessages.UserNotAllowedEdit);
                                break;
                        }
                        updatedPost.UpdatedDate = DateTime.Now;
                        updatedPost.State = post.State;
                        updatedPost.Content = post.Content;
                        updatedPost.Title = post.Title;
                        await _postsRepository.Update(updatedPost);
                        await _unitOfWork.Commit();
                        return (true, string.Empty);
                    }
                    return (false, ErrorMessages.UserIdNotExists);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Updates the post state
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="state">State</param>
        /// <returns>Update result</returns>
        public async Task<(bool, string)> UpdatePostState(Guid id, int state)
        {
            var post = await GetPost(id);
            if (post != null)
            {
                post.UpdatedDate = DateTime.Now;
                post.State = (PostStates)state;
                if (post.State == PostStates.Approved)
                {
                    post.PublishedDate = DateTime.Now;
                }
                await _postsRepository.Update(post);
                await _unitOfWork.Commit();
                return (true, string.Empty);
            }
            return (false, ErrorMessages.PostNotFound);
        }

        /// <summary>
        /// Deletes a post by Id
        /// </summary>
        /// <param name="id">PostId</param>
        /// <returns>Delete result</returns>
        public async Task<(bool, string)> DeletePost(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);

            var post = await GetPost(id);
            if (post == null)
            {
                return (false, ErrorMessages.PostNotFound);
            }
            else
            {
                if (post.State == PostStates.Approved)
                {
                    return (false, ErrorMessages.PostIsApproved);
                }
                try
                {
                    await _postsRepository.Delete(id);
                    await _unitOfWork.Commit();
                    return (true, string.Empty);
                }
                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets the states catalogs
        /// </summary>
        /// <returns>Dictionary of post states</returns>
        public async Task<Dictionary<int, string>> GetStates()
        {
            var result = new Dictionary<int, string>();
            result.Add((int)PostStates.Approved, PostStates.Approved.ToString());
            result.Add((int)PostStates.Rejected, PostStates.Rejected.ToString());   
            result.Add((int)PostStates.PendingApproval, PostStates.PendingApproval.ToString());
            
            return result;
        }

        /// <summary>
        /// Rejects a post and creates a reject comment
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="rejectComment">Reject comment</param>
        /// <returns>Result of reject</returns>
        public async Task<(bool, string)> RejectPost(Guid id, string rejectComment, IEnumerable<Claim> claims)
        {
            if (claims == null || !claims.Any())
            {
                return (false, ErrorMessages.CannotVerifyIdentity);
            }
            var postReject = await _postsRepository.Get(id);
            if (postReject != null)
            {
                var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    postReject.State = PostStates.Rejected;
                    postReject.UpdatedDate = DateTime.Now;
                    await _postsRepository.Update(postReject);
                    await _unitOfWork.Commit();
                    var comment = new Comment
                    {
                        CreatedDate = DateTime.Now,
                        PostId = id,
                        UserId = userId,
                        Content = rejectComment,
                        FromRejectedPost = true,
                    };
                    var result = await _commentsService.CreateComment(comment);
                    if (!string.IsNullOrEmpty(result.Item2))
                        return (false, result.Item2);
                    return (true, string.Empty);
                }
                return (false, ErrorMessages.CannotVerifyIdentity);
            }
            return (false, ErrorMessages.PostIdNotExists);
        }
    }
}
