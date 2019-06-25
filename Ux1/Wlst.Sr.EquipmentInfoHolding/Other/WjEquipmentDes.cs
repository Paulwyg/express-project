using System;
using System.Collections.Generic;

namespace Wlst.Sr.EquipmentInfoHolding.Other
{
    /// <summary>
    /// 设备型号描述
    /// </summary>
    public class WjEquipmentDes
    {
        private static Dictionary<string, Tuple<string, string>> _equipmentModuleDes =
            new Dictionary<string, Tuple<string, string>>();


        /// <summary>
        /// 根据模块关键字获取设备描述
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public static string GetNameByModuleKey(string moduleKey)
        {
            if (_equipmentModuleDes.ContainsKey(moduleKey))
                return _equipmentModuleDes[moduleKey].Item1;
            return "";
        }

        /// <summary>
        /// 根据模块关键字获取设备描述
        /// </summary>
        /// <param name="moduleKey"></param>
        /// <returns></returns>
        public static string GetDesByModuleKey(string moduleKey)
        {
            if (_equipmentModuleDes.ContainsKey(moduleKey))
                return _equipmentModuleDes[moduleKey].Item2;
            return "";
        }

        /// <summary>
        /// 注册模块名称以及描述
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="des"></param>
        public static void RegModuleKeyValue(string key, string name, string des)
        {
            if (!_equipmentModuleDes.ContainsKey(key))
                _equipmentModuleDes.Add(key, new Tuple<string, string>(name, des));
            else _equipmentModuleDes[key] = new Tuple<string, string>(name, des);
        }
    }
}