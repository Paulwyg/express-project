using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wlst.client;

namespace Wlst.Ux.Setting.EventTaskViewModel.ViewModel
{
    public class EventTaskItemViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
                                          
    {
        private string _taskName;

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            get { return _taskName; }
            set
            {
                if (value != _taskName)
                {
                    _taskName = value;
                    this.RaisePropertyChanged(() => this.TaskName);
                }
            }
        }

        private string _taskDes;

        /// <summary>
        /// 任务简介
        /// </summary>
        public string TaskDes
        {
            get { return _taskDes; }
            set
            {
                if (value != _taskDes)
                {
                    _taskDes = value;
                    this.RaisePropertyChanged(() => this.TaskDes);
                }
            }
        }



        private int _startMinutes;

        /// <summary>
        /// 任务起始时间 - Hour*60+minute
        /// </summary>
        public int StartMinutes
        {
            get { return _startMinutes; }
            set
            {
                if (value != _startMinutes)
                {
                    _startMinutes = value;
                    this.RaisePropertyChanged(() => this.StartMinutes);
                    
                }
            }
        }

        private int _interval;

        /// <summary>
        /// 任务执行间隔
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set
            {
                if (value != _interval)
                {
                    _interval = value;
                    this.RaisePropertyChanged(() => this.Interval);
                }
            }
        }

        private int _extraExamine;

        /// <summary>
        /// 补测次数
        /// </summary>
        [Range(0, 5, ErrorMessage = "补测次数介于0-5之间")]
        public int TimesRepartol
        {
            get { return _extraExamine; }
            set
            {
                if (value != _extraExamine)
                {
                    _extraExamine = value;
                    this.RaisePropertyChanged(() => this.TimesRepartol);
                }
            }
        }

        private int _minutesIntervalsRepartol;
        /// <summary>
        /// 补测间隔
        /// </summary>
        public int MinutesIntervalsRePartol
        {
            get { return _minutesIntervalsRepartol; }
            set
            {
                if (value != _minutesIntervalsRepartol)
                {
                    _minutesIntervalsRepartol = value;
                    this.RaisePropertyChanged(() => this.MinutesIntervalsRePartol);
                }
            }
        }
        

        private int _areaId;

        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (value != _areaId)
                {
                    _areaId = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }

        private int _equType;

        /// <summary>
        /// 设备类型
        /// </summary>
        public int EquType
        {
            get { return _equType; }
            set
            {
                if (value != _equType)
                {
                    _equType = value;
                    this.RaisePropertyChanged(() => this.EquType);
                }
            }
        }

        private bool _is2090;

        /// <summary>
        /// 设置抄表参数的可读性
        /// </summary>
        public bool Is2090
        {
            get { return _is2090; }
            set
            {
                if (value != _is2090)
                {
                    _is2090 = value;
                    this.RaisePropertyChanged(() => this.Is2090);
                }
            }
        }

        private bool _isLux;

        /// <summary>
        /// 设置光控巡测参数的可读性
        /// </summary>
        public bool IsLux
        {
            get { return _isLux; }
            set
            {
                if (value != _isLux)
                {
                    _isLux = value;
                    this.RaisePropertyChanged(() => this.IsLux);
                }
            }
        }

        public EventTaskItemViewModel(AttachEquPartolSchdule.AttachEquPartolSchduleItem taskinfo)
        {
            AreaId = taskinfo.AreaId;
            EquType = taskinfo.EquType;
            if (EquType == 1)
            {
                TaskName = "终端巡测";
                TaskDes = "终端巡测的默认巡测时间30分钟。对参数设置的要求是：巡测时间至少间隔为10分钟，不大于3小时；补测次数与补测间隔的乘积不得大于巡测间隔。";

            }
            if (EquType == 2)
            {
                TaskName = "单灯巡测";
                TaskDes = "单灯巡测的默认巡测时间30分钟。对参数设置的要求是：巡测时间至少间隔为10分钟，不大于3小时；补测次数与补测间隔的乘积不得大于巡测间隔。";
            }
            if (EquType == 3)
            {
                TaskName = "光控巡测";
                TaskDes = "对于485连接方式的光控默认巡测时间为2分钟,其它模式为主动上报。对参数设置的要求是：光控巡测任务至少为1分钟，不大于15分钟；补测次数与补测间隔的乘积不得大于巡测间隔。默认光控巡测任务参数不可修改。";
            }
            if (EquType == 4)
            {
                TaskName = "定时抄表";
                TaskDes = "若区域中存在电表设备则任务中存在电表抄表任务，默认小时设置为11点，补抄次数3次，补抄间隔15分钟。抄表任务将按照设定的小时每天在该小时时执行抄表。对抄表未成功的电表将在设定的间隔抄表时间执行补抄。补抄次数到达设定的补抄次数时停止补抄。抄表间隔时间至少为5分钟，设置的抄表时+间隔分钟*补抄次数 小于23小时。";
            }
            if (EquType == 5)
            {
                TaskName = "线路检测巡测";
                TaskDes = "线路检测巡测任务的默认巡测时间30分钟。对参数设置的要求是：巡测时间至少间隔为10分钟，不大于3小时；补测次数与补测间隔的乘积不得大于巡测间隔。补测次数与补测间隔的乘积不得大于巡测间隔。";
            }
            if (EquType == 6)
            {
                TaskName = "漏电保护巡测";
                TaskDes = "漏电保护巡测的任务对参数设置的要求是：补测次数与补测间隔的乘积不得大于巡测间隔。";
            }
            if (EquType == 7)
            {
                TaskName = "节电设备巡测";
                TaskDes = "节电设备巡测的默认巡测时间为1小时。巡测时间至少10分钟，不大于24小时。对参数设置的要求是补测次数与补测间隔的乘积不得大于巡测间隔。";
            }
            Interval = taskinfo.MinutesIntervals;
            StartMinutes = taskinfo.StartMinutes;          
            TimesRepartol = taskinfo.TimesRePartol;
            MinutesIntervalsRePartol = taskinfo.MinutesIntervalsRePartol;
            
        }

        

        //将Int转换成巡测时间
        public static string TransferToString(int minutes)
        {
            int hour = minutes/60;
            int min = minutes%60;

            return hour + ":" + string.Format("{0:D2}", min);
        }
    }
}
