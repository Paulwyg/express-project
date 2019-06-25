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
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;
using WindowForWlst;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Ux.Statistics.UxStatistics.ViewModel;

namespace Wlst.Ux.Statistics.UxStatistics.Views
{
    /// <summary>
    /// PieView.xaml 的交互逻辑
    /// </summary>
    public partial class PieView : CustomChromeWindow
    {

        public PieView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "数据统计";
        }


        public void SetContext(StatisticsViewModel x, string name)
        {
            DataContext = x;
            //this.PieData = x;
        }

        private void ChartSelectionBehavior_SelectionChanged(object sender, ChartSelectionChangedEventArgs e)
        {

            DataPoint tmppp = new PieDataPoint();
            foreach (DataPoint info in e.AddedPoints)
            {
                tmppp = info;

            }
            if (tmppp == null) return;


            var abc = tmppp.DataItem as Data;//.Label.ToString() ;
            if (abc == null) return;
            //tb3.Text = abc.Category + " " + abc.Value;

            var lst = new object[3];
            lst[0] = abc.Date;
            lst[1] = abc.IsRtu ;
            lst[2] = abc.Category;

            //RegionManage.ShowViewByIdAttachRegionWithArgu(Wlst.Ux.About.Services.ViewIdAssign.UxThreeLvViewId, lst);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Wlst.print.Prints.PrintVisi(this.PPieView,true);

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //Wlst.print.Prints.Print(this.PPieView);
        }
    }
}
