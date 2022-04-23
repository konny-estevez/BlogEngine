using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    /// <summary>
    /// ConfirmationAccount class
    /// </summary>
    public class ConfirmationAccount
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets email
        /// </summary>
        [Required,EmailAddress]
        public string Email { get; set; }
    }
}
