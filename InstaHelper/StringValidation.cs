using System.Text.RegularExpressions;

namespace InstaHelper
{
    static class StringValidation
    {
        private static readonly char[] symbols = new[] { '<', '>', ':', ';', '„', '“', '\'', '/', '|', '?', '*', '"', ',', ' ', '@' };

        public static bool IsValidInstagramUsername(this string username)
        {
            if (Regex.IsMatch(username, @"[а-яА-ЯёЁ]") || username.IndexOfAny(symbols) != -1 || string.IsNullOrEmpty(username))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string ValidateInstagramUsername(this string username)
        {
            return username.ToLower();
        }

        public static bool IsValidInstagramPostlink(this string postlink)
        {
            if (Regex.IsMatch(postlink, @"[а-яА-ЯёЁ]") || !postlink.StartsWith("https://www.instagram.com/"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string ValidateInstagramPostlink(this string postlink)
        {
            return postlink.Split('?')[0];
        }
    }
}
