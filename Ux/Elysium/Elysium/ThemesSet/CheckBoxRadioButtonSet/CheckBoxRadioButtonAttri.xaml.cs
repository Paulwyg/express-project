using System;
using System.Collections.Generic;
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

namespace Elysium.ThemesSet.CheckBoxRadioButtonSet
{
    /// <summary>
    /// CheckBoxRadioButtonAttri.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxRadioButtonAttri : UserControl
    {
        public CheckBoxRadioButtonAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取显示按钮
        /// </summary>
        public CheckBox ShowCheckBox
        {
            get { return checkBoxshow; }
        }

        /// <summary>
        /// 获取显示按钮
        /// </summary>
        public RadioButton ShowRadioButton
        {
            get { return radioButtonshow; }
        }
    }
}
