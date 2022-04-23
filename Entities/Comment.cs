using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Comment class
    /// </summary>
    public class Comment : BaseEntity<Comment>
    {
        /// <summary>
        /// Gets or sets the content
        /// </summary>
        [Required,StringLength(1024)]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets FromRejectedPost 
        /// </summary>
        public bool FromRejectedPost { get; set; }

        /// <summary>
        /// Gets or sets the post Id
        /// </summary>
        [ForeignKey("FK_PostId")]
        public Guid PostId { get; set; }

        /// <summary>
        /// Gets or sets the post Id
        /// </summary>
        [ForeignKey("FK_UserId")]
        public string UserId { get; set; }
    }
}
