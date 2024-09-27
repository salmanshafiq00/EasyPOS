using System.Globalization;
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
}
