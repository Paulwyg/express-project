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
    /// SelectTmlList.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTmlList : CustomChromeWindow
    {
        public SelectTmlList()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "";
        }

        private ObservableCollection<RtuAmpSxxViewModel.TmlItems> dt;
        public void SetContext(ObservableCollection<RtuAmpSxxViewModel.TmlItems> oit)
        {
            dt = oit;
            DataContext = oit;
            RTLV.ItemsSource = oit;
        }

        public event EventHandler<EventArgsSelectTmlList> OnFormBtnOkClick;

        public class EventArgsSelectTmlList : EventArgs
        {
            public ObservableCollection<RtuAmpSxxViewModel.TmlItems> SelectTmlListInfo;

            public EventArgsSelectTmlList(ObservableCollection<RtuAmpSxxViewModel.TmlItems> info)
            {
                SelectTmlListInfo = info;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsSelectTmlList(dt));
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var zc = dt;
            foreach (var t in zc)
            {
                t.IsSelect = false;
                foreach (var tt in t.Child)
                {
                    tt.IsSelect = false;
                }
            }

            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsSelectTmlList(zc));
            }
            this.Close();
        }
    }
}
