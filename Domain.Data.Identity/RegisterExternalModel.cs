using System.ComponentModel.DataAnnotations;

namespace Domain.Data.Identity
{
    /// <summary>
    /// Model to register an external user
    /// </summary>
    public class RegisterExternalModel
    {
        /// <summary>
        /// The user name
        /// </summary>
        [Required]
        public string UserName { get; set; }
    }
}