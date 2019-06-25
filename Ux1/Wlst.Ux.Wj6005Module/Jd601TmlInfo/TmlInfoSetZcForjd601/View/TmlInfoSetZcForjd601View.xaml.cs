using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlInfoSetZcForjd601.Services;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlInfoSetZcForjd601.View
{
    /// <summary>
    /// TmlInfoSetZcForjd601View.xaml 的交互逻辑
    /// </summary>
    /// 
    [ViewExport( Ux.Wj6005Module.Services.ViewIdAssign.TmlInfoSetZcForjd601ViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInfoSetZcForjd601View : UserControl
    {
        public TmlInfoSetZcForjd601View()
        {
            InitializeComponent();
        }
        [Import]
        public IITmlInfoSetZcForjd601 Model
        {
            get { return DataContext as IITmlInfoSetZcForjd601; }
            set { DataContext = value; }
        }
    }
}
