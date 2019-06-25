using System;
using System.Collections.Generic;
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
using Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.ViewModels;

namespace Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.Views
{
    /// <summary>
    /// SelectTimeTable.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTimeTable : CustomChromeWindow
    {
        public SelectTimeTable()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "选择时间表";
        }

        public event EventHandler<EventArgsSelectTimeTable> OnFormBtnOkClick;

        private OneItemTemporary _dt;
        public void SetContext(OneItemTemporary oit)
        {
            _dt = oit;           
            DataContext = oit;

        }


        //是否关闭界面
        bool close = false;
        bool close1 = false;


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            close1 = true;
            if (close1)
            {
                close = true;
            }
            if (close)
            {

                if (OnFormBtnOkClick != null)
                {
                    OnFormBtnOkClick(this, new EventArgsSelectTimeTable(_dt));
                }
                this.Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsSelectTimeTable(null));
            }
            this.Close();
        }

        public class EventArgsSelectTimeTable : EventArgs
        {
            public OneItemTemporary SelectTimeTable;

            public EventArgsSelectTimeTable(OneItemTemporary info)
            {
                SelectTimeTable = info;
            }
        }
    }
}
