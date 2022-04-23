#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public DetailsModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public Post Post { get; set; }
        public string ErrorMessages { get; set; }
        public ListResponse<Comment> ListResponse { get; set; }
        public Comment Comment { get; set; }
        public IList<Comment> Comments{ get; set; } = new List<Comment>();

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Posts/" + id, null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<Post>();
                    Post = data;
                }
                else
                {
                    ErrorMessages = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessages = ex.Message;
            }
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Posts/" + Post.Id + "/Comments", null);
                if (result.IsSuccessStatusCode)
                {
                    ListResponse = await result.Content.ReadFromJsonAsync<ListResponse<Comment>>();
                    Comments = ListResponse.Results.ToList();
                }
                else
                {
                    ErrorMessages = await result.Content.ReadAsStringAsync();
                }
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessages = ex.Message;
            }

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
