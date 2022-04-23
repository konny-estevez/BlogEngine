namespace WebApp.ViewModels
{
    /// <summary>
    /// Post view model class
    /// </summary>
    public class Post : BaseModel<Post>
    {
        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Content { get;  set; }

        /// <summary>
        /// Gets or sets state description
        /// </summary>
        public int State { get;  set; }

        /// <summary>
        /// Gets or sets the state description
        /// </summary>
        public string StateDescription { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get;  set; }

        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public Guid UserId { get;  set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// gets or sets PublishedDate 
        /// </summary>
        public DateTime? PublishedDate { get; set; }
    }
}
