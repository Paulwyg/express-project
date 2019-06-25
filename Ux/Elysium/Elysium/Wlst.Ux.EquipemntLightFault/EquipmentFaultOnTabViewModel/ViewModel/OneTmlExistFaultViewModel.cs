using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipemntLightFault.Model;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel
{
    public class OneTmlExistFaultViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public OneTmlExistFaultViewModel()
        {


        }

        public OneTmlExistFaultViewModel(FaultInfoBase  error)
        {
            this.Id   = error.Id  ;
            this.FaultName = error.FaultName;
            this.FaultRemak = "报警";
        
            this.EquipmentNameOne = error.RtuName ;
            this.EquipmentNameTwo = error.RtuLoopName ;
            this.EquipmentNameTree = error.LampId +"";
            this.Color = error.Color;
            this.CreatTime = error.DateCreate .ToString( "yyyy-MM-dd HH:mm:ss") ;
            this.AlarmTimes = error.AlarmTimes;
            this.EquipmentRtuId = error.RtuId;
            this.EquipmentPhyId = error.RtuPhyId ;
            this.Count = error.AlarmCount;
           // this.AlarmsOrNotAlarm = "报警";
            DateCreateId = error.RecordId ;
            this.LoopId = error.LoopId;

        }


        public int LoopId;
        public long DateCreateId;
        /// <summary>
        /// 消警
        /// </summary>
        /// <param name="error"></param>
        public OneTmlExistFaultViewModel(OneTmlExistFaultViewModel error)
        {
            this.Id = error.Id;
            this.FaultName = error.FaultName;
            this.FaultRemak = "消警";

            this.EquipmentNameOne = error.EquipmentNameOne;
            this.EquipmentNameTwo = error.EquipmentNameTwo;
            this.EquipmentNameTree = error.EquipmentNameTree;
            this.Color = error.Color;
            this.CreatTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.AlarmTimes = error.AlarmTimes;
            this.EquipmentRtuId = error.EquipmentRtuId;
            this.EquipmentPhyId = error.EquipmentPhyId;
            this.Count = error.Count;
            // this.AlarmsOrNotAlarm = "报警";
        }

        #region attri

        private int _Id;

        /// <summary>
        /// 故障全局识别地址
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value != _Id)
                {
                    _Id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private int _IEquipmentId;

        /// <summary>
        /// 地址
        /// </summary>
        public int EquipmentRtuId
        {
            get { return _IEquipmentId; }
            set
            {
                if (value != _IEquipmentId)
                {
                    _IEquipmentId = value;
                    this.RaisePropertyChanged(() => this.EquipmentRtuId);
                }
            }
        }


        private int _IEquipmentPphyId;

        /// <summary>
        /// 地址
        /// </summary>
        public int EquipmentPhyId
        {
            get { return _IEquipmentPphyId; }
            set
            {
                if (value != _IEquipmentPphyId)
                {
                    _IEquipmentPphyId = value;
                    this.RaisePropertyChanged(() => this.EquipmentPhyId);
                }
            }
        }



        //private string  _alarmsss;

        ///// <summary>
        ///// 地址
        ///// </summary>
        //public string  AlarmsOrNotAlarm
        //{
        //    get { return _alarmsss; }
        //    set
        //    {
        //        if (value != _alarmsss)
        //        {
        //            _alarmsss = value;
        //            this.RaisePropertyChanged(() => this.AlarmsOrNotAlarm);
        //        }
        //    }
        //}

        private string _CreatTime;

        /// <summary>
        /// 发生时间
        /// </summary>
        public string CreatTime
        {
            get { return _CreatTime; }
            set
            {
                if (value != _CreatTime)
                {
                    _CreatTime = value;
                    this.RaisePropertyChanged(() => this.CreatTime);
                }
            }
        }

        private string _FaultName;

        /// <summary>
        /// 故障名称
        /// </summary>
        public string FaultName
        {
            get { return _FaultName; }
            set
            {
                if (value != _FaultName)
                {
                    _FaultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
                }
            }
        }

        private string _EquipmentNameOne;

        /// <summary>
        /// 终端地址一
        /// </summary>
        public string EquipmentNameOne
        {
            get { return _EquipmentNameOne; }
            set
            {
                if (value != _EquipmentNameOne)
                {
                    _EquipmentNameOne = value;
                    this.RaisePropertyChanged(() => this.EquipmentNameOne);
                }
            }
        }

        private string _EquipmentNameTwo;

        /// <summary>
        /// 终端地址二 可为回路地址名称
        /// </summary>
        public string EquipmentNameTwo
        {
            get { return _EquipmentNameTwo; }
            set
            {
                if (value != _EquipmentNameTwo)
                {
                    _EquipmentNameTwo = value;
                    this.RaisePropertyChanged(() => this.EquipmentNameTwo);
                }
            }
        }

        private string _EquipmentNameTree;

        /// <summary>
        /// 终端地址三 可为其他参数显示信息
        /// </summary>
        public string EquipmentNameTree
        {
            get { return _EquipmentNameTree; }
            set
            {
                if (value != _EquipmentNameTree)
                {
                    _EquipmentNameTree = value;
                    this.RaisePropertyChanged(() => this.EquipmentNameTree);
                }
            }
        }

        /// <summary>
        /// 语音报警次数 默认三次
        /// </summary>
        public int AlarmTimes { get; set; }


        /// <summary>
        /// 故障备注信息
        /// </summary>
        public string FaultRemak { get; set; }

        private string _Color;

        /// <summary>
        /// 报警数据显示颜色
        /// </summary>
        public string Color
        {
            get { return _Color; }
            set
            {
                if (value != _Color)
                {
                    _Color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }


        private int _count;
        /// <summary>
        /// 在一段时间内的报警次数
        /// </summary>
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    this.RaisePropertyChanged(() => this.Count);
                }
            }
        }
        #endregion
    }
}
