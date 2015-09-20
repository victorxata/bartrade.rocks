using System.ComponentModel.DataAnnotations;

namespace Domain.Data.Identity
{
    /// <summary>
    /// Model to change current password
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// The old password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        /// <summary>
        /// The confirmation for the new password
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}