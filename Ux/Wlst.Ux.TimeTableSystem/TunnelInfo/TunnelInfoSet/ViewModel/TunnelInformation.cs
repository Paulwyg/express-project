using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Views;
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Services;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.ViewModel
{
    public partial class TunnelInformation : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public TunnelInformation(int areaId)
        {
            AreaId = areaId;
            SchemeId = 0;
            SchemeName = "新方案";
            TunnelName = "新隧道";
            SubOperationCount = 0;
            ProtectTime = 300;

        }

 
        public TunnelInformation(int areaId,TunnelControlPlan.TunnelControlOnePlan info)
        {
            AreaId = areaId;
            SchemeId = info.PlanId;
            SchemeName = info.PlanName;
            TunnelName = info.TunnelName;
            ProtectTime = info.TimeProtect;
            IsLuxOrTime = info.IsLightControl;
            ControlMode = info.IsLightControl == 1 ? "光控" : "时控";
            IsSelectlightEnable = info.IsLightControl == 1 ;
            IsSelectTimeEnable = info.IsLightControl == 2;
            SubOperationCount = info.ItemPlan.Count;

            List<int> needAddRtu = new List<int>();


            var currentLux = new IdNameDesc();
            var currentLux2 = new IdNameDesc();
            foreach (var x in LuxGetServer.GetAllLuxEquipment)
            {
                if (info.LuxId == x.Value)
                {
                    currentLux.Id = x.Value;
                    currentLux.Name = x.Name;
                    currentLux.NameDesc = x.Value2.ToString("d4") + "-" + x.Name;
                }
                if (info.LuxIdBackup == x.Value)
                {
                    currentLux2.Id = x.Value;
                    currentLux2.Name = x.Name;
                    currentLux2.NameDesc = x.Value2.ToString("d4") + "-" + x.Name;
                }

            }

            CurrentSelectLux = currentLux;
            CurrentSelectLux2 = currentLux2;

            var terminalItem = new ObservableCollection<OneItemTerminal>();        
            foreach (var t in info.RtusBelongThisTunnel)
            {

             
                var itemsInfo = new ObservableCollection<TimeInfoName>();
                var itemsInfoNew = new ObservableCollection<TimeInfoName>();
                var lst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);
                var para = lst as Wj3005Rtu;
                if (para == null) continue;
                var name = new List<string>();
                var wjswouts = (from tt in para.WjSwitchOuts orderby tt.Key select tt).ToList();

                foreach (var l in wjswouts)
                {
                    name.Add(l.Value.SwitchName);
                }


                for (int i = 0; i < 8; i++)
                {

                    itemsInfo.Add(new TimeInfoName()
                    {
                        IsCheckSwitch = false,//f.SwitchOutNeedOpen.Contains(i + 1),
                        IsEnabledOn = i + 1 <= para.WjSwitchOuts.Count,
                        TimeTableName = i + 1 <= para.WjSwitchOuts.Count ? name[i] : null

                    });
                    itemsInfoNew.Add(new TimeInfoName()
                    {
                        IsCheckSwitch = false,//f.SwitchOutNeedOpen.Contains(i + 1),
                        IsEnabledOn = i + 1 <= para.WjSwitchOuts.Count,
                        TimeTableName = i + 1 <= para.WjSwitchOuts.Count ? name[i] : null

                    });
                }

                terminalItem.Add(new OneItemTerminal()
                {
                    RtuId = t,
                    RtuName = lst.RtuName,
                    Items = itemsInfo,
                    IsEnable = false

                });

                //加载隧道所属终端
                CurrentSelectedTerminalItems.Add(new OneItemTerminal()
                {
                    //RtuId = t,
                    //IsSelected = true,
                    RtuId = t,
                    RtuName = lst.RtuName,
                    Items = itemsInfoNew,
                    IsEnable = false

                });
                needAddRtu.Add(t);

                
            }




            //操作
            foreach (var t in info.ItemPlan)
            {
                ObservableCollection<OneItemTerminal> terminalItemOne = new ObservableCollection<OneItemTerminal>();

                foreach (var g in terminalItem)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        g.Items[i].IsCheckSwitch = false;
                    }

                    terminalItemOne.Add(new OneItemTerminal()
                                            {
                                                RtuId = g.RtuId,
                                                RtuName = g.RtuName,
                                                Items = g.Items ,
                                            });
                }


                foreach (var f in t.ItemRtuOpe)
                {

                    foreach (var ong in terminalItemOne)
                    {
                   
                        if(ong.RtuId == f.RtuId)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                ong.Items[i].IsCheckSwitch = f.SwitchOutNeedOpen.Contains(i + 1);
                            }
                            ong.IsEnable = true;
                        }
                    }
                    //var itemsInfo=new ObservableCollection<TimeInfoName>();
                    //var lst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.RtuId);
                    //var para = lst as Wj3005Rtu;
                    //if (para == null) continue;
                    //var name = new List<string>();
                    //var wjswouts = (from tt in para.WjSwitchOuts orderby tt.Key select tt).ToList();

                    //foreach (var l in wjswouts)
                    //{
                    //    name.Add(l.Value.SwitchName);
                    //}
                
                    
                    //for(int i=0;i<8;i++)
                    //{

                    //    itemsInfo.Add(new TimeInfoName()
                    //                      {
                    //                          IsCheckSwitch = f.SwitchOutNeedOpen.Contains(i + 1),
                    //                          IsEnabledOn = i + 1 <= para.WjSwitchOuts.Count,
                    //                          TimeTableName = i + 1 <= para.WjSwitchOuts.Count ? name[i] : null

                    //                      });
                    //}


                  
                }

                ObservableCollection<OneItemTerminal> tmp = new ObservableCollection<OneItemTerminal>();

                var oneItem = (from tt in terminalItemOne orderby tt.PhyId select tt).ToList();

                foreach (var g in oneItem)
                {
                    var ttitem = new ObservableCollection<TimeInfoName>();
                    foreach(var tt in g.Items)
                    {
                        ttitem.Add(new TimeInfoName()
                                       {
                                           IsCheckSwitch = tt.IsCheckSwitch,
                                           IsEnabledOn =tt.IsEnabledOn,
                                           TimeTableName = tt.TimeTableName
                                       });
                    }

                    tmp.Add(new OneItemTerminal()
                                {
                                    RtuId = g.RtuId,
                                    RtuName = g.RtuName,
                                    Items = ttitem,
                                    IsEnable = g.IsEnable
                                });

                   
                }
   
                
                OperationItems.Add(new OneItemOperation(tmp)
                                       {
                                           OperationName = t.OpeName,
                                           OperationDesc = t.OpeDesc,
                                           //CurrentSelectLux = currentLux,
                                           //CurrentSelectLux2 = currentLux2,
                                           MaxLux = t.MaxValue,
                                           LastTimeHour = t.MaxValue/60,
                                           LastTimeMinute = t.MaxValue%60,
                                           SelectedItems = tmp
                                       });
            }

        }

        
    }

    /// <summary>
    /// 隧道方案扼要信息
    /// </summary>
    public partial class TunnelInformation
    {
        private int _schemeId;

        //光控改变时的操作判断
        public bool LuxChanged;

        /// <summary>
        ///方案Id  方案地址由服务器设置，新增的方案地址全部为0提交服务器后服务器分配
        /// </summary>
        public int SchemeId
        {
            get { return _schemeId; }
            set
            {
                if (value != _schemeId)
                {
                    _schemeId = value;
                    this.RaisePropertyChanged(() => this.SchemeId);
                }
            }
        }


        private int _areaId;

        /// <summary>
        ///当前区域ID
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


        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _schemeName;
        /// <summary>
        /// 方案名称  
        /// </summary>
        public string SchemeName
        {
            get { return _schemeName ; }
            set
            {
                if (value != _schemeName)
                {
                    _schemeName = value;
                    this.RaisePropertyChanged(() => this.SchemeName);
                }
            }
        }




        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _tunnelName;
        /// <summary>
        /// 隧道名称  
        /// </summary>
        public string TunnelName
        {
            get { return _tunnelName; }
            set
            {
                if (value != _tunnelName)
                {
                    _tunnelName = value;
                    this.RaisePropertyChanged(() => this.TunnelName);
                }
            }
        }



        private string _controlMode;
        /// <summary>
        /// 控制方式  
        /// </summary>
        public string ControlMode
        {
            get { return _controlMode; }
            set
            {
                if (value != _controlMode)
                {
                    _controlMode = value;
                    this.RaisePropertyChanged(() => this.ControlMode);
                }
            }
        }

        

        private int _subOperationCount;
        /// <summary>
        ///子操作数量
        /// </summary>
        public int SubOperationCount
        {
            get { return _subOperationCount; }
            set
            {
                if (value != _subOperationCount)
                {
                    _subOperationCount = value;
                    this.RaisePropertyChanged(() => this.SubOperationCount);
                }
            }
        }


        private int _isLuxOrTime;
        /// <summary>
        ///控制类型 1、光控，2、时控
        /// </summary>
        public int IsLuxOrTime
        {
            get { return _isLuxOrTime; }
            set
            {
                if (value != _isLuxOrTime)
                {
                    _isLuxOrTime = value;
                    this.RaisePropertyChanged(() => this.IsLuxOrTime);
                    IsLuxOrTimeStr = "未知";
                    if (value == 1)
                    {
                        IsLuxOrTimeStr = "光控";
                        Visi=Visibility.Visible;
                        Visi1=Visibility.Hidden;
                    }
                    if (value == 2)
                    {
                        IsLuxOrTimeStr = "时控";
                        Visi1 = Visibility.Visible;
                        Visi = Visibility.Hidden;
                    }
                }
            }
        }

        private string _isLuxOrTimeStr;
        /// <summary>
        /// 控制方式
        /// </summary>
        public string IsLuxOrTimeStr
        {
            get { return _isLuxOrTimeStr; }
            set
            {
                if (value != _isLuxOrTimeStr)
                {
                    _isLuxOrTimeStr = value;
                    this.RaisePropertyChanged(() => this.IsLuxOrTimeStr);
                }
            }
        }



        private Visibility _txtVisi;

        /// <summary>
        /// 选中方案是否能显示
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

        private Visibility _txtVisi1;

        /// <summary>
        /// 选中方案是否能显示
        /// </summary>
        public Visibility Visi1
        {
            get { return _txtVisi1; }
            set
            {
                if (value != _txtVisi1)
                {
                    _txtVisi1 = value;
                    this.RaisePropertyChanged(() => this.Visi1);
                }
            }
        }



        private int _protectTime;
        /// <summary>
        ///保护时间
        /// </summary>
        public int ProtectTime
        {
            get { return _protectTime; }
            set
            {
                if (value != _protectTime)
                {
                    if (value < 0) value = 0;
                    _protectTime = value;
                    this.RaisePropertyChanged(() => this.ProtectTime);
                }
            }
        }


        #region LuxCollection

        private ObservableCollection<IdNameDesc> _luxCollection;
        public ObservableCollection<IdNameDesc> LuxCollection
        {
            get
            {
                if (_luxCollection == null)
                {
                    _luxCollection = new ObservableCollection<IdNameDesc>();
                    _luxCollection.Add(new IdNameDesc { Id = 0, Name = " " });
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        _luxCollection.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
                    }
                    if (LuxId > 0)
                    {
                        foreach (var t in _luxCollection.Where(t => t.Id == LuxId))
                        {
                            CurrentSelectLux = t;
                            break;
                        }
                    }
                }
                return _luxCollection;
            }
            set
            {
                if (_luxCollection == value) return;
                _luxCollection = value;
                RaisePropertyChanged(() => LuxCollection);
            }
        }
        #endregion

        #region CurrentSelectLux
        private IdNameDesc _currentSelectLux;

        /// <summary>
        /// 当前选中的光控
        /// </summary>
        public IdNameDesc CurrentSelectLux
        {
            get
            {
                return _currentSelectLux ?? (_currentSelectLux = new IdNameDesc());
            }
            set
            {
                if (_currentSelectLux == value) return;
                _currentSelectLux = value;
                RaisePropertyChanged(() => CurrentSelectLux);
                if (_currentSelectLux != null)
                    LuxId = _currentSelectLux.Id;


                //if (_currentSelectLux.Id>0)
                //    ShowCurrentSelectLux2 = Visibility.Visible;
                //else
                //    ShowCurrentSelectLux2 = Visibility.Hidden;

            }
        }

        #endregion

        #region LuxCollection2

        private ObservableCollection<IdNameDesc> _luxCollection2;
        public ObservableCollection<IdNameDesc> LuxCollection2
        {
            get
            {
                if (_luxCollection2 == null)
                {
                    _luxCollection2 = new ObservableCollection<IdNameDesc>();
                    _luxCollection2.Add(new IdNameDesc { Id = 0, Name = " " });
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        if (t.Value != CurrentSelectLux.Id)
                        {
                            _luxCollection2.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
                        }

                    }


                    if (LuxId2 != 0)
                    {
                        foreach (var t in _luxCollection2.Where(t => t.Id == LuxId2))
                        {
                            CurrentSelectLux2 = t;
                            break;
                        }
                    }
                }



                return _luxCollection2;
            }
            set
            {
                if (_luxCollection2 == value) return;
                _luxCollection2 = value;
                RaisePropertyChanged(() => LuxCollection2);
            }
        }
        #endregion

        #region CurrentSelectLux2
        private IdNameDesc _currentSelectLux2;

        /// <summary>
        /// 备用光控
        /// </summary>
        public IdNameDesc CurrentSelectLux2
        {
            get { return _currentSelectLux2 ?? (_currentSelectLux = new IdNameDesc()); }
            set
            {
                if (_currentSelectLux2 == value) return;
                _currentSelectLux2 = value;
                RaisePropertyChanged(() => CurrentSelectLux2);
                if (_currentSelectLux2 != null)
                    LuxId2 = _currentSelectLux2.Id;
            }
        }

        #endregion


        private int _luxid;

        /// <summary>
        /// 该时间表使用的光控探头ID
        /// </summary>
        public int LuxId
        {
            get
            {
                return _luxid;
            }
            set
            {
                if (_luxid == value) return;
                _luxid = value;
                foreach (var t in LuxCollection.Where(t => t.Id == value))
                {
                    CurrentSelectLux = t;
                    LuxName = t.Name;
                    LuxChanged = false;
                    break;
                }
                RaisePropertyChanged(() => LuxId);

            }
        }

        private int _luxid2;

        /// <summary>
        /// 该时间表使用的光控探头ID
        /// </summary>
        public int LuxId2
        {
            get
            {
                return _luxid2;
            }
            set
            {
                if (_luxid2 == value) return;
                _luxid2 = value;
                foreach (var t in LuxCollection2.Where(t => t.Id == value))
                {
                    CurrentSelectLux2 = t;
                    LuxName2 = t.Name;
                    break;
                }
                RaisePropertyChanged(() => LuxId2);
            }
        }

        private string _luxname;
        /// <summary>
        /// 光控名称
        /// </summary>
        public string LuxName
        {
            get { return _luxname; }
            set
            {
                if (_luxname != value)
                {
                    _luxname = value;
                    this.RaisePropertyChanged(() => this.LuxName);
                }
            }
        }

        private string _luxname2;

        /// <summary>
        /// 光控名称
        /// </summary>
        public string LuxName2
        {
            get { return _luxname2; }
            set
            {
                if (_luxname2 != value)
                {
                    _luxname2 = value;
                    this.RaisePropertyChanged(() => this.LuxName2);
                }
            }
        }


        public class IdNameDesc : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _id;

            /// <summary>
            /// 光控Id
            /// </summary>
            public int Id
            {
                get { return _id; }
                set
                {
                    if (_id != value)
                    {
                        _id = value;
                        RaisePropertyChanged(() => Id);
                    }
                }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        RaisePropertyChanged(() => Name);
                    }
                }
            }


            private string _luxNsdfame;

            public string NameDesc
            {
                get { return _luxNsdfame; }
                set
                {
                    if (_luxNsdfame != value)
                    {
                        _luxNsdfame = value;
                        RaisePropertyChanged(() => NameDesc);
                    }
                }
            }


        }






        /// <summary>
        /// 记录
        /// </summary>
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


        private bool _isSelectlightEnable;
        /// <summary>
        /// 光控按钮是否能操作
        /// </summary>
        public bool IsSelectlightEnable
        {
            get { return _isSelectlightEnable; }
            set
            {
                if (value != _isSelectlightEnable)
                {
                    _isSelectlightEnable = value;
                    this.RaisePropertyChanged(() => this.IsSelectlightEnable);
                }

            }
        }


        private bool _isSelectTimeEnable;
        /// <summary>
        /// 时控按钮是否能操作
        /// </summary>
        public bool IsSelectTimeEnable
        {
            get { return _isSelectTimeEnable; }
            set
            {
                if (value != _isSelectTimeEnable)
                {
                    _isSelectTimeEnable = value;
                    this.RaisePropertyChanged(() => this.IsSelectTimeEnable);
                }

            }
        }
    }

    /// <summary>
    /// ICommand 增加删除操作
    /// </summary>
    public partial class TunnelInformation
    {
        /// <summary>
        /// 增加操作
        /// </summary>

        #region CmdAdd
        private DateTime _dtCmdAdd;
        private ICommand _cmdAdd;

        public ICommand CmdAdd
        {
            get
            {
                return _cmdAdd ??
                       (_cmdAdd = new RelayCommand(ExCmdAdd, CanCmdAdd, true));
            }
        }

        private bool CanCmdAdd()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && CurrentSelectedTerminalItems.Count > 0 &&
                   DateTime.Now.Ticks - _dtCmdAdd.Ticks > 10000000;
        }
        private void ExCmdAdd()
        {
            _dtCmdAdd = DateTime.Now;



            //SelectedItem.Clear();
            var tmp = new ObservableCollection<OneItemTerminal>();
            foreach (var t in CurrentSelectedTerminalItems)
            {
                var middleItems = new ObservableCollection<TimeInfoName>();     
                foreach (var f in t.Items)
                {

                    
                    middleItems.Add(new TimeInfoName
                                        {
                                            IsCheckSwitch = f.IsCheckSwitch,
                                            TimeTableName = f.TimeTableName,
                                            IsEnabledOn = f.IsEnabledOn
                                        });
                }
              
                tmp.Add(new OneItemTerminal
                               {
                                   Index = t.Index,
                                   IsSelected = t.IsSelected,
                                   IsEnable = true,
                                   RtuId = t.RtuId,
                                   RtuName = t.RtuName,
                                   Items = middleItems
                               });
            }


            var oti = new OneItemOperation(tmp);
            OperationItems.Add(oti);
            CurrentSelectedOperationItem = oti;
        }

        #endregion

        /// <summary>
        /// 删除操作
        /// </summary>

        #region CmdDelete
        private DateTime _dtCmdDelete;
        private ICommand _cmdDelete;

        public ICommand CmdDelete
        {
            get
            {
                return _cmdDelete ??
                       (_cmdDelete = new RelayCommand(ExCmdDelete, CanCmdDelete, true));
            }
        }

        private bool CanCmdDelete()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   DateTime.Now.Ticks - _dtCmdDelete.Ticks > 10000000 && CurrentSelectedOperationItem != null;
        }
        private void ExCmdDelete()
        {
            _dtCmdDelete = DateTime.Now;
            if (OperationItems.Contains(CurrentSelectedOperationItem))
            {
                OperationItems.Remove(CurrentSelectedOperationItem);
                if (OperationItems.Count > 0) 
                    CurrentSelectedOperationItem = OperationItems[0];
            }
        }

        #endregion

        /// <summary>
        /// 选择终端
        /// </summary>
        #region CmdSelectTerminal

        private SelectTerminal _addTerminalInfo = null;
        private DateTime _dtCmdSelectTerminal;
        private ICommand _cmdSelectTerminal;

        public ICommand CmdSelectTerminal
        {
            get
            {
                return _cmdSelectTerminal ??
                       (_cmdSelectTerminal = new RelayCommand(ExCmdSelectTerminal, CanCmdSelectTerminal, true));
            }
        }

        private bool CanCmdSelectTerminal()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSelectTerminal.Ticks > 10000000;
        }

        private void ExCmdSelectTerminal()
        {
            _dtCmdSelectTerminal = DateTime.Now;
            _addTerminalInfo=new SelectTerminal();
            var tml = new TunnelInformation(TunnelInfoSetVm.AreaId1);
            GetTerminal(TunnelInfoSetVm.AreaId1);
            foreach (var t in CurrentSelectedTerminalItems)
            {
                foreach (var f in SelectedTerminalItems)
                {
                    if (t.RtuId == f.RtuId)
                        f.IsSelected = true;
                }
            }
            tml.SelectedTerminalItems = SelectedTerminalItems;
            _addTerminalInfo.OnFormBtnOkClick +=
                    new EventHandler<SelectTerminal.EventArgsSelectTerminal>(_addTerminalInfo_OnFormBtnOkClick);
            _addTerminalInfo.SetContext(tml);
            _addTerminalInfo.ShowDialog();
        }

        #endregion


        //获取该区域所有类型为“隧道”的终端
        private void GetTerminal(int areaId)
        {
            
            SelectedTerminalItems.Clear();
            int index = 0;
            var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(areaId);
            var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaId);
            if (grp == null) return;
            var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml, areaId, true);

            foreach (var f in gprs.Item1)
            {
                var switchInfo = new ObservableCollection<TimeInfoName>();
                if (tmlLstOfArea.Contains(f) == false) continue;
                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                
                var vol = para as Wj3005Rtu;
               
                //if (vol != null && vol.WjVoltage.RtuUsedType == 0)
                if (vol != null)
                {
                    var wjswout = (from t in vol.WjSwitchOuts orderby t.Key select t).ToList();
                    foreach (var t in wjswout)
                    {
                        switchInfo.Add(new TimeInfoName
                                           {
                                               TimeTableName = t.Value.SwitchName,
                                               IsCheckSwitch = false,
                                               IsEnabledOn = true
                                           });
                    }
                    int x = switchInfo.Count;
                    for (int i = 0; i < 8 - x; i++)
                    {
                        switchInfo.Add(new TimeInfoName
                                           {
                                               IsEnabledOn = false
                                           });
                    }
                    index++;
                    SelectedTerminalItems.Add(new OneItemTerminal
                                                  {
                                                      Index = index,
                                                      IsSelected = false,
                                                      RtuId = para.RtuId,
                                                      RtuName = para.RtuName,
                                                      IsEnable = true,
                                                      IsEnableUsed = true,
                                                      Items = switchInfo
                                                  });
                }
            }
            var sortLst =
                Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(gprs.Item2);
            foreach (var f in sortLst)
            {
                var switchInfo = new ObservableCollection<TimeInfoName>();
                if (tmlLstOfArea.Contains(f) == false) continue;
                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                var vol = para as Wj3005Rtu;

                //if (vol != null && vol.WjVoltage.RtuUsedType == 0)
                if (vol != null)
                {
                    var wjswout = (from t in vol.WjSwitchOuts orderby t.Key select t).ToList();
                    foreach (var t in wjswout)
                    {
                        switchInfo.Add(new TimeInfoName
                                           {
                                               TimeTableName = t.Value.SwitchName,
                                               IsCheckSwitch = false,
                                               IsEnabledOn = true
                                           });
                    }
                    int y = switchInfo.Count;
                    for (int i = 0; i < 8 - y; i++)
                    {
                        switchInfo.Add(new TimeInfoName
                                           {
                                               IsEnabledOn = false
                                           });
                    }
                    index++;
                    SelectedTerminalItems.Add(new OneItemTerminal
                                                  {
                                                      Index = index,
                                                      IsSelected = false,
                                                      RtuId = para.RtuId,
                                                      RtuName = para.RtuName,
                                                      IsEnable = true,
                                                      IsEnableUsed = true,
                                                      Items = switchInfo
                                                  });
                }
            }

            foreach (var t in SelectedTerminalItems)
            {
                foreach (var f in TunnelInfoSetVm.PassTunnelItems)
                {
                    foreach (var x in f.CurrentSelectedTerminalItems)
                    {
                        if (t.RtuId == x.RtuId)
                            t.OwnerScheme = f.SchemeName;
                    }
                }
                if (t.OwnerScheme != null)
                {
                    bool x=false ;
                    if (CurrentSelectedTerminalItems.Count == 0)
                        t.IsEnableUsed = false;
                    else
                        foreach (var i in CurrentSelectedTerminalItems)
                        {
                            if (i.RtuId == t.RtuId)
                                x = true;                               
                        }
                    if (x == false)
                        t.IsEnableUsed = false;
                }
            }
            
        }


        private void _addTerminalInfo_OnFormBtnOkClick(object sender, SelectTerminal.EventArgsSelectTerminal e) //todo 暂存
        {
            CurrentSelectedTerminalItems.Clear();//"选择终端"列表清空
            var selectedItems = new ObservableCollection<OneItemTerminal>();
            var updateinfo = e.SelectTerminalInfo;
            if (updateinfo == null) return;

            foreach (var t in SelectedTerminalItems)
            {
                if (t.IsSelected)
                {
                    CurrentSelectedTerminalItems.Add(t); //"选择终端"列表添加
                }
            }

            //增加或删除操作中的终端
            if (OperationItems.Count != 0)
            {
                var x = OperationItems[0].SelectedItems;
                foreach (var t in x)
                {
                    foreach (var f in SelectedTerminalItems)
                    {
                        if(t.RtuId==f.RtuId)
                            selectedItems.Add(f);
                    }
                }




                // 各操作根据 CurrentSelectedTerminalItems 选中设备 增加或删除终端  
                var en = selectedItems.Intersect(CurrentSelectedTerminalItems).ToList(); //交集
                IEnumerable<OneItemTerminal> en1 = CurrentSelectedTerminalItems.Except(en).ToList();  //差集
                IEnumerable<OneItemTerminal> en2 = selectedItems.Except(en).ToList();  //差集
                foreach (var t in OperationItems)
                {
                    
                    foreach (var f1 in en1)
                    {
                        var middleItems1 = new ObservableCollection<TimeInfoName>();
                        foreach (var f in f1.Items)
                        {


                            middleItems1.Add(new TimeInfoName
                            {
                                IsCheckSwitch = f.IsCheckSwitch,
                                TimeTableName = f.TimeTableName,
                                IsEnabledOn = f.IsEnabledOn
                            });
                        }

                        t.SelectedItems.Add(new OneItemTerminal
                        {
                            Index = f1.Index,
                            IsSelected = f1.IsSelected,
                            IsEnable = true,
                            RtuId = f1.RtuId,
                            RtuName = f1.RtuName,
                            Items = middleItems1
                        }); 
                    }
                    foreach (var f2 in en2)
                    {
                        t.SelectedItems.Remove(t.SelectedItems.FirstOrDefault(p => p.RtuId == f2.RtuId));
                    }
                    
                }
            }
            try
            {
                _addTerminalInfo.OnFormBtnOkClick -=
                    new EventHandler<SelectTerminal.EventArgsSelectTerminal>(_addTerminalInfo_OnFormBtnOkClick);
            }
            catch (Exception ex)
            {

            }
            _addTerminalInfo = null;
        }

        
        private ObservableCollection<OneItemOperation> _sOperationItems;
        /// <summary>
        /// 操作数据集合
        /// </summary>
        public ObservableCollection<OneItemOperation> OperationItems
        {
            get
            {
                if (_sOperationItems == null) _sOperationItems = new ObservableCollection<OneItemOperation>();
                return _sOperationItems;
            }
            set
            {
                if (_sOperationItems != value)
                {
                    _sOperationItems = value;
                    this.RaisePropertyChanged(() => this.OperationItems);
                }
            }
        }

        private OneItemOperation _currentSelectedOperationItem;
        /// <summary>
        /// 当前选中操作
        /// </summary>
        public OneItemOperation CurrentSelectedOperationItem
        {
            get { return _currentSelectedOperationItem; }
            set
            {
                if (_currentSelectedOperationItem == value || value == null) return;
                _currentSelectedOperationItem = value;
                RaisePropertyChanged(() => CurrentSelectedOperationItem);
            }
        }

        private ObservableCollection<OneItemTerminal> _selectedTerminalItems;
        /// <summary>
        /// 所选区域下符合条件的终端
        /// </summary>
        public ObservableCollection<OneItemTerminal> SelectedTerminalItems
        {
            get { return _selectedTerminalItems ?? (_selectedTerminalItems = new ObservableCollection<OneItemTerminal>()); }
            set
            {
                if (_selectedTerminalItems != value)
                {
                    _selectedTerminalItems = value;
                    this.RaisePropertyChanged(() => this.SelectedTerminalItems);
                }
            }
        }

        public static ObservableCollection<OneItemTerminal> PassTerminalItems;
       
        private ObservableCollection<OneItemTerminal> _currentSelectedTerminalItems;
        /// <summary>
        /// 该方案中生效终端
        /// </summary>
        public ObservableCollection<OneItemTerminal> CurrentSelectedTerminalItems
        {
            get
            {
                PassTerminalItems = _currentSelectedTerminalItems;
                return _currentSelectedTerminalItems ??
                       (_currentSelectedTerminalItems = new ObservableCollection<OneItemTerminal>());
            }
            set
            {
                if (_currentSelectedTerminalItems != value)
                {
                    _currentSelectedTerminalItems = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectedTerminalItems);
                }
            }
        }

        ////所选终端
        //private ObservableCollection<OneItemTerminal> _selectedItem;

        //public ObservableCollection<OneItemTerminal> SelectedItem
        //{
        //    get
        //    {
        //        if (_selectedItem == null)
        //            _selectedItem = new ObservableCollection<OneItemTerminal>();
        //        return _selectedItem;
        //    }
        //    set
        //    {
        //        if (_selectedItem != value)
        //        {
        //            _selectedItem = value;
        //            this.RaisePropertyChanged(() => this.SelectedItem);
        //        }
        //    }
        //}

    }
}
