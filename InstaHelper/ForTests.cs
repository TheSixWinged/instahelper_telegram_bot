using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InstaHelper
{
    public class ForTests
    {
        private readonly char[] symbols = new[] { '<', '>', ':', ';', '„', '“', '\'', '/', '|', '?', '*', '"', ',', ' ', '@' };
        public bool IsValidInstagramUsername(string username)
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

        public string ValidateInstagramUsername(string username)
        {
            return username.ToLower();
        }

        public bool IsValidInstagramPostlink(string postlink)
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

        public string ValidateInstagramPostlink(string postlink)
        {
            return postlink.Split('?')[0];
        }

        public List<string> SortMembers(List<string> input_list, List<string> del_list)
        {
            foreach (string del_member in del_list)
            {
                input_list = input_list.Where(x => x != del_member).ToList();
            }

            return input_list;
        }
    }
}
