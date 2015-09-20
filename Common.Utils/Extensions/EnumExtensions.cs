using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Common.Utils.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// gets the name property of an Enum Member's XmlEnumAttribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXmlEnumAttributeName(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes =
                (XmlEnumAttribute[]) fieldInfo.GetCustomAttributes
                    (typeof (XmlEnumAttribute), false);

            var retVal = ((attributes.Length > 0) ? attributes[0].Name : value.ToString());
            if (string.IsNullOrEmpty(retVal))
                retVal = "Unknown";

            return retVal;
        }

        /// <summary>
        /// Convert a string value to an enum value and cast it to the correct enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum</typeparam>
        /// <param name="value">Key in the enum</param>
        public static TEnum AsEnum<TEnum>(this string value) where TEnum : struct
        {
            var enumTypeOf = typeof(TEnum);

            if (enumTypeOf.IsEnum == false)
                throw new ArgumentException("TEnum must be an enum");

            return (TEnum)Enum.Parse(enumTypeOf, value);
        }

        /// <summary>
        /// Convert a string value to an enum value and cast it to the correct enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum</typeparam>
        /// <param name="value">Key in the enum</param>
        /// <param name="valueIfFailParse">The value to return if parsing fails and an exception is thrown</param>
        public static TEnum AsEnum<TEnum>(this string value, TEnum valueIfFailParse) where TEnum : struct
        {
            var enumTypeOf = typeof(TEnum);

            if (enumTypeOf.IsEnum == false)
                throw new ArgumentException("TEnum must be an enum");
            try
            {
                return (TEnum)Enum.Parse(enumTypeOf, value);
            }
            catch
            {
                return valueIfFailParse;
            }
        }

        /// <summary>
        /// Returns the description attribute if present. Defaults to calling .ToString()
        /// Sample enum declaration:
        ///    enum ExampleCompanyEnum
        ///    {
        ///        [Description("")]
        ///        NotSet = 0,
        ///
        ///        [Description("Sajan, Inc.")]
        ///        SajanInc = 2,
        ///        Microsoft = 3
        ///    }
        /// </summary>
        /// <param name="theEnumValue"></param>
        /// <returns></returns>
        public static string ToLongString(this Enum theEnumValue)
        {
            var theEnum = theEnumValue.GetType();

            var info = theEnum.GetMember(theEnumValue.ToString());
            var theAttributes = info[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return theAttributes.Count() == 1
                ? ((DescriptionAttribute)(theAttributes[0])).Description
                : theEnumValue.ToString();
        }

        /// <summary>
        /// Fill a dictionary from an enum; the key is strongly typed to the enum type.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="theEnumType"></param>
        /// <param name="useLongString">ToLongString or ToString</param>
        /// <returns></returns>
        public static Dictionary<TEnum, string> ToDictionary<TEnum>(this Type theEnumType, bool useLongString = true) where TEnum : struct
        {
            //Handle nullable enum types
            var underlying = Nullable.GetUnderlyingType(theEnumType);
            if (underlying != null) theEnumType = underlying;

            if (theEnumType.IsEnum == false)
                throw new ArgumentException("TEnum must be an enum");

            var result = new Dictionary<TEnum, string>();
            foreach (FieldInfo info in theEnumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var theAttributes = info.GetCustomAttributes(typeof(DescriptionAttribute), false);

                var theValue = useLongString && theAttributes.Count() == 1
                    ? ((DescriptionAttribute)(theAttributes[0])).Description
                    : info.Name;

                result.Add(((TEnum)info.GetRawConstantValue()), theValue);
            }
            return result;
        }

        ///  <summary>
        ///  Fills a dictionary from an *enum* type.
        /// 
        ///  Use a Description attribute to rename the values. For examples, see the ExampleCompanyEnum enum in the comments of ToLongString.
        ///  </summary>
        ///  <param name="theEnumType"></param>
        /// <param name="useLongString">ToLongString or ToString</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this Type theEnumType, bool useLongString = true)
        {
            //Handle nullable enum types
            var underlying = Nullable.GetUnderlyingType(theEnumType);
            if (underlying != null) theEnumType = underlying;

            if (theEnumType.IsEnum == false)
                throw new ArgumentException("theEnumType must be an enum");

            var result = new Dictionary<string, string>();
            foreach (FieldInfo info in theEnumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var theAttributes = info.GetCustomAttributes(typeof(DescriptionAttribute), false);

                var theValue = useLongString && theAttributes.Count() == 1
                    ? ((DescriptionAttribute)(theAttributes[0])).Description
                    : info.Name;

                result.Add(((int)info.GetRawConstantValue()).ToString(CultureInfo.InvariantCulture), theValue);
            }
            return result;
        }
    }
}