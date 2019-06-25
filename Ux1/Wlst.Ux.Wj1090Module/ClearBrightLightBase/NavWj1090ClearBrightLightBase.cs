using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.CoreOne.UtilityFunction;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

namespace Wlst.Ux.Wj1090Module.ClearBrightLightBase
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavWj1090ClearBrightLightBase:MenuItemBase
    {
        public NavWj1090ClearBrightLightBase()
        {
            Id = Services.MenuIdAssgin.NavToClearBrightLightBaseId;
            Text = "清除亮灯率";
            Tag = "清除亮灯率";
            Classic = "右键菜单-线路防盗集中器-专有";
            Description = "线路防盗集中器清除亮灯率，防盗通用，ID 为" + Services.MenuIdAssgin.NavToClearBrightLightBaseId;
            Tooltips = "清除亮灯率";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx,true);
            IsPrivilegLeave = true;

            InitAction();
        }

        private static bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            var equipment = Argu as IIEquipmentInfo;
            if (equipment == null)
                return;
            LogInfo.Log("正在发送清除亮灯率命令!!!");
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ClearBrightLightBase;
            info.Data.RtuId =equipment.RtuId ;
            info.Data.ControlId = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
            Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ClearBrightLightBase,
            GetRecClearBrightLightData,
            typeof(NavWj1090ClearBrightLightBase), this);
        }

        private void GetRecClearBrightLightData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
        {
            var info = infos.AddrLst;
            if(info.Count<1) return;
            try
            {
                //var rtuId = infos.AddrLst[0];
                var controlId = infos.AddrLst[0];
                LogInfo.Log("编号为：" + controlId.ToString(CultureInfo.InvariantCulture)+"的集中器清除亮灯率成功！");
            }
            catch (Exception e)
            {
                LogInfo.Log("WJ1090清除亮灯率异常，"+e);
            }


        }
    }
}
