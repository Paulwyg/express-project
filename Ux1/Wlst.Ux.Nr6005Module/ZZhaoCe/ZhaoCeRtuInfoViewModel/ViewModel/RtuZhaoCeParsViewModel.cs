using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class RtuZhaoCeParsViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        //public int RtuIdLogic = 0;
        public RtuZhaoCeParsViewModel(Wlst .client .ZhaoCeInfo   rtuInfos)
        {
           // RtuIdLogic = rtuId;

            if (rtuInfos.RtuPara == null ||  rtuInfos.RtuPara.Count == 0) return;
            var rtuInfo = rtuInfos.RtuPara[0];
            if (rtuInfo == null) return;

            Visi = Visibility.Collapsed;//不呈现电能参数

            this.HeartBeatPeriod = rtuInfo.HeartBeatPeriod;
            this.ErrorDelays = rtuInfo.ErrorDelays;
            this.ReportDataPeriod = rtuInfo.ReportDataPeriod;
            PhyId = rtuInfo.RtuPhyId;
   
            this.RtuId = rtuInfos.RtuId;
            this.SwitchOutCount = rtuInfo.SwitchOutCount;
            this.SwitchInCount = rtuInfo.SwitchInCount;
            this.SinCount = rtuInfo.SinCount;
            this.CitePayTime = rtuInfo.CitePayTime;
            this.SelfPayTime = rtuInfo.SelfPayTime;
            this.VoRange = rtuInfo.VoRange;
            this.VoLower = rtuInfo.VoLower;
            this.VoUpper = rtuInfo.VoUpper;
            this.GroupFirst = rtuInfo.GroupFirst;


            this.SwitchChange.Clear();
            this.SwitchOutInfo.Clear();
            foreach (var t in rtuInfo.SwitchOutInfo)
            {
                this.SwitchOutInfo.Add(new SwitchOutInfoViewModel(t));
            }

            this.LoopInfo.Clear();
            for (int i = 0; i < this.SinCount; i++)
            {
                if (rtuInfo.LoopInfo.Count > i)
                {
                    this.LoopInfo.Add(new LoopInfoViewModel(rtuInfo.LoopInfo[i]));
                }
            }


            this.DateTimeRecevie = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// string.Format("{0:G}", DateTime.Now);
            if (
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                    ContainsKey(this.RtuId))
            {
                var rtuInfomation =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                        InfoItems [this.RtuId];
                this.RtuName = rtuInfomation.RtuName;


                var rtupara = rtuInfomation as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (rtupara == null) return;
                

                foreach (var t in rtupara.WjLoops .Values )
                    {
                        bool find = false;
                        foreach (var g in LoopInfo)
                        {
                            if (g.LoopId == t.LoopId)
                            {
                                find = true;
                                g.LoopName = t.LoopName;
                                if (t.SwitchOutputId  > 0) g.KKK = "K" + t.SwitchOutputId ;
                                break;
                            }
                        }
                        if (!find)
                        {
                            this.LoopInfo.Add(new LoopInfoViewModel(t.LoopId)
                                                  {
                                                      LoopName = t.LoopName,

                                                  });
                        }
                    }
                
            }


            //SwitchIdS = "开关量矢量:";
            //Hops = "开关量跳变：";
            //for (int i = 1; i < rtuInfo.SwitchChange.Count + 1; i++)
            //{
            //    this.SwitchChange.Add(
            //        new NameValueInt()
            //            {
            //                Value = i,
            //                Name = rtuInfo.SwitchChange[i - 1] == 1 ? "报警" : "不报警"
            //            });

            //        if (rtuInfo.SwitchChange[i - 1] == 1)
            //            Hops += i + ":Y " + "; ";
            //        else Hops += i + ":- " + "; ";
            //}

            //this.SwitchIn40Vector.Clear();
            //for (int i = 1; i < rtuInfo.SwitchIn40Vector.Count + 1; i++)
            //{
            //    this.SwitchIn40Vector.Add(
            //        new NameValueInt()
            //            {
            //                Value = i,
            //                Name = rtuInfo.SwitchIn40Vector[i - 1].ToString(CultureInfo.InvariantCulture)
            //            });

            //        SwitchIdS += i + " - " + rtuInfo.SwitchIn40Vector[i - 1] + "; ";
                
            //}

            foreach (var t in LoopInfo)
            {
                int swin = 0;
                string baojin = "--";
                if (rtuInfo.SwitchIn40Vector.Count > t.LoopId - 1 && t.LoopId > 0)
                {
                    swin = rtuInfo.SwitchIn40Vector[t.LoopId - 1];
                    if (rtuInfo.SwitchChange.Count + 1 > swin)
                        baojin = rtuInfo.SwitchChange[swin - 1] == 1 ? "报警" : "--";
                }

                t.SwitchInId = swin + "";
                t.IsSwitchInHop = baojin;
                //this.LoopInfo.Add(new LoopInfoViewModel(rtuInfo.LoopInfo[i]) { SwitchInId = swin + "", IsSwitchInHop = baojin });
            }

        }

        public void Addk7K8(int rtuId, Wlst.client.ZhaoCeInfo.RtuZhaoRtuPara rtuInfo)
        {
            RtuId = rtuId;
            if (rtuInfo == null) return;
            if (rtuInfo.SwitchOutInfo == null) return;
            foreach (var t in rtuInfo.SwitchOutInfo)
            {
                this.SwitchOutInfo.Add(new SwitchOutInfoViewModel(t));
            }
        }

        public  void AddElectricityArgs(int rtuId, List<int> lst)
        {
            RtuId = rtuId;
            if (lst.Count < 3) return;
            Visi = Visibility.Visible; //呈现 电能参数
            this.AphaseRadio = lst[0];
            this.BphaseRadio = lst[1];
            this.CphaseRadio = lst[2];

        }


        #region other info
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

        private string _RtuName;
        /// <summary>
        /// 终端名称
        /// </summary>
        public string RtuName
        {
            get { return _RtuName; }
            set
            {
                if (_RtuName != value)
                {
                    _RtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private string _hps;
        /// <summary>
        /// 终端名称
        /// </summary>
        public string Hops
        {
            get { return _hps; }
            set
            {
                if (_hps != value)
                {
                    _hps = value;
                    this.RaisePropertyChanged(() => this.Hops);
                }
            }
        }     private string _hpss;
        /// <summary>
        /// 终端名称
        /// </summary>
        public string SwitchIdS
        {
            get { return _hpss; }
            set
            {
                if (_hpss != value)
                {
                    _hpss = value;
                    this.RaisePropertyChanged(() => this.SwitchIdS);
                }
            }
        }

        #endregion

        #region general paras

        private int _heartBeatPeriod;

        /// <summary>
        /// 心跳包
        /// </summary>
        public int HeartBeatPeriod
        {
            get { return _heartBeatPeriod; }
            set
            {
                if (_heartBeatPeriod != value)
                {
                    _heartBeatPeriod = value;
                    this.RaisePropertyChanged(() => this.HeartBeatPeriod);
                }
            }
        }

        private int _errorDelays;

        /// <summary>
        /// 工作参数 报警延时（秒）
        /// </summary>
        public int ErrorDelays
        {
            get { return _errorDelays; }
            set
            {
                if (_errorDelays != value)
                {
                    _errorDelays = value;
                    this.RaisePropertyChanged(() => this.ErrorDelays);
                }
            }
        }


        private int _reportDataPeriod;

        /// <summary>
        /// 通讯参数 主报周期
        /// </summary>
        public int ReportDataPeriod
        {
            get { return _reportDataPeriod; }
            set
            {
                if (_reportDataPeriod != value)
                {
                    _reportDataPeriod = value;
                    this.RaisePropertyChanged(() => this.ReportDataPeriod);
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
                    
                    if(Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey( value))PhyId =Wlst .Sr .EquipmentInfoHolding .Services .EquipmentDataInfoHold .InfoItems [value ].RtuPhyId ;
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
        private int _switchOutCount;

        /// <summary>
        /// 开关量输出路数
        /// </summary>
        public int SwitchOutCount
        {
            get { return _switchOutCount; }
            set
            {
                if (_switchOutCount != value)
                {
                    _switchOutCount = value;
                    this.RaisePropertyChanged(() => this.SwitchOutCount);
                }
            }
        }

        private int _switchInCount;

        /// <summary>
        /// 开关量输入路数
        /// </summary>
        public int SwitchInCount
        {
            get { return _switchInCount; }
            set
            {
                if (_switchInCount != value)
                {
                    _switchInCount = value;
                    this.RaisePropertyChanged(() => this.SwitchInCount);
                }
            }
        }

        private int _sinCount;

        /// <summary>
        /// 模拟量输入路数
        /// </summary>
        public int SinCount
        {
            get { return _sinCount; }
            set
            {
                if (_sinCount != value)
                {
                    _sinCount = value;
                    this.RaisePropertyChanged(() => this.SinCount);
                }
            }
        }

        private string _citePayTime;

        /// <summary>
        /// 市付费启用时分，hhmm
        /// </summary>
        public string CitePayTime
        {
            get { return _citePayTime; }
            set
            {
                if (_citePayTime != value)
                {
                    _citePayTime = value;
                    this.RaisePropertyChanged(() => this.CitePayTime);
                }
            }
        }

        private string _selfPayTime;

        /// <summary>
        /// 自付费启用时分，hhmm
        /// </summary>
        public string SelfPayTime
        {
            get { return _selfPayTime; }
            set
            {
                if (_selfPayTime != value)
                {
                    _selfPayTime = value;
                    this.RaisePropertyChanged(() => this.SelfPayTime);
                }
            }
        }

        private int _voRange;

        /// <summary>
        /// 电压量程
        /// </summary>
        public int VoRange
        {
            get { return _voRange; }
            set
            {
                if (_voRange != value)
                {
                    _voRange = value;
                    this.RaisePropertyChanged(() => this.VoRange);
                }
            }
        }

        private int _voUpper;

        /// <summary>
        /// 电压上限
        /// </summary>
        public int VoUpper
        {
            get { return _voUpper; }
            set
            {
                if (_voUpper != value)
                {
                    _voUpper = value;
                    this.RaisePropertyChanged(() => this.VoUpper);
                }
            }
        }

        private int _aphaseRadio;
        /// <summary>
        /// A相互感比
        /// </summary>
        public int AphaseRadio
        {
            get { return _aphaseRadio; }
            set
            {
                if (value != _aphaseRadio)
                {
                    if (value < 0) return;
                    _aphaseRadio = value;
                    this.RaisePropertyChanged(() => this.AphaseRadio);
                }
            }
        }


        private int _bphaseRadio;
        /// <summary>
        /// B相互感比
        /// </summary>
        public int BphaseRadio
        {
            get { return _bphaseRadio; }
            set
            {
                if (value != _bphaseRadio)
                {
                    if (value < 0) return;
                    _bphaseRadio = value;
                    this.RaisePropertyChanged(() => this.BphaseRadio);
                }
            }
        }


        private int _cphaseRadio;
        /// <summary>
        /// C相互感比
        /// </summary>
        public int CphaseRadio
        {
            get { return _cphaseRadio; }
            set
            {
                if (value != _cphaseRadio)
                {
                    if (value < 0) return;
                    _cphaseRadio = value;
                    this.RaisePropertyChanged(() => this.CphaseRadio);
                }
            }
        }

        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }






        private int _voLower;

        /// <summary>
        /// 电压下限
        /// </summary>
        public int VoLower
        {
            get { return _voLower; }
            set
            {
                if (_voLower != value)
                {
                    _voLower = value;
                    this.RaisePropertyChanged(() => this.VoLower);
                }
            }
        }

        private int _groupFirst;

        /// <summary>
        /// 组优先
        /// </summary>
        public int GroupFirst
        {
            get { return _groupFirst; }
            set
            {
                if (_groupFirst != value)
                {
                    _groupFirst = value;
                    this.RaisePropertyChanged(() => this.GroupFirst);
                }
            }
        }

        #endregion

        #region Obs paras
        //Wlst .Cr .Core .Models .IdName 
        private ObservableCollection<NameValueInt> _switchChange;

        /// <summary>
        /// 开关量跳变报警  1~16； 0不报警 1报警
        /// </summary>
        public  ObservableCollection<NameValueInt> SwitchChange
        {
            get
            {
                if (_switchChange == null)
                    _switchChange = new ObservableCollection<NameValueInt>();
                return _switchChange;
            }
        }
        private ObservableCollection<NameValueInt> _switchIn40Vector;

        /// <summary>
        /// 开关量输出40矢量
        /// </summary>
        public  ObservableCollection<NameValueInt> SwitchIn40Vector
        {
            get
            {
                if (_switchIn40Vector == null)
                    _switchIn40Vector = new ObservableCollection<NameValueInt>();
                return _switchIn40Vector;
            }
        }


        private ObservableCollection<SwitchOutInfoViewModel> _switchOutInfo;

        /// <summary>
        /// 终端开关量输出信息
        /// </summary>
        public  ObservableCollection<SwitchOutInfoViewModel> SwitchOutInfo
        {
            get
            {
                if (_switchOutInfo == null)
                    _switchOutInfo = new ObservableCollection<SwitchOutInfoViewModel>();
                return _switchOutInfo;
            }
        }

        private ObservableCollection<LoopInfoViewModel> _loopInfo;

        /// <summary>
        /// 回路信息
        /// </summary>
        public  ObservableCollection<LoopInfoViewModel> LoopInfo
        {
            get
            {
                if (_loopInfo == null)
                    _loopInfo = new ObservableCollection<LoopInfoViewModel>();
                return _loopInfo;
            }
        }
        #endregion
    }
}
