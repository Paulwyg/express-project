using System;
using System.Reflection;

namespace Wlst.Cr.Core.Modularity
{
    public class AssemblyInfo
    {
        /// <summary>
        /// 获取程序集
        /// </summary>
        public Assembly assembly { get; private set; }

        public AssemblyInfo(Assembly assembly)
        {
            try
            {
                this.assembly = assembly;
                Configuration =
                    GetAttributes(assembly, typeof (AssemblyConfigurationAttribute)) as AssemblyConfigurationAttribute;


                NameInfo = assembly.GetName().ToString();
                VersionInfo =
                    GetAttributes(assembly, typeof (AssemblyFileVersionAttribute)) as AssemblyFileVersionAttribute;
                CompanyInfo = GetAttributes(assembly, typeof (AssemblyCompanyAttribute)) as
                              AssemblyCompanyAttribute;
                ProductInfo = GetAttributes(assembly, typeof (AssemblyProductAttribute)) as
                              AssemblyProductAttribute;

                TitleInfo = GetAttributes(assembly, typeof (AssemblyTitleAttribute)) as AssemblyTitleAttribute;
                CopyrightInfo = GetAttributes(assembly, typeof (AssemblyCopyrightAttribute)) as
                                AssemblyCopyrightAttribute;
                DescriptionInfo = GetAttributes(assembly, typeof (AssemblyDescriptionAttribute)) as
                                  AssemblyDescriptionAttribute;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                //CETC50_Core.UtilityFunction..Log("AssemblyInfo get Error:" + ex);
            }
        }

        private object GetAttributes(Assembly info, Type type)
        {
            try
            {
                return info.GetCustomAttributes(type, false)[0];
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public AssemblyConfigurationAttribute Configuration { get; private set; }

        /// <summary>         
        ///  版本信息        
        ///   </summary>         
        public AssemblyFileVersionAttribute VersionInfo { get; private set; }

        /// <summary>
        /// 公司信息
        /// </summary>
        public AssemblyCompanyAttribute CompanyInfo { get; private set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public AssemblyProductAttribute ProductInfo { get; private set; }

        /// <summary>
        /// 标题信息
        /// </summary>
        public AssemblyTitleAttribute TitleInfo { get; private set; }

        /// <summary>
        /// 标题信息
        /// </summary>
        public string NameInfo { get; private set; }

        /// <summary>
        /// 版权信息
        /// </summary>
        public AssemblyCopyrightAttribute CopyrightInfo { get; private set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public AssemblyDescriptionAttribute DescriptionInfo { get; private set; }
    }
}
