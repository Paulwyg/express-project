using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows.Input;


using Telerik.Windows.Controls.ColorEditor;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.StateBarModule.UserOperateMsgView.Services;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Ux.StateBarModule.UserOperateMsgView.ViewModel
{

    [Export(typeof(IIUserOperatorRecords))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class UserOperateMsgViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIUserOperatorRecords
    {
        public UserOperateMsgViewModel()
        {
            InitAction();
            //InitEvent();
            IsChecked = false;
            Wlst.Cr.CoreMims.Services.SndOrderServer.BeforeSnd += BeforSnd1;


            //EventPublisher.AddEventSubScriptionTokener(
            //    Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
            //    FundOrderFilters);

            // Wlst .Cr .Core.ModuleServices .DelayEvent .RegisterDelayEvent(OnLoadCpt ,2,DelayEventHappen.EventOne);
            Wlst.Cr.Coreb.AsyncTask .Qtz .AddQtz("nuu", 8888, DateTime.Now.Ticks, 60, Funcx);
        }

        void BeforSnd1(MsgWithMobile data)
        {
            if (data == null) return;
            OpType OP = new OpType();


            #region 单灯

            if (data.WstSluRightOperator != null) //单灯 发送命令
            {
                foreach (var f in data.WstSluRightOperator.OperatorItems)
                {
                    int sluid = f.SluId;
                    //int addrtype = f.AddrType;
                    //int addr = f.Addr;

                    string showx = ""; //  f.AddrType
                    if (f.AddrType == 0) showx = "全部";
                    else if (f.AddrType == 1) showx = "组:" + f.Addr;
                    else if (f.AddrType == 3) showx = "控制器:" + f.Addr;
                    else if (f.AddrType == 2)
                    {
                        if (f.Addr == 10) showx = "全部";
                        else if (f.Addr == 20) showx = "双数";
                        else if (f.Addr == 21) showx = "单数";
                        else showx = "其他模式";

                    }

                    var opName = "未知";
                    if (f.CmdType == 5)
                    {
                        opName = "调光：" + f.CmdPwmField.Scale + "%";

                    }
                    else
                    {
                        opName = "开关灯";
                        foreach (var g in f.CmdMix)
                        {
                            if (g == 1)
                            {
                                opName = "开灯";
                                break;
                            }

                            if (g == 4)
                            {
                                opName = "关灯";
                                break;
                            }
                        }

                    }

                    var rc = GerRecord(sluid, opName, showx);
                    rc.Addr = f.Addr;
                    rc.Gid = data.Head.Gid;
                    rc.AddrType = f.AddrType;
                    this.Records.Insert(0, rc);
                    recalctrlinfo();
                }

                return;

            }

            if (data.WstSluMeasure != null) //单灯 发送命令
            {



                var opName = "未知";
                int sluid = data.WstSluMeasure.SluId;
                ;
                string showx = "---";
                if (data.WstSluMeasure.Type == 0)
                {
                    opName = "巡测";
                }
                else if (data.WstSluMeasure.Type == 5 || data.WstSluMeasure.Type == 7)
                {
                    opName = "基本参数";
                }

                var rc = GerRecord(sluid, opName, showx);
                rc.AddrType = data.WstSluMeasure.Type;
                this.Records.Insert(0, rc);
                recalctrlinfo();
            }

            if (data.WstSluReadCtrlArgs != null)
            {
                int sluid = data.WstSluReadCtrlArgs.SluId;
                int ctrlid = data.WstSluReadCtrlArgs.CtrlId;
                string showx = "控制器:" + ctrlid;
                if (data.WstSluReadCtrlArgs.ReadCtrldata || data.WstSluReadCtrlArgs.ReadData)
                {
                    var rc = GerRecord(sluid, "选测", showx);
                    rc.Addr = ctrlid;
                    rc.AddrType = 5;
                    this.Records.Insert(0, rc);
                    recalctrlinfo();
                }
            }

            #endregion




            #region rtu

            if (data.WstRtuOrders != null)
            {
                if (data.WstRtuOrders.Op == 22) //召测时钟
                {
                    OP = OpType.ZcTime;
                }

                if (data.WstRtuOrders.Op == 31) //选测终端数据
                {
                    OP = OpType.RtuMeasure;
                }

                if (data.WstRtuOrders.Op == 21) //发送对时
                {
                    OP = OpType.AsynTime;
                }

                if (data.WstRtuOrders.Op == 41) //发送节假日
                {
                    OP = OpType.SndHoliday;
                }

                if (data.WstRtuOrders.Op == 11) //发送周设置
                {
                    OP = OpType.Snd13Week;
                }

                if (data.WstRtuOrders.Op == 12) //发送周设置
                {
                    OP = OpType.Snd46Week;
                }

                if (data.WstRtuOrders.Op == 13) //发送周设置
                {
                    OP = OpType.Snd78Week;
                }

                if (data.WstRtuOrders.Op == 14) //发送周设置
                {
                    OP = OpType.SndAllYearWeek;
                }

                if (data.WstRtuOrders.RtuIds.Count < 1) return;
                int rtuId = data.WstRtuOrders.RtuIds[0];
                AddNewRecordItem(rtuId, OP);
                return;

            }

            if (data.WstRtuCntOrderOpenCloseLight != null)
            {
                if (data.WstRtuCntOrderOpenCloseLight.IsOpen == 1)
                {
                    OP = OpType.RtuOpen;
                }

                if (data.WstRtuCntOrderOpenCloseLight.IsOpen == 2)
                {
                    OP = OpType.RtuClose;
                }

                int rtuId = 0;
                if (data.Args.Addr.Count > 0)
                {
                    rtuId = data.Args.Addr[0];
                }

                List<int> loopid = data.WstRtuCntOrderOpenCloseLight.Loops;
                foreach (var i in loopid)
                {
                    if (i == 0) //全部回路操作,只有开关灯操作  。其他没有回路参数的，默认为-1
                    {
                        int switchOutCount = 6; //默认为6个开关量
                        if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
                            return;
                        var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];
                        if (info.RtuModel == EnumRtuModel.Wj3006) switchOutCount = 8;
                        for (int j = 1; j < switchOutCount + 1; j++)
                        {
                            AddNewRecordItem(rtuId, OP, j);
                        }
                    }
                    else
                    {
                        AddNewRecordItem(rtuId, OP, i);
                    }

                }

                return;
            }

            #endregion



        }

        void Funcx(object obj)
        {
            try
            {
                ClearRecordItem();
                //60 s  
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }


        private bool _sIsChecked;

        public bool IsChecked
        {
            get { return _sIsChecked; }
            set
            {
                if (value != _sIsChecked)
                {
                    _sIsChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        //private void FundEventHandlers(PublishEventArgs args)
        //{

        //    try{

        //        if (args.EventType == PublishEventType.Core)
        //        {
        //            if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.UserOperateRtu)
        //            {
        //                if (args.GetParams().Count < 4) return;

        //                int rtuid = (int)args.GetParams()[0];
        //                OpType op = (OpType) args.GetParams()[1];
        //                int loopid =(int) args.GetParams()[2];;
        //                if (op == OpType.SndWeek) return;
        //                EnumRtuModel rtuModel = (EnumRtuModel)args.GetParams()[3];
        //                if (rtuModel == EnumRtuModel.Wj3005 || rtuModel == EnumRtuModel.Wj3006)
        //                {
        //                    if (loopid == 0) //全部回路操作,只有开关灯操作  。其他没有回路参数的，默认为-1
        //                    {
        //                        int switchOutCount = 6; //默认为6个开关量
        //                        //if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid))
        //                        //    return;
        //                        //var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid ];
        //                        if (rtuModel == EnumRtuModel.Wj3006) switchOutCount = 8;
        //                        for (int i = 1; i < switchOutCount + 1; i++)
        //                        {
        //                            var tu = new Tuple<int, int, int,int>(rtuid, (int) op, i,-1);
        //                            if (userOpDic.ContainsKey(tu))
        //                            {
        //                                userOpDic[tu] = 0;
        //                            }
        //                            else
        //                            {
        //                                userOpDic.Add(tu, 0);
        //                            }
        //                            AddNewRecordItem(rtuid, op, i);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        var tu = new Tuple<int, int, int,int>(rtuid, (int)op, loopid,-1);
        //                        if (userOpDic.ContainsKey(tu))
        //                        {
        //                            userOpDic[tu] = 0;
        //                        }
        //                        else
        //                        {
        //                            userOpDic.Add(tu, 0);
        //                        }
        //                        AddNewRecordItem(rtuid, op, loopid);
        //                    }
        //                }


        //                if (rtuModel == EnumRtuModel.Wj2090)
        //                {
        //                    int opPara = -1;
        //                    if (args.GetParams().Count ==5) opPara   = (int)args.GetParams()[4];

        //                    var tu = new Tuple<int, int, int,int>(rtuid, (int)op, loopid,opPara);
        //                    if (userOpDic.ContainsKey(tu))
        //                    {
        //                        userOpDic[tu] = 0;
        //                    }
        //                    else
        //                    {
        //                        userOpDic.Add(tu, 0);
        //                    }

        //                    AddNewRecordSluItem(rtuid, op, loopid, opPara);


        //                }


        //                //if(op==OpType.SndWeek)
        //                //{
        //                //     ExWeekSetCount.Clear();
        //                //     if (ExWeekSetCount.ContainsKey(rtuid) == false) ExWeekSetCount.TryAdd(rtuid , 0);
        //                //}                
        //            }
        //        }
        //    }
        //    catch (Exception xe)
        //    {
        //        WriteLog.WriteLogError("UserOperate error in FundEventHandlers:ex:" + xe);
        //    }

        //}

        //     private bool isload = false;
        private bool _isfilter;
        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        //private bool FundOrderFilter(PublishEventArgs args)
        //{
        //    return _isfilter;

        //}
        //public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
        //    }
        //    return false;
        //}

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,//.wlst_svr_ans_cnt_request_snd_rtu_time,
                //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          ResponseSndWeekSetK1K3, typeof(UserOperateMsgViewModel), this, true);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_svr_ans_cnt_order_rtu_open_close_light,// .wlst_svr_ans_cnt_wj3090_order_open_close_light ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_opencloseLight ,
                ExExecuteOpenLight,
                typeof(UserOperateMsgViewModel), this, true);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_svr_ans_slu_right_operator,// .wst_slu_right_operator,
                // .wlst_svr_ans_cnt_wj2090_order_right_operator ,//.ClientPart.wlst_Wj2090svr_ans_clinet_right_operator_slu,
                SluMeasureBack,
                typeof(UserOperateMsgViewModel), this, true);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_svr_ans_slu_ctrl_measure,
                // .wlst_svr_ans_cnt_wj2090_order_auto_fe ,//.ClientPart.wlst_Wj2090_svr_to_clinet_slu_auto_fe,
                SluCtrlMeasure,
                typeof(UserOperateMsgViewModel), this, true);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_svr_ans_read_ctrl_args,
                CtrlArgsMeasure,
                typeof(UserOperateMsgViewModel), this, true);
        }


        //private void InitEvent()
        //{
        //    //ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxSys.wlst_sys_svr_to_cnt_info,//.ClientPart.wlst_Infrastructure_server_to_clinet_msginfo,
        //    //                              MsgAction, typeof(UserOperateMsgViewModel), this);



        //    //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.ActionAddShowInfo += AddNewRecordItem;


        //    //EventPublisher.AddEventSubScriptionTokener(
        //    //   Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        //}


        private void ExExecuteOpenLight(string session, Wlst.mobile.MsgWithMobile args)
        {
            var datax = args.WstRtuSvrAnsCntOrderOpenCloseLight;
            var lst = args.Args.Addr;
            OpType OpTemp;
            if (lst == null) return;
            if (datax.IsOpen)
            {
                OpTemp = OpType.RtuOpen;
            }
            else
            {
                OpTemp = OpType.RtuClose;
            }

            //var tu = new Tuple<int, int, int,int>(datax.RtuId, (int)OpTemp, datax.LoopId,-1);

            //if(userOpDic.ContainsKey(tu))
            //{
            UpdateRecordItem(datax.RtuId, OpTemp, datax.LoopId);
            //    userOpDic[tu] = 1;

            //}

        }
        ////    private ConcurrentDictionary<int, int> ExWeekSetCount =
        ////new ConcurrentDictionary<int, int>();

        private void ResponseSndWeekSetK1K3(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (equinfo.Count == 0 || infos.WstRtuOrders == null || infos.WstRtuOrders.RtuIds.Count == 0) return;
            if (equinfo.ContainsKey(infos.WstRtuOrders.RtuIds[0]) == false) return;



            //if (_isViewShow == false) return;
            var datax = infos.WstRtuOrders;

            OpType OpTemp = new OpType();
            int loooop = -1;
            if (datax.Op == 31)   //选测
            {
                OpTemp = OpType.RtuMeasure;
                foreach (var item in datax.Items)
                {
                    //var tu = new Tuple<int, int, int,int>(item.RtuId, (int)OpTemp, loooop,-1);
                    //if (userOpDic.ContainsKey(tu))
                    //{
                    UpdateRecordItem(item.RtuId, OpTemp, loooop);
                    //userOpDic[tu] = 1;
                    //}
                }
            }
            if (datax.Op == 21) //对时
            {
                OpTemp = OpType.AsynTime;
                foreach (var g in datax.RtuIds)
                {
                    UpdateRecordItem(g, OpTemp, loooop);
                }

                //foreach (var item in datax.Items)
                //{
                //    //var tu = new Tuple<int, int, int,int>(item.RtuId, (int)OpTemp, loooop,-1);
                //    //if (userOpDic.ContainsKey(tu))
                //    //{
                //        UpdateRecordItem(item.RtuId, OpTemp, loooop);
                //    //    userOpDic[tu] = 1;
                //    //}
                //}   
            }
            if (datax.Op == 22)//召测时间
            {
                OpTemp = OpType.ZcTime;

                foreach (var g in datax.RtuIds)
                {
                    UpdateRecordItem(g, OpTemp, loooop, datax.Date);
                }
                //foreach (var item in datax.Items)
                //{
                //    //var tu = new Tuple<int, int, int,int>(item.RtuId, (int)OpTemp, loooop,-1);
                //    //if (userOpDic.ContainsKey(tu))
                //    //{
                //        UpdateRecordItem(item.RtuId, OpTemp, loooop, datax.Date);
                //    //    userOpDic[tu] = 1;
                //    //}
                //}   
            }
            if (datax.Op == 11)//周设置13
            {
                OpTemp = OpType.Snd13Week;

                foreach (var g in datax.RtuIds)
                {
                    UpdateRecordItem(g, OpTemp, loooop);
                }

            }
            if (datax.Op == 12)//周设置46
            {
                OpTemp = OpType.Snd46Week;

                foreach (var g in datax.RtuIds)
                {
                    UpdateRecordItem(g, OpTemp, loooop);
                }

            }
            if (datax.Op == 13)//周设置78
            {
                OpTemp = OpType.Snd78Week;

                foreach (var g in datax.RtuIds)
                {
                    UpdateRecordItem(g, OpTemp, loooop);
                }

            }
            if (datax.Op == 14)//周设置全年
            {
                OpTemp = OpType.SndAllYearWeek;

                foreach (var g in datax.RtuIds)
                {
                    UpdateRecordItem(g, OpTemp, loooop);
                }

            }

        }

        public void SluMeasureBack(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstSluSvrAnsRightOperator;
            if (info == null) return;
            if (info.Op == 1)
            {
                OpType OpTemp = new OpType();

                if (infos.Args.Addr.Count > 2)
                {
                    int sluid = infos.Args.Addr[0];
                    int addr = infos.Args.Addr[1];
                    int grid = infos.Args.Addr[2];
                    foreach (var g in Records)
                    {
                        if (g.Gid == infos.Head.Gid)
                        {
                            g.Nindex = info.Nindex;
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

                foreach (var g in Records)
                {
                    if (g.RtuId == info.SluId)
                    {
                        if (g.Nindex == info.Nindex)
                        {
                            if (g.Finish) break;
                            bool Succ = false;
                            if (info.Status == 0x3a) //成功
                            {
                                Succ = true;
                            }
                            else if (info.Status == 0x61) //成功
                            {
                                Succ = true;
                            }
                            else if (info.Status == 0x62) //成功
                            {
                                Succ = true;
                            }
                            //else if (info.Status == 0x63) //成功
                            //{
                            //    g.IsSuccessful = "失败";
                            //    g.AttachInfo = "硬件队列已满.";
                            //}
                            //else if (info.Status == 0x5a) //成功
                            //{
                            //    g.IsSuccessful = "失败";
                            //    g.AttachInfo = "数据错误.";
                            //}
                            //else
                            //{
                            //    g.IsSuccessful = "失败";
                            //    g.AttachInfo = "原因未知.";
                            //}

                            //if (dtime == null) dtime = DateTime.Now.ToString("HH:mm:ss");
                            if (Succ)
                            {
                                g.AnsTime = DateTime.Now.ToString("HH:mm:ss");
                                //  DateTime ts = Convert.ToDateTime(DateTime.Now );
                                TimeSpan tss = DateTime.Now.Subtract(g.OpTime);
                                g.TimeDifference = tss.TotalSeconds.ToString("f2") + " s";  //时差
                                g.Finish = true;
                            }
                            //g.AnsTime = dtime;//应答时间   DateTime.Now.ToString("HH:mm:ss") ;
                            //DateTime ts = Convert.ToDateTime(dtime);
                            //TimeSpan tss = ts.Subtract(g.OpTime);
                            //g.TimeDifference = tss.TotalSeconds + " s";  //时差


                        }
                    }
                }
            }


        }

        public void SluCtrlMeasure(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (ctrlinfo.Count == 0) return;
            var info = infos.WstSluSvrAnsSluMeasure;
            if (info == null) return;
            var sluids = (from t in ctrlinfo select t.Key.Item1).ToList();
            if (sluids.Contains(info.SluId) == false) return;

            var typs = (from t in ctrlinfo select t.Key.Item2).ToList();
            if (typs.Contains(info.Type) == false) return;



            foreach (var g in Records)
            {
                if (g.RtuId == info.SluId && g.AddrType == info.Type)
                {
                    if (info.Type == 0)
                    {
                        if (g.Finish) break;
                        g.AnsTime = DateTime.Now.ToString("HH:mm:ss");
                        // DateTime ts = Convert.ToDateTime(DateTime.Now);
                        TimeSpan tss = DateTime.Now.Subtract(g.OpTime);
                        g.TimeDifference = tss.TotalSeconds.ToString("f2") + " 秒"; //时差
                        g.Finish = true;
                        recalctrlinfo();
                    }
                    else if (info.Type == 5)
                    {
                        foreach (var f in info.InfoBaseic5)
                        {
                            if (f.Info.CtrlId == g.Addr)
                            {
                                if (g.Finish) break;
                                g.AnsTime = DateTime.Now.ToString("HH:mm:ss");
                                // DateTime ts = Convert.ToDateTime(DateTime.Now);
                                TimeSpan tss = DateTime.Now.Subtract(g.OpTime);
                                g.TimeDifference = tss.TotalSeconds.ToString("f2") + " 秒"; //时差
                                g.Finish = true;
                                recalctrlinfo();
                            }
                        }

                    }
                }
            }
        }

        public void CtrlArgsMeasure(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstSluSvrAnsReadCtrlArgs;//wstsvr_ans_slu_ctrl_measure;
            if (info == null) return;

            foreach (var g in Records)
            {
                if (g.RtuId == info.SluId && g.Addr == info.CtrlId)
                {
                    if (g.Finish) break;
                    g.AnsTime = DateTime.Now.ToString("HH:mm:ss");
                    // DateTime ts = Convert.ToDateTime(DateTime.Now);
                    TimeSpan tss = DateTime.Now.Subtract(g.OpTime);
                    g.TimeDifference = tss.TotalSeconds.ToString("f2") + " 秒";  //时差
                    g.Finish = true;
                }
            } recalctrlinfo();
        }

        #region tab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "用户操作"; }
        }

        #endregion

        private ObservableCollection<OperateItem> _records;

        /// <summary>
        /// 操作码+操作终端+操作回路 如222222+1+3  操作码22222 1终端3回路
        /// 操作码+操作终端+操作集合 如 22222+1+135  操作码22222 1终端1、3、5回路
        /// </summary>
        public ObservableCollection<OperateItem> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<OperateItem>();
                return _records;
            }
        }

        //private OperatorRecordItem curr;
        //public OperatorRecordItem CurrentSelectItem
        //{
        //    get { return curr; }
        //    set
        //    {
        //        if (value == curr) return;
        //        curr = value;
        //        this.RaisePropertyChanged(() => this.CurrentSelectItem);
        //    }
        //}

        #region CmdDelete

        private DateTime _dtDelete;

        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExCmdDelete, CanExCmdDelete, true); }
        }

        private void ExCmdDelete()
        {
            _dtDelete = DateTime.Now;
            this.Records.Clear();
            recalctrlinfo();
        }

        private bool CanExCmdDelete()
        {
            return Records.Count > 0 && DateTime.Now.Ticks - _dtDelete.Ticks > 30000000;
        }

        #endregion


        /// <summary>
        /// 新增加新的用户操作信息
        /// </summary>
        /// <param name="rtuId">操作的终端地址</param>
        /// <param name="oper">操作类型 </param>
        /// <param name="loopid">操作回路 </param>
        public void AddNewRecordItem(int rtuId, OpType oper, int loopid = -1)
        {
            // if (isload == false) return;
            //return;
            if (IsChecked) return;
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId)) return;
            var rtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuName;
            foreach (var g in Records)
            {
                if (g.RtuId == rtuId && g.Operatr == oper && g.LoopId == loopid)
                {
                    Records.Remove(g);
                    break;
                }
            }
            var ins = new OperateItem()
            {
                RtuName = rtuName,
                RtuId = rtuId,
                Operatr = oper,
                OpTime = DateTime.Now,
                LoopId = loopid,
                TimeDifference = null,
                AnsTime = null,
                OpPara = -1,

            };
            Records.Insert(0, ins);
            CurrentItem = ins;





            if (Records.Count > 300)
            {
                ClearRecordItem();
            }


            recalctrlinfo();

        }

        /// <summary>
        /// 计算单灯操作的  
        /// </summary>
        void recalctrlinfo()
        {
            ctrlinfo.Clear();
            equinfo.Clear();
            foreach (var f in Records)
            {
                if (f.Finish) continue;
                if (f.AddrType == 0 || f.AddrType == 5)
                {
                    if (f.RtuId < 1599999 && f.RtuId > 1500000)
                    {
                        var key = new Tuple<int, int, int>(f.RtuId, f.AddrType, f.Addr);
                        if (ctrlinfo.ContainsKey(key)) continue;
                        ctrlinfo.TryAdd(key, 1);
                    }
                }
                if (equinfo.ContainsKey(f.RtuId) == false) equinfo.TryAdd(f.RtuId, 1);
            }
        }
        //快速索引   sluid   - type - addr
        private ConcurrentDictionary<Tuple<int, int, int>, int> ctrlinfo = new ConcurrentDictionary<Tuple<int, int, int>, int>();
        private ConcurrentDictionary<int, int> equinfo = new ConcurrentDictionary<int, int>();

        public OperateItem GerRecord(int rtuId, string opName, string loopName)
        {
            if (IsChecked) return null;
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId)) return null;
            var rtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuName;
            //foreach (var g in Records)
            //{
            //    if (g.RtuId == rtuId && g.Operatr == oper && g.LoopId == loopid)
            //    {
            //        Records.Remove(g);
            //        break;
            //    }
            //}
            var ins = new OperateItem()
            {
                RtuName = rtuName,
                RtuId = rtuId,
                // Operatr = oper,

                OperatrName = opName,
                OpTime = DateTime.Now,
                // LoopId = loopid,
                SwitchOutName = loopName,
                TimeDifference = null,
                AnsTime = null,
                //OpPara = -1,

            };
            return ins;
        }

        public void AddNewRecordSluItem(int rtuId, OpType oper, int ctrlid, int opPara = -1)
        {
            // if (isload == false) return;
            //return;
            if (IsChecked) return;
            string ctrlName = null;
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId)) return;
            var rtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuName;
            var t = EquipmentDataInfoHold.InfoItems[rtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (ctrlid == 1000)  //10 全部  21单数  20 双数 *100
            {
                ctrlName = "全部控制器";
            }
            else if (ctrlid == 2000)
            {
                ctrlName = "双数控制器";
            }
            else if (ctrlid == 2100)
            {
                ctrlName = "单数控制器";
            }
            else if (ctrlid == -1)
            {
                ctrlName = "---";
            }
            else
            {
                if (!t.WjSluCtrls.ContainsKey(ctrlid)) return;
                ctrlName = t.WjSluCtrls[ctrlid].RtuName;
            }

            foreach (var g in Records)
            {
                if (g.RtuId == rtuId && g.Operatr == oper && g.LoopId == ctrlid && g.OpPara == opPara)
                {
                    Records.Remove(g);
                    break;
                }
            }
            var ins = new OperateItem()
            {
                RtuName = rtuName,
                RtuId = rtuId,
                Operatr = oper,
                OpTime = DateTime.Now,
                LoopId = ctrlid,
                SwitchOutName = ctrlName,
                TimeDifference = null,
                AnsTime = null,
                OpPara = opPara,


            };
            if (opPara >= 0) ins.OperatrName = ins.OperatrName + " " + opPara + "0%调光";
            Records.Insert(0, ins);
            CurrentItem = ins;

            if (Records.Count > 100)
            {
                Records.Clear();
            } recalctrlinfo();
        }


        public void UpdateRecordItem(int rtuId, OpType oper, int loopid, string dtime = null)
        {
            // if (isload == false) return;
            //return;
            if (IsChecked) return;


            //  if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId)) return;

            foreach (var g in Records)
            {
                if (g.RtuId == rtuId && g.LoopId == loopid && g.Operatr == oper)
                {
                    if (g.Finish) return;
                    //if (dtime == null) dtime = DateTime.Now.ToString("HH:mm:ss");
                    g.AnsTime = dtime;//应答时间   DateTime.Now.ToString("HH:mm:ss") ;
                    //DateTime ts = Convert.ToDateTime(dtime);
                    TimeSpan tss = DateTime.Now.Subtract(g.OpTime);

                    g.TimeDifference = tss.TotalSeconds.ToString("f2") + " 秒";  //时差


                    if (oper == OpType.ZcTime)
                    {
                        g.TimeDifference = dtime;
                    }
                    g.Finish = true;
                }
            }
        }

        public void ClearRecordItem()
        {
            recalctrlinfo();
            // if (isload == false) return;
            //return;
            if (IsChecked) return;
            if (Records.Count == 0) return;
            for (int i = Records.Count - 1; i >= 0; i--)
            {
                bool isNeedRemove = false;
                TimeSpan ts = DateTime.Now.Subtract(Records[i].OpTime);
                if (Records[i].AnsTime == null)
                {
                    if (ts.TotalSeconds > 600) isNeedRemove = true;
                }
                else
                {
                    if (ts.TotalSeconds > 300) isNeedRemove = true;

                }
                if (isNeedRemove)
                {
                    var tus = new Tuple<int, int, int, int>(Records[i].RtuId, (int)Records[i].Operatr,
                                                            Records[i].LoopId, Records[i].OpPara);
                    if (userOpDic.ContainsKey(tus))
                        userOpDic.Remove(tus);
                    Records.RemoveAt(i); //如果没有应答，10分钟后清除
                }
                recalctrlinfo();
            }


            //foreach (var g in Records)
            //{
            //   //根据时差清空数据
            //    if(g.AnsTime ==null )
            //    {
            //        TimeSpan ts = DateTime.Now.Subtract(g.OpTime);
            //        if (ts.TotalSeconds > 1800) Records.Remove(g);  //如果没有应答，30分钟后清除
            //        userOpDic.Remove(new Tuple<int, int, int, int>(g.RtuId, (int) g.Operatr, g.LoopId, g.OpPara));
            //    }
            //    else
            //    {
            //        DateTime ts = Convert.ToDateTime(g.AnsTime );
            //        TimeSpan tss = ts.Subtract(g.OpTime);
            //        if (tss.TotalSeconds > 600) Records.Remove(g);//如果有应答了，10分钟后 清除
            //        userOpDic.Remove(new Tuple<int, int, int, int>(g.RtuId, (int) g.Operatr, g.LoopId, g.OpPara));
            //    }

            //}

        }



        private OperateItem _cr;

        public OperateItem CurrentItem
        {
            get { return _cr; }
            set
            {
                if (_cr != value)
                {
                    _cr = value;
                    this.RaisePropertyChanged(() => this.CurrentItem);
                }
            }
        }

        private Dictionary<Tuple<int, int, int, int>, int> userOpDic =
                new Dictionary<Tuple<int, int, int, int>, int>();


        private int count = 0;
        private DateTime dtLast = DateTime.Now;
        public void CurrentSelectItemDoubleClicked()
        {
            try
            {
                if (DateTime.Now.Ticks - dtLast.Ticks < 50000000) count++;
                else count = 0;
                if (count > 3) _isfilter = true;
                else _isfilter = false;
                dtLast = DateTime.Now;

                if (CurrentItem == null) return;
                if (CurrentItem.RtuId < 100) return;
                if (CurrentItem.RtuId < 1000000) return;
                //if (CurrentItem.RtuId > 1100000) return;
                //发布事件  选中当前节点
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                };
                args.AddParams(CurrentItem.RtuId);
                EventPublish.PublishEvent(args);
            }
            catch (Exception ex) { }
        }

    }
}
