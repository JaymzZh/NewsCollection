using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NewsCollection.Core
{
    public class HtmlHelper
    {
        private static readonly List<string> UserAgents = new List<string>
        {
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; it; rv:1.8.1.11) Gecko/20071127 Firefox/2.0.0.11",
            "Opera/9.25 (Windows NT 5.1; U; en)",
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
            "Mozilla/5.0 (compatible; Konqueror/3.5; Linux) KHTML/3.5.5 (like Gecko) (Kubuntu)",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.8.0.12) Gecko/20070731 Ubuntu/dapper-security Firefox/1.5.0.12",
            "Lynx/2.8.5rel.1 libwww-FM/2.14 SSL-MM/1.4.1 GNUTLS/1.2.9",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/535.7 (KHTML, like Gecko) Ubuntu/11.04 Chromium/16.0.912.77 Chrome/16.0.912.77 Safari/535.7",
            "Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:10.0) Gecko/20100101 Firefox/10.0 "
        };

        /// <summary>
        /// 获得网页源代码
        /// </summary>
        /// <param name="urlStr">网址</param>
        /// <param name="encoding">网页编码</param>
        /// <param name="method">获取方式（'GET'/'POST'），默认'GET'</param>
        /// <param name="timeout">超时时间，默认50秒</param>
        /// <param name="accept">接收类型</param>
        /// <returns>以字符串形式保存的网页源代码</returns>
        public static string GetWebSource(string urlStr, Encoding encoding, string method = "GET", int timeout = 50000,
            string accept = "*/*")
        {
            // Set http web address
            Uri url = new Uri(urlStr);

            // Set Web Request
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method; // (GET/POST)
            request.Referer = urlStr;
            request.Timeout = timeout; // Timeout(ms)
            request.Accept = accept; // All Types
            request.UserAgent = UserAgents[new Random().Next(UserAgents.Count)];
            request.CookieContainer = new CookieContainer(); // Cookies

            string respHtml;
            // Create HttpWebResponse Object
            using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(
                    resp.GetResponseStream(),
                    encoding);
                respHtml = reader.ReadToEnd();
            }

            return respHtml;
        }

        /// <summary>
        /// 去除很多带“\”的符号，例：回车符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TrimOther(string source)
        {
            source = Regex.Replace(source, "<script[^>]*?>.*?</script>", string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            source = Regex.Replace(source, "<noscript[^>]*?>.*?</noscript>", string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            source = Regex.Replace(source, "<style[^>]*?>.*?</style>", string.Empty,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            source = Regex.Replace(source, "\n|\t|\r", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            source = Regex.Replace(source, ">\\s+<", "><", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            source = Regex.Replace(source, "\\s+<", "<", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            source = Regex.Replace(source, ">\\s+", ">", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return source
                .Replace(@"\""", "\"")
                .Replace("  ", " ")
                .Replace("&nbsp;", "");
        }
    }
}