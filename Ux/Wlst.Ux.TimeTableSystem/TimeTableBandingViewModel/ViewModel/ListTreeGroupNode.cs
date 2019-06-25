using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel
{

    public class ListTreeGroupNode : ListTreeNodeBase
    {
        public ListTreeGroupNode()
        {
            Visi = Visibility.Visible;
            this.IsListTreeNodeGroup = true;
            this.IsThisGroupHasSpecialVisual = Visibility.Visible;
            this.IsThisTmlSpecialVisual = Visibility.Collapsed;
        }

        public ListTreeGroupNode(ListTreeNodeBase mvvmFather, int groupId)
        {
            Visi = Visibility.Visible;
            this._father = mvvmFather;
            this.IsListTreeNodeGroup = true;
            this.IsThisGroupHasSpecialVisual = Visibility.Visible;
            this.IsThisTmlSpecialVisual = Visibility.Collapsed;
            this.NodeId = groupId;
            this.PhyId = groupId;
            GetNodeInfomation();

            this.GetNodeTimeTableInformation();


            this.AddChild();
            // GetNodeTimeTableIdInformation();
            UpdateTmlTimeTableInfo();
        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();
            if (!IsListTreeNodeGroup) return;
            //group

            //添加分组到子节点中
            if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId))
                return;
            var atttmp = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].LstGrp);

            foreach (
                var t in atttmp)
            {
                if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(t))
                    continue;
                ChildTreeItems.Add(new ListTreeGroupNode(this, t));
            }
            //加载终端节点
            //var ordtml =
            //    (from t in
            //         Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].LstTml
            //     orderby t ascending
            //     select t).ToList();
            var ordtml = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].LstTml);


            foreach ( var t in ordtml)
            {
                if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(t))
                    continue;
                var f =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[t] as
                    IIRtuParaWork;
                if (f == null) continue;
                ChildTreeItems.Add(new ListTreeTmlNode(this, f.RtuId));
            }
        }

        private void GetNodeInfomation()
        {
            if (Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId))
                this.NodeName =
                    Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].GroupName;
        }


        private void GetNodeTimeTableInformation()
        {
            this.K1TimeTalbe =
                Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo( this.NodeId, 1);
            this.K2TimeTalbe =
                Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(this.NodeId, 2);
            this.K3TimeTalbe =
                Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(this.NodeId, 3);
            this.K4TimeTalbe =
                Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(this.NodeId, 4);
            this.K5TimeTalbe =
                Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(this.NodeId, 5);
            this.K6TimeTalbe =
                Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(this.NodeId, 6);
        }

        /// <summary>
        /// 根据子节点的时间表情况统计出来 本组的 时间表   以最多的那张时间表为准 其他的均为特殊时间表
        /// </summary>
        private void GetNodeTimeTableIdInformation()
        {
            Dictionary<int, int> countK1 = new Dictionary<int, int>();
            Dictionary<int, int> countK2 = new Dictionary<int, int>();
            Dictionary<int, int> countK3 = new Dictionary<int, int>();
            Dictionary<int, int> countK4 = new Dictionary<int, int>();
            Dictionary<int, int> countK5 = new Dictionary<int, int>();
            Dictionary<int, int> countK6 = new Dictionary<int, int>();
            foreach (var t in this.ChildTreeItems)
            {
                if (t.IsListTreeNodeGroup) continue;
                if (countK1.ContainsKey(t.K1TimeTalbe)) countK1[t.K1TimeTalbe] = countK1[t.K1TimeTalbe] + 1;
                else countK1.Add(t.K1TimeTalbe, 1);

                if (countK2.ContainsKey(t.K2TimeTalbe)) countK2[t.K2TimeTalbe] = countK2[t.K2TimeTalbe] + 1;
                else countK2.Add(t.K2TimeTalbe, 1);

                if (countK3.ContainsKey(t.K3TimeTalbe)) countK3[t.K3TimeTalbe] = countK3[t.K3TimeTalbe] + 1;
                else countK3.Add(t.K3TimeTalbe, 1);

                if (countK4.ContainsKey(t.K4TimeTalbe)) countK4[t.K4TimeTalbe] = countK4[t.K4TimeTalbe] + 1;
                else countK4.Add(t.K4TimeTalbe, 1);

                if (countK5.ContainsKey(t.K5TimeTalbe)) countK5[t.K5TimeTalbe] = countK5[t.K5TimeTalbe] + 1;
                else countK5.Add(t.K5TimeTalbe, 1);

                if (countK6.ContainsKey(t.K6TimeTalbe)) countK6[t.K6TimeTalbe] = countK6[t.K6TimeTalbe] + 1;
                else countK6.Add(t.K6TimeTalbe, 1);
            }

            int max = 0;
            int timeTableId = 0;
            foreach (var t in countK1)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    timeTableId = t.Key;
                }
            }
            this.K1TimeTalbe = timeTableId;

            max = 0;
            timeTableId = 0;
            foreach (var t in countK2)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    timeTableId = t.Key;
                }
            }
            this.K2TimeTalbe = timeTableId;

            max = 0;
            timeTableId = 0;
            foreach (var t in countK3)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    timeTableId = t.Key;
                }
            }
            this.K3TimeTalbe = timeTableId;

            max = 0;
            timeTableId = 0;
            foreach (var t in countK4)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    timeTableId = t.Key;
                }
            }
            this.K4TimeTalbe = timeTableId;

            max = 0;
            timeTableId = 0;
            foreach (var t in countK5)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    timeTableId = t.Key;
                }
            }
            this.K5TimeTalbe = timeTableId;

            max = 0;
            timeTableId = 0;
            foreach (var t in countK6)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    timeTableId = t.Key;
                }
            }
            this.K6TimeTalbe = timeTableId;
        }



        private void UpdateTmlTimeTableInfo()
        {
            bool bolHasSpecial = false;
            foreach (var t in this.ChildTreeItems)
            {
                if (t.IsListTreeNodeGroup)
                {
                    if (t.IsThisGroupHasSpecialTermial) bolHasSpecial = true;
                }
                else
                {
                    if (t.IsThisTmlSpecialTerminal) bolHasSpecial = true;
                }
            }
            this.IsThisGroupHasSpecialTermial = bolHasSpecial;
        }
    }
}
