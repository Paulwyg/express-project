using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WindowForWlst;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView
{
    /// <summary>
    /// RecordSunView.xaml 的交互逻辑
    /// </summary>
    public partial class RecordSunView : CustomChromeWindow
    {
        public RecordSunView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "全年时间查看";
        }

        public void SetContext(OpenOrClose oit)
        {
            DataContext = oit;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(SunRasieTable1);
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("日期");
                lsttitle.Add("1月");
                lsttitle.Add("2月");
                lsttitle.Add("3月");
                lsttitle.Add("4月");
                lsttitle.Add("5月");
                lsttitle.Add("6月");
                lsttitle.Add("7月");
                lsttitle.Add("8月");
                lsttitle.Add("9月");
                lsttitle.Add("10月");
                lsttitle.Add("11月");
                lsttitle.Add("12月");


                var lstobj = new List<List<object>>();

                foreach (var g in TimeInfoMnVm.PassrecordSun1)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Records[0].Name);
                    tmp.Add(g.Records[1].Name);
                    tmp.Add(g.Records[2].Name);
                    tmp.Add(g.Records[3].Name);
                    tmp.Add(g.Records[4].Name);
                    tmp.Add(g.Records[5].Name);
                    tmp.Add(g.Records[6].Name);
                    tmp.Add(g.Records[7].Name);
                    tmp.Add(g.Records[8].Name);
                    tmp.Add(g.Records[9].Name);
                    tmp.Add(g.Records[10].Name);
                    tmp.Add(g.Records[11].Name);
                    tmp.Add(g.Records[12].Name);



                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(SunRasieTable2);
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("日期");
                lsttitle.Add("1月");
                lsttitle.Add("2月");
                lsttitle.Add("3月");
                lsttitle.Add("4月");
                lsttitle.Add("5月");
                lsttitle.Add("6月");
                lsttitle.Add("7月");
                lsttitle.Add("8月");
                lsttitle.Add("9月");
                lsttitle.Add("10月");
                lsttitle.Add("11月");
                lsttitle.Add("12月");


                var lstobj = new List<List<object>>();

                foreach (var g in TimeInfoMnVm.PassrecordSun2)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Records[0].Name);
                    tmp.Add(g.Records[1].Name);
                    tmp.Add(g.Records[2].Name);
                    tmp.Add(g.Records[3].Name);
                    tmp.Add(g.Records[4].Name);
                    tmp.Add(g.Records[5].Name);
                    tmp.Add(g.Records[6].Name);
                    tmp.Add(g.Records[7].Name);
                    tmp.Add(g.Records[8].Name);
                    tmp.Add(g.Records[9].Name);
                    tmp.Add(g.Records[10].Name);
                    tmp.Add(g.Records[11].Name);
                    tmp.Add(g.Records[12].Name);



                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(SunRasieTable3);
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("日期");
                lsttitle.Add("1月");
                lsttitle.Add("2月");
                lsttitle.Add("3月");
                lsttitle.Add("4月");
                lsttitle.Add("5月");
                lsttitle.Add("6月");
                lsttitle.Add("7月");
                lsttitle.Add("8月");
                lsttitle.Add("9月");
                lsttitle.Add("10月");
                lsttitle.Add("11月");
                lsttitle.Add("12月");


                var lstobj = new List<List<object>>();

                foreach (var g in TimeInfoMnVm.PassrecordSun3)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Records[0].Name);
                    tmp.Add(g.Records[1].Name);
                    tmp.Add(g.Records[2].Name);
                    tmp.Add(g.Records[3].Name);
                    tmp.Add(g.Records[4].Name);
                    tmp.Add(g.Records[5].Name);
                    tmp.Add(g.Records[6].Name);
                    tmp.Add(g.Records[7].Name);
                    tmp.Add(g.Records[8].Name);
                    tmp.Add(g.Records[9].Name);
                    tmp.Add(g.Records[10].Name);
                    tmp.Add(g.Records[11].Name);
                    tmp.Add(g.Records[12].Name);



                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(SunRasieTable4);
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("日期");
                lsttitle.Add("1月");
                lsttitle.Add("2月");
                lsttitle.Add("3月");
                lsttitle.Add("4月");
                lsttitle.Add("5月");
                lsttitle.Add("6月");
                lsttitle.Add("7月");
                lsttitle.Add("8月");
                lsttitle.Add("9月");
                lsttitle.Add("10月");
                lsttitle.Add("11月");
                lsttitle.Add("12月");


                var lstobj = new List<List<object>>();

                foreach (var g in TimeInfoMnVm.PassrecordSun4)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Records[0].Name);
                    tmp.Add(g.Records[1].Name);
                    tmp.Add(g.Records[2].Name);
                    tmp.Add(g.Records[3].Name);
                    tmp.Add(g.Records[4].Name);
                    tmp.Add(g.Records[5].Name);
                    tmp.Add(g.Records[6].Name);
                    tmp.Add(g.Records[7].Name);
                    tmp.Add(g.Records[8].Name);
                    tmp.Add(g.Records[9].Name);
                    tmp.Add(g.Records[10].Name);
                    tmp.Add(g.Records[11].Name);
                    tmp.Add(g.Records[12].Name);



                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }
        }
    }
}
