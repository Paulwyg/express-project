using System.ComponentModel.Composition.Primitives;

namespace Wlst.Cr.Core.Modularity
{
   /// <summary>
   /// 
   /// </summary>
   public  interface IIModuleItemInfo
    {
       /// <summary>
       /// 通过路径获取到的模块程序集
       /// </summary>
       AssemblyInfo AssemblyInfo { get; }


       /// <summary>
       /// 通过路径获取到的程序集处理后的catalog
       /// </summary>
        ComposablePartCatalog Catalog { get; }

       /// <summary>
        /// 模块的配置信息
        /// </summary>
         AssemblyConfig AssemblyConfigInfo { get;  }

        /// <summary>
        /// 模块是否已经加载
        /// </summary>
         bool IsLoaded { get;  }

        /// <summary>
        /// 是否需要在程序登陆成功时立即加载
        /// </summary>
         bool IsAutoLoadAfterLogin { get;  }
    }
}
