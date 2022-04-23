using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    /// <summary>
    /// User model class
    /// </summary>
    public class User
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
        /// Gets or sets address
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets brithday
        /// </summary>
        [Required] 
        public DateTime Birthday { get; set; }

        /// <summary>
        /// Gets or sets city
        /// </summary>
        [Required] 
        public string City { get; set; }

        /// <summary>
        /// Gets or sets country
        /// </summary>
        [Required] 
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets mobile phone
        /// </summary>
        [Required, Phone] 
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        [Required, Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets postal code
        /// </summary>
        [Required]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets state
        /// </summary>
        [Required]
        public string State { get; set; }

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

        /// <summary>
        /// Gets or sets user's roles
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Converts from entity to view model
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>ViewModels.User</returns>
        public static User ToModel(Entities.User entity)
        {
            return new User
            {
                Id = Guid.Parse(entity.Id),
                Address = entity.Address,
                Birthday = entity.Birthday,
                City = entity.City,
                Country = entity.Country,
                CreatedDate = entity.CreatedDate,
                Email = entity.Email,
                FirstName = entity.FirstName,
                IsActive = entity.EmailConfirmed,
                LastName = entity.LastName,
                MobilePhone = entity.MobilePhone,
                PhoneNumber = entity.PhoneNumber,
                PostalCode = entity.PostalCode,
                State = entity.State,
                UpdatedDate = entity.UpdatedDate,
                Role = entity.Role,
            };
        }

        /// <summary>
        /// Converts from the view model instance to entity
        /// </summary>
        /// <returns>Entities.User</returns>
        public Entities.User FromModel()
        {
            return new Entities.User
            {
                Id = this.Id.ToString(),
                Address = this.Address,
                Birthday = this.Birthday,
                City = this.City,
                Country = this.Country,
                CreatedDate = this.CreatedDate,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                MobilePhone = this.MobilePhone,
                PasswordHash = this.Password,
                PhoneNumber = this.PhoneNumber,
                PostalCode = this.PostalCode,
                State = this.State,
                UpdatedDate = this.UpdatedDate,
                Role = this.Role,
                EmailConfirmed = this.IsActive,
             };
        }
    }
}
