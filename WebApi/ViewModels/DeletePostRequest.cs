namespace WebApi.ViewModels
{
    /// <summary>
    /// DeletePostRequest class
    /// </summary>
    public class DeletePostRequest
    {
        /// <summary>
        /// gets or sets UserId
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        public string UserName { get; set; }
    }
}