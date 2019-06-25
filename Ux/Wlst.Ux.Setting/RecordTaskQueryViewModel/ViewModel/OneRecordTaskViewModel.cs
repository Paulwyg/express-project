using System;
using Wlst.Cr.Core.CoreServices;


namespace Wlst.Ux.Setting.RecordTaskQueryViewModel.ViewModel
{
    public class OneRecordTaskViewModel : ObservableObject
    {
        #region attri

        private int _recordIndex;

        /// <summary>
        /// 记录编号  顺序
        /// </summary>
        public int RecordIndex
        {
            get { return _recordIndex; }
            set
            {
                if (value != _recordIndex)
                {
                    _recordIndex = value;
                    this.RaisePropertyChanged(() => this.RecordIndex);
                }
            }
        }


        private string _SndTime;

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public string DateCreate
        {
            get { return _SndTime; }
            set
            {
                if (value != _SndTime)
                {
                    _SndTime = value;
                    this.RaisePropertyChanged(() => this.DateCreate);
                }
            }
        }

        private string _rtuId;

        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperatorName
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.OperatorName);
                }
            }
        }

        private int _rtuName;

        /// <summary>
        /// 任务实例Id
        /// </summary>
        public int TaskId
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.TaskId);
                }
            }
        }

        private string _loop;

        /// <summary>
        /// 任务实例名称
        /// </summary>
        public string TaskName
        {
            get { return _loop; }
            set
            {
                if (value != _loop)
                {
                    _loop = value;
                    this.RaisePropertyChanged(() => this.TaskName);
                }
            }
        }


        private string _K1K3RcvTime;

        /// <summary>
        /// 任务描述信息
        /// </summary>
        public string TaskDescription
        {
            get { return _K1K3RcvTime; }
            set
            {
                if (value != _K1K3RcvTime)
                {
                    _K1K3RcvTime = value;
                    this.RaisePropertyChanged(() => this.TaskDescription);
                }
            }
        }

        private string _K4K6RcvTime;

        /// <summary>
        /// 操作对象
        /// </summary>
        public string OperatorTarget
        {
            get { return _K4K6RcvTime; }
            set
            {
                if (value != _K4K6RcvTime)
                {
                    _K4K6RcvTime = value;
                    this.RaisePropertyChanged(() => this.OperatorTarget);
                }
            }
        }

        private string _OperatorTargetArg;

        /// <summary>
        /// 操作对象 附加识别参数
        /// </summary>
        public string OperatorTargetArg
        {
            get { return _OperatorTargetArg; }
            set
            {
                if (value != _OperatorTargetArg)
                {
                    _OperatorTargetArg = value;
                    this.RaisePropertyChanged(() => this.OperatorTargetArg);
                }
            }
        }

        private string _AttachInfo;

        /// <summary>
        /// 操作对象
        /// </summary>
        public string AttachInfo
        {
            get { return _AttachInfo; }
            set
            {
                if (value != _AttachInfo)
                {
                    _AttachInfo = value;
                    this.RaisePropertyChanged(() => this.AttachInfo);
                }
            }
        }

        #endregion


        public static OneRecordTaskViewModel GetOneRecordVm(Wlst .client .TaskRecord  .TaskRecordItem   record)
        {
            return new OneRecordTaskViewModel()
                       {
                           DateCreate = new DateTime(record.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),//string.Format("{0:G}",new DateTime(record.DateCreate)),
                           AttachInfo = record.Remark ,//.AttachInfo,
                         //  OperatorName = record.OperatorName,
                           OperatorTarget = record.OperatorTar ,
                         //  OperatorTargetArg = record.OperatorTarArg ,
                           TaskDescription = record.TaskDescription,
                           TaskId = record.TaskId,
                           TaskName = record.TaskName,
                         //  RecordIndex = record.EventId,
                       };

        }


    }


}
