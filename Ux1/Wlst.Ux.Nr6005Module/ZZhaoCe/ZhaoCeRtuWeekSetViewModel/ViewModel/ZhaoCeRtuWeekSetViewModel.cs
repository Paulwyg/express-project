using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.ViewModel
{
    [Export(typeof(IIZhaoCeRtuWeekSetViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuWeekSetViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIZhaoCeRtuWeekSetViewModel
    {
        public ZhaoCeRtuWeekSetViewModel()
        {
            InitAction();
        }


        private ObservableCollection<OneRtuZhaoCeTime> _rtusWeekSet;

        public ObservableCollection<OneRtuZhaoCeTime> RtusWeekSet
        {
            get
            {
                if (_rtusWeekSet == null)
                    _rtusWeekSet = new ObservableCollection<OneRtuZhaoCeTime>();
                return _rtusWeekSet;
            }
        }


        public void OnUserHideOrClosing()
        {
            this.RtusWeekSet.Clear();
        }

        private OneRtuZhaoCeTime _currentSelectedItem;

        public OneRtuZhaoCeTime CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set
            {
                if (_currentSelectedItem != value)
                {
                    _currentSelectedItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectedItem);
                }
            }
        }

        private string _msgbox;

        public string MsgBox
        {
            get { return _msgbox; }
            set
            {
                if (_msgbox != value)
                {
                    _msgbox = value;
                    this.RaisePropertyChanged(() => this.MsgBox);
                }
            }
        }

        private DateTime _beforeTime;

        public DateTime BeforeTime
        {
            get { return _beforeTime; }
            set
            {
                if (_beforeTime != value)
                {
                    _beforeTime = value;
                    this.RaisePropertyChanged(() => this.BeforeTime);
                }
            }
        }
        private void AddRtuZhaoceInfo(int rtuId,List< Wlst .client .ZhaoCeInfo .ZhaoCeOneLoopOneWeekTime> info,List<List<List< Wlst .client .ZhaoCeInfo .ZhaoCeWeekSetYear>>>infoyear,int mode)
        {



            var grporrtuid = 0;
            var areaid = new int();

            if (mode == 1)
            {   
                grporrtuid = IsGrp(rtuId).Item1;
                areaid = IsGrp(rtuId).Item2;
                var dic = Wlst.Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(areaid,grporrtuid);
                if (dic == null)
                {
                    dic=new Dictionary<int, int>();
                    dic.Add(10,0);
                }
                var timetale=new Dictionary<int, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
                for (int i = 1; i < 9; i++)
                {
                    if (dic.ContainsKey(i))
                    {
                        var t = Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(areaid,dic[i]);
                        timetale.Add(i,t); 
                    }
                }

                var par = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];

                var fllg = false;
                if (par.RtuModel == EnumRtuModel.Wj3005 || par.RtuModel == EnumRtuModel.Wj3090)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (timetale.ContainsKey(i) && fllg == false)
                        {
                            foreach (var tt in timetale[i].RuleItems)
                            {
                                if (tt.TimetableSectionId > 1)
                                {
                                    fllg = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (fllg)
                {
                    MsgBox = "该3005终端绑定多段时间表。";
                }
                else
                {
                    MsgBox = "若字体颜色为红色，则说明该回路时间表与主台时间表不一致。";
                }

                if (RtusWeekSet != null)
                {
                    if (DateTime.Now > BeforeTime.AddMinutes(1))
                    {
                        RtusWeekSet.Clear();

                        var fff = new OneRtuZhaoCeTime(rtuId, info, timetale, fllg);
                        this.RtusWeekSet.Add(fff);
                        CurrentSelectedItem = RtusWeekSet.First();
                        BeforeTime = DateTime.Now;
                    }
                    else
                    {
                        foreach (var t in RtusWeekSet)
                        {
                            if (t.RtuId == rtuId)
                            {
                                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];
                                int type = 0;

                                if (para.RtuModel == EnumRtuModel.Wj3090) type = 6;
                                else if (para.RtuModel == EnumRtuModel.Wj3005) type = 6;
                                else if (para.RtuModel == EnumRtuModel.Wj3006) type = 8;


                                foreach (var f in info)
                                {
                                    if (t.WeekTime.Count <= type)
                                    {
                                        if (timetale.ContainsKey(f.LoopId))
                                        {
                                            t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(f, timetale[f.LoopId], fllg));
                                        }
                                        else
                                        {
                                            t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(f,
                                                                                           new TimeTableInfoWithRtuOrGrpBandingInfo
                                                                                               .TimeTableItem(), fllg));
                                        }
                                    }
                                    //if (t.WeekTime.Count >= type) t.WeekTime.Clear();

                                    //if (timetale.ContainsKey(f.LoopId))
                                    //{
                                    //    t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(f, timetale[f.LoopId], fllg));
                                    //}
                                    //else
                                    //{
                                    //    t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(f,
                                    //                                                   new TimeTableInfoWithRtuOrGrpBandingInfo
                                    //                                                       .TimeTableItem(), fllg));
                                    //}

                                }
                                CurrentSelectedItem = t;
                                return;
                            }
                        }
                    }
                }
                RtusWeekSet.Clear();

                var ff = new OneRtuZhaoCeTime(rtuId, info, timetale, fllg);
                this.RtusWeekSet.Add(ff);
                CurrentSelectedItem = RtusWeekSet.First();
                BeforeTime = DateTime.Now;


            }
            else if (mode == 4)
            {
                List<int> maxsection = new List<int>();
                int maxsectionid = 0;
                for (int i = 0; i < infoyear.Count; i++)
                {
                    maxsection.Add(0);
                    for (int j = 0; j < 7; j++)
                    {
                        foreach (var t in infoyear[i][j])
                        {
                            if (t.SectionId>maxsection[i])
                            {
                                maxsection[i] = t.SectionId;
                            }
                        }
                    }

                }
                maxsectionid = maxsection.Max();


                grporrtuid = IsGrp(rtuId).Item1;
                areaid = IsGrp(rtuId).Item2;
                var dic = Wlst.Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(areaid,grporrtuid);
                if (dic == null)
                {
                    dic = new Dictionary<int, int>();
                    dic.Add(10, 0);
                }
                var timetale = new Dictionary<int, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
                for (int i = 1; i < 9; i++)
                {
                    if (dic.ContainsKey(i))
                    {
                        var t = Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(areaid, dic[i]);
                        timetale.Add(i, t);
                    }
                }

                foreach (var t in RtusWeekSet)
                {
                    if (t.RtuId == rtuId)
                    {
                        t.WeekTime.Clear();
                        for(int i=0;i<8;i++)
                        {
                            if (timetale.ContainsKey(i+1))
                            {
                                t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(infoyear[i], maxsectionid, timetale[i + 1],i));
                            }
                            else
                            {
                                t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(infoyear[i], maxsectionid, new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem(),i));
                            }
                            
                        }
                        CurrentSelectedItem = t;
                        return;
                    }
                }

               RtusWeekSet.Clear();
               var ff = new OneRtuZhaoCeTime(rtuId, infoyear,maxsectionid,timetale);
               this.RtusWeekSet.Add(ff);
               CurrentSelectedItem = RtusWeekSet.First();
            }


            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuWeekSetViewIdFor4, true);



            //todo

        }

        private Tuple<int,int> IsGrp(int rtuid)
        {
            var isgrp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuid);
            int grporrtuid = 0;
            int areaid = new int();
            if (isgrp == null)
            {
                grporrtuid = rtuid;
                //areaid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].AreaId;
                areaid = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuid);
            }
            else
            {
                grporrtuid = isgrp.Item2;
                //areaid = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoRtuBelong[rtuid].Item1;
                areaid = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuid);
            }
            var tu = new Tuple<int, int>(grporrtuid, areaid);
            return tu;
        }

        


        //private void AddRtuZhaoceInfo(int rtuId, List<Wlst.client.ZhaoCeInfo.ZhaoCeOneLoopOneWeekTime> info)
        //{
        //    Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
        //       Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuWeekSetViewId, true);
        //    foreach (var t in RtusWeekSet)
        //    {
        //        if (t.RtuId == rtuId)
        //        {
        //            foreach (var f in info.K4K6OpenCloseTime)
        //            {
        //                t.WeekTime.Add(new OneLoopOneWeekTimeViewModel(f));
        //            }
        //            CurrentSelectedItem = t;
        //            return;
        //        }
        //    }
        //    var ff = new OneRtuZhaoCeTime(rtuId, info);
        //    this.RtusWeekSet.Add(ff);
        //    CurrentSelectedItem = ff;

            
        //}


        #region delete current



        private ICommand _deleteCurrentCommand;

        public ICommand DeleteCurrentCommand
        {
            get
            {
                if (_deleteCurrentCommand == null)
                    _deleteCurrentCommand = new RelayCommand(ExDelete, CanExDelete,false );
                return _deleteCurrentCommand;
            }
        }

        private bool CanExDelete()
        {
            return this.RtusWeekSet.Count > 0;
        }

        private void ExDelete()
        {
            if (this.RtusWeekSet.Contains(this.CurrentSelectedItem))
            {
                this.RtusWeekSet.Remove(this.CurrentSelectedItem);
                if (this.RtusWeekSet.Count > 0) this.CurrentSelectedItem = this.RtusWeekSet[this.RtusWeekSet.Count - 1];
            }
            if (this.RtusWeekSet.Count < 1)
            {
                Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                    Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuWeekSetViewIdFor4, false);
            }
        }

        #endregion
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
            get { return "召测周设置"; }
        }


    }

    public partial class ZhaoCeRtuWeekSetViewModel
    {
  

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_zc_rtu_info ,// .wlst_svr_ans_cnt_request_zc_rtu_k1k3 ,//.ClientPart.wlst_ZhaoCeRtuWeekSet_server_ans_clinet_order_ZcRtuWeekSetInfoK1K3  ,
                ZcRtuWeekSetK1K3Info,
                typeof(ZhaoCeRtuWeekSetViewModel), this,true);

            //ProtocolServer.RegistProtocol(
            //   Wlst.Sr.ProtocolPhone.ClientListen.wlst_svr_ans_cnt_request_zc_rtu_k4k6,//.ClientPart.wlst_ZhaoCeRtuWeekSet_server_ans_clinet_order_ZcRtuWeekSetInfoK4K6 ,
            //   ZcRtuWeekSetK4K6Info,
            //   typeof(ZhaoCeRtuWeekSetViewModel), this);
        }

        //public void NavInitBeforShow(params object[] parsObjects)
        //{
        //    throw new NotImplementedException();
        //}

        internal class OneDayOneSecionTime
        {
            public int Day;
            public int LoopId;
            public int SectionId;

            public int OpenTime;
            public int CloseTime;

            public bool IsSameWithSet;
        }

        private int WeekSetCount = 0;
        private int WeekSetAnsCount = 0;
        private int RtuIdTmp = 0;
        public void ZcRtuWeekSetK1K3Info(string session, Wlst .mobile .MsgWithMobile  args)
        {

            try
            {
                if (args == null || args.WstRtuZcInfo == null) return;
                if (args.WstRtuZcInfo.Op > 5 || args.WstRtuZcInfo.Op<1) return;
                int rtuId = args.WstRtuZcInfo.RtuId;
                var rtuZhaoCeInfo = new List<ZhaoCeInfo.ZhaoCeOneLoopOneWeekTime>(args.WstRtuZcInfo.Times);
                var rtuTimesYear = new List<List<List<ZhaoCeInfo.ZhaoCeWeekSetYear>>>();


                //lvf 如果不是本客户端操作的 不处理 2018年8月9日09:34:11
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(rtuId) == false) return;
                System.TimeSpan ts = DateTime.Now - Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[rtuId];
                if (ts.Minutes > 5)
                {
                    //可以不清除,但可能占用内存
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(rtuId);
                    return;
                }


                if (args.WstRtuZcInfo.Op == 4)
                {
                    var rtutimesyear = args.WstRtuZcInfo.TimesYear;
                    if (rtutimesyear == null) return;


                    ////loop -  sectionid - day and time
                    //var dic = new Dictionary<int, Dictionary<int, List<OneDayOneSecionTime>>>();
                    //foreach (var f in rtutimesyear)
                    //{
                    //    if (dic.ContainsKey(f.LoopId) == false)
                    //        dic.Add(f.LoopId, new Dictionary<int, List<OneDayOneSecionTime>>());
                    //    if (dic[f.LoopId].ContainsKey(f.SectionId) == false) dic[f.LoopId][f.SectionId].Add(f);
                    //}

                    //var dicSet=new Dictionary< int ,Dictionary<int, List< ZhaoCeInfo .ZhaoCeWeekSetYear >>>();

                  

                    for (int ii = 0; ii < 8; ii++)
                    {
                        rtuTimesYear.Add(new List<List<ZhaoCeInfo.ZhaoCeWeekSetYear>>());
                        for (int i = 0; i < 7; i++)
                        {
                            rtuTimesYear[ii].Add(new List<ZhaoCeInfo.ZhaoCeWeekSetYear>());
                            foreach (var t in rtutimesyear)
                            { 
                                //var weekstart = DateTime.Now.AddDays(i -(int) DateTime.Now.DayOfWeek).ToString("yy-MM-dd");
                                var weekstart = DateTime.Now.AddDays(i - (int)DateTime.Now.DayOfWeek).ToString("MM-dd");
                                var date = new DateTime(t.Date);
                                var flg = true;
                                if (date.ToString("MM-dd") == weekstart && t.LoopId == ii + 1) //if (date.ToString("yy-MM-dd") == weekstart && t.LoopId == ii + 1)
                                {
                                    rtuTimesYear[ii][i].Add(t);
                                }
                            }
                            rtuTimesYear[ii][i] = (from t in rtuTimesYear[ii][i] orderby t.SectionId select t).ToList();
                        }
                    }
                    
                    this.AddRtuZhaoceInfo(rtuId, rtuZhaoCeInfo, rtuTimesYear, 4);

                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(rtuId);

                }
                else
                {
                    if (rtuZhaoCeInfo == null) return;
                    this.AddRtuZhaoceInfo(rtuId, rtuZhaoCeInfo, rtuTimesYear, 1);
                } 

                    string name = "未解析名称";
                    var rtuInfomation =
                               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                                   InfoItems [rtuId];
                    if (rtuInfomation != null)
                    {
                        name = rtuInfomation.RtuName;
                    }
                     Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                        rtuId, name , Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.ServerReply, "终端召测周设置数据");
               
               
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        //public void ZcRtuWeekSetK4K6Info(string session,Wlst .mobile .MsgWithMobile args)
        //{
        //    try
        //    {
        //        var lst = args.Args .Addr ;
        //        if (lst == null) return;
        //        int rtuId = lst.ElementAt(0);

        //        var rtuZhaoCeInfo = args.WstSvrAnsCntZcRtuWeeksetK4k6 ;
        //        if (rtuZhaoCeInfo == null) return;
        //        this.AddRtuZhaoceInfo(rtuId, rtuZhaoCeInfo);
                
        //        string name = "未解析名称";
        //        var rtuInfomation =
        //                   Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
        //                       EquipmentInfoDictionary[rtuId];
        //        if (rtuInfomation != null)
        //        {
        //            name = rtuInfomation.RtuName;
        //        }
        //         Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
        //            rtuId, name , Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.ServerReply, "终端召测周设置数据");
        //    }
        //    catch (Exception ex)
        //    {
        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
        //    }
        //}


    }

}
