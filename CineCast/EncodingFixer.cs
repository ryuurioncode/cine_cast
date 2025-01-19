using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CineCast
{
    static class EncodingFixer
    {
        static string testString = "тест";
        static string desiredString = "%D1%82%D0%B5%D1%81%D1%82".ToLower();
        static Encoding targetEncoding = FindEncoding(testString, desiredString);

        public static Encoding FindEncoding(string test, string expect)
        {
            var encodings = Encoding.GetEncodings().Select(x => x.GetEncoding()).ToList();
            foreach (var encoding in encodings)
            {
                var resultString = HttpUtility.UrlEncode(testString, encoding).ToLower();
                if (desiredString == resultString)
                    return encoding;
            }
            throw new Exception("System has no desired encoding!");
        }

        public static string UrlEncode(string source)
        {
            return HttpUtility.UrlEncode(source, targetEncoding)
                .Replace("+", "%20");
        }
    }
}
