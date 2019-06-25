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

namespace Wlst.Ux.StateBarModule.NewMsgViewBottom
{
    /// <summary>
    /// NewMsgViewBottomView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(StateBarModule.Services.ViewIdAssign.NewMsgViewBottomViewId,
    AttachNow = true,
    AttachRegion = "NewMsgRegionr"
    )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewMsgViewBottomView : UserControl
    {
        public NewMsgViewBottomView()
        {
            InitializeComponent();
        }


        [Import]
        public NewMsgViewBottomVm Model
        {
            get { return DataContext as NewMsgViewBottomVm; }
            set { DataContext = value; }
        }

        private DateTime dt = DateTime.Now;
        private int count = 0;
        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now.Ticks - dt.Ticks < 10000000) count++;
            else count = 0;
            dt = DateTime.Now;
            if(count >5)
            {
                //tb.Width = 0;
            }
        }



    }
}
