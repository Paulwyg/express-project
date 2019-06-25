using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel;
using Wlst.client;

namespace Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class RtuZhaoCeParsViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public RtuZhaoCeParsViewModel(int rtuId, Wlst.client.ZhaoCeInfo.RtuZhaoRtuPara1 rtuInfo)
        {
            this.DateTimeRecevie = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.RtuId = rtuId;

            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(this.RtuId))
            {
                var rtuInfomation = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[this.RtuId];
                this.RtuName = rtuInfomation.RtuName;
            }

            if(rtuInfo.DataMark == 1)
            {
                this.SwitchOutTotal = Convert.ToString(rtuInfo.SwitchOutField.SwitchOutTotal);
                this.SwitchOutInfo.Clear();

                for (int i = 0; i < rtuInfo.SwitchOutField.SwitchOutTotal; i++)
                {
                    this.SwitchOutInfo.Add(new SwitchOutInfoViewModel(rtuInfo.SwitchOutField, i));
                }
            }
            else if (rtuInfo.DataMark == 2)
            {
                this.VoltageTransformer = Convert.ToString(rtuInfo.SwitchInField.VoltageTransformer);
                this.LoopTotal = Convert.ToString(rtuInfo.SwitchInField.LoopTotal);

                this.SwitchInInfo.Clear();
                for (int i = 0; i < rtuInfo.SwitchInField.LoopTotal; i++)
                {
                    this.SwitchInInfo.Add(new SwitchInInfoViewModel(rtuInfo.SwitchInField, i));
                }
            }
            else if (rtuInfo.DataMark == 3)
            {
                this.LoopTotal = Convert.ToString(rtuInfo.SwitchInLimitField.LoopTotal);

                this.SwitchInLimitInfo.Clear();
                for (int i = 0; i < rtuInfo.SwitchInLimitField.LoopTotal; i++)
                {
                    try
                    {
                        var mtp = new SwitchInLimitViewModel(rtuInfo.SwitchInLimitField, i);
                        this.SwitchInLimitInfo.Add(mtp);
                    }
                    catch (Exception exs)
                    {
                    }
                }
            }
        }

        public RtuZhaoCeParsViewModel()
        {
            this.DateTimeRecevie = "--";
            this.LoopTotal = "--";
            this.PhyId = "--";
            this.RtuName = "--";
            this.SwitchInInfo.Clear();
            this.SwitchOutInfo.Clear();
            this.SwitchOutTotal = "--";
            this.VoltageTransformer = "--";
        }

        private string _datatime;
        /// <summary>
        /// 接收时间  作为搜索关键字
        /// </summary>
        public string DateTimeRecevie
        {
            get { return _datatime; }
            set
            {
                if (_datatime != value)
                {
                    _datatime = value;
                    this.RaisePropertyChanged(() => this.DateTimeRecevie);
                }
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
                if (_rtuName != value)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
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
                if (_rtuId != value)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);

                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(value)) 
                        PhyId =  Convert.ToString( Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value].RtuPhyId);
                }
            }
        }

        private string _phyid;
        /// <summary>
        /// 物理地址
        /// </summary>
        public string PhyId
        {
            get { return _phyid; }
            set
            {
                if (_phyid != value)
                {
                    _phyid = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private string _switchOutTotal;
        /// <summary>
        /// 开关量输出路数
        /// </summary>
        public string SwitchOutTotal
        {
            get { return _switchOutTotal; }
            set
            {
                if (_switchOutTotal != value)
                {
                    _switchOutTotal = value;
                    this.RaisePropertyChanged(() => this.SwitchOutTotal);
                }
            }
        }

        private string _loopTotal;
        /// <summary>
        /// 回路数量
        /// </summary>
        public string LoopTotal
        {
            get { return _loopTotal; }
            set
            {
                if (_loopTotal != value)
                {
                    _loopTotal = value;
                    this.RaisePropertyChanged(() => this. LoopTotal);
                }
            }
        }

        private string _VoltageTransformer;
        /// <summary>
        /// 电压互感比,默认5
        /// </summary>
        public string VoltageTransformer 
        {
            get { return _VoltageTransformer; }
            set
            {
                if (_VoltageTransformer != value)
                {
                    _VoltageTransformer = value;
                    this.RaisePropertyChanged(() => this.VoltageTransformer);
                }
            }
        }

        private ObservableCollection<SwitchOutInfoViewModel> _switchOutInfo;

        /// <summary>
        /// 开关量输出信息
        /// </summary>
        public ObservableCollection<SwitchOutInfoViewModel> SwitchOutInfo
        {
            get
            {
                if (_switchOutInfo == null)
                    _switchOutInfo = new ObservableCollection<SwitchOutInfoViewModel>();
                return _switchOutInfo;
            }
        }

        private ObservableCollection<SwitchInInfoViewModel> _switchInInfo;
        /// <summary>
        /// 电流回路信息
        /// </summary>
        public ObservableCollection<SwitchInInfoViewModel> SwitchInInfo
        {
            get
            {
                if (_switchInInfo == null)
                    _switchInInfo = new ObservableCollection<SwitchInInfoViewModel>();
                return _switchInInfo;
            }
        }

        private ObservableCollection<SwitchInLimitViewModel> _switchInLimitInfo;
        /// <summary>
        /// 电流上下限信息
        /// </summary>
        public ObservableCollection<SwitchInLimitViewModel> SwitchInLimitInfo
        {
            get
            {
                if (_switchInLimitInfo == null)
                    _switchInLimitInfo = new ObservableCollection<SwitchInLimitViewModel>();
                return _switchInLimitInfo;
            }
        }

        
    }
}
