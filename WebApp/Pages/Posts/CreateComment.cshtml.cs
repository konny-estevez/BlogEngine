#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class CreateCommentModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public CreateCommentModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public IActionResult OnGet(Guid? id)
        {
            Comment.PostId = id.GetValueOrDefault();
            return Page();
        }

        [BindProperty]
        public Comment Comment { get; set; } = new Comment();
        public string ErrorMessages { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            Comment.PostId = id.GetValueOrDefault();
            var userId = Request.Cookies.FirstOrDefault(x => x.Key == "userId").Value;
            Comment.UserId = userId;
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Post, "Comments", Comment);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<Comment>();
                    return RedirectToPage("./Details", new { id = Comment.PostId });
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
            return Page();
        }
    }
}
