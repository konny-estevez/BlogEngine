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

namespace WebApp.Pages.Users
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
        public User User { get; set; }

        public string ErrorMessages { get; set; }

        // To protect from overUsering attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Address = User.Address ?? ".";
            User.City = User.City ?? ".";
            User.State = User.State ?? ".";
            User.Country = User.Country ?? ".";
            User.PostalCode = User.PostalCode ?? ".";
            User.Role = User.Role ?? ".";

            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Post, "Users", User);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<User>();
                    return RedirectToPage("/Login");
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
    }
}
