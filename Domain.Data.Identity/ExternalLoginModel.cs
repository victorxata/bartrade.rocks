namespace Domain.Data.Identity
{
    /// <summary>
    /// Model to link to external login providers
    /// </summary>
    public class ExternalLoginModel
    {
        /// <summary>
        /// The name of the provider
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The authentication url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The current state
        /// </summary>
        public string State { get; set; }
    }
}