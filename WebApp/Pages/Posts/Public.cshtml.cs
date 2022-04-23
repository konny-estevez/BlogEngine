using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class PublicModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public PublicModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public ListResponse<Post> Result { get; set; }
        public IList<Post> Posts { get;set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Posts/Approved", null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<ListResponse<Post>>();
                    Result = data;
                    Posts = data.Results.ToList();
                }
                else
                {
                    Posts = new List<Post>();
                    ErrorMessage = result.ReasonPhrase;
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return RedirectToPage("/Index");
        }
    }
}
