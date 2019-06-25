//using System.Windows;
//using Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel;
//using Wlst.Ux.EquipemntTree.Models;
//using Wlst.Ux.EquipemntTree.Resources;

//namespace Wlst.Ux.EquipemntTree.GrpSingleTabShowViewModel.ViewModels
//{
//    public class TreeNodeItemSingleSpecialViewModel : TreeNodeBaseNode
//    {

       

//        public TreeNodeItemSingleSpecialViewModel(string grpName)
//        {
//            this.NodeType = TypeOfTabTreeNode.IsGrpSpecial;
//            //Visi = Visibility.Visible;
//            this._father = null;
//            //TreeSingleViewModel.RegisterNodeToControl(this);

//            this.NodeName = grpName ;
//            this.ImagesIcon = ImageResources.GroupIcon;
//            this.NodeId = 0;
//        }

//        public override void OnNodeSelectActive()
//        {
//            base.OnNodeSelectActive();
//            TreeNodeItemSingleGroupViewModel.CurrentSelectGroupNode = this;
//        }
//    }


//    public class TreeNodeItemSingleAllViewModel : TreeNodeBaseNode
//    {
//        public TreeNodeItemSingleAllViewModel(string grpName)
//        {
//            this.NodeType = TypeOfTabTreeNode.IsAll ;
//            //Visi = Visibility.Visible;
//            this._father = null;
//            //TreeSingleViewModel.RegisterNodeToControl(this);

//            this.NodeName = grpName;
//            this.ImagesIcon = ImageResources.GroupIcon;
//            this.NodeId = 0;
//        }

//        public override void OnNodeSelectActive()
//        {
//            base.OnNodeSelectActive();
//            TreeNodeItemSingleGroupViewModel.CurrentSelectGroupNode = this;
//        }
//    }
//}
