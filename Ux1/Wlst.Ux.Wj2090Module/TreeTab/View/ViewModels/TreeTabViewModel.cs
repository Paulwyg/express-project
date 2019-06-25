using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.CoreMims.NodeServices;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Services;
 
using Wlst.Ux.Wj2090Module.TreeTab.View.Serivices;

namespace Wlst.Ux.Wj2090Module.TreeTab.View.ViewModels
{
    [Export(typeof(IIAreaTree))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeTabViewModel : Wlst.Cr.CoreMims.NodeServices.TreeViewControl, IIAreaTree
    {
        private int _hxxx = 0;

        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int) Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightTree;
                    if (_hxxx > 24) _hxxx = 24;
                    if (_hxxx < 12) _hxxx = 12;
                }

                return _hxxx;
            }
        }

        public static TreeTabViewModel MySelf;

        public TreeTabViewModel():base(OnNodeSelected, null )
        {

            if (MySelf == null) MySelf = this;
            IsLoadOnlyOneArea = true;
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            LoadNode();
            IsSearchTreeVisi = Visibility.Collapsed;
            //Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Update, 1, DelayEventHappen.EventOne);  //20170519
            this.SetIsCollapsedWhenChildItemsEmptyNByKeyType(1, true);
            this.SetIsCollapsedWhenChildItemsEmptyNByKeyType(2, true);
            this.SetIsCollapsedWhenChildItemsEmptyNByKeyType(4, true);

            IsVir = true;
        }

    

        protected bool IsLoadOnlyOneArea = false;


        #region tab iinterface

        public int Index
        {
            get { return 3; }
        }

        public string Title
        {
            get
            {
                return "单灯分组";
                return "Map";
            }
        }


        public bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

    


        private EventHandler<NodeSelectedArgs> OnSelectedNodeByCodeIns;

        event EventHandler<NodeSelectedArgs> IIAreaTree.OnSelectedNodeByCode
        {
            add { OnSelectedNodeByCodeIns += value; }
            remove
            {
                if (OnSelectedNodeByCodeIns != null) OnSelectedNodeByCodeIns -= value;
            }
        }


    };

    //search
    public partial class TreeTabViewModel
    {

        private ObservableCollection<TreeViewBaseNode> _searchchildTreeItemsInfo;

        public ObservableCollection<TreeViewBaseNode> ChildTreeItemsSearch
        {
            get
            {
                if (_searchchildTreeItemsInfo == null)
                    _searchchildTreeItemsInfo = new ObservableCollection<TreeViewBaseNode>();
                return _searchchildTreeItemsInfo;
            }
        }

        #region Search Node

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    this.RaisePropertyChanged(() => this.SearchText);
                    SearchNode(_searchText);
                    if (!string.IsNullOrEmpty(SearchTextCtrl) && !string.IsNullOrEmpty(SearchText))
                    {
                        SearchNodeCtrlBySlu(SearchTextCtrl);
                    }
                    else if (!string.IsNullOrEmpty(SearchTextCtrl) && string.IsNullOrEmpty(SearchText))
                    {
                        SearchNodeCtrl(SearchTextCtrl);
                    }

                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        var ins = new PublishEventArgs()
                        {
                            EventType = PublishEventType.Core,
                            EventId =
                                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                        };
                        EventPublish.PublishEvent(ins);
                    }
                }
            }
        }

        private string _searchCtrl;

        public string SearchTextCtrl
        {
            get { return _searchCtrl; }
            set
            {
                if (_searchCtrl != value)
                {
                    _searchCtrl = value;
                    this.RaisePropertyChanged(() => this.SearchTextCtrl);
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        SearchNodeCtrlBySlu(value);
                    }
                    else
                    {
                        SearchNodeCtrl(value);
                    }

                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        var ins = new PublishEventArgs()
                        {
                            EventType = PublishEventType.Core,
                            EventId =
                                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                        };
                        EventPublish.PublishEvent(ins);
                    }
                }
            }
        }

        //CmdClearUpSearchText

        #region CmdClearUpSearchText

        private ICommand _cmdCmdClearUpSearchText;

        public ICommand CmdClearUpSearchText
        {
            get
            {
                if (_cmdCmdClearUpSearchText == null)
                    _cmdCmdClearUpSearchText = new RelayCommand(ExCmdClearUpSearchText, CanCmdClearUpSearchText, false);
                return _cmdCmdClearUpSearchText;
            }
        }

        private void ExCmdClearUpSearchText()
        {
            SearchText = "";

        }


        private bool CanCmdClearUpSearchText()
        {
            return !string.IsNullOrEmpty(SearchText);
            ;
        }



        #endregion

        #region CmdClearUpSearchTextCtrl

        private ICommand _cmdCmdClearUpSearchTextCtrl;

        public ICommand CmdClearUpSearchTextCtrl
        {
            get
            {
                if (_cmdCmdClearUpSearchTextCtrl == null)
                    _cmdCmdClearUpSearchTextCtrl =
                        new RelayCommand(ExCmdClearUpSearchTextCtrl, CanCmdClearUpSearchTextCtrl, false);
                return _cmdCmdClearUpSearchTextCtrl;
            }
        }

        private void ExCmdClearUpSearchTextCtrl()
        {
            SearchTextCtrl = "";

        }


        private bool CanCmdClearUpSearchTextCtrl()
        {
            return !string.IsNullOrEmpty(SearchTextCtrl);
            ;
        }

        #endregion

  

        //查询集中器
        private void SearchNode(string keyWord)
        {
            ChildTreeItemsSearch.Clear();
            if (keyWord == "")
            {
                IsSearchTreeVisi = Visibility.Collapsed;
                ChildTreeItemsSearch.Clear();
                return;
            }

            foreach (var l in DicItems)
            {
                if (l.Key.Item1 != 3) continue;
                if (l.Value.Count == 0) continue;


                var nodeId = l.Value[0];
                if (nodeId.Id3StoreN.ToString().Contains(keyWord))
                {
                    nodeId.NodeName4B = " 物理地址-" + nodeId.Id3StoreN;
                    ChildTreeItemsSearch.Add(nodeId);
                    continue;
                }

                if (nodeId.Key2.ToString().Contains(keyWord))
                {
                    nodeId.NodeName4B = " 逻辑地址-" + nodeId.Key2;
                    ChildTreeItemsSearch.Add(nodeId);
                    continue;
                }

                if (nodeId.Str2StoreN!=null &&  nodeId.Str2StoreN.Contains(keyWord))
                {
                    nodeId.NodeName4B = " 手机号码-" + nodeId.Str2StoreN;
                    ChildTreeItemsSearch.Add(nodeId);
                    continue;
                }

                if (nodeId.Str3StoreN != null && StringContainKeyword(nodeId.Str3StoreN, keyWord))
                {
                    nodeId.NodeName4B = " 集中器名称";
                    ChildTreeItemsSearch.Add(nodeId);
                    continue;
                }

                if (nodeId.Str1StoreN != null && StringContainKeyword(nodeId.Str1StoreN, keyWord))
                {
                    nodeId.NodeName4B = " Ip-" + nodeId.Str1StoreN.Trim();
                    ChildTreeItemsSearch.Add(nodeId);
                }

            }

            IsSearchTreeVisi = Visibility.Visible;

            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };

            var info = (from t in ChildTreeItemsSearch select t.Key2).ToList();
            ins.AddParams(info);
            if (info.Count > 0)
            {
                EventPublish.PublishEvent(ins);
            }

        }

        private ObservableCollection<TreeViewBaseNode> _collectionWj2090SrCtrl;

        public ObservableCollection<TreeViewBaseNode> ChildTreeItemsSearchCtrl
        {
            get
            {
                return _collectionWj2090SrCtrl ??
                       (_collectionWj2090SrCtrl = new ObservableCollection<TreeViewBaseNode>());
            }
            set
            {
                if (value == _collectionWj2090SrCtrl) return;
                _collectionWj2090SrCtrl = value;
                RaisePropertyChanged(() => ChildTreeItemsSearchCtrl);
            }
        }

        private void SearchNodeCtrl(string keyWord)
        {
            ChildTreeItemsSearchCtrl.Clear();
            if (keyWord == "")
            {
                IsSearchTreeVisiCtrl = Visibility.Collapsed;
                ChildTreeItemsSearchCtrl.Clear();
                return;
            }


            foreach (var l in DicItems)
            {
                if (l.Key.Item1 != 5) continue;
                if (l.Value.Count == 0) continue;
                var f = l.Value[0];

                if (f.Str2StoreN!=null &&  f.Str2StoreN.ToUpper().Contains(keyWord.ToUpper()))
                {
 

                    f.NodeName4B = " -灯杆编码" + f.Str2StoreN;
                    ChildTreeItemsSearchCtrl.Add(f);
                    continue;
                }

                if (f.Str1StoreN != null && f.Str1StoreN.ToString().ToUpper().Contains(keyWord.ToUpper()))
                {

                    f.NodeName4B = " -条形码" + f.Str1StoreN;
                    ChildTreeItemsSearchCtrl.Add(f );
                    continue;
                }

            }
             

            IsSearchTreeVisiCtrl = Visibility.Visible;

            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };

            var info = (from t in ChildTreeItemsSearch select t.Key2 ).ToList();
            ins.AddParams(info);
            if (info.Count > 0)
            {
                EventPublish.PublishEvent(ins);
            }

        }

        //查询终端
        private void SearchNodeCtrlBySlu(string CtrlkeyWord)
        {
            ChildTreeItemsSearchCtrl.Clear();
            if (CtrlkeyWord == "")
            {
                IsSearchTreeVisiCtrl = Visibility.Collapsed;
                ChildTreeItemsSearchCtrl.Clear();
                return;
            }

          
            var rtus = (from t in ChildTreeItemsSearch select t.Key2 ).ToList();

            foreach (var l in DicItems)
            {
                if (l.Key.Item1 != 5) continue;
                if (l.Value.Count == 0) continue;
                if (rtus.Contains(l.Key.Item2) == false) continue;

                var f = l.Value[0];

                if (f.Str2StoreN != null && f.Str2StoreN.ToUpper().Contains(CtrlkeyWord.ToUpper()))
                {


                    f.NodeName4B = " -灯杆编码" + f.Str2StoreN;
                    ChildTreeItemsSearchCtrl.Add(f);
                    continue;
                }

                if (f.Str1StoreN != null && f.Str1StoreN.ToString().ToUpper().Contains(CtrlkeyWord.ToUpper()))
                {

                    f.NodeName4B = " -条形码" + f.Str1StoreN;
                    ChildTreeItemsSearchCtrl.Add(f);
                    continue;
                }

            }

            IsSearchTreeVisiCtrl = Visibility.Visible;

        }

        private Visibility _isSearchTreeVisi;

        public Visibility IsSearchTreeVisi
        {
            get { return _isSearchTreeVisi; }
            set
            {
                if (value == _isSearchTreeVisi) return;
                _isSearchTreeVisi = value;
                this.RaisePropertyChanged(() => this.IsSearchTreeVisi);
            }
        }

        private Visibility _isSearchTreeVisiCtrl;

        public Visibility IsSearchTreeVisiCtrl
        {
            get { return _isSearchTreeVisiCtrl; }
            set
            {
                if (value == _isSearchTreeVisiCtrl) return;
                _isSearchTreeVisiCtrl = value;
                this.RaisePropertyChanged(() => this.IsSearchTreeVisiCtrl);
            }
        }


        /// <summary>
        /// 前者是否包含后者数据 
        /// </summary>
        /// <param name="containerStinng"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool StringContainKeyword(string containerStinng, string keyword)
        {
            if (containerStinng.Contains(keyword)) return true;
            string conv = chinesecap(containerStinng);
            if (conv.Contains(keyword)) return true;
            if (containerStinng.ToUpper().Contains(keyword.ToUpper())) return true;
            return false;
        }


        /// <summary>
        /// 返回汉字字符串的拼音的首字母
        /// </summary>
        /// <param name="chinesestr">要转换的字符串</param>
        /// <returns></returns>
        public string chinesecap(string chinesestr)
        {
            byte[] zw = new byte[2];
            string charstr = "";
            string capstr = "";
            for (int i = 0; i <= chinesestr.Length - 1; i++)
            {
                charstr = chinesestr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
                zw = System.Text.Encoding.Default.GetBytes(charstr);
                // 得到汉字符的字节数组
                if (zw.Length == 2)
                {
                    int i1 = (short) (zw[0]);
                    int i2 = (short) (zw[1]);
                    long chinesestrInt = i1 * 256 + i2;
                    //table of the constant list
                    // a; //45217..45252
                    // z; //54481..55289
                    capstr += GetChinesefirst(chinesestrInt);
                }
                else
                {
                    capstr += charstr;
                }

                //capstr = capstr + chinastr;
            }

            return capstr;
        }

        private string GetChinesefirst(long chinesestrInt)
        {
            string chinastr = "";
            //table of the constant list
            // a; //45217..45252
            // b; //45253..45760
            // c; //45761..46317
            // d; //46318..46825
            // e; //46826..47009
            // f; //47010..47296
            // g; //47297..47613

            // h; //47614..48118
            // j; //48119..49061
            // k; //49062..49323
            // l; //49324..49895
            // m; //49896..50370
            // n; //50371..50613
            // o; //50614..50621
            // p; //50622..50905
            // q; //50906..51386

            // r; //51387..51445
            // s; //51446..52217
            // t; //52218..52697
            //没有u,v
            // w; //52698..52979
            // x; //52980..53640
            // y; //53689..54480
            // z; //54481..55289

            if ((chinesestrInt >= 45217) && (chinesestrInt <= 45252))
            {
                chinastr = "a";
            }
            else if ((chinesestrInt >= 45253) && (chinesestrInt <= 45760))
            {
                chinastr = "b";
            }
            else if ((chinesestrInt >= 45761) && (chinesestrInt <= 46317))
            {
                chinastr = "c";
            }
            else if ((chinesestrInt >= 46318) && (chinesestrInt <= 46825))
            {
                chinastr = "d";
            }
            else if ((chinesestrInt >= 46826) && (chinesestrInt <= 47009))
            {
                chinastr = "e";
            }
            else if ((chinesestrInt >= 47010) && (chinesestrInt <= 47296))
            {
                chinastr = "f";
            }
            else if ((chinesestrInt >= 47297) && (chinesestrInt <= 47613))
            {
                chinastr = "g";
            }
            else if ((chinesestrInt >= 47614) && (chinesestrInt <= 48118))
            {
                chinastr = "h";
            }

            else if ((chinesestrInt >= 48119) && (chinesestrInt <= 49061))
            {
                chinastr = "j";
            }
            else if ((chinesestrInt >= 49062) && (chinesestrInt <= 49323))
            {
                chinastr = "k";
            }
            else if ((chinesestrInt >= 49324) && (chinesestrInt <= 49895))
            {
                chinastr = "l";
            }
            else if ((chinesestrInt >= 49896) && (chinesestrInt <= 50370))
            {
                chinastr = "m";
            }

            else if ((chinesestrInt >= 50371) && (chinesestrInt <= 50613))
            {
                chinastr = "n";
            }
            else if ((chinesestrInt >= 50614) && (chinesestrInt <= 50621))
            {
                chinastr = "o";
            }
            else if ((chinesestrInt >= 50622) && (chinesestrInt <= 50905))
            {
                chinastr = "p";
            }
            else if ((chinesestrInt >= 50906) && (chinesestrInt <= 51386))
            {
                chinastr = "q";
            }

            else if ((chinesestrInt >= 51387) && (chinesestrInt <= 51445))
            {
                chinastr = "r";
            }
            else if ((chinesestrInt >= 51446) && (chinesestrInt <= 52217))
            {
                chinastr = "s";
            }
            else if ((chinesestrInt >= 52218) && (chinesestrInt <= 52697))
            {
                chinastr = "t";
            }
            else if ((chinesestrInt >= 52698) && (chinesestrInt <= 52979))
            {
                chinastr = "w";
            }
            else if ((chinesestrInt >= 52980) && (chinesestrInt <= 53640))
            {
                chinastr = "x";
            }
            else if ((chinesestrInt >= 53689) && (chinesestrInt <= 54480))
            {
                chinastr = "y";
            }
            else if ((chinesestrInt >= 54481) && (chinesestrInt <= 55289))
            {
                chinastr = "z";
            }

            return chinastr;
        }

        #endregion

        #region Reflesh  刷新 重新加载节点

        private DateTime _dtQuery;
        private ICommand _cmdQuery;

        public ICommand Reflesh
        {
            get { return _cmdQuery ?? (_cmdQuery = new RelayCommand(ExQuery, CanQuery, false)); }
        }

        private void ExQuery()
        {
            _dtQuery = DateTime.Now;
            this.LoadNode();
        }

        private bool CanQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 60000000;
        }

        #endregion

    }

 


    /// <summary>
    /// open close light
    /// </summary>
    public partial class TreeTabViewModel
    {

        private int _curnetselectrtu = 0;
        private int _curstate = 0;
        private int _curselectctrl = 0;

        private void OnCurrentSelectedNode(int nodeid)
        {
            var equip = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeid);
            if (equip != null)
            {
                if (_curnetselectrtu == nodeid) return;
                _curnetselectrtu = nodeid;
                _curselectctrl = 0;
                _curstate = equip.RtuStateCode;
                CurRtuInof = "快速操作: " + equip.RtuPhyId.ToString("d3") + " - " + equip.RtuName;

            }
        }

        private void OnCurrentSelectedCtrlNode(int nodeid, int ctrlid)
        {
            var equip = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeid);
            if (equip != null)
            {
                if (_curnetselectrtu == nodeid && _curselectctrl == ctrlid) return;
                _curnetselectrtu = nodeid;
                _curselectctrl = ctrlid;
                _curstate = equip.RtuStateCode;
                CurRtuInof = "快速操作: " + equip.RtuPhyId.ToString("d3") + " - " + equip.RtuName;

            }
        }

        #region CmdOc

        private ICommand _cmdCmdOcrchText;

        public ICommand CmdOpenCloselight
        {
            get
            {
                if (_cmdCmdOcrchText == null)
                    _cmdCmdOcrchText = new RelayCommand<string>(ExCmdOpenCloselight, CanCmdOpenCloselight, true);
                return _cmdCmdOcrchText;
            }
        }

        private void ExCmdOpenCloselight(string s)
        {
            try
            {
                if (_curselectctrl < 1) return;
                int x = 0;
                if (Int32.TryParse(s, out x))
                {


                    var loops = (from t in KxInfo where t.IsSelected orderby t.Value ascending select t.Value)
                        .ToList();
                    if (loops.Count == 0) return;

                    string tr = x == 1 ? "开灯" : "关灯";
                    if (x == 1)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 1)
                        {
                            if (
                                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                    "您将要进行" + tr + "操作，是否继续？", WlstMessageBoxType.YesNo) ==
                                WlstMessageBoxResults.No)
                            {
                                return;
                            }
                        }
                        else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                        {


                            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                            if (sss == UMessageBoxWantPassWord.CancelReturn)
                            {
                                return;
                            }

                            if (sss != UserInfo.UserLoginInfo.UserPassword)
                            {
                                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                    UMessageBoxButton.Yes);
                                return;
                            }
                        }
                    }
                    else if (x == 2)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 1)
                        {
                            if (
                                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                    "您将要进行" + tr + "操作，是否继续？", WlstMessageBoxType.YesNo) ==
                                WlstMessageBoxResults.No)
                            {
                                return;
                            }
                        }
                        else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 2)
                        {


                            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                            if (sss == UMessageBoxWantPassWord.CancelReturn)
                            {
                                return;
                            }

                            if (sss != UserInfo.UserLoginInfo.UserPassword)
                            {
                                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                    UMessageBoxButton.Yes);
                                return;
                            }
                        }
                    }
                    else if (x == 3)
                    {
                        // 刷新 test  事后去除 lvf 2019年5月27日10:57:52 
                        LoadNode();
                    }

                    OpenClsoelIGT(_curnetselectrtu, _curselectctrl, loops, x == 1);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OpenClsoelIGT(int rtuId, int ctrlId, List<int> loops, bool isOpen)
        {

            var info = Wlst.Sr.ProtocolPhone.LxSlu
                .wst_slu_right_operator; //.wlst_cnt_wj3090_order_open_close_light;
            //.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight;

            bool l1 = KxInfo[0].IsSelected;
            bool l2 = KxInfo[1].IsSelected;
            bool l3 = KxInfo[2].IsSelected;
            bool l4 = KxInfo[3].IsSelected;
            if (l1 == false && l2 == false && l3 == false && l4 == false) return;

            var data = new Wlst.client.SluRightOperators.SluRightOperator();

            data.SluId = rtuId;
            data.OperationOrder = 0;
            data.AddrType = 3;
            data.Addr = ctrlId;

            data.CmdType = 4; //todo
            int orderid = isOpen ? 1 : 4;
            //var scale = orderid;
            //if (scale > 4) scale = scale - 5;

            data.CmdMix = new List<int>() {l1 ? orderid : 0, l2 ? orderid : 0, l3 ? orderid : 0, l4 ? orderid : 0};
            data.CmdPwmField = new Wlst.client.SluRightOperators.SluRightOperator.CmdPwm()
            {
                LoopCanDo = new List<int>() {1, 2, 3, 4},
                Scale = 0
            };


            info.WstSluRightOperator.OperatorItems.Add(data);
            SndOrderServer.OrderSnd(info, 0, 0, true);
        }


        private bool CanCmdOpenCloselight(string s)
        {
            if (_curnetselectrtu < 1000) return false;
            if (_curselectctrl < 1) return false;
            if ((from t in KxInfo where t.IsSelected select t).Count() == 0) return false;
            return _curstate == 2;
        }



        #endregion


        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _searchchsdfsdfsInfo;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> KxInfo
        {
            get
            {
                if (_searchchsdfsdfsInfo == null)
                {
                    _searchchsdfsdfsInfo = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool>();
                    for (int i = 1; i < 17; i++)
                        _searchchsdfsdfsInfo.Add(new NameIntBool() {Name = "K" + i, Value = i, IsSelected = false});
                }

                return _searchchsdfsdfsInfo;
            }
        }



        public string CurRtuInof
        {
            get { return GetZstring(() => CurRtuInof); }
            set { SetZ(() => CurRtuInof, value); }
        }

        public int CmdPwmValue
        {
            get { return GetZint(() => CmdPwmValue); }
            set
            {
                if (value > 10) value = 10;
                if (value < 0) value = 0;
                SetZ(() => CmdPwmValue, value);
                StrButton = "调光 " + (value * 10) + "%";
            }
        }

        public string StrButton
        {
            get { return GetZstring(() => StrButton); }
            set { SetZ(() => StrButton, value); }
        }
    }

}
