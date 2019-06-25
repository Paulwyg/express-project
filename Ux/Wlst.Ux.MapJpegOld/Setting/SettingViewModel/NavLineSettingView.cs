using System.ComponentModel.Composition;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.RadMapJpeg.Setting.SettingViewModel
{
    [Export(typeof(IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavLineSettingView :SettingBase  
    {
        public NavLineSettingView()
        {
            this.Id =RadMapJpeg .Services .MenuIdAssgin .NavLineMapSettingViewId;
            this.ViewId = RadMapJpeg.Services.ViewIdAssign.SettingViewId ;// Infrastructure.IdAssign.ViewIdNameAssign.RadMapJpegSettingViewId;
            this.PathSetting = "地图选项";
        }
    }
}