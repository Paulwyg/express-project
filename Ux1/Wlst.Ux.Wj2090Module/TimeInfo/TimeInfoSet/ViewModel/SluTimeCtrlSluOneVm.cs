using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;


namespace Wlst.Ux.Wj2090Module.TimeInfo.ViewModel
{
    public class SluTimeCtrlSluOneVm : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public SluTimeCtrlSluOneVm(int sluId)
        {
            
            Is485 = true;
            SluId = sluId;
            AddrsCtrls = new List<int>();
            OperatorTypeSelected = 101;
            var holdinf = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId);
            if (holdinf != null)
            {
                SluName = holdinf.RtuName;
               // Is485 = holdinf.AttachRtuId != 0;
            }
        }

        //public SluTimeCtrlSluOneVm(SluTimeCtrlSluOne info)
        //{
        //    SluId = info.SluId;
        //    AddrsCtrls = new List<int>();
        //    AddrsCtrls.AddRange(info.AddrsCtrls);
        //    foreach (var g in OperatorType)
        //    {
        //        if (g.Value == info.OperatorType) g.IsSelected = true;
        //        else g.IsSelected = false;
        //    }
        //    var holdinf = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(SluId);
        //    if (holdinf != null)
        //    {
        //        SluName = holdinf.RtuName;
        //    }
        //}

        public void UpdateInfoBySluOne(SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne  info)
        {

            SluId = info.SluId;
            AddrsCtrls = new List<int>();
            AddrsCtrls.AddRange(info.CtrlPhys);

            OperatorTypeSelected = info.OperatorType;

            var holdinf = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( SluId);
            if (holdinf != null)
            {
                SluName = holdinf.RtuName;
                //if (holdinf.AttachRtuId == 0) Is485 = false;//lp  控制器不支持点选指令 所有只能控制到组
                //else Is485 = true;

                Is485 = true;
            }
            else
            {
                SluName = "" + SluId;
            }
            if (Is485)
                SelfDef = "组:" + AddrsCtrls.Count;
            else
                SelfDef = "总:" + AddrsCtrls.Count;
        }

        public event EventHandler OnSelectedSelfDefContrls;


        #region  CmdSetSefDef

        private ICommand _cmdAddTimeTable;

        public ICommand CmdSetSefDef
        {
            get
            {
                return _cmdAddTimeTable ??
                       (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true));
            }
        }

        private bool CanCmdAddTimeTable()
        {
            return OperatorTypeSelected == 4;
        }

        private void ExCmdAddTimeTable()
        {
            if (OnSelectedSelfDefContrls != null) OnSelectedSelfDefContrls(this, EventArgs.Empty);
        }

        #endregion


        public void UpdateInfoBySluOne(int x)
        {

            AddrsCtrls = new List<int>();
            if (Is485)
                SelfDef = "组:" + 0;
            else
                SelfDef = "总:" + 0;
            OperatorTypeSelected = 101;
            //foreach (var g in OperatorType)
            //{
            //    g.IsSelected = false;
            //}
        }


        private int _sSluId;

        public int SluId
        {
            get { return _sSluId; }
            set
            {
                if (value != _sSluId)
                {
                    _sSluId = value;
                    this.RaisePropertyChanged(() => this.SluId);

                    SluShowId = value+"";
                    var sluinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value );
                    if (sluinfo != null)
                    {

                        if (sluinfo.RtuFid  == 0)
                        {
                            SluShowId = sluinfo.RtuPhyId.ToString("D4");
                        }
                        else
                        {
                            var mtps =
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                                    sluinfo.RtuFid );
                            if (mtps != null)
                            {
                                SluShowId = mtps.RtuPhyId.ToString("D4");
                            }
                         
                        }
                    }
                  

                }
            }
        }

        private string _ssdfSluId;

        public string  SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }


        private bool _sIs485;

        public bool Is485
        {
            get { return _sIs485; }
            set
            {
                if (value != _sIs485)
                {
                    _sIs485 = value;
                    this.RaisePropertyChanged(() => this.Is485);
                }
            }
        }

        private string _sSluName;

        public string SluName
        {
            get { return _sSluName; }
            set
            {
                if (value != _sSluName)
                {
                    _sSluName = value;
                    this.RaisePropertyChanged(() => this.SluName);
                }
            }
        }



        //private ObservableCollection<NameIntBool> _operationOperatorType = null;

        ///// <summary>
        /////  1 全部、2 单数、3 双数、4、自定义操作   其他11-99： 如32 地址除3余2的控制器操作 101 不操作
        ///// </summary>
        //public ObservableCollection<NameIntBool> OperatorType
        //{
        //    get
        //    {
        //        if (_operationOperatorType == null)
        //        {
        //            _operationOperatorType = new ObservableCollection<NameIntBool>();
        //            _operationOperatorType.Add(new NameIntBool() {Name = "不操作", Value = 101, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "全部", Value = 1, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "单数", Value = 2, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "双数", Value = 3, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "隔二亮一", Value = 31, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "隔三亮一", Value = 41, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "隔四亮一", Value = 51, IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBool() {Name = "自定义", Value = 4, IsSelected = false});

        //        }
        //        return _operationOperatorType;
        //    }
        //}

        private int _sOperatorTypeSelected;

        /// <summary>
        ///  1 全部、2 单数、3 双数、4、自定义操作   其他11-99： 如32 地址除3余2的控制器操作 101 不操作
        /// </summary>
        public int OperatorTypeSelected
        {
            get { return _sOperatorTypeSelected; }
            set
            {
                if (value != _sOperatorTypeSelected)
                {
                    _sOperatorTypeSelected = value;
                    this.RaisePropertyChanged(() => this.OperatorTypeSelected);
                    if (value == 101)
                    {
                        IsShowSelfSelected = false;
                        Infosdf = "";
                    }
                    else
                    {
                        IsShowSelfSelected = true;
                        switch (value )
                        {
                            case 4: Infosdf = "自定义";
                                break;
                            case 10: Infosdf = "全部";
                                break;
                            case 21: Infosdf = "单数";
                                break;
                            case 20: Infosdf = "双数";
                                break;
                            case 31: Infosdf = "隔二亮一";
                                break;
                            case 41: Infosdf = "隔三亮一";
                                break;
                            case 51: Infosdf = "隔四亮一";
                                break;
                            default:
                                Infosdf = "";
                                break;
                        }
                    }
                }
            }
        }

        bool IsShowSelfSelectsdfed;

        public bool IsShowSelfSelected
        {
            get { return IsShowSelfSelectsdfed; }
            set
            {
                if (value != IsShowSelfSelectsdfed)
                {
                    IsShowSelfSelectsdfed = value;
                    this.RaisePropertyChanged(() => this.IsShowSelfSelected);
                }
            }
        }

        private string _marksdfeds;


        public string Infosdf
        {
            get { return _marksdfeds; }
            set
            {
                if (value != _marksdfeds)
                {
                    _marksdfeds = value;
                    this.RaisePropertyChanged(() => this.Infosdf);
                }
            }
        }

        private string _markeds;


        public string Marked
        {
            get { return _markeds; }
            set
            {
                if (value != _markeds)
                {
                    _markeds = value;
                    this.RaisePropertyChanged(() => this.Marked);
                }
            }
        }


        private string _mSelfDef;


        public string SelfDef
        {
            get { return _mSelfDef; }
            set
            {
                if (value != _mSelfDef)
                {
                    _mSelfDef = value;
                    this.RaisePropertyChanged(() => this.SelfDef);
                }
            }
        }

        public List<int> AddrsCtrls;

        public void UpdateAddrsCtrls(List<int> addr)
        {
            AddrsCtrls.Clear();
            AddrsCtrls.AddRange(addr);
            if (Is485)
                SelfDef = "组:" + addr.Count;
            else
                SelfDef = "总:" + addr.Count;
        }
    }
}
