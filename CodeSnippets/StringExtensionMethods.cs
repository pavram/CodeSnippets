using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSnippets
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// use a string as a format-string directly.
        /// Rather than calling the String.Format static function.
        /// </summary>
        /// <param name="FormatString"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string FormatWith(this string FormatString, params object[] arguments)
        {
            return string.Format(FormatString, arguments);
        }
    }
}
