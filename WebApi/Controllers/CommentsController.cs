using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Code;
using WebApi.Middlewares;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    /// <summary>
    /// Comments Controller class
    /// </summary>
    [EnableCors("AllowCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    [Authorize(Roles = "Public,Writer,Editor,Admin")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IUsersService _usersService;

        /// <summary>
        /// Comments Controller constructor
        /// </summary>
        /// <param name="commentsService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CommentsController(ICommentsService commentsService, IUsersService usersService)
        {
            _commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        /// <summary>
        /// Gets the list of Comments
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="query">Query filter</param>
        /// <returns>IEnumerable of Comment</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Comment>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(ErrorMessages.IncorrectQueryValues);
            }
            var result = await _commentsService.GetComments(query, page, pageSize);
            if (result.Item2 > 0)
            {
                var response = new ListResponse<Comment>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = result.Item1.Select(x =>  new Comment().ToModel(x))
                };
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the Comment according to CommentId
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Comment</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _commentsService.GetComment(id);
            if (result != null)
            {
                var comment = new Comment().ToModel(result);
                var user = await _usersService.GetUser(Guid.Parse(result.UserId));
                if (user != null)
                {
                    comment.UserName = user.UserName;
                    comment.FullName = $"{user.FirstName} {user.LastName}";
                }
                return Ok(comment);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates a new Comment
        /// </summary>
        /// <param name="comment">New Comment</param>
        /// <returns>Created Comment</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Comment comment)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentsService.CreateComment(comment.FromModel());
                if (result.Item1 != null)
                {
                    return CreatedAtAction(nameof(Post), new Comment().ToModel(result.Item1));
                }
                else
                {
                    return BadRequest(result.Item2);
                }
            }
            return BadRequest(ErrorMessages.InvalidModel);
        }

        /// <summary>
        /// Updates an existing Comment
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="comment">Updated Comment</param>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Comment comment)
        {
            if (comment.Id != id)
            {
                return BadRequest(ErrorMessages.NotMatchCommentId);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var result = await _commentsService.UpdateComment(comment.FromModel());
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
        /// Deletes an existing Comment
        /// </summary>
        /// <param name="id">id</param>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _commentsService.DeleteComment(id);
            if (result.Item1)
            {
                return Ok(true);
            }
            return BadRequest(result.Item2);
        }
    }
}
