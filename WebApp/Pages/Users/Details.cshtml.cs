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

namespace WebApp.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public DetailsModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public User User { get; set; }

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
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessages = ex.Message;
            }

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
