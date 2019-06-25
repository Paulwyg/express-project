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
using Telerik.Windows.Controls.GridView;
using WindowForWlst;
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Views
{
    /// <summary>
    /// SelectTerminal.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTerminal : CustomChromeWindow
    {
        public SelectTerminal()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "隧道方案终端选择";
        }

        public event EventHandler<EventArgsSelectTerminal> OnFormBtnOkClick;

        private TunnelInformation _dt;
        public void SetContext(TunnelInformation oit)
        {
            _dt = oit;
            DataContext = oit;
        }

        //是否关闭界面
        bool close = false;
        bool close1 = false;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            close1 = true;
            if(close1)
            {
                close = true;
            }
            if (close)
            {

                if (OnFormBtnOkClick != null)
                {
                    OnFormBtnOkClick(this, new EventArgsSelectTerminal(_dt));
                }
                this.Close();
            }
        }

        public class EventArgsSelectTerminal : EventArgs
        {
            public TunnelInformation SelectTerminalInfo;

            public EventArgsSelectTerminal(TunnelInformation info)
            {
                SelectTerminalInfo = info;
            }
        }
    }
}
