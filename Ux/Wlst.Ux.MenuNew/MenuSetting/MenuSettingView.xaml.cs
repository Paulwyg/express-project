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
using Wlst.Ux.MenuNew.MenuSetting.Services;
using Wlst.Ux.MenuNew.Services;

namespace Wlst.Ux.MenuNew.MenuSetting
{
    /// <summary>
    /// MenuSettingView.xaml 的交互逻辑
    /// </summary>
    
    [ViewExport(ViewIdAssign.MenuSettingId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuSettingView : UserControl
    {
        public MenuSettingView()
        {
            InitializeComponent();
        }

        [Import]
        public IINewDataSetting Model
        {

            get { return DataContext as IINewDataSetting; }
            set { DataContext = value; }
        }
    }
}
