using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Newtonsoft.Json;
using Wlst.Ux.fuyang.BroadcastStrategy.Services;

namespace Wlst.Ux.fuyang.BroadcastStrategy.ViewModel
{
    [Export(typeof(IIBroadcastStrategy))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class BroadcastStrategyViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIBroadcastStrategy
    {
        private bool _isViewActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            string dir = Directory.GetCurrentDirectory() + "\\Config";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + "fuyang.txt";
            if (File.Exists(path))
            {
                var sr = new StreamReader(path, Encoding.Default);
                string rrr = sr.ReadToEnd();
                sr.Close();
                string[] line = rrr.Split(Environment.NewLine.ToCharArray());
                requestUrl = line[2].Split(',')[0];
                url = line[0].Split(',')[0];
            }
            LoadStrategys();
            _isViewActive = true;
            
        }

        public void OnUserHideOrClosing()
        {
            _isViewActive = false;
            Msg = "";
            StrategyItems.Clear();
        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "播放策略管理"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        private Thread _schedule;
        public BroadcastStrategyViewModel()
        {
             _schedule = new Thread(Run);
            _schedule.Start();
        }


        void Run()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                    if (_isViewActive == false) continue;
                    Request();
                }
                catch (Exception ex)
                {
                    //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                }
            }
        }

        private void Request()
        {
            //string dir = Directory.GetCurrentDirectory() + "\\Config";
            //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            //string path = dir + "\\" + "fuyang.txt";
            //string requestUrl = "";
            //if (File.Exists(path))
            //{
            //    var sr = new StreamReader(path, Encoding.Default);
            //    string rrr = sr.ReadToEnd();
            //    sr.Close();
            //    string[] line = rrr.Split(Environment.NewLine.ToCharArray());
            //    requestUrl = line[2].Split(',')[0];
            //}
            //var request = (HttpWebRequest) WebRequest.Create("http://183.47.55.131:8088/fy/light/strategy/");
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            var response = (HttpWebResponse) request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream == null) return;
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            var jo = (JObject) JsonConvert.DeserializeObject(retString);


            var itemss = new Dictionary<string, OneItemStrategy>();
            foreach (var t in jo["data"]["strategys"])
            {
                var tmpss = new OneItemStrategy
                                {
                                    Id = t["id"].ToString(),
                                    StrategyNum = t["strategyNum"].ToString(),
                                    StrategyName = t["strategyName"].ToString(),
                                    StrategyExplain = t["strategyExplain"].ToString(),
                                    StrategyStatus = t["strategyStatus"].ToString() == "1" ? "使用中" : "未使用"
                                };
                if (itemss.ContainsKey(tmpss.Id)) continue;
                itemss.Add(tmpss.Id, tmpss);

            }
            bool res= Compar(itemss);
            if (res)
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate
                                                                     {
                                                                         items.Clear();
                                                                         StrategyItems.Clear();

                                                                         foreach (var t in jo["data"]["strategys"])
                                                                         {
                                                                             var tmp = new OneItemStrategy
                                                                                           {
                                                                                               Id = t["id"].ToString(),
                                                                                               StrategyNum =
                                                                                                   t["strategyNum"].
                                                                                                   ToString(),
                                                                                               StrategyName =
                                                                                                   t["strategyName"].
                                                                                                   ToString(),
                                                                                               StrategyExplain =
                                                                                                   t["strategyExplain"].
                                                                                                   ToString(),
                                                                                               StrategyStatus =
                                                                                                   t["strategyStatus"].
                                                                                                       ToString() == "1"
                                                                                                       ? "使用中"
                                                                                                       : "未使用"
                                                                                           };
                                                                             if (items.ContainsKey(tmp.Id)) continue;
                                                                             items.Add(tmp.Id, tmp);
                                                                             StrategyItems.Add(tmp);
                                                                         }
                                                                     }));
            }

            //Wlst .Cr .Core .UtilityFunction .WriteLog .WriteLogInfo();
        }


        private bool Compar(Dictionary<string, OneItemStrategy> data)
        {
            if (data.Count != items.Count) return true;
            foreach (var f in data )
            {
                if (items.ContainsKey(f.Key) == false) return true;
                if (items[f.Key].StrategyNum != f.Value.StrategyNum) return true;
                if (items[f.Key].StrategyExplain != f.Value.StrategyExplain) return true;
                if (items[f.Key].StrategyStatus != f.Value.StrategyStatus) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// ICommand
    /// </summary>
    public partial class BroadcastStrategyViewModel
    {
        /// <summary>
        /// 启用
        /// </summary>
       #region CmdStart
        private DateTime _dtCmdStart;
        private ICommand _cmdStart;

        public ICommand CmdStart
        {
            get
            {
                return _cmdStart ??
                       (_cmdStart = new RelayCommand(ExCmdStart, CanCmdStart, true));
            }  
        }
        private bool CanCmdStart()
        {
            return DateTime.Now.Ticks - _dtCmdStart.Ticks > 10000000;
        }

        private void ExCmdStart()
        {
            _dtCmdStart=new DateTime();
            var request = (HttpWebRequest)WebRequest.Create("http://183.47.55.131:8088/fy/light/strategy/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var dataStr = JsonConvert.SerializeObject(StrategyComboBoxSelected);//要发送的数据转json格式
            byte[] data = Encoding.UTF8.GetBytes("strategys=" + dataStr);
            request.ContentLength = data.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            var resp = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = resp.GetResponseStream();
            if (myResponseStream == null) return;
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            var result = myStreamReader.ReadToEnd(); //获取响应内容  
            myStreamReader.Close();
            myResponseStream.Close();
        }

        #endregion

        /// <summary>
        /// 启用/立即演示/继续/暂停
        /// </summary>
        #region CmdOperate
        private DateTime _dtCmdOperate;
        private ICommand _cmdOperate;

        public ICommand CmdOperate
        {
            get
            {
                return _cmdOperate ??
                       (_cmdOperate = new RelayCommand<string>(ExCmdOperate, CanCmdOperate, true));
            }
        }
        private bool CanCmdOperate(string data)
        {
            return DateTime.Now.Ticks - _dtCmdOperate.Ticks > 10000000;
        }

        private void ExCmdOperate(string datax)
        {
            _dtCmdOperate = new DateTime();
            //string dir = Directory.GetCurrentDirectory() + "\\Config";
            //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            //string path = dir + "\\" + "fuyang.txt";
            //string url = "";
            //if (File.Exists(path))
            //{
            //    var sr = new StreamReader(path, Encoding.Default);
            //    string rrr = sr.ReadToEnd();
            //    sr.Close();
            //    string[] line = rrr.Split(Environment.NewLine.ToCharArray());
            //    url = line[0].Split(',')[0];
            //}
            //var url = "http://183.47.55.131:8088/fy/light/control";
            int x;
            if (Int32.TryParse(datax, out x) == false) return;
            var code = "";
            var operateUrl = "";
            if (x == 1)
            {
                operateUrl = url + "/strategy? {\"strategyType\":1"
                      + ",\"strategyNum\":" + StrategyComboBoxSelected.StrategyNum + "}";
                code = "启动指令";
            }
            else if (x == 2)
            {
                operateUrl = url + "/strategy? {\"strategyType\":2"
                      + ",\"strategyNum\":" + StrategyComboBoxSelected.StrategyNum + "}";
                code = "立即演示指令";
            }
            else if (x == 3)
            {
                operateUrl = url + "/stop?{\"strategyType\":2}";
                code = "继续指令";
            }
            else
            {
                operateUrl = url + "/stop?{\"strategyType\":1}";
                code = "暂停指令";
            }
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在下发"+code+" ...";
            var request = (HttpWebRequest)WebRequest.Create(operateUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            var response = (HttpWebResponse) request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream == null) return;
            var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            var jo = (JObject) JsonConvert.DeserializeObject(retString);
            var state = jo["code"].ToString() == "200"
                            ? " 下发成功"
                            : jo["code"].ToString() == "400"
                                  ? " 失败"
                                  : jo["code"].ToString() == "401"
                                        ? " 未认证"
                                        : jo["code"].ToString() == "403"
                                              ? " 未授权"
                                              : jo["code"].ToString() == "404"
                                                    ? " 接口不存在"
                                                    : jo["code"].ToString() == "500" ? " 内部错误" : " 未知错误";
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + state;

            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("阜阳项目,调用接口，指令类型:" + code + ",URL:" + operateUrl + "返回结果:" + state);
        }

        #endregion

        /// <summary>
        /// 继续
        /// </summary>
        #region CmdContinue
        private DateTime _dtCmdContinue;
        private ICommand _cmdContinue;

        public ICommand CmdContinue
        {
            get
            {
                return _cmdContinue ??
                       (_cmdContinue = new RelayCommand(ExCmdContinue, CanCmdContinue, true));
            }
        }
        private bool CanCmdContinue()
        {
            return DateTime.Now.Ticks - _dtCmdContinue.Ticks > 10000000;
        }

        private void ExCmdContinue()
        {
            _dtCmdContinue = new DateTime();
        }

        #endregion

        /// <summary>
        /// 暂停
        /// </summary>
        #region CmdStop
        private DateTime _dtCmdStop;
        private ICommand _cmdStop;

        public ICommand CmdStop
        {
            get
            {
                return _cmdStop ??
                       (_cmdStop = new RelayCommand(ExCmdStop, CanCmdStop, true));
            }
        }
        private bool CanCmdStop()
        {
            return DateTime.Now.Ticks - _dtCmdStop.Ticks > 10000000;
        }

        private void ExCmdStop()
        {
            _dtCmdStop = new DateTime();
        }

        #endregion

        /// <summary>
        /// 刷新
        /// </summary>
        #region CmdRefresh
        private DateTime _dtCmdRefresh;
        private ICommand _cmdRefresh;

        public ICommand CmdRefresh
        {
            get
            {
                return _cmdRefresh ??
                       (_cmdRefresh = new RelayCommand(ExCmdRefresh, CanCmdRefresh, true));
            }
        }
        private bool CanCmdRefresh()
        {
            return DateTime.Now.Ticks - _dtCmdRefresh.Ticks > 10000000;
        }

        private void ExCmdRefresh()
        {
            _dtCmdRefresh = new DateTime();
            LoadStrategys();
        }

        #endregion

        private string url;
        private string requestUrl;
        private Dictionary<string , OneItemStrategy> items = new Dictionary<string , OneItemStrategy>();  
        private void LoadStrategys(){
            try
            {

                StrategyItems.Clear();
                var request = (HttpWebRequest) WebRequest.Create(requestUrl);
                request.Method = "GET";
                request.ContentType = "application/json";
                var response = (HttpWebResponse) request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                if (myResponseStream == null) return;
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                var jo = (JObject) JsonConvert.DeserializeObject(retString);

                items.Clear();
                foreach (var t in jo["data"]["strategys"])
                {
                    var tmp = new OneItemStrategy
                                  {
                                      Id = t["id"].ToString(),
                                      StrategyNum = t["strategyNum"].ToString(),
                                      StrategyName = t["strategyName"].ToString(),
                                      StrategyExplain = t["strategyExplain"].ToString(),
                                      StrategyStatus = t["strategyStatus"].ToString() == "1" ? "使用中" : "未使用"
                                  };
                    if (items.ContainsKey(tmp.Id)) continue;
                    items.Add(tmp.Id, tmp);
                    StrategyItems.Add(tmp);
                }
                if (StrategyItems != null)
                {
                    foreach (var t in StrategyItems)
                    {
                        if (t.StrategyStatus == "使用中")
                            StrategyComboBoxSelected = t;
                    }

                }
            }
            catch
                (Exception ex)
                {

                   
                }
        }
    }

    /// <summary>
    /// 集合或变量
    /// </summary>
    public partial class BroadcastStrategyViewModel
    {
        private string _msg;
        /// <summary>
        /// 通知
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value == _msg) return;
                _msg = value;
                RaisePropertyChanged(() => Msg);
            }
        }

        private ObservableCollection<OneItemStrategy> _strategyItems;
        /// <summary>
        /// 播放策略集合
        /// </summary>
        public ObservableCollection<OneItemStrategy> StrategyItems
        {
            get { return _strategyItems ?? (_strategyItems = new ObservableCollection<OneItemStrategy>()); }
            set
            {
                if (value == _strategyItems) return;
                _strategyItems = value;
                RaisePropertyChanged(() => StrategyItems);
            }

        }

        private OneItemStrategy _strategyComboBoxSelected;
        /// <summary>
        /// 当前播放策略
        /// </summary>
        public OneItemStrategy StrategyComboBoxSelected
        {
            get
            {
                return _strategyComboBoxSelected;
            }
            set
            {
                if (value == _strategyComboBoxSelected) return;
                _strategyComboBoxSelected = value;
                RaisePropertyChanged(() => StrategyComboBoxSelected);
            }

        }
    }
}
