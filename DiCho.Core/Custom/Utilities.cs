using DiCho.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiCho.Core.Custom
{
    public static class Utilities
    {
        public static string ToSnakeCase(this string o) =>
    Regex.Replace(o, @"(\w)([A-Z])", "$1-$2").ToLower();

        public static string SnackCaseToLower(this string o)
            => o.Contains("-") ? string.Join(string.Empty, o.Split("-")).Trim().ToLower() : string.Join(string.Empty, o.Split("_")).Trim().ToLower();
        //public static void SetHeaderHttpClient(this HttpClient httpClient)
        //{
        //    httpClient.DefaultRequestHeaders.Clear();
        //    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
        //}
        public static string GetQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties()
                .Where(w => w.GetValue(obj, null) != null && !w.CustomAttributes.Any(a => a.AttributeType == typeof(SkipAttribute)))
                .Select(s => $"{s.Name.ToSnakeCase()}={Uri.EscapeUriString(s.GetValue(obj, null).ToString())}");
            if (properties.Any())
                return string.Join("&", properties.ToArray());
            return string.Empty;
        }
        public static string DisplayName(this Enum value)
        {
            try
            {
                Type enumType = value.GetType();
                var enumValue = Enum.GetName(enumType, value);
                MemberInfo member = enumType.GetMember(enumValue)[0];

                var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                var outString = ((DisplayAttribute)attrs[0]).Name;

                if (((DisplayAttribute)attrs[0]).ResourceType != null)
                {
                    outString = ((DisplayAttribute)attrs[0]).GetName();
                }

                return outString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
