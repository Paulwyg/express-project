using System;
using System.Collections.Generic;

namespace Wlst.Cr.Core.Modularity
{
    /// <summary>
    /// Company=Cetc50;ModuleName=CoreModuleConfig;ModuleId=7;AutoLoad=2;DependsOnModuleNames=a,b,c,d
    /// </summary>
    public class AssemblyConfig
    {
        //Company=Cetc50;ModuleName=CoreModuleConfig;ModuleId=7;AutoLoad=2;DependsOnModuleNames=a,b,c,d
        /// <summary>
        /// 公司名字大写
        /// </summary>
        public string CompanyUpper;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName;
        /// <summary>
        /// 模块Id
        /// </summary>
        public int ModuleId;
        /// <summary>
        /// 启动设置
        /// </summary>
        public ModuleLoadSqu AutoLoad;
        /// <summary>
        /// 依赖模块集
        /// </summary>
        public List<string> DependsOnModuleNames;

        /// <summary>
        /// 是否为Cetc
        /// </summary>
        public bool IsCetc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">Company=Cetc50;ModuleName=CoreModuleConfig;ModuleId=7;AutoLoad=2;DependsOnModuleNames=a,b,c,d</param>
        public AssemblyConfig(string config)
        {
            this.CompanyUpper = string.Empty;
            this.ModuleName = string.Empty;
            DependsOnModuleNames = new List<string>();
            string[] sp = config.Split(';');
            for (int i = 1; i < sp.Length + 1; i++)
            {
                Ana(sp[i - 1], i);
            }

            this.IsCetc = this.CompanyUpper.Contains("CETC");
        }

        private void Ana(string config, int index)
        {

            string[] sp = config.Split('=');
            if (sp.Length < 2) return;
            switch (index)
            {
                case 1:
                    this.CompanyUpper = sp[1].Trim().ToUpper();
                    break;
                case 2:
                    this.ModuleName = sp[1].Trim();
                    break;
                case 3:
                    try
                    {
                        this.ModuleId = Convert.ToInt32(sp[1].Trim());
                    }
                    catch (Exception ex)
                    {
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                    }
                    break;
                case 4:
                    try
                    {
                        int auto = Convert.ToInt32(sp[1].Trim());
                        if (auto == 1) this.AutoLoad = ModuleLoadSqu.AutoLoad ;
                        else if (auto == 2) this.AutoLoad = ModuleLoadSqu.AutoLoad ;
                        else this.AutoLoad = ModuleLoadSqu.LoadByUserSet;

                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                case 5:
                    string[] spdepends = sp[1].Trim().Split(',');
                    this.DependsOnModuleNames.Clear();
                    foreach (var t in spdepends)
                    {
                        if (!string.IsNullOrEmpty(t)) DependsOnModuleNames.Add(t.Trim());
                    }
                    break;

            }

        }

    };

    /// <summary>
    /// 模块加载顺序
    /// </summary>
    public enum ModuleLoadSqu
    {
        /// <summary>
        /// 在程序运行的时候立即加载 1
        /// </summary>
        AutoLoad=1,

        /// <summary>
        /// 根据用户设置 在启动后再动态加载 3
        /// </summary>
        LoadByUserSet,
    }
}
