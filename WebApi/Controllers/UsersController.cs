using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Code;
using WebApi.Middlewares;
using WebApi.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    /// <summary>
    /// Users Controller class
    /// </summary>
    [EnableCors("AllowCorsPolicy")]
    [Route("api/[controller]")]
    [ValidateModel]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IPostsService _postsService;

        /// <summary>
        /// Users Controller constructor
        /// </summary>
        /// <param name="usersService">Users Service interface</param>
        /// <param name="postsService">Posts Service interface</param>
        public UsersController(IUsersService usersService, IPostsService postsService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _postsService = postsService ?? throw new ArgumentNullException(nameof(postsService));
        }

        /// <summary>
        /// Gets the list of users
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="query">Query filter</param>
        /// <returns>IEnumerable of User</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(ErrorMessages.IncorrectQueryValues);
            }
            var result = await _usersService.GetUsers(page, pageSize, query);
            if (result.Item2 > 0)
            {
                var response = new ListResponse<User>
                {
                    Count = result.Item2,
                    Page = page,
                    PageSize = pageSize,
                    Results = result.Item1.Select(x => ViewModels.User.ToModel(x))
                };
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the user according to UserId
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>User</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _usersService.GetUser(id);
            if (result != null)
            {
                return Ok(ViewModels.User.ToModel(result));
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the posts for the UserId
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>IEnumerable of Posts</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<Post>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        [HttpGet("{id}/Posts")]
        public async Task<IActionResult> GetPosts(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _postsService.GetPostsByUser(id, page, pageSize);
            if (string.IsNullOrEmpty(result.Item3))
            {
                return Ok(result.Item1);
            }
            else
            {
                return NotFound(result.Item3);
            }
        }

        /// <summary>
        /// Gets the roles catalog
        /// </summary>
        /// <returns>Dictionary of roles</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _usersService.GetRoles());
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">New user</param>
        /// <returns>Created user</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var result = await _usersService.CreateUser(user.FromModel());
            if (result.Item1 != null)
            {
                return CreatedAtAction(nameof(Post), ViewModels.User.ToModel(result.Item1));
            }
            else
            {
                return BadRequest(result.Item2);
            }
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="user">Updated user</param>
        /// <returns>Updated result</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User user)
        {
            if (user.Id != id)
            {
                return BadRequest(ErrorMessages.NotMatchUserId);
            }
            else
            {
                var result = await _usersService.UpdateUser(user.FromModel());
                if (result.Item1)
                {
                    return Ok(true);
                }
                return BadRequest(result.Item2);
            }
        }

        /// <summary>
        /// Updates an existign user role
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="newRole">New Role</param>
        /// <returns>Update role result</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] string newRole)
        {
            ArgumentNullException.ThrowIfNull(newRole);
            var result = await _usersService.UpdateUserRole(id, newRole);
            if (result.Item1)
            {
                return Ok(true);
            }
            return BadRequest(result.Item2);
        }

        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Delete result</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestResult))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _usersService.DeleteUser(id);
            if (result.Item1)
            {
                return Ok(true);
            }
            return BadRequest(result.Item2);
        }
    }
}
