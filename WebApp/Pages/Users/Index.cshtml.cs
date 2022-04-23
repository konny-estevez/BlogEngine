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
    public class IndexModel : PageModel
    {
        private readonly IConfigurationSection _apiUrl;

        public IndexModel(IConfiguration configuration)
        {
            _apiUrl = configuration.GetRequiredSection("ApiUrl");
        }

        public IList<User> Users { get;set; } = new List<User>();
        public string ErrorMessage { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await new ApiRequest(_apiUrl).ExecuteRequest(Request, Response, HttpMethod.Get, "Users", null);
                if (result.IsSuccessStatusCode)
                {
                    var data = await result.Content.ReadFromJsonAsync<ListResponse<User>>();
                    Users = data.Results.ToList();
                    return Page();
                }
                else
                {
                    ErrorMessage = result.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                BadRequest(ex.Message);
            }
            return RedirectToPage("/Index");
        }
    }
}
