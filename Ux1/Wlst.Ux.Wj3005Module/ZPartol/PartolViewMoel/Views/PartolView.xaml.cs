using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.WJ3005Module.Services;
using Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel.Services;

namespace Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel.Views
{
    /// <summary>
    /// PartolView.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// PatrolView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign .PartolViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class PartolView : UserControl
    {
        public PartolView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WindowsLoaded);
        }

        [Import]
        public IIPartolViewModel Model
        {
            get { return DataContext as IIPartolViewModel; }
            set { DataContext = value; }
        }

        private void RadGridView1_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.SaveDisplayIndex(RadGridView1.Columns, XmlConfigName + ".RadGridView1");
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel";

        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.LoadDisplayIndex(RadGridView1.Columns, XmlConfigName + ".RadGridView1");
        }


    }
}
