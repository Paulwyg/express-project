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
using Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightQuery.Model;
using Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightQuery.Services;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightQuery.Views
{
    /// <summary>
    /// RtuOpenCloseLightQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.RtuOpenCloseLightQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuOpenCloseLightQueryView : UserControl
    {
        public RtuOpenCloseLightQueryView()
        {
            InitializeComponent();
        }


        [Import]
        public IIRtuOpenCloseLightQuery Model
        {
            get { return DataContext as IIRtuOpenCloseLightQuery; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(SunRasieTable);
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(executerecord);

            }
            catch (Exception ex)
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rd1.Visibility == Visibility.Visible)
                {
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rd1);
                    return;
                }
                if (rd2.Visibility == Visibility.Visible)
                {
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rd2);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<int> SumYears=new List<int>();
            int Sum = 0;
            int SumDay = 0;


            for (DateTime day = new DateTime(DateTime.Now.Year, 1, 1); day <= new DateTime(DateTime.Now.Year, 12, 31); day =day.AddDays(1))
            {

                var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(day.Month, day.Day);
                if (info != null)
                {
                    Sum = Sum + info.time_sunrise + (1440 - info.time_sunset);
                    SumDay = SumDay + 1;
                }
            }

            int Sum366 = 0;
            var day229 = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(2, 29);
            int Sum365 = 0;

            string year366 = "";
            string year365 = "";

            if (SumDay == 365)
            {
                Sum366 = Sum + (day229.time_sunrise + (1440 - day229.time_sunset));
                Sum365 = Sum;
                year366 = "如果";
                year365 = "今年";
            }
            else if(SumDay == 366)
            {
                Sum365 = Sum - (day229.time_sunrise + (1440 - day229.time_sunset));
                Sum366 = Sum;
                year365 = "如果";
                year366 = "今年";
            }

            string strSum366 = (Sum366 / 60).ToString("D2") + "小时" + (Sum366 % 60).ToString("D2") + "分钟";
            string strSum365 = (Sum365 / 60).ToString("D2") + "小时" + (Sum365 % 60).ToString("D2") + "分钟";

            int Sum366to1 = Sum366 / 366;
            int Sum365to1 = Sum365 / 365;
            string strSum366to1 = (Sum366to1 / 60).ToString("D2") + "小时" + (Sum366to1 % 60).ToString("D2") + "分钟";
            string strSum365to1 = (Sum365to1 / 60).ToString("D2") + "小时" + (Sum365to1 % 60).ToString("D2") + "分钟";

            string strline = "全夜时间即日落时刻-日出时刻的时长" + "。 \n";

            string str = strline + year366 + "有366天，则总全夜时长为" + Sum366 + "分钟，即" + strSum366 + "；平均每天全夜时长为" + Sum366to1 + "分钟，即" + strSum366to1 + "。 \n" +
                         year365 + "有365天，则总全夜时长为" + Sum365 + "分钟，即" + strSum365 + "；平均每天全夜时长为" + Sum365to1 + "分钟，即" + strSum365to1 + "。";

            if (year365 == "今年") str = strline + year365 + "有365天，则总全夜时长为" + Sum365 + "分钟，即" + strSum365 + "；平均每天全夜时长为" + Sum365to1 + "分钟，即" + strSum365to1 + "。 \n" +
                                         year366 + "有366天，则总全夜时长为" + Sum366 + "分钟，即" + strSum366 + "；平均每天全夜时长为" + Sum366to1 + "分钟，即" + strSum366to1 + "。";

            WlstMessageBox.Show("统计全夜时长", str , WlstMessageBoxType.Ok);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(daymonthyear);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
