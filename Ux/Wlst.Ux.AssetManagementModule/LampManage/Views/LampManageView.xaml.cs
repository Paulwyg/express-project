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
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.AssetManagementModule.LampManage.Services;
using Wlst.Ux.AssetManagementModule.LampManage.ViewModel;
using Wlst.Ux.AssetManagementModule.Services;

namespace Wlst.Ux.AssetManagementModule.LampManage.Views
{
    /// <summary>
    /// LampManageView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.LampManageViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LampManageView : UserControl
    {
        public LampManageView()
        {
            InitializeComponent();
        }

        [Import]
        public IILampManage Model
        {
            get { return DataContext as IILampManage; }
            set { DataContext = value; }
        }

        private void comboBoxTextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbbName_LostFocus(object sender, RoutedEventArgs e)
        {
            string _textString = ((ComboBox) sender).Text;

            if (string.IsNullOrEmpty(_textString) == false)
            {
                LampManageViewModel.AddToBureauLstComboBox(_textString);

                for(int i = 0 ; i < ((ComboBox) sender).Items.Count ; i++)
                {
                    if (((NameValueInt)(((ComboBox)sender).Items[i])).Name == _textString)
                    {
                        ((ComboBox) sender).SelectedIndex = i;
                    }
                }
            }
        }
    }
}
