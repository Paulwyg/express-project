using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;


namespace Wlst.Ux.Nr6005Module.ZDataQuery.RtuOpenCloseLightQuery.Services
{
    public class RtuOpenCloseItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public RtuOpenCloseItem(Wlst .client .RtuOpenCloseLightCaleRecord .RtuOpenCloseLightRecordItem      info)
        {
            this.RtuId = info.RtuId ;
            this.RtuLoop = info.RtuLoop;
            this.RtuReplyTime = new DateTime(info.RtuReplyTime).ToString("yyy-MM-dd HH:mm:ss");
            this.RtuReplyType = info.RtuReplyType == 1
                                    ? "终端单命令应答"
                                    : info.RtuReplyType ==  2
                                          ? "终端组合命令应答"
                                          : info.RtuReplyType ==3
                                                ? "终端无应答，时间表时间"
                                                : info.RtuReplyType == 4
                                                      ? "节假日时间"
                                                      : "临时时间表时间";
            this.IsOpen = info.IsOpen ? "开灯" : "关灯";

        }
        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }
        /// <summary>
        /// 终端地址
        /// </summary>
        private int rtu_id;

        public int RtuId
        {
            get { return rtu_id; }
            set
            {
                if (rtu_id != value)
                {
                    rtu_id = value;
                    PhyId = value;
                    this.RaisePropertyChanged(() => this.RtuId);


                    if (
                       !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems .ContainsKey
                            (rtu_id))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems 
                            [rtu_id];
                    this.RtuName = tml.RtuName;
                    PhyId = tml.RtuPhyId ;
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
        /// <summary>
        /// 终端地址
        /// </summary>
        private string rtu_name;

        public string RtuName
        {
            get { return rtu_name; }
            set
            {
                if (rtu_name != value)
                {
                    rtu_name = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        /// <summary>
        /// 终端回路
        /// </summary>
        private int _rtuLoop;

        public int RtuLoop
        {
            get { return _rtuLoop; }
            set
            {
                if (_rtuLoop != value)
                {
                    _rtuLoop = value;
                    this.RaisePropertyChanged(() => this.RtuLoop);
                }
            }
        }

        /// <summary>
        /// 终端应答时间
        /// </summary>
        private string _rtuReplyTime;

        public string RtuReplyTime
        {
            get { return _rtuReplyTime; }
            set
            {
                if (_rtuReplyTime != value)
                {
                    _rtuReplyTime = value;
                    this.RaisePropertyChanged(() => this.RtuReplyTime);
                }
            }
        }

        /// <summary>
        /// 是否为开灯
        /// </summary>
        private string _ssOpen;

        public string IsOpen
        {
            get { return _ssOpen; }
            set
            {
                if (_ssOpen != value)
                {
                    _ssOpen = value;
                    this.RaisePropertyChanged(() => this.IsOpen);
                }
            }
        }

        /// <summary>
        /// 应答类型 1-单命令应答；2-复合命令应答；3-时间表开灯命令终端无应答记录时间表时间；4-节假日操作时间；5-临时时间表操作时间
        /// </summary>
        private string _rtuReplyType;

        public string RtuReplyType
        {
            get { return _rtuReplyType; }
            set
            {
                if (_rtuReplyType != value)
                {
                    _rtuReplyType = value;
                    this.RaisePropertyChanged(() => this.RtuReplyType);
                }
            }
        }


        public double RealTime;

        /// <summary>
        /// 计算出来的时间
        /// </summary>
        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    this.RaisePropertyChanged(() => this.Time);
                }
            }
        }
    }
}
