using System;
using System.IO;
using System.Net;
using System.Text;

namespace wlst.sr.iif
{
   public  class HttpGetPost
    {
        /// <summary>
        /// GET请求与获取结果   参数携带需要带上第一个？  参数之间用&分割 
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="getdataAttr">get携带的数据</param>
        /// <returns></returns>
        public static string HttpGet(string Url, string getdataAttr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + getdataAttr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// POST请求与获取结果  参数之间用&分割 
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="postDataStr">post携带的数据 </param>
        /// <returns></returns>
        public static string HttpPost(string url, string postDataStr)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 6000000;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataStr.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                writer.Write(postDataStr);
                writer.Flush();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                return retString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
