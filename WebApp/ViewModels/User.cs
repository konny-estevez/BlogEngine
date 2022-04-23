using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    /// <summary>
    /// User model class
    /// </summary>
    public class User : BaseModel<User>
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets email
        /// </summary>
        [Required,EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gerts or sets OasswordCheck
        /// </summary>
        [Required,Compare("Password")]
        public string PasswordCheck { get; set; }

        /// <summary>
        /// Gets or sets address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets brithday
        /// </summary>
        [Required] 
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Gets or sets city
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets country
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets mobile phone
        /// </summary>
        [Required, Phone] 
        public string? MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        [Required, Phone]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets postal code
        /// </summary>
        public string? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets state
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the role
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Gets or sets created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets active state
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets update date
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
