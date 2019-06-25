using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.CrissCrossEquipemntTree.GrpSingleTabShowViewModel.Services;
using Wlst.Ux.CrissCrossEquipemntTree.Models;

namespace Wlst.Ux.CrissCrossEquipemntTree.Views
{
    /// <summary>
    /// GrpSingleTabShowView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( CrissCrossEquipemntTree.Services.ViewIdAssign.GrpSingleTabShowViewId, AttachRegion = CrissCrossEquipemntTree.Services.ViewIdAssign.GrpSingleTabShowViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class GrpSingleTabShowView : UserControl
    {
        public GrpSingleTabShowView()
        {
            InitializeComponent();
            _mySelf = this;
            LoadXml();
            tvProperties.IsVirtualizing =
                !Wlst.Ux.CrissCrossEquipemntTree.SettingViewModel.Services.SettingExtend.Myself.IsShowTheSelectdNodeInTree;
            

        }

        private static GrpSingleTabShowView _mySelf;
        public static GrpSingleTabShowView MySelf
        {
            get { return _mySelf; }
        }


        [Import]
        public IISingleTree Model
        {
            get
            {
                return DataContext as IISingleTree;
            }
            set
            {
                DataContext = value;
                value.OnSelectedNodeByCode += new EventHandler<NodeSelectedArgs>(value_OnSelectedNodeByCode);
                value.OnClearSerchTest += new EventHandler<NodeSelectedArgs>(value_OnClearSerchTest);

            }
        }

        void value_OnSelectedNodeByCode(object sender, NodeSelectedArgs e)
        {
            if (tvProperties.IsVirtualizing  ) return;
            //throw new NotImplementedException();
            var rtuid = e.RtuIdSelected;
            if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(rtuid)) return;
            var itemsControlOne = tvProperties as  ItemsControl;
            if (itemsControlOne == null) return;

            foreach (var g in itemsControlOne.Items )
            {
                var tmps = g as Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel.TreeNodeBaseNode;
              //  var tvssssii = (RadTreeView)tvProperties.ItemContainerGenerator.ContainerFromItem(g);
                var tvifsdfsdfi = (RadTreeViewItem)tvProperties.ItemContainerGenerator.ContainerFromItem(g);
                

                if (tmps == null || tvifsdfsdfi==null ) continue;
                if (tmps.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    tvifsdfsdfi.IsSelected = true;
                    tvifsdfsdfi.IsExpanded = true;
                    //tvifsdfsdfi.Focus();
                   // tvifsdfsdfi.MoveFocus()
                    if (tvifsdfsdfi.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                    {
                        tvifsdfsdfi.UpdateLayout();
                    }
                    //var tvi = (ItemsControl)tvProperties.ItemContainerGenerator.ContainerFromItem(g);
                   
                    //if (tvi == null) continue;
                    foreach (var hhh in tvifsdfsdfi.Items)
                    {
                        var rtutmps = hhh as Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel.TreeNodeBaseNode;
                        if (rtutmps == null) continue;
                        if(rtutmps .NodeId ==rtuid )
                        {
                            //var ssss = FindVisualChild(tvifsdfsdfi,rtuid );
                            //if (ssss != null)
                            //{
                            //    var gggsd = ssss.DataContext  as Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel.TreeNodeBaseNode;
                            //    if(gggsd ==null )
                            //    {
                            //        return;
                            //    }
                            //    ssss.IsSelected = true;
                            //    ssss.Focus();
                            //    ssss.IsExpanded = true;
                            //    return;
                            //}
                            var tvii = (RadTreeViewItem)tvifsdfsdfi.ItemContainerGenerator.ContainerFromItem(hhh);
                         //   var tvisssi = tvProperties.ItemContainerGenerator.ContainerFromItem(hhh);
                            if (tvii == null ) return; 
                            //tvii.IsSelected = true;
                            //tvii.IsExpanded = true; 
                           
                            //tvii.Focus();
                            var tmlVm = hhh as Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel.TreeNodeItemTmlViewModel ;
                            if (tmlVm != null) tmlVm.IsSelectedByCode = true;
                            

                            tvProperties.SelectedItem = tvii;
                            tvii.EnsureVisible();
                            tvii.Focus();
                            tvii.IsSelected = true;
                            tvii.IsExpanded = true;

                            if (tmlVm != null) tmlVm.IsSelectedByCode = false ;

                            //tvii.IsSelected = true;
                            //tvii.IsExpanded = true;

                         //   tvii.Focus();


                            // Creating a FocusNavigationDirection object and setting it to a 
                            // local field that contains the direction selected.
                            //FocusNavigationDirection focusDirection = FocusNavigationDirection.Up ;

                            // MoveFocus takes a TraveralReqest as its argument.
                            //TraversalRequest request = new TraversalRequest(focusDirection);
                            //tvii.MoveFocus(request);

                            //// Gets the element with keyboard focus.
                            //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                            //// Change keyboard focus. 
                            //if (elementWithFocus != null)
                            //{
                            //    elementWithFocus.MoveFocus(request);
                            //}

                            return ;
                        }
                    }
                    //foreach (var mmm in )

                    break;
                }
            }




        }


        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <param name="id"> </param>
        /// <returns></returns>
        private RadTreeViewItem  FindVisualChild(Visual visual,int id) 
        {

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {

                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);

                if (child != null)
                {

                    RadTreeViewItem correctlyTyped = child as RadTreeViewItem;

                    if (correctlyTyped != null)
                    {
                        var gggsd =
                            correctlyTyped.DataContext as
                            Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel.TreeNodeBaseNode;
                        if (gggsd == null) continue;
                        if (gggsd.NodeId == id)
                            return correctlyTyped;

                    }



                    RadTreeViewItem descendent = FindVisualChild(child,id );

                    if (descendent != null)
                    {

                        var gggsd =
                       descendent.DataContext as
                       Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel.TreeNodeBaseNode;
                        if (gggsd == null) continue;
                        if (gggsd.NodeId == id)
                            return correctlyTyped;

                    }

                }

            }



            return null;

        }



        public void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
        }
        private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
            
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
           
        }

        private T GetTemplatedAncestor<T>(FrameworkElement element) where T : FrameworkElement
        {
            if (element is T)
            {
                return element as T;
            }

            var templatedParent = element.TemplatedParent as FrameworkElement;
            return templatedParent != null ? GetTemplatedAncestor<T>(templatedParent) : null;
        }

        private void tvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                var tmp = item.DataContext as GrpComSingleMuliViewModel.TreeNodeBaseNode;
                if(tmp !=null )tmp .OnDoubleClick();
            }
        }

        private void SearchTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Model != null) foreach (var f in Model.ChildTreeItems) f.IsExpanded = false;

           // var grx = tvProperties as Telerik.Windows.Controls.RadTreeView;

            var fgx = FindFirstVisualChild<RadTreeViewItem>(tvProperties);
            foreach  (var fg in fgx) fg.IsExpanded = false;
      
        }




        public List<T> FindFirstVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(obj);
            var rtn = new List<T>();
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    rtn.Add((T) child);
                }
                else
                {
                    var childOfChild = FindFirstVisualChild<T>(child);
                    if (childOfChild.Count > 0)
                    {
                        rtn.AddRange(childOfChild);
                    }
                }
            }
            return rtn;
        }

        private string sr = "";

        private void tvProperties_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Equals("\b"))
            {
                if (sr.Length > 0)
                {
                    sr = sr.Substring(0, sr.Length - 1);
                }
            }
            else
            {
                sr += e.Text;
            }

            Model.SearchText = sr;

            //var tsts = e.Text;
            //if(String .IsNullOrEmpty( tsts )==false )
            //{
            //    ins.Add(tsts);
            //}

            // Model .SearchText=
        }
        void value_OnClearSerchTest(object sender, NodeSelectedArgs e)
        {
            //throw new NotImplementedException();
            sr = e.SearchText;
        }
        private int SearchLimit = 0;
        
        private void LoadXml()
        {
            var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("LdTabTreeSetConfg");
            if (infos.ContainsKey("SearchLimit"))
            {
                SearchLimit = Convert.ToInt32(infos["SearchLimit"]);
            }
            else SearchLimit = 0;

            QueryKeywords.Height = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 1) == true ? 31 : 0;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchLimit != 1) return;
                Model.SearchNodeold(Model.SearchText);
            }
        }


        //      public T FindFirstVisualChild<T>(DependencyObject obj, string childName) where T : DependencyObject 
  //      { 
  //          for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) 
  //          { 
  //              DependencyObject child = VisualTreeHelper.GetChild(obj, i); 
  //              if (child != null && child is T && child.GetValue(NameProperty).ToString() == childName) 
  //             { 
  //                 return (T)child; 
  //             } 
  //             else 
  //           { 
  //                  T childOfChild = FindFirstVisualChild<T>(child, childName); 
  //                  if (childOfChild != null) 
  //                { 
  //                    return childOfChild; 
  //                  } 
  //             } 
  //          } 
  //          return null; 
  //} 

    }

 
}
