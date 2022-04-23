namespace WebApp.ViewModels
{
    /// <summary>
    /// Interface for base model
    /// </summary>
    public abstract class BaseModel<T> where T : class
    {
        /// <summary>
        /// Gets or sets entity Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Updated Date
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
