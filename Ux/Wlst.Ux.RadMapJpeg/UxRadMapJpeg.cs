using System.Threading;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.RadMapJpeg.Views;

namespace Wlst.Ux.RadMapJpeg
{
    [ModuleExport(typeof (UxRadMapJpeg))]
    public class UxRadMapJpeg : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new Setting.SettingExtend().InitLoad();
          
        }

        //View i36N id 11010000~11019999

     

        #endregion
    }
}