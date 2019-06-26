using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Menu.Services;

namespace Wlst.Ux.Menu.Views
{
    /// <summary>
    /// MenuView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(  Services .ViewIdAssign .MenuViewId ,  Services .ViewIdAssign .MenuViewAttachRegion,true  )]
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