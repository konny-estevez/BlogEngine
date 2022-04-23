namespace ApplicationCore.Models
{
    /// <summary>
    /// JwtOptions class
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Gets or sets Issuer value
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets Audience value
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets Key value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets ExpireTime value
        /// </summary>
        public short ExpireTime { get; set; }
    }
}
