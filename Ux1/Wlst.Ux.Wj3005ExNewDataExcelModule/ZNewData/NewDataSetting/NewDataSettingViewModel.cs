using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.ViewModel;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.NewDataSetting
{


    /// <summary>
    /// key：
    /// Int ：  1-99  ，
    /// string  101-199 ，
    /// boolean 201-299 （1、false，2、true ，其他 false），
    /// double  301-399 
    /// value:
    /// 值当中 [[1]] 标记为本次设置的默认值，且必须放在最开始处 ,外部为 [[默认值]]
    /// </summary>
    internal partial class SettingDesc
    {
        static int Moduleid = NewDataSettingViewModel.Moduleid;// Ux.EquipemntLightFault.Services.ViewIdAssign.ViewIdAssignBaseId + 1;
        private static bool IsInitDesc = false;
        internal static Dictionary<int, string> dicDesc = new Dictionary<int, string>();
        internal static void InitDesc()
        {
            if (IsInitDesc) return;
            IsInitDesc = true;

            dicDesc.Add(0, "本文档为最新数据自定义存储文档.");
            GetInitInt(ref dicDesc);
            GetInitString(ref dicDesc);
            GetInitBoolean(ref dicDesc);
            GetInitDouble(ref dicDesc);
            Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dicDesc, true);
        }


        /// <summary>
        /// 未设置默认值则未 0
        /// </summary>
        /// <param name="dicDesc"></param>
        static void GetInitInt(ref Dictionary<int, string> dicDesc)
        {

        }

        /// <summary>
        /// 未设置默认值则为 empty
        /// </summary>
        /// <param name="dicDesc"></param>
        static void GetInitString(ref Dictionary<int, string> dicDesc)
        {
            int baseid = 100;
 

        }

        /// <summary>
        /// 未设置默认值 则为false
        /// </summary>
        /// <param name="dicDesc"></param>
        static void GetInitBoolean(ref Dictionary<int, string> dicDesc)
        {
            int baseid = 200;
            dicDesc.Add(201, "[[1]]最新数据显示电压相位");
            dicDesc.Add(202, "[[1]]最新数据模式1下不显示无回路的开关量输出");
            dicDesc.Add(203, "[[1]]最新数据模式1下不显示未绑定时间表的输出");
        }

        /// <summary>
        /// 未设置默认值则未 0
        /// </summary>
        /// <param name="dicDesc"></param>
        static void GetInitDouble(ref Dictionary<int, string> dicDesc)
        {
            int baseid = 300;
        }
    }
    internal partial class SettingDesc
    {

        static string GetDefautString(int key)
        {
            if (dicDesc.ContainsKey(key) == false) return string.Empty;
            if (dicDesc[key].Contains("[[") && dicDesc[key].Contains("]]"))
            {
                var tmp = dicDesc[key].Substring(0, dicDesc[key].IndexOf("]]"));
                var rtn = tmp.Replace("[[", "");
                return rtn;
            }

            return string.Empty;

        }

        static int GetDefautInt(int key)
        {
            var tmp = GetDefautString(key);
            int rtn = 0;
            if (String.IsNullOrEmpty(tmp) == false)
                Int32.TryParse(tmp, out rtn);
            return rtn;
        }


        static double GetDefautDoubel(int key)
        {
            var tmp = GetDefautString(key);
            double rtn = 0;
            if (String.IsNullOrEmpty(tmp) == false)
                Double.TryParse(tmp, out rtn);
            return rtn;
        }


        static bool GetDefautBooelan(int key)
        {
            var tmp = GetDefautInt(key);
            return tmp == 2;
        }

        internal static ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> GetItems()
        {
            var _items = new ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey>();

            /// Int ：  1-99  ，
            /// string  101-199 ，
            /// boolean 201-299 （1、false，2、true ，其他 false），
            /// double  301-399 
            InitDesc();
            var keys = SettingDesc.dicDesc.Keys.ToList();
            foreach (var f in keys)
            {
                if (f < 1) continue; //特殊定义
                if (f < 100)
                {
                    var def = SettingDesc.GetDefautInt(f);

                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueInt = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetInt(Moduleid, f, def),
                    });
                }
                if (100 < f && f < 200)
                {
                    var def = SettingDesc.GetDefautString(f);
                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueString = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetString(Moduleid, f, def),
                    });
                }
                if (200 < f && f < 300)
                {
                    var def = SettingDesc.GetDefautBooelan(f);
                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueBool = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetBoolean(Moduleid, f, def),
                    });
                }
                if (300 < f && f < 400)
                {
                    var def = SettingDesc.GetDefautDoubel(f);
                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueDouble = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetDouble(Moduleid, f, def),
                    });
                }
            }
            return _items;
        }

        internal static void SaveItems(ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> items)
        {
            var dic = new Dictionary<int, string>();
            foreach (var f in items)
            {
                if (f.Key < 100)
                {
                    dic.Add(f.Key, f.Value.ValueInt + "");
                }
                if (100 < f.Key && f.Key < 200)
                {
                    dic.Add(f.Key, f.Value.ValueString + "");
                }
                if (200 < f.Key && f.Key < 300)
                {
                    dic.Add(f.Key, (f.Value.ValueBool ? 2 : 1) + "");
                }
                if (300 < f.Key && f.Key < 400)
                {
                    dic.Add(f.Key, f.Value.ValueDouble + "");
                }
            }

            Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dic, false);
        }
    }

    public partial class NewDataSettingViewModel
    {
        internal static int Moduleid = Ux.Wj3005ExNewDataExcelModule.Services.ViewIdAssign.ViewIdAssignBaseId + 1;
        private ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> _items = null;

        public ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> Items
        {
            get
            {
                if (_items == null || _items.Count == 0)
                {
                    if (_items == null)
                        _items = new ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey>();

                    var tmpDadta = SettingDesc.GetItems();
                    foreach (var f in tmpDadta)
                    {
                        if (_items.ContainsKey(f.Key) == false) _items.Add(f.Key, f.Value);
                    }
                }
                return _items;
            }

        }

    }


    [Export(typeof (IINewDataSetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataSettingViewModel : ObservableObject, IINewDataSetting
    {
        public NewDataSettingViewModel()
        {
            //this.NavOnLoad();

            this.RowHeight = TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight;
            this.TimeNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength;
            LoopNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength;
            VaNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength;
            KxNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.KxNameLength;
            BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor;
            K1BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor;
            K2BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor;
            K3BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor;
            K4BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor;
            K5BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor;
            K6BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor;
            K7BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K7BackgroundColor;
            K8BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K8BackgroundColor;

            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].IsSelected = TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(i);
            }

            //   ShowDw = true;

            var tmp = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 1); //数据查询倒序呈现
            this.IsShowReverseData = tmp == true;//? tmp.Value : false;

            var tmp1 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 2); //最新数据呈现模式
            this.NewDataShowMode = tmp1 == -1 ? 1 : tmp1;//? tmp.Value : false;

            IsShowShieldMark = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 5, false); //最新数据显示屏蔽标识
            IsShowComboBox = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 7, true ); //最新数据显示下拉框
            IsShowSumPower = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 8, false); //终端巡测及数据查询界面显示三相功率和总功率

            NewDataWidth = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 13, 600);//最新数据 宽度
            IsShowAbc=Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 14, true);//是否显示 ABC 电流

            OpExtendShow = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt( 2801, 15, 1);// 最新数据 额外显示内容

            IsStateBarShowPhyId = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 16, false); //状态栏显示物理地址
            IsStateBarShowRtuName = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 17, false); //状态栏显示终端名称
            IsStateBarShowGrpName = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 18, false); //状态栏显示终端分组
            IsStateBarShowRemark = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 19, false); //状态栏显示终端备注

            this.IsCloseMeasureNewData = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 20, false);  //选测是否关闭最新数据界面
            this.MeasureOverTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 21, 5);   //选测多久后判断失败  默认值为5s


        }

        #region  define

        private int _rowHeight;

        public int RowHeight
        {
            get { return _rowHeight; }
            set
            {
                if (value != _rowHeight)
                {
                    _rowHeight = value;
                    this.RaisePropertyChanged(() => this.RowHeight);
                }
            }
        }


        private int _loopNameLength;


        public int LoopNameLength
        {
            get { return _loopNameLength; }
            set
            {
                if (value != _loopNameLength)
                {
                    _loopNameLength = value;
                    this.RaisePropertyChanged(() => this.LoopNameLength);
                }
            }
        }


        private int _timeNameLength;

        public int TimeNameLength
        {
            get { return _timeNameLength; }
            set
            {
                if (value != _timeNameLength)
                {
                    _timeNameLength = value;
                    this.RaisePropertyChanged(() => this.TimeNameLength);
                }
            }
        }

        private int _vaNameLength;

        public int VaNameLength
        {
            get { return _vaNameLength; }
            set
            {
                if (value != _vaNameLength)
                {
                    _vaNameLength = value;
                    this.RaisePropertyChanged(() => this.VaNameLength);
                }
            }
        }

        private int _kxNameLength;

        [Range(30, 130, ErrorMessage = "开关量介于30-130之间")]
        public int KxNameLength
        {
            get { return _kxNameLength; }
            set
            {
                if (value != _kxNameLength)
                {
                    _kxNameLength = value;
                    this.RaisePropertyChanged(() => this.KxNameLength);
                }
            }
        }

        #region IsShowReverseData

        private bool _isShowReverseData;

        /// <summary>
        /// 数据显示倒序
        /// </summary>
        public bool IsShowReverseData
        {
            get { return _isShowReverseData; }
            set
            {
                if (_isShowReverseData == value) return;
                _isShowReverseData = value;
                RaisePropertyChanged(() => IsShowReverseData);
            }
        }

        #endregion
        //private bool   _vasdgd;

        //public bool IsShowLoopId
        //{
        //    get { return _vasdgd; }
        //    set
        //    {
        //        if (value != _vasdgd)
        //        {
        //            _vasdgd = value;
        //            this.RaisePropertyChanged(() => this.IsShowLoopId);
        //        }
        //    }
        //}






        private int _vasRtuNameLength;

        public int RtuNameLength
        {
            get { return _vasRtuNameLength; }
            set
            {
                if (value != _vasRtuNameLength)
                {
                    _vasRtuNameLength = value;
                    this.RaisePropertyChanged(() => this.RtuNameLength);
                }
            }
        }

        private int _newDataShowMode;

        public int NewDataShowMode
        {
            get { return _newDataShowMode; }
            set
            {
                if (value != _newDataShowMode)
                {
                    _newDataShowMode = value;
                    this.RaisePropertyChanged(() => this.NewDataShowMode);
                }
            }
        }


        private int _newDatawidth;
        /// <summary>
        /// 最新数据的 宽度
        /// </summary>
        public int NewDataWidth
        {
            get { return _newDatawidth; }
            set
            {
                if (value != _newDatawidth)
                {
                    _newDatawidth = value;
                    this.RaisePropertyChanged(() => this.NewDataWidth);
                }
            }
        }

        private bool _isShowABC;

        public bool IsShowAbc
        {
            get { return _isShowABC; }
            set
            {
                if (value != _isShowABC)
                {
                    _isShowABC = value;
                    this.RaisePropertyChanged(() => this.IsShowAbc);
                }
            }
        }

        private bool _isShowShieldMark;
        /// <summary>
        /// 最新数据显示屏蔽标识
        /// </summary>
        public bool IsShowShieldMark
        {
            get { return _isShowShieldMark; }
            set
            {
                if (value != _isShowShieldMark)
                {
                    _isShowShieldMark = value;
                    this.RaisePropertyChanged(() => this.IsShowShieldMark);
                }
            }
        }




        private bool _isShowComboBox;
        /// <summary>
        /// 非下拉框显示
        /// </summary>
        public bool IsShowComboBox
        {
            get { return _isShowComboBox; }
            set
            {
                if (value != _isShowComboBox)
                {
                    _isShowComboBox = value;
                    this.RaisePropertyChanged(() => this.IsShowComboBox);
                }
            }
        }


        private bool _isShowSumPower;
        /// <summary>
        /// 终端巡测及数据查询界面显示三相功率和总功率
        /// </summary>
        public bool IsShowSumPower
        {
            get { return _isShowSumPower; }
            set
            {
                if (value != _isShowSumPower)
                {
                    _isShowSumPower = value;
                    this.RaisePropertyChanged(() => this.IsShowSumPower);
                }
            }
        }

        private bool _isStateBarShowPhyId;
        /// <summary>
        /// 状态栏显示物理地址
        /// </summary>
        public bool IsStateBarShowPhyId
        {
            get { return _isStateBarShowPhyId; }
            set
            {
                if (value != _isStateBarShowPhyId)
                {
                    _isStateBarShowPhyId = value;
                    this.RaisePropertyChanged(() => this.IsStateBarShowPhyId);
                }
            }
        }

        private bool _isStateBarShowRtuName;
        /// <summary>
        /// 状态栏显示终端名称
        /// </summary>
        public bool IsStateBarShowRtuName
        {
            get { return _isStateBarShowRtuName; }
            set
            {
                if (value != _isStateBarShowRtuName)
                {
                    _isStateBarShowRtuName = value;
                    this.RaisePropertyChanged(() => this.IsStateBarShowRtuName);
                }
            }
        }

        private bool _isStateBarShowGrpName;
        /// <summary>
        /// 状态栏显示分组名称
        /// </summary>
        public bool IsStateBarShowGrpName
        {
            get { return _isStateBarShowGrpName; }
            set
            {
                if (value != _isStateBarShowGrpName)
                {
                    _isStateBarShowGrpName = value;
                    this.RaisePropertyChanged(() => this.IsStateBarShowGrpName);
                }
            }
        }


        private bool _isStateBarShowRemark;
        /// <summary>
        /// 状态栏显示终端备注
        /// </summary>
        public bool IsStateBarShowRemark
        {
            get { return _isStateBarShowRemark; }
            set
            {
                if (value != _isStateBarShowRemark)
                {
                    _isStateBarShowRemark = value;
                    this.RaisePropertyChanged(() => this.IsStateBarShowRemark);
                }
            }
        }




        private bool _isShowRealState;
        /// <summary>
        /// 最新数据显示真实状态
        /// </summary>
        public bool IsShowRealState
        {
            get { return _isShowRealState; }
            set
            {
                if (value != _isShowRealState)
                {
                    _isShowRealState = value;
                    this.RaisePropertyChanged(() => this.IsShowRealState);
                }
            }
        }
        #endregion


         #region OpExtendShow

        private int _backgrOpExtendShowoundColor;
        /// <summary>
        /// 1、无，2、第一条故障，3、备注信息
        /// </summary>
        public int OpExtendShow
        {
            get { return _backgrOpExtendShowoundColor; }
            set
            {

                if (value != _backgrOpExtendShowoundColor)
                {
                    _backgrOpExtendShowoundColor = value;
                    RaisePropertyChanged(() => this.OpExtendShow);
                }
            }
        }

        #endregion

         
        #region BackgroundColor

        private string _backgroundColor;

        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {

                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    RaisePropertyChanged(() => this.BackgroundColor);
                }
            }
        }

        #endregion

        #region K1BackgroundColor

        private string _k1BackgroundColor;

        public string K1BackgroundColor
        {
            get { return _k1BackgroundColor; }
            set
            {
                if (value != _k1BackgroundColor)
                {
                    _k1BackgroundColor = value;
                    RaisePropertyChanged(() => K1BackgroundColor);
                }
            }
        }

        #endregion

        #region K2BackgroundColor

        private string _k2BackgroundColor;

        public string K2BackgroundColor
        {
            get { return _k2BackgroundColor; }
            set
            {
                if (value != _k2BackgroundColor)
                {
                    _k2BackgroundColor = value;
                    RaisePropertyChanged(() => this.K2BackgroundColor);
                }
            }
        }

        #endregion

        #region K3BackgroundColor

        private string _k3BackgroundColor;

        public string K3BackgroundColor
        {
            get { return _k3BackgroundColor; }
            set
            {
                if (value != _k3BackgroundColor)
                {
                    _k3BackgroundColor = value;
                    RaisePropertyChanged(() => K3BackgroundColor);
                }
            }
        }

        #endregion

        #region K4BackgroundColor

        private string _k4BackgroundColor;

        public string K4BackgroundColor
        {
            get { return _k4BackgroundColor; }
            set
            {
                if (value != _k4BackgroundColor)
                {
                    _k4BackgroundColor = value;
                    RaisePropertyChanged(() => K4BackgroundColor);
                }
            }
        }

        #endregion

        #region K5BackgroundColor

        private string _k5BackgroundColor;

        public string K5BackgroundColor
        {
            get { return _k5BackgroundColor; }
            set
            {
                if (value != _k5BackgroundColor)
                {
                    _k5BackgroundColor = value;
                    RaisePropertyChanged(() => this.K5BackgroundColor);
                }
            }
        }

        #endregion

        #region K6BackgroundColor

        private string _k6BackgroundColor;

        public string K6BackgroundColor
        {
            get { return _k6BackgroundColor; }
            set
            {
                if (value != _k6BackgroundColor)
                {
                    _k6BackgroundColor = value;
                    RaisePropertyChanged(() => K6BackgroundColor);
                }
            }
        }

        #endregion

        #region K7BackgroundColor

        private string _k7BackgroundColor;

        public string K7BackgroundColor
        {
            get { return _k7BackgroundColor; }
            set
            {
                if (value != _k7BackgroundColor)
                {
                    _k7BackgroundColor = value;
                    RaisePropertyChanged(() => K7BackgroundColor);
                }
            }
        }

        #endregion

        #region K8BackgroundColor

        private string _k8BackgroundColor;

        public string K8BackgroundColor
        {
            get { return _k8BackgroundColor; }
            set
            {
                if (value != _k8BackgroundColor)
                {
                    _k8BackgroundColor = value;
                    RaisePropertyChanged(() => K8BackgroundColor);
                }
            }
        }

        #endregion



        #region IsCloseMeasureNewData

        private bool _isCloseMeasureNewData;

        /// <summary>
        /// 选测关闭最新数据框
        /// </summary>
        public bool IsCloseMeasureNewData
        {
            get { return _isCloseMeasureNewData; }
            set
            {
                if (_isCloseMeasureNewData == value) return;
                _isCloseMeasureNewData = value;
                RaisePropertyChanged(() => IsCloseMeasureNewData);
            }
        }

        #endregion

        #region MeasureOverTime

        private int _measureOverTime;

        /// <summary>
        /// 选测后,超过几秒,判定为失败  lvf 2018年3月29日08:40:50
        /// </summary>
        public int MeasureOverTime
        {
            get { return _measureOverTime; }
            set
            {
                if (_measureOverTime == value) return;
                if (value < 0) value = 0;
                if (value > 60) value = 60;
                _measureOverTime = value;

                RaisePropertyChanged(() => MeasureOverTime);
            }
        }

        #endregion


        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> item = null;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> Item
        {
            get
            {
                if (item == null)
                {
                    item = new ObservableCollection<NameIntBool>();
                    item.Add(new NameIntBool() {IsSelected = true, AreaId = 0, Value = 1}); //序号
                    item.Add(new NameIntBool() {IsSelected = true, AreaId = 0, Value = 10}); //回路名称
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 0, Value = 100}); //参考电流
                    item.Add(new NameIntBool() {IsSelected = true, AreaId = 0, Value = 1000}); //亮灯率
                    item.Add(new NameIntBool() {IsSelected = true, AreaId = 0, Value = 10000}); //功率因数



                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 1, Value = 1}); //互感器比
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 1, Value = 10}); //回路上限
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 1, Value = 100}); //回路下限
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 1, Value = 1000}); //线路状态
                    

                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 2, Value = 1}); //昨日数据
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 2, Value = 10}); //状态
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 2, Value = 100}); //电压
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 2, Value = 1000}); //电流
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 2, Value = 10000}); //功率

                    item.Add(new NameIntBool() {IsSelected = true, AreaId = 3, Value = 1}); //手动选测自动显示数据
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 3, Value = 10}); //显示回路数据电压电流等单位
                    item.Add(new NameIntBool() {IsSelected = false, AreaId = 3, Value = 100}); //历史数据查询显示高级选项
                    item.Add(new NameIntBool() { IsSelected = false, AreaId = 3, Value = 1000 }); //显示屏蔽回路电流
    
                    item.Add(new NameIntBool() { IsSelected = false, AreaId = 0, Value = 100000 }); //功率
                    item.Add(new NameIntBool() { IsSelected = false, AreaId = 3, Value = 10000 }); //显示屏蔽回路电压
                    item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 10000 }); //线路方向
                }
                return item;
            }
        }

        private DateTime _dtApply;
        private ICommand _cmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_cmdApply == null) _cmdApply = new RelayCommand(Ex, CanEx, false);
                return _cmdApply;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Ex()
        {
            _dtApply = DateTime.Now;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight = this.RowHeight;
            if (TmlNewDataViewModel.ViewModel.NewDataViewModel.Myself != null)
                TmlNewDataViewModel.ViewModel.NewDataViewModel.Myself.RowHightx = this.RowHeight;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength = this.LoopNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength = this.TimeNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength = this.VaNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.KxNameLength = this.KxNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.RtuNameLength = this.RtuNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor = this.BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor = this.K1BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor = this.K2BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor = this.K3BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor = this.K4BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor = this.K5BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor = this.K6BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K7BackgroundColor = this.K7BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K8BackgroundColor = this.K8BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.IsShowPropoery =
                (from t in Item select t.IsSelected).ToList();

            // RtuLoopInfoVm._constColor = new string[] { BackgroundColor, K1BackgroundColor, K2BackgroundColor, K3BackgroundColor, K4BackgroundColor, K5BackgroundColor, K6BackgroundColor };
            NewDataViewModel.ConstColor = new string[]
                                              {
                                                  BackgroundColor, K1BackgroundColor, K2BackgroundColor, K3BackgroundColor,
                                                  K4BackgroundColor, K5BackgroundColor, K6BackgroundColor
                                              };
            this.SavConfig();

        }

        private bool CanEx()
        {
            //if (
            //    this.RowHeight == TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight &&
            //    this.TimeNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength &&
            //    this.LoopNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength &&
            //    this.VaNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength &&
            //    this.KxNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.KxNameLength &&
            //    this.RtuNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.RtuNameLength &&
            //    this.IsShowLoopId == TmlNewDataViewModel.ViewModel.NewDataViewModel.IsShowLoopId &&
            //    this.IsCompare == TmlNewDataViewModel.ViewModel.NewDataViewModel.IsCompare &&
            //    this.IsDetailed == TmlNewDataViewModel.ViewModel.NewDataViewModel.IsDetailed &&
            //    this.IsOnlineRate == TmlNewDataViewModel.ViewModel.NewDataViewModel.IsOnlineRate &&
            //    this.BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor &&
            //    this.K1BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor &&
            //    this.K2BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor &&
            //    this.K3BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor &&
            //    this.K4BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor &&
            //    this.K5BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor &&
            //    this.K6BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor &&
            //    this.K7BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K7BackgroundColor &&
            //    this.K8BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K8BackgroundColor &&
            //    this.OnMeasureShowData == TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData &&
            //    this.ShowDw == TmlNewDataViewModel.ViewModel.NewDataViewModel.ShowDw &&
            //    this.HsdataQueryShGjOp == Wlst.Sr.EquipmentInfoHolding.Services.Others.HsdataQueryShGjOp
            //    )
            //    return false;
            return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            this.RowHeight = TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight;
            this.TimeNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength;
            LoopNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength;
            VaNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength;
            KxNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.KxNameLength;
            RtuNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.RtuNameLength;
            BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor;
            K1BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor;
            K2BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor;
            K3BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor;
            K4BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor;
            K5BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor;
            K6BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor;
            K7BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K7BackgroundColor;
            K8BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K8BackgroundColor;
            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].IsSelected = TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(i);
            }

            var tmp = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 1); //数据查询倒序呈现
            this.IsShowReverseData = tmp == true;//? tmp.Value : false;
            var tmp1 = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 2); //最新数据呈现模式
            this.NewDataShowMode = tmp1 == -1 ? 1 : tmp1;//? tmp.Value : false;
           
        }



    }


    public partial class NewDataSettingViewModel
    {

        public const string XmlConfigName = "NewDataLenghtSetConfg";

        /// <summary>
        /// RowHeight LoopNameLength TimeNameLength VaNameLength
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int, int, int, int, Tuple<int, int, int, int, int>, BackgroundSet> LoadNewDataLenghtSetConfgX()
        {
            //public static int RowHeight = 25;
            //public static int LoopNameLength = 120;
            //public static int TimeNameLength = 120;
            //public static int VaNameLength = 80;
            //public static int KxNameLength = 30;

            int x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0;
          
            int x8 = 0;
            string background = "",
                   k1background = "",
                   k2background = "",
                   k3background = "",
                   k4background = "",
                   k5background = "",
                   k6background = "",
                   k7background = "",
                   k8background = "";

            //bool isshow = false;
            //bool iscomp = false;
            //bool isdetailed = false;
            //bool isonlinerate = false;
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("NewDataRowHeight"))
            {
                try
                {
                    x1 = Convert.ToInt32(info["NewDataRowHeight"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("NewDataLoopNameLength"))
            {
                try
                {
                    x2 = Convert.ToInt32(info["NewDataLoopNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("NewDataTimeNameLength"))
            {
                try
                {
                    x3 = Convert.ToInt32(info["NewDataTimeNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }



            if (info.ContainsKey("NewDataVaNameLength"))
            {
                try
                {
                    x4 = Convert.ToInt32(info["NewDataVaNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("RtuNameLength"))
            {
                try
                {
                    x5 = Convert.ToInt32(info["RtuNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }

          
          
            if (info.ContainsKey("KxNameLength"))
            {
                try
                {
                    x8 = Convert.ToInt32(info["KxNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("BackgroundColor"))
            {
                try
                {
                    background = info["BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                background = "Black";
            }

            if (info.ContainsKey("K1BackgroundColor"))
            {
                try
                {
                    k1background = info["K1BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k1background = "Black";
            }
            if (info.ContainsKey("K2BackgroundColor"))
            {
                try
                {
                    k2background = info["K2BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k2background = "Black";
            }
            if (info.ContainsKey("K3BackgroundColor"))
            {
                try
                {
                    k3background = info["K3BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k3background = "Black";
            }
            if (info.ContainsKey("K4BackgroundColor"))
            {
                try
                {
                    k4background = info["K4BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k4background = "Black";
            }
            if (info.ContainsKey("K5BackgroundColor"))
            {
                try
                {
                    k5background = info["K5BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k5background = "Black";
            }
            if (info.ContainsKey("K6BackgroundColor"))
            {
                try
                {
                    k6background = info["K6BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k6background = "Black";
            }
            if (info.ContainsKey("K7BackgroundColor"))
            {
                try
                {
                    k7background = info["K7BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k7background = "Black";
            }
            if (info.ContainsKey("K8BackgroundColor"))
            {
                try
                {
                    k8background = info["K8BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k8background = "Black";
            }


         


            int show0 = 0;
            //nt x9 = 0;
            if (info.ContainsKey("IsShow0"))
            {
                try
                {
                    show0 = Convert.ToInt32(info["IsShow0"]);
                }
                catch (Exception ex)
                {
                }
            }
            int show1 = 0;
            //nt x9 = 0;
            if (info.ContainsKey("IsShow1"))
            {
                try
                {
                    show1 = Convert.ToInt32(info["IsShow1"]);
                }
                catch (Exception ex)
                {
                }
            }
            int show2 = 0;
            //nt x9 = 0;
            if (info.ContainsKey("IsShow2"))
            {
                try
                {
                    show2 = Convert.ToInt32(info["IsShow2"]);
                }
                catch (Exception ex)
                {
                }
            }
            int show3 = 0;
            //nt x9 = 0;
            if (info.ContainsKey("IsShow3"))
            {
                try
                {
                    show3 = Convert.ToInt32(info["IsShow3"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (x1 < 15) x1 = 15;
            if (x2 < 60) x2 = 60;
            if (x3 < 60) x3 = 60;
            if (x4 < 60) x4 = 60;
            if (x5 < 250) x5 = 250;
            if (x8 < 30) x8 = 30;
            return new Tuple<int, int, int, int, int, Tuple<int, int, int, int, int>, BackgroundSet>(x1, x2, x3, x4, x5, new Tuple<int, int, int, int, int>(x8, show0, show1, show2, show3),
                                                                                 new BackgroundSet()
                                                                                     {
                                                                                         Background = background,
                                                                                         K1Background = k1background,
                                                                                         K2Background = k2background,
                                                                                         K3Background = k3background,
                                                                                         K4Background = k4background,
                                                                                         K5Background = k5background,
                                                                                         K6Background = k6background,
                                                                                         K7Background = k7background,
                                                                                         K8Background = k8background,
                                                                                   
                                                                                     });

        }



        public void SavConfig()
        {
            var info = new Dictionary<string, string>();
            info.Add("NewDataRowHeight", this.RowHeight + "");
            info.Add("NewDataLoopNameLength", this.LoopNameLength + "");

            info.Add("NewDataTimeNameLength", this.TimeNameLength + "");
            info.Add("NewDataVaNameLength", this.VaNameLength + "");
            info.Add("KxNameLength", this.KxNameLength + "");
            info.Add("RtuNameLength", this.RtuNameLength + "");

            info.Add("BackgroundColor", BackgroundColor);
            info.Add("K1BackgroundColor", K1BackgroundColor);
            info.Add("K2BackgroundColor", K2BackgroundColor);
            info.Add("K3BackgroundColor", K3BackgroundColor);
            info.Add("K4BackgroundColor", K4BackgroundColor);
            info.Add("K5BackgroundColor", K5BackgroundColor);
            info.Add("K6BackgroundColor", K6BackgroundColor);
            info.Add("K7BackgroundColor", K7BackgroundColor);
            info.Add("K8BackgroundColor", K8BackgroundColor);


            //item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 1 }); //序号
            //item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 10 }); //回路名称
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 0, Value = 100 }); //参考电流
            //item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 1000 }); //亮灯率
            //item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 10000 }); //功率因数


            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 1 }); //互感器比
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 10 }); //回路上限
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 100 }); //回路下限
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 1000 }); //线路状态


            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 1 }); //昨日数据
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 10 }); //状态
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 100 }); //电压
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 1000 }); //电流
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 10000 }); //功率

            //item.Add(new NameIntBool() { IsSelected = true, AreaId = 3, Value = 1 }); //手动选测自动显示数据
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 3, Value = 10 }); //显示回路数据电压电流等单位
            //item.Add(new NameIntBool() { IsSelected = false, AreaId = 3, Value = 100 }); //历史数据查询显示高级选项


            for (int i = 0; i < 4; i++)
            {
                int xcount = 0;
                foreach (var f in Item)
                {
                    if (f.AreaId == i && f.IsSelected)
                    {
                        xcount += f.Value;
                    }
                }
                info.Add("IsShow" + i, xcount + "");
            }



            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);

            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();
            dicOp.Add(1, IsShowReverseData ? "1" : "0");
            dicOp.Add(2, NewDataShowMode + "");
            dicOp.Add(3, item[17].IsSelected ? "1" : "0");
            dicOp.Add(4, item[19].IsSelected ? "1" : "0");
            dicOp.Add(5, IsShowShieldMark ? "1" : "0");
            dicOp.Add(6, IsShowRealState ? "1" : "0");
            dicOp.Add(7, IsShowComboBox ? "1" : "0");
            dicOp.Add(8, IsShowSumPower ? "1" : "0");

            dicOp.Add(13, NewDataWidth + "");
            dicOp.Add(14, IsShowAbc ? "1" : "0");
            dicOp.Add(15, OpExtendShow + "");

            dicOp.Add(16, IsStateBarShowPhyId ? "1" : "0");
            dicOp.Add(17, IsStateBarShowRtuName ? "1" : "0");
            dicOp.Add(18, IsStateBarShowGrpName ? "1" : "0");
            dicOp.Add(19, IsStateBarShowRemark ? "1" : "0");


            dicOp.Add(20, IsCloseMeasureNewData ? "1" : "0");
            dicOp.Add(21, MeasureOverTime + "");
            

            //  OpExtendShow = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt( 2801, 15, 1);// 最新数据 额外显示内容

            dicDesc.Add(1, "数据查询倒序显示");
            dicDesc.Add(2, "最新数据显示样式");
            dicDesc.Add(3, "屏蔽回路显示电流");
            dicDesc.Add(4, "屏蔽回路显示电压");
            dicDesc.Add(5, "最新数据显示屏蔽标识");
            dicDesc.Add(6, "最新数据显示实际状态");
            dicDesc.Add(7, "最新数据非下拉框显示");
            dicDesc.Add(8, "终端巡测及数据查询界面显示三相功率和总功率");

            dicDesc.Add(13, "最新数据 宽度");
            dicDesc.Add(14, "是否显示 ABC 电流");
            dicDesc.Add(15, "最新数据显示第一条故障、备注等");

            dicDesc.Add(16, "状态栏显示物理地址");
            dicDesc.Add(17, "状态栏显示终端名称");
            dicDesc.Add(18, "状态栏显示终端分组");
            dicDesc.Add(19, "状态栏显示终端备注");
            dicDesc.Add(20, "选测后关闭最新数据框");
            dicDesc.Add(21, "选测超过一定时间判定为选测失败");

            //      NewDataWidth = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 13, 600);//最新数据 宽度
            //IsShowAbc=Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 14, true);//是否显示 ABC 电流
            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(2801, dicOp, dicDesc); //3005模块，数据查询倒序

            SettingDesc.SaveItems(Items);
        }
    }

    public class BackgroundSet
    {
        public string Background { get; set; }
        public string K1Background { get; set; }
        public string K2Background { get; set; }
        public string K3Background { get; set; }
        public string K4Background { get; set; }
        public string K5Background { get; set; }
        public string K6Background { get; set; }
        public string K7Background { get; set; }
        public string K8Background { get; set; }
        //public bool OnMeasureShowData { get; set; }
        //public bool ShowDw { get; set; }
        //public bool HsdataQueryShGjOp{ get; set; }

    }
}
