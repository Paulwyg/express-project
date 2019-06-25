using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel
{
    [Export(typeof (IITreeNodeLoadExport))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ExportTreeNodex : IITreeNodeLoadExport
    {
        private List<int> _rtumodels = null;

        public List<int> RtuModes
        {
            get
            {
                if (_rtumodels == null)
                {
                    _rtumodels = new List<int>();
                    _rtumodels.Add(1090);
                    _rtumodels.Add(30910);
                    _rtumodels.Add(30920);
                }
                return _rtumodels;

            }
        }

        public ObservableCollection<TreeNodeBaseViewModel> GetTreeNodeInfo(int rtuId)
        {
            var ntg = LoadInfo(rtuId);  
            var rtn = new ObservableCollection<TreeNodeBaseViewModel>();
            foreach (var f in ntg) rtn.Add(f);
            return rtn;
        }





        public ObservableCollection<TreeNodeLineViewModel> LoadInfo(int nodeId)  
        {
            var childTreeItems = new ObservableCollection<TreeNodeLineViewModel>();
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeId);//.GetEquipmentInfo(nodeId);
            if (info == null) return childTreeItems;
            var lines = info as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return childTreeItems;
            if (lines.WjLduLines == null) return childTreeItems;

            var errors =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(nodeId);

            var lineerrs = new List<Tuple<int, int>>();
            foreach (var infogggg in errors)
            {
                lineerrs.Add(new Tuple<int, int>(infogggg.FaultId, infogggg.FaultId)); //41 被盗 42 短路
                // }
            }

            foreach (var t in lines.WjLduLines.Values)
            {

                var str = "";
                var strForeGround = "Black";
                if (t.IsUsed)
                {
                    strForeGround = "Black";
                    if (lineerrs.Contains(new Tuple<int, int>(t.LduLineId, 42)))
                    {
                        str = " -短路";
                        strForeGround = "Red";
                    }
                    if (lineerrs.Contains(new Tuple<int, int>(t.LduLineId, 41)))
                    {
                        str = " -被盗";
                        strForeGround = "Red";
                    }
                }
                else
                {
                    str = " -[未启用]";
                }

                var infos = new TreeNodeLineViewModel(t.LduLineName + str, t.LduLineId, t.IsUsed, nodeId);
                infos.ForeGround = strForeGround;
                childTreeItems.Add(infos);
            }
            return childTreeItems;
        }
    }



}
