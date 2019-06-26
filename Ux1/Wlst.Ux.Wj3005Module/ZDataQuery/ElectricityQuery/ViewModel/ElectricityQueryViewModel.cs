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
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.WJ3005Module.ZDataQuery.DailyDataQuery.ViewModel;
using Wlst.Ux.WJ3005Module.ZDataQuery.ElectricityQuery.Services;
using Wlst.client;


namespace Wlst.Ux.WJ3005Module.ZDataQuery.ElectricityQuery.ViewModel
{
    [Export(typeof (IIElectricityQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public  partial class ElectricityQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIElectricityQueryViewModel
    {
        private bool _thisViewActive = false;

        public ElectricityQueryViewModel()
        {

            this.InitEvent();
            this.InitAction();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //lvf 获取区域信息  并将区域终端存于rtusbelongArea list中   2018年4月9日15:25:14
            getAreaRId();
            _thisViewActive = true;
            DtEndTimeTime = DateTime.Now;
            DtStartTimeTime = DateTime.Now.AddDays(-1);
            QueryMode = 3;
            if (RtuId == 0)
            {
                PhyId = 0;
                RtuName = "无";
            }

        }


        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            //RtuId = 0;
            Records.Clear();
            recordsKeys.Clear();
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
                return "电能查询"; //I36N .Services.I36N .ConvertByCodingOne("11090001", "Setting");
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
            buttonClick = 0;
            Records.Clear();
            recordsKeys.Clear();
            //var rtulst = GetRtusLst();
            //var index = 1;
            if (RtuId == 0 && QueryMode==2)
            {
                UMessageBox.Show("提醒", "未选择终端！", UMessageBoxButton.Ok);
                return;
            }
            Query();
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  查询成功，共计" + Records.Count + " 条数据.";
        }

        private void Query()
        {
            var tStartTime = new DateTime(DtStartTimeTime.Year, DtStartTimeTime.Month, DtStartTimeTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEndTimeTime.Year, DtEndTimeTime.Month, DtEndTimeTime.Day, 23, 59, 59);
            var rtulst = GetRtusLst();
            if (rtulst.Count  ==0)
            {
                UMessageBox.Show("提醒", "未符合标准的终端！", UMessageBoxButton.Ok);
                return;
            }
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_elec_data;
            // .wlst_cnt_wj3090_request_open_close_light_record ;//.ServerPart.wlst_OpenCloseLight_clinet_request_rtuopencloseLightrecord;
            info.WstRtuElecData.DtEnd = tEndTime.Ticks;
            info.WstRtuElecData.DtStart = tStartTime.Ticks;
            info.WstRtuElecData.RtuItems = rtulst;
            info.WstRtuElecData.OP = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
        }

        private List<int> GetRtusLst()
        {
            var rtulst = new List<int>();
            if (QueryMode == 2)
            {
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId) == false)
                    return rtulst;
                var rtuInfo =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as Wj3005Rtu;
                if (rtuInfo == null || rtuInfo.WjVoltage == null) return rtulst;
                if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) return rtulst;
                if (rtuInfo.WjVoltage.IsHasElec == false) return rtulst;

                rtulst.Add(RtuId);
            }
            else
            {

                if (AreaId == -1) //全部区域终端
                {
                    //return new List<int>();
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            //if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false)
                            //    continue;
                            var rtuInfo =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g] as Wj3005Rtu;
                            if (rtuInfo == null || rtuInfo.WjVoltage == null) continue;
                            if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) continue;
                            if (rtuInfo.WjVoltage.IsHasElec == false) continue;

                            if (rtulst.Contains(g) == false) rtulst.Add(g);
                        }
                    }

                }

                if (GrpId == -1)
                {
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false)
                                continue;
                            var rtuInfo =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g] as Wj3005Rtu;
                            if (rtuInfo == null || rtuInfo.WjVoltage == null) continue;
                            if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) continue;
                            if (rtuInfo.WjVoltage.IsHasElec == false) continue;

                            if (rtulst.Contains(g) == false) rtulst.Add(g);
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
                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false)
                                continue;
                            var rtuInfo =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g] as Wj3005Rtu;
                            if (rtuInfo == null || rtuInfo.WjVoltage == null) continue;
                            if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) continue;
                            if (rtuInfo.WjVoltage.IsHasElec == false) continue;

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


        private static int buttonClick = 0;
        #region CmdReadElec

        private ICommand _cmdReadElecquery;


        private DateTime _dtCmdReadElec;

        public ICommand CmdReadElec
        {
            get
            {
                if (_cmdReadElecquery == null)
                    _cmdReadElecquery = new RelayCommand(ExCmdReadElec, CanCmdReadElec, false);
                return _cmdReadElecquery;
            }
        }

        private void ExCmdReadElec()
        {
            _dtCmdReadElec = DateTime.Now;
            buttonClick = 1;
            Records.Clear();
            recordsKeys.Clear();
            //var rtulst = GetRtusLst();
            //var index = 1;
            if (RtuId == 0 && QueryMode==2)
            {
                UMessageBox.Show("提醒", "未选择终端！", UMessageBoxButton.Ok);
                return;
            }
            ReadElec();
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  查询成功，共计" + Records.Count + " 条数据.";
        }

        private bool _isAns = false;
        private void ReadElec()
        {
            var tStartTime = new DateTime(DtStartTimeTime.Year, DtStartTimeTime.Month, DtStartTimeTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEndTimeTime.Year, DtEndTimeTime.Month, DtEndTimeTime.Day, 23, 59, 59);
            var rtulst = GetRtusLst();
            if (rtulst.Count == 0)
            {
                UMessageBox.Show("提醒", "未符合标准的终端！", UMessageBoxButton.Ok);
                return;
            }
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_elec_data;
            // .wlst_cnt_wj3090_request_open_close_light_record ;//.ServerPart.wlst_OpenCloseLight_clinet_request_rtuopencloseLightrecord;
            info.WstRtuElecData.DtEnd = tEndTime.Ticks;
            info.WstRtuElecData.DtStart = tStartTime.Ticks;
            info.WstRtuElecData.RtuItems = rtulst;
            info.WstRtuElecData.OP = 2;
            SndOrderServer.OrderSnd(info, 10, 6);
            _isAns = false;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在读取实时电能...";

            Wlst.Cr.Coreb.AsyncTask .Qtz .AddQtz("null", 8888, DateTime.Now.Ticks + 5 * 10000000,100, ShowMeasureFail, DateTime.Now.Ticks,0);

        }


        /// <summary>
        /// 等待一段时间后,判断是否选测是否应答;如果未应答
        /// </summary>
        private void ShowMeasureFail(object obj)
        {
            if (_isAns) return;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  读取实时电能失败...";
        }

        private bool CanCmdReadElec()
        {
            return DateTime.Now.Ticks - _dtCmdReadElec.Ticks > 30000000;
        }


        #endregion


        
        //打印

        #region CmdPrint

        private DateTime _dtCmdPrint;
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
            _dtCmdPrint = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("序号");
                tabletitle.Add("终端地址");
                tabletitle.Add("终端名称");
                tabletitle.Add("采集时间");
                tabletitle.Add("A相电能");
                tabletitle.Add("B相电能");
                tabletitle.Add("C相电能");
                tabletitle.Add("总电能");
                tabletitle.Add("A相电压");
                tabletitle.Add("B相电压");
                tabletitle.Add("C相电压");
                tabletitle.Add("A相电流");
                tabletitle.Add("B相电流");
                tabletitle.Add("C相电流");

                tabletitle.Add("A相功率");
                tabletitle.Add("B相功率");
                tabletitle.Add("C相功率");
                tabletitle.Add("A相无功功率");
                tabletitle.Add("B相无功功率");
                tabletitle.Add("C相无功功率");
                tabletitle.Add("A相功率因素");
                tabletitle.Add("B相功率因素");
                tabletitle.Add("C相功率因素");
                tabletitle.Add("A相无功电能");
                tabletitle.Add("B相无功电能");
                tabletitle.Add("C相无功电能");

                var table = new List<List<string>>();
                foreach (var g in Records)
                {

                    var tem = new List<string>();
                    tem.Add(g.Index + "");
                    tem.Add(g.PhyId + "");
                    tem.Add(g.RtuName);
                    tem.Add(g.DateCreate);
                    tem.Add(g.AactiveElec.ToString("f2") + "");
                    tem.Add(g.BactiveElec.ToString("f2") + "");
                    tem.Add(g.CactiveElec.ToString("f2") + "");
                    tem.Add(g.ABCElec.ToString("f2") + "");
                    tem.Add(g.Avoltage.ToString("f2") + "");
                    tem.Add(g.Bvoltage.ToString("f2") + "");
                    tem.Add(g.Cvoltage.ToString("f2") + "");
                    tem.Add(g.Acurrent.ToString("f2") + "");
                    tem.Add(g.Bcurrent.ToString("f2") + "");
                    tem.Add(g.Ccurrent.ToString("f2") + "");

                    tem.Add(g.AactivePower.ToString("f2") + "");
                    tem.Add(g.BactivePower.ToString("f2") + "");
                    tem.Add(g.CactivePower.ToString("f2") + "");

                    tem.Add(g.AreactivePower.ToString("f2") + "");
                    tem.Add(g.BreactivePower.ToString("f2") + "");
                    tem.Add(g.CreactivePower.ToString("f2") + "");

                    tem.Add(g.APowerFactor.ToString("f2") + "");
                    tem.Add(g.BPowerFactor.ToString("f2") + "");
                    tem.Add(g.CPowerFactor.ToString("f2") + "");


                    tem.Add(g.AreactiveElec.ToString("f2") + "");
                    tem.Add(g.BreactiveElec.ToString("f2") + "");
                    tem.Add(g.CreactiveElec.ToString("f2") + "");

                    table.Add(tem);
                }
                Wlst.print.Prints.Print(tabletitle, table, false, "电能查询表",
                                        Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdPrint.Ticks > 30000000;
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
                lsttitle.Add("终端名称");
                lsttitle.Add("采集时间");
                lsttitle.Add("A相有功电能累积");
                lsttitle.Add("B相有功电能累积");
                lsttitle.Add("C相有功电能累积");
                lsttitle.Add("总有功电能累积");
                lsttitle.Add("A相实时电压");
                lsttitle.Add("B相实时电压");
                lsttitle.Add("C相实时电压");
                lsttitle.Add("A相实时电流");
                lsttitle.Add("B相实时电流");
                lsttitle.Add("C相实时电流");

                lsttitle.Add("A相有功功率");
                lsttitle.Add("B相有功功率");
                lsttitle.Add("C相有功功率");
                lsttitle.Add("A相无功功率");
                lsttitle.Add("B相无功功率");
                lsttitle.Add("C相无功功率");
                lsttitle.Add("A相功率因素");
                lsttitle.Add("B相功率因素");
                lsttitle.Add("C相功率因素");
                lsttitle.Add("A相无功电能");
                lsttitle.Add("B相无功电能");
                lsttitle.Add("C相无功电能");


                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tem = new List<object>();
                    tem.Add(g.Index + "");
                    tem.Add(g.PhyId + "");
                    tem.Add(g.RtuName);
                    tem.Add(g.DateCreate);
                    tem.Add(g.AactiveElec.ToString("f2") + "");
                    tem.Add(g.BactiveElec.ToString("f2") + "");
                    tem.Add(g.CactiveElec.ToString("f2") + "");
                    tem.Add(g.ABCElec.ToString("f2") + "");
                    tem.Add(g.Avoltage.ToString("f2") + "");
                    tem.Add(g.Bvoltage.ToString("f2") + "");
                    tem.Add(g.Cvoltage.ToString("f2") + "");
                    tem.Add(g.Acurrent.ToString("f2") + "");
                    tem.Add(g.Bcurrent.ToString("f2") + "");
                    tem.Add(g.Ccurrent.ToString("f2") + "");

                    tem.Add(g.AactivePower.ToString("f2") + "");
                    tem.Add(g.BactivePower.ToString("f2") + "");
                    tem.Add(g.CactivePower.ToString("f2") + "");

                    tem.Add(g.AreactivePower.ToString("f2") + "");
                    tem.Add(g.BreactivePower.ToString("f2") + "");
                    tem.Add(g.CreactivePower.ToString("f2") + "");

                    tem.Add(g.APowerFactor.ToString("f2") + "");
                    tem.Add(g.BPowerFactor.ToString("f2") + "");
                    tem.Add(g.CPowerFactor.ToString("f2") + "");


                    tem.Add(g.AreactiveElec.ToString("f2") + "");
                    tem.Add(g.BreactiveElec.ToString("f2") + "");
                    tem.Add(g.CreactiveElec.ToString("f2") + "");


                    lstobj.Add(tem);
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
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_elec_data, // .wlst_svr_ans_cnt_request_wj3090_measure_data ,
                RecordDataRequest,
                typeof (ElectricityQueryViewModel), this);
        }

        private List<Tuple<int, long>> recordsKeys = new List<Tuple<int, long>>(); 
        public void RecordDataRequest(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (!_thisViewActive) return;
            if (infos == null || infos.WstRtuElecData == null) return;
            // 界面为立即抄电能,但数据不是
            if (buttonClick == 1 && infos.WstRtuElecData.OP != 2) return;
            //界面为查询,但数据不是
            if (buttonClick == 0 && infos.WstRtuElecData.OP != 1) return;

            if (infos.WstRtuElecData.OP == 2) _isAns = true;
            //Records .Clear();
            //recordsKeys.Clear();
            lock (this)
            {


                var list = infos.WstRtuElecData.Items;
                if (list.Count ==0)
                {
                    if (buttonClick == 0)
                    {
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 电能查询成功，共计" + Records.Count +
                                 " 条数据.";
                    }
                    else
                    {
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 读取电能成功，共计" + Records.Count +
                                 " 条数据.";
                    }
                }
                foreach (var g in list)
                {
                    foreach (var k in g.Items)
                    {
                        bool alreadyHave = false;
                        var item = new ElectricityOneItem();
                        var tu = new Tuple<int, long>(g.RtuId, k.DateCreate);
                        if (recordsKeys.Contains(tu))
                        {
                            foreach (var j in Records)
                            {
                                if (j.RtuId == g.RtuId && j.LngDateCreate == k.DateCreate)
                                {
                                    item = j;
                                    alreadyHave = true;
                                    break;
                                }
                            }
                        }

                        if (alreadyHave) //是否已存在
                        {
                            if (k.Phase == 1) //A相
                            {
                                item.APowerFactor = k.PowerFactor;
                                item.AactiveElec = k.ActiveElec;
                                item.AactivePower = k.ActivePower;
                                item.Acurrent = k.Current;
                                item.AreactiveElec = k.ReactiveElec;
                                item.AreactivePower = k.ReactivePower;
                                item.Avoltage = k.Voltage;
                                item.LngDateCreate = k.DateCreate;
                                item.DateCreate = new DateTime(k.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (k.Phase == 2) //B相
                            {
                                item.BPowerFactor = k.PowerFactor;
                                item.BactiveElec = k.ActiveElec;
                                item.BactivePower = k.ActivePower;
                                item.Bcurrent = k.Current;
                                item.BreactiveElec = k.ReactiveElec;
                                item.BreactivePower = k.ReactivePower;
                                item.Bvoltage = k.Voltage;
                                item.DateCreate = new DateTime(k.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                                item.LngDateCreate = k.DateCreate;

                            }
                            else if (k.Phase == 3) //C相
                            {
                                item.CPowerFactor = k.PowerFactor;
                                item.CactiveElec = k.ActiveElec;
                                item.CactivePower = k.ActivePower;
                                item.Ccurrent = k.Current;
                                item.CreactiveElec = k.ReactiveElec;
                                item.CreactivePower = k.ReactivePower;
                                item.Cvoltage = k.Voltage;
                                item.DateCreate = new DateTime(k.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                                item.LngDateCreate = k.DateCreate;
                            }
                            //lvf 2018年8月14日07:40:08 添加中电能
                            item.ABCElec = item.AactiveElec + item.BactiveElec + item.CactiveElec;

                        }
                        else //新加
                        {
                            item.RtuId = g.RtuId;
                            item.Frequency = g.Frequency;
                            item.DateCreate = new DateTime(k.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                            item.LngDateCreate = k.DateCreate;
                            if (k.Phase == 1) //A相
                            {
                                item.APowerFactor = k.PowerFactor;
                                item.AactiveElec = k.ActiveElec;
                                item.AactivePower = k.ActivePower;
                                item.Acurrent = k.Current;
                                item.AreactiveElec = k.ReactiveElec;
                                item.AreactivePower = k.ReactivePower;
                                item.Avoltage = k.Voltage;

                            }
                            else if (k.Phase == 2) //B相
                            {
                                item.BPowerFactor = k.PowerFactor;
                                item.BactiveElec = k.ActiveElec;
                                item.BactivePower = k.ActivePower;
                                item.Bcurrent = k.Current;
                                item.BreactiveElec = k.ReactiveElec;
                                item.BreactivePower = k.ReactivePower;
                                item.Bvoltage = k.Voltage;

                            }
                            else if (k.Phase == 3) //C相
                            {
                                item.CPowerFactor = k.PowerFactor;
                                item.CactiveElec = k.ActiveElec;
                                item.CactivePower = k.ActivePower;
                                item.Ccurrent = k.Current;
                                item.CreactiveElec = k.ReactiveElec;
                                item.CreactivePower = k.ReactivePower;
                                item.Cvoltage = k.Voltage;
                            }
                            //lvf 2018年8月14日07:40:08 添加中电能
                            item.ABCElec = item.AactiveElec + item.BactiveElec + item.CactiveElec;

                            item.Index = Records.Count + 1;
                            Records.Add(item);
                            if (recordsKeys.Contains(tu) == false) recordsKeys.Add(tu);

                            //if (Records.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count +
                            //         " 条数据.";
                        }


                    }







                    if (Records.Count%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                    if (buttonClick == 0)
                    {
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 电能查询成功，共计" + Records.Count +
                                 " 条数据.";
                    }
                    else
                    {
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 读取电能成功，共计" + Records.Count +
                                 " 条数据.";
                    }

                }


            }





            ////  Remind = "数据已反馈，查询命令已结束，请查看数据！";
            //var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(info.RtuId);
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" +
            //         (tmp == null ? info.RtuId + "" : tmp.RtuName) + "--终端数据查询成功，共计" + Records.Count + "条数据.";
            ////info.Items .Count + " 条数据.";
        }


        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }


        public override void ExPublishedEvent(
            PublishEventArgs args)
        {

            if (_thisViewActive == false) return;

            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    if (QueryMode != 2) return;
                    int id = Convert.ToInt32(args.GetParams()[0]);
                    //if (id > 1100000)
                    //{
                    //    var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                    //    if (tmps == null) return;
                    //    id = tmps.RtuFid;
                    //}
                    if (id < 1000000 || id > 1100000) return;

                    RtuId = id;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


    /// <summary>
    /// attribute
    /// </summary>
    public partial  class ElectricityQueryViewModel
    {


        #region  QueryMode
        private int _queryMode;

        /// <summary>
        /// 查询模式   1：全部设备   2：当前设备   3：区域查询  lvf 2018年6月15日09:37:17
        /// </summary>
        public int QueryMode
        {
            get { return _queryMode; }
            set
            {
                if (value != _queryMode)
                {
                    _queryMode = value;
                    this.RaisePropertyChanged(() => this.QueryMode);
                    //if (QueryMode ==2 )
                    //{
                    //    RtuId = 0;
                    //    RtuName = "通过终端树勾选终端进行故障查询.";
                    //}
                }
            }
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


        private bool _blgrpEnable;

        /// <summary>
        /// 
        /// </summary>
        public bool IsGrpEnable
        {
            get { return _blgrpEnable; }
            set
            {
                if (value != _blgrpEnable)
                {
                    _blgrpEnable = value;
                    this.RaisePropertyChanged(() => this.IsGrpEnable);
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
                IsGrpEnable = false;
            }
            else
            {
                GrpVisi = Visibility.Visible;
                IsGrpEnable = true;
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

        #region RtuId

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
                    if (RtuId == 0)
                    {
                        PhyId =0 ;
                        RtuName = "无";
                    }
                    //基本信息
                    var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(_rtuId);
                    if (info == null) return;
                    RtuName = info.RtuName;
                    PhyId = info.RtuPhyId;
                    //InstallDate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                    //Position = info.RtuInstallAddr;

                    //LoopsNum = 0;
                    ////回路信息
                    //var tmps =
                    //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    //        RtuId]
                    //    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    //if (tmps == null) return;
                    //LoopsNum = tmps.WjLoops.Count;

                    ////区域信息
                    //var areaId =
                    //    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(_rtuId);

                    //if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
                    //{
                    //    AreaName =
                    //        Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                    //}
                    //else
                    //{
                    //    AreaName = "未知";
                    //}
                    //if (areaId == 0) AreaName = "默认区域";

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
        #endregion

        #region Records


        private ObservableCollection<ElectricityOneItem> _records;

        public ObservableCollection<ElectricityOneItem> Records
        {
            get { return _records ?? (_records = new ObservableCollection<ElectricityOneItem>()); }
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

        #region DtStartTimeTime

        private DateTime _dtStartTimeTime;

        public DateTime DtStartTimeTime
        {
            get { return _dtStartTimeTime; }
            set
            {
                if (value != _dtStartTimeTime)
                {
                    _dtStartTimeTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTimeTime);

                }
            }
        }

        #endregion

        #region DtEndTimeTime

        private DateTime _dtEndTimeTime;

        public DateTime DtEndTimeTime
        {
            get { return _dtEndTimeTime; }
            set
            {
                if (value != _dtEndTimeTime)
                {
                    _dtEndTimeTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTimeTime);
                }
            }
        }

        #endregion

    
    }
}






public class ElectricityOneItem : ObservableObject
    {
        public ElectricityOneItem()
        {

            RtuName = "未知";
            PhyId = 0;

            RtuId = 0;
            Index = 0;



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
                    //InstallDate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                    //Position = info.RtuInstallAddr;

                    //LoopsNum = 0;
                    ////回路信息
                    //var tmps =
                    //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    //        RtuId]
                    //    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    //if (tmps == null) return;
                    //LoopsNum = tmps.WjLoops.Count;

                    ////区域信息
                    //var areaId =
                    //    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(_rtuId);

                    //if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
                    //{
                    //    AreaName =
                    //        Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                    //}
                    //else
                    //{
                    //    AreaName = "未知";
                    //}
                    //if (areaId == 0) AreaName = "默认区域";

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

        private double _frequency;

        public double Frequency
        {
            get { return _frequency; }
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    this.RaisePropertyChanged(() => this.Frequency);
                }
            }
        }


        private string _dateCreate;

        /// <summary>
        /// 客户的接收到数据的时间
        /// </summary>
        public string DateCreate
        {
            get { return _dateCreate; }
            set
            {
                if (value != _dateCreate)
                {
                    _dateCreate = value;
                    this.RaisePropertyChanged(() => this.DateCreate);
                }
            }
        }



        private long _lngDateCreate;

        /// <summary>
        /// 
        /// </summary>
        public long LngDateCreate
        {
            get { return _lngDateCreate; }
            set
            {
                if (value != _lngDateCreate)
                {
                    _lngDateCreate = value;
                    this.RaisePropertyChanged(() => this.LngDateCreate);
                }
            }
        }

        #region A相

        private double _avoltage;
        /// <summary>
        /// 电压
        /// </summary>
        public double Avoltage
        {
            get { return _avoltage; }
            set
            {
                if (_avoltage != value)
                {
                    _avoltage = value;
                    this.RaisePropertyChanged(() => this.Avoltage);
                }
            }
        }


        private double _acurrent;
        /// <summary>
        /// 电流
        /// </summary>
        public double Acurrent
        {
            get { return _acurrent; }
            set
            {
                if (_acurrent != value)
                {
                    _acurrent = value;
                    this.RaisePropertyChanged(() => this.Acurrent);
                }
            }
        }
        private double _aPowerFactor;
        /// <summary>
        /// 功率因素
        /// </summary>
        public double APowerFactor
        {
            get { return _aPowerFactor; }
            set
            {
                if (_aPowerFactor != value)
                {
                    _aPowerFactor = value;
                    this.RaisePropertyChanged(() => this.APowerFactor);
                }
            }
        }


        private double _aactivePower;
        /// <summary>
        /// 有功功率
        /// </summary>
        public double AactivePower
        {
            get { return _aactivePower; }
            set
            {
                if (_aactivePower != value)
                {
                    _aactivePower = value;
                    this.RaisePropertyChanged(() => this.AactivePower);
                }
            }
        }

        private double _areactivePower;
        /// <summary>
        /// 无功功率
        /// </summary>
        public double AreactivePower
        {
            get { return _areactivePower; }
            set
            {
                if (_areactivePower != value)
                {
                    _areactivePower = value;
                    this.RaisePropertyChanged(() => this.AreactivePower);
                }
            }
        }
        private double _aactiveElec;
        /// <summary>
        /// 有功电能
        /// </summary>
        public double AactiveElec
        {
            get { return _aactiveElec; }
            set
            {
                if (_aactiveElec != value)
                {
                    _aactiveElec = value;
                    this.RaisePropertyChanged(() => this.AactiveElec);
                }
            }
        }


        private double _areactiveElec;
        /// <summary>
        /// 无功电能
        /// </summary>
        public double AreactiveElec
        {
            get { return _areactiveElec; }
            set
            {
                if (_areactiveElec != value)
                {
                    _areactiveElec = value;
                    this.RaisePropertyChanged(() => this.AreactiveElec);
                }
            }
        }


        #endregion

        #region B相

        private double _bvoltage;
        /// <summary>
        /// 电压
        /// </summary>
        public double Bvoltage
        {
            get { return _bvoltage; }
            set
            {
                if (_bvoltage != value)
                {
                    _bvoltage = value;
                    this.RaisePropertyChanged(() => this.Bvoltage);
                }
            }
        }


        private double _bcurrent;
        /// <summary>
        /// 电流
        /// </summary>
        public double Bcurrent
        {
            get { return _bcurrent; }
            set
            {
                if (_bcurrent != value)
                {
                    _bcurrent = value;
                    this.RaisePropertyChanged(() => this.Bcurrent);
                }
            }
        }
        private double _bPowerFactor;
        /// <summary>
        /// 功率因素
        /// </summary>
        public double BPowerFactor
        {
            get { return _bPowerFactor; }
            set
            {
                if (_bPowerFactor != value)
                {
                    _bPowerFactor = value;
                    this.RaisePropertyChanged(() => this.BPowerFactor);
                }
            }
        }


        private double _bactivePower;
        /// <summary>
        /// 有功功率
        /// </summary>
        public double BactivePower
        {
            get { return _bactivePower; }
            set
            {
                if (_bactivePower != value)
                {
                    _bactivePower = value;
                    this.RaisePropertyChanged(() => this.BactivePower);
                }
            }
        }

        private double _breactivePower;
        /// <summary>
        /// 无功功率
        /// </summary>
        public double BreactivePower
        {
            get { return _breactivePower; }
            set
            {
                if (_breactivePower != value)
                {
                    _breactivePower = value;
                    this.RaisePropertyChanged(() => this.BreactivePower);
                }
            }
        }
        private double _bactiveElec;
        /// <summary>
        /// 有功电能
        /// </summary>
        public double BactiveElec
        {
            get { return _bactiveElec; }
            set
            {
                if (_bactiveElec != value)
                {
                    _bactiveElec = value;
                    this.RaisePropertyChanged(() => this.BactiveElec);
                }
            }
        }


        private double _breactiveElec;
        /// <summary>
        /// 无功电能
        /// </summary>
        public double BreactiveElec
        {
            get { return _breactiveElec; }
            set
            {
                if (_breactiveElec != value)
                {
                    _breactiveElec = value;
                    this.RaisePropertyChanged(() => this.BreactiveElec);
                }
            }
        }


        #endregion

        #region C相

        private double _cvoltage;
        /// <summary>
        /// 电压
        /// </summary>
        public double Cvoltage
        {
            get { return _cvoltage; }
            set
            {
                if (_cvoltage != value)
                {
                    _cvoltage = value;
                    this.RaisePropertyChanged(() => this.Cvoltage);
                }
            }
        }


        private double _ccurrent;
        /// <summary>
        /// 电流
        /// </summary>
        public double Ccurrent
        {
            get { return _ccurrent; }
            set
            {
                if (_ccurrent != value)
                {
                    _ccurrent = value;
                    this.RaisePropertyChanged(() => this.Ccurrent);
                }
            }
        }
        private double _cPowerFactor;
        /// <summary>
        /// 功率因素
        /// </summary>
        public double CPowerFactor
        {
            get { return _cPowerFactor; }
            set
            {
                if (_cPowerFactor != value)
                {
                    _cPowerFactor = value;
                    this.RaisePropertyChanged(() => this.CPowerFactor);
                }
            }
        }


        private double _cactivePower;
        /// <summary>
        /// 有功功率
        /// </summary>
        public double CactivePower
        {
            get { return _cactivePower; }
            set
            {
                if (_cactivePower != value)
                {
                    _cactivePower = value;
                    this.RaisePropertyChanged(() => this.CactivePower);
                }
            }
        }

        private double _creactivePower;
        /// <summary>
        /// 无功功率
        /// </summary>
        public double CreactivePower
        {
            get { return _creactivePower; }
            set
            {
                if (_creactivePower != value)
                {
                    _creactivePower = value;
                    this.RaisePropertyChanged(() => this.CreactivePower);
                }
            }
        }
        private double _cactiveElec;
        /// <summary>
        /// 有功电能
        /// </summary>
        public double CactiveElec
        {
            get { return _cactiveElec; }
            set
            {
                if (_cactiveElec != value)
                {
                    _cactiveElec = value;
                    this.RaisePropertyChanged(() => this.CactiveElec);
                }
            }
        }

        private double _abcElec;
        /// <summary>
        /// 有功电能
        /// </summary>
        public double ABCElec
        {
            get { return _abcElec; }
            set
            {
                if (_abcElec != value)
                {
                    _abcElec = value;
                    this.RaisePropertyChanged(() => this.ABCElec);
                }
            }
        }


        private double _creactiveElec;
        /// <summary>
        /// 无功电能
        /// </summary>
        public double CreactiveElec
        {
            get { return _creactiveElec; }
            set
            {
                if (_creactiveElec != value)
                {
                    _creactiveElec = value;
                    this.RaisePropertyChanged(() => this.CreactiveElec);
                }
            }
        }


        #endregion
        #endregion

    }

