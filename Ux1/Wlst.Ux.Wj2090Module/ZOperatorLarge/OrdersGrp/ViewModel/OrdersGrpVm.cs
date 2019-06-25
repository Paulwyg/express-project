using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.ViewModel
{
    [Export(typeof (IIOrdersGrp))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OrdersGrpVm : Wlst.Cr.Core.CoreServices.ObservableObject, IIOrdersGrp
    {
        public OrdersGrpVm()
        {
            this.InitAction();
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯控制中心"; }
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

        private bool _thisViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {

            _thisViewActive = true;
            this.LoadTree();
            FlagDataType = 1;
            ////GetAreaId4W();
            OcCount = 0;
            _isSyncTime = false;
            _isZcTime = false;
            _isZcVer = false;
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            Items.Clear();
        }

        #region Items

        private ObservableCollection<TreeGroupNode> _concentratorItems;

        public ObservableCollection<TreeGroupNode> Items   //SluInfo
        {
            get { return _concentratorItems ?? (_concentratorItems = new ObservableCollection<TreeGroupNode>()); }
            set
            {
                if (value == _concentratorItems) return;
                _concentratorItems = value;
                this.RaisePropertyChanged(() => this.Items);
            }
        }


        //private ObservableCollection<TreeNodeBase> _concentratorQueryItems;    //查询后的结果

        //public ObservableCollection<TreeNodeBase> CollectionQueryWj2090
        //{
        //    get { return _concentratorQueryItems ?? (_concentratorQueryItems = new ObservableCollection<TreeNodeBase>()); }
        //    set
        //    {
        //        if (value == _concentratorQueryItems) return;
        //        _concentratorQueryItems = value;
        //        this.RaisePropertyChanged(() => this.CollectionQueryWj2090);
        //    }
        //}

        private ObservableCollection<ReslutGrpItem> _coReslutItems;

        public ObservableCollection<ReslutGrpItem> ReslutItems
        {
            get { return _coReslutItems ?? (_coReslutItems = new ObservableCollection<ReslutGrpItem>()); }
            set
            {
                if (value == _coReslutItems) return;
                _coReslutItems = value;
                this.RaisePropertyChanged(() => this.ReslutItems);
            }
        }

        private void LoadTree()
        {
            Items.Clear();
            //CollectionQueryWj2090.Clear();


            //////////var a = new List<int>();
            //////////foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX)
            //////////{
            //////////    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
            //////////    {
            //////////         a.AddRange(Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].LstTml) ;
            //////////    }
            //////////}
            ////////////var grps = (from t in SrInfo.SluGrpInfoHold.MySelf.Info orderby t.Key ascending select t.Key).ToList();
            //////////var infx = (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems 
            //////////            where t.Key > Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart &&
            //////////                  t.Key < Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd && a.Contains(t.Key)
            //////////            select t.Key).ToList();
            //////////foreach (var g in infx)
            //////////{
                
            //////////    this.CollectionWj2090.Add(new SluInfo(g));
            //////////    this.CollectionQueryWj2090.Add(new SluInfo(g));
            //////////}


            var areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            //foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR )
            //{
            //    if (areas.Contains(f) == false) areas.Add(f);
            //}
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false) areas.Add(f);
                }
            }
            AreaCount = areas.Count > 1;
            //this.CollectionWj2090.Add(new TreeGroupNode(-1, -1));
            foreach (var f in areas)
            {
                var grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(f);
                foreach (var g in grps)
                {
                    this.Items.Add(new TreeGroupNode(f, g.GroupId));
                }
                this.Items.Add(new TreeGroupNode(f, 0));
            }

            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].ChildTreeItems.Count == 0) Items.RemoveAt(i);
            }




        }

    

        #endregion

        #region attri



        private int _flagVsdfsdisi;

        /// <summary>
        /// 1 控制界面，2数据返回界面，3其他界面返回
        /// </summary>
        public int FlagDataType
        {
            get { return _flagVsdfsdisi; }
            set
            {
                if (value < 1) value = 1;
                if (value > 3) value = 3;
                if (_flagVsdfsdisi == value) return;
                _flagVsdfsdisi = value;
                RaisePropertyChanged(() => FlagDataType);

                if (value == 1) CmdText = "查看执行情况";
                else if (value == 3) CmdText = "返回";
                else CmdText = "返回控制界面";
            }
        }

        private bool _flsdfsdffsddisi;

        /// <summary>
        /// 
        /// </summary>
        public bool IsHasData
        {
            get { return _flsdfsdffsddisi; }
            set
            {
                if (_flsdfsdffsddisi == value) return;
                _flsdfsdffsddisi = value;
                RaisePropertyChanged(() => IsHasData);

            }
        }

        private string _fssdfsdfsdddisi;

        /// <summary>
        /// 
        /// </summary>
        public string CmdText
        {
            get { return _fssdfsdfsdddisi; }
            set
            {
                if (_fssdfsdfsdddisi == value) return;
                _fssdfsdfsdddisi = value;
                RaisePropertyChanged(() => CmdText);
            }
        }

        private int _sluId;

        /// <summary>
        /// 单灯逻辑地址
        /// </summary>
        public int CountAll
        {
            get { return _sluId; }
            set
            {
                if (_sluId == value) return;
                _sluId = value;
                RaisePropertyChanged(() => CountAll);
            }
        }



        private int _sluPhyId;

        /// <summary>
        /// 单灯物理地址
        /// </summary>
        public int CountReturn
        {
            get { return _sluPhyId; }
            set
            {
                if (value != _sluPhyId)
                {
                    _sluPhyId = value;
                    this.RaisePropertyChanged(() => this.CountReturn);
                }
            }
        }

        #region Remind

        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (_remind == value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }



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
                   
                }
            }
        }

        #endregion

        #endregion


        #region OcCount


        private int _remindOcCount;
        public int OcCount
        {
            get { return _remindOcCount; }
            set
            {
                if (_remindOcCount == value) return;
                _remindOcCount = value;
                RaisePropertyChanged(() => OcCount);
            }
        }
        #endregion

        private bool _isSyncTime; //按下 对时按钮
        private bool _isZcTime; //按下  召测对时按钮
        private bool _isZcVer; //按下   召测版本按钮

        #region SyncTime

        private string _syncTime;
        /// <summary>
        /// 对时按钮的文本
        /// </summary>
        public string SyncTime
        {
            get { return _syncTime; }
            set
            {
                if (_syncTime == value) return;
                _syncTime = value;
                RaisePropertyChanged(() => SyncTime);
            }
        }

        #endregion

        #region ZcTime

        private string _zcTime;
        /// <summary>
        /// 召测时间按钮文本
        /// </summary>
        public string ZcTime
        {
            get { return _zcTime; }
            set
            {
                if (_zcTime == value) return;
                _zcTime = value;
                RaisePropertyChanged(() => ZcTime);
            }
        }

        #endregion

        #region ZcVer
        private string _zcVer;
        /// <summary>
        /// 召测版本按钮文本
        /// </summary>
        public string ZcVer
        {
            get { return _zcVer; }
            set
            {
                if (_zcVer == value) return;
                _zcVer = value;
                RaisePropertyChanged(() => ZcVer);
            }
        }

        #endregion

        #region 对时报表数据

        private ObservableCollection<SyncTimeOneReport> _syncTimeReport;
        public ObservableCollection<SyncTimeOneReport> SyncTimeReport
        {
            get { return _syncTimeReport ?? (_syncTimeReport = new ObservableCollection<SyncTimeOneReport>()); }
        }
        #endregion

        #region CmdOrders

        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdOrders
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand<string>(ExCmdZcOrSnd, CanCmdZcOrSnd, true)); }
        }


        protected int LastExute = 0;
        private long _lastexutettime = 0;

        private void ExCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            _lastexutettime = DateTime.Now.Ticks;
            LastExute = x;

            if (x < 1) return;
            if (x > 15) return;
            if(x==1)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
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
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }
                //var sss = UMessageBoxWantSomefromUser.Show("输入验证码", "您将要进行开关灯操作，\r\n若确认请输入验证码:1234", "");
                //if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                //{
                //    return;
                //}
                //if (sss != "1234")
                //{
                //    UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                //    return;
                //}
            }
            if (x == 4)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
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
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }
                //var sss = UMessageBoxWantSomefromUser.Show("输入验证码", "您将要进行开关灯操作，\r\n若确认请输入验证码:1234", "");
                //if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                //{
                //    return;
                //}
                //if (sss != "1234")
                //{
                //    UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                //    return;
                //}
            }
            SndCmdOrders(x);

        }

        private bool CanCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x < 1) return false;
            if (x > 15) return false;
            return DateTime.Now.Ticks - _lastexutettime > 30000000;
        }

        #endregion


        #region CmdShowData 控制中心与数据显示


        private ICommand _cmdShowData;

        public ICommand CmdShowData
        {
            get { return _cmdShowData ?? (_cmdShowData = new RelayCommand(ExShowData, CanShowData, false)); }
        }

        private void ExShowData()
        {
            if (FlagDataType == 1) FlagDataType = 2;
            else
            {
                if (FlagDataType == 2) FlagDataType = 1;
            }

        }

        private bool CanShowData()
        {
            //  if (SlusReturn.Count == 0) return false;
            if (ReslutItems.Count > 0) return true;
            if (FlagDataType == 2) return true;
            return false;
        }

        #endregion

        //2018.8.9  增加对时、召测时间、召测版本按钮
        #region CmdOther 其他
        private ICommand _cmdOther;

        public ICommand CmdOther
        {
            get { return _cmdOther ?? (_cmdOther = new RelayCommand(ExOther, CanOther, false)); }
        }

        private void ExOther()
        {
            var slulist = GetRightOperatorSlus();
            OcCount = slulist.Count;
            if (OcCount < 1)
            {
                UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                return;
            }
            if (FlagDataType == 1) FlagDataType = 3;
            else
            {
                if (FlagDataType == 3) FlagDataType = 1;
            }
            SyncTime = "对时";
            ZcTime = "召测时间";
            ZcVer = "召测版本";
            _isSyncTime = false;
            _isZcTime = false;
            _isZcVer = false;
            SyncTimeReport.Clear();
            foreach (var t in slulist)
            {
                SyncTimeReport.Add(new SyncTimeOneReport()
                                       {
                                           SluId=t.Item1,
                                           SyncTimeAns=false,
                                           ZcTimeAns="---",
                                           ZcVerAns = "---"

                                       });
            }

        }

        private bool CanOther()
        {
            return true;
        }

        #endregion

        //2018.8.9  增加对时、召测时间、召测版本按钮
        #region CmdZcTimeOrVer

        private ICommand _cmdZcTimeOrVer;

        public ICommand CmdZcTimeOrVer
        {
            get { return _cmdZcTimeOrVer ?? (_cmdZcTimeOrVer = new RelayCommand<string>(ExCmdZcTimeOrVer, CanCmdZcTimeOrVer, true)); }
        }
        private void ExCmdZcTimeOrVer(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            ZcTimeOrVer(x);
        }
         private bool CanCmdZcTimeOrVer(string str)
         {
             return true;
         }

        private void ZcTimeOrVer(int x)
        {
            if (x != 10 && x != 14 && x != 15) return;
            if (x == 10) _isSyncTime = true;
            if (x == 15) _isZcTime = true;
            if (x == 14) _isZcVer = true;
            var slu = GetRightOperatorSlus();
            foreach (var t in slu)
            {
                var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_zc_or_set;
                info.WstSluZcOrSet.Op = x;
                info.WstSluZcOrSet.SluId = t.Item1;
                SndOrderServer.OrderSnd(info);
            }
        }
        #endregion

        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("终端地址");
                lsttitle.Add("物理地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("对时");
                lsttitle.Add("召测时间");
                lsttitle.Add("召测版本");

                var lstobj = new List<List<object>>();
                int index = 1;
                foreach (var g in SyncTimeReport)
                {

                    var tmp = new List<object>();
                    tmp.Add(index);
                    tmp.Add(g.SluId);
                    tmp.Add(g.SluShowId);
                    tmp.Add(g.SluName);
                    tmp.Add(g.SyncTimeAns ? "成功" : "失败");
                    tmp.Add(g.ZcTimeAns);
                    tmp.Add(g.ZcVerAns);
                    index++;

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (SyncTimeReport.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdSelectes

        private ICommand _cmCmdSelectes;

        public ICommand CmdSelectes
        {
            get { return _cmCmdSelectes ?? (_cmCmdSelectes = new RelayCommand<string>(ExCmdSelectes, CanCmdSelectes, true)); }
        }



        private void ExCmdSelectes(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }



            foreach (var f in this.Items)
            {
                if(f.ChildTreeItems.Count<1) continue;
                foreach (var t in f.ChildTreeItems)
                {
                    if (x > 0 && t.OperatorType.Count > x - 1)
                    {
                        t.OperatorType[x - 1].IsSelected = !t.OperatorType[x - 1].IsSelected;
                    }
                }
          

            }

            //foreach (var f in this.CollectionWj2090)
            //{
            //    if (x > 0 && f.OperatorType.Count > x - 1)
            //    {
            //        f.OperatorType[x - 1].IsSelected = !f.OperatorType[x - 1].IsSelected;
            //    }

            //}

        }

        private bool CanCmdSelectes(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x < 1) return false;
            if (x > 4) return false;
            return true;
        }

        #endregion


        #region CmdQuery

        private ICommand _cmCmdQuery;

        public ICommand CmdQuery
        {
            get { return _cmCmdQuery ?? (_cmCmdQuery = new RelayCommand<string>(ExCmdQuery, CanCmdQuery, true)); }
        }


        public void Query()
        {
            ObservableCollection<SluInfo> tmp = new ObservableCollection<SluInfo>();
            if (string.IsNullOrEmpty(SearchText))
            {
                LoadTree();
                return;
            }
            //todo
            foreach (var f in this.Items)  //遍历缓存
            {
                
                foreach (var g in f.ChildTreeItems)
                {
                    if (g.SluName.Contains(SearchText))
                    {
                        if (tmp.Contains(g) == false) tmp.Add(g);

                    }
                }

            }
            if (tmp.Count == 0) return;
            Items.Clear();
            var trtmp = new TreeGroupNode();


            trtmp.NodeName = "查询结果";
            trtmp.IsGroup = true;

            foreach (var g in trtmp.OperatorType)
            {
                g.SelfNode = trtmp;
            }
            foreach (var t in tmp)
            {
                trtmp.ChildTreeItems.Add(new SluInfo( trtmp, t.SluId));
            }
            Items.Add(trtmp);
        }

        public void ExCmdQuery(string str)
        {
            int x = 0; 
           
           Query();

        }

        private bool CanCmdQuery(string str)
        {

            return true;
        }

        #endregion

        private bool _currentSelectAllStateTmp = false;
        public void SelectAllSwitchOut(int kx)
        {
            _currentSelectAllStateTmp = !_currentSelectAllStateTmp;
            switch (kx)
            {
                case 1:
                    foreach (var t in this.Items)
                    {
                        t.OperatorType[0].IsSelected = _currentSelectAllStateTmp;
                    }
                    break;
                case 2:
                    foreach (var t in this.Items)
                    {
                        t.OperatorType[1].IsSelected = _currentSelectAllStateTmp;
                    }
                    break;
                case 3:
                    foreach (var t in this.Items)
                    {
                        t.OperatorType[2].IsSelected = _currentSelectAllStateTmp;
                    }
                    break;
                case 4:
                    foreach (var t in this.Items)
                    {
                        t.OperatorType[3].IsSelected = _currentSelectAllStateTmp;
                    }
                    break;
           

            }


        }

        private bool  _areaCount;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public bool  AreaCount
        {
            get { return _areaCount; }
            set
            {
                if (_areaCount != value)
                {
                    _areaCount = value;
                    this.RaisePropertyChanged(() => this.AreaCount);
                }
            }
        }



        //public void GetAreaId4W()
        //{
        //    AreaName.Clear();
        //    foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
        //    {
        //        if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
        //        {
        //            string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
        //            AreaName.Add(new AreaInt() {Value = area, Key = t});
        //        }
        //    }
        //    if (AreaName.Count > 0) AreaComboBoxSelected = AreaName[0];
        //}
        //private static ObservableCollection<AreaInt> _devices;

        //public static ObservableCollection<AreaInt> AreaName
        //{
        //    get
        //    {
        //        if (_devices == null)
        //        {
        //            _devices = new ObservableCollection<AreaInt>();
        //        }
        //        return _devices;
        //    }

        //}

        //public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
        //{
        //    private int _key;

        //    public int Key
        //    {
        //        get { return _key; }
        //        set
        //        {
        //            if (_key != value)
        //            {
        //                _key = value;
        //                this.RaisePropertyChanged(() => this.Key);
        //            }
        //        }
        //    }

        //    private string _value;

        //    public string Value
        //    {
        //        get { return _value; }
        //        set
        //        {
        //            if (value != _value)
        //            {
        //                _value = value;
        //                this.RaisePropertyChanged(() => this.Value);
        //            }
        //        }
        //    }
        //}

        //private AreaInt _areacomboboxselected;
        //private int AreaId;

        //public AreaInt AreaComboBoxSelected
        //{
        //    get { return _areacomboboxselected; }
        //    set
        //    {
        //        if (_areacomboboxselected != value)
        //        {
        //            _areacomboboxselected = value;
        //            this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
        //            if (value == null) return;
        //            AreaId = value.Key;
        //        }
        //    }
        //}


    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class OrdersGrpVm
    {

        /// <summary>
        /// 集中器
        /// </summary>
        public void InitAction()
        {
            InitSluAction();
        }

        public void InitSluAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_right_operator ,// .wlst_svr_ans_cnt_wj2090_order_right_operator ,//.ClientPart.wlst_Wj2090svr_ans_clinet_right_operator_slu,
                SluMeasureBack,
                typeof (OrdersGrpVm), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu .wst_svr_to_cnt_auto_slu_fe ,// .wlst_svr_ans_cnt_wj2090_order_auto_fe ,//.ClientPart.wlst_Wj2090_svr_to_clinet_slu_auto_fe,
                SluRightOperFe,
                typeof (OrdersGrpVm), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_svr_ans_slu_zc_or_set,
                //.ClientPart.wlst_Wj2090_svr_ans_clinet_order_slu_zc_or_set , 
                OnZcOrSetBack,
                typeof(OrdersGrpVm), this);
        }



        public void SluMeasureBack(string session,Wlst .mobile .MsgWithMobile   infos)
        {
            var info = infos.WstSluSvrAnsRightOperator  ;
            if (info == null) return;
            if (_thisViewActive == false) return;

            if (info.Op  == 1)
            {
                if (infos.Args.Addr.Count > 2)
                {
                    int sluid = infos.Args.Addr[0];
                    int typx = infos.Args.Addr[1];
                    int grid = infos.Args.Addr[2];
                    foreach (var g in ReslutItems)
                    {
                        if (g.SluId == sluid && g.addr == grid && g.addrtype == typx)//
                        {
                            g.Nindex = info.Nindex;
                            g.AttachInfo = "已下发...";
                            return;
                        }
                    }
                }

            }
            else
            {
                //var ts = new Tuple<int, int>(info.SluId, info.Nindex);
                //if (!_second.ContainsKey(ts)) return;
                //var ngt = _second[ts];

                //var tu = new Tuple<int, long>(info.SluId, ngt);

                foreach (var g in ReslutItems)
                {
                    if (g.SluId == info.SluId)
                    {
                        if (g.Nindex == info.Nindex)
                        {
                            g.ReplyTime = DateTime.Now.ToString("HH:mm:ss");
                            if (info.Status == 0x3a) //成功
                            {
                                g.IsSuccessful = "成功";

                                g.AttachInfo = "操作成功...";
                            }
                            else if (info.Status == 0x61) //成功
                            {
                                g.IsSuccessful = "成功";
                                g.AttachInfo = "硬件正在操作.";
                            }
                            else if (info.Status == 0x62) //成功
                            {
                                g.IsSuccessful = "成功";
                                g.AttachInfo = "指令排队中，等待硬件操作.";
                            }
                            else if (info.Status == 0x63) //成功
                            {
                                g.IsSuccessful = "失败";
                                g.AttachInfo = "硬件队列已满.";
                            }
                            else if (info.Status == 0x5a) //成功
                            {
                                g.IsSuccessful = "失败";
                                g.AttachInfo = "数据错误.";
                            }
                            else
                            {
                                g.IsSuccessful = "失败";
                                g.AttachInfo = "原因未知.";
                            }
                        }
                    }
                }
            }


        }

        public void SluRightOperFe(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstSluSvrToCntAutoFe   ;
            if (info == null) return;
            if (_thisViewActive == false) return;


            //if (info.OperationCmd == 0x74 || info.OperationCmd == 0x7d) return;
            foreach (var g in ReslutItems)
            {
                if (g.SluId == info.SluId)
                {
                    //if (g.Nindex == info.Nindex)
                    //{
                        g.AttachInfo = "命令执行完毕.";
                        if (info.CtrlIds.Count == 0)
                        {
                            g.AttachInfo = "无";
                        }
                        else
                        {
                            var ntsg = GetPhyIdsByCtrls(info.CtrlIds,g.SluId );
                            var str = "共计:" + ntsg.Count + "个;分别是：";
                            foreach (var ff in ntsg)
                            {
                                str += ff;
                            }
                            g.AttachInfo = str;
                        }

                    //}
                    return;
                }
            }
        }

        public void OnZcOrSetBack(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            if (infos == null || infos.WstSluSvrAnsZcOrSet == null) return;
            var data = infos.WstSluSvrAnsZcOrSet;
            if (data.Op == 10)
            {
                if (_isSyncTime == false) return;

                foreach (var f in SyncTimeReport)
                {
                    if (f.SluId == data.SluId)
                        f.SyncTimeAns = !data.ZcJzqTime.TimeFault;
                }
                lock (this)      
                {
                    int ansNum = 0;
                    foreach (var g in SyncTimeReport)
                    {
                        if (g.SyncTimeAns == true) ansNum++;
                    }
                    SyncTime = ansNum + " 补测";


                    //if (_rtusThatOpe.Contains(f)) _rtusThatOpe.Remove(f);
                    //OcCountAns++;
                    //SyncTime = OcCountAns + " 补测";
                }
            }
            if(data.Op==14)
            {
                if (_isZcVer == false) return;

                foreach (var f in SyncTimeReport)
                {
                    if (f.SluId == data.SluId)
                        f.ZcVerAns = data.ZcSoftVersion;
                }
                lock (this) 
                {
                    //if (_rtusThatOpe.Contains(f)) _rtusThatOpe.Remove(f);


                    int ansNum = 0;
                    foreach (var g in SyncTimeReport)
                    {
                        if (g.ZcVerAns != "---") ansNum++;
                    }
                    ZcVer = ansNum + " 补测";
                }
            }
            if(data.Op==15)
            {
                if (_isZcTime == false) return;
                 foreach (var f in SyncTimeReport)
                {
                    if (f.SluId == data.SluId)
                        f.ZcTimeAns = new DateTime(data.ZcJzqTime.DateTime).ToString(
                            "yyyy-MM-dd HH:mm:ss");
                }
                 lock (this) 
                 {
                     int ansNum = 0;
                     foreach (var g in SyncTimeReport)
                     {
                         if (g.ZcTimeAns != "---") ansNum++;
                     }
                     ZcTime = ansNum + " 补测";
                 }
            }
        }

        private List<int> GetPhyIdsByCtrls(List<int> ruts, int sluId)
        {
            var rtn = new List<int>();
            var sluinfo =
               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                   sluId);
            var cons = sluinfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (cons == null) return rtn;
            foreach (var g in ruts)
            {
                if (cons.WjSluCtrls .ContainsKey(g))
                    rtn.Add(cons.WjSluCtrls [g].CtrlPhyId);
            }
            return rtn;
        }


    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class OrdersGrpVm
    {


        /// <summary>
        /// 
        /// </summary>
        private void SndCmdOrders(int orderId)
        {
            if (orderId < 1 || orderId > 15) return;
            var rtus = GetRightOperatorSlus();
            if (rtus.Count == 0)
            {
                //提示
                return;
            }
            ReslutItems.Clear();
            FlagDataType = 2;
            this.SndCmdOperatorRight(rtus,orderId );
            //foreach (var g in rtus)
            //{
            //    this.SndCmdOperatorRight(g.Item1, g.Item6, g.Item2, g.Item3, g.Item4, g.Item5, orderId);
            //}
        }


        private List<Tuple<int, bool, int, string>> GetRightOperatorSlus()
        {


            var rtn = new List<Tuple<int, bool, int, string>>();

            foreach (var g in this.Items) //组
            {
                if (g.ChildTreeItems.Count < 1) continue;
                foreach (var t in g.ChildTreeItems)
                {
                    foreach (var f in t.OperatorType) //终端slu
                    {
                        if (f.IsSelected == false) continue;
                        if (f.Value < 0) continue;
                        rtn.Add(new Tuple<int, bool, int, string>(t.SluId, f.IsGrp, f.Value, f.Name));
                    }
                }

            
            }
            var lstall = (from t in rtn where t.Item2 == false && t.Item3 == 10 select t.Item1).ToList();
            for (int j = rtn.Count - 1; j >= 0; j--)
            {
                if (!lstall.Contains(rtn[j].Item1)) continue;
                if (rtn[j].Item2 == false && rtn[j].Item3 == 10) continue;
                rtn.RemoveAt(j);
            }

            return rtn;



            //foreach (var g in this.CollectionWj2090) //组
            //{
            //    foreach (var f in g.OperatorType) //终端slu
            //    {
            //        if (f.IsSelected == false) continue;
            //        if (f.Value < 0) continue;
            //        rtn.Add(new Tuple<int, bool, int, string>(g.SluId, f.IsGrp, f.Value, f.Name));
            //    }
            //}
            //var lstall = (from t in rtn where t.Item2 == false && t.Item3 == 10 select t.Item1).ToList();
            //for (int j = rtn.Count - 1; j >= 0; j--)
            //{
            //    if (!lstall.Contains(rtn[j].Item1)) continue;
            //    if (rtn[j].Item2 == false && rtn[j].Item3 == 10) continue;
            //    rtn.RemoveAt(j);
            //}

            //return rtn;
        }




        private void SndCmdOperatorRight(List<Tuple<int, bool, int,string >> tar, int orderid)
        {
            // orderid 1-4 开灯 节能 节能 关灯  5-15 调光0%-100%
            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_right_operator ;

            int index = 0;
            bool l1 = SelectedLight[0].IsSelected;
            bool l2 = SelectedLight[1].IsSelected;
            bool l3 = SelectedLight[2].IsSelected;
            bool l4 = SelectedLight[3].IsSelected;
            if (l1 == false && l2 == false && l3 == false && l4 == false) return;



            foreach (var f in tar)
            {
                index++;
                var data = new Wlst.client.SluRightOperators.SluRightOperator();

                data.SluId = f.Item1;
                data.OperationOrder = 0;
                data.AddrType = f.Item2 ? 1 : 2;
                data.Addr = f.Item3;

                data.CmdType = orderid < 5 ? 4 : 5;
                var scale = orderid;
                if (scale > 4) scale = scale - 5;

                data.CmdMix = new List<int>() {l1 ? orderid : 0, l2 ? orderid : 0, l3 ? orderid : 0, l4 ? orderid : 0};
                data.CmdPwmField = new Wlst.client.SluRightOperators.SluRightOperator.CmdPwm()
                                       {
                                           LoopCanDo = new List<int>(),
                                           Scale = scale *10   //lvf 调光*10 2018年6月28日09:33:45
                                       };
                if (l1) data.CmdPwmField.LoopCanDo.Add(1);
                if (l2) data.CmdPwmField.LoopCanDo.Add(2);
                if (l3) data.CmdPwmField.LoopCanDo.Add(3);
                if (l4) data.CmdPwmField.LoopCanDo.Add(4);



                var nts = new ReslutGrpItem()
                              {
                                  SluId = f.Item1,
                                  addr = f.Item3,
                                  addrtype = f.Item2 ? 1 : 2,
                                  
                                  Index = ReslutItems.Count + 1,
                                  Nindex = -1,
                                  GrpName = f.Item3 + "-" + f.Item4,
                                  AttachInfo = "已经提交服务器..."

                              };

                ReslutItems.Add(nts);
                info.WstSluRightOperator .OperatorItems.Add(data);

            }
            SndOrderServer.OrderSnd(info);
        }


        private ObservableCollection<NameIntBool> _operationOperatorType = null;


        public ObservableCollection<NameIntBool> SelectedLight
        {
            get
            {
                if (_operationOperatorType == null)
                {
                    _operationOperatorType = new ObservableCollection<NameIntBool>();
                    _operationOperatorType.Add(new NameIntBool() { Name = "灯1", Value = 1,  IsSelected = true });
                    _operationOperatorType.Add(new NameIntBool() { Name = "灯2", Value = 2,  IsSelected = false });
                    _operationOperatorType.Add(new NameIntBool() { Name = "灯3", Value = 3,  IsSelected = false });
                    _operationOperatorType.Add(new NameIntBool() { Name = "灯4", Value = 4,  IsSelected = false });

                }
                return _operationOperatorType;
            }
        }

    }

    public class SyncTimeOneReport: Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _sSluId;

        public int SluId
        {
            get { return _sSluId; }
            set
            {
                if (value != _sSluId)
                {
                    _sSluId = value;
                    this.RaisePropertyChanged(() => this.SluId);

                    SluShowId = value + "";
                    SluName = value + "";

                    var sluinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                    if (sluinfo != null)
                    {
                        SluName = sluinfo.RtuName;
                        if (sluinfo.RtuFid == 0)
                        {
                            SluShowId = sluinfo.RtuPhyId.ToString("D4");
                        }
                        else
                        {
                            var mtps =
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                    sluinfo.RtuFid);
                            if (mtps != null)
                            {
                                SluShowId = mtps.RtuPhyId.ToString("D4");
                            }

                        }
                    }


                }
            }
        }

        private string _ssdfSluId;

        public string SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }


        private string _sSluName;

        public string SluName
        {
            get { return _sSluName; }
            set
            {
                if (value != _sSluName)
                {
                    _sSluName = value;
                    this.RaisePropertyChanged(() => this.SluName);
                }
            }
        }

        #region 当前状态

        private EnumTmlState _state;
        public EnumTmlState State
        {
            get { return _state; }
            set
            {
                if (value == _state) return;
                _state = value;
                RaisePropertyChanged(() => State);
            }
        }

        #endregion

        #region 对时应答

        private bool _syncTimeAns;
        public bool SyncTimeAns
        {
            get { return _syncTimeAns; }
            set
            {
                if (_syncTimeAns == value) return;
                _syncTimeAns = value;
                RaisePropertyChanged(() => SyncTimeAns);
            }
        }

        #endregion

        #region 召测对时

        private string _zcTimeAns;
        public string ZcTimeAns
        {
            get { return _zcTimeAns; }
            set
            {
                if (_zcTimeAns == value) return;
                _zcTimeAns = value;
                RaisePropertyChanged(() => ZcTimeAns);
            }
        }

        #endregion

        #region 召测版本

        private string _zcVerAns;
        public string ZcVerAns
        {
            get { return _zcVerAns; }
            set
            {
                if (_zcVerAns == value) return;
                _zcVerAns = value;
                RaisePropertyChanged(() => ZcVerAns);
            }
        }

        #endregion

    }
    
}
