using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wlst.client;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.StateBarModule.NewMsgViewBottom
{
    

    [Export(typeof(NewMsgViewBottomVm))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NewMsgViewBottomVm : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public NewMsgViewBottomVm()
        {
            InitAction();

            if (File.Exists("Config\\长沙.txt")) ischangsha = true;   //长沙专用屏蔽抄表成功
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

            IsStateBarShowPhyId = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 16, false);
            IsStateBarShowRtuName = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 17, false);
            IsStateBarShowGrpName = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 18, false);
            IsStateBarShowRemark = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 19, false);


            Wlst.Cr.Coreb.AsyncTask  .Qtz  .AddQtz("none", 8888, DateTime.Now.Ticks + 5000, 1, Updatetime);
        }

        private bool FundOrderFilter(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.Core)
            {
                switch (args.EventId)
                {
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:       
                        return true;
                    case  Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglMeasure:
                        return true;
                    case  Wlst .Sr .EquipmentInfoHolding .Services.EventIdAssign.RunningInfoUpdate2:
                        return true;

                }
            }
            return false;
        }
        private void FundEventHandler(PublishEventArgs args)
        {
            try
            {
                try
                {
                    if (args.EventType == PublishEventType.Core)
                    {


                        if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)//EventId =EventIdAssign.PushErrNums
                        {
                            var lst = new List<string>();
                            //DtRtuMsg = "";
                            int rtuId = Convert.ToInt32(args.GetParams()[0]);
                            if (rtuId > 0 &&
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(rtuId))
                            {
                                var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
                                if (tmps == null) return;
                                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 16, false))
                                    //DtRtuMsg += tmps.RtuPhyId+" - ";
                                    lst.Add(tmps.RtuPhyId + "");
                                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 17, false))
                                    //DtRtuMsg += tmps.RtuName + " - ";
                                    lst.Add(tmps.RtuName + "");
                                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 18, false))
                                {
                                    var groupidx =
                                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(
                                            rtuId);
                                    if (groupidx != null)
                                    {
                                        var infosss =
                                            Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.
                                                GetGroupInfomation(
                                                    groupidx.Item1, groupidx.Item2);
                                        if (infosss != null) lst.Add(infosss.GroupName);

                                        //  if (infosss != null) DtRtuMsg += infosss.GroupName + " - ";

                                    }
                                    else
                                    {
                                        lst.Add("特殊终端");
                                    }
                                }

                                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 19, false))
                                    if(string.IsNullOrEmpty(tmps.RtuRemark) ==false  )lst.Add(tmps.RtuRemark);
                                // DtRtuMsg += tmps.RtuRemark;// +"          ";

                                DtRtuMsg = "";
                                if(lst.Count>0)
                                {
                                    for (int i=0;i<lst .Count -1;i++)
                                    {
                                        DtRtuMsg += lst[i] + " - ";
                                    }
                                    DtRtuMsg += lst[lst.Count - 1] + "      ";
                                }
                            }
                        }
                        if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)//EventId =EventIdAssign.PushErrNums
                        {

                            if (args.GetParams().Count == 0) return;
                            var rtuids = args.GetParams()[0] as List<int>;
                            if (rtuids == null || rtuids.Count == 0) return;
                            foreach (var g in rtuids)
                            {
                                var rtuInfo =
                                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                if (rtuInfo == null) return;
                                if (rtuInfo.RtuModel == EnumRtuModel.Wj2090)
                                {
                                    DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + "获取 " + rtuInfo.RtuPhyId + "号 集中器:" + rtuInfo.RtuName + "  ,选测数据";

                                }
                            }
                           
                        }




                        //lvf 2018年6月5日11:29:36  中间层不愿意改，只能客户端监听相关设备时间，引用相关模块
                        if (args.EventId == Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglMeasure)
                        {
                            if (args.GetParams().Count == 0) return;
                            var rtuids = args.GetParams()[0] as List<int>;
                            if (rtuids == null || rtuids.Count == 0) return;

                            if (rtuids[0] > 0)
                            {
                                var rtuid = rtuids[0];
             
                                var name = "";

                                var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(rtuid);
                                if (para == null) return ;
                                name = para.OrderId +" - " + para.CtrlName;
                                var sluid = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(rtuid);
                                if (sluid != -1)
                                {
                                    var sluinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluid);
                                    if ( sluinfo!= null)
                                    {
                                        DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + "获取 集中器："+ sluinfo.PhyId+" "+sluinfo.FieldName+" ， 序号" + para.OrderId + "，" + para.CtrlName + "  控制器实时数据";
                                    }
                                }else
                                {
                                    DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + "获取  序号: " + para.OrderId +"-"+para.CtrlName + "  控制器实时数据";
                                }
                                

                            



                            }
                                
                            //else
                            //    DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + stratt + "  " + name + "  " + operatorContent;
                          
                        }
                    }
                    //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                    //{
                    //    var infos = args.GetParams()[1] as EquipmentPreFaultExChange;
                    //    if (infos == null) return;
                    //    OnPreDataBack(infos);
                    //}
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError(
                        "EquipmentDataQuery.EquipmentFaultRecordQueryViewModel FundEventHandler occer an error:" +
                        ex);
                }

            }
            catch (Exception ex)
            {
            }
        }

        void Updatetime(object obj)
        {
            DtTime = DateTime.Now.ToString("HH:mm:ss");
        }

        private static bool IsStateBarShowPhyId;
        private static bool IsStateBarShowRtuName;
        private static bool IsStateBarShowGrpName;
        private static bool IsStateBarShowRemark;

        private string _dttime;

        public string  DtTime
        {
            get { return _dttime; }
            set
            {
                if (value != _dttime)
                {
                    _dttime = value;
                    this.RaisePropertyChanged(() => this.DtTime);
                }
            }
        }

        private string _dtmsg;

        public string DtMsg
        {
            get { return _dtmsg; }
            set
            {
                if (value != _dtmsg)
                {
                    _dtmsg = value;
                    this.RaisePropertyChanged(() => this.DtMsg);
                }
            }
        }

        private string _dtRtumsg;

        public string DtRtuMsg
        {
            get { return _dtRtumsg; }
            set
            {
                if (value != _dtRtumsg)
                {
                    _dtRtumsg = value;
                    this.RaisePropertyChanged(() => this.DtRtuMsg);
                }
            }
        }
 



        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxSys.wlst_sys_svr_to_cnt_info,//.ClientPart.wlst_Infrastructure_server_to_clinet_msginfo,
                                          MsgAction, typeof(NewMsgViewBottomVm), this,true);
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.ActionAddShowInfo += AddNewRecordItem;



        }

        private void MsgAction(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.WstSysSvrToCntInfo == null) return;
            AddNewRecordItem(infos.WstSysSvrToCntInfo.RtuId, infos.WstSysSvrToCntInfo.RtuName, OperatrType.SystemInfo,
                             infos.WstSysSvrToCntInfo.Msg);
        }






        private bool ischangsha = false;//长沙专用屏蔽抄表成功
        /// <summary>
        /// 新增加新的用户操作信息
        /// </summary>
        /// <param name="rtuId">操作的终端地址</param>
        /// <param name="rtuName">终端 </param>
        /// <param name="operatr">用户操作还是服务器应答 </param>
        /// <param name="operatorContent">执行情况 如 完成或 等待 </param>
        public void AddNewRecordItem(int rtuId, string rtuName, OperatrType operatr, string operatorContent)
        {

            if(!string.IsNullOrEmpty(operatorContent) && operatorContent.Contains("抄表成功")&& ischangsha) return;
            //var ins = new OperatorRecordItem()
            //{
            //    RtuName = rtuName,
            //    RtuId = rtuId,
            //    Operatr = operatr,
            //    OperatorContent = operatorContent,
            //    OpTime = DateTime.Now
            //};
            //Records.Insert(0, ins);
            //CurrentItem = ins;

            //if (Records.Count > 100)
            //{
            //    Records.Clear();
            //}
            var stratt = "用户操作 ";
            if (operatr == OperatrType.ServerReply) stratt = "系统应答 ";
            else if (operatr == OperatrType.SystemInfo) stratt = "系统信息 ";

            string name = rtuName;
            int phyid = rtuId;



            if (rtuId > 0 && Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
            {

                //判断 区域 lvf 2018年6月25日11:13:55
                //Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR


                name = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuName;
                phyid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;

                //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuFid > 0 &&
                //    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                //        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuFid))
                //{
                //    name =
                //        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                //            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuFid].RtuName;
                //    // phyid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;
                //    //DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + stratt + "   " + phyid + "  " + strname + "  " +
                //    //        operatorContent;
                //}
                //else
                //{

                //    DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + stratt + "   " + phyid + "  " + strname + "  " +
                //            operatorContent;
                //}

            }
            if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info.ContainsKey(rtuId))
            {
                var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(rtuId);
                if (para != null)
                {
                    name = para.FieldName;
                    phyid = para.PhyId;
                }
            }

            if (phyid > 0)
                DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + stratt + "   地址:" + phyid + "  " + name + "  " +
                        operatorContent;
            else
                DtMsg = DateTime.Now.ToString("HH:mm:ss  ") + stratt + "  " + name + "  " + operatorContent;

        }






    }
}
