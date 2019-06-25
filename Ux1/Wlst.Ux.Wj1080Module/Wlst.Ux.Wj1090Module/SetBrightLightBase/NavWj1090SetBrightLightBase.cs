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

namespace Wlst.Ux.Wj1090Module.SetBrightLightBase
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavWj1090SetBrightLightBase : MenuItemBase
    {
        public NavWj1090SetBrightLightBase()
        {
            Id = Services.MenuIdAssgin.NavToSetBrightLightBaseId;
            Text = "设置亮灯率";
            Tag = "设置亮灯率";
            Classic = "右键菜单-线路防盗集中器-专有";
            Description = "线路防盗集中器清除亮灯率，防盗通用，ID 为" + Services.MenuIdAssgin.NavToSetBrightLightBaseId;
            Tooltips = "设置亮灯率";
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
            LogInfo.Log("正在发送设置亮灯率命令!!!");
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_SetBrightLightBase;
            info.Data.RtuId = equipment.RtuId;
            info.Data.ControlId = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
            Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetBrightLightBase,
            GetRecSetBrightLightData,
            typeof(NavWj1090SetBrightLightBase), this);
        }

        private void GetRecSetBrightLightData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
        {
            var info = infos.AddrLst;
            if (info.Count < 1) return;
            try
            {
                //var rtuId = infos.AddrLst[0];
                var controlId = infos.AddrLst[0];
                LogInfo.Log("编号为："+controlId.ToString(CultureInfo.InvariantCulture)+"的集中器设置亮灯率成功！！");

            }
            catch (Exception)
            {
                {}
                throw;
            }


        }
    }
}
