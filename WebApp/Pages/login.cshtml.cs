using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfigurationSection _apiUrl;

        [Required]
        public string Email { get; set; }

        [Required, EmailAddress]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public void OnGet()
        {
            Request.Cookies.ToList().ForEach(cookie => Response.Cookies.Delete(cookie.Key));
        }

        public async Task<IActionResult> OnPost(Login login)
        {
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Post, "Login/Login", login);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<TokenResponse>();
                    var token = data.Token;
                    Response.Cookies.Append("token", token);
                    Response.Cookies.Append("userId", data.UserId);
                    Response.Cookies.Append("role", data.Role);
                    Response.Cookies.Append("fullname", data.FullName);
                    Response.Cookies.Append("email", login.Email.Trim());
                    switch (data.Role)
                    {
                        case "Writer":
                        case "Editor":
                            return RedirectToPage("./Posts/Index");
                            break;
                        case "Public":
                            return RedirectToPage("./Posts/Public");
                            break;
                        case "Admin":
                            return RedirectToPage("./Users/Index");
                            break;
                        default:
                            break;
                    }
                    ErrorMessage = data.Role;
                }
                else
                {
                    Response.Cookies.Delete("token");
                    ErrorMessage = result.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return Page();
        }
    }
}