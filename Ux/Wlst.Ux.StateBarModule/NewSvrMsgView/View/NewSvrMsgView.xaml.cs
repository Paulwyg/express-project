using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.StateBarModule.NewSvrMsgView.Services;

namespace Wlst.Ux.StateBarModule.NewSvrMsgView.View
{
    /// <summary>
    /// NewSvrMsgView.xaml 的交互逻辑 EquipmentNewDataHoldingNewSvrMsgView
    /// </summary>
    //[ViewExport( StateBarModule.Services.ViewIdAssign.NewSvrMsgViewId,
    //    AttachNow = true,
    //    AttachRegion = StateBarModule.Services.ViewIdAssign.NewSvrMsgViewAttachRegion
    //    )]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewSvrMsgView : UserControl
    {
        public NewSvrMsgView()
        {
            InitializeComponent();

         
        }

 

        [Import]
        public IIOperatorOnTimeRecords Model
        {
            get { return DataContext as IIOperatorOnTimeRecords; }
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
