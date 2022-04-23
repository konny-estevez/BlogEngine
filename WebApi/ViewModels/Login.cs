using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    /// <summary>
    /// Login class
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets user email to login
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password to login
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
