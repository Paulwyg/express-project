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

namespace Wlst.Ux.EquipemntLightFault.RtuAmpSxxNewRuleSection
{
    /// <summary>
    /// NewRuleSection.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.NewRuleSectionViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewRuleSection : UserControl
    {
        public NewRuleSection()
        {
            InitializeComponent();
        }

        [Import]
        public IINewRuleSectionvm Model
        {
            get { return DataContext as IINewRuleSectionvm; }
            set { DataContext = value; }
        }
    }

  
}
