using System.ComponentModel.DataAnnotations;

namespace Domain.Data.Identity
{
    /// <summary>
    /// Model to set the password when the old one is forgotten
    /// </summary>
    public class SetPasswordModel
    {
        /// <summary>
        /// The user name
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The token received by email
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// The confirmation for the new password
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
