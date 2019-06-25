using System.Windows;
using Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.vms
{
    public class TreeNodeItemSingleSpecialViewModel : TreeNodeBaseNode
    {

       

        public TreeNodeItemSingleSpecialViewModel(string grpName)
        {
            this.NodeType = TypeOfTabTreeNode.IsGrpSpecial;
            //Visi = Visibility.Visible;
            this._father = null;
            //TreeSingleViewModel.RegisterNodeToControl(this);

            this.NodeName = grpName ;
            this.ImagesIcon = ImageResources.GroupIcon;
            this.NodeId = 0;
        }

        public override void OnNodeSelectActive()
        {
            base.OnNodeSelectActive();
            TreeNodeItemSingleGroupViewModel.CurrentSelectGroupNode = this;
        }
    }


    public class TreeNodeItemSingleAllViewModel : TreeNodeBaseNode
    {
        public TreeNodeItemSingleAllViewModel(string grpName)
        {
            this.NodeType = TypeOfTabTreeNode.IsAll ;
            //Visi = Visibility.Visible;
            this._father = null;
            //TreeSingleViewModel.RegisterNodeToControl(this);

            this.NodeName = grpName;
            this.ImagesIcon = ImageResources.GroupIcon;
            this.NodeId = 0;
        }

        public override void OnNodeSelectActive()
        {
            base.OnNodeSelectActive();
            TreeNodeItemSingleGroupViewModel.CurrentSelectGroupNode = this;
        }
    }
}
