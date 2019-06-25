using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.Menu
{
    [ModuleExport(typeof (UxMenuModule))]
    public class UxMenuModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            ViewModel.MenuViewModule.PublishResetMenuEvent();//如果为卸载菜单模块后再加载  则需要发布一个事件触发
        }

        #endregion
    }
}