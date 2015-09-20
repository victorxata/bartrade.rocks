using System.ComponentModel.DataAnnotations;

namespace Domain.Data.Identity
{
    /// <summary>
    /// Model to add an external login
    /// </summary>
    public class AddExternalLoginModel
    {
        /// <summary>
        /// The external token
        /// </summary>
        [Required]
        public string ExternalAccessToken { get; set; }
    }
}