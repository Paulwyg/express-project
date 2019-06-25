using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.fuyang.BroadcastStrategy.ViewModel;
using Wlst.Ux.fuyang.OnlineStatus.Services;

namespace Wlst.Ux.fuyang.OnlineStatus.ViewModel
{
    [Export(typeof(IIOnlineStatus))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OnlineStatusViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIOnlineStatus
    {
        public void NavOnLoad(params object[] parsObjects)
        {
            OnlineComboBox.Add(new OneItemOnline
                                {
                                    IsOffline = "全部"
                                });
            OnlineComboBox.Add(new OneItemOnline
                                {
                                    IsOffline = "在线"
                                });
            OnlineComboBox.Add(new OneItemOnline
                                {
                                    IsOffline = "离线"
                                });
            OnlineComboBoxSelected = OnlineComboBox[0];
            LoadStrategys();
        }

        public void OnUserHideOrClosing()
        {
            OnlineComboBox.Clear();
        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "设备在线状态查询"; }
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
    }

    public partial class OnlineStatusViewModel
    {
        /// <summary>
        /// 查询
        /// </summary>
        #region CmdQuery
        private DateTime _dtCmdQuery;
        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                return _cmdQuery ??
                       (_cmdQuery = new RelayCommand(ExCmdQuery, CanCmdQuery, true));
            }
        }
        private bool CanCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 10000000;
        }

        private void ExCmdQuery()
        {
            _dtCmdQuery = new DateTime();
            OnlineItems.Clear();
            foreach (var t in OnlineDic)
            {
                if ((t.Num == EquNumber.Trim() || EquNumber.Trim() == "") && (t.Name == EquName.Trim() || EquName.Trim() == "") &&
                    (t.IsOffline == OnlineComboBoxSelected.IsOffline || OnlineComboBoxSelected.IsOffline == "全部"))
                {
                    OnlineItems.Add(t);
                }
            }
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
            EquNumber = null;
            EquName = null;
            OnlineComboBoxSelected = OnlineComboBox[0];
        }

        #endregion

        private void LoadStrategys()
        {
            try
            {
                OnlineItems.Clear();
                string dir = Directory.GetCurrentDirectory() + "\\Config";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + "fuyang.txt";
                string url = "";
                if (File.Exists(path))
                {
                    var sr = new StreamReader(path, Encoding.Default);
                    string rrr = sr.ReadToEnd();
                    sr.Close();
                    string[] line = rrr.Split(Environment.NewLine.ToCharArray());
                    url = line[4].Split(',')[0];
                }
                //var request = (HttpWebRequest) WebRequest.Create("http://183.47.55.131:8088/fy/control/status/");
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                var response = (HttpWebResponse) request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                if (myResponseStream == null) return;
                var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                var jo = (JObject) JsonConvert.DeserializeObject(retString);
                var index = 0;
                foreach (var t in jo["data"]["infos"])
                {
                    index++;
                    OnlineItems.Add(new OneItemOnline
                                        {
                                            Id = index,
                                            Num = t["num"].ToString(),
                                            Name = t["name"].ToString(),
                                            IsOffline = t["isOffline"].ToString() == "0" ? "在线" : "离线",
                                            OfflineTime = t["offlineTime"].ToString()
                                        }
                        );
                    OnlineDic.Add(new OneItemOnline
                                      {
                                          Id = index,
                                          Num = t["num"].ToString(),
                                          Name = t["name"].ToString(),
                                          IsOffline = t["isOffline"].ToString() == "0" ? "在线" : "离线",
                                          OfflineTime = t["offlineTime"].ToString()
                                      }
                        );
                }

            }
            catch (Exception ex)
            {


            }
        }
    }

    public partial class OnlineStatusViewModel
    {
        private ObservableCollection<OneItemOnline> _onlineItems;
        /// <summary>
        /// 设备集合
        /// </summary>
        public ObservableCollection<OneItemOnline> OnlineItems
        {
            get { return _onlineItems ?? (_onlineItems = new ObservableCollection<OneItemOnline>()); }
            set
            {
                if (value == _onlineItems) return;
                _onlineItems = value;
                RaisePropertyChanged(() => OnlineItems);
            }

        }

        private ObservableCollection<OneItemOnline> _onlineDic;
        /// <summary>
        /// 设备集合
        /// </summary>
        public ObservableCollection<OneItemOnline> OnlineDic
        {
            get { return _onlineDic ?? (_onlineDic = new ObservableCollection<OneItemOnline>()); }
            set
            {
                if (value == _onlineDic) return;
                _onlineDic = value;
                RaisePropertyChanged(() => OnlineDic);
            }

        }

        private ObservableCollection<OneItemOnline> _onlineComboBox;
        /// <summary>
        /// 下拉框
        /// </summary>
        public ObservableCollection<OneItemOnline> OnlineComboBox
        {
            get { return _onlineComboBox ?? (_onlineComboBox = new ObservableCollection<OneItemOnline>()); }
            set
            {
                if (value == _onlineComboBox) return;
                _onlineComboBox = value;
                RaisePropertyChanged(() => OnlineComboBox);
            }

        }

        private OneItemOnline _onlineComboBoxSelected;
        /// <summary>
        /// 选中设备
        /// </summary>
        public OneItemOnline OnlineComboBoxSelected
        {
            get { return _onlineComboBoxSelected ?? (_onlineComboBoxSelected = new OneItemOnline()); }
            set
            {
                if (value == _onlineComboBoxSelected) return;
                _onlineComboBoxSelected = value;
                RaisePropertyChanged(() => OnlineComboBoxSelected);
            }

        }

        private string _equNumber="";
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquNumber
        {
            get { return _equNumber; }
            set
            {
                if (_equNumber != value)
                {
                    _equNumber = value;
                    this.RaisePropertyChanged(() => this.EquNumber);
                }
            }
        }

        private string _equName="";
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquName
        {
            get { return _equName; }
            set
            {
                if (_equName != value)
                {
                    _equName = value;
                    this.RaisePropertyChanged(() => this.EquName);
                }
            }
        }
    }
}
