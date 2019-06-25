using System.ComponentModel.Composition;
using Wlst.Cr.Core.CommandCore;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.RecordMsg
{
   
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRecordMsgView : MenuItemBase
    {
        public NavToRecordMsgView()
        {
            Id =EquipemntLightFault .Services .MenuIdAssgin.NavToRecordMsgViewId ;
            Text = "短信记录";
            Tag = "短信记录";
            //    Describle = "通信设置，ID 为" + Msg.Services.MenuIdAssgin.NavToDeviceSet;
            Tooltips = "短信记录";
            base.IsEnabled = true;
            base.IsCheckable = false;
            Description = "查询短消息发送记录"; Classic = "主菜单";
            base.Command = new RelayCommand(Ex,CanEx ,true );
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            this.ExNavWithArgs( EquipemntLightFault.Services.ViewIdAssign.RecordMsgViewId,
                               1);
        }


    }
}
