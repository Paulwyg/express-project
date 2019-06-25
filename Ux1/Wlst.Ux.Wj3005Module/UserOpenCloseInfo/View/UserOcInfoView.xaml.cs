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
using Wlst.Ux.WJ3005Module.UserOpenCloseInfo.Services;

namespace Wlst.Ux.WJ3005Module.UserOpenCloseInfo.View
{
    /// <summary>
    /// UserOcInfoView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.UserOcInfoViewId,
    AttachNow = true,
    AttachRegion = WJ3005Module.Services.ViewIdAssign.NewSvrUserOcInfoViewAttachRegion
    )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserOcInfoView : UserControl
    {
        public UserOcInfoView()
        {
            InitializeComponent();
        }
        [Import]
        public IIUserOcInfo Model
        {
            get { return DataContext as IIUserOcInfo; }
            set { DataContext = value; }
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (e.RightButton == MouseButtonState.Pressed)

            //    Model.Records.Clear();
            if (e.LeftButton == MouseButtonState.Pressed)
                Model.CurrentSelectItemDoubleClicked();
        }
    }
}
