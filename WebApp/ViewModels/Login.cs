using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }    
    }
}
