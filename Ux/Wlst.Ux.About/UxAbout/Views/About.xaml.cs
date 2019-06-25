using System.ComponentModel.Composition;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.About.UxAbout.Services;

namespace Wlst.Ux.About.UxAbout.Views
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Wlst.Ux.About.Services.ViewIdAssign.UxAboutViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        [Import]
        public IIUxAboutModule Model
        {
            get { return DataContext as IIUxAboutModule; }
            set { DataContext = value; }
        }
    }
}
