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
using Wlst.Ux.Wj2096Module.TimeInfoSet.Services;
using WindowForWlst;

namespace Wlst.Ux.Wj2096Module.TimeInfoSet.Views
{
    /// <summary>
    /// SetYearTime.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2096Module.Services.ViewIdAssign.SetYearTimeViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SetYearTime : UserControl
    {
        public SetYearTime()
        {
            InitializeComponent();
        }

        [Import]
        private IISetYearTime Model
        {
            get { return DataContext as IISetYearTime; }
            set
            {
                DataContext = value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          //Application.Current.Shutdown();   
        }

    }
}
