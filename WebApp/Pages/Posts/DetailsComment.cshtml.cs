#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class DetailsCommentModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public DetailsCommentModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public Comment Comment { get; set; }

        public string ErrorMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Comments/" + id, null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<Comment>();
                    Comment = data;
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

            if (Comment == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
