using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Nr6005Module.TmlInfoValidQuery.Services;

namespace Wlst.Ux.Nr6005Module.TmlInfoValidQuery.ViewModel
{
    [Export(typeof(IITmlInfoValidQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInfoValidQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITmlInfoValidQuery
    {
        public TmlInfoValidQueryViewModel()
        {
            
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            InvalidTmlList.Clear();

            KGLSelect = false;
            MNLSelect = false;
            TYSelect = false;
            BYSelect = false;
            ZBZQSelect = false;
            ZBZQThres = 250;
            PBYDLSelect = false;

        }

        public void OnUserHideOrClosing()
        {
            InvalidTmlList.Clear();
        }

        #region define
        private ObservableCollection<InvalidTmlModel> _invalidTmlList;

        public ObservableCollection<InvalidTmlModel> InvalidTmlList
        {
            get
            {
                if (_invalidTmlList == null)
                    _invalidTmlList = new ObservableCollection<InvalidTmlModel>();
                return _invalidTmlList;
            }
            set
            {
                if (_invalidTmlList == value) return;
                _invalidTmlList = value;
                this.RaisePropertyChanged(() => this.InvalidTmlList);
            }
        }

        private bool  _kglSelect;
        public bool KGLSelect
        {
            get
            {
                return _kglSelect;
            }
            set
            {
                if (_kglSelect != value)
                {
                    _kglSelect = value;
                    this.RaisePropertyChanged(() => this.KGLSelect);
                }
            }
        }

        private bool _mnlSelect;
        public bool MNLSelect
        {
            get
            {
                return _mnlSelect;
            }
            set
            {
                if (_mnlSelect != value)
                {
                    _mnlSelect = value;
                    this.RaisePropertyChanged(() => this.MNLSelect);
                }
            }
        }

        private bool _tySelect;
        public bool TYSelect
        {
            get
            {
                return _tySelect;
            }
            set
            {
                if (_tySelect != value)
                {
                    _tySelect = value;
                    this.RaisePropertyChanged(() => this.TYSelect);
                }
            }
        }

        private bool _bySelect;
        public bool BYSelect
        {
            get
            {
                return _bySelect;
            }
            set
            {
                if (_bySelect != value)
                {
                    _bySelect = value;
                    this.RaisePropertyChanged(() => this.BYSelect);
                }
            }
        }

        private bool _zbzqSelect;
        public bool ZBZQSelect
        {
            get
            {
                return _zbzqSelect;
            }
            set
            {
                if (_zbzqSelect != value)
                {
                    _zbzqSelect = value;
                    this.RaisePropertyChanged(() => this.ZBZQSelect);
                }
            }
        }

        private int _zbzqThres;
        public int ZBZQThres
        {
            get
            {
                return _zbzqThres;
            }
            set
            {
                if (_zbzqThres != value)
                {
                    _zbzqThres = value;
                    this.RaisePropertyChanged(() => this.ZBZQThres);
                }
            }
        }

        private bool _pbydllSelect;
        public bool PBYDLSelect
        {
            get
            {
                return _pbydllSelect;
            }
            set
            {
                if (_pbydllSelect != value)
                {
                    _pbydllSelect = value;
                    this.RaisePropertyChanged(() => this.PBYDLSelect);
                }
            }
        }

        private string _showMsg;
        public string ShowMsg
        {
            get
            {
                return _showMsg;
            }
            set
            {
                if (_showMsg != value)
                {
                    _showMsg = value;
                    this.RaisePropertyChanged(() => this.ShowMsg);
                }
            }
        }
        #endregion

        #region Command
        #region Query
        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdQuery == null)
                    _cmdQuery = new RelayCommand(ExQuery, CanQuery, false);
                return _cmdQuery;
            }
        }

        private bool CanQuery()
        {
            return true;
        }


        private void ExQuery()
        {
            InvalidTmlList.Clear();
            int index = 1;
            foreach (var t in EquipmentDataInfoHold.InfoItems)
            {
                bool add = false;
                string faultType = string.Empty;

                if (t.Value.EquipmentType == WjParaBase.EquType.Rtu)
                {
                    if (MNLSelect == true)
                    {
                        var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Value.RtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        if (tt != null)
                        {
                            List<int> MoniliangID = new List<int>();

                            foreach (var ttt in tt.WjLoops.Values)
                            {
                                if (ttt.VectorMoniliang != 0)
                                {
                                    foreach (var m in MoniliangID)
                                    {
                                        if(m == ttt.VectorMoniliang)
                                        {
                                            add = true;
                                            faultType += "模拟量缺省,";
                                            break;
                                        }
                                    }

                                    if(add)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        MoniliangID.Add(ttt.VectorMoniliang);
                                    }
                                }
                            }

                            foreach (var m in MoniliangID)
                            {
                                if ((m < 1) || (m > MoniliangID.Count))
                                {
                                    add = true;
                                    faultType += "模拟量缺省,";
                                    break;
                                }
                            }


                        }
                    }

                    if (KGLSelect == true)
                    {
                        var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Value.RtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        bool fault = false;

                        if (tt != null)
                        {
                            foreach (var ttt in tt.WjSwitchOuts.Values)
                            {
                                if (ttt.SwitchVecotr != ttt.SwitchId)
                                {
                                    add = true;
                                    faultType += "开关量缺省,";
                                    fault = true;
                                    break;
                                }
                            }

                            int[] loopNum = new int[6];

                            if(fault == false)
                            {
                                foreach (var ttt in tt.WjLoops.Values)
                                {
                                    if (ttt.SwitchOutputId != 0)
                                    {
                                        loopNum[ttt.SwitchOutputId - 1]++;

                                        if (ttt.SwitchOutputId != ttt.VectorSwitchIn)
                                        {
                                            add = true;
                                            faultType += "开关量缺省,";
                                            fault = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if(fault == false)
                            {
                                for(int i = 1; i < 6; i ++)
                                {
                                    if((loopNum[i] != 0) && (loopNum[i - 1] == 0))
                                    {
                                        add = true;
                                        faultType += "开关量缺省,";
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (TYSelect == true)
                    {
                        if (t.Value.RtuStateCode == 1)
                        {
                            add = true;
                            faultType += "状态停运,";
                        }
                    }

                    if (BYSelect == true)
                    {
                        if (t.Value.RtuStateCode == 0)
                        {
                            add = true;
                            faultType += "状态不用,";
                        }
                    }

                    if (ZBZQSelect == true)
                    {
                        var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Value.RtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        if (tt != null)
                        {
                            if(tt.WjGprs.RtuReportCycle < ZBZQThres)
                            {
                                add = true;
                                faultType += "主报周期过小,";
                            }
                        }
                    }

                    if (PBYDLSelect == true)
                    {
                        var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Value.RtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        if (tt != null)
                        {
                            foreach (var ttt in tt.WjLoops.Values)
                            {
                                if (ttt.IsShieldLoop != 0)
                                {

                                }
                            }
                        }
                    }

                    if (add == true)
                    {
                        var tmp = ServicesGrpSingleInfoHold.GetRtuBelongGrp(t.Value.RtuId);

                        var grpName = "未分组";
                        if (tmp != null)
                        {
                            grpName =
                                (ServicesGrpSingleInfoHold.InfoGroups[new Tuple<int, int>(tmp.Item1, tmp.Item2)]).
                                    GroupName;
                        }

                        InvalidTmlList.Add(new InvalidTmlModel
                                               {
                                                   Index = index,
                                                   RtuId = t.Value.RtuId,
                                                   PhyId = t.Value.RtuPhyId,
                                                   RtuName = t.Value.RtuName,
                                                   GrpName = grpName,
                                                   FaultType = faultType.Substring(0, faultType.Length - 1)
                                               });

                        index++;
                    }
                    else
                    {
                    }
                }
            }

            if(InvalidTmlList.Count != 0)
            {
                ShowMsg = "共查询到" + InvalidTmlList.Count + "个记录";
            }
            else
            {
                ShowMsg = string.Empty;
            }


        }
        #endregion

        #region Export
        private ICommand _cmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdExport == null)
                    _cmdExport = new RelayCommand(ExExport, CanExport, false);
                return _cmdExport;
            }
        }

        private bool CanExport()
        {
            return InvalidTmlList.Count > 0;
        }

        private void ExExport()
        {

        }
        #endregion
        #endregion

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public string Title
        {
            get { return "终端参数合法性查询"; }
        }

        #endregion
    }
}
