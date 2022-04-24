using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// User class inherits from IdentityUser
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the date created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date updated
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        [Required, StringLength(60), Column(TypeName = "varchar(60)")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        [Required,StringLength(60), Column(TypeName = "varchar(60)")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birthday
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Gets or sets user's address
        /// </summary>
        [Required, StringLength(150), Column(TypeName = "varchar(150)")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets user's city
        /// </summary>
        [Required, StringLength(50), Column(TypeName = "varchar(50)")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets user's state
        /// </summary>
        [Required, StringLength(35), Column(TypeName = "varchar(35)")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets user's country
        /// </summary>
        [Required, StringLength(35), Column(TypeName = "varchar(35)")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets user's postal code
        /// </summary>
        [Required, StringLength(10), Column(TypeName = "varchar(10)")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets user's mobile phone
        /// </summary>
        [Required, StringLength(20), Column(TypeName = "varchar(20)")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the user's roles
        /// </summary>
        [NotMapped]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the posts
        /// </summary>
        public ICollection<Post> Posts { get; set; }
    }
}
