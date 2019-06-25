using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Menu.Services;

namespace Wlst.Ux.Menu.Views
{
    /// <summary>
    /// MenuView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = true, ID = Services .ViewIdAssign .MenuViewId ,
        AttachRegion = Services .ViewIdAssign .MenuViewAttachRegion )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        [Import]
        public IIMenuViewModule Model
        {
            get { return DataContext as IIMenuViewModule; }
            set { DataContext = value; }
        }
    }
}