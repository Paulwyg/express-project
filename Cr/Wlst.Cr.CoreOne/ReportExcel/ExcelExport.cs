using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using org.in2bits.MyXls;
using System.IO;


namespace Wlst.Cr.CoreOne.ReportExcel
{
    public class ExcelExport
    {
        public const int MaxRowCount = 100000;

        /// <summary>
        /// 按行写入数据
        /// </summary>
        /// <param name="filePath">完整的保存路径 包含文件名</param>
        /// <param name="firstTitleRow">第一行的Title信息</param>
        /// <param name="rowsInfo">每行数据</param>
        public static void ExcelExportWriteByRow(string filePath, List<object> firstTitleRow,
                                                 List<List<object>> rowsInfo)
        {
            try
            {
                string sheetName = Path.GetFileNameWithoutExtension(filePath);
                var xls = new XlsDocument();
                Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);

                for (int j = 0; j < firstTitleRow.Count; j++)
                {
                    if (firstTitleRow[j] == null) continue;
                    sheet.Cells.Add(1, j + 1, firstTitleRow[j].ToString());
                }
                for (int j = 0; j < rowsInfo.Count; j++)
                {
                    if (j > MaxRowCount)
                    {
                        sheet.Cells.Add(j + 2, 1, "超出100000列，不再写入.");
                        break;
                    }
                    for (int k = 0; k < rowsInfo[j].Count; k++)
                    {
                        if (rowsInfo[j][k] == null) continue;
                        sheet.Cells.Add(j + 2, k + 1, rowsInfo[j][k].ToString());
                    }
                }
                string strFilePath = Path.GetDirectoryName(filePath);
                xls.FileName = sheetName;
                xls.Save(strFilePath);
                xls = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("系统在导出报表是出错：" + ex);
            }


        }

        /// <summary>
        /// 按列写入数据
        /// </summary>
        /// <param name="filePath">完整的保存路径 包含文件名</param>
        /// <param name="columsInfoOffirstColumIsTitle">每列的数据 每列第一个数据为该列的Title信息</param>
        public static void ExcelExportWriteByColum(string filePath, List<List<object>> columsInfoOffirstColumIsTitle)
        {
            try
            {
                string sheetName = Path.GetFileNameWithoutExtension(filePath);
                var xls = new XlsDocument();
                Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);


                for (int j = 0; j < columsInfoOffirstColumIsTitle.Count; j++)
                {
                    for (int k = 0; k < columsInfoOffirstColumIsTitle[j].Count; k++)
                    {
                        if (j == 0 && k > MaxRowCount)
                        {
                            sheet.Cells.Add(k + 1, 1, "超出100000列，不再写入.");
                            break;
                        }
                        else if (k > MaxRowCount)
                        {
                            break;
                        }
                        if (columsInfoOffirstColumIsTitle[j][k] == null) continue;
                        sheet.Cells.Add(k + 1, j + 1, columsInfoOffirstColumIsTitle[j][k].ToString());
                    }
                }
                string strFilePath = Path.GetDirectoryName(filePath);
                xls.FileName = sheetName;
                xls.Save(strFilePath);
                xls = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("系统在导出报表是出错：" + ex);
            }


        }



        /// <summary>
        /// 按列写入数据
        /// </summary>
        /// <param name="filePath">完整的保存路径 包含文件名</param>
        /// <param name="listView">显示的列表</param>
        private static void ExcelExportWriteByListView(string filePath, ListView listView)
        {
            try
            {
                string sheetName = Path.GetFileNameWithoutExtension(filePath);
                var xls = new XlsDocument();
                Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);

                //foreach (var g in listView .Items )
                //{
                //    var t = g as ListViewItem;
                //    if (t == null) continue;
                //    ListBoxItem b
                    
                //}
                //for (int j = 0; j < listView.Items.Count; j++)
                //{
                //    for (int k = 0; k < listView.ItemsSource; k++)
                //    {
                //        if (j == 0 && k > MaxRowCount)
                //        {
                //            sheet.Cells.Add(k + 1, 1, "超出100000列，不再写入.");
                //            break;
                //        }
                //        else if (k > MaxRowCount)
                //        {
                //            break;
                //        }
                //        if (columsInfoOffirstColumIsTitle[j][k] == null) continue;
                //        sheet.Cells.Add(k + 1, j + 1, columsInfoOffirstColumIsTitle[j][k].ToString());
                //    }
                //}
                string strFilePath = Path.GetDirectoryName(filePath);
                xls.FileName = sheetName;
                xls.Save(strFilePath);
                xls = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("系统在导出报表是出错：" + ex);
            }


        }


    }
}
