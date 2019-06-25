using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1080Module.Wj1080InfoSet.Services;

namespace Wlst.Ux.Wj1080Module.Wj1080InfoSet.Views
{
    /// <summary>
    /// Wj1080TmlInfoSetView.xaml 的交互逻辑  Wj1080ModuleWj1080TmlInfoSetView
    /// </summary>
    [ViewExport(Wj1080Module .Services .ViewIdAssign .Wj1080TmlInfoSetViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1080TmlInfoSetView : UserControl 
    {
        public Wj1080TmlInfoSetView()
        {
            InitializeComponent();
        }


        [Import]
        public IITmlInformationViewModel Model
        {
            get { return DataContext as IITmlInformationViewModel; }
            set { DataContext = value; }
        }

        private void textBlock11_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Model != null) Model.ShowInfo = "";
           
        }
    }
}
