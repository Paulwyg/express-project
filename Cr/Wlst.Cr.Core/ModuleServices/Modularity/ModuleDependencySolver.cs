//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Wlst.Cr.Core.Modularity
//{
//    /// <summary>
//    /// 解析器
//    /// </summary>
//    public  class ModuleDependencySolver
//    {

//        #region 计算依赖项字典

//        /// <summary>
//        /// 获取需要家长的模块的模块、依赖项信息
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要计算依赖项的模块</param>
//        /// <returns>所有模块信息</returns>
//        public static Dictionary<string, List<string>> GetDependsMap(ModuleCatalogItemCollection items,
//                                                                     List<ModuleItemInfo> needChecks)
//        {
//            Dictionary<string, List<string>> depends = new Dictionary<string, List<string>>();

//            var getDepends = GetCheckModuleDepends(items, needChecks);
//            foreach (var t in getDepends)
//            {
//                if (depends.ContainsKey(t.Key)) continue;
//                depends.Add(t.Key, t.Value);
//            }

//            List<string> lstNeedCheck = new List<string>();
//            foreach (var t in depends)
//            {
//                foreach (var g in t.Value)
//                {
//                    if (!depends.ContainsKey(g) && !lstNeedCheck.Contains(g)) lstNeedCheck.Add(g);
//                }
//            }

//            while (lstNeedCheck.Count > 0)
//            {
//                var get = GetCheckModuleDepends(items, lstNeedCheck); //获取需要检查的模块的依赖信息
//                foreach (var t in get)
//                {
//                    if (depends.ContainsKey(t.Key)) continue;
//                    depends.Add(t.Key, t.Value); //将获取到的信息加入到字典中
//                }
//                lstNeedCheck.Clear();

//                foreach (var t in depends) //再次全部计算未包含在计算中的依赖项信息
//                {
//                    foreach (var g in t.Value)
//                    {
//                        if (!depends.ContainsKey(g) && !lstNeedCheck.Contains(g)) lstNeedCheck.Add(g);
//                    }
//                }
//            }

//            return depends;
//        }



//        /// <summary>
//        /// 计算模块的依赖项，如果该依赖性不存在依然写入字典 
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要检查依赖性的模块</param>
//        /// <returns>需要检查依赖项的模块的所有依赖性</returns>
//        private static Dictionary<string, List<string>> GetCheckModuleDepends(ModuleCatalogItemCollection items,
//                                                                              List<string> needChecks)
//        {
//            Dictionary<string, List<string>> depends = new Dictionary<string, List<string>>();
//            foreach (var t in needChecks)
//            {
//                if (depends.ContainsKey(t)) continue;
//                depends.Add(t, new List<string>());
//                foreach (var f in items)
//                {
//                    if (f.AssemblyConfigInfo != null && f.AssemblyConfigInfo.ModuleName.Equals(t))
//                    {
//                        foreach (var g in f.AssemblyConfigInfo.DependsOnModuleNames)
//                        {
//                            if (string.IsNullOrEmpty(g)) continue;
//                            if (depends[t].Contains(g)) continue;
//                            depends[t].Add(g);
//                        }
//                    }
//                }
//            }
//            return depends;
//        }

//        /// <summary>
//        /// 计算模块的依赖项，如果该依赖性不存在依然写入字典 
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要检查依赖性的模块</param>
//        /// <returns>需要检查依赖项的模块的所有依赖性</returns>
//        private static Dictionary<string, List<string>> GetCheckModuleDepends(ModuleCatalogItemCollection items,
//                                                                              List<ModuleItemInfo> needChecks)
//        {
//            Dictionary<string, List<string>> depends = new Dictionary<string, List<string>>();
//            foreach (var t in needChecks)
//            {
//                if (t.AssemblyConfigInfo != null)
//                {
//                    if (depends.ContainsKey(t.AssemblyConfigInfo.ModuleName)) continue;
//                    depends.Add(t.AssemblyConfigInfo.ModuleName, new List<string>());
//                    foreach (var f in t.AssemblyConfigInfo.DependsOnModuleNames)
//                    {
//                        if (string.IsNullOrEmpty(f)) continue;
//                        if (depends[t.AssemblyConfigInfo.ModuleName].Contains(f)) continue;
//                        depends[t.AssemblyConfigInfo.ModuleName].Add(f);
//                    }
//                }
//            }
//            return depends;
//        }

//        #endregion

//        /// <summary>
//        /// 是否需要计算的模块的所有依赖项均存在于模块中
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要检查是否所有依赖项都存在的的模块</param>
//        /// <returns></returns>
//        public static bool IsAllDependsModuleExist(ModuleCatalogItemCollection items,
//                                                   List<ModuleItemInfo> needChecks)
//        {
//            var result = GetDependsMap(items, needChecks);

//            var allModules =
//                (from t in items where t.AssemblyConfigInfo != null select t.AssemblyConfigInfo.ModuleName).ToList();


//            foreach (var f in result.Keys)
//            {
//                if (allModules.Contains(f)) continue;
//                return false;
//            }
//            return true;
//        }

//        /// <summary>
//        /// 检查模块的依赖性不存在于给定模块中并将依赖性返回
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要检查依赖项的模块</param>
//        /// <returns>模块正常加载需要的一些系统中不存在的模块列表 如果有的话，无则返回空的list</returns>
//        public static List<string> GetThisModuleNotExistIn(ModuleCatalogItemCollection items,
//                                                           List<ModuleItemInfo> needChecks)
//        {
//            var lstReturn = new List<string>();
//            var result = GetDependsMap(items, needChecks);

//            var allModules =
//                (from t in items where t.AssemblyConfigInfo != null select t.AssemblyConfigInfo.ModuleName).ToList();


//            foreach (var f in result.Keys)
//            {
//                if (allModules.Contains(f)) continue;
//                lstReturn.Add(f);
//            }
//            return lstReturn;
//        }

//        /// <summary>
//        /// 或许交叉引用模块
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要运行的模块</param>
//        /// <returns></returns>
//        public static IEnumerable<string> GetThisModuleCrossRef(ModuleCatalogItemCollection items,
//                                                                List<ModuleItemInfo> needChecks)
//        {
//            //if (!IsAllDependsModuleExist(items, needChecks)) return false;
//            var result = GetDependsMap(items, needChecks);
//            int maxCount = result.Count;
//            int executetime = 0;
//            List<string> delete = new List<string>();
//            while (result.Count > 0)
//            {

//                foreach (var t in result) //从字典中确认依赖性为空的模块并记载到delete集合中
//                {
//                    if (t.Value.Count == 0) delete.Add(t.Key);
//                }
//                foreach (var t in delete) //从字典中删除依赖性为空的模块
//                {
//                    if (result.ContainsKey(t)) result.Remove(t);
//                }
//                foreach (var t in result) //遍历字典中的模块依赖性，删除已经确认没问题的模块
//                {
//                    for (int i = t.Value.Count - 1; i >= 0; i--)
//                    {
//                        if (delete.Contains(t.Value[i])) t.Value.RemoveAt(i);
//                    }
//                }

//                executetime++;
//                if (executetime > maxCount)
//                {
//                    //计算次数太多了 肯定存在互相引用的模块
//                    break;
//                }
//            }
//            if (result.Count > 0) return result.Keys;
//            return new List<string>();
//        }

//        /// <summary>
//        /// 检查模块是否可以正常运行 包括递归检测
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要运行的模块</param>
//        /// <returns></returns>
//        public static bool CanDependsModulesNormalLoad(ModuleCatalogItemCollection items,
//                                                       List<ModuleItemInfo> needChecks)
//        {
//            if (!IsAllDependsModuleExist(items, needChecks)) return false;
//            var result = GetDependsMap(items, needChecks);
//            int maxCount = result.Count;
//            int executetime = 0;
//            List<string> delete = new List<string>();
//            while (result.Count > 0)
//            {

//                foreach (var t in result) //从字典中确认依赖性为空的模块并记载到delete集合中
//                {
//                    if (t.Value.Count == 0) delete.Add(t.Key);
//                }
//                foreach (var t in delete) //从字典中删除依赖性为空的模块
//                {
//                    if (result.ContainsKey(t)) result.Remove(t);
//                }
//                foreach (var t in result) //遍历字典中的模块依赖性，删除已经确认没问题的模块
//                {
//                    for (int i = t.Value.Count - 1; i >= 0; i--)
//                    {
//                        if (delete.Contains(t.Value[i])) t.Value.RemoveAt(i);
//                    }
//                }

//                executetime++;
//                if (executetime > maxCount)
//                {
//                    //计算次数太多了 肯定存在互相引用的模块
//                    break;
//                }
//            }
//            if (result.Count > 0) return false;
//            return true;
//        }

//        /// <summary>
//        /// 获取模块加载顺序
//        /// </summary>
//        /// <param name="items">所有模块</param>
//        /// <param name="needChecks">需要运行的模块</param>
//        /// <returns></returns>
//        public static List<string> GetThisModulesNormalLoadSqu(ModuleCatalogItemCollection items,
//                                                               List<ModuleItemInfo> needChecks)
//        {
//            //if (!IsAllDependsModuleExist(items, needChecks)) return false;
//            var result = GetDependsMap(items, needChecks);
//            int maxCount = result.Count;
//            int executetime = 0;
//            List<string> lstSqu = new List<string>();
//            while (result.Count > 0)
//            {

//                foreach (var t in result) //从字典中确认依赖性为空的模块并记载到delete集合中
//                {
//                    if (t.Value.Count == 0) lstSqu.Add(t.Key);
//                }
//                foreach (var t in lstSqu) //从字典中删除依赖性为空的模块
//                {
//                    if (result.ContainsKey(t)) result.Remove(t);
//                }
//                foreach (var t in result) //遍历字典中的模块依赖性，删除已经确认没问题的模块
//                {
//                    for (int i = t.Value.Count - 1; i >= 0; i--)
//                    {
//                        if (lstSqu.Contains(t.Value[i])) t.Value.RemoveAt(i);
//                    }
//                }

//                executetime++;
//                if (executetime > maxCount)
//                {
//                    //计算次数太多了 肯定存在互相引用的模块
//                    break;
//                }
//            }
//            //    if (result.Count > 0) return false;
//            return lstSqu;
//        }
//    }
//}
