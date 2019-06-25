using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Nr6005Module.EmergencyEquipment.Services;
using Wlst.Ux.Nr6005Module.ZDataQuery.DailyDataQuery.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.EmergencyEquipment.ViewModel
{
    [Export(typeof (IIEmergencyEquipment))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EmergencyEquipmentViewModel : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
        IIEmergencyEquipment
    {
        public EmergencyEquipmentViewModel()
        {
            InitEvent();
        }

        private bool _isViewActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            Items.Clear();
            _isViewActive = true;
            intRtuType = 0;
            IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
            SelectIndex = 0;
            Load(1);
            Load(9999);

            _currentSelectAllStateTmp = false;
        }

        public void OnUserHideOrClosing()
        {
            _isViewActive = false;
        }

        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "应急设备设置"; }
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

        #region 应急类型

        private int _intRtuType;

        public int intRtuType
        {
            get { return _intRtuType; }
            set
            {
                if (value != _intRtuType)
                {
                    _intRtuType = value;
                    RaisePropertyChanged(() => intRtuType);
                    Load(value + 1);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _rtuType;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RtuType
        {
            get
            {
                if (_rtuType == null)
                {
                    _rtuType = new ObservableCollection<NameValueInt>();
                    _rtuType.Add(new NameValueInt() { Name = "应急一级渍水路段", Value = 0 });
                    _rtuType.Add(new NameValueInt() { Name = "应急二级渍水路段", Value = 1 });
                    _rtuType.Add(new NameValueInt() { Name = "应急三级渍水路段", Value = 2 });
                    _rtuType.Add(new NameValueInt() { Name = "应急四级渍水路段", Value = 3 });
                    _rtuType.Add(new NameValueInt() { Name = "应急五级渍水路段", Value = 4 });
                }
                return _rtuType;
            }
        }

        #endregion

        #region Items

        private ObservableCollection<TreeNodeBase> _items;
        public ObservableCollection<TreeNodeBase> Items
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBase>()); }
            set
            {
                if (value != _items)
                {
                    _items = value;
                    RaisePropertyChanged(() => Items);

                }
            }
        }

        #endregion

        #region SpItems

        private ObservableCollection<TreeNodeBase> _spitems;
        public ObservableCollection<TreeNodeBase> SpItems
        {
            get { return _spitems ?? (_spitems = new ObservableCollection<TreeNodeBase>()); }
            set
            {
                if (value != _spitems)
                {
                    _spitems = value;
                    RaisePropertyChanged(() => SpItems);

                }
            }
        }

        #endregion

        #region IsD


        private bool _cheIsD;

        public bool IsD
        {
            get { return _cheIsD; }
            set
            {
                if (value != _cheIsD)
                {
                    _cheIsD = value;
                    RaisePropertyChanged(() => IsD);
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
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion


        #region RemindSp

        private string _remindSp;

        public string RemindSp
        {
            get { return _remindSp; }
            set
            {
                if (value == _remindSp) return;
                _remindSp = value;
                RaisePropertyChanged(() => RemindSp);
            }
        }

        #endregion

        #region TabSelectIndex

        private int _selectIndex;

        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                if (value != _selectIndex)
                {
                    _selectIndex = value;
                    RaisePropertyChanged(() => SelectIndex);
                }
            }
        }

        #endregion
        
    }

    public partial class EmergencyEquipmentViewModel
    {
        #region CmdSave

        private DateTime _dtSave;

        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                    _cmdSave = new RelayCommand(ExSave, CanSave, true);
                return _cmdSave;
            }
        }

        private void ExSave()
        {
            _dtSave = DateTime.Now;
            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;
            foreach (var f in Items)
            {
                if (f.IsSwitch1Checked == false && f.IsSwitch2Checked == false && f.IsSwitch3Checked == false &&
                    f.IsSwitch4Checked == false && f.IsSwitch5Checked == false && f.IsSwitch6Checked == false &&
                    f.IsSwitch7Checked == false && f.IsSwitch8Checked == false)
                {
                    WlstMessageBox.Show("警告", f.NodeName + "未选中回路，无法保存！", WlstMessageBoxType.Ok);
                    return;
                }
            }
            Save();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
        }

        private bool CanSave()
        {
            return DateTime.Now.Ticks - _dtSave.Ticks > 30000000 && Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
        }
        #endregion

        #region CmdSaveSp

        private DateTime _dtSaveSp;

        private ICommand _cmdSaveSp;

        public ICommand CmdSaveSp
        {
            get
            {
                if (_cmdSaveSp == null)
                    _cmdSaveSp = new RelayCommand(ExSaveSp, CanSaveSp, true);
                return _cmdSaveSp;
            }
        }

        private void ExSaveSp()
        {
            _dtSaveSp = DateTime.Now;
            //var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            //if (infoss != WlstMessageBoxResults.Yes) return;
            foreach (var f in SpItems)
            {
                if (f.IsSwitch1Checked == false && f.IsSwitch2Checked == false && f.IsSwitch3Checked == false &&
                    f.IsSwitch4Checked == false && f.IsSwitch5Checked == false && f.IsSwitch6Checked == false &&
                    f.IsSwitch7Checked == false && f.IsSwitch8Checked == false)
                {
                    WlstMessageBox.Show("警告", f.NodeName + "未选中回路，无法保存！", WlstMessageBoxType.Ok);
                    return;
                }
            }
            SaveSp();
            RemindSp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
        }

        private bool CanSaveSp()
        {
            return DateTime.Now.Ticks - _dtSaveSp.Ticks > 30000000 && Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
        }
        #endregion

        #region CmdDelete

        private DateTime _dtDelete;

        private ICommand _cmdDelete;

        public ICommand CmdDelete
        {
            get
            {
                if (_cmdDelete == null)
                    _cmdDelete = new RelayCommand(ExDelete, CanDelete, true);
                return _cmdDelete;
            }
        }

        private void ExDelete()
        {
            _dtDelete = DateTime.Now;
            var infoss = WlstMessageBox.Show("确认删除", "确认删除，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;
            var items = new ObservableCollection<TreeNodeBase>();
            foreach (var t in Items)
            {
                items.Add(t);
            }
            foreach (var f in items)
            {
                if (f.IsCheck)
                    Items.Remove(f);
            }
        }

        private bool CanDelete()
        {
            return DateTime.Now.Ticks - _dtDelete.Ticks > 30000000 && Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D && Items.Count>0;
        }
        #endregion

        #region CmdDeleteSp

        private DateTime _dtDeleteSp;

        private ICommand _cmdDeleteSp;

        public ICommand CmdDeleteSp
        {
            get
            {
                if (_cmdDeleteSp == null)
                    _cmdDeleteSp = new RelayCommand(ExDeleteSp, CanDeleteSp, true);
                return _cmdDeleteSp;
            }
        }

        private void ExDeleteSp()
        {
            _dtDeleteSp = DateTime.Now;
            var infoss = WlstMessageBox.Show("确认删除", "确认删除，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;
            var items = new ObservableCollection<TreeNodeBase>();
            foreach (var t in SpItems)
            {
                items.Add(t);
            }
            foreach (var f in items)
            {
                if (f.IsCheck)
                    SpItems.Remove(f);
            }
        }

        private bool CanDeleteSp()
        {
            return DateTime.Now.Ticks - _dtDeleteSp.Ticks > 30000000 && Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D && SpItems.Count > 0;
        }
        #endregion

        #region CmdAllCheckBox

        private bool _currentSelectAllState1;
        private ICommand _cmdAllCheckBox;
        public ICommand CmdAllCheckBox
        {
            get { return _cmdAllCheckBox ?? (_cmdAllCheckBox = new RelayCommand(ExCmdAllCheckBox, CanCmdAllCheckBox, true)); }
        }
        private void ExCmdAllCheckBox()
        {
            _currentSelectAllState1 = !_currentSelectAllState1;
            if(SelectIndex == 0)
            {
                foreach (var t in Items) t.IsCheck = _currentSelectAllState1;
            }else if(SelectIndex ==1 )
            {
                foreach (var t in SpItems) t.IsCheck = _currentSelectAllState1;
            }

        }
        private bool CanCmdAllCheckBox()
        {

            return  Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;;
        }
        #endregion

        #region CmdAllSwitch

        private bool _currentSelectAllSwitch;
        private ICommand _cmdAllSwitch;
        public ICommand CmdAllSwitch
        {
            get { return _cmdAllSwitch ?? (_cmdAllSwitch = new RelayCommand(ExCmdAllSwitch, CanCmdAllSwitch, true)); }
        }
        private void ExCmdAllSwitch()
        {
            _currentSelectAllSwitch = !_currentSelectAllSwitch;
            if (SelectIndex ==0)
            {
                foreach (var t in Items)
                {
                    t.IsSwitch1Checked = _currentSelectAllSwitch;
                    t.IsSwitch2Checked = _currentSelectAllSwitch;
                    t.IsSwitch3Checked = _currentSelectAllSwitch;
                    t.IsSwitch4Checked = _currentSelectAllSwitch;
                    t.IsSwitch5Checked = _currentSelectAllSwitch;
                    t.IsSwitch6Checked = _currentSelectAllSwitch;
                    if (t.Is3006)
                    {
                        t.IsSwitch7Checked = _currentSelectAllSwitch;
                        t.IsSwitch8Checked = _currentSelectAllSwitch;
                    }

                }
            }else if(SelectIndex ==1)
            {
                foreach (var t in SpItems)
                {
                    t.IsSwitch1Checked = _currentSelectAllSwitch;
                    t.IsSwitch2Checked = _currentSelectAllSwitch;
                    t.IsSwitch3Checked = _currentSelectAllSwitch;
                    t.IsSwitch4Checked = _currentSelectAllSwitch;
                    t.IsSwitch5Checked = _currentSelectAllSwitch;
                    t.IsSwitch6Checked = _currentSelectAllSwitch;
                    if (t.Is3006)
                    {
                        t.IsSwitch7Checked = _currentSelectAllSwitch;
                        t.IsSwitch8Checked = _currentSelectAllSwitch;
                    }

                }
            
            
            }

        }
        private bool CanCmdAllSwitch()
        {
            return  Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;;
        }
        #endregion

        private long dtSave = 0;
        private void Save()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_eme_class_info;
            info.WstSpeEmeClassInfo.Op = 2;
            info.WstSpeEmeClassInfo.Item = new List<RtuEmeClassInfo.RtuEmeClassInfoItem>();
            info.WstSpeEmeClassInfo.Item.Add(new RtuEmeClassInfo.RtuEmeClassInfoItem()
                                                 {
                                                     EmeId = intRtuType+1,
                                                     RtuItems = new List<RtuEmeClassInfo.RtuEmRtuItem>()
                                                 });

            foreach (var f in Items)
            {
                var switchitem = new List<int>();
                if (f.IsSwitch1Checked) switchitem.Add(1);
                if (f.IsSwitch2Checked) switchitem.Add(2);
                if (f.IsSwitch3Checked) switchitem.Add(3);
                if (f.IsSwitch4Checked) switchitem.Add(4);
                if (f.IsSwitch5Checked) switchitem.Add(5);
                if (f.IsSwitch6Checked) switchitem.Add(6);
                if (f.IsSwitch7Checked) switchitem.Add(7);
                if (f.IsSwitch8Checked) switchitem.Add(8);
                var item = new RtuEmeClassInfo.RtuEmRtuItem()
                               {
                                   RtuId = f.RtuId,
                                   RtuSwitchOuts = switchitem
                               };
               // if (switchitem.Count > 0)
                    info.WstSpeEmeClassInfo.Item[0].RtuItems.Add(item);
            }
            SndOrderServer.OrderSnd(info);
            dtSave = DateTime.Now.Ticks ;

        }

        private long dtSaveSp = 0;
        private void SaveSp()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_eme_class_info;
            info.WstSpeEmeClassInfo.Op = 2;
            info.WstSpeEmeClassInfo.Item = new List<RtuEmeClassInfo.RtuEmeClassInfoItem>();
            info.WstSpeEmeClassInfo.Item.Add(new RtuEmeClassInfo.RtuEmeClassInfoItem()
            {
                EmeId =9999,
                RtuItems = new List<RtuEmeClassInfo.RtuEmRtuItem>()
            });

            foreach (var f in SpItems)
            {
                var switchitem = new List<int>();
                if (f.IsSwitch1Checked) switchitem.Add(1);
                if (f.IsSwitch2Checked) switchitem.Add(2);
                if (f.IsSwitch3Checked) switchitem.Add(3);
                if (f.IsSwitch4Checked) switchitem.Add(4);
                if (f.IsSwitch5Checked) switchitem.Add(5);
                if (f.IsSwitch6Checked) switchitem.Add(6);
                if (f.IsSwitch7Checked) switchitem.Add(7);
                if (f.IsSwitch8Checked) switchitem.Add(8);
                var item = new RtuEmeClassInfo.RtuEmRtuItem()
                {
                    RtuId = f.RtuId,
                    RtuSwitchOuts = switchitem
                };
                // if (switchitem.Count > 0)
                info.WstSpeEmeClassInfo.Item[0].RtuItems.Add(item);
            }
            SndOrderServer.OrderSnd(info);
            dtSaveSp = DateTime.Now.Ticks;

        }


        private bool _currentSelectAllStateTmp = false;
        public void SelectAllSwitchOut(int kx)
        {
            _currentSelectAllStateTmp = !_currentSelectAllStateTmp;
            switch (kx)
            {
                case 1:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch1Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 2:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch2Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 3:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch3Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 4:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch4Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 5:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch5Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 6:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch6Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 7:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch7Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 8:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch8Checked = _currentSelectAllStateTmp;
                    }
                    break;

            }


        }

    }

    public partial class EmergencyEquipmentViewModel
    {





        public void Load(int emeid)
        {
            if (emeid < 9999)
            {
                var data = Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.GetEmeInfo(emeid);
                Items.Clear();



                foreach (var f in data)
                {
                    Items.Add(new TreeNodeBase()
                                  {
                                      RtuId = f.Key,
                                      IsSwitch1Checked = f.Value.Contains(1),
                                      IsSwitch2Checked = f.Value.Contains(2),
                                      IsSwitch3Checked = f.Value.Contains(3),
                                      IsSwitch4Checked = f.Value.Contains(4),
                                      IsSwitch5Checked = f.Value.Contains(5),
                                      IsSwitch6Checked = f.Value.Contains(6),
                                      IsSwitch7Checked = f.Value.Contains(7),
                                      IsSwitch8Checked = f.Value.Contains(8),
                                      Level = emeid
                                  });
                }
            }else if(emeid ==9999)
            {
                var data = Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.GetEmeInfo(emeid);
                SpItems.Clear();



                foreach (var f in data)
                {
                    SpItems.Add(new TreeNodeBase()
                    {
                        RtuId = f.Key,
                        IsSwitch1Checked = f.Value.Contains(1),
                        IsSwitch2Checked = f.Value.Contains(2),
                        IsSwitch3Checked = f.Value.Contains(3),
                        IsSwitch4Checked = f.Value.Contains(4),
                        IsSwitch5Checked = f.Value.Contains(5),
                        IsSwitch6Checked = f.Value.Contains(6),
                        IsSwitch7Checked = f.Value.Contains(7),
                        IsSwitch8Checked = f.Value.Contains(8),
                        Level = emeid
                    });
                }
            }

        }


        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core,true);
            this.AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.AreaEmedataUpdate,
                                    PublishEventType.Core);
        }



        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            if (_isViewActive == false) return;
            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D == false) return;
                    int id = Convert.ToInt32(args.GetParams()[0]);
                    if (id > 1100000) return;


                    if (SelectIndex == 0) //应急规划
                    {
                        var ntg = (from t in Items where t.RtuId == id select t).Count() > 0;
                        if (ntg) return;


                        var tml =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        if (tml == null) return;
                        //if (tml.RtuStateCode != 2) return;
                        bool is3006 = tml.RtuModel == EnumRtuModel.Wj3006;
                        var name = tml.RtuPhyId + "-" + tml.RtuName;

                        foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.AreaInfo)
                        {
                            //f key ==1   intrtutype==1
                            if ((f.Key - 1) == intRtuType) continue;
                            if (f.Value.ContainsKey(id))
                            {
                                WlstMessageBox.Show("警告", "当前终端已绑定 " + RtuType[f.Key - 1].Name + "，请先解除绑定！",
                                                    WlstMessageBoxType.Ok);
                                return;
                            }
                        }

                        var info = WlstMessageBox.Show("确认添加",
                                                       "确认添加 " + name + " 到 " + RtuType[intRtuType].Name + " 中？",
                                                       WlstMessageBoxType.YesNo);
                        if (info == WlstMessageBoxResults.Yes)
                            SelectRtuIdChange(id, is3006);
                    }
                    else if (SelectIndex == 1) // 特殊终端
                    {
                        var ntg = (from t in SpItems where t.RtuId == id select t).Count() > 0;
                        if (ntg) return;


                        var tml =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        if (tml == null) return;
                        //if (tml.RtuStateCode != 2) return;
                        bool is3006 = tml.RtuModel == EnumRtuModel.Wj3006;
                        var name = tml.RtuPhyId + "-" + tml.RtuName;

                        foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.AreaInfo)
                        {
                            if (f.Value.ContainsKey(id))
                            {
                                WlstMessageBox.Show("警告", "当前终端已绑定 " + RtuType[f.Key - 1].Name + "，请先解除绑定！",
                                                    WlstMessageBoxType.Ok);
                                return;
                            }
                        }

                        var info = WlstMessageBox.Show("确认添加",
                                                       "确认添加 " + name + " 到 特殊终端 中？",
                                                       WlstMessageBoxType.YesNo);
                        if (info == WlstMessageBoxResults.Yes)
                            SelectSpRtuIdChange(id, is3006);

                    }



                }
                if(args .EventId == Wlst .Sr .EquipmentInfoHolding .Services .EventIdAssign .AreaEmedataUpdate )
                {
                    int op = Convert.ToInt32(args.GetParams()[0]);
                    if(op ==2)
                    {
                        //if(DateTime .Now .Ticks -  dtSave < 100000000)
                        //{
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 保存成功";
                            RemindSp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 保存成功";
                        //}
                    }
                }
            }
            catch (Exception)
            {
 
            }
        }

        /// <summary>
        /// 选中终端变化  提取数据
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="is3006"> </param>
        private void SelectRtuIdChange(int rtuId, bool is3006)
        {
            if (rtuId < 1) return;
            int i = Items.Count(t => rtuId == t.RtuId);
            if (i < Items.Count || Items.Count < 1)
            {
                var item = new TreeNodeBase()
                               {
                                   RtuId = rtuId,
                                   IsSwitch1Checked = false,
                                   IsSwitch2Checked = false,
                                   IsSwitch3Checked = false,
                                   IsSwitch4Checked = false,
                                   IsSwitch5Checked = false,
                                   IsSwitch6Checked = false,
                                   IsSwitch7Checked = false,
                                   IsSwitch8Checked = false,
                                   Level = intRtuType,
                                   Is3006 = is3006
                               };
                Items.Add(item);
            }
        }

        /// <summary>
        /// 选中终端变化  提取数据
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="is3006"> </param>
        private void SelectSpRtuIdChange(int rtuId, bool is3006)
        {
            if (rtuId < 1) return;
            int i = SpItems.Count(t => rtuId == t.RtuId);
            if (i < SpItems.Count || SpItems.Count < 1)
            {
                var item = new TreeNodeBase()
                {
                    RtuId = rtuId,
                    IsSwitch1Checked = false,
                    IsSwitch2Checked = false,
                    IsSwitch3Checked = false,
                    IsSwitch4Checked = false,
                    IsSwitch5Checked = false,
                    IsSwitch6Checked = false,
                    IsSwitch7Checked = false,
                    IsSwitch8Checked = false,
                    Level = intRtuType,
                    Is3006 = is3006,
                    
                };
                SpItems.Add(item);
            }
        }
    }

    public class TreeNodeBase: Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
    {
        #region PhysicalId

        private int _physicalId;
        /// <summary>
        /// 物理地址
        /// </summary>
        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if (value == _physicalId) return;
                _physicalId = value;
                RaisePropertyChanged(() => PhysicalId);
            }
        }
        #endregion

        #region RtuId

        private int _rtuId;
        /// <summary>
        /// 逻辑地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value == _rtuId) return;
                _rtuId = value;
                RaisePropertyChanged(() => RtuId);
                PhysicalId = value;
                IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
                if (
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                         InfoItems.ContainsKey
                         (_rtuId))
                    return;
                var tml =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                        [_rtuId];
                this.NodeName = tml.RtuName;
                PhysicalId = tml.RtuPhyId;
            }
        }
        #endregion

        #region NodeName
        private string _nodeName;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                if (_nodeName != value)
                {
                    _nodeName = value;
                    this.RaisePropertyChanged(() => this.NodeName);
                }
            }
        }
        #endregion

        #region Remark
        private string _remark;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    this.RaisePropertyChanged(() => this.Remark);
                }
            }
        }
        #endregion

        #region Is3006
        private bool _is3006;

        public bool Is3006
        {
            get { return _is3006; }
            set
            {
                _is3006 = value;
                if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D == false) _is3006 = false;
                this.RaisePropertyChanged(() => this.Is3006);
            }
        }
        #endregion

        #region IsSwitch1Checked

        private bool _isSwitch1Checked;
        public bool IsSwitch1Checked
        {
            get { return _isSwitch1Checked; }
            set
            {
                if (_isSwitch1Checked == value) return;
                _isSwitch1Checked = value;
                RaisePropertyChanged(() => IsSwitch1Checked);
            }
        }
        #endregion

        #region IsSwitch2Checked

        private bool _isSwitch2Checked;
        public bool IsSwitch2Checked
        {
            get { return _isSwitch2Checked; }
            set
            {
                if (_isSwitch2Checked == value) return;
                _isSwitch2Checked = value;
                RaisePropertyChanged(() => IsSwitch2Checked);
            }
        }
        #endregion

        #region IsSwitch3Checked

        private bool _isSwitch3Checked;
        public bool IsSwitch3Checked
        {
            get { return _isSwitch3Checked; }
            set
            {
                if (_isSwitch3Checked == value) return;
                _isSwitch3Checked = value;
                RaisePropertyChanged(() => IsSwitch3Checked);
            }
        }
        #endregion

        #region IsSwitch4Checked

        private bool _isSwitch4Checked;
        public bool IsSwitch4Checked
        {
            get { return _isSwitch4Checked; }
            set
            {
                if (_isSwitch4Checked == value) return;
                _isSwitch4Checked = value;
                RaisePropertyChanged(() => IsSwitch4Checked);
            }
        }
        #endregion

        #region IsSwitch5Checked

        private bool _isSwitch5Checked;
        public bool IsSwitch5Checked
        {
            get { return _isSwitch5Checked; }
            set
            {
                if (_isSwitch5Checked == value) return;
                _isSwitch5Checked = value;
                RaisePropertyChanged(() => IsSwitch5Checked);
            }
        }
        #endregion

        #region IsSwitch6Checked

        private bool _isSwitch6Checked;
        public bool IsSwitch6Checked
        {
            get { return _isSwitch6Checked; }
            set
            {
                if (_isSwitch6Checked == value) return;
                _isSwitch6Checked = value;
                RaisePropertyChanged(() => IsSwitch6Checked);
            }
        }
        #endregion

        #region IsSwitch7Checked

        private bool _isSwitch7Checked;
        public bool IsSwitch7Checked
        {
            get { return _isSwitch7Checked; }
            set
            {
                if (_isSwitch7Checked == value) return;              
                    _isSwitch7Checked = value;
                RaisePropertyChanged(() => IsSwitch7Checked);
            }
        }
        #endregion

        #region IsSwitch8Checked

        private bool _isSwitch8Checked;
        public bool IsSwitch8Checked
        {
            get { return _isSwitch8Checked; }
            set
            {
                if (_isSwitch8Checked == value) return;
                    _isSwitch8Checked = value;              
                RaisePropertyChanged(() => IsSwitch8Checked);
            }
        }
        #endregion

        #region Level

        private int _level;
        /// <summary>
        /// 应急级别
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                if (value == _level) return;
                _level = value;
                RaisePropertyChanged(() => Level);
            }
        }
        #endregion

        #region IsCheck
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                if (_isCheck == value) return;
                _isCheck = value;
                RaisePropertyChanged(() => IsCheck);
            }
        }
        #endregion

        #region IsD
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _isD;
        public bool IsD
        {
            get { return _isD; }
            set
            {
                if (_isD == value) return;
                _isD = value;
                RaisePropertyChanged(() => IsD);
            }
        }
        #endregion
    }
}
