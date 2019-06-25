using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WindowForWlst;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.Views
{
    /// <summary>
    /// WatchTmlList.xaml 的交互逻辑
    /// </summary>
    public partial class WatchTmlList : CustomChromeWindow
    {
        public WatchTmlList()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private ObservableCollection<RtuAmpSxxViewModel.WatchItems> dt;

        public void SetContext(ObservableCollection<RtuAmpSxxViewModel.WatchItems> oit)
        {
            dt = oit;
            DataContext = oit;
            RTLV.ItemsSource = oit;
        }

        public event EventHandler<WatchTmlList.EventArgsSelectTmlList> OnFormBtnOkClick;

        public class EventArgsSelectTmlList : EventArgs
        {
            public ObservableCollection<RtuAmpSxxViewModel.WatchItems> WatchTmlListInfo;

            public EventArgsSelectTmlList(ObservableCollection<RtuAmpSxxViewModel.WatchItems> info)
            {
                WatchTmlListInfo = info;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("回路");
                lsttitle.Add("回路名称");
                lsttitle.Add("平均电流");

                var lstobj = new List<List<object>>();

                foreach (var g in dt)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Id);
                    tmp.Add(g.Name);
                    tmp.Add(g.LoopId);
                    tmp.Add(g.LoopName);
                    tmp.Add(g.AverageA);

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }
        }

    }
}
