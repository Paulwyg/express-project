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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView
{
    /// <summary>
    /// SetupPara.xaml 的交互逻辑
    /// </summary>
    public partial class SetupPara : CustomChromeWindow
    {
        public SetupPara()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "参数批量设置";
        }

        private TimeTableInfomationItem dt;

        private ObservableCollection<TimeTableInfomationItem> _dtAll =
            new ObservableCollection<TimeTableInfomationItem>();
        private int areaid;
        private bool flag = false;

        public void SetContext(ObservableCollection<TimeTableInfomationItem> oit, int area, int tableid)
        {
            _dtAll = oit;
            dt = oit[0];
            DataContext = oit;
            areaid = area;
            dt.IsEdit = true;
            if (dt.CurrentSelectLux != null) dt.ShowCurrentSelectLux2 = 22;
        }

        public event EventHandler<EventArgsAddTimeTable> OnFormBtnOkClick;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var t in _dtAll)
            {
                t.IsEdit = true;

                t.LuxOffValue = dt.LuxOffValue;
                t.LuxOnValue = dt.LuxOnValue;
                t.LightOnOffset = dt.LightOnOffset;
                t.LightOffOffset = dt.LightOffOffset;
                t.LuxEffective = dt.LuxEffective;

                if (OnFormBtnOkClick != null)
                {
                    OnFormBtnOkClick(this, new EventArgsAddTimeTable(t));
                }
                this.Close();

                t.IsEdit = false;
            }

            



        }



    }


}
