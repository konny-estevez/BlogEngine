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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Code;
using WebApp.ViewModels;

namespace WebApp.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public EditModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        [BindProperty]
        public Post Post { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

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
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessages = ex.Message;
                }
            if (Post.StateDescription.Equals("Rejected"))
            {
                try
                {
                    var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Posts/" + Post.Id + "/Rejected", null);
                    if (result.IsSuccessStatusCode)
                    {
                        var data = await result.Content.ReadFromJsonAsync<ListResponse<Comment>>();
                        Comments = data.Results.ToList();
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
            }
            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Put, "Posts/" + Post.Id, Post);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<bool>();
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
            return RedirectToPage("./Index");
        }

        private bool PostExists(Guid id)
        {
            //return _context.Post.Any(e => e.Id == id);
            return false;
        }
    }
}
