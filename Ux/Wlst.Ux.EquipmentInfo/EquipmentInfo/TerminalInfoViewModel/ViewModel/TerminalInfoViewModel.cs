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
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.client;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.TerminalInfoViewModel.ViewModel
{
    public partial class TerminalInfoViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {

        private ObservableCollection<TerminalInfoOneViewModel> _item;

        public ObservableCollection<TerminalInfoOneViewModel> Item
        {
            get
            {
                if (_item == null)
                    _item = new ObservableCollection<TerminalInfoOneViewModel>();

                return _item;
            }
            set
            {
                if (value == _item) return;
                _item = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }

        private int _terminalNumber;
        /// <summary>
        /// 终端数量
        /// </summary>
        public int TerminalNumber
        {
            get { return _terminalNumber; }
            set
            {
                if (_terminalNumber == value) return;
                _terminalNumber = value;
                this.RaisePropertyChanged(() => this.TerminalNumber);
            }
        }


    }

    public partial class TerminalInfoViewModel
    {
        //查询
        #region CmdQuery
        private DateTime _dtCmdQuery;
        private ICommand _cmdCmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdCmdQuery == null)
                    _cmdCmdQuery = new RelayCommand(ExCmdQuery, CanExCmdQuery, false);
                return _cmdCmdQuery;
            }
        }

        private void ExCmdQuery()
        {
            _dtCmdQuery = DateTime.Now;
            Item.Clear();
            int i = 0;
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu)
                    continue;
                i++;
                RequestTerminalInformation(t.Value, i);
            }
            TerminalNumber = i;

        }
        private bool CanExCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 30000000;
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
            WriteData();

        }

        private bool CanExCmdExport()
        {
            if (Item.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
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
                tabletitle.Add("物理地址");
                tabletitle.Add("逻辑地址");
                tabletitle.Add("终端名称");
                tabletitle.Add("状态");
                tabletitle.Add("归属分组");
                tabletitle.Add("IP地址");
                tabletitle.Add("手机号码");
                tabletitle.Add("开通时间");
                var index = 0;
                var table = new List<List<string>>();
                foreach (var g in Item)
                {
                    index++;
                    var tem = new List<string>();
                    tem.Add(index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.LogicId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuState);
                    tem.Add(g.GrpName);
                    tem.Add(g.IP);
                    tem.Add(g.SimNumber);
                    tem.Add(g.DataCreate.ToString("MM/dd HH:mm:ss"));
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, true, "终端设备信息", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Item.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdPrint.Ticks > 30000000;
        }
        #endregion
    }

    public partial class TerminalInfoViewModel
    {
        private void RequestTerminalInformation(WjParaBase info,int i)
        {
            var tt = new TerminalInfoOneViewModel();
            tt.Index = i;
            
            tt.LogicId = info.RtuId;
            tt.RtuName = info.RtuName;
            tt.RtuState = info.RtuStateCode == 2 ? "使用" : info.RtuStateCode == 1 ? "停运" : "不用";
            var groupidx =
                         Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(info.RtuId);
            if (groupidx != null)
            {
                var infosss =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                        groupidx.Item1, groupidx.Item2);
                if (infosss != null) tt.GrpName = infosss.GroupName;
            }
            //tt.GrpName = Get_Special_GrpName(info.RtuId);
            var infos = info as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            if (infos != null)
            {
                tt.IP = new System.Net.IPAddress(BitConverter.GetBytes(infos.WjGprs.StaticIp)).ToString();
                tt.SimNumber = infos.WjGprs.MobileNo;
                tt.PhyId = infos.RtuPhyId;
            }
            tt.DataCreate=new DateTime(info.DateCreate);
            Item.Add(tt);
        }

        private string Get_Special_GrpName(int rtuId)
        {

            int areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);

            if (areaId != -1)
            {
                var areaInfo = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(areaId);

                return "特殊终端" + "-" + areaInfo.AreaName;
            }

            return "----";

        }

        private void WriteData()
        {
            try
            {

                //储存附属设备
                var fssbinfo = new Dictionary<int, List<int>>();
                // var lst = new List<>();

                var lstt = new ObservableCollection<TerminalInfoOneViewModel>();
                Dictionary<int, string> tmpdir = new Dictionary<int, string>();
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
                {
                    //if (t.Value.LstTml.Count == 0) continue;
                    foreach (var g in t.Value.LstTml)
                    {
                        if (tmpdir.ContainsKey(g)) continue;
                        tmpdir.Add(g, t.Value.AreaId.ToString("d2") + "-" + t.Key + "-" + t.Value.GroupName);
                    }
                }

                foreach (
                      var t in
                          Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                      )
                {
                    if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;

                    #region 巡测数据加载
                    var ttt = new TerminalInfoOneViewModel() { LogicId = t.Key, PhyId = t.Value.RtuPhyId };
                    ttt.RtuName = t.Value.RtuName;
                    ttt.RequestNewDataTime = "--";
                    ttt.ReceiveNewDataTime = "--";
                    ttt.TimeSpan = "--";
                    ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                    if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
                    //                else ttt.GrpName = "----";
                    else ttt.GrpName = Get_Special_GrpName(t.Key);

                    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                    if (tmp != null && tmp.RtuNewData != null)
                    {
                        ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                        ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                        ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                        ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                        ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                        ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                        ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                        ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                        ttt.ErrorCount = tmp.ErrorCount;
                    }

                    lstt.Add(ttt);
                    #endregion

                }

                foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    if (t.Value.RtuFid != 0)
                    {
                        if (fssbinfo.ContainsKey(t.Value.RtuFid))
                        {
                            fssbinfo[t.Value.RtuFid].Add(t.Key);
                        }
                        else
                        {
                            fssbinfo.Add(t.Value.RtuFid, new List<int>() { t.Key });
                        }
                    }
                    //else
                    //{
                    //    if (t.Value.EquipmentType == WjParaBase.EquType.Rtu) 
                    //    {
                    //        lst.Add(t.Value);
                    //    }
                    //}
                }


                //建立表头
                var writeinfo = new List<List<object>>();
                var titleinfo = new List<object>();
                titleinfo.Add("序号");

                titleinfo.Add("物理地址");
                titleinfo.Add("逻辑地址");
                titleinfo.Add("终端名称");
                titleinfo.Add("归属分组");

                //最新数据
                titleinfo.Add("最新数据的使用状态");
                titleinfo.Add("最新数据时间");

                titleinfo.Add("使用状态");
                titleinfo.Add("Ip地址");
                titleinfo.Add("电话号码");
                titleinfo.Add("心跳周期");
                titleinfo.Add("主报周期");

                //二期新增参数
                titleinfo.Add("开通日期");
                titleinfo.Add("最后更新日期");
                titleinfo.Add("安装位置");
                titleinfo.Add("备注信息");
                titleinfo.Add("唯一识别码");
                titleinfo.Add("是否通过电流判断辅助触点");

                //回路信息

                titleinfo.Add("输出矢量序列");
                titleinfo.Add("K1路数");
                titleinfo.Add("K2路数");
                titleinfo.Add("K3路数");
                titleinfo.Add("K4路数");
                titleinfo.Add("K5路数");
                titleinfo.Add("K6路数");
                titleinfo.Add("K7路数");
                titleinfo.Add("K8路数");
                titleinfo.Add("回路总数");
                titleinfo.Add("开关量路数");
                titleinfo.Add("开关量矢量序列");
                titleinfo.Add("电流矢量序列");
                titleinfo.Add("跳变报警序列");
                titleinfo.Add("相位序列");
                titleinfo.Add("互感器比序列");

                //防盗信息
                titleinfo.Add("防盗路数");
                titleinfo.Add("防盗名称");

                //电表信息
                titleinfo.Add("电表变比");


                //终端型号
                titleinfo.Add("设备型号");





                //        var lst = (from t in MeasurePatrolData orderby t.PhysicalId select t).ToList();
                int index = 1;
                var lst = (from t in lstt orderby t.PhyId select t).ToList();
                foreach (var t in lst)
                {
                    var tmp = new List<object>();

                    tmp.Add(index);
                    index++;

                    tmp.Add(t.PhyId);
                    tmp.Add(t.LogicId);
                    tmp.Add(t.RtuName);
                    tmp.Add(t.GrpName);


                    //最新数据
                    var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.LogicId);
                    if (run != null && run.RtuNewData != null)
                    {
                        if (t.RtuState == "使用")
                        {
                            var title = "";
                            if (run.RtuNewData.Alarms.ContainsKey(1) && run.RtuNewData.Alarms[1]) title += "停电";
                            else title += "供电";

                            if (run.RtuNewData.Alarms.ContainsKey(3) && run.RtuNewData.Alarms[3]) title += "停运中";
                            else title += "使用中 ";

                            tmp.Add(title);
                        }
                        else
                        {
                            tmp.Add("--");
                        }
                        tmp.Add(run.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        tmp.Add("");
                        tmp.Add("");
                    }

                    tmp.Add(t.RtuState);


                    var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.LogicId);
                    if (tmp1 == null || tmp1.EquipmentType != WjParaBase.EquType.Rtu) continue;
                    var tmp2 = tmp1 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tmp2 == null) continue;
                    if (tmp2.WjGprs == null || tmp2.WjLoops == null || tmp2.WjSwitchOuts == null || tmp2.WjVoltage == null) continue;

                    tmp.Add(new System.Net.IPAddress(BitConverter.GetBytes(tmp2.WjGprs.StaticIp)).ToString());
                    tmp.Add(tmp2.WjGprs.MobileNo);
                    tmp.Add(tmp2.WjGprs.RtuHeartbeatCycle);
                    tmp.Add(tmp2.WjGprs.RtuReportCycle);

                    //二期新增参数
                    tmp.Add(string.Format("{0:G}", new DateTime(tmp2.DateCreate)));
                    tmp.Add(string.Format("{0:G}", new DateTime(tmp2.DateUpdate)));
                    tmp.Add(tmp2.RtuInstallAddr);
                    tmp.Add(tmp2.RtuRemark);
                    tmp.Add(tmp2.Idf);
                    if (tmp2.WjVoltage != null)
                    {
                        string isSwitchinputJudgeByA = (tmp2.WjVoltage.IsSwitchinputJudgebyA == true) ? "是" : "否";
                        tmp.Add(isSwitchinputJudgeByA);
                    }
                    else
                    {
                        tmp.Add("");
                    }

                    //回路信息

                    string str = "";
                    var ggg =
                        (from gg in tmp2.WjSwitchOuts.Values orderby gg.SwitchId select gg).
                            ToList();
                    foreach (var g in ggg)
                    {
                        str += g.SwitchVecotr + "-";
                    }
                    if (str.Length > 1) str = str.Substring(0, str.Length - 1);
                    tmp.Add(str);

                    var sout = new Dictionary<int, int>();
                    var shield = new Dictionary<int, Tuple<int, double>>();
                    var alarmHop = new Dictionary<int, bool>();
                    var kglsl = "";
                    var mnlsl = "";
                    var tbbjsl = "";
                    var xwsl = "";
                    var hgxbsl = "";

                    var gggg =
                        (from gg in tmp2.WjLoops.Values orderby gg.LoopId select gg).
                            ToList();

                    foreach (var g in gggg)
                    {
                        if (!sout.ContainsKey(g.SwitchOutputId)) sout.Add(g.SwitchOutputId, 0);
                        sout[g.SwitchOutputId] = sout[g.SwitchOutputId] + 1;

                        if (!shield.ContainsKey(g.SwitchOutputId))
                        {
                            var a = new Tuple<int, double>(g.IsShieldLoop, g.ShieldLittleA);
                            shield.Add(g.SwitchOutputId, a);
                        }
                        if (!alarmHop.ContainsKey(g.SwitchOutputId)) alarmHop.Add(g.SwitchOutputId, g.IsAlarmHop);

                        kglsl += g.VectorSwitchIn + "-";
                        mnlsl += g.VectorMoniliang + "-";
                        tbbjsl += (g.IsAlarmHop ? 1 : 0) + "-";

                        if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Aphase) xwsl += 0 + "-";
                        else if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Bphase) xwsl += 1 + "-";
                        else if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Cphase) xwsl += 2 + "-";
                        else xwsl += g.VoltagePhaseCode + "-";

                        hgxbsl += g.CurrentRange + "-";
                    }
                    int hlsum = 0;
                    for (int i = 1; i < 9; i++)
                    {
                        if (sout.ContainsKey(i))
                        {
                            tmp.Add(sout[i]);
                            hlsum = hlsum + sout[i];
                        }
                        else
                        {
                            tmp.Add(0);
                        }
                    }

                    tmp.Add(hlsum);
                    tmp.Add(ggg.Count);

                    if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
                    if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
                    if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
                    if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
                    if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);

                    tmp.Add(kglsl);
                    tmp.Add(mnlsl);
                    tmp.Add(tbbjsl);
                    tmp.Add(xwsl);
                    tmp.Add(hgxbsl);




                    // 防盗信息
                    string fdname = "";
                    int fdsum = 0;

                    if (fssbinfo.ContainsKey(t.LogicId))
                    {
                        foreach (var f in fssbinfo[t.LogicId])
                        {
                            if (EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                            {
                                if (EquipmentDataInfoHold.InfoItems[f].RtuModel == EnumRtuModel.Wj1090)
                                {
                                    var fdxx = EquipmentDataInfoHold.InfoItems[f] as Wj1090Ldu;
                                    var ntg = (from txr in fdxx.WjLduLines orderby txr.Value.LduLineId ascending select txr.Value).ToList();
                                    foreach (var tg in ntg)
                                    {
                                        fdname = fdname + tg.LduLineName + "/";
                                        fdsum = fdsum + 1;
                                    }

                                }
                            }
                        }
                    }
                    tmp.Add(fdsum);
                    tmp.Add(fdname);

                    // 电表变比
                    string dbbianbi = "";
                    if (fssbinfo.ContainsKey(t.LogicId))
                    {
                        foreach (var f in fssbinfo[t.LogicId])
                        {
                            if (EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                            {
                                if (EquipmentDataInfoHold.InfoItems[f].RtuModel == EnumRtuModel.Wj1050)
                                {
                                    var fdxx = EquipmentDataInfoHold.InfoItems[f] as Wj1050Mru;
                                    dbbianbi = dbbianbi + fdxx.WjMru.MruRatio;
                                }
                            }
                        }
                    }
                    tmp.Add(dbbianbi);

                    // 设备型号
                    string sbxh = "";
                    sbxh = (int)tmp2.RtuModel + "";
                    tmp.Add(sbxh);

                    writeinfo.Add(tmp);
                }

                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出终端参数报表时报错:" + ex);
            }
        }
    }

    public class TerminalInfoOneViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public const long OneSecond = 10000000;
        private const int SwitchOutLoopSum = 8;

        public long DateCreateTime;

        public int AlarmCount;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageA;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageB;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageC;

        /// <summary>
        /// 电流
        /// </summary>
        public Double RtuCurrentSumA;

        /// <summary>
        /// 电流
        /// </summary>
        public Double RtuCurrentSumB;

        /// <summary>
        /// 电流
        /// </summary>
        public Double RtuCurrentSumC;


        private int _index;
        /// <summary>
        /// 终端序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }

        private int _Reqerror;
        /// <summary>
        /// 请求终端最新数据时间
        /// </summary>
        public int ErrorCount
        {
            get { return _Reqerror; }
            set
            {
                if (_Reqerror == value) return;
                _Reqerror = value;
                this.RaisePropertyChanged(() => this.ErrorCount);
            }
        }

        private int _phyId;
        /// <summary>
        /// 物理地址
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId == value) return;
                _phyId = value;
                RaisePropertyChanged(() => PhyId);
            }
        }

        private int _logicId;
        /// <summary>
        /// 逻辑地址
        /// </summary>
        public int LogicId
        {
            get { return _logicId; }
            set
            {
                if (_logicId == value) return;
                _logicId = value;
                RaisePropertyChanged(() => LogicId);
            }
        }

        private string _rtuName;
        /// <summary>
        /// 终端名称 
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName == value) return;
                _rtuName = value;
                this.RaisePropertyChanged(() => this.RtuName);
            }
        }

        private string _rtuState;
        /// <summary>
        /// 状态
        /// </summary>
        public string RtuState
        {
            get { return _rtuState; }
            set
            {
                if (_rtuState == value) return;
                _rtuState = value;
                this.RaisePropertyChanged(() => this.RtuState);
            }
        }

        private string _grpname;
        /// <summary>
        /// 归属分组
        /// </summary>
        public string GrpName
        {
            get { return _grpname; }
            set
            {
                if (_grpname == value) return;
                _grpname = value;
                this.RaisePropertyChanged(() => this.GrpName);
            }
        }

        private string _iP;
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP
        {
            get { return _iP; }
            set
            {
                if (_iP == value) return;
                _iP = value;
                this.RaisePropertyChanged(() => this.IP);
            }
        }

        private string _simNumber;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string SimNumber
        {
            get { return _simNumber; }
            set
            {
                if (_simNumber == value) return;
                _simNumber = value;
                this.RaisePropertyChanged(() => this.SimNumber);
            }
        }

        private DateTime _dataCreate;

        /// <summary>
        /// 开通时间
        /// </summary>
        public DateTime DataCreate
        {
            get { return _dataCreate; }
            set
            {
                if (_dataCreate == value) return;
                _dataCreate = value;
                this.RaisePropertyChanged(() => this.DataCreate);
            }
        }

        private string _RequestNewDataTime;

        /// <summary>
        /// 请求终端最新数据时间
        /// </summary>
        public string RequestNewDataTime
        {
            get { return _RequestNewDataTime; }
            set
            {
                if (_RequestNewDataTime == value) return;
                _RequestNewDataTime = value;
                this.RaisePropertyChanged(() => this.RequestNewDataTime);
            }
        }



        private string _ReceiveNewDataTime;
        //需要显示的接收最新数据时间
        public string ReceiveNewDataTime
        {
            get { return _ReceiveNewDataTime; }
            set
            {
                if (_ReceiveNewDataTime == value) return;
                _ReceiveNewDataTime = value;
                this.RaisePropertyChanged(() => this.ReceiveNewDataTime);
            }
        }

        private string _timeSpan;
        /// <summary>
        /// 发送与接收时间长
        /// </summary>
        public string TimeSpan
        {
            get { return _timeSpan; }
            set
            {
                if (_timeSpan == value) return;
                _timeSpan = value;
                this.RaisePropertyChanged(() => this.TimeSpan);
            }
        }


        private ObservableCollection<NameIntBool> _switchOutState;

        /// <summary>
        /// 回路是否开启 IsSelected==true 开启 Value为回路编号 1为输出1 
        /// </summary>
        public ObservableCollection<NameIntBool> SwitchOutState
        {
            get
            {
                if (_switchOutState == null)
                {
                    _switchOutState = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i < SwitchOutLoopSum + 1; i++)
                    {
                        _switchOutState.Add(new NameIntBool() { IsSelected = false, Name = "", Value = i });
                    }
                }
                return _switchOutState;
            }
        }

        private string _rtuPowerSumA;
        //功率
        public string RtuPowerSumA
        {
            get { return _rtuPowerSumA; }
            set
            {
                if (_rtuPowerSumA == value) return;
                _rtuPowerSumA = value;
                this.RaisePropertyChanged(() => this.RtuPowerSumA);
            }
        }

        private string _rtuPowerSumB;
        //功率
        public string RtuPowerSumB
        {
            get { return _rtuPowerSumB; }
            set
            {
                if (_rtuPowerSumB == value) return;
                _rtuPowerSumB = value;
                this.RaisePropertyChanged(() => this.RtuPowerSumB);
            }
        }

        private string _rtuPowerSumC;
        //功率
        public string RtuPowerSumC
        {
            get { return _rtuPowerSumC; }
            set
            {
                if (_rtuPowerSumC == value) return;
                _rtuPowerSumC = value;
                this.RaisePropertyChanged(() => this.RtuPowerSumC);
            }
        }

        private string _rtuPowerSum;
        //功率
        public string RtuPowerSum
        {
            get { return _rtuPowerSum; }
            set
            {
                if (_rtuPowerSum == value) return;
                _rtuPowerSum = value;
                this.RaisePropertyChanged(() => this.RtuPowerSum);
            }
        }

        /// <summary>
        /// 设置开关量输出吸合状态
        /// </summary>
        /// <param name="state">开关量1~16</param>
        public void SetSwitchOutState(List<bool> state)
        {
            for (int i = 1; i < state.Count() + 1; i++)
            {
                foreach (var t in SwitchOutState)
                {
                    if (t.Value == i) t.IsSelected = state[i - 1];
                }
            }
        }
    }
}
