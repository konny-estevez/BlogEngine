using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Code;
using WebApi.Middlewares;
using WebApi.ViewModels;

namespace WebApi.Controller
{
    /// <summary>
    /// Posts Controller class
    /// </summary>
    [EnableCors("AllowCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    [Authorize(Roles = "Public,Writer,Editor")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;
        private readonly IUsersService _usersService;

        /// <summary>
        /// Posts Controller constructor
        /// </summary>
        public PostsController(IPostsService postsService, ICommentsService commentsService, IUsersService usersService)
        {
            _postsService = postsService ?? throw new ArgumentNullException(nameof(postsService));
            _commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <summary>
        /// Gets the list of approved posts
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="keyword">Keyword filter</param>
        /// <returns>ListResponse of Post</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListResponse<Post>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpGet]
        [Authorize(Roles = "Editor,Writer")]
        public async Task<IActionResult> Get(string? keyword, int page = 1, int pageSize = 20)
        {
            var result = await _postsService.GetNotApprovedPosts(keyword, page, pageSize);
            if (result.Item2 > 0)
            {
                var response = new ListResponse<Post>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = result.Item1.Select(x => new Post().ToModel(x))
                };
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the Post according to PostId
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Post</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _postsService.GetPost(id);
            if (result != null)
            {
                var model = new Post().ToModel(result);
                var user = await _usersService.GetUser(Guid.Parse(result.UserId));
                if (user != null)
                {
                    model.FullName = $"{user.FirstName} {user.LastName}";
                    model.UserName = user.UserName;
                }
                return Ok(model);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the list of approved posts
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="keyword">Keyword filter</param>
        /// <returns>ListResponse of Post</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListResponse<Post>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpGet("Approved")]
        public async Task<IActionResult> GetApproved(string? keyword, int page = 1, int pageSize = 20)
        {
            var result = await _postsService.GetApprovedPosts(keyword, page, pageSize);
            if (result.Item2 > 0)
            {
                var users = await _usersService.GetUsers(1, int.MaxValue, null);
                var posts = result.Item1.Select(x => new Post().ToModel(x)).ToList();
                foreach (var item in posts)
                {
                    var user = users.Item1.FirstOrDefault(x => x.Id == item.UserId.ToString());
                    if (user != null)
                    {
                        item.UserName = user.UserName;
                        item.FullName = $"{user.FirstName} {user.LastName}";
                    }
                }
                var response = new ListResponse<Post>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = posts,
                };
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the list of pending posts
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="keyword">Keyword filter</param>
        /// <returns>ListResponse of Post</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListResponse<Post>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpGet("Pending")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetPending(string? keyword, int page=1, int pageSize = 20)
        {
            var result = await _postsService.GetPendingPosts(keyword, page, pageSize);
            if (result.Item2 > 0)
            {
                var response = new ListResponse<Post>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = result.Item1.Select(x => new Post().ToModel(x))
                };
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the comments for the PostId
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>ListResponse of Post</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListResponse<Comment>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [HttpGet("{id}/Comments")]
        public async Task<IActionResult> GetComments(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _commentsService.GetCommentsByPost(id, page, pageSize);
            if (string.IsNullOrEmpty(result.Item3) && result.Item2 > 0)
            {
                var users = await _usersService.GetUsers(1, int.MaxValue, null);
                var comments = result.Item1.Select(x => new Comment().ToModel(x)).ToList();
                foreach (var item in comments)
                {
                    var user = users.Item1.FirstOrDefault(x => x.Id == item.UserId.ToString());
                    if (user != null)
                    {
                        item.UserName = user.UserName;
                        item.FullName = $"{user.FirstName} {user.LastName}";
                    }
                }
                var response = new ListResponse<Comment>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = comments,
                };
                return Ok(response);
            }
            else
            {
                return NotFound(result.Item3);
            }
        }

        /// <summary>
        /// Gets the comments for the rejected PostId
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>ListResponse of Comment</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ListResponse<Comment>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [HttpGet("{id}/Rejected")]
        public async Task<IActionResult> GetCommentsRejected(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _commentsService.GetCommentsRejectedByPost(id, page, pageSize);
            if (string.IsNullOrEmpty(result.Item3) && result.Item2 > 0)
            {
                var response = new ListResponse<Comment>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = result.Item1.Select(x => new Comment().ToModel(x))
                };
                return Ok(response);
            }
            else
            {
                return NotFound(result.Item3);
            }
        }

        /// <summary>
        /// Gets the states catalog
        /// </summary>
        /// <returns>Dictionary of states</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Dictionary<int, string>), StatusCodes.Status200OK)]
        [HttpGet("States")]
        public async Task<IActionResult> GetStates()
        {
            return Ok(await _postsService.GetStates());
        }

        /// <summary>
        /// Creates a new Post
        /// </summary>
        /// <param name="Post">New Post</param>
        /// <returns>Created Post</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Post>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Post([FromBody] Post Post)
        {
            var result = await _postsService.CreatePost(Post.FromModel());
            if (result.Item1 != null)
            {
                return CreatedAtAction(nameof(Post),  new Post().ToModel(result.Item1));
            }
            else
            {
                return BadRequest(result.Item2);
            }
        }

        /// <summary>
        /// Updates an existing Post
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="Post">Updated Post</param>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPut("{id}")]
        [Authorize(Roles = "Writer,Editor")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Post Post)
        {
            if (Post.Id != id)
            {
                return BadRequest(ErrorMessages.NotMatchPostId);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var result = await _postsService.UpdatePost(Post.FromModel(), User.Claims);
                    if (result.Item1)
                    {
                        return Ok(true);
                    }
                    return BadRequest(result.Item2);
                }
                return BadRequest(ErrorMessages.InvalidModel);
            }
        }

        /// <summary>
        /// Updates an existing Post state
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="state">Post state</param>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPatch("{id}")]
        [Authorize(Roles = "Writer,Editor")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] int state)
        {
            var result = await _postsService.UpdatePostState(id, state);
            if (result.Item1)
            {
                return Ok(true);
            }
            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Updates an existing Post state
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="rejectComment">Reject Comment</param>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPut("Reject/{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] string rejectComment)
        {
            var result = await _postsService.RejectPost(id, rejectComment, User.Claims);
            if (result.Item1)
            {
                return Ok(true);
            }
            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Deletes an existing post
        /// </summary>
        /// <param name="id">id</param>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _postsService.DeletePost(id);
            if (result.Item1)
            {
                return Ok(true);
            }
            return BadRequest(result.Item2);
        }
    }
}
