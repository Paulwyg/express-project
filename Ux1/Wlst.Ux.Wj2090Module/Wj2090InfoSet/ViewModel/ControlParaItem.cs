using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel;
 


namespace Wlst.Ux.Wj2090Module.ControlInfoSet.ViewModel
{
    public class ControlParaItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        /// <summary>
        /// 序号
        /// </summary>
        #region Index

        private int _rtuId;
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId == value) return;
                _rtuId = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }
        #endregion

        /// <summary>
        /// 开灯序号
        /// </summary>
        #region LightIndex
        private int _lightIndex;
        public int LightIndex
        {
            get { return _lightIndex; }
            set
            {
                if (_lightIndex == value) return;
                _lightIndex = value;
                RaisePropertyChanged(() => LightIndex);
            }
        }
        #endregion

        /// <summary>
        /// 条形码
        /// </summary>
        #region BarCodeId

        private long _barCodeId;
        public long BarCodeId
        {
            get { return _barCodeId; }
            set
            {
                if (_barCodeId == value) return;
                _barCodeId = value;
                RaisePropertyChanged(() => BarCodeId);
            }
        }

        #endregion

        #region BarCode
        private string _barCode;
        private string barCode;
        public string BarCode
        {
            get { return _barCode; }
            set
            {
                if (_barCode == value) return;
                _barCode = value;
                if (_barCode != null)
                {
                    _barCode = _barCode.Replace(" ", "");
                    _barCode = string.Format("{0:D13}", long.Parse(_barCode));
                    for (int i = 0; i < 3; i++)
                    {
                        _barCode = _barCode.Insert(4 * i + 3, " ");
                    }
                }
                RaisePropertyChanged(() => BarCode);
                if (_barCode != "")
                {
                    barCode = _barCode;
                    barCode = barCode.Replace(" ", "");
                    BarCodeId = Convert.ToInt64(barCode);
                }

            }
        }
        #endregion

        /// <summary>
        /// 灯杆编码
        /// </summary>
        #region LampCode
        private string _lampCode;
        public string LampCode
        {
            get { return _lampCode; }
            set
            {
                if (_lampCode == value) return;
                _lampCode = value;
                RaisePropertyChanged(() => LampCode);
            }
        }
        #endregion


        public event EventHandler<AttriChangedArgs> OnAttriChanged;

        /// <summary>
        /// 主动报警
        /// </summary>
        #region IsActiveAlarm
        private bool _isActiveAlarm;
        public bool IsActiveAlarm
        {
            get { return _isActiveAlarm; }
            set
            {
                if (_isActiveAlarm == value) return;
                _isActiveAlarm = value;
                RaisePropertyChanged(() => IsActiveAlarm);

                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() {AttriIndex = 1});
            }
        }
        #endregion

        /// <summary>
        /// 投运
        /// </summary>
        #region IsRun
        private bool _isRun;
        public bool IsRun
        {
            get { return _isRun; }
            set
            {
                if (_isRun == value) return;
                _isRun = value;
                RaisePropertyChanged(() => IsRun);

                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 2 });
            }
        }
        #endregion

        /// <summary>
        /// 回路数量
        /// </summary>
        #region LoopNumItems
        private ObservableCollection<NameValueInt> _loopNumItems = null;
        public ObservableCollection<NameValueInt> LoopNumItems
        {
            get 
            {
                if (_loopNumItems == null)
                {
                    _loopNumItems = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopNumItems.Add(new NameValueInt() { Name = "LoopNum", Value = i });
                    }
                    CurrentSelectLoopNumItem = _loopNumItems[3];
                }
                return _loopNumItems; 
            }
        }

        #endregion

        #region CurrentSelectLoopNumItem

        private NameValueInt _currentSelectLoopNumItem;
        public NameValueInt CurrentSelectLoopNumItem
        {
            get { return _currentSelectLoopNumItem; }
            set
            {
                if (_currentSelectLoopNumItem == value) return;
                _currentSelectLoopNumItem = value;
                RaisePropertyChanged(() => CurrentSelectLoopNumItem);
                foreach (var tttt in IsEnableByLoop)
                {
                    tttt.IsSelected = false;
                    if (tttt.Value <= _currentSelectLoopNumItem.Value)
                        tttt.IsSelected = true;
                }
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 3 });
            }
        }
        #endregion

        /// <summary>
        /// 根据回路数判断控件的可用状态
        /// </summary>
        ObservableCollection<NameIntBool> _isEnableByLoop;
        public ObservableCollection<NameIntBool> IsEnableByLoop
        {
            get
            {
                if (_isEnableByLoop == null)
                {
                    _isEnableByLoop = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _isEnableByLoop.Add(
                            new NameIntBool() { IsSelected = false, Name = "IsEnableByLoop", Value = i });
                    }

                }
                return _isEnableByLoop;
            }
        }

        /// <summary>
        /// 上电开灯1-4
        /// </summary>
        //ObservableCollection<NameIntBool> _isPowerOnLight;
        //public ObservableCollection<NameIntBool> IsPowerOnLight
        //{
        //    get
        //    {
        //        if (_isPowerOnLight == null)
        //        {
        //            _isPowerOnLight = new ObservableCollection<NameIntBool>();
        //            for (int i = 1; i <= 4; i++)
        //            {
        //                _isPowerOnLight.Add(
        //                    new NameIntBool() { IsSelected = true, Name = "IsPowerOnLight", Value = i });
        //            }

        //        }
        //        return _isPowerOnLight;
        //    }
        //}
        #region IsPowerOnLight1
        private bool _isPowerOnLight1;
        public bool IsPowerOnLight1
        {
            get { return _isPowerOnLight1; }
            set
            {
                if (_isPowerOnLight1 == value) return;
                _isPowerOnLight1 = value;
                RaisePropertyChanged(() => IsPowerOnLight1);

                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 4 });
            }
        }
        #endregion

        #region IsPowerOnLight2
        private bool _isPowerOnLight2;
        public bool IsPowerOnLight2
        {
            get { return _isPowerOnLight2; }
            set
            {
                if (_isPowerOnLight2 == value) return;
                _isPowerOnLight2 = value;
                RaisePropertyChanged(() => IsPowerOnLight2);

                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 5 });
            }
        }
        #endregion

        #region IsPowerOnLight3
        private bool _isPowerOnLight3;
        public bool IsPowerOnLight3
        {
            get { return _isPowerOnLight3; }
            set
            {
                if (_isPowerOnLight3 == value) return;
                _isPowerOnLight3 = value;
                RaisePropertyChanged(() => IsPowerOnLight3);

                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 6 });
            }
        }
        #endregion

        #region IsPowerOnLight4
        private bool _isPowerOnLight4;
        public bool IsPowerOnLight4
        {
            get { return _isPowerOnLight4; }
            set
            {
                if (_isPowerOnLight4 == value) return;
                _isPowerOnLight4 = value;
                RaisePropertyChanged(() => IsPowerOnLight4);

                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 7 });
            }
        }
        #endregion

        /// <summary>
        /// 回路1-4矢量
        /// </summary>
        #region LoopVectorItems1

        private ObservableCollection<NameValueInt> _loopVectorItems1 = null;
        public ObservableCollection<NameValueInt> LoopVectorItems1
        {
            get
            {
                if (_loopVectorItems1 == null)
                {
                    _loopVectorItems1 = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopVectorItems1.Add(new NameValueInt() { Name = "LoopVector", Value = i });
                    }
                    CurrentSelectLoopVectorItem1 = _loopVectorItems1[0];
                }
                return _loopVectorItems1;
            }
        }

        #endregion

        #region CurrentSelectLoopVectorItem1

        private NameValueInt _currentSelectLoopVectorItem1;
        public NameValueInt CurrentSelectLoopVectorItem1
        {
            get { return _currentSelectLoopVectorItem1; }
            set
            {
                if (_currentSelectLoopVectorItem1 == value) return;
                _currentSelectLoopVectorItem1 = value;
                RaisePropertyChanged(() => CurrentSelectLoopVectorItem1);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 8 });
            }
        }
        #endregion
        
        #region LoopVectorItems2
        private ObservableCollection<NameValueInt> _loopVectorItems2 = null;
        public ObservableCollection<NameValueInt> LoopVectorItems2
        {
            get
            {
                if (_loopVectorItems2 == null)
                {
                    _loopVectorItems2 = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopVectorItems2.Add(new NameValueInt() { Name = "LoopVector", Value = i });
                    }
                    CurrentSelectLoopVectorItem2 = _loopVectorItems2[1];
                }
                return _loopVectorItems2;
            }
        }

        #endregion

        #region CurrentSelectLoopVectorItem2

        private NameValueInt _currentSelectLoopVectorItem2;
        public NameValueInt CurrentSelectLoopVectorItem2
        {
            get { return _currentSelectLoopVectorItem2; }
            set
            {
                if (_currentSelectLoopVectorItem2 == value) return;
                _currentSelectLoopVectorItem2 = value;
                RaisePropertyChanged(() => CurrentSelectLoopVectorItem2);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 9 });
            }
        }
        #endregion

        #region LoopVectorItems3
        private ObservableCollection<NameValueInt> _loopVectorItems3 = null;
        public ObservableCollection<NameValueInt> LoopVectorItems3
        {
            get
            {
                if (_loopVectorItems3 == null)
                {
                    _loopVectorItems3 = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopVectorItems3.Add(new NameValueInt() { Name = "LoopVector", Value = i });
                    }
                    CurrentSelectLoopVectorItem3 = _loopVectorItems3[2];

                }
                return _loopVectorItems3;
            }
        }

        #endregion

        #region CurrentSelectLoopVectorItem3

        private NameValueInt _currentSelectLoopVectorItem3;
        public NameValueInt CurrentSelectLoopVectorItem3
        {
            get { return _currentSelectLoopVectorItem3; }
            set
            {
                if (_currentSelectLoopVectorItem3 == value) return;
                _currentSelectLoopVectorItem3 = value;
                RaisePropertyChanged(() => CurrentSelectLoopVectorItem3);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 10 });
            }
        }
        #endregion

        #region LoopVectorItems4
        private ObservableCollection<NameValueInt> _loopVectorItems4 = null;
        public ObservableCollection<NameValueInt> LoopVectorItems4
        {
            get
            {
                if (_loopVectorItems4 == null)
                {
                    _loopVectorItems4 = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _loopVectorItems4.Add(new NameValueInt() { Name = "LoopVector", Value = i });
                    }
                    CurrentSelectLoopVectorItem4 = _loopVectorItems4[3];
                }
                return _loopVectorItems4;
            }
        }

        #endregion

        #region CurrentSelectLoopVectorItem4

        private NameValueInt _currentSelectLoopVectorItem4;
        public NameValueInt CurrentSelectLoopVectorItem4
        {
            get { return _currentSelectLoopVectorItem4; }
            set
            {
                if (_currentSelectLoopVectorItem4 == value) return;
                _currentSelectLoopVectorItem4 = value;
                RaisePropertyChanged(() => CurrentSelectLoopVectorItem4);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 11 });
            }
        }
        #endregion

        /// <summary>
        /// 回路1-4额定功率
        /// </summary>
        #region LoopRatePowerItems1
        private ObservableCollection<NameValueInt> _loopRatePowerItems1 = null;
        public ObservableCollection<NameValueInt> LoopRatePowerItems1
        {
            get
            {
                if (_loopRatePowerItems1 == null)
                {
                    _loopRatePowerItems1 = new ObservableCollection<NameValueInt>();
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "未设置", Value = 0 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "20", Value = 1 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "50", Value = 14 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "75", Value = 15 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "100", Value = 2 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "120", Value = 3 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "150", Value = 4 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "200", Value = 5 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "250", Value = 6 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "300", Value = 7 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "400", Value = 8 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "600", Value = 9 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "800", Value = 10 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "1000", Value = 11 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "1500", Value = 12 });
                    _loopRatePowerItems1.Add(new NameValueInt() { Name = "2000", Value = 13 });
                    CurrentSelectLoopRatePowerIndex1 = _loopRatePowerItems1[8];
                }
                return _loopRatePowerItems1;
            }
        }

        #endregion

        #region CurrentSelectLoopRatePowerIndex1

        private NameValueInt _currentSelectLoopRatePowerIndex1;
        public NameValueInt CurrentSelectLoopRatePowerIndex1
        {
            get { return _currentSelectLoopRatePowerIndex1; }
            set
            {
                if (_currentSelectLoopRatePowerIndex1 == value) return;
                _currentSelectLoopRatePowerIndex1 = value;
                RaisePropertyChanged(() => CurrentSelectLoopRatePowerIndex1);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 12 }); ;
            }
        }
        #endregion
        
        #region LoopRatePowerItems2
        private ObservableCollection<NameValueInt> _loopRatePowerItems2 = null;
        public ObservableCollection<NameValueInt> LoopRatePowerItems2
        {
            get
            {
                if (_loopRatePowerItems2 == null)
                {
                    _loopRatePowerItems2 = new ObservableCollection<NameValueInt>();
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "未设置", Value = 0 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "20", Value = 1 });
                 
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "50", Value = 14 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "75", Value = 15 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "100", Value = 2 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "120", Value = 3 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "150", Value = 4 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "200", Value = 5 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "250", Value = 6 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "300", Value = 7 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "400", Value = 8 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "600", Value = 9 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "800", Value = 10 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "1000", Value = 11 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "1500", Value = 12 });
                    _loopRatePowerItems2.Add(new NameValueInt() { Name = "2000", Value = 13 });
                  CurrentSelectLoopRatePowerIndex2 = _loopRatePowerItems2[8];
                }
                return _loopRatePowerItems2;
            }
        }

        #endregion

        #region CurrentSelectLoopRatePowerIndex2

        private NameValueInt _currentSelectLoopRatePowerIndex2;
        public NameValueInt CurrentSelectLoopRatePowerIndex2
        {
            get { return _currentSelectLoopRatePowerIndex2; }
            set
            {
                if (_currentSelectLoopRatePowerIndex2 == value) return;
                _currentSelectLoopRatePowerIndex2 = value;
                RaisePropertyChanged(() => CurrentSelectLoopRatePowerIndex2);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 13 });
            }
        }
        #endregion

        #region LoopRatePowerItems3
        private ObservableCollection<NameValueInt> _loopRatePowerItems3 = null;
        public ObservableCollection<NameValueInt> LoopRatePowerItems3
        {
            get
            {
                if (_loopRatePowerItems3 == null)
                {
                    _loopRatePowerItems3 = new ObservableCollection<NameValueInt>();
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "未设置", Value = 0 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "20", Value = 1 });
       
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "50", Value = 14 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "75", Value = 15 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "100", Value = 2 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "120", Value = 3 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "150", Value = 4 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "200", Value = 5 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "250", Value = 6 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "300", Value = 7 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "400", Value = 8 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "600", Value = 9 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "800", Value = 10 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "1000", Value = 11 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "1500", Value = 12 });
                    _loopRatePowerItems3.Add(new NameValueInt() { Name = "2000", Value = 13 });
                    CurrentSelectLoopRatePowerIndex3 = _loopRatePowerItems3[8];
                }
                return _loopRatePowerItems3;
            }
        }

        #endregion

        #region CurrentSelectLoopRatePowerIndex3

        private NameValueInt _currentSelectLoopRatePowerIndex3;
        public NameValueInt CurrentSelectLoopRatePowerIndex3
        {
            get { return _currentSelectLoopRatePowerIndex3; }
            set
            {
                if (_currentSelectLoopRatePowerIndex3 == value) return;
                _currentSelectLoopRatePowerIndex3 = value;
                RaisePropertyChanged(() => CurrentSelectLoopRatePowerIndex3);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 14 });
            }
        }
        #endregion

        #region LoopRatePowerItems4
        private ObservableCollection<NameValueInt> _loopRatePowerItems4 = null;
        public ObservableCollection<NameValueInt> LoopRatePowerItems4
        {
            get
            {
                if (_loopRatePowerItems4 == null)
                {
                    _loopRatePowerItems4 = new ObservableCollection<NameValueInt>();
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "未设置", Value = 0 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "20", Value = 1 });
                
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "50", Value = 14 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "75", Value = 15 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "100", Value = 2 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "120", Value = 3 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "150", Value = 4 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "200", Value = 5 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "250", Value = 6 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "300", Value = 7 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "400", Value = 8 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "600", Value = 9 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "800", Value = 10 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "1000", Value = 11 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "1500", Value = 12 });
                    _loopRatePowerItems4.Add(new NameValueInt() { Name = "2000", Value = 13 });
                    CurrentSelectLoopRatePowerIndex4 = _loopRatePowerItems4[8];
                }
                return _loopRatePowerItems4;
            }
        }

        #endregion

        #region CurrentSelectLoopRatePowerIndex4

        private NameValueInt _currentSelectLoopRatePowerIndex4;
        public NameValueInt CurrentSelectLoopRatePowerIndex4
        {
            get { return _currentSelectLoopRatePowerIndex4; }
            set
            {
                if (_currentSelectLoopRatePowerIndex4 == value) return;
                _currentSelectLoopRatePowerIndex4 = value;
                RaisePropertyChanged(() => CurrentSelectLoopRatePowerIndex4);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 15 });
            }
        }
        #endregion

        /// <summary>
        /// 功率上限
        /// </summary>
        #region PowerMax
        private int  _powerMax;
        public int PowerMax
        {
            get { return _powerMax; }
            set
            {
                if (_powerMax == value) return;
                _powerMax = value;
                RaisePropertyChanged(() => PowerMax);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 16 });
            }
        }
        #endregion

        /// <summary>
        /// 功率下限
        /// </summary>
        #region PowerMin
        private int _powerMin;
        public int PowerMin
        {
            get { return _powerMin; }
            set
            {
                if (_powerMin == value) return;
                _powerMin = value;
                RaisePropertyChanged(() => PowerMin);
                if (OnAttriChanged != null) OnAttriChanged(this, new AttriChangedArgs() { AttriIndex = 17 });
            }
        }
        #endregion

        /// <summary>
        /// 路由1-4，目前协议未用
        /// </summary>
        #region Route1
        private int  _route1;
        public int Route1
        {
            get { return _route1; }
            set
            {
                if (_route1 == value) return;
                _route1 = value;
                RaisePropertyChanged(() => Route1);
            }
        }
        #endregion

        #region Route2
        private int _route2;
        public int Route2
        {
            get { return _route2; }
            set
            {
                if (_route2 == value) return;
                _route2 = value;
                RaisePropertyChanged(() => Route2);
            }
        }
        #endregion

        #region Route3
        private int _route3;
        public int Route3
        {
            get { return _route3; }
            set
            {
                if (_route3 == value) return;
                _route3 = value;
                RaisePropertyChanged(() => Route3);
            }
        }
        #endregion

        #region Route4
        private int _route4;
        public int Route4
        {
            get { return _route4; }
            set
            {
                if (_route4 == value) return;
                _route4 = value;
                RaisePropertyChanged(() => Route4);
            }
        }
        #endregion

        private bool _isChecked;

        /// <summary>
        /// 是否选中该条数据
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }

        public double Xgis;
        public double Ygis;
    }
    public class AttriChangedArgs:EventArgs
    {
        public int AttriIndex;
    }
}
