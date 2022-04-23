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

namespace WebApp.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;
        private KeyValuePair<string, string> token;

        public EditModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        [BindProperty]
        public User User { get; set; }

        public IEnumerable<string> Roles {get; set; } = new List<string>();

        public string ErrorMessages { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Users/" + id, null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<User>();
                    User = data;
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
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Users/Roles", null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<IEnumerable<string>>();
                    Roles = data;
                }
                else
                {
                    ErrorMessages = result.ReasonPhrase + " - " + await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessages += ex.Message;
            }

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                User.Password = ".";
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Put, "Users/" + User.Id, User);
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
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Patch, "Users/" + User.Id, User.Role);
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
                return Page();
            }
            return RedirectToPage("./Index");
        }

        private bool UserExists(Guid id)
        {
            return false;
        }
    }
}
