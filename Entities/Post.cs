using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Post class
    /// </summary>
    [Index(new[] { nameof(Title), nameof(UserId) }, IsUnique = true)]
    public class Post : BaseEntity<Post>
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [Required, StringLength(50), Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        [Required, StringLength(1024), Column(TypeName = "varchar(1024)")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        public PostStates State { get; set; }

        /// <summary>
        /// Gets or sets the published date
        /// </summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the user Id
        /// </summary>
        [ForeignKey("FK_User"), Column(TypeName = "varchar(64)")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the comments
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }

    public enum PostStates
    {
        New,
        PendingApproval,
        Approved,
        Rejected
    }
}
