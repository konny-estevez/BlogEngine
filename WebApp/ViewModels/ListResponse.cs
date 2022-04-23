namespace WebApp.ViewModels
{
    /// <summary>
    /// ListResponse
    /// </summary>
    /// <typeparam name="T">T modelView</typeparam>
    public class ListResponse<T>
    {
        /// <summary>
        /// Gets or sets record count 
        /// </summary>
        public long Count { get; set; }
 
        /// <summary>
        /// Gets or sets page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets results
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
