using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.About.UxShowErr.Sevices;

namespace Wlst.Ux.About.UxShowErr.Views
{
    /// <summary>
    /// ShowErrView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.About.Services.ViewIdAssign.UxShowErrViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ShowErrView : UserControl
    {
        public ShowErrView()
        {
            InitializeComponent();
        }

        [Import]
        private IIUxShowErrModule Model
        {
            get { return DataContext as IIUxShowErrModule; }
            set
            {
                DataContext = value;
            }
        }
    }


}
