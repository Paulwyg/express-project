using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Elysium.ThemesSet.Common;
using Microsoft.Practices.Prism;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingViewModel.Services;
using System.Collections.ObjectModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingViewModel.ViewModel
{


    ///// <summary>
    ///// key：
    ///// Int ：  1-99  ，
    ///// string  101-199 ，
    ///// boolean 201-299 （1、false，2、true ，其他 false），
    ///// double  301-399 
    ///// value:
    ///// 值当中 [[1]] 标记为本次设置的默认值，且必须放在最开始处 ,外部为 [[默认值]]
    ///// </summary>
    //internal partial class SettingDesc
    //{
    //    static int Moduleid = EquipmentFaultManageSettingViewModel.Moduleid;// Ux.EquipemntLightFault.Services.ViewIdAssign.ViewIdAssignBaseId + 1;
    //    private static bool IsInitDesc = false;
    //    internal static Dictionary<int, string> dicDesc = new Dictionary<int, string>();
    //    internal static void InitDesc()
    //    {
    //        if (IsInitDesc) return;
    //        IsInitDesc = true;

    //        dicDesc.Add(0, "本文档为故障自定义存储文档.");
    //        GetInitInt(ref dicDesc);
    //        GetInitString(ref dicDesc);
    //        GetInitBoolean(ref dicDesc);
    //        GetInitDouble(ref dicDesc);
    //        Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dicDesc, true);
    //    }


    //    /// <summary>
    //    /// 未设置默认值则未 0
    //    /// </summary>
    //    /// <param name="dicDesc"></param>
    //    static void GetInitInt(ref Dictionary<int, string> dicDesc)
    //    {

    //    }

    //    /// <summary>
    //    /// 未设置默认值则为 empty
    //    /// </summary>
    //    /// <param name="dicDesc"></param>
    //    static void GetInitString(ref Dictionary<int, string> dicDesc)
    //    {
    //        int baseid = 100;
    //        dicDesc.Add(101, "亮化终端判定1-终端名称包含:");
    //        dicDesc.Add(102, "亮化终端判定2-终端名称包含:");
    //        dicDesc.Add(103, "亮化终端判定3-终端名称包含:");
    //        dicDesc.Add(104, "亮化终端判定4-终端名称包含:");
    //        dicDesc.Add(105, "亮化终端判定5-终端名称包含:");

    //    }

    //    /// <summary>
    //    /// 未设置默认值 则为false
    //    /// </summary>
    //    /// <param name="dicDesc"></param>
    //    static void GetInitBoolean(ref Dictionary<int, string> dicDesc)
    //    {
    //        int baseid = 200;
    //        dicDesc.Add(201, "[[1]]现存故障查询显示路灯、亮化筛选功能 [101-105]");
    //        //dicDesc.Add(202, "是否启用派单功能");
    //        //dicDesc.Add(202, "分组显示是否显示城区局和灯杆号");
    //        //dicDesc.Add(202, "分组终端是否显示电压电流上限下限");
    //        //dicDesc.Add(202, "最新故障界面显示背景色");


    //        //dicDesc.Add(202, "最新故障界面显示背景色");
    //        //dicDesc.Add(202, "显示删除故障按钮");
    //        //dicDesc.Add(202, "历史故障下方显示详细故障数据  lvf 2018年5月9日09:37:13");
    //        //dicDesc.Add(202, "双击故障复制信息到剪贴板  lvf 2018年10月8日09:03:35");
    //        //dicDesc.Add(202, "历史故障显示统计数据  lvf 2018年5月9日09:37:13");
    //    }

    //    /// <summary>
    //    /// 未设置默认值则未 0
    //    /// </summary>
    //    /// <param name="dicDesc"></param>
    //    static void GetInitDouble(ref Dictionary<int, string> dicDesc)
    //    {
    //        int baseid = 300;
    //    }
    //}
    //internal partial class SettingDesc
    //{

    //    static string GetDefautString(int key)
    //    {
    //        if (dicDesc.ContainsKey(key) == false) return string.Empty;
    //        if (dicDesc[key].Contains("[[") && dicDesc[key].Contains("]]"))
    //        {
    //            var tmp = dicDesc[key].Substring(0, dicDesc[key].IndexOf("]]"));
    //            var rtn = tmp.Replace("[[", "");
    //            return rtn;
    //        }

    //        return string.Empty;

    //    }

    //    static int GetDefautInt(int key)
    //    {
    //        var tmp = GetDefautString(key);
    //        int rtn = 0;
    //        if (String.IsNullOrEmpty(tmp) == false)
    //            Int32.TryParse(tmp, out rtn);
    //        return rtn;
    //    }


    //    static double GetDefautDoubel(int key)
    //    {
    //        var tmp = GetDefautString(key);
    //        double rtn = 0;
    //        if (String.IsNullOrEmpty(tmp) == false)
    //            Double.TryParse(tmp, out rtn);
    //        return rtn;
    //    }


    //    static bool GetDefautBooelan(int key)
    //    {
    //        var tmp = GetDefautInt(key);
    //        return tmp == 2;
    //    }

    //    internal static ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> GetItems()
    //    {
    //        var _items = new ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey>();

    //        /// Int ：  1-99  ，
    //        /// string  101-199 ，
    //        /// boolean 201-299 （1、false，2、true ，其他 false），
    //        /// double  301-399 
    //        InitDesc();
    //        var keys = SettingDesc.dicDesc.Keys.ToList();
    //        foreach (var f in keys)
    //        {
    //            if (f < 1) continue; //特殊定义
    //            if (f < 100)
    //            {
    //                var def = SettingDesc.GetDefautInt(f);

    //                _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
    //                {
    //                    Key = f,
    //                    ValueInt = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetInt(Moduleid, f, def),
    //                });
    //            }
    //            if (100 < f && f < 200)
    //            {
    //                var def = SettingDesc.GetDefautString(f);
    //                _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
    //                {
    //                    Key = f,
    //                    ValueString = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetString(Moduleid, f, def),
    //                });
    //            }
    //            if (200 < f && f < 300)
    //            {
    //                var def = SettingDesc.GetDefautBooelan(f);
    //                _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
    //                {
    //                    Key = f,
    //                    ValueBool = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetBoolean(Moduleid, f, def),
    //                });
    //            }
    //            if (300 < f && f < 400)
    //            {
    //                var def = SettingDesc.GetDefautDoubel(f);
    //                _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
    //                {
    //                    Key = f,
    //                    ValueDouble = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetDouble(Moduleid, f, def),
    //                });
    //            }
    //        }
    //        return _items;
    //    }

    //    internal static void SaveItems(ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> items)
    //    {
    //        var dic = new Dictionary<int, string>();
    //        foreach (var f in items)
    //        {
    //            if (f.Key < 100)
    //            {
    //                dic.Add(f.Key, f.Value.ValueInt + "");
    //            }
    //            if (100 < f.Key && f.Key < 200)
    //            {
    //                dic.Add(f.Key, f.Value.ValueString + "");
    //            }
    //            if (200 < f.Key && f.Key < 300)
    //            {
    //                dic.Add(f.Key, (f.Value.ValueBool ? 2 : 1) + "");
    //            }
    //            if (300 < f.Key && f.Key < 400)
    //            {
    //                dic.Add(f.Key, f.Value.ValueDouble + "");
    //            }
    //        }

    //        Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dic, false);
    //    }
    //}


    public partial class EquipmentFaultManageSettingViewModel
    {
        internal static int ModuleId = Ux.EquipemntLightFault.Services.ViewIdAssign.ViewIdAssignBaseId + 1;
        private ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> _items = null;

        public ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> Items
        {
            get
            {

                if (_items == null || _items.Count == 0)
                {
                    InitDesc();

                    if (_items == null)
                        _items = new ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey>();

                    var tmpDadta = GetItems();
                    foreach (var f in tmpDadta)
                    {
                        if (_items.ContainsKey(f.Key) == false) _items.Add(f.Key, f.Value);
                    }
                }
                return _items;
            }

        }


        private bool IsInitDesc = false;
        private void InitDesc()
        {

            if (IsInitDesc) return;
            IsInitDesc = true;
            this.AddBoolean(0, "本文档为故障自定义设置文档。");
            this.AddBoolean(201, "[[1]]现存故障查询显示路灯、亮化筛选功能 [101-105]");
            this.AddString(101, "亮化终端判定1-终端名称包含:");
            this.AddString(102, "亮化终端判定2-终端名称包含:");
            this.AddString(103, "亮化终端判定3-终端名称包含:");
            this.AddString(104, "亮化终端判定4-终端名称包含:");
            this.AddString(105, "亮化终端判定5-终端名称包含:");
            this.AddBoolean(202, "[[1]]全局树晚上不显示白天报警图标");
            this.AddBoolean(203, "[[1]]全局树白天不显示晚上报警图标");
            this.SaveDesc();
        }



        private DateTime _dtApplyxx;
        private ICommand _cmdApplyxx;

        public ICommand CmdApply1
        {
            get
            {

                if (_cmdApplyxx == null) _cmdApplyxx = new RelayCommand(Exxx, CanExXX, false);
                return _cmdApplyxx;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Exxx()
        {
            _dtApplyxx = DateTime.Now;

            this.SaveItems(Items);


        }

        private bool CanExXX()
        {
            return DateTime.Now.Ticks - _dtApplyxx.Ticks > 10000000;
        }

    }


    [Export(typeof(IIEquipmentFaultManageSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultManageSettingViewModel : SettingDesc, IIEquipmentFaultManageSettingViewModel
    {
        
        public EquipmentFaultManageSettingViewModel() :base(ModuleId)
        {
 
           // this.InitDesc();

            this.NavOnLoad();
            
        }

   



        public const string XmlConfigName = "EquipmentFaultSetting";

        public void NavOnLoad(params object[] parsObjects)
        {
            if (Items == null) return;

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);

            if (info.ContainsKey("IsShowCQJandDGGH"))
            {
                IsShowCQJandDGGH = info["IsShowCQJandDGGH"].Contains("yes");
            }
            else IsShowCQJandDGGH = false;

            if (info.ContainsKey("IsShowVAPHL"))
            {
                IsShowVAPHL = info["IsShowVAPHL"].Contains("yes");
            }
            else IsShowVAPHL = false;

            if (info.ContainsKey("EnablePaidan"))
            {
                EnablePaidan = info["EnablePaidan"].Contains("yes");
            }
            else EnablePaidan = false;

            EquipemntLightFaultSetting.IsShowCQJandDGGH = IsShowCQJandDGGH;
            EquipemntLightFaultSetting.IsShowVAPHL = IsShowVAPHL;

            IsShowBlackground = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3101, 1, false, "\\SystemColorAndFont");

            IsShowDelFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 1, false);

            IsCalcFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 2, false);

            IsShowCalcFaultDetail = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 3, false);

            IsCopyFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 4, false);

            IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;


        }



        //private ObservableCollection<Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> _record = null;

        //public ObservableCollection<Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> LhRtu
        //{
        //    get
        //    {
        //        if (_record == null)
        //        {
        //            //dicDesc.Add(10001, "亮化终端判定1-终端名称包含:");
        //            //dicDesc.Add(10002, "亮化终端判定2-终端名称包含:");
        //            //dicDesc.Add(10003, "亮化终端判定3-终端名称包含:");
        //            //dicDesc.Add(10004, "亮化终端判定4-终端名称包含:");
        //            //dicDesc.Add(10005, "亮化终端判定5-终端名称包含:");
        //            _record = new ObservableCollection<Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey>();
        //            for (int i = 1; i < 6; i++)
        //            {
        //                var setinfo = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetString(Moduleid, 10000 + i, string.Empty);
        //                _record.Add(new Cr.CoreOne.Models.IntStringBoolDoubleKey()
        //                {
        //                    Key = 10000 + i,
        //                    ValueString = setinfo,
        //                });
        //            }

        //            //dicDesc.Add(20001, "现存故障查询显示路灯、亮化筛选功能 [10001-10005]");
        //            var isuseld = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetString(Moduleid, 20001, "1");
        //            _record.Add(new Cr.CoreOne.Models.IntStringBoolDoubleKey()
        //            {
        //                Key = 20000 + 1,
        //                ValueBool = isuseld.Contains("2"),
        //            });



        //        }
        //        return _record;
        //    }
        //    set
        //    {
        //        if (_record != value)
        //        {
        //            _record = value;
        //            this.RaisePropertyChanged(() => this.LhRtu);
        //        }
        //    }
        //}

        private bool _enablePaidan;

        /// <summary>
        /// 是否启用派单功能
        /// </summary>
        public bool EnablePaidan
        {
            get { return _enablePaidan; }
            set
            {
                if (value != _enablePaidan)
                {
                    _enablePaidan = value;
                    this.RaisePropertyChanged(() => this.EnablePaidan);
                }
            }
        }

        private bool _isShowCQJandDGGH;

        /// <summary>
        /// 分组显示是否显示城区局和灯杆号
        /// </summary>
        public bool IsShowCQJandDGGH
        {
            get { return _isShowCQJandDGGH; }
            set
            {
                if (value != _isShowCQJandDGGH)
                {
                    _isShowCQJandDGGH = value;
                    this.RaisePropertyChanged(() => this.IsShowCQJandDGGH);
                }
            }
        }


        private bool _isShowVAPHL;

        /// <summary>
        /// 分组终端是否显示电压电流上限下限 
        /// </summary>
        public bool IsShowVAPHL
        {
            get { return _isShowVAPHL; }
            set
            {
                if (value != _isShowVAPHL)
                {
                    _isShowVAPHL = value;
                    this.RaisePropertyChanged(() => this.IsShowVAPHL);
                }
            }
        }

        private bool _isShowBlackground;

        /// <summary>
        /// 最新故障界面显示背景色
        /// </summary>
        public bool IsShowBlackground
        {
            get { return _isShowBlackground; }
            set
            {
                if (value != _isShowBlackground)
                {
                    _isShowBlackground = value;
                    this.RaisePropertyChanged(() => this.IsShowBlackground);
                }
            }
        }

        #region IsCalcFault
        private bool _isCalcFault;

        /// <summary>
        /// 历史故障显示统计数据  lvf 2018年5月9日09:37:13
        /// </summary>
        public bool IsCalcFault
        {
            get { return _isCalcFault; }
            set
            {
                if (value != _isCalcFault)
                {
                    _isCalcFault = value;
                    this.RaisePropertyChanged(() => this.IsCalcFault);

                    if (_isCalcFault == false) IsShowCalcFaultDetail = false;
                }
            }
        }
        #endregion


        #region IsCopyFault
        private bool _isCopyFault;

        /// <summary>
        /// 双击故障复制信息到剪贴板  lvf 2018年10月8日09:03:35
        /// </summary>
        public bool IsCopyFault
        {
            get { return _isCopyFault; }
            set
            {
                if (value != _isCopyFault)
                {
                    _isCopyFault = value;
                    this.RaisePropertyChanged(() => this.IsCopyFault);

                }
            }
        }
        #endregion

        #region IsShowCalcFaultDetail
        private bool _isShowCalcFaultDetail;

        /// <summary>
        /// 历史故障下方显示详细故障数据  lvf 2018年5月9日09:37:13
        /// </summary>
        public bool IsShowCalcFaultDetail
        {
            get { return _isShowCalcFaultDetail; }
            set
            {
                if (value != _isShowCalcFaultDetail)
                {
                    _isShowCalcFaultDetail = value;
                    this.RaisePropertyChanged(() => this.IsShowCalcFaultDetail);
                }
            }
        }
        #endregion



        #region IsD


        private bool _cheIsD;

        public bool IsD
        {
            get { return _cheIsD; }
            set
            {
                if (value != _cheIsD)
                {
                    _cheIsD = value;
                    RaisePropertyChanged(() => IsD);
                }
            }
        }



        #endregion


        #region IsShowDelFault
        private bool _isShowDelFault;

        /// <summary>
        /// 显示删除故障按钮
        /// </summary>
        public bool IsShowDelFault
        {
            get { return _isShowDelFault; }
            set
            {
                if (value != _isShowDelFault)
                {
                    _isShowDelFault = value;
                    this.RaisePropertyChanged(() => this.IsShowDelFault);
                }
            }
        }
        #endregion


        #region SystemName

        private string _backgroundColor;

        public string SystemName
        {
            get { return _backgroundColor; }
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    RaisePropertyChanged(() => this.SystemName);
                }
            }
        }

        #endregion


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

            Dictionary<string, string> info = new Dictionary<string, string>();

            if (IsShowCQJandDGGH) info.Add("IsShowCQJandDGGH", "yes");
            else info.Add("IsShowCQJandDGGH", "no");

            if (IsShowVAPHL) info.Add("IsShowVAPHL", "yes");
            else info.Add("IsShowVAPHL", "no");

            if (EnablePaidan) info.Add("EnablePaidan", "yes");
            else info.Add("EnablePaidan", "no");

            EquipemntLightFaultSetting.IsShowCQJandDGGH = IsShowCQJandDGGH;
            EquipemntLightFaultSetting.IsShowVAPHL = IsShowVAPHL;

            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);

            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();
            dicOp.Add(1, IsShowBlackground ? "1" : "0");
            dicDesc.Add(1, "最新故障界面显示背景色");
            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3101, dicOp, dicDesc, "\\SystemColorAndFont");

            var dicOp1 = new Dictionary<int, string>();
            var dicDesc1 = new Dictionary<int, string>();
            dicOp1.Add(1, IsShowDelFault ? "1" : "0");
            dicDesc1.Add(1, "是否显示删除故障按钮");

            dicOp1.Add(2, IsCalcFault ? "1" : "0");
            dicDesc1.Add(2, "历史故障开启统计功能");

            dicOp1.Add(3, IsShowCalcFaultDetail ? "1" : "0");
            dicDesc1.Add(3, "统计时下方显示详细信息");

            dicOp1.Add(4, IsCopyFault ? "1" : "0");
            dicDesc1.Add(4, "双击复制故障信息至剪贴板");

            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3102, dicOp1, dicDesc1);

            //var dic = new Dictionary<int, string>();

            //if (LhRtu[5].IsSelected)
            //{
            //    dic.Add(20001, "2");
            //}
            //else
            //{
            //    dic.Add(20001, "1");
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    dic.Add(10001 + i, LhRtu[i].Name);
            //}
            //Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dic, false);

            this.SaveItems(Items);


        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 10000000;
        }

    }
}
