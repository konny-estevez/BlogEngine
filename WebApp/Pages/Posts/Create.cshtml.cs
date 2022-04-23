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
    public class CreateModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public CreateModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Post Post { get; set; }

        public string ErrorMessages { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Post.UserId = Guid.Parse(Request.Cookies.FirstOrDefault(x => x.Key == "userId").Value);
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Post, "Posts", Post);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<Post>();
                    return RedirectToPage("./Index");
                }
                else
                {
                    ErrorMessages = await result.Content.ReadAsStringAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessages = ex.Message;
            }
            return RedirectToPage("/Index");
        }
    }
}
