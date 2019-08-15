using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Wlst.mobile;
using System.Collections.Concurrent;
using System.Threading;

namespace Wlst.Cr.CoreMims
{
    public partial class HttpGetPostforMsgWithMobile
    {
        /// <summary>
        /// var infos = MsgWithMobile.Deserialize(data)
        /// </summary>
        /// <param name="cmd">instances.Head.Cmd</param>
        /// <param name="instances"> System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(instances))</param>
        /// <returns></returns>
        public static byte[] OrderSndHttp(string cmd, string instances)
        {
            try
            {
                var data = HttpGetx(cmd, instances);

                if (string.IsNullOrEmpty(data) || data.Equals("ok") || data.Equals("error")) return null;

                var rtn = System.Convert.FromBase64String(data);
                return rtn;
                //var infos = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
                //if (infos != null)
                //{
                //    return infos;
                //}
            }
            catch (Exception ex)
            {
                Wlst.Cr.Coreb.Servers.WriteLog.WriteLogError(ex.ToString());
            }


            return null;
        }

        public static byte[] OrderSndPostHttp(string cmd, string instances)
        {
            try
            {
                var data = HttpPostxx(HttpUrl + cmd, "?pb2=" + instances);

                if (string.IsNullOrEmpty(data) || data.Equals("ok") || data.Equals("error")) return null;

                var rtn = System.Convert.FromBase64String(data);
                return rtn;
                //var infos = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
                //if (infos != null)
                //{
                //    return infos;
                //}
            }
            catch (Exception ex)
            {
                Wlst.Cr.Coreb.Servers.WriteLog.WriteLogError(ex.ToString());
            }


            return null;
        }



        public static MsgWithMobile OrderSndHttp(MsgWithMobile instances)
        {

            if (instances != null && instances.Head != null)
            {
                try
                {
                    var data = HttpGetx(instances.Head.Cmd,
                        System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(instances)));

                    if (string.IsNullOrEmpty(data) || data.Equals("ok") || data.Equals("error")) return null;

                    var infos = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
                    if (infos != null)
                    {
                        return infos;
                    }
                }
                catch (Exception ex)
                {
                    Wlst.Cr.Coreb.Servers.WriteLog.WriteLogError(ex.ToString());
                }
            }

            return null;
        }

        /// <summary>
        /// http:1.0.0.1:8080/mims/
        /// </summary>
        public static string HttpUrl = "";

        //是否异步获取服务器数据  
        public static bool IsHttpGetAsync = true;

        private static string HttpGetx(string cmd, string data)
        {
            if (IsHttpGetAsync)
            {
                return HttpGetPostAsync(cmd, data,true );
            }
            else
            {
                var xr = HttpGetx1(cmd, data, Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName);
                return xr;
            }

        }

        private static string HttpPostx(string cmd, string data)
        {
            if (IsHttpGetAsync)
            {
                return HttpGetPostAsync(cmd, data, false);
            }
            else
            {
                var xr = HttpPostx1(cmd, data, Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName);
                return xr;
            }

        }
        private static string HttpGetx1(string cmd, string data, string username)
        {
            if (string.IsNullOrEmpty(data)) data = "1";
            var str = HttpGet(HttpUrl + cmd, "?username=" + username + "&pb2=" + data);
            return str;
        }

        private static string HttpPostx1(string cmd, string data, string username)
        {
            if (string.IsNullOrEmpty(data)) data = "1";

            var datax = new { pb2 = data, username = username, scode = "", uuid = "" };
            var jsonToSend = JsonConvert.SerializeObject(datax, Newtonsoft.Json.Formatting.None, new IsoDateTimeConverter());

            return HttpPost(HttpUrl + cmd, jsonToSend);
        }

        /// <summary>
        /// GET请求与获取结果   参数携带需要带上第一个？  参数之间用&分割 2
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="getdataAttr">get携带的数据</param>
        /// <returns></returns>
        internal static string HttpGet(string Url, string getdataAttr)
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
        internal static string HttpPost(string url, string postDataStr)
        {
            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "POST";
                //request.Timeout = 6000000;
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentLength = postDataStr.Length;
                //StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                //writer.Write(postDataStr);
                //writer.Flush();
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //string encoding = response.ContentEncoding;
                //if (encoding == null || encoding.Length < 1)
                //{
                //    encoding = "UTF-8"; //默认编码
                //}
                //StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                //string retString = reader.ReadToEnd();
                //return retString;




                string ret = string.Empty;
                try
                {

                    byte[] byteArray = Encoding.UTF8.GetBytes(postDataStr); //转化
                    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    webReq.Method = "POST";
                    webReq.ContentType = "application/json";

                    webReq.ContentLength = byteArray.Length;
                    Stream newStream = webReq.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                    newStream.Close();
                    HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    ret = sr.ReadToEnd();
                    sr.Close();
                    response.Close();
                    newStream.Close();
                }
                catch (Exception ex)
                {
                    //WriteSystemLog.WriteSystemLogError("HttpPost Error :" + ex);
                }
                return ret;
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
        public static string HttpPostxx(string url, string postDataStr)
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

        ///// <summary>
        ///// GET请求与获取结果   参数携带需要带上第一个？  参数之间用&分割 2
        ///// </summary>
        ///// <param name="Url"></param>
        ///// <param name="getdataAttr">get携带的数据</param>
        ///// <returns></returns>
        //internal static async Task<string > HttpGet(string Url, string getdataAttr)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url + getdataAttr);
        //        request.Method = "GET";
        //        request.ContentType = "text/html;charset=UTF-8";
        //        request.Timeout = 10000;
        //        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        //        Stream myResponseStream = response.GetResponseStream();
        //        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
        //        string retString = myStreamReader.ReadToEnd();
        //        myStreamReader.Close();
        //        myResponseStream.Close();
        //        return retString;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// POST请求与获取结果  参数之间用&分割 
        ///// </summary>
        ///// <param name="url">路径</param>
        ///// <param name="postDataStr">post携带的数据 </param>
        ///// <returns></returns>
        //internal static async Task<string> HttpPost(string url, string postDataStr)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.Timeout = 6000000;
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = postDataStr.Length;
        //        StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
        //        writer.Write(postDataStr);
        //        writer.Flush();
        //        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        //        string encoding = response.ContentEncoding;
        //        if (encoding == null || encoding.Length < 1)
        //        {
        //            encoding = "UTF-8"; //默认编码
        //        }
        //        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
        //        string retString = reader.ReadToEnd();
        //        return retString;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }

    public partial class HttpGetPostforMsgWithMobile
    {
        private static long Idx = 1;
        private static object lockobj = 1;
        private static ConcurrentDictionary<long, Tuple<long, string>> DicIdx = new ConcurrentDictionary<long, Tuple<long, string>>();
        //cmd data isGet Idx
        private static ConcurrentQueue<Tuple<string, string,bool , long>> QueueIdx = new ConcurrentQueue<Tuple<string, string, bool, long>>();
        private static bool RunningQtz = false;
        private static void InitQtz()
        {

            if (RunningQtz)
                return;

            lock (lockobj)
            {
                if (RunningQtz)
                    return;

                //注册调度函数
                Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("nt", 0, DateTime.Now.AddSeconds(3).Ticks, OnHttpGetxAsync, 250);
                //注册调度函数
                Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("nt", 0, DateTime.Now.AddSeconds(3).Ticks, OnHttpGetxAsync, 500);
                //注册调度函数
                Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("nt", 0, DateTime.Now.AddSeconds(3).Ticks, OnHttpGetxAsync, 1000);
                RunningQtz = true;
            }

        }


        private static long  lastCleantime=0;
        private static void OnHttpGetxAsync(object obj)
        {
            if (QueueIdx.Count > 0)
            {
                Tuple<string, string, bool, long> tmp = null;
                if (QueueIdx.TryDequeue(out tmp))
                {
                    if (tmp != null)
                    {
                        if (tmp.Item3)
                        {
                            var xr = HttpGetx1(tmp.Item1, tmp.Item2, Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName);
                            if (DicIdx.ContainsKey(tmp.Item4) == false) DicIdx.TryAdd(tmp.Item4, new Tuple<long, string>(DateTime.Now.Ticks, xr));
                        }
                        else
                        {
                            var xr = HttpPostx1(tmp.Item1, tmp.Item2, Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName);
                            if (DicIdx.ContainsKey(tmp.Item4) == false) DicIdx.TryAdd(tmp.Item4, new Tuple<long, string>(DateTime.Now.Ticks, xr));
                        }
                    }
                }
            }

          //  if (DateTime.Now.Second % 10 != 1) return;
            if (DateTime.Now.Ticks - lastCleantime < 10000000) return;
            lastCleantime = DateTime.Now.Ticks;

            var dlt = DateTime.Now.AddMinutes(-5).Ticks;
            var dlts = (from t in DicIdx where t.Value.Item1 < dlt select t.Key).ToList();
            foreach (var f in dlts)
            {
                Tuple<long, string> trx;
                if (DicIdx.ContainsKey(f))
                {
                    DicIdx.TryRemove(f, out trx);
                }
            }

        }

        private static string HttpGetPostAsync(string cmd, string data,bool isGet)
        {
            InitQtz();
            var id = Interlocked.Increment(ref Idx);
            QueueIdx.Enqueue(new Tuple<string, string,bool , long>(cmd, data,isGet , id));
            var deadtime = DateTime.Now.AddSeconds(30).Ticks;

            while (DateTime.Now.Ticks < deadtime)
            {
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                if (DicIdx.ContainsKey(id))
                {
                    return DicIdx[id].Item2;
                }
            }
            // var xr = HttpGetx1(cmd, data, Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName);
            return string.Empty;

        }
    }
}
