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

namespace Elysium.ThemesSet.DatePickerSet
{
    /// <summary>
    /// TreeViewAttri.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerAttri : UserControl
    {
        public DatePickerAttri()
        {
            InitializeComponent();
            datePicker1.SelectedDate = DateTime.Now;
        }
        public DatePicker ShowView
        {
            get { return datePicker1; }
        }
    }
}
