using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Data.Models.Validation
{
    public class EnumeratorIncludedInAttribute : ValidationAttribute
    {
        private readonly Type _type;

        public EnumeratorIncludedInAttribute(Type objectType)
        {
            _type = objectType;
        }

        public override bool IsValid(object value)
        {
            var values = System.Enum.GetValues(_type);
            return values.Cast<object>().Any(val => val.ToString() == value.ToString());
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The element included in {name} cannot be found in the enumerator";
        }
    }
}