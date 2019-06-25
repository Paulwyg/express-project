using System.Windows;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel
{

    public class ListTreeTmlNode : ListTreeNodeBase
    {
        public ListTreeTmlNode()
        {
            Visi = Visibility.Visible;
            this.IsListTreeNodeGroup = false;
            this.IsThisTmlSpecialVisual  = Visibility.Visible;
            this.IsThisGroupHasSpecialVisual  = Visibility.Collapsed;
            this.IsThisTmlSpecialTerminalEnable  = true;
        }

        public ListTreeTmlNode(ListTreeNodeBase mvvmFather, int tmlId)
        {
            Visi = Visibility.Visible;
            this._father = mvvmFather;
            this.IsListTreeNodeGroup = false;
            this.IsThisTmlSpecialVisual = Visibility.Visible;
            this.IsThisGroupHasSpecialVisual = Visibility.Collapsed;
            this.IsThisTmlSpecialTerminalEnable = true;
            this.NodeId = tmlId;
            this.PhyId = tmlId;
            GetNodeInfomation();
            GetNodeTimeTableInformation();
        }

        private void GetNodeInfomation()
        {
            if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .ContainsKey(this.NodeId)) return;
            var f =
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
                    this.NodeId];
            this.NodeName = f.RtuName;
            this.PhyId = f.PhyId;
        }

        private void GetNodeTimeTableInformation()
        {
            this.K1TimeTalbe =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetTmlWeekTableBelongInfo(this.NodeId, 1);
            this.K2TimeTalbe =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetTmlWeekTableBelongInfo(this.NodeId, 2);
            this.K3TimeTalbe =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetTmlWeekTableBelongInfo(this.NodeId, 3);
            this.K4TimeTalbe =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetTmlWeekTableBelongInfo(this.NodeId, 4);
            this.K5TimeTalbe =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetTmlWeekTableBelongInfo(this.NodeId, 5);
            this.K6TimeTalbe =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetTmlWeekTableBelongInfo(this.NodeId, 6);

            if (_father == null)
            {
                IsThisTmlSpecialTerminal = true;
                return;
            }
            if (K1TimeTalbe == _father.K1TimeTalbe &&
                K2TimeTalbe == _father.K2TimeTalbe &&
                K3TimeTalbe == _father.K3TimeTalbe &&
                K4TimeTalbe == _father.K4TimeTalbe &&
                K5TimeTalbe == _father.K5TimeTalbe &&
                K6TimeTalbe == _father.K6TimeTalbe
                )
                this.IsThisTmlSpecialTerminal = false;
            else
                this.IsThisTmlSpecialTerminal = true;


        }

    }
}
