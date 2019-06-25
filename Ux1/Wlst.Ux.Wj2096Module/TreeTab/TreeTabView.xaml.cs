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

namespace Wlst.Ux.Wj2096Module.TreeTab
{
  

    /// <summary>
    /// TreeTabView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Services.ViewIdAssign.TreeTabVeiwId, AttachRegion = Services.ViewIdAssign.TreeTabVeiwAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeTabView : UserControl
    {
        public TreeTabView()
        {
            InitializeComponent();
            _mySelf = this;
            //LoadXml();


        }

        private static TreeTabView _mySelf;
        public static TreeTabView MySelf
        {
            get { return _mySelf; }
        }


        [Import]
        public IITreeTab Model
        {
            get
            {
                return DataContext as IITreeTab;
            }
            set
            {
                DataContext = value;
                //value.OnSelectedNodeByCode += new EventHandler<NodeSelectedArgs>(value_OnSelectedNodeByCode);
                //value.OnClearSerchTest += new EventHandler<NodeSelectedArgs>(value_OnClearSerchTest);

            }
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
            //var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            //if (item != null)
            //{
            //    var tmp = item.DataContext as GrpComSingleMuliViewModel.TreeNodeBaseNode;
            //    if (tmp != null) tmp.OnDoubleClick();
            //}
        }

        private void SearchTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (Model != null) foreach (var f in Model.ChildItems) f.IsExpanded = false;

            //// var grx = tvProperties as Telerik.Windows.Controls.RadTreeView;

            //var fgx = FindFirstVisualChild<RadTreeViewItem>(tvProperties);
            //foreach (var fg in fgx) fg.IsExpanded = false;

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
                    rtn.Add((T)child);
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

            //Model.SearchText = sr;

            //var tsts = e.Text;
            //if(String .IsNullOrEmpty( tsts )==false )
            //{
            //    ins.Add(tsts);
            //}

            // Model .SearchText=
        }
        //void value_OnClearSerchTest(object sender, NodeSelectedArgs e)
        //{
        //    //throw new NotImplementedException();
        //    sr = e.SearchText;
        //}
        //private int SearchLimit = 0;

        //private void LoadXml()
        //{
        //    var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("TabTreeSetConfg");
        //    if (infos.ContainsKey("SearchLimit"))
        //    {
        //        SearchLimit = Convert.ToInt32(infos["SearchLimit"]);
        //    }
        //    else SearchLimit = 0;

        //    QueryKeywords.Height = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 1) == true ? 31 : 0;

        //}

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //if (SearchLimit != 1) return;
                //Model.SearchNodeold(Model.SearchText);
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
