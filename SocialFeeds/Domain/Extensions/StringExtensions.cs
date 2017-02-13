using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore;

namespace SocialFeeds.Domain.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Indicates whether the specified string is null or an empty string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }


        public static string ToFormat(this string formatMe, params object[] args)
        {
            return string.Format(formatMe, args);
        }

        /// <summary>
        ///     Update query srting value
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceQueryStringItem(this string url, string key, string value)
        {
            string result;
            if (!url.Contains("?"))
            {
                result = string.Format("{0}?{1}={2}", url, key, value);
            }
            else
            {
                if (!url.Contains(key + "="))
                    result = string.Format("{0}&{1}={2}", url, key, value);
                else
                    result = Regex.Replace(url, "([?&]" + key + ")=[^?&]+", "$1=" + value);
            }
            return result;
        }

        public static string GetPlainTextWithoutHtmlTags(this string input)
        {
            return HttpUtility.HtmlDecode(Regex.Replace(input, "<[^>]*>", string.Empty));
        }

        /// Like linq take - takes the first x characters
        public static string Truncate(this string s, int length, bool atWord, bool addEllipsis)
        {
            // Return if the string is less than or equal to the truncation length     
            if (s == null || s.Length <= length)
                return s;
            // Do a simple tuncation at the desired length     
            var s2 = s.Substring(0, length);
            // Truncate the string at the word     
            if (atWord)
            {
                // List of characters that denote the start or a new word (add to or remove more as necessary)          
                var alternativeCutOffs = new List<char> {' ', ',', '.', '?', '/', ':', ';', '\'', '\"', '\'', '-'};
                // Get the index of the last space in the truncated string          
                var lastSpace = s2.LastIndexOf(' ');
                // If the last space index isn't -1 and also the next character in the original          
                // string isn't contained in the alternativeCutOffs List (which means the previous          
                // truncation actually truncated at the end of a word),then shorten string to the last space          
                if (lastSpace != -1 && s.Length >= length + 1 && !alternativeCutOffs.Contains(s.ToCharArray()[length]))
                    s2 = s2.Remove(lastSpace);
            } // Add Ellipsis if desired     
            if (addEllipsis)
                s2 += "...";
            return s2;
        }

        /// <summary>
        ///     escape character for sitecore x path query
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string EscapeCharacterForXPath(this string xpath)
        {
            string word;
            var newPath = string.Empty;
            var bufferpath = xpath.Split('[');
            var tab = bufferpath[0].Split('/');
            bool isExcaped;
            var first = true;

            foreach (var part in tab)
            {
                isExcaped = false;
                word = part;

                if (part.Contains(" "))
                {
                    word = "#" + part + "#";
                    isExcaped = true;
                }

                if (word.Contains('-'))
                    if (isExcaped)
                        word = word.Replace("-", "##-##");
                    else
                        word = "#" + word.Replace("-", "##-##") + "#";
                if (first)
                {
                    first = false;
                    newPath = word;
                }
                else
                {
                    newPath += "/" + word;
                }
            }
            bufferpath[0] = newPath;
            newPath = "";
            foreach (var s in bufferpath)
                newPath += string.IsNullOrEmpty(newPath) ? s : "[" + s;


            return newPath;
        }

        /// <summary>
        ///     escape character for sitecore x path query
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static string EscapeCharacterForTabID(this string tabID)
        {
            tabID = tabID.Replace("<br />", "").Replace("<br/>", "").Replace("/", "").Replace(" ", "");
            return tabID;
        }

        public static bool IsNumeric(this string s)
        {
            var result = 0;

            return !string.IsNullOrEmpty(s) && int.TryParse(s, out result);
        }

        public static string ValueOrDefault(this string source, string defaultValue)
        {
            return !string.IsNullOrEmpty(source) ? source : defaultValue;
        }

        public static DateTime? ConvertToDate(this string value)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None,
                out dateTime))
                return dateTime;

            return null;
        }

        //public static T RetrieveItemById<T>(this string itemId) where T : class
        //{
        //    var sitecoreContext = (ISitecoreContext) SitecoreContext.GetFromHttpContext((string) null);
        //    return sitecoreContext.Cast<T>(Context.Database.GetItem(itemId));
        //}
    }
}