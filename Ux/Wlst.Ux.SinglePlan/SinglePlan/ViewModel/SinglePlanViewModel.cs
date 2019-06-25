using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.SinglePlan.SinglePlan.Services;
using Wlst.Ux.SinglePlan.SinglePlan.View;
using Wlst.iifx;
using Wlst.Ux.SinglePlan.Services;
using Wlst.Cr.Coreb.EventHelper;

namespace Wlst.Ux.SinglePlan.SinglePlan.ViewModel
{
    [Export(typeof(IISinglePlan))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SinglePlanViewModel : EventHandlerHelperExtendNotifyProperyChanged, IISinglePlan
    {
        public SinglePlanViewModel()
        {
            this.AddEventFilterInfo(EventIdAssign.SaveInstructionId, PublishEventType.Core, true);
            this.AddEventFilterInfo(EventIdAssign.SavePlanId, PublishEventType.Core, true);
        }
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventId == EventIdAssign.SaveInstructionId)
            {
                
                RequestSingleInstruction(AreaId);
            }
            if (args.EventId == EventIdAssign.SavePlanId)
            {
                RequestSinglePlan(AreaId);
            }
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            AreaName.Clear();

            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                    AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }

            if (AreaName.Count > 0)
                AreaComboBoxSelected = AreaName[0];
            if (AreaName.Count > 1)
            {
                Visi = Visibility.Visible;
            }
            else
            {
                Visi = Visibility.Collapsed;
            }

        }
        public void OnUserHideOrClosing()
        {
        }
        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "新单灯方案管理"; }
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

    /// <summary>
    /// ICommand
    /// </summary>
    public partial class SinglePlanViewModel
    {
        /// <summary>
        /// 增加方案
        /// </summary>
        #region CmdAddPlan

        public static AddOrModifyPlan _addOrModifyPlan = null;
        private DateTime _dtCmdAddPlan;
        private ICommand _cmdAddPlan;

        public ICommand CmdAddPlan
        {
            get
            {
                return _cmdAddPlan ??
                       (_cmdAddPlan = new RelayCommand(ExCmdAddPlan, CanCmdAddPlan, true));
            }

        }

        private bool CanCmdAddPlan()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddPlan.Ticks > 10000000;
        }

        private void ExCmdAddPlan()
        {
            //var id = 0;
            //foreach (var t in TunnelItems)
            //{
            //    if (t.SchemeId >= id)
            //    {
            //        id = t.SchemeId;
            //    }
            //}
            _dtCmdAddPlan = DateTime.Now;
            _addOrModifyPlan = new AddOrModifyPlan();
            var tvx = new SluOnePlan(AreaId);
            //_addOrModifyPlan.OnFormBtnOkClick +=
            //        new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            _addOrModifyPlan.SetContext(tvx);
            _addOrModifyPlan.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 修改方案
        /// </summary>
        #region CmdModifyPlan

        private DateTime _dtCmdModifyPlan;
        private ICommand _cmdModifyPlan;

        public ICommand CmdModifyPlan
        {
            get
            {
                return _cmdModifyPlan ??
                       (_cmdModifyPlan = new RelayCommand(ExCmdModifyPlan, CanCmdModifyPlan, true));
            }

        }

        private bool CanCmdModifyPlan()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdModifyPlan.Ticks > 10000000;
        }

        private void ExCmdModifyPlan()
        {
            //var id = 0;
            //foreach (var t in TunnelItems)
            //{
            //    if (t.SchemeId >= id)
            //    {
            //        id = t.SchemeId;
            //    }
            //}
            _dtCmdModifyPlan = DateTime.Now;
            _addOrModifyPlan = new AddOrModifyPlan();
            var req = RequestDetailInfoPlan(AreaId, CurrentSelectPlan.PlanId);
            var tvx = new SluOnePlan(req);
            //_addOrModifyPlan.OnFormBtnOkClick +=
            //        new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            _addOrModifyPlan.SetContext(tvx);
            _addOrModifyPlan.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 删除方案
        /// </summary>
        #region CmdDeletePlan

        private DateTime _dtCmdDeletePlan;
        private ICommand _cmdDeletePlan;

        public ICommand CmdDeletePlan
        {
            get
            {
                return _cmdDeletePlan ??
                       (_cmdDeletePlan = new RelayCommand(ExCmdDeletePlan, CanCmdDeletePlan, true));
            }

        }

        private bool CanCmdDeletePlan()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdDeletePlan.Ticks > 10000000;
        }

        private void ExCmdDeletePlan()
        {
            _dtCmdDeletePlan = DateTime.Now;
            var data = WlstMessageBox.Show("提示", "确认删除 " + CurrentSelectPlan.PlanName, WlstMessageBoxType.YesNo);
            if (data == WlstMessageBoxResults.No) return;
            DeletePlan(AreaId, CurrentSelectPlan.PlanId);
        }

        #endregion

        /// <summary>
        /// 导出方案
        /// </summary>
        #region CmdExportPlan
        private DateTime _dtCmdExportPlan;
        private ICommand _cmdCmdExportPlan;

        public ICommand CmdExportPlan
        {
            get
            {
                if (_cmdCmdExportPlan == null)
                    _cmdCmdExportPlan = new RelayCommand(ExCmdExportPlan, CanExCmdExportPlan, false);
                return _cmdCmdExportPlan;
            }
        }

        private void ExCmdExportPlan()
        {
            _dtCmdExportPlan = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("单灯方案名称");
                lsttitle.Add("方案生成时间");
                lsttitle.Add("描述");
                lsttitle.Add("状态");

                var lstobj = new List<List<object>>();

                foreach (var g in SluPlans)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.PlanId);
                    tmp.Add(g.PlanName);
                    tmp.Add(g.PlanTime);
                    tmp.Add(g.PlanDesc);
                    tmp.Add(g.State);

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

        private bool CanExCmdExportPlan()
        {
            if (SluPlans.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExportPlan.Ticks > 30000000;
            return false;
        }

        #endregion

        /// <summary>
        /// 增加指令
        /// </summary>
        #region CmdAddInstruction

        public static AddOrModifyInstruction _addOrModifyInstruction = null;
        private DateTime _dtCmdAddInstruction;
        private ICommand _cmdAddInstruction;

        public ICommand CmdAddInstruction
        {
            get
            {
                return _cmdAddInstruction ??
                       (_cmdAddInstruction = new RelayCommand(ExCmdAddInstruction, CanCmdAddInstruction, true));
            }

        }

        private bool CanCmdAddInstruction()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddInstruction.Ticks > 10000000;
        }

        private void ExCmdAddInstruction()
        {
            //var id = 0;
            //foreach (var t in TunnelItems)
            //{
            //    if (t.SchemeId >= id)
            //    {
            //        id = t.SchemeId;
            //    }
            //}
            _dtCmdAddInstruction = DateTime.Now;
            _addOrModifyInstruction = new AddOrModifyInstruction();
            var tvx = new SluOneInstruction(AreaId);
            _addOrModifyInstruction.SetContext(tvx);
            _addOrModifyInstruction.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 修改指令
        /// </summary>
        #region CmdModifyInstruction

        private DateTime _dtCmdModifyInstruction;
        private ICommand _cmdModifyInstruction;

        public ICommand CmdModifyInstruction
        {
            get
            {
                return _cmdModifyInstruction ??
                       (_cmdModifyInstruction = new RelayCommand(ExCmdModifyInstruction, CanCmdModifyInstruction, true));
            }

        }

        private bool CanCmdModifyInstruction()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdModifyInstruction.Ticks > 10000000;
        }

        private void ExCmdModifyInstruction()
        {
            //var id = 0;
            //foreach (var t in TunnelItems)
            //{
            //    if (t.SchemeId >= id)
            //    {
            //        id = t.SchemeId;
            //    }
            //}
            _dtCmdModifyInstruction = DateTime.Now;
            _addOrModifyInstruction = new AddOrModifyInstruction();
            var req=RequestDetailInfoInstruction(AreaId, CurrentSelectInstruction.InstructionId);
            var tvx = new SluOneInstruction(req);
            //tvx.SubOperationCount = tvx.OperationItems.Count;
            //tvx.IsLuxOrTime = 1;
            //tvx.IsSelectlightEnable = true;
            //tvx.IsSelectTimeEnable = false;
            //tvx.ControlMode = "光控";
            //_addOrModifyPlan.OnFormBtnOkClick +=
            //        new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            _addOrModifyInstruction.SetContext(tvx);
            _addOrModifyInstruction.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 删除指令
        /// </summary>
        #region CmdDeletePlan

        private DateTime _dtCmdDeleteInstruction;
        private ICommand _cmdDeleteInstruction;

        public ICommand CmdDeleteInstruction
        {
            get
            {
                return _cmdDeleteInstruction ??
                       (_cmdDeleteInstruction = new RelayCommand(ExCmdDeleteInstruction, CanCmdDeleteInstruction, true));
            }

        }

        private bool CanCmdDeleteInstruction()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdDeleteInstruction.Ticks > 10000000;
        }

        private void ExCmdDeleteInstruction()
        {
            _dtCmdDeleteInstruction = DateTime.Now;
            var data = WlstMessageBox.Show("提示", "确认删除 " + CurrentSelectInstruction.InstructionName, WlstMessageBoxType.YesNo);
            if (data == WlstMessageBoxResults.No) return;
            DeleteInstruction(AreaId, CurrentSelectInstruction.InstructionId);
        }

        #endregion

        /// <summary>
        /// 导出指令
        /// </summary>
        #region CmdExportInstruction
        private DateTime _dtCmdExportInstruction;
        private ICommand _cmdCmdExportInstruction;

        public ICommand CmdExportInstruction
        {
            get
            {
                if (_cmdCmdExportInstruction == null)
                    _cmdCmdExportInstruction = new RelayCommand(ExCmdExportInstruction, CanExCmdExportInstruction, false);
                return _cmdCmdExportInstruction;
            }
        }

        private void ExCmdExportInstruction()
        {
            _dtCmdExportInstruction = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("单灯指令名称");
                lsttitle.Add("指令生成时间");
                lsttitle.Add("描述");

                var lstobj = new List<List<object>>();

                foreach (var g in SluInstructions)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.InstructionId);
                    tmp.Add(g.InstructionName);
                    tmp.Add(g.InstructionTime);
                    tmp.Add(g.InstructionDesc);

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

        private bool CanExCmdExportInstruction()
        {
            if (SluInstructions.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExportInstruction.Ticks > 30000000;
            return false;
        }

        #endregion

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
                    RequestSinglePlan(AreaId);
                    RequestSingleInstruction(AreaId);
                    RequestSingleGroup(AreaId);
                }
            }
        }

        private ObservableCollection<SluOnePlan> _sluPlans;
        /// <summary>
        /// 单灯方案集合
        /// </summary>
        public ObservableCollection<SluOnePlan> SluPlans
        {
            get
            {
                if (_sluPlans == null) _sluPlans = new ObservableCollection<SluOnePlan>();
                return _sluPlans;
            }
        }

        private SluOnePlan _currentSelectPlan;
        /// <summary>
        /// 选中单灯方案
        /// </summary>
        public SluOnePlan CurrentSelectPlan
        {
            get { return _currentSelectPlan; }
            set
            {
                if (value != _currentSelectPlan)
                {
                    _currentSelectPlan = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectPlan);
                }

            }
        }

        private ObservableCollection<SluOneInstruction> _sluInstructions;
        /// <summary>
        /// 单灯指令集合
        /// </summary>
        public ObservableCollection<SluOneInstruction> SluInstructions
        {
            get
            {
                if (_sluInstructions == null) _sluInstructions = new ObservableCollection<SluOneInstruction>();
                return _sluInstructions;
            }
        }

        private SluOneInstruction _currentSelectInstruction;
        /// <summary>
        /// 选中单灯指令
        /// </summary>
        public SluOneInstruction CurrentSelectInstruction
        {
            get { return _currentSelectInstruction; }
            set
            {
                if (value != _currentSelectInstruction)
                {
                    _currentSelectInstruction = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectInstruction);
                }

            }
        }
    }

    public partial class SinglePlanViewModel
    {
        //private SluPlanBandingInfo.SluPlanBandingBriefInfo RequestSinglePlan(int areaId)
        //{
        //    var req =new SluPlanBandingInfo();
        //    req.AreaId = areaId;
        //    var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1225", System.Convert.ToBase64String(SluPlanBandingInfo.SerializeToBytes(req)));
        //    var res = SluPlanBandingInfo.SluPlanBandingBriefInfo.Deserialize(data);
        //    return res;
        //}

        /// <summary>
        /// 请求区域下单灯方案
        /// </summary>
        /// <param name="areaId"></param>
        private void RequestSinglePlan(int areaId)
        {
            //var req = new MsgWithIif()
            //              {
            //                  Get1225 = new InfoRq()
            //              };
            //req.Get1225.AreaId = areaId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1225", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if(data==null) return;
            //var res = MsgWithIif.Deserialize(data);
            //SluPlans.Clear();
            //foreach (var item in res.Back1225.Items)
            //{
            //    SluPlans.Add(new SluOnePlan(item));
            //}
            //CurrentSelectPlan = SluPlans[0];

            var req = new InfoRq();
            req.AreaId = areaId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1225", System.Convert.ToBase64String(InfoRq.SerializeToBytes(req)));
            if (data == null) return;
            var res = SluPlanBandingInfo.Deserialize(data);
            SluPlans.Clear();
            foreach (var item in res.Items)
            {
                SluPlans.Add(new SluOnePlan(item));
            }
            if (SluPlans.Count != 0)
                CurrentSelectPlan = SluPlans[0];
        }

        /// <summary>
        /// 请求区域下单灯指令
        /// </summary>
        /// <param name="areaId"></param>
        private void RequestSingleInstruction(int areaId)
        {
            //var req = new MsgWithIif()
            //{
            //    Get1223 = new InfoRq()
            //};
            //req.Get1223.AreaId = areaId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1223", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if (data == null) return;
            //var res = MsgWithIif.Deserialize(data);
            //SluInstructions.Clear();
            //foreach (var item in res.Back1223.Items)
            //{
            //    SluInstructions.Add(new SluOneInstruction(item));
            //}
            //CurrentSelectInstruction = SluInstructions[0];
            var req = new InfoRq();
            req.AreaId = areaId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1223", System.Convert.ToBase64String(InfoRq.SerializeToBytes(req)));
            if (data == null) return;
            var res = SluPlanBriefInfo.Deserialize(data);
            SluInstructions.Clear();
            foreach (var item in res.Items)
            {
                SluInstructions.Add(new SluOneInstruction(item));
            }
            if (SluInstructions.Count != 0)
                CurrentSelectInstruction = SluInstructions[0];
        }

        /// <summary>
        /// 请求区域下集中器分组
        /// </summary>
        /// <param name="areaId"></param>
        private void RequestSingleGroup(int areaId)
        {
            //var req = new MsgWithIif()
            //{
            //    Get1221 = new InfoRq()
            //};
            //req.Get1221.AreaId = areaId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1221", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if (data == null) return;
            //var res = MsgWithIif.Deserialize(data);
            var req = new InfoRq();
            req.AreaId = areaId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1221", System.Convert.ToBase64String(InfoRq.SerializeToBytes(req)));
            if (data == null) return;
            var res = SluPlanGrpInfoBk.Deserialize(data);
        }

        /// <summary>
        /// 请求选中指令详细信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="instructionId"></param>
        /// <returns></returns>
        private SluPlanDetailInfo RequestDetailInfoInstruction(int areaId, int instructionId)
        {
            //var req = new MsgWithIif()
            //              {
            //                  Get1224 = new SluPlanDetailInfoRq()
            //              };
            //req.Get1224.AreaId = areaId;
            //req.Get1224.PlanId = instructionId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1224", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if (data == null) return null;
            //var res = MsgWithIif.Deserialize(data);
            //return res.Back1224;
            var req = new SluPlanDetailInfoRq();
            req.AreaId = areaId;
            req.PlanId = instructionId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1224", System.Convert.ToBase64String(SluPlanDetailInfoRq.SerializeToBytes(req)));
            if (data == null) return null;
            var res = SluPlanDetailInfo.Deserialize(data);
            return res;
        }

        /// <summary>
        /// 请求选中方案详细信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        private SluPlanBandingDetailInfo RequestDetailInfoPlan(int areaId, int planId)
        {
            //var req = new MsgWithIif()
            //{
            //   Get1226 = new SluPlanDetailInfoRq()
            //};
            //req.Get1226.AreaId = areaId;
            //req.Get1226.PlanId = planId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1226", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if (data == null) return null;
            //var res = MsgWithIif.Deserialize(data);
            //return res.Back1226;
            var req = new SluPlanDetailInfoRq();
            req.AreaId = areaId;
            req.PlanId = planId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1226", System.Convert.ToBase64String(SluPlanDetailInfoRq.SerializeToBytes(req)));
            if (data == null) return null;
            var res = SluPlanBandingDetailInfo.Deserialize(data);
            return res;
        }

        /// <summary>
        /// 删除选中方案
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="planId"></param>
        private void DeletePlan(int areaId, int planId)
        {
            //var req = new MsgWithIif()
            //              {
            //                  Post4076 = new SluPlanDetailInfoRq()
            //              };
            //req.Post4076.AreaId = areaId;
            //req.Post4076.PlanId = planId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4076", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if (data == null) return ;
            //var res = MsgWithIif.Deserialize(data);
            //RequestSinglePlan(areaId);
            var req = new SluPlanDetailInfoRq();
            req.AreaId = areaId;
            req.PlanId = planId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4076", System.Convert.ToBase64String(SluPlanDetailInfoRq.SerializeToBytes(req)));
            if (data == null) return;
            var res = CommAns.Deserialize(data);
            if(res.Head.IfSt!=1)
            {
                WlstMessageBox.Show("提示", "删除失败", WlstMessageBoxType.Ok);
                return;
            }
            WlstMessageBox.Show("提示", "删除成功", WlstMessageBoxType.Ok);
            RequestSinglePlan(areaId);
        }

        /// <summary>
        /// 删除选中指令
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="instructionId"></param>
        private void DeleteInstruction(int areaId, int instructionId)
        {
            //var req = new MsgWithIif()
            //{
            //    Post4074 = new SluPlanDetailInfoRq()
            //};
            //req.Post4074.AreaId = areaId;
            //req.Post4074.PlanId = instructionId;
            //var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4074", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            //if (data == null) return;
            //var res = MsgWithIif.Deserialize(data);
            //RequestSingleInstruction(areaId);
            var req = new SluPlanDetailInfoRq();
            req.AreaId = areaId;
            req.PlanId = instructionId;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4074", System.Convert.ToBase64String(SluPlanDetailInfoRq.SerializeToBytes(req)));
            if (data == null) return;
            var res = CommAns.Deserialize(data);
            if (res.Head.IfSt != 1)
            {
                WlstMessageBox.Show("提示", "删除失败", WlstMessageBoxType.Ok);
                return;
            }
            WlstMessageBox.Show("提示", "删除成功", WlstMessageBoxType.Ok);
            RequestSingleInstruction(areaId);
        }
    }


}
