using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Wlst.Cr.CoreOne.Services
{
    public class OtherSvr
    {
        public static void ChangeDatePickerToToday(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var spsr = e.OriginalSource.ToString();
            if (spsr.Contains("TextBoxView") == false) return;

            var sd = sender as DatePicker;
            if (sd == null) return;
            sd.SelectedDate = DateTime.Now;
        }
    }
}
