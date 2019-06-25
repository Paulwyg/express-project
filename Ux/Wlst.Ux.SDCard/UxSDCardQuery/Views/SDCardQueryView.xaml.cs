using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.SDCard.UxSDCardQuery.Services;

namespace Wlst.Ux.SDCard.UxSDCardQuery.Views
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.SDCard.Services.ViewIdAssign.UxSDCardQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SDCardQuery : UserControl
    {
        public SDCardQuery()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(WindowsLoaded);
        }

        [Import]
        public IIUxSDCardQueryModule Model
        {
            get { return DataContext as IIUxSDCardQueryModule; }
            set { DataContext = value; }
        }

        private void exportgridview_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.SaveDisplayIndex(exportgridview.Columns, XmlConfigName + ".exportgridview");
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.WJ3005Module.ZDataQuery.DailyDataQuery";

        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.LoadDisplayIndex(exportgridview.Columns, XmlConfigName + ".exportgridview");
        }
    }
}
