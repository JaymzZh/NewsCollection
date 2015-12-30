using System.IO;
using System.Net;
using System.Text;

namespace NewsCollection.Core
{
    public class HtmlHelper
    {
        public static string GetWebClient(string url)
        {
            string strHtml;
            WebClient myWebClient = new WebClient();
            using (Stream myStream = myWebClient.OpenRead(url))
            {
                StreamReader sr = new StreamReader(myStream, Encoding.Default);//注意编码
                strHtml = sr.ReadToEnd();
            }
            return strHtml;
        }
    }
}
