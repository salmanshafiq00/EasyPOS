using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EasyPOS.CodeGen.Helpers;

internal static class Utility
{
    public static string CamelCaseClassName(string name)
    {
        return CamelCase(name);
    }

    public static string CamelCaseEnumValue(string name)
    {
        return CamelCase(name);
    }

    public static string CamelCasePropertyName(string name)
    {
        return CamelCase(name);
    }

    public static string ToCamelCase(this string name)
    {
        return CamelCase(name);
    }

    private static string CamelCase(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return name;
        }
        return name[0].ToString(CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture) + name.Substring(1);
    }

    public static string ToKebabCase(this string input)
    {
        // Replace capital letters with hyphen followed by lowercase, except for single capital letter at the end
        string result = Regex.Replace(input, "(?<!^)(A-Z|(?<=[a-z])[A-Z])", "-$1").ToLower();
        // Handle single capital letter at the end
        result = Regex.Replace(result, "-([a-z])$", "$1");
        return result;
    }

    public static string SeparateWords(this string input)
    {
        // Regular expression that identifies transitions from:
        // - a lowercase letter followed by an uppercase letter
        // - two uppercase letters followed by a lowercase letter
        // Skip cases like AbdurR where it's a lowercase followed by a single uppercase letter at the end.
        var regex = new Regex(@"(?<=[a-z])(?=[A-Z][a-z])|(?<=[a-z])(?=[A-Z][A-Z])|(?<=[A-Z]{2})(?=[A-Z][a-z])");

        // Replace matching points with a space
        return regex.Replace(input, " ");
    }
}
