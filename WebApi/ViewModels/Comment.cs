namespace WebApi.ViewModels
{
    public class Comment : BaseModel<Entities.Comment>
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

        public override Entities.Comment FromModel()
        {
            return new Entities.Comment
            {
                Content = this.Content,
                FromRejectedPost = this.FromRejectedPost,
                PostId = this.PostId,
                UserId = this.UserId,
                Id = this.Id,
                CreatedDate = this.CreatedDate,
                UpdatedDate = this.UpdatedDate,
            };
        }

        public override Comment ToModel(Entities.Comment entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new Comment
            {
                CreatedDate = entity.CreatedDate,
                Content = entity.Content,
                FromRejectedPost = entity.FromRejectedPost,
                UserId = entity.UserId,
                PostId = entity.PostId,
                Id = entity.Id,
                UpdatedDate = entity.UpdatedDate,
            };
        }
    }
}
