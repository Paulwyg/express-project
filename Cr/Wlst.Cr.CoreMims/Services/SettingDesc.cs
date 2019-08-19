using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using System.Collections.ObjectModel;

namespace Wlst.Cr.CoreMims.Services
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
    public partial class SettingDesc: ObservableObject
    {
        protected int Moduleid = 0;// Ux.EquipemntLightFault.Services.ViewIdAssign.ViewIdAssignBaseId + 1;
        //private static bool IsInitDesc = false;
        protected  Dictionary<int, string> dicDesc = new Dictionary<int, string>();
        //protected void InitDesc()
        //{
        //    if (IsInitDesc) return;
        //    IsInitDesc = true;

        //    dicDesc.Add(0, "本文档为故障自定义存储文档.");
        //    GetInitInt(ref dicDesc);
        //    GetInitString(ref dicDesc);
        //    GetInitBoolean(ref dicDesc);
        //    GetInitDouble(ref dicDesc);
        //    Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dicDesc, true);
        //}

        public SettingDesc(int modulid)
        {
            Moduleid = modulid;
        }

        public void SaveDesc()
        {
            Wlst.Cr.CoreMims.Services.SystemOptionSvr.SaveModuleXml(Moduleid, dicDesc, true);

        }


        /// <summary>
        /// 未设置默认值则未 0 ,key  :1-99
        /// </summary>
        /// <param name="dicDesc"></param>
        public bool AddInt(int key, string desc)
        {
            if (key < 1 || key > 99) return false;
            if (dicDesc.ContainsKey(key)) return false;
            dicDesc.Add(key, desc);
            return true;
        }


        /// <summary>
        /// 未设置默认值则未 0 ,key  :101-199
        /// </summary>
        /// <param name="dicDesc"></param>
        public bool AddString(int key, string desc)
        {
            if (key < 101 || key > 199) return false;
            if (dicDesc.ContainsKey(key)) return false;
            dicDesc.Add(key, desc);
            return true;
        }

        /// <summary>
        /// 未设置默认值则未 0 ,key  :301-399
        /// </summary>
        /// <param name="dicDesc"></param>
        public bool AddDouble(int key, string desc)
        {
            if (key < 301 || key > 399) return false;
            if (dicDesc.ContainsKey(key)) return false;
            dicDesc.Add(key, desc);
            return true;
        }

        /// <summary>
        /// 未设置默认值则未 0 ,key  :201-299
        /// </summary>
        /// <param name="dicDesc"></param>
        public bool AddBoolean(int key, string desc)
        {
            if (key < 201 || key > 299) return false;
            if (dicDesc.ContainsKey(key)) return false;
            dicDesc.Add(key, desc);
            return true;
        }


        ///// <summary>
        ///// 未设置默认值则为 empty
        ///// </summary>
        ///// <param name="dicDesc"></param>
        //void GetInitString(ref Dictionary<int, string> dicDesc)
        //{
        //    int baseid = 100;
        //    dicDesc.Add(101, "亮化终端判定1-终端名称包含:");
        //    dicDesc.Add(102, "亮化终端判定2-终端名称包含:");
        //    dicDesc.Add(103, "亮化终端判定3-终端名称包含:");
        //    dicDesc.Add(104, "亮化终端判定4-终端名称包含:");
        //    dicDesc.Add(105, "亮化终端判定5-终端名称包含:");

        //}

        ///// <summary>
        ///// 未设置默认值 则为false
        ///// </summary>
        ///// <param name="dicDesc"></param>
        //void GetInitBoolean(ref Dictionary<int, string> dicDesc)
        //{
        //    int baseid = 200;
        //    dicDesc.Add(201, "[[1]]现存故障查询显示路灯、亮化筛选功能 [101-105]");
        //    //dicDesc.Add(202, "是否启用派单功能");
        //    //dicDesc.Add(202, "分组显示是否显示城区局和灯杆号");
        //    //dicDesc.Add(202, "分组终端是否显示电压电流上限下限");
        //    //dicDesc.Add(202, "最新故障界面显示背景色");


        //    //dicDesc.Add(202, "最新故障界面显示背景色");
        //    //dicDesc.Add(202, "显示删除故障按钮");
        //    //dicDesc.Add(202, "历史故障下方显示详细故障数据  lvf 2018年5月9日09:37:13");
        //    //dicDesc.Add(202, "双击故障复制信息到剪贴板  lvf 2018年10月8日09:03:35");
        //    //dicDesc.Add(202, "历史故障显示统计数据  lvf 2018年5月9日09:37:13");
        //}

        ///// <summary>
        ///// 未设置默认值则未 0
        ///// </summary>
        ///// <param name="dicDesc"></param>
        //void GetInitDouble(ref Dictionary<int, string> dicDesc)
        //{
        //    int baseid = 300;
        //}
    }

    public partial class SettingDesc
    {

        string GetDefautString(int key)
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

        int GetDefautInt(int key)
        {
            var tmp = GetDefautString(key);
            int rtn = 0;
            if (String.IsNullOrEmpty(tmp) == false)
                Int32.TryParse(tmp, out rtn);
            return rtn;
        }


        double GetDefautDoubel(int key)
        {
            var tmp = GetDefautString(key);
            double rtn = 0;
            if (String.IsNullOrEmpty(tmp) == false)
                Double.TryParse(tmp, out rtn);
            return rtn;
        }


        bool GetDefautBooelan(int key)
        {
            var tmp = GetDefautInt(key);
            return tmp == 2;
        }

        protected ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> GetItems()
        {
            var _items = new ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey>();


            /// Int ：  1-99  ，
            /// string  101-199 ，
            /// boolean 201-299 （1、false，2、true ，其他 false），
            /// double  301-399 
            //InitDesc();
            var keys = dicDesc.Keys.ToList();
            foreach (var f in keys)
            {
                if (f < 1) continue; //特殊定义
                if (f < 100)
                {
                    var def = GetDefautInt(f);

                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueInt = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetInt(Moduleid, f, def),
                    });
                }
                if (100 < f && f < 200)
                {
                    var def = GetDefautString(f);
                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueString = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetString(Moduleid, f, def),
                    });
                }
                if (200 < f && f < 300)
                {
                    var def = GetDefautBooelan(f);
                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueBool = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetBoolean(Moduleid, f, def),
                    });
                }
                if (300 < f && f < 400)
                {
                    var def = GetDefautDoubel(f);
                    _items.Add(f, new Cr.CoreOne.Models.IntStringBoolDoubleKey()
                    {
                        Key = f,
                        ValueDouble = Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetDouble(Moduleid, f, def),
                    });
                }
            }
            return _items;
        }

        protected void SaveItems(ObservableDictionary<int, Wlst.Cr.CoreOne.Models.IntStringBoolDoubleKey> items)
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

}
