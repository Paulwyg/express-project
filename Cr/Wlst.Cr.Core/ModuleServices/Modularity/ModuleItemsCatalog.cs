using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
 
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Cr.Core.Modularity
{

    /// <summary>
    /// 
    /// </summary>
    public class ModuleItemsCatalog : IIModuleItemsCatalog
    {

        
        #region


        /// <summary>
        /// 提供外部加载模块
        /// </summary>
        /// <param name="moduleId"></param>
        public void LoadModuleItem(int moduleId)
        {
            foreach (var t in this .Items )
            {
                if(t.AssemblyConfigInfo !=null && t.AssemblyConfigInfo .ModuleId ==moduleId )
                {
                    LoadModuleItem(t);
                    break;
                }
            }
        }
        /// <summary>
        /// 提供外部加载模块
        /// </summary>
        /// <param name="item"></param>
        public void LoadModuleItem(ModuleItemInfo item)
        {
            var t = this.GetLoadModuleSqu(item);
            foreach (var f in t)
            {
                try
                {
                    LoadUnloadManage.LoadModuleItem(f);
                    
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError(
                        " CETC50_Core.Modularity LoadModuleItem while load :" + f.AssemblyConfigInfo.ToString() +
                        " Exception:" + ex);
                }
            }
            this.AsyItemsDatafromCatalog();
        }


        /// <summary>
        /// 提供外部卸载模块
        /// </summary>
        /// <param name="moduleId"></param>
        public void UnLoadModuleItem(int moduleId)
        {
            foreach (var t in this.Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == moduleId)
                {
                    UnLoadModuleItem(t);
                    break;
                }
            }
        }
        /// <summary>
        /// 提供外部卸载模块
        /// </summary>
        /// <param name="item"></param>
        public void UnLoadModuleItem(ModuleItemInfo item)
        {
            // var t = this.GetLoadModuleSqu(item);
            if (item.AssemblyConfigInfo == null) return;
            try
            {
                LoadUnloadManage.UnLoadModuleItem(item);

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    " CETC50_Core.Modularity UnLoadModuleItem while load :" + item.AssemblyConfigInfo.ToString() +
                    " Exception:" + ex);
            }
            this.AsyItemsDatafromCatalog();
        }


        #endregion

        #region 加载 顺序获取

        /// <summary>
        /// 第一次加载 模块入口
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ModuleItemInfo> FirstLoad(bool loadall = true)
        {
            if (!this.IsAllModuleExistAutoLogin())
            {
                var t = this.GetLostModulesAutoLogin();
                if (t.Any())
                {
                    string str = "";
                    foreach (var f in t) str += f + ";";
                    var reslut = MessageBox.Show("程序运行缺少支撑模块，缺少模块为：" + str + ";强制运行请点 Ok.", "缺少支撑模块,",
                                                 MessageBoxButton.OKCancel);
                    if (reslut == MessageBoxResult.Cancel)
                    {
                        Application.Current.Shutdown(1);
                    }
                }
            }
            var gg = this.GetCrossModulesAutoLogin();
            if (gg.Any())
            {
                string str = "";
                foreach (var f in gg) str += f + ";";
                var reslut = MessageBox.Show("程序内部模块具有交叉引用：" + str + ";强制运行请点 Ok.", "发现交叉引用,",
                                             MessageBoxButton.OKCancel);
                if (reslut == MessageBoxResult.Cancel)
                {
                    Application.Current.Shutdown(1);
                }
            }
            var tt = this.GetLoadSquAutoLogin();
            var lstReturn = new List<ModuleItemInfo>();
            foreach (var ff in tt)
            {
                var ggg = this.GetModuleInfoByName(ff);
                if (ggg != null) lstReturn.Add(ggg);
            }
            if (!loadall)
            {
                var tmp = DeleteModulesThatSet3();
                foreach (var t in tmp) if (lstReturn.Contains(t)) lstReturn.Remove(t);
            }
            return lstReturn;
        }

        private List<ModuleItemInfo> DeleteModulesThatSet3()
        {
            return  (from t in this.Items
                       where
                           t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.LoadByUserSet &&
                           t.IsAutoLoadAfterLogin == false
                       select t).ToList();
        }


        /// <summary>
        /// 获取加载模块的顺序 单个模块加载顺序入口
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected IEnumerable<ModuleItemInfo> GetLoadModuleSqu(ModuleItemInfo item)
        {
            if (!this.IsAllModuleExistThatforThisModue(item))
            {
                var t = this.GetLostModulesThatforThisModue(item);
                if (t.Any())
                {
                    string str = "";
                    foreach (var f in t) str += f + ";";
                    var reslut = MessageBox.Show("程序运行缺少模块，缺少模块为：" + str + ";强制运行请点 Ok.", "缺少支撑模块,",
                                                 MessageBoxButton.OKCancel);
                    if (reslut == MessageBoxResult.Cancel)
                    {
                        Application.Current.Shutdown(1);
                    }
                }
            }
            var gg = this.GetCrossModuleThatforThisModue(item);
            if (gg.Any())
            {
                string str = "";
                foreach (var f in gg) str += f + ";";
                var reslut = MessageBox.Show("程序内部模块具有交叉引用：" + str + ";强制运行请点 Ok.", "发现交叉引用,",
                                             MessageBoxButton.OKCancel);
                if (reslut == MessageBoxResult.Cancel)
                {
                    Application.Current.Shutdown(1);
                }
            }
            var tt = this.GetLoadSquThatforThisModue(item);
            var lstReturn = new List<ModuleItemInfo>();
            foreach (var ff in tt)
            {
                var ggg = this.GetModuleInfoByName(ff);
                if (ggg != null) lstReturn.Add(ggg);
            }
            return lstReturn;
        }

        #endregion

        #region 批量加载模块计算信息

        /// <summary>
        /// 是否模块可正常加载
        /// </summary>
        /// <returns></returns>
        protected bool CanLoadAssemblyThatAutoLogin()
        {

            List<ModuleItemInfo> lstNeedLoad =
                Items.Where(
                    t =>
                    t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoad ).
                    ToList();
            if (ModuleDependencySolver.IsAllDependsModuleExist(this.Items, lstNeedLoad))
            {
                if (ModuleDependencySolver.CanDependsModulesNormalLoad(this.Items, lstNeedLoad))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取启动缺少的模块
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetLostModulesAutoLogin()
        {
            List<ModuleItemInfo> lstNeedLoad =
                Items.Where(
                    t =>
                    t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoad ).
                    ToList();
            return ModuleDependencySolver.GetThisModuleNotExistIn(this.Items, lstNeedLoad);
        }

        /// <summary>
        /// 获取交叉引用模块
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetCrossModulesAutoLogin()
        {
            List<ModuleItemInfo> lstNeedLoad =
                Items.Where(
                    t =>
                    t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoad ).
                    ToList();
            return ModuleDependencySolver.GetThisModuleCrossRef(this.Items, lstNeedLoad);
        }

        /// <summary>
        /// 是否所有模块均存在
        /// </summary>
        /// <returns></returns>
        protected bool IsAllModuleExistAutoLogin()
        {

            List<ModuleItemInfo> lstNeedLoad =
                Items.Where(
                    t =>
                    t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoad ).
                    ToList();
            return ModuleDependencySolver.IsAllDependsModuleExist(this.Items, lstNeedLoad);
        }

        /// <summary>
        /// 获取启动顺序
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetLoadSquAutoLogin()
        {
            List<ModuleItemInfo> lstNeedLoad =
                Items.Where(
                    t =>
                    t.AssemblyConfigInfo != null && (t.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoad ||
                    (t.AssemblyConfigInfo .AutoLoad ==ModuleLoadSqu.LoadByUserSet && t.IsAutoLoadAfterLogin) ) ).
                    ToList();
            return ModuleDependencySolver.GetThisModulesNormalLoadSqu(this.Items, lstNeedLoad);

        }

        #endregion

  

        #region 自由加载

        /// <summary>
        /// 是否可以正常加载本模块 CanLoadAssemblyThatforThisModue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected bool CanLoadAssemblyThatforThisModue(ModuleItemInfo item)
        {
            var lst = new List<ModuleItemInfo>() {item};
            return ModuleDependencySolver.CanDependsModulesNormalLoad(this.Items, lst);
            return true;
        }

        /// <summary>
        /// 本模块加载的所有依赖项是否都存在 IsAllModuleExistThatforThisModue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected bool IsAllModuleExistThatforThisModue(ModuleItemInfo item)
        {
            var lst = new List<ModuleItemInfo>() {item};
            return ModuleDependencySolver.IsAllDependsModuleExist(this.Items, lst);
        }

        /// <summary>
        /// 获取本模块依赖模块缺少的模块 GetLostModulesThatforThisModue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected IEnumerable<string> GetLostModulesThatforThisModue(ModuleItemInfo item)
        {
            var lst = new List<ModuleItemInfo>() {item};
            return ModuleDependencySolver.GetThisModuleNotExistIn(this.Items, lst);
        }

        /// <summary>
        /// 获取交叉引用模块 GetCrossModuleThatforThisModue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected IEnumerable<string> GetCrossModuleThatforThisModue(ModuleItemInfo item)
        {
            var lst = new List<ModuleItemInfo>() {item};
            return ModuleDependencySolver.GetThisModuleCrossRef(this.Items, lst);
        }

        /// <summary>
        /// 获取本模块的正常加载顺序 GetLoadSquThatforThisModue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected   IEnumerable<string> GetLoadSquThatforThisModue(ModuleItemInfo item)
        {
            var lst = new List<ModuleItemInfo>() {item};
            return ModuleDependencySolver.GetThisModulesNormalLoadSqu(this.Items, lst);
        }



        #endregion

        #region Items Information

        private ModuleCatalogItemCollection _items;

        private ModuleCatalogItemCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ModuleCatalogItemCollection();
                    this._items.CollectionChanged += this.OnNotifyCollectionChanged;
                    this.InnerItemLoad();

                }
                return _items;
            }
        }

        #region  load config save config
        /// <summary>
        /// 程序自动装载模块 Modules
        /// </summary>
        protected void InnerItemLoad()
        {
            this.Items.Clear();
            string baseDirec = System.AppDomain.CurrentDomain.BaseDirectory;
            this.GetModuleInfoByDirectory(baseDirec); 
          
            string moduleDire = baseDirec + "Modules\\";
            this.GetModuleInfoByDirectory(moduleDire);
            this.LoadConfig();
           
        }



        private void GetModuleInfoByDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                return;
            }
            //if (!File.Exists(directory)) return;
            DirectoryInfo theFolder = new DirectoryInfo(directory); // 给出你的目录文件位置 

            FileInfo[] fileInfo = theFolder.GetFiles(); // 获得当前的文件夹内的所有文件数组

            foreach (FileInfo NextFile in fileInfo) //遍历文件
            {
                if (NextFile.Extension == ".dll" || NextFile.Extension == ".mdl") // 得到你想要的格式
                {
                    try
                    {
                        if (!File.Exists(NextFile.FullName)) continue;
                        //ass.TryAdd(NextFile.FullName, new Tuple<bool, Assembly>(false, null));

                        //ThreadPool.QueueUserWorkItem(_callBack, NextFile.FullName);

                        var assembly = Assembly.LoadFile(NextFile.FullName);
                        var moduleInfo = new ModuleItemInfo(assembly);
                        if (moduleInfo.AssemblyConfigInfo == null) continue;
                        if (!moduleInfo.AssemblyConfigInfo.IsCetc) continue;
                        this.AddItem(moduleInfo);
                    }
                    catch (Exception ex)
                    {
                        UtilityFunction.WriteLog.WriteLogError("Load Assembly Info Error:" + ex);
                    }

                }
            }
            //try
            //{
            //    while (ass.Count > 0)
            //    {
            //        var infso = (from t in ass where t.Value.Item1 select t.Key).ToList();
            //        foreach (var g in infso)
            //        {
            //            Tuple<bool, Assembly> nts = null;
            //            ass.TryRemove(g, out nts);

            //            //var moduleInfo = new ModuleItemInfo(assembly);
            //            //if (moduleInfo.AssemblyConfigInfo == null) continue;
            //            //if (!moduleInfo.AssemblyConfigInfo.IsCetc) continue;
            //            //this.AddItem(moduleInfo);

            //            if (nts != null && nts.Item2 != null)
            //            {
            //                var moduleInfo = new ModuleItemInfo(nts.Item2);
            //                if (moduleInfo.AssemblyConfigInfo == null) continue;
            //                if (!moduleInfo.AssemblyConfigInfo.IsCetc) continue;
            //                this.AddItem(moduleInfo);
            //                this.AddItem(moduleInfo);
            //            }

            //        }

            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        //#region 线程加速 加载

        //private static ConcurrentDictionary<string, Tuple<bool, Assembly>> ass =
        //    new ConcurrentDictionary<string, Tuple<bool, Assembly>>();


        //private static WaitCallback _callBack = new WaitCallback(PooledFunc);
        //private static void PooledFunc(object state)
        //{
        //    var strsss = state as string;
        //    if (string.IsNullOrEmpty(strsss))
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        var assembly = Assembly.LoadFile(strsss);
        //        ass[strsss] = new Tuple<bool, Assembly>(true, assembly);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    if (ass.ContainsKey(strsss))
        //    {
        //        ass[strsss] = new Tuple<bool, Assembly>(true, null);
        //    }
        //}

        //#endregion


        /// <summary>
        /// 保存配置信息到数据库
        /// </summary>
        public void SaveConfig()
        {
            var index = 1;
            var Sql = new string[Items.Count + 1];
            Sql[0] = "delete from  module_config_onload"; // "truncate table module_config_onstart";

            foreach (var t in Items)
            {
                if (t.AssemblyConfigInfo == null) continue;

                Sql[index] =
                    "insert into module_config_onload(module_id ,module_name ,should_load_onrun) values(" +
                    t.AssemblyConfigInfo.ModuleId + ",'" + t.AssemblyConfigInfo.ModuleName + "','" +
                    t.IsAutoLoadAfterLogin  + "')";
                index++;

            }
            SqlLiteHelper.ExecuteTransaction(Sql);
        }


        private void LoadConfig()
        {
            int gggg = 0;
            var xxxx =
                SqlLiteHelper.ExecuteScalar(
                    "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'module_config_onload'");
            try
            {
                gggg = Convert.ToInt32(xxxx);
            }
            catch (Exception ex)
            {
            }
            if (gggg < 1)
            {
                SqlLiteHelper.ExecuteNonQuery(
                    "CREATE TABLE 'module_config_onload' ('module_id' integer PRIMARY KEY,'module_name' text,'should_load_onrun' text)");

            }


            System.Data.DataSet ds = SqlLiteHelper.ExecuteQuery("select * from module_config_onload", null);
            if (ds == null) return;
            int mCount = ds.Tables[0].Rows.Count;
            for (int i = 0; i < mCount; i++)
            {
                try
                {
                    //grp_id,grp_name,status,tml_list,grp_list,grp_server_id
                    var moduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["module_id"].ToString().Trim());
                    var loadOrNot = ds.Tables[0].Rows[i]["should_load_onrun"].ToString().Contains("rue");



                    //var t = PetaPoCoHelp.DB.Query<ModulesInfo>("Select * from module_config_onload");

                    //string baseDirec = System.AppDomain.CurrentDomain.BaseDirectory;
                    //foreach (var f in t)
                    //{
                    this.SetModuleItemAutoLoad(moduleId, loadOrNot);
                    //UpdateItemInfo(f.module_id, f.should_load_onrun);
                    //}
                }

                catch (Exception ex)
                {
                    WriteLog.WriteLogError(
                        "Core CoreModuleLoad Function loadItem from SQLlite table module_config_onstart  Occer an Error:" +
                        ex.ToString());
                }
            }
        }

        #endregion

        /// <summary>
        /// 提供外部读取模块信息
        /// </summary>
        public IEnumerable<IIModuleItemInfo> ModuleItems
        {
            get
            {
                this.AsyItemsDatafromCatalog();
                return Items;
            }
        }

        /// <summary>
        /// 提供外部通知模块内项数发生变法
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void OnNotifyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(sender, e);
            }
        }

        ///// <summary>
        ///// 当模块内部登陆成功自加载发生变法时
        ///// </summary>
        //public event EventHandler OnModuleAutoLoadAfterLoginChanged;

        /// <summary>
        /// 设置模块的自加载设置
        /// </summary>
        public  void SetModuleItemAutoLoad(int moduleId, bool autoLoad)
        {
            foreach (var t in Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == moduleId)
                {
                    if (t.IsAutoLoadAfterLogin != autoLoad)
                    {
                        t.IsAutoLoadAfterLogin = autoLoad;
                        break;
                        //if (OnModuleAutoLoadAfterLoginChanged != null)
                        //{
                        //    this.OnModuleAutoLoadAfterLoginChanged(t, EventArgs.Empty);
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// 当模块加载状态发生变法
        /// </summary>
        public event EventHandler OnModuleLoadedStateChanged;

        /// <summary>
        /// 设置模块的加载状态
        /// </summary>
        private void SetModuleItemLoadedStateChange(int moduleId, bool isModuleLoaded)
        {
            foreach (var t in Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == moduleId)
                {
                    if (t.IsAutoLoadAfterLogin != isModuleLoaded)
                    {
                        t.IsAutoLoadAfterLogin = isModuleLoaded;
                        if (OnModuleLoadedStateChanged != null)
                        {
                            this.OnModuleLoadedStateChanged(t, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置模块的加载状态
        /// </summary>
        private void SetModuleItemLoadedStateChange(string  moduleName, bool isModuleLoaded)
        {
            foreach (var t in Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleName  == moduleName )
                {
                    if (t.IsLoaded  != isModuleLoaded)
                    {
                        t.IsLoaded = isModuleLoaded;
                        if (OnModuleLoadedStateChanged != null)
                        {
                            this.OnModuleLoadedStateChanged(t, EventArgs.Empty);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 通过名称获取模块
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleItemInfo GetModuleInfoByName(string name)
        {
            foreach (var t in this.Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleName.Equals(name)) return t;
            }
            return null;
        }

        /// <summary>
        /// 动态增加程序集
        /// </summary>
        /// <param name="assembly"></param>
        public int AddModuelItem(Assembly assembly)
        {
            ModuleItemInfo moduleItemInfo = new ModuleItemInfo(assembly);
            return AddItem(moduleItemInfo);
        }

        /// <summary>
        /// 动态增加程序集
        /// </summary>
        /// <param name="assemblyPath"></param>
        public int AddModuelItem(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                UtilityFunction.WriteLog.WriteLogInfo("给定地址无法获取文件..." + assemblyPath);
              return   0;
            }
            var assembly = Assembly.LoadFile(assemblyPath);
            var moduleInfo = new ModuleItemInfo(assembly);
            if (moduleInfo.AssemblyConfigInfo == null)
            {
                UtilityFunction.WriteLog.WriteLogInfo("无法获取程序集的配置信息...");
                return 0;
            }
            if (!moduleInfo.AssemblyConfigInfo.IsCetc)
            {
                UtilityFunction.WriteLog.WriteLogInfo("程序集中的配置信息设定非指定公司设计...");
                return 0;
            }
            return this.AddItem(moduleInfo);
        }

        /// <summary>
        /// 添加模块到
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int AddItem(ModuleItemInfo item)
        {
            if (item.AssemblyConfigInfo == null) return 0;
            if (!item.AssemblyConfigInfo.IsCetc) return 0;
            if (item.AssemblyConfigInfo.ModuleId < 1) return 0;
            if (this.ContainsItem(item)) return item.AssemblyConfigInfo.ModuleId;
            this.Items.Add(item);
            return item.AssemblyConfigInfo.ModuleId;
        }

        private bool ContainsItem(ModuleItemInfo itemInfo )
        {
            if (itemInfo.AssemblyConfigInfo == null) return true;
            foreach (var t in this .Items )
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == itemInfo.AssemblyConfigInfo.ModuleId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(ModuleItemInfo item)
        {
            if (item.AssemblyConfigInfo == null) return false;
            if (!item.AssemblyConfigInfo.IsCetc) return false;
            if (item.AssemblyConfigInfo.ModuleId < 1) return false;
            if (item.IsLoaded) return false;


            foreach (var t in this.Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == item.AssemblyConfigInfo.ModuleId)
                {
                    this.Items.Remove(t);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool RemoveItem(int moduleId)
        {

            foreach (var t in this.Items)
            {
                if (t.AssemblyConfigInfo != null && t.AssemblyConfigInfo.ModuleId == moduleId)
                {
                    return this.RemoveItem(t);

                }
            }

            return false;
        }

        protected class ModuleCatalogItemCollection : Collection<ModuleItemInfo>, INotifyCollectionChanged
        {
            public event NotifyCollectionChangedEventHandler CollectionChanged;

            protected override void InsertItem(int index, ModuleItemInfo item)
            {
                base.InsertItem(index, item);

                this.OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                                                                                    item, index));
            }


            protected void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
            {
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, eventArgs);
                }
            }


        }

        #endregion

        /// <summary>
        /// 同步Catalog的加载信息到Item中
        /// </summary>
        private void AsyItemsDatafromCatalog()
        {
            foreach (var t in this.Items)
            {
                if (t.AssemblyConfigInfo != null)
                    this.SetModuleItemLoadedStateChange(t.AssemblyConfigInfo.ModuleName, false);
            }

            try
            {
                foreach (var t in ModuleCatalog.Modules)
                {
                    this.SetModuleItemLoadedStateChange(t.ModuleName, true);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        #region ModuleCatalog

        private ModuleCatalogHelp _moduleCatalog;

        protected ModuleCatalogHelp ModuleCatalog
        {
            get
            {
                if (_moduleCatalog == null) _moduleCatalog = new ModuleCatalogHelp();
                return _moduleCatalog;
            }
        }

        /// <summary>
        /// 实现模块的动态装载功能，系统从boot里面到处add delete模块的函数后通过用户添加程序集来进行动态装载
        /// </summary>
        protected class ModuleCatalogHelp
        {
            private IModuleCatalog _moduleeeCatalog;

            protected IModuleCatalog ModuleCatalog
            {
                get
                {
                    if (_moduleeeCatalog == null)
                        _moduleeeCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
                    return _moduleeeCatalog;
                }
            }

            /// <summary>
            /// 获取所有系统已经加载的模块 
            /// </summary>
            public IEnumerable<Microsoft.Practices.Prism.Modularity.ModuleInfo> Modules
            {
                get
                {
                    if (ModuleCatalog != null) return ModuleCatalog.Modules;
                    return new List<Microsoft.Practices.Prism.Modularity.ModuleInfo>();
                }
            }

            /// <summary>
            /// 运行的模块程序集中是否包含指定的模块
            /// </summary>
            /// <returns></returns>
            public bool IsModuleCatalogContainModuleInfo(string moduleName)
            {

                if (ModuleCatalog != null)
                {
                    return ModuleCatalog.Modules.Any(t => moduleName.Contains(t.ModuleName));
                }
                return false;
            }

            /// <summary>
            /// 获取已经预加载了的所有模块名称
            /// </summary>
            /// <returns></returns>
            public List<string> GetAllLoadModuleNames()
            {
                var lst = new List<string>();
                if (ModuleCatalog != null)
                {
                    lst.AddRange(ModuleCatalog.Modules.Select(t => t.ModuleName));
                }
                return lst;
            }

            /// <summary>
            /// 运行的模块程序集中是是否已经加载模块
            /// </summary>
            /// <returns></returns>
            private bool IsModuleCatalogAlreadyLoadModuleInfo(string moduleName)
            {
                if (ModuleCatalog != null)
                {
                    foreach (var t in ModuleCatalog.Modules)
                    {
                        if (t.ModuleName.Equals(moduleName))
                        {
                            if (t.State == ModuleState.Initialized) return true;
                            else return false;
                        }
                    }
                }
                return false;
            }

        }

        #endregion

        /// <summary>
        /// 解析器
        /// </summary>
        protected class ModuleDependencySolver
        {

            #region 计算依赖项字典

            /// <summary>
            /// 获取需要家长的模块的模块、依赖项信息
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要计算依赖项的模块</param>
            /// <returns>所有模块信息</returns>
            public static Dictionary<string, List<string>> GetDependsMap(ModuleCatalogItemCollection items,
                                                                         List<ModuleItemInfo> needChecks)
            {
                Dictionary<string, List<string>> depends = new Dictionary<string, List<string>>();

                var getDepends = GetCheckModuleDepends(items, needChecks);
                foreach (var t in getDepends)
                {
                    if (depends.ContainsKey(t.Key)) continue;
                    depends.Add(t.Key, t.Value);
                }

                List<string> lstNeedCheck = new List<string>();
                foreach (var t in depends)
                {
                    foreach (var g in t.Value)
                    {
                        if (!depends.ContainsKey(g) && !lstNeedCheck.Contains(g)) lstNeedCheck.Add(g);
                    }
                }

                while (lstNeedCheck.Count > 0)
                {
                    var get = GetCheckModuleDepends(items, lstNeedCheck); //获取需要检查的模块的依赖信息
                    foreach (var t in get)
                    {
                        if (depends.ContainsKey(t.Key)) continue;
                        depends.Add(t.Key, t.Value); //将获取到的信息加入到字典中
                    }
                    lstNeedCheck.Clear();

                    foreach (var t in depends) //再次全部计算未包含在计算中的依赖项信息
                    {
                        foreach (var g in t.Value)
                        {
                            if (!depends.ContainsKey(g) && !lstNeedCheck.Contains(g)) lstNeedCheck.Add(g);
                        }
                    }
                }

                return depends;
            }



            /// <summary>
            /// 计算模块的依赖项，如果该依赖性不存在依然写入字典 
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要检查依赖性的模块</param>
            /// <returns>需要检查依赖项的模块的所有依赖性</returns>
            private static Dictionary<string, List<string>> GetCheckModuleDepends(ModuleCatalogItemCollection items,
                                                                                  List<string> needChecks)
            {
                Dictionary<string, List<string>> depends = new Dictionary<string, List<string>>();
                foreach (var t in needChecks)
                {
                    if (depends.ContainsKey(t)) continue;
                    depends.Add(t, new List<string>());
                    foreach (var f in items)
                    {
                        if (f.AssemblyConfigInfo != null && f.AssemblyConfigInfo.ModuleName.Equals(t))
                        {
                            foreach (var g in f.AssemblyConfigInfo.DependsOnModuleNames)
                            {
                                if (string.IsNullOrEmpty(g)) continue;
                                if (depends[t].Contains(g)) continue;
                                depends[t].Add(g);
                            }
                        }
                    }
                }
                return depends;
            }

            /// <summary>
            /// 计算模块的依赖项，如果该依赖性不存在依然写入字典 
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要检查依赖性的模块</param>
            /// <returns>需要检查依赖项的模块的所有依赖性</returns>
            private static Dictionary<string, List<string>> GetCheckModuleDepends(ModuleCatalogItemCollection items,
                                                                                  List<ModuleItemInfo> needChecks)
            {
                Dictionary<string, List<string>> depends = new Dictionary<string, List<string>>();
                foreach (var t in needChecks)
                {
                    if (t.AssemblyConfigInfo != null)
                    {
                        if (depends.ContainsKey(t.AssemblyConfigInfo.ModuleName)) continue;
                        depends.Add(t.AssemblyConfigInfo.ModuleName, new List<string>());
                        foreach (var f in t.AssemblyConfigInfo.DependsOnModuleNames)
                        {
                            if (string.IsNullOrEmpty(f)) continue;
                            if (depends[t.AssemblyConfigInfo.ModuleName].Contains(f)) continue;
                            depends[t.AssemblyConfigInfo.ModuleName].Add(f);
                        }
                    }
                }
                return depends;
            }

            #endregion

            /// <summary>
            /// 是否需要计算的模块的所有依赖项均存在于模块中
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要检查是否所有依赖项都存在的的模块</param>
            /// <returns></returns>
            public static bool IsAllDependsModuleExist(ModuleCatalogItemCollection items,
                                                       List<ModuleItemInfo> needChecks)
            {
                var result = GetDependsMap(items, needChecks);

                var allModules =
                    (from t in items where t.AssemblyConfigInfo != null select t.AssemblyConfigInfo.ModuleName).ToList();


                foreach (var f in result.Keys)
                {
                    if (allModules.Contains(f)) continue;
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 检查模块的依赖性不存在于给定模块中并将依赖性返回
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要检查依赖项的模块</param>
            /// <returns>模块正常加载需要的一些系统中不存在的模块列表 如果有的话，无则返回空的list</returns>
            public static List<string> GetThisModuleNotExistIn(ModuleCatalogItemCollection items,
                                                               List<ModuleItemInfo> needChecks)
            {
                var lstReturn = new List<string>();
                var result = GetDependsMap(items, needChecks);

                var allModules =
                    (from t in items where t.AssemblyConfigInfo != null select t.AssemblyConfigInfo.ModuleName).ToList();


                foreach (var f in result.Keys)
                {
                    if (allModules.Contains(f)) continue;
                    lstReturn.Add(f);
                }
                return lstReturn;
            }

            /// <summary>
            /// 或许交叉引用模块
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要运行的模块</param>
            /// <returns></returns>
            public static IEnumerable<string> GetThisModuleCrossRef(ModuleCatalogItemCollection items,
                                                                    List<ModuleItemInfo> needChecks)
            {
                //if (!IsAllDependsModuleExist(items, needChecks)) return false;
                var result = GetDependsMap(items, needChecks);
                int maxCount = result.Count;
                int executetime = 0;
                List<string> delete = new List<string>();
                while (result.Count > 0)
                {

                    foreach (var t in result) //从字典中确认依赖性为空的模块并记载到delete集合中
                    {
                        if (t.Value.Count == 0) delete.Add(t.Key);
                    }
                    foreach (var t in delete) //从字典中删除依赖性为空的模块
                    {
                        if (result.ContainsKey(t)) result.Remove(t);
                    }
                    foreach (var t in result) //遍历字典中的模块依赖性，删除已经确认没问题的模块
                    {
                        for (int i = t.Value.Count - 1; i >= 0; i--)
                        {
                            if (delete.Contains(t.Value[i])) t.Value.RemoveAt(i);
                        }
                    }

                    executetime++;
                    if (executetime > maxCount)
                    {
                        //计算次数太多了 肯定存在互相引用的模块
                        break;
                    }
                }
                if (result.Count > 0) return result.Keys;
                return new List<string>();
            }

            /// <summary>
            /// 检查模块是否可以正常运行 包括递归检测
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要运行的模块</param>
            /// <returns></returns>
            public static bool CanDependsModulesNormalLoad(ModuleCatalogItemCollection items,
                                                           List<ModuleItemInfo> needChecks)
            {
                if (!IsAllDependsModuleExist(items, needChecks)) return false;
                var result = GetDependsMap(items, needChecks);
                int maxCount = result.Count;
                int executetime = 0;
                List<string> delete = new List<string>();
                while (result.Count > 0)
                {

                    foreach (var t in result) //从字典中确认依赖性为空的模块并记载到delete集合中
                    {
                        if (t.Value.Count == 0) delete.Add(t.Key);
                    }
                    foreach (var t in delete) //从字典中删除依赖性为空的模块
                    {
                        if (result.ContainsKey(t)) result.Remove(t);
                    }
                    foreach (var t in result) //遍历字典中的模块依赖性，删除已经确认没问题的模块
                    {
                        for (int i = t.Value.Count - 1; i >= 0; i--)
                        {
                            if (delete.Contains(t.Value[i])) t.Value.RemoveAt(i);
                        }
                    }

                    executetime++;
                    if (executetime > maxCount)
                    {
                        //计算次数太多了 肯定存在互相引用的模块
                        break;
                    }
                }
                if (result.Count > 0) return false;
                return true;
            }

            /// <summary>
            /// 获取模块加载顺序
            /// </summary>
            /// <param name="items">所有模块</param>
            /// <param name="needChecks">需要运行的模块</param>
            /// <returns></returns>
            public static List<string> GetThisModulesNormalLoadSqu(ModuleCatalogItemCollection items,
                                                                   List<ModuleItemInfo> needChecks)
            {
                //if (!IsAllDependsModuleExist(items, needChecks)) return false;
                var result = GetDependsMap(items, needChecks);
                int maxCount = result.Count;
                int executetime = 0;
                List<string> lstSqu = new List<string>();
                while (result.Count > 0)
                {

                    foreach (var t in result) //从字典中确认依赖性为空的模块并记载到delete集合中
                    {
                        if (t.Value.Count == 0) lstSqu.Add(t.Key);
                    }
                    foreach (var t in lstSqu) //从字典中删除依赖性为空的模块
                    {
                        if (result.ContainsKey(t)) result.Remove(t);
                    }
                    foreach (var t in result) //遍历字典中的模块依赖性，删除已经确认没问题的模块
                    {
                        for (int i = t.Value.Count - 1; i >= 0; i--)
                        {
                            if (lstSqu.Contains(t.Value[i])) t.Value.RemoveAt(i);
                        }
                    }

                    executetime++;
                    if (executetime > maxCount)
                    {
                        //计算次数太多了 肯定存在互相引用的模块
                        break;
                    }
                }
                //    if (result.Count > 0) return false;
                return lstSqu;
            }
        }

        #region Add Delete Module Fun

        private ModuleControlManage _loadUnloadManage;
        protected  ModuleControlManage LoadUnloadManage
        {
            get
            {
                if (_loadUnloadManage == null)
                {
                    _loadUnloadManage = new ModuleControlManage();
                    _loadUnloadManage.OnLoadModuleCompleted += new EventHandler<LoadModuleCompletedEventArgs>(_loadUnloadManage_OnLoadModuleCompleted);
                }
                return _loadUnloadManage;
            }
        }

        void _loadUnloadManage_OnLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            string name = e.ModuleInfo.ModuleName;
            this.SetModuleItemLoadedStateChange(name, true);
        }


        /// <summary>
        /// 模块加载控制
        /// </summary>
        protected class ModuleControlManage
        {
            private IModuleCatalog _moduleCatalog;

            private IModuleManager _moduleManager;

            //private IModuleManager _moduleManager;
            /// <summary>
            /// 获取bootstrapper中模块添加函数；可实现模块的添加
            /// </summary>
            private Action<ComposablePartCatalog> AddModule { get; set; }

            /// <summary>
            /// 获取bootstrapper中模块删除函数；
            /// </summary>
            private Action<ComposablePartCatalog> RemoveModule { get; set; }

            //定义委托
            private delegate void DoTask(ComposablePartCatalog composablePartCatalog);

            private void GetModuleManager()
            {
                if (_moduleManager == null)
                {
                    try
                    {
                        _moduleManager = ServiceLocator.Current.GetInstance<IModuleManager>();
                        if (_moduleManager != null)
                        {
                            _moduleManager.LoadModuleCompleted +=
                                new EventHandler<LoadModuleCompletedEventArgs>(OnOnLoadModuleCompleted);
                        }
                    }
                    catch (Exception ex)
                    {
                        string fa = ex.ToString();
                    }
                }
            }

            /// <summary>
            /// 当模块加载成功后发布事件
            /// </summary>
            public event EventHandler<LoadModuleCompletedEventArgs> OnLoadModuleCompleted;

            private void OnOnLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
            {

                if (OnLoadModuleCompleted != null) OnLoadModuleCompleted(sender, e);
            }


            /// <summary>
            /// 运行的模块程序集中是否包含指定的模块
            /// </summary>
            /// <returns></returns>
            protected bool IsModuleCatalogContainModuleInfo(string moduleName)
            {

                if (_moduleCatalog == null) _moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
                if (_moduleCatalog != null)
                {
                    return _moduleCatalog.Modules.Any(t => moduleName.Contains(t.ModuleName));
                }
                return false;
            }

            /// <summary>
            /// 运行的模块程序集中是是否已经加载模块
            /// </summary>
            /// <returns></returns>
            protected bool IsModuleCatalogAlreadyLoadModuleInfo(string moduleName)
            {

                if (_moduleCatalog == null) _moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
                if (_moduleCatalog != null)
                {
                    foreach (var t in _moduleCatalog.Modules)
                    {
                        if (t.ModuleName.Equals(moduleName))
                        {
                            if (t.State == ModuleState.Initialized) return true;
                            else return false;
                        }
                    }
                }
                return false;
            }

            #region Load Assembly

            /// <summary>
            /// 动态加载模块
            /// </summary>
            /// <param name="item"></param>
            public  void LoadModuleItem(ModuleItemInfo item)
            {
                try
                {
                    if (item.AssemblyConfigInfo == null)
                    {
                        UtilityFunction.WriteLog.WriteLogInfo("加载的模块配置无模块配置信息，拒绝加载!!!!");
                        return;
                    }
                    if (IsModuleCatalogAlreadyLoadModuleInfo(item.AssemblyConfigInfo.ModuleName))
                    {
                        //UtilityFunction.LogInfo.Log("程序已经加载该模块，拒绝再次加载!!!!");
                        return;
                    }
                    if (this.AddModuleToCatalog(item))
                    {
                        Thread.Sleep(100);
                        if (IsModuleCatalogContainModuleInfo(item.AssemblyConfigInfo.ModuleName))
                        {
                            if (_moduleManager == null) this.GetModuleManager();
                            if (_moduleManager != null)
                            {

                                var args = new PublishEventArgs()
                                {
                                    EventId = ModuleServices.ModuleAssemblyEvent.ModulePreLoadEvent,
                                    EventType = PublishEventType.Core
                                };
                                args.AddParams(item.AssemblyConfigInfo);
                                EventPublish.PublishEvent(args);

                                UtilityFunction.WriteLog.WriteLogInfo("Current Load ...：" +
                                                                       item.AssemblyConfigInfo.ModuleName);
                                _moduleManager.LoadModule(item.AssemblyConfigInfo.ModuleName);

                                UtilityFunction.WriteLog.WriteLogInfo("程序核心加载模块成功，新加载模块为：" +
                                                                        item.AssemblyConfigInfo.ModuleName);
                                var arg = new PublishEventArgs()
                                {
                                    EventId = ModuleServices.ModuleAssemblyEvent.ModuleLoadedEvent ,
                                    EventType = PublishEventType.Core
                                };
                               
                                arg.AddParams(item.AssemblyConfigInfo);
                                arg.AddParams(true);
                                EventPublish.PublishEvent(arg);
                            }
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogInfo("程序核心动态加载模块出错...," + ex);
                    UtilityFunction.WriteLog.WriteLogInfo("程序核心动态加载模块出错...");

                    var arg = new PublishEventArgs()
                    {
                        EventId = ModuleServices.ModuleAssemblyEvent.ModuleLoadedEvent,
                        EventType = PublishEventType.Core
                    };

                    arg.AddParams(item.AssemblyConfigInfo);
                    arg.AddParams(false );
                    EventPublish.PublishEvent(arg);
                }
            }


            /// <summary>
            /// 将模块预加载到模块Catalog中
            /// </summary>
            /// <param name="item"></param>
            private bool AddModuleToCatalog(ModuleItemInfo item)
            {
                try
                {
                    if (item.AssemblyConfigInfo == null)
                    {
                        return false;
                    }
                    if (IsModuleCatalogContainModuleInfo(item.AssemblyConfigInfo.ModuleName))
                    {
                        return false;
                    }

                    if (AddModule == null)
                    {
                        AddModule = ServiceLocator.Current.GetInstance<Action<ComposablePartCatalog>>("AddModule");
                    }
                    if (AddModule == null) return false;

                    if (item.Catalog == null) return false;

                    Application.Current.Dispatcher.Invoke(
                        System.Windows.Threading.DispatcherPriority.Normal, new DoTask(AddModule), item.Catalog);
                    return true;
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogInfo("程序核心动态加载模块出错...," + ex);
                    UtilityFunction.WriteLog.WriteLogInfo("程序核心动态加载模块出错...");
                }
                return false;
            }




            #endregion



            /// <summary>
            /// 获取Catalog中的信息
            /// </summary>
            public IEnumerable<string> GetCatalogLoadModuleNames()
            {
                var lst = new List<string>();
                if (_moduleCatalog == null) _moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
                if (_moduleCatalog != null)
                {
                    foreach (var t in _moduleCatalog.Modules)
                    {
                        lst.Add(t.ModuleName);
                    }
                }
                return lst;
            }



            #region  卸载模块

            /// <summary>
            /// 卸载模块
            /// </summary>
            /// <param name="item"></param>
            public bool  UnLoadModuleItem(ModuleItemInfo item)
            {
                try
                {
                    if (item.AssemblyConfigInfo == null) return false ;
                    //if (item.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoadBeforeLogin) return;
                    //if (item.AssemblyConfigInfo.AutoLoad == ModuleLoadSqu.AutoLoadAfterLogin) return;
                    if (!item.IsLoaded)
                    {
                        UtilityFunction.WriteLog.WriteLogInfo("模块并未运行，请检查....");
                        return false ;
                    }
                    var ff = GetModulesRunDependsOnThisModule(item);
                    if (ff.Count() > 0)
                    {
                        string st = "";
                        foreach (var t in ff) st += t + ";";
                        UtilityFunction.WriteLog.WriteLogInfo("系统不允许卸载该模块，至少正在运行的模块：" + st + "正常运行需要该模块：" +
                                                                item.AssemblyConfigInfo.ModuleName +
                                                                "的支持，如果需要卸载，请至少先卸载:" +
                                                                st);

                        MessageBox.Show("系统不允许卸载该模块，至少正在运行的模块：" + st + " 正常运行需要该模块：" +
                                        item.AssemblyConfigInfo.ModuleName +
                                        "的支持，如果需要卸载，请至少先卸载:" +
                                        st, "卸载出错", MessageBoxButton.OK);
                        return false ;
                    }

                    if (RemoveModule == null)
                    {
                        RemoveModule = ServiceLocator.Current.GetInstance<Action<ComposablePartCatalog>>("RemoveModule");
                    }

                    var args = new PublishEventArgs()
                    {
                        EventId = ModuleServices.ModuleAssemblyEvent.ModulePreUnLoadEvent,
                        EventType = PublishEventType.Core
                    };
                    args.AddParams(item.AssemblyConfigInfo);
                    EventPublish.PublishEvent(args);
                    //  DestructorComposablePartCatalog(lstAssemblyInfo[i].assembly);
                    UtilityFunction.WriteLog.WriteLogInfo("正在卸载程序集：" + item.AssemblyConfigInfo.ModuleName + "...");
                    Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                                                          new DoTask(RemoveModule), item.Catalog);
                    UtilityFunction.WriteLog.WriteLogInfo( "卸载程序集：" + item.AssemblyConfigInfo.ModuleName + " 完成。");
                    //item.IsLoaded = false;
                    var arg = new PublishEventArgs()
                                  {
                                      EventId = ModuleServices.ModuleAssemblyEvent.ModuleUnLoadedEvent,
                                      EventType = PublishEventType.Core
                                  };
                    arg.AddParams(item.AssemblyConfigInfo);
                    arg.AddParams(true);
                    EventPublish.PublishEvent( arg);
                    return true;
                    
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogInfo("程序核心动态卸载模块出错...," + ex);
                    UtilityFunction.WriteLog.WriteLogInfo("程序核心动态卸载模块出错...");
                    var arg = new PublishEventArgs()
                    {
                        EventId = ModuleServices.ModuleAssemblyEvent.ModuleUnLoadedEvent,
                        EventType = PublishEventType.Core
                    };
                    arg.AddParams(item.AssemblyConfigInfo);
                    arg.AddParams(false );
                    EventPublish.PublishEvent(arg);
                }
                return false;
            }

            /// <summary>
            /// 查询是否有正在运行的模块 依赖于本模块
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            private IEnumerable<string> GetModulesRunDependsOnThisModule(ModuleItemInfo item)
            {
                var lst = new List<string>();
                if (item.AssemblyConfigInfo == null) return lst;
                ;
                if (_moduleCatalog == null) _moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
                if (_moduleCatalog != null)
                {
                    foreach (var t in _moduleCatalog.Modules)
                    {
                        if (t.State == ModuleState.Initialized)
                        {
                            foreach (var f in t.DependsOn)
                            {
                                if (f.Equals(item.AssemblyConfigInfo.ModuleName))
                                {
                                    lst.Add(t.ModuleName );
                                }
                            }
                        }
                    }
                }
                return lst;
            }

            #endregion









        }


        #endregion
    }
}
