using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Domain.Data.Models.Validation
{
    public class LanguageSearchAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                var exists = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.Name == value.ToString());
                return exists != null;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The language in {name} cannot be found in the database";
        }
    }
}