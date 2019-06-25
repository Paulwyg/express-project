using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{
    public class OneGrpOrRtuLoopsSet : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public bool IsGroup;

        public OneGrpOrRtuLoopsSet(int areaid,int grpOrRtuId,bool isgrp)
        {
            AreaId = areaid;
            RtuOrGrpId = grpOrRtuId;
            if (isgrp == false) //rtu
            {
                    Msg = "终端";
                    var tmp =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[grpOrRtuId];

                    //var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetShowFidPhyAndNameByRtuId(grpOrRtuId);

                    if (tmp == null)
                    {
                        PhyId = RtuOrGrpId + "";
                        RtuOrGrpName = RtuOrGrpId + "";
                        PhyIdMsg = PhyId + "(" +Msg + ")";
                    }
                    else
                    {
                        PhyId = tmp.RtuPhyId.ToString("d4");
                        RtuOrGrpName = tmp.RtuName;
                        PhyIdMsg = PhyId + "(" + Msg + ")";
                    }

                    if (tmp.RtuModel == EnumRtuModel.Wj3005 || tmp.RtuModel == EnumRtuModel.Wj3090)
                    {
                        Has3005 = true;
                        Has3006 = false;
                    }
                    else
                    {
                        Has3005 = false;
                        Has3006 = true;
                    }
            }
            else
            {
                Msg = "组";
                var tu = new Tuple<int, int>(areaid, grpOrRtuId);
                if (Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                {
                    var tmp =
                        Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[tu];
                    RtuOrGrpName = tmp.GroupName;
                    PhyId = tmp.GroupId.ToString("d2");
                    PhyIdMsg = PhyId + "(" + Msg + ")";


                    Has3005 = false;
                    Has3006 = false;

                    foreach (var t in tmp.LstTml)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                        {
                            var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                            if (info != null)
                            {
                                if (info.RtuModel == EnumRtuModel.Wj3005 || info.RtuModel == EnumRtuModel.Wj3090)
                                {
                                    Has3005 = true;
                                }
                                else 
                                {
                                    Has3006 = true;
                                }
                            }
                            if (Has3005&&Has3006) continue;
                        }
                    }
                }
            }
            FristLoadTimeTableInfo(isgrp,Has3005);
        }

        private void FristLoadTimeTableInfo(bool isgrp,bool has3005)
        {
            for (int k = 1; k < 9; k++)
            {
                int timetableid = 
                    Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(AreaId,RtuOrGrpId, k);
                if (timetableid < 1)
                {
                    Items.Add(new TimeInfoName()
                                  {TimeTableName = "-", TimeTableNameDescriotion = "未绑定-双击此处进行绑定操作", TimeTalbe = -1});
                    continue;
                }

                var tmp = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(AreaId,timetableid);
                if (tmp == null || (isgrp == false && has3005 && k > 6))
                {
                    Items.Add(new TimeInfoName()
                                  {TimeTableName = "-", TimeTableNameDescriotion = "未绑定-双击此处进行绑定操作", TimeTalbe = -1});
                    continue;
                }
                Items.Add(new TimeInfoName()
                              {
                                  TimeTableNameDescriotion = tmp.TimeDesc,
                                  TimeTalbe = tmp.TimeId,
                                  TimeTableName = tmp.TimeName
                              });
            }

        }


        private ObservableCollection<TimeInfoName> _items = null;

        public ObservableCollection<TimeInfoName> Items
        {
            get
            {
                if (_items == null) _items = new ObservableCollection<TimeInfoName>();
                return _items;
            }
        }

        public void UpdateTimeInfo(int kx, int timetableid, string timetablename, string timetabledescription)
        {
            if (Items.Count < kx) return;
            if (timetableid < 1)
            {
                Items[kx - 1].TimeTableNameDescriotion = "未绑定-双击此处进行绑定操作";
                Items[kx - 1].TimeTableName = "-";
                Items[kx - 1].TimeTalbe = -1;
            }
            else
            {
                Items[kx - 1].TimeTableNameDescriotion = timetabledescription;
                Items[kx - 1].TimeTableName = timetablename;
                Items[kx - 1].TimeTalbe = timetableid;
            }
        }

        #region

        private string _iphyd;

        public string PhyId
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


        private int _rtuid;

        public int RtuOrGrpId
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid != value)
                {
                    _rtuid = value;
                    this.RaisePropertyChanged(() => this.RtuOrGrpId);
                }
            }
        }


        private string _rtuname;

        public string RtuOrGrpName
        {
            get { return _rtuname; }
            set
            {
                if (_rtuname != value)
                {
                    _rtuname = value;
                    this.RaisePropertyChanged(() => this.RtuOrGrpName);
                }
            }
        }

        private string _phyidmsg;

        public string PhyIdMsg
        {
            get { return _phyidmsg; }
            set
            {
                if (_phyidmsg != value)
                {
                    _phyidmsg = value;
                    this.RaisePropertyChanged(() => this.PhyIdMsg);
                }
            }
        }


        private string _rmsg;

        public string Msg
        {
            get { return _rmsg; }
            set
            {
                if (_rmsg != value)
                {
                    _rmsg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }
            }
        }

        private bool _has3005;

        public bool Has3005
        {
            get { return _has3005; }
            set
            {
                if (_has3005 != value)
                {
                    _has3005 = value;
                    this.RaisePropertyChanged(() => this.Has3005);
                }
            }
        }

        private bool _has3006;

        public bool Has3006
        {
            get { return _has3006; }
            set
            {
                if (_has3006 != value)
                {
                    _has3006 = value;
                    this.RaisePropertyChanged(() => this.Has3006);
                }
            }
        }

        private int _areaid;

        public int AreaId
        {
            get { return _areaid; }
            set
            {
                if (_areaid != value)
                {
                    _areaid = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }
        #endregion
    }

    public class TimeInfoName : Wlst.Cr.Core.CoreServices.ObservableObject
    {


        /// <summary>
        /// k 时间表地址序号
        /// </summary>
        public int TimeTalbe;



        private string _k1TimeTableName;

        /// <summary>
        /// k 时间表名称
        /// </summary>
        public string TimeTableName
        {
            get { return _k1TimeTableName; }
            set
            {
                if (_k1TimeTableName != value)
                {
                    _k1TimeTableName = value;
                    this.RaisePropertyChanged(() => this.TimeTableName);

                }
            }
        }





        private string _k1TimeTableNameDescriotion;

        /// <summary>
        /// k 时间表描述
        /// </summary>
        public string TimeTableNameDescriotion
        {
            get { return _k1TimeTableNameDescriotion; }
            set
            {
                if (_k1TimeTableNameDescriotion != value)
                {
                    _k1TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.TimeTableNameDescriotion);

                }
            }
        }






    }
}
