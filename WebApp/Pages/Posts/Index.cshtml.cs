using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public IndexModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public ListResponse<Post> Result { get; set; }
        public IList<Post> Posts { get;set; } = new List<Post>();
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = Request.Cookies.FirstOrDefault(x => x.Key == "role");
            var url = string.Empty;
            switch (role.Value)
            {
                case "Public":
                    url = "Posts/Approved";
                    break;
                case "Editor":
                    url = "Posts/Pending";
                    break;
                case "Writer":
                    url = "Posts";
                    break;
                default:
                    url = "Posts";
                    break;
            }

            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, url, null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<ListResponse<Post>>();
                    Result = data;
                    Posts = data.Results.ToList();
                    return Page();
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return Page();
                }
                else
                {
                    ErrorMessage = result.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                BadRequest(ex.Message);
            }
            return RedirectToPage("/Index");
        }
    }
}
