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

namespace Wlst.Ux.ExtendYixinEsu.TreeTabRtuSet
{
    /// <summary>
    /// TreeTabRtuSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ExtendYixinEsu.Services.ViewIdAssign.TreeTabRtuSetViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeTabRtuSetView : UserControl
    {
        public TreeTabRtuSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IITreeTabRtuSet Model
        {
            get { return DataContext as IITreeTabRtuSet; }
            set { DataContext = value; }
        }

    }

}
