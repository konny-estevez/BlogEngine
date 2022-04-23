using ApplicationCore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApi.Middlewares;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    /// <summary>
    /// Login Controller class
    /// </summary>
    [EnableCors("AllowCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    public class LoginController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokensService _tokensService;

        /// <summary>
        /// Login Controller constructor
        /// </summary>
        /// <param name="usersService">Users Service interface</param>
        /// <param name="tokensService">Tokens Service interface</param>
        public LoginController(IUsersService usersService, ITokensService tokensService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
        }

        /// <summary>
        /// Executes user login
        /// </summary>
        /// <param name="userlogin">userlogin</param>
        /// <returns>Result of login</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedResult))]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login userlogin)
        {
            var result = await _usersService.LoginUser(userlogin.Email, userlogin.Password);
            if (result.Item1 != null)
            {
                var token = _tokensService.CreateToken(userlogin.Email, result.Item1);
                var response = new TokenResponse
                {
                    ExpiresOn = DateTime.Now.AddMinutes(token.Item2.ExpireTime),
                    Issuer = token.Item2.Issuer,
                    Type = "Bearer",
                    Token = token.Item1,
                    FullName = $"{result.Item1.FirstName} {result.Item1.LastName}",
                    Role = result.Item1.Role,
                    UserId = result.Item1.Id,
                };
                return Ok(response);
            }
            else
            {
                return Unauthorized(result.Item2);
            }
        }

        /// <summary>
        /// Validates a user
        /// </summary>
        /// <param name="validateModel">validateModel</param>
        /// <returns>Result of validation</returns>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedResult))]
        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmEmail(ConfirmationAccount validateModel)
        {
            var result = await _usersService.ConfirmEmail(validateModel.Id, validateModel.Email);
            if (result.Item1)
            {
                return Ok(true);
            }
            else
            {
                return Unauthorized(result.Item2);
            }
        }
    }
}
