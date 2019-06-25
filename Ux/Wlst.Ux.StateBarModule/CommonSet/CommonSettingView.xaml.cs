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

namespace Wlst.Ux.StateBarModule.CommonSet
{
    /// <summary>
    /// CommonSettingView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( StateBarModule.Services.ViewIdAssign.CommonSettingViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CommonSettingView : UserControl
    {
        public CommonSettingView()
        {
            
            InitializeComponent();
            
        }



        [Import]
        public IICommonSetting Model
        {
            get { return DataContext as IICommonSetting; }
            set { DataContext = value; }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsOldUseTwoOpenLightSectionLabel.Visibility=Visibility.Hidden;
            IsOldUseTwoOpenLightSectionLabel.MaxWidth = 0;
            IsOldUseTwoOpenLightSection.Visibility = Visibility.Visible;
        }

        //private void IsLdlAs100Per_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    IsLdlAs100PerLabel.Visibility = Visibility.Hidden;
        //    IsLdlAs100PerLabel.MaxWidth = 0;
        //    IsLdlAs100Per.Visibility = Visibility.Visible;
        //}
    }
}
