using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlParametersInfoSetForJd601.Services;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlParametersInfoSetForJd601.View
{
    /// <summary>
    /// TmlParmetersInfoSetForJd601View.xaml 的交互逻辑
    /// </summary>
    /// 
    [ViewExport(Ux.Wj6005Module.Services.ViewIdAssign.TmlParmetersInfoSetForJd601ViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlParametersInfoSetForJd601View : UserControl
    {
        public TmlParametersInfoSetForJd601View()
        {
            InitializeComponent();
        }
        [Import]
        public IITmlParametersInfoSetForJd601 Model
        {
            get { return DataContext as IITmlParametersInfoSetForJd601; }
            set { DataContext = value; }
        }
    }
}
