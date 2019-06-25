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
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.Services;

namespace Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.Views
{
    /// <summary>
    /// Wj1090paraInfoSet.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj9001Module.Services.ViewIdAssign.Wj9001ParaSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001ParaInfoSet : UserControl
    {
        public Wj9001ParaInfoSet()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj9001ParaInfoSet Model
        {
            get { return DataContext as IIWj9001ParaInfoSet; }
            set
            {
                DataContext = value;
                value.OnNavOnLoadSelectdRtus += new EventHandler(value_OnNavOnLoadSelectdRtus);
            }

        }

        void value_OnNavOnLoadSelectdRtus(object sender, EventArgs e)
        {
            //rtl.AutoExpandItems = true;
            //rtl.ExpandAllGroups();
            if (sender == null)
            {
                rtl.ExpandAllHierarchyItems();
            }
            else
            {
                rtl.CollapseAllHierarchyItems();
            }
            //throw new NotImplementedException();
        }

    }
}
