namespace WebApi.ViewModels
{
    /// <summary>
    /// Post view model class
    /// </summary>
    public class Post : BaseModel<Entities.Post>
    {
        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Content { get;  set; }

        /// <summary>
        /// Gets or sets state description
        /// </summary>
        //public int State { get;  set; }

        /// <summary>
        /// Gets or sets the state description
        /// </summary>
        public string? StateDescription { get; set; }

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
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the user's full name
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets post's PublishedDate 
        /// </summary>
        public DateTime? PublishedDate { get; private set; }

        public override Entities.Post FromModel()
        {
            return new Entities.Post
            {
                Id = this.Id,
                CreatedDate = this.CreatedDate,
                UpdatedDate = this.UpdatedDate,
                Content = this.Content,
                Title = this.Title,
                UserId = this.UserId.ToString(),
            };
        }

        public override Post ToModel(Entities.Post entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new Post
            {
                Id = entity.Id,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                UserId = Guid.Parse(entity.UserId),
                Content = entity.Content,
                Title = entity.Title,
                //State = (int)entity.State,
                StateDescription = entity.State.ToString(),
                PublishedDate = entity.PublishedDate,
            };
        }
    }
}
