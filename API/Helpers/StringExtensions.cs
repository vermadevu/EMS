using System.Text.RegularExpressions;
namespace API.Helpers
{
    public static class StringExtensions
    {
        public static string ToDisplayName(this string value)
        {
            return Regex.Replace(value, "([a-z])([A-Z])", "$1 $2");
        }
    }
}
