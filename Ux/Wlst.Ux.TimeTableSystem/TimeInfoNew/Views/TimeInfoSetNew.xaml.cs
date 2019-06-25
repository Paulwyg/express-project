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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView;

namespace Wlst.Ux.TimeTableSystem.TimeInfoNew.Views
{
    /// <summary>
    /// TimeInfoSetNew.xaml 的交互逻辑
    /// </summary>
    public partial class TimeInfoSetNew : UserControl
    {
        public TimeInfoSetNew()
        {
            InitializeComponent();
        }

        //[Import]
        //public IITimeInfoMn Model
        //{
        //    get { return DataContext as IITimeInfoMn; }
        //    set
        //    {
        //        DataContext = value;
        //    }
        //}
    }
}
