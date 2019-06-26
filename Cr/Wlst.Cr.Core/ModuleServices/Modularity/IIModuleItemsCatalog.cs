using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace Wlst.Cr.Core.Modularity
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIModuleItemsCatalog
    {
        /// <summary>
        /// 获取自动加载列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<ModuleItemInfo> FirstLoad(bool loadall=true );


        /// <summary>
        /// 提供外部加载模块
        /// </summary>
        /// <param name="moduleId"></param>
        void LoadModuleItem(int moduleId);

        /// <summary>
        /// 提供外部加载模块
        /// </summary>
        /// <param name="item"></param>
        void LoadModuleItem(ModuleItemInfo item);


        /// <summary>
        /// 提供外部读取模块信息
        /// </summary>
        IEnumerable<IIModuleItemInfo> ModuleItems { get; }

        /// <summary>
        /// 提供外部通知模块内项数发生变法
        /// </summary>
        event NotifyCollectionChangedEventHandler CollectionChanged;


        /// <summary>
        /// 设置模块的自加载设置
        /// </summary>
        void SetModuleItemAutoLoad(int moduleId, bool autoLoad);


        /// <summary>
        /// 当模块加载状态发生变法
        /// </summary>
        event EventHandler OnModuleLoadedStateChanged;

        /// <summary>
        /// 通过名称获取模块
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ModuleItemInfo GetModuleInfoByName(string name);

        /// <summary>
        /// 动态增加程序集
        /// </summary>
        /// <param name="assembly"></param>
        int AddModuelItem(Assembly assembly);

        /// <summary>
        /// 动态增加程序集
        /// </summary>
        /// <param name="assemblyPath"></param>
        int AddModuelItem(string assemblyPath);

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool RemoveItem(ModuleItemInfo item);

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        bool RemoveItem(int moduleId);

        /// <summary>
        /// 保存配置信息
        /// </summary>
        void SaveConfig();

        /// <summary>
        /// 提供外部卸载模块
        /// </summary>
        /// <param name="moduleId"></param>
         void UnLoadModuleItem(int moduleId);

        /// <summary>
        /// 提供外部卸载模块
        /// </summary>
        /// <param name="item"></param>
        void UnLoadModuleItem(ModuleItemInfo item);

    }
}
