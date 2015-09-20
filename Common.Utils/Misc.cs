using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Utils
{
    public static class Misc
    {
        public static string GetCurrentIpAddress()
        {
            if (HttpContext.Current == null) return string.Empty;

            var request = HttpContext.Current.Request;

            var result = request.ServerVariables["REMOTE_ADDR"];
            return result;
        }

        public static List<CultureInfo> GetCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        }

        public static List<RegionInfoWithLcid> GetISO3166CountryList()
        {
            var countries = new List<RegionInfoWithLcid>();
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                var country = new RegionInfoWithLcid();
                country.Region = new RegionInfo(culture.LCID);
                country.LCID = culture.LCID;

                if (countries.All(p => p.Region.Name != country.Region.Name))
                    countries.Add(country);
            }
            return countries.OrderBy(p => p.Region.EnglishName).ToList();
        }


        public class RegionInfoWithLcid
        {
            public RegionInfo Region { get; set; }
            public int LCID { get; set; }
        }

        public static string HtmlEncode(string text)
        {
            var chars = HttpUtility.HtmlEncode(text).ToCharArray();
            var result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (var c in chars)
            {
                var value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}

