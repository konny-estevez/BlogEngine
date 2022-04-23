namespace WebApp.ViewModels
{
    /// <summary>
    /// Comment model class
    /// </summary>
    public class Comment : BaseModel<Comment>
    {
        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets FromRejectedPost 
        /// </summary>
        public bool FromRejectedPost { get; set; }

        /// <summary>
        /// Gets or sets the post Id
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Gets or sets the post Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's full name
        /// </summary>
        public string? FullName { get; set; }
    }
}
