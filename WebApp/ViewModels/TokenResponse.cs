namespace WebApp.ViewModels
{
    /// <summary>
    /// TokenResponse model class
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets token value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets token type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets token issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets token expiration
        /// </summary>
        public DateTime ExpiresOn { get; set; }

        /// <summary>
        /// Gets or sets user's Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets user's role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets user's Id
        /// </summary>
        public string UserId { get; set; }
    }
}
