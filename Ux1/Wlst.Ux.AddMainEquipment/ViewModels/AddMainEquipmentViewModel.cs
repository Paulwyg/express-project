using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.ComponentHold;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.AddMainEquipment.Services;

using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.AddMainEquipment.ViewModels
{
    [Export(typeof(IIAddMainEquipment))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AddMainEquipmentViewModel : ObservableObject, IIAddMainEquipment
    {
        private ObservableCollection<EquipmentViewItem> _equipmentModules;

        /// <summary>
        /// 设备模型集合
        /// </summary>
        public ObservableCollection<EquipmentViewItem> EquipmentModules
        {
            get { return _equipmentModules ?? (_equipmentModules = new ObservableCollection<EquipmentViewItem>()); }
        }

        private int _phyId;

        /// <summary>
        /// 物理地址设置
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId != value)
                {
                    _phyId = value;
                    RaisePropertyChanged(() => PhyId);
                }
            }
        }

        public void GetAreaId4W()
        {
            AreaName.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                    AreaName.Add(new AreaInt() { Value = area, Key = t });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = area, Key = t });
                    }
                }
            }
            //foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
            //{
            //    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
            //    {
            //        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
            //        AreaName.Add(new AreaInt() {Value = area, Key = t});
            //    }
            //}
            if (AreaName.Count > 0)
                AreaComboBoxSelected = AreaName[0];
            Visi = AreaName.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private static ObservableCollection<AreaInt> _areaName;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_areaName == null)
                {
                    _areaName = new ObservableCollection<AreaInt>();
                }
                return _areaName;
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

        private AreaInt _areacomboboxselected;
        private int AreaId;

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
                }
            }
        }

        public void GetGrpIdByAreaId()
        {
            GroupName.Clear();

            if (AreaId == -1)//全部区域
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
                GroupName.Add(new GroupInt() { Value = "无", Key = -1 });
                if (grps.Count > 0)
                {
                    var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
                    foreach (var f in grpsTmp)
                    {

                        GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });

                    }


                }
                GroupComboBoxSelected = GroupName[0];
            }



        }

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




        #endregion

        private EquipmentViewItem _currentSelectEquipmentMoudle;

        public EquipmentViewItem CurrentSelectEquipmentMoudle
        {
            get { return _currentSelectEquipmentMoudle ?? (_currentSelectEquipmentMoudle = new EquipmentViewItem()); }
            set
            {
                if (value != _currentSelectEquipmentMoudle)
                {
                    _currentSelectEquipmentMoudle = value;
                    RaisePropertyChanged(() => CurrentSelectEquipmentMoudle);
                    OnSelectModelChange();
                    IsShowArea = true;
                    if (CurrentSelectEquipmentMoudle.ModuleKey == 1080 || CurrentSelectEquipmentMoudle.ModuleKey == 2096 || CurrentSelectEquipmentMoudle.ModuleKey == 2290)
                        IsShowArea = false;
                }

            }
        }


        private bool _isShowArea;
        /// <summary>
        /// 有些设备不支持 区域分组添加
        /// </summary>
        public bool IsShowArea
        {
            get { return _isShowArea; }
            set
            {
                if (_isShowArea != value)
                {
                    _isShowArea = value;
                    RaisePropertyChanged(() => IsShowArea);
                }
            }
        }



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

        private bool _isTBQS;

        public bool IsTBQS
        {
            get { return _isTBQS; }
            set
            {
                if (_isTBQS != value)
                {
                    _isTBQS = value;
                    RaisePropertyChanged(() => IsTBQS);
                }
            }
        }

        private void OnSelectModelChange()
        {
            if (CurrentSelectEquipmentMoudle == null) return;
            var mouduleKey = CurrentSelectEquipmentMoudle.ModuleKey;
            //  EnumDeviceType tartgettype = EnumDeviceType.Rtu;
            int idstart = 0;
            if (mouduleKey == 3090 || mouduleKey == 3005)
            {
                idstart = 1000000;
            }
            else if (mouduleKey == 1080)
            {
                idstart = 1400000;
            }
            else if (mouduleKey == 2090)
            {
                idstart = 1500000;
            }


            if (IsTBQS == false)
            {
                var max = 0;

                foreach (
                    var t in
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;

                    if (t.Value.RtuPhyId > max) max = t.Value.RtuPhyId;

                }
                this.PhyId = max + 1;
            }
            else
            {
                var index = 1;
                bool flag = false;

                do
                {
                    flag = false;

                    foreach (
                        var t in
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                    {
                        if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;

                        if (t.Value.RtuPhyId == index)
                        {
                            index++;
                            flag = true;
                            break;
                        }
                    }
                    
                } while (flag == true);

                this.PhyId = index;
            }
        }



        public AddMainEquipmentViewModel()
        {
            _canEx = true;
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }

        private Button _btn;
        public void SetButton(Button btn)
        {
            _btn = btn;
        }
        public void NavOnLoad(params object[] parsObjects)
        {
            OnInit();
            if(_btn !=null )
            _btn.Visibility = Visibility.Collapsed;
        }

        private void OnInit()
        {
            EquipmentModules.Clear();
            GetAreaId4W();
            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName[0];
            //var t =EquipmentModelComponentHolding .GetLstMoudule();

            foreach (var f in EquipmentModelComponentHolding.DicEquipmentModels.Values)
            {
                if (!f.CanBeMainEquipment) continue;
                if (f.ModelKey < 1) continue;
                var equipmentItemView = new EquipmentViewItem
                                            {
                                                ModuleKey = f.ModelKey,
                                                ModuleDes = f.ModuleDescription,
                                                //ImageIcon =
                                                //    new BitmapImage(
                                                //    new Uri(@"pack://siteoforigin:,,," + f.Args.ImageSourcePath)),
                                                ModuleInfoSetViewId = f.Args.ModelInfoSetViewId,
                                                ModuleInfoSetViewAttachRegion = f.Args.ModelInfoSetViewAttachRegion,
                                                Name = f.Args.Name
                                            };
                try
                {
                    equipmentItemView.ImageIcon = new BitmapImage(
                        new Uri(@"pack://siteoforigin:,,," + f.Args.ImageSourcePath));
                }
                catch (Exception ex)
                {

                }
                EquipmentModules.Add(equipmentItemView);

            }
            if (EquipmentModules.Count > 0)
            {
                CurrentSelectEquipmentMoudle = EquipmentModules[0];
            }
            _canEx = true;
        }

        #region

        private DateTime _dtAddNew;
        private ICommand _addNew;

        public ICommand AddNew
        {
            get { return _addNew ?? (_addNew = new RelayCommand(ExAddNew, CanEx, true)); }
        }

        private bool _canEx;
        bool CanEx()
        {
            return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D && _canEx && DateTime.Now.Ticks - _dtAddNew.Ticks > 30000000;
        }

        private long _sndguid;
        private void ExAddNew()
        {
            if (CurrentSelectEquipmentMoudle == null) return;
            _dtAddNew = DateTime.Now;
            var mouduleKey = CurrentSelectEquipmentMoudle.ModuleKey;
            if (mouduleKey < 1)
            {
                //WlstMessageBox.Show(I36N.Services.I36N.ConvertByCoding("11040005"),
                //                    I36N.Services.I36N.ConvertByCoding("11040006"),WlstMessageBoxType.Ok);
                //UMessageBox.Show(I36N.Services.I36N.ConvertByCoding("11040005"),
                //                 I36N.Services.I36N.ConvertByCoding("11040006"), UMessageBoxButton.Ok);
                WlstMessageBox.Show("请选择主设备类型，必须一项!", "请选择", WlstMessageBoxType.Ok);
                return;
            }
            if (PhyId < 1)
            {
                //WlstMessageBox.Show(I36N.Services.I36N.ConvertByCoding("11040007"),
                //                    I36N.Services.I36N.ConvertByCoding("11040008"),WlstMessageBoxType.Ok);
                //UMessageBox.Show(I36N.Services.I36N.ConvertByCoding("11040007"),
                //                I36N.Services.I36N.ConvertByCoding("11040008"), UMessageBoxButton.Ok);
                WlstMessageBox.Show("主设备地址不合法，请重新输入!", "错误", WlstMessageBoxType.Ok);
                return;
            }


            WjParaBase cons = null;
            var bolfind = false;



            int idstart = 0;
            if (mouduleKey == 3090 || mouduleKey == 3005 || mouduleKey == 3006)
            {
                idstart = 1000000;
            }
            else if (mouduleKey == 1080)
            {
                idstart = 1400000;
            }
            else if (mouduleKey == 2090)
            {
                idstart = 1500000;
            }

            foreach (
                var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Key > idstart && t.Key < (idstart + 99999))
                {
                    if (t.Value.RtuPhyId == PhyId)
                    {
                        bolfind = true;
                        cons = t.Value;
                        break;
                    }
                }
            }


            if (bolfind)
            {
                try
                {
                    // UMessageBox.Show( I36N.Services.I36N.ConvertByCoding("11040009"),I36N.Services.I36N.ConvertByCoding("11040010", cons.RtuId, cons.RtuName, PhyId),
                    //逻辑地址：" + cons.RtuId + ",名称：" + cons.RtuName + "已经使用该（" + PhyId + "）地址，请重新输入...
                    //  UMessageBoxButton.Ok);

                    WlstMessageBox.Show("地址已存在", "逻辑地址：" + cons.RtuId + ",名称：" + cons.RtuName + "已经使用该（" + PhyId + "）地址，请重新输入...", WlstMessageBoxType.Ok);
                    return;
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("Error:" + ex);
                    return;
                }

            }


            int mode = 0;
            try
            {
                mode = mouduleKey;
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            if (mode == 0)
            {
                //  UMessageBox.Show(I36N.Services.I36N.ConvertByCoding("11040011"),
                //  I36N.Services.I36N.ConvertByCoding("11040012"), UMessageBoxButton.Ok);
                WlstMessageBox.Show("设备型号转换出错......,请重新添加", "未知错误", WlstMessageBoxType.Ok);
                //WlstMessageBox.Show(I36N.Services.I36N.ConvertByCoding("11040011"),
                //                    I36N.Services.I36N.ConvertByCoding("11040012"),WlstMessageBoxType.Ok);
                return;
            }

            var infoss = WlstMessageBox.Show("确认增加", "即将增加设备，是 继续增加，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;


            //此处应该加上  设置成功与否的一个标志  增加失败提示
            _sndguid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.AddMainEquipment(PhyId, mode, AreaId,GrpId);



        }



        #endregion


        #region event

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    var recguid = args.GetParams()[1];
                    var rec = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (rec == null || rec.Count == 0)
                    {
                        return;
                    }
                    if (_sndguid == long.Parse(recguid.ToString()))
                    {
                        if (
                            WlstMessageBox.Show("上海五零盛同信息科技有限公司", "主设备增加成功，是否直接导航到配置界面进行设备参数设置",
                                                WlstMessageBoxType.YesNo) == WlstMessageBoxResults.Yes)
                        {
                            if (rec[0].Item1 > 1000000 && rec[0].Item1 < (1000000 + 99999))
                            {
                                object[] parsObjects = new object[] {rec[0].Item1, "newAdd3005"};

                                RegionManage.ShowViewByIdAttachRegionWithArgu(
                                    CurrentSelectEquipmentMoudle.ModuleInfoSetViewId, parsObjects);
                            }
                            else
                            {
                                RegionManage.ShowViewByIdAttachRegionWithArgu(
                                    CurrentSelectEquipmentMoudle.ModuleInfoSetViewId, rec[0].Item1);
                            }

                        }
                    }
                }
                if (args.EventType == "onuseraddslusgleequipment")
                {
                    if (args.GetParams().Count < 1) return;
                    long recguid = 0;
                    if (Int64.TryParse(args.GetParams()[0].ToString(), out recguid))
                    {
                        if (_sndguid == recguid)
                        {
                            WlstMessageBox.Show("上海五零盛同信息科技有限公司", "单灯设备增加成功!",
                                                WlstMessageBoxType.Ok);

                        }
                    }
                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("AddEquitment error in FundEventHandlers:ex:" + xe);
            }
        }


        //if(data .Op ==4)
             //   {
             //       var ar1 = new PublishEventArgs()
             //       {
             //           EventId = 1000,
             //           EventType = "onuseraddslusgleequipment"
             //       };
             //       ar.AddParams(sdata .Head .Gid );
             //       EventPublish.PublishEvent(ar1);
             //   }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == EventIdAssign.EquipmentUserAddEventId)
                        return true;
                }
                if (args.EventType == "onuseraddslusgleequipment")
                {
                    if (args.EventId == 1000)
                        return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }
        #endregion

        #region Show in Tab
        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                return "增加设备";
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
    }
}
