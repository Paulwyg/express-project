using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace Wlst.Cr.Core.Modularity
{
    /// <summary>
    /// 模块信息
    /// </summary>
    public class ModuleItemInfo : IIModuleItemInfo
    {
        /// <summary>
        /// 模块信息
        /// </summary>
        /// <param name="assembly"></param>
        public ModuleItemInfo(Assembly assembly)
        {
            this.IsLoaded = false;
            this.Aassembly = assembly;
        }

        /// <summary>
        /// 通过路径获取到的程序集处理后的catalog
        /// </summary>
        public ComposablePartCatalog Catalog { get; private set; }

        private Assembly _assembly;

        /// <summary>
        /// 程序集信息
        /// </summary>
        public Assembly Aassembly
        {
            get { return _assembly; }
            private set
            {
                if (_assembly != value)
                {
                    _assembly = value;
                    AssemblyInfo = new AssemblyInfo(_assembly);
                    Catalog = new AssemblyCatalog(_assembly);
                }
            }
        }

        private AssemblyInfo _assemblyInfo;

        /// <summary>
        /// 通过路径获取到的模块程序集
        /// </summary>
        public AssemblyInfo AssemblyInfo
        {
            get { return _assemblyInfo; }
            private set
            {
                if (value == _assemblyInfo) return;
                _assemblyInfo = value;
                if (_assemblyInfo.Configuration == null)
                {
                    AssemblyConfigInfo = null;
                    return;
                }
                string config = this._assemblyInfo.Configuration.Configuration;
                AssemblyConfigInfo = new AssemblyConfig(config);
            }
        }

        /// <summary>
        /// 模块的配置信息
        /// </summary>
        public AssemblyConfig AssemblyConfigInfo { get; set; }

        /// <summary>
        /// 模块是否已经加载
        /// </summary>
        public bool IsLoaded { get; set; }

        /// <summary>
        /// 是否需要在程序登陆成功时立即加载
        /// </summary>
        public bool IsAutoLoadAfterLogin { get; set; }

    }
}
