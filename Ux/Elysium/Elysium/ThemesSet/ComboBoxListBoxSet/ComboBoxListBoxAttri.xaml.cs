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

namespace Elysium.ThemesSet.ComboBoxListBoxSet
{
    /// <summary>
    /// ComboBoxListBoxAttri.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxListBoxAttri : UserControl
    {
        public ComboBoxListBoxAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示效果按钮
        /// </summary>
        public ComboBox ShowComboBox
        {
            get { return comboBox1; }
        }

        /// <summary>
        /// 显示效果按钮
        /// </summary>
        public ListBox ShowListBox
        {
            get { return listBox1; }
        }
    }
}
