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
using Wlst.Ux.StateBarModule.UserOperateMsgView.Services;


namespace Wlst.Ux.StateBarModule.UserOperateMsgView.View
{

    /// <summary>
    /// UserOperateMsgView.xaml 的交互逻辑 EquipmentNewDataHoldingNewSvrMsgView
    /// </summary>
    [ViewExport(StateBarModule.Services.ViewIdAssign.UserOperateMsgViewId,
        AttachNow = true,
        AttachRegion = StateBarModule.Services.ViewIdAssign.NewSvrMsgViewAttachRegion
        )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserOperateMsgView : UserControl
    {
        public UserOperateMsgView()
        {
            InitializeComponent();
        }


        [Import]
        public IIUserOperatorRecords Model
        {
            get { return DataContext as IIUserOperatorRecords; }
            set { DataContext = value; }
        }



        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)

                Model.Records.Clear();
            else if (e.LeftButton == MouseButtonState.Pressed)
                Model.CurrentSelectItemDoubleClicked();
        }
    }
}
