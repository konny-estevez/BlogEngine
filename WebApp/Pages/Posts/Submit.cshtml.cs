#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class SubmitModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public SubmitModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        [BindProperty]
        public Post Post { get; set; }
        public string ErrorMessages { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Patch, "Posts/" + Post.Id, 1);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<bool>();
                }
                else
                {
                    ErrorMessages = await result.Content.ReadAsStringAsync();
                }
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ErrorMessages = ex.Message;
            }

            return RedirectToPage("./Index");
        }
    }
}
