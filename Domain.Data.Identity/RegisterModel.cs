using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Data.Identity
{
    /// <summary>
    /// Model to save the registration for the user
    /// </summary>
    public class RegisterModel
    {
        public string Id { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// The image url of the user
        /// </summary>
        [MaxLength(400)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The confirmation of the password
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [MaxLength(255)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        public void RegisterModelFactory(ApplicationUser user)
        {
            var myType = GetType();
            var props = new List<PropertyInfo>(myType.GetProperties());
            foreach (var prop in props)
            {
                if ((prop.Name.ToLower() == "password") || (prop.Name.ToLower() == "confirmpassword"))
                    continue;

                var pi = typeof(ApplicationUser).GetProperty(prop.Name);
                var piD = typeof (RegisterModel).GetProperty(prop.Name);

                if ((pi == null) || (piD== null))
                    continue;

                var val = pi.GetValue(user);
                
                if (val != null)
                    piD.SetValue(this, val);
            }

        }

    }
}