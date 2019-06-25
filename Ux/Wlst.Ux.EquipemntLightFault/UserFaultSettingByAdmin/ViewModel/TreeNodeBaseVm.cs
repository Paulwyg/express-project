using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.ViewModel
{
    public class TreeNodeBaseVm : TreeNodeBaseViewModel
    {

        private ObservableCollection<TreeNodeBaseViewModel> _items;

        public ObservableCollection<TreeNodeBaseViewModel> ChildTreeItems
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBaseViewModel>()); }
        }

        private bool _isChiledSelected = true;

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                base.IsSelected = value;
                if (_isChiledSelected)
                {
                    if (NodeId > 0)
                        foreach (var f in ChildTreeItems) f.IsSelected = value;
                }
            }
        }


        public void SetIsselectWithOutChiledSelected(bool selected)
        {
            _isChiledSelected = false;
            IsSelected = selected;
            _isChiledSelected = true;
        }
    }
}
