using Entities;
using ApplicationCore.Repositories;
using Infrastructure.Repository;

namespace ApplicationCore.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostsRepository _postsRepository;

        /// <summary>
        /// CommentsService class constructor
        /// </summary>
        /// <param name="commentsRepository">Comments Repository interface</param>
        /// <param name="unitOfWork">Unit of Work interface</param>
        /// <param name="postsRepository">Posts Repository interface</param>
        public CommentsService(ICommentsRepository commentsRepository, IUnitOfWork unitOfWork, IPostsRepository postsRepository)
        {
            _commentsRepository = commentsRepository ?? throw new ArgumentNullException(nameof(postsRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _postsRepository = postsRepository ?? throw new ArgumentNullException(nameof(postsRepository));
        }

        /// <summary>
        /// Gets all comments with keyword and order by created date
        /// </summary>
        /// <param name="keyword">Keyword filter</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>IEnumerable of Comments with total count</returns>
        public async Task<(IEnumerable<Comment>, long)> GetComments(string keyword, int page, int pageSize)
        {
            return await _commentsRepository.GetAll(f => f.Content.Contains(keyword), x => x.OrderBy(o => o.CreatedDate), null, page, pageSize);
        }

        /// <summary>
        /// Gets a comment by CommentId
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Comment</returns>
        public async Task<Comment> GetComment(Guid id)
        {
            return await _commentsRepository.Get(id);
        }

        /// <summary>
        /// Gets the comments by PostId order by created date
        /// </summary>
        /// <param name="id">PostId</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>IEnumerable of Comment with total count</returns>
        public async Task<(IEnumerable<Comment>,  long, string)> GetCommentsByPost(Guid id, int page, int pageSize)
        {
            var post = await _postsRepository.Get(x => x.Id == id, null, null);
            if (post != null)
            {
                var result = await _commentsRepository.GetAll(x => x.PostId == id && !x.FromRejectedPost, null, null, page, pageSize);
                if (result.Item1 != null)
                {
                    return (result.Item1, result.Item2, string.Empty);
                }
                return (null, 0, ErrorMessages.ErrorGettingComments);
            }
            else
            {
                return (null, 0, ErrorMessages.PostIdNotExists);
            }
        }

        /// <summary>
        /// Gets the comments by rejected PostId order by created date
        /// </summary>
        /// <param name="id">PostId</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>IEnumerable of Comment with total count</returns>
        public async Task<(IEnumerable<Comment>, long, string)> GetCommentsRejectedByPost(Guid id, int page, int pageSize)
        {
            var post = await _postsRepository.Get(x => x.Id == id, null, null);
            if (post != null)
            {
                var result = await _commentsRepository.GetAll(x => x.PostId == id && x.FromRejectedPost, null, null, page, pageSize);
                if (result.Item1 != null)
                {
                    return (result.Item1, result.Item2, string.Empty);
                }
                return (null, 0, ErrorMessages.ErrorGettingComments);
            }
            else
            {
                return (null, 0, ErrorMessages.PostIdNotExists);
            }
        }

        /// <summary>
        /// Creates a new comment
        /// </summary>
        /// <param name="comment">New comment</param>
        /// <returns>Creation result</returns>
        public async Task<(Comment, string)> CreateComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment);
            var post = await _postsRepository.Get(comment.PostId);
            if (post == null)
            {
                return (null, ErrorMessages.PostIdNotExists);
            }

            comment.CreatedDate = DateTime.Now;
            comment.Id = new Guid();
            try
            {
                var entity = await _commentsRepository.Create(comment);
                var result = await _unitOfWork.Commit();
                if (entity != null)
                    return (entity, string.Empty);
                return (null, ErrorMessages.ErrorCreatingComment);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(ErrorMessages.DuplicatedRecordPattern))
                {
                    return (null, ErrorMessages.DuplicatedComment);
                }
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing comment
        /// </summary>
        /// <param name="comment">Updated comment</param>
        /// <returns>Update result</returns>
        public async Task<(bool, string)> UpdateComment(Comment comment)
        {
            ArgumentNullException.ThrowIfNull(comment);
            try
            {
                var updatedComment = await GetComment(comment.Id);
                if (updatedComment == null)
                {
                    return (false, ErrorMessages.CommentNotFound);
                }
                else
                {
                    updatedComment.UpdatedDate= DateTime.Now;
                    updatedComment.Content = comment.Content;
                    await _commentsRepository.Update(updatedComment);
                    await _unitOfWork.Commit();
                    return (true, string.Empty);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Deletes an existing comment by Id
        /// </summary>
        /// <param name="id">CommentId</param>
        /// <returns>Delete result</returns>
        public async Task<(bool, string)> DeleteComment(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);

            var comment = await GetComment(id);
            if (comment == null)
            {
                return (false, ErrorMessages.CommentNotFound);
            }
            else
            {
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
    }
}
