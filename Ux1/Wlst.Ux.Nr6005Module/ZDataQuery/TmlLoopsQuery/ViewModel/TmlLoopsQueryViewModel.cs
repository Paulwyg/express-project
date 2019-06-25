using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Nr6005Module.ZDataQuery.TmlLoopsQuery.Services;


namespace Wlst.Ux.Nr6005Module.ZDataQuery.TmlLoopsQuery.ViewModel
{
    [Export(typeof (IITmlLoopsQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public  class TmlLoopsQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITmlLoopsQueryViewModel
    {
        private bool _thisViewActive = false;

        public TmlLoopsQueryViewModel()
        {


        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //lvf 获取区域信息  并将区域终端存于rtusbelongArea list中   2018年4月9日15:25:14
            getAreaRId();
            
        }


        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            Records.Clear();
            
        }

        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                return "终端回路统计"; //I36N .Services.I36N .ConvertByCodingOne("11090001", "Setting");
                //return "Setting";
            }
        }

        public bool CanClose
        {
            get { return true; }
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



        #region AreaID

        public void getAreaRId()
        {
            AreaName.Clear();
            AreaName.Add(new AreaInt() { Value = "全部", Key = -1 });
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {

                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    var tmlLstOfArea =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t.Value.AreaId);
                    if (tmlLstOfArea.Count == 0) continue;
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        var tmlLstOfArea =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t);
                        if (tmlLstOfArea.Count == 0) continue;
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }
            AreaComboBoxSelected = AreaName[0];

        }

        private static ObservableCollection<AreaInt> _devices;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }

        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private AreaInt _areacomboboxselected;

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;

                    GetGrpIdByAreaId();

                    //this.Records.Clear();

                    ////将属于这个区域的终端存入 RtusBelongArea 中，查询时判断是否属于该区域， lvf 2018年4月9日15:26:32
                    //RtusBelongArea.Clear();
                    //Remind = "";
                    //var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    //if (rtulst.Count == 0) return;
                    //RtusBelongArea.AddRange(rtulst);

                }
            }
        }




        public static int AreaId = new int();

        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        #endregion


        #region  Group


        private Visibility _txtgrpVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility GrpVisi
        {
            get { return _txtgrpVisi; }
            set
            {
                if (value != _txtgrpVisi)
                {
                    _txtgrpVisi = value;
                    this.RaisePropertyChanged(() => this.GrpVisi);
                }
            }
        }

        private static ObservableCollection<GroupInt> _grpdevices;

        public static ObservableCollection<GroupInt> GroupName
        {
            get
            {
                if (_grpdevices == null)
                {
                    _grpdevices = new ObservableCollection<GroupInt>();
                }
                return _grpdevices;
            }

        }

        public class GroupInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private GroupInt _grpcomboboxselected;
        private int GrpId;

        public GroupInt GroupComboBoxSelected
        {
            get { return _grpcomboboxselected; }
            set
            {
                if (_grpcomboboxselected != value)
                {
                    _grpcomboboxselected = value;
                    this.RaisePropertyChanged(() => this.GroupComboBoxSelected);
                    if (value == null) return;
                    GrpId = value.Key;
                }
            }
        }


        public void GetGrpIdByAreaId()
        {
            GroupName.Clear();

            if (AreaId == -1) //全部区域
            {
                GrpVisi = Visibility.Collapsed;

            }
            else
            {
                GrpVisi = Visibility.Visible;
                var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                if (area == null) return;
                var grps =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
                GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
                if (grps.Count > 0)
                {
                    var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
                    foreach (var f in grpsTmp)
                    {
                        var grptml =
    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(AreaId,
                                                                                  f.GroupId);
                        if (grptml.Count == 0) continue;


                        GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });
                    }
                }
                GroupComboBoxSelected = GroupName[0];
            }



        }

        #endregion


        #region Records


        private ObservableCollection<TmlLoopsOneItem> _records;

        public ObservableCollection<TmlLoopsOneItem> Records
        {
            get { return _records ?? (_records = new ObservableCollection<TmlLoopsOneItem>()); }
            set
            {
                if (_records != value)
                {
                    _records = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }
        }


        #endregion


        #region Remind

        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                this.RaisePropertyChanged(() => this.Remind);
            }
        }

        #endregion


        #region CmdQuery


        private ICommand _cmdquery;


        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdquery == null)
                    _cmdquery = new RelayCommand(ExCmdQuery, CanCmdQuery, false);
                return _cmdquery;
            }
        }

        private void ExCmdQuery()
        {
            _dtQuery = DateTime.Now;
            Records.Clear();
            var rtulst =GetRtusLst();
            var index = 1;
            foreach (var g in rtulst)
            {
                Records.Add(new TmlLoopsOneItem()
                                {
                                    Index = index++,
                                    RtuId = g,
                                });   
            }
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  查询成功，共计" + Records.Count + " 条数据.";
        }


        private List<int> GetRtusLst()
        {
            var rtulst = new List<int>();
            if ( AreaId ==-1)//全部区域终端
            {
                var tmplst =Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;
                foreach (var g in tmplst)
                {
                    if (g > 1000000 && g < 1100000)
                    {
                        if (rtulst.Contains(g) == false) rtulst.Add(g);
                    }
                }
                
            }
            else
            {
                if (GrpId == -1)
                {
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            if (rtulst.Contains(g)==false )rtulst .Add(g);
                        }
                    }
                }
                else
                {
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                        AreaId, GrpId); //GetRtuInArea(AreaId);
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            if (rtulst.Contains(g) == false) rtulst.Add(g);
                        }
                    }
                }
            }


            return rtulst;

        }

        private bool CanCmdQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }


        #endregion

        //打印
        #region CmdPrint

        private DateTime _dtCmdExport;
        private ICommand _cmdPrint;
        public ICommand CmdPrint
        {
            get
            {
                if (_cmdPrint == null)
                    _cmdPrint = new RelayCommand(ExCmdPrint, CanExPrint, false);
                return _cmdPrint;
            }
        }

        private void ExCmdPrint()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("序号");
                tabletitle.Add("终端地址");
                tabletitle.Add("终端名称");
                tabletitle.Add("区域");
                tabletitle.Add("回路数量");
                tabletitle.Add("安装日期");
                tabletitle.Add("安装地点");
                var table = new List<List<string>>();
                foreach (var g in Records)
                {
                   
                    var tem = new List<string>();
                    tem.Add(g.Index+"");
                    tem.Add(g.PhyId+"");
                    tem.Add(g.RtuName);
                    tem.Add(g.AreaName);
                    tem.Add(g.LoopsNum+"");
                    tem.Add(g.InstallDate + "");
                    tem.Add(g.Position + "");
                   
                    table.Add(tem);
                }
                Wlst.print.Prints.Print(tabletitle, table, false, "箱体回路统计表", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }
        #endregion
    }






    public class TmlLoopsOneItem : ObservableObject
    {
        public TmlLoopsOneItem()
        {

            RtuName = "未知";
            PhyId = 0;
            InstallDate = "";
            Position = "";
            RtuId = 0;
            Index = 0;
            LoopsNum = 0;

        }


        #region   attri

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }


        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);

                    //基本信息
                    var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(_rtuId);
                    if (info == null) return;
                    RtuName = info.RtuName;
                    PhyId = info.RtuPhyId;
                    InstallDate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                    Position = info.RtuInstallAddr;

                    LoopsNum = 0;
                    //回路信息
                    var tmps =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tmps == null) return;
                    LoopsNum = tmps.WjLoops.Count;

                    //区域信息
                    var areaId =
                        Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(_rtuId);

                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
                    {
                        AreaName =
                            Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                    }
                    else
                    {
                        AreaName = "未知";
                    }
                    if (areaId == 0) AreaName = "默认区域";

                }
            }
        }

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }


        private int _loopsNum;

        public int LoopsNum
        {
            get { return _loopsNum; }
            set
            {
                if (_loopsNum != value)
                {
                    _loopsNum = value;
                    this.RaisePropertyChanged(() => this.LoopsNum);
                }
            }
        }


        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }


        private string _areaName;

        public string AreaName
        {
            get { return _areaName; }
            set
            {
                if (value != _areaName)
                {
                    _areaName = value;
                    this.RaisePropertyChanged(() => this.AreaName);
                }
            }
        }


        private string _installDate;

        public string InstallDate
        {
            get { return _installDate; }
            set
            {
                if (value != _installDate)
                {
                    _installDate = value;
                    this.RaisePropertyChanged(() => this.InstallDate);
                }
            }
        }



        private string _position;

        public string Position
        {
            get { return _position; }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    this.RaisePropertyChanged(() => this.Position);
                }
            }
        }



        #endregion

    }
}
