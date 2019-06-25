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
using Wlst.Ux.Setting.Services;

namespace Wlst.Ux.Setting.SettingViewNew
{
    /// <summary>
    /// SettingVewNew.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.SettingViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SettingVewNew : UserControl
    {
        public SettingVewNew()
        {
            InitializeComponent();

            vm= new SettingViewNewModel();

            vm.ShowView = OptionView;
            DataContext = vm;

        }

        private SettingViewNewModel vm = null;


        //[Import]
        //public IISettingViewModel Model
        //{

        //    get { return DataContext as IISettingViewModel; }
        //    set { DataContext = value; }
        //}
    }
}
