namespace Entities
{
    /// <summary>
    /// Base Entity
    /// </summary>
    /// <typeparam name="T">T type</typeparam>
    public class BaseEntity<T> where T : class
    {
        /// <summary>
        /// Gets or sets Id value
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date updated
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Method that clones the object
        /// </summary>
        /// <returns>T object</returns>
        public T? Clone()
        {
            return this.MemberwiseClone() as T;
        }
    }
}
