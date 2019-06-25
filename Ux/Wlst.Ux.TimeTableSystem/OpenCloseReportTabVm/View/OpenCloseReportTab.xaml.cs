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
using Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.Services;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.View
{


    //[ViewExport(TimeTableSystem.Services.ViewIdAssign.OpenCloseReportTabViewId,AttachNow = true, 
    //    AttachRegion = TimeTableSystem.Services.ViewIdAssign.OpenCloseReportTabViewAttachRegion)]
    //[PartCreationPolicy(CreationPolicy.Shared)]  暂停显示 感觉无意义
    public partial class OpenCloseReportTab : UserControl
    {
        public OpenCloseReportTab()
        {
            InitializeComponent();


        }



        [Import]
        public IIOpenCloseReportTabVm Model
        {
            get { return DataContext as IIOpenCloseReportTabVm; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;
            if (but != null)
            {
                try
                {
                    int x = Convert.ToInt32(but.ToolTip);
                    Model.OnSelectChanged(x);
                }
                catch (Exception ex)
                {

                }
            }
        }


    }
}
