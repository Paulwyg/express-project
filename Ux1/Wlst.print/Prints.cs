using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using FontStyle = System.Drawing.FontStyle;
using MessageBox = System.Windows.Forms.MessageBox;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;


namespace Wlst.print
{
    public class Prints
    {
        ///// <summary>
        ///// 打印
        ///// </summary>
        ///// <param name="print"></param>
        //public static void Print(Grid print)
        //{
        //    PrintDialog dialog = new PrintDialog();
            
        //    if (dialog.ShowDialog() == true)
        //    {

        //        dialog.PrintVisual(print, "Print Test");
        //    }
        //}

        ///// <summary>
        ///// 保存图片
        ///// </summary>
        ///// <param name="print"></param>
        //  static void SaveGridToPng(Grid print)
        //{

        //    //将控件中内容转化为图片格式
        //    var bitmapRender = new RenderTargetBitmap((int) print.ActualWidth, (int) print.ActualHeight, 96, 96,
        //                                              PixelFormats.Pbgra32);
        //    bitmapRender.Render(print);
        //    var bmpEncoder = new BmpBitmapEncoder();
        //    bmpEncoder.Frames.Add(BitmapFrame.Create(bitmapRender));
        //      if(System .IO .Directory .Exists(Environment.CurrentDirectory + "\\print" )==false )
        //          Directory.CreateDirectory(Environment.CurrentDirectory + "\\print");
        //    using (var file = File.Create(Environment.CurrentDirectory + "\\print\\output.GIF"))
        //        bmpEncoder.Save(file);
        //    //设置打印内容及其字体，颜色和位置
        //    //e.Graphics.DrawString("Hello World", new Font(new FontFamily("黑体"), 24), Brushes.Red, 50, 50);
        //}

        ///// <summary>
        ///// 预览
        ///// </summary>
        ///// <param name="print"></param>

        //public static void PrintVisi(Grid print,bool a)
        //{
        //    SaveGridToPng(print);

        //    //实例化打印对象
        //    PrintDocument printDocument1 = new PrintDocument();
        //    //设置打印用的纸张,当设置为Custom的时候，可以自定义纸张的大小
        //    //printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custum", 500, 500);
        //    //注册PrintPage事件，打印每一页时会触发该事件
        //    printDocument1.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
        //    //打印方向，true 如果以横向方向; 应打印的页，否则为 false
        //    printDocument1.DefaultPageSettings.Landscape = a;
        //    //初始化打印预览对话框对象
        //    PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        //    //将printDocument1对象赋值给打印预览对话框的Document属性
        //    printPreviewDialog1.Document = printDocument1;
        //    //打开打印预览对话框
        //    DialogResult result = printPreviewDialog1.ShowDialog();
        //    if (result == DialogResult.OK)
        //        printDocument1.Print(); //开始打印
        //}


        //private static void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        //{


        //    // Create image.
        //    Image newImage = Image.FromFile(Environment.CurrentDirectory + "\\print\\output.GIF");

        //    // Create point for upper-left corner of image.
        //    e.Graphics.DrawImage(newImage, 0, 0);

        //}


        //以下用户可自定义
        //当前要打印文本的字体及字号
        private static Font TableFont = new Font("Verdana", 10, FontStyle.Regular);
        //表头字体
        private static Font HeadFont = new Font("Verdana", 20, FontStyle.Bold);
        //打印时间字体
        private static Font TimeFont = new Font("Verdana", 12, FontStyle.Bold);
        //附加内容字体
        private static Font AddFont = new Font("Verdana", 12, FontStyle.Bold);
        //表头文字
        private static string HeadText = string.Empty;
        //附加文字
        private static string AddText = string.Empty;
        private static string AddText1 = string.Empty;
        private static string AddText2 = string.Empty;
        //表头高度
        //打印页数
        private static int PageNumber = 1;
        private static int HeadHeight = 40;
        //表的基本单位
        private static int[] XUnit;
        private static int YUnit = TableFont.Height * 2;
        //以下为模块内部使用
        private static PrintDocument DataTablePrinter;
        private static DataRow DataGridRow;
        private static DataTable DataTablePrint;
        //当前要所要打印的记录行数,由计算得到
        private static int PageRecordNumber;
        //正要打印的页号
        private static int PrintingPageNumber = 1;
        //已经打印完的记录数
        private static int PrintRecordComplete;
        private static int PLeft;
        private static int PTop;
        private static int PRight;
        private static int PBottom;
        private static int PWidth;
        private static int PHeigh;
        //当前画笔颜色
        private static SolidBrush DrawBrush = new SolidBrush(Color.Black);
        //每页打印的记录条数
        private static int PrintRecordNumber;
        //第一页打印的记录条数
        private static int FirstPrintRecordNumber;
        //总共应该打印的页数
        private static int TotalPage;
        //与列名无关的统计数据行的类目数（如，总计，小计......）
        public static int TotalNum = 0;
        //列的最大宽度
        public static int MaxWidth = 150;

        /// <summary>
        /// 打印
        /// <param name="tabletitle">表头</param>
        /// <param name="table">打印内容</param>
        /// <param name="tf">设置打印横向还是纵向</param>
        /// <param name="title">打印文件的标题</param>
        /// <param name="add">附加打印内容1</param>
        /// <param name="add1">附加打印内容2</param>
        /// <param name="add2">附加打印内容3</param>  
        ///  </summary>     
        public static void Print(List<string> tabletitle, List<List<string>> table, bool tf, string title, string add,
                                 string add1, string add2)
        {
            try
            {
                System.Windows.Forms.PrintDialog pd = new System.Windows.Forms.PrintDialog();
                pd.AllowSomePages = true;


                var doc = CreatePrintDocument(tabletitle, table, tf, title, add, add1, add2);
                if (doc == null)
                {
                    var information = WlstMessageBox.Show
                                           ("打印提示", "打印超过10张，请重新筛选数据后打印！", WlstMessageBoxType.Ok);
                    return;
                }
                pd.Document = doc;
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    doc.Print();
                    //CreatePrintDocument(tabletitle, table, tf, title, add, add1, add2).Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");

            }
        }

      /// <summary>
      /// 打印预览
      /// <param name="tabletitle">表头</param>
      /// <param name="table">打印内容</param>
      /// <param name="tf">设置打印横向还是纵向</param>
      /// <param name="title">打印文件的标题</param>
      /// <param name="add">附加打印内容1</param>
      /// <param name="add1">附加打印内容2</param>
      /// <param name="add2">附加打印内容3</param>  
      /// </summary>
        public static void PrintPriview(List<string> tabletitle, List<List<string>> table, bool tf, string title,
                                        string add, string add1, string add2)
        {
            try
            {
                PrintPreviewDialog PrintPriview = new PrintPreviewDialog();
                var doc = CreatePrintDocument(tabletitle, table, tf, title, add, add1, add2);
                if (doc == null)
                {
                    var information = WlstMessageBox.Show
                                           ("打印提示", "打印超过10张，请重新筛选数据后打印！", WlstMessageBoxType.Ok);
                    return;
                }
                PrintPriview.Document = doc;// CreatePrintDocument(tabletitle, table, tf, title, add, add1, add2);
                PrintPriview.WindowState = FormWindowState.Maximized;
                PrintPriview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");

            }
        }

        /// <summary>
        /// 创建打印文件
        /// </summary>
        private static PrintDocument CreatePrintDocument(List<string> tabletitle, List<List<string>> table, bool tf,
                                                         string title, string add, string add1, string add2)
        {

            DataTable dt = new DataTable();
            DataRow row = dt.NewRow();

            foreach (var t in tabletitle)
            {

                dt.Columns.Add(t);

            }

            for (int i = 0; i < table.Count; i++)
            {
                for (int j = 0; j < table[0].Count; j++)

                    row[tabletitle[j]] = table[i][j];
                dt.Rows.Add(row);

                row = dt.NewRow();

            }

            DataTablePrint = dt;
            HeadText = title;
            AddText = add;
            AddText1 = add1;
            AddText2 = add2;
            DataTablePrinter = new PrintDocument();

            PageSetupDialog pageSetup = new PageSetupDialog();
            pageSetup.Document = DataTablePrinter;
            DataTablePrinter.DefaultPageSettings = pageSetup.PageSettings;
            DataTablePrinter.DefaultPageSettings.Landscape = tf; //设置打印横向还是纵向
            //PLeft = 30; 
            PLeft = DataTablePrinter.DefaultPageSettings.Margins.Left;
            PTop = DataTablePrinter.DefaultPageSettings.Margins.Top;
            PRight = DataTablePrinter.DefaultPageSettings.Margins.Right;
            PBottom = DataTablePrinter.DefaultPageSettings.Margins.Bottom;
            PWidth = DataTablePrinter.DefaultPageSettings.Bounds.Width;
            PHeigh = DataTablePrinter.DefaultPageSettings.Bounds.Height;
            XUnit = new int[DataTablePrint.Columns.Count];
            PrintRecordNumber = Convert.ToInt32((PHeigh - PTop - PBottom - YUnit) / YUnit);
            FirstPrintRecordNumber = Convert.ToInt32((PHeigh - PTop - PBottom - HeadHeight - YUnit) / YUnit);

            if (DataTablePrint.Rows.Count > PrintRecordNumber)
            {
                if ((DataTablePrint.Rows.Count - FirstPrintRecordNumber) % PrintRecordNumber == 0)
                {
                    TotalPage = (DataTablePrint.Rows.Count - FirstPrintRecordNumber) / PrintRecordNumber + 1;
                }
                else
                {
                    TotalPage = (DataTablePrint.Rows.Count - FirstPrintRecordNumber) / PrintRecordNumber + 2;
                }
            }
            else
            {
                TotalPage = 1;
            }

            if (TotalPage > 10) return null;//默认大于10张就提示 不打印

            DataTablePrinter.PrintPage += new PrintPageEventHandler(DataTablePrinter_PrintPage);
            DataTablePrinter.DocumentName = HeadText;

            return DataTablePrinter;
        }

        /// <summary>
        /// 打印当前页
        /// </summary>
        private static void DataTablePrinter_PrintPage(object sende, PrintPageEventArgs Ev)
        {


            int tableWith = 0;
            string ColumnText;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            //打印表格线格式
            Pen Pen = new Pen(Brushes.Black, 1);

            #region 设置列宽

            int cn = 0;
            int j = DataTablePrint.Columns.Count;
            foreach (DataRow dr in DataTablePrint.Rows)
            {

                int columnNumber = 0;

                for (int i = 0; i < DataTablePrint.Columns.Count; i++)
                {

                    int colwidth = Convert.ToInt32(Ev.Graphics.MeasureString(dr[i].ToString().Trim(), TableFont).Width);
                    if (colwidth < MaxWidth)
                    {
                        if (colwidth > XUnit[i])
                        {
                            XUnit[i] = colwidth;
                        }
                    }
                    else
                    {
                        //dr[i] = (dr[i].ToString()).Substring(0,9)+"...";        //判断行宽是否超出最大宽度值
                        dr[i] = GetStr(dr[i].ToString(), 18, "...");
                        XUnit[i] = MaxWidth;
                    }

                    columnNumber = columnNumber + XUnit[i];
                    if (columnNumber > PWidth - 50)
                    {
                        if (cn < j)
                            cn = i;
                        break;
                    }
                    cn = i;
                }
                j = cn;
            }

            for (int i = DataTablePrint.Columns.Count - 1; i > cn; i--)
            {
                DataTablePrint.Columns.Remove(DataTablePrint.Columns[i].ColumnName);
            }

            int cn1 = 0;
            int columnNumber1 = 0;
            if (PrintingPageNumber == 1)
            {
                for (int Cols = 0; Cols <= DataTablePrint.Columns.Count - 1; Cols++)
                {
                    ColumnText = DataTablePrint.Columns[Cols].ColumnName.Trim();
                    int colwidth = Convert.ToInt32(Ev.Graphics.MeasureString(ColumnText, TableFont).Width);
                    if (colwidth < MaxWidth)
                    {

                        if (colwidth > XUnit[Cols])
                        {
                            XUnit[Cols] = colwidth;
                        }
                    }
                    else
                    {
                        //DataTablePrint.Columns[Cols].ColumnName = ColumnText.Substring(0, 5)+"...";
                        DataTablePrint.Columns[Cols].ColumnName = GetStr(DataTablePrint.Columns[Cols].ColumnName, 18,
                                                                         "...");
                        XUnit[Cols] = MaxWidth;
                    }
                    columnNumber1 = columnNumber1 + XUnit[Cols];
                    if (columnNumber1 > PWidth - 50)
                        break;
                    cn1 = Cols;

                }
                for (int i = DataTablePrint.Columns.Count - 1; i > cn1; i--)
                {
                    DataTablePrint.Columns.Remove(DataTablePrint.Columns[i].ColumnName);
                }
            }
            for (int i = 0; i < DataTablePrint.Columns.Count; i++)
            {
                tableWith += XUnit[i];
            }

            #endregion

            PLeft = (Ev.PageBounds.Width - tableWith) / 2;
            int x = PLeft;
            int y = PTop;
            int stringY = PTop + (YUnit - TableFont.Height) / 2;
            int rowOfTop = PTop;

            //第一页
            if (PrintingPageNumber == 1)
            {
                //打印表头
                Ev.Graphics.DrawString(HeadText, HeadFont, DrawBrush, new Point(Ev.PageBounds.Width / 2, PTop - 70), sf);
                //附加内容
                Ev.Graphics.DrawString(AddText, AddFont, DrawBrush, new Point(Ev.PageBounds.Width / 2 - 300, PTop - 10),
                                       sf);
                Ev.Graphics.DrawString(AddText1, AddFont, DrawBrush, new Point(Ev.PageBounds.Width / 2 + 200, PTop - 10),
                                       sf);


                //设置为第一页时行数
                if (DataTablePrint.Rows.Count <= FirstPrintRecordNumber)
                {
                    PageRecordNumber = DataTablePrint.Rows.Count;
                    //打印时间
                    Ev.Graphics.DrawString("打印时间：" + DateTime.Now.ToString(), TimeFont, DrawBrush,
                                           new Point(Ev.PageBounds.Width / 2 + 200, Ev.PageBounds.Height - PTop), sf);
                    //记录条数
                    Ev.Graphics.DrawString("记录条数：" + DataTablePrint.Rows.Count, TimeFont, DrawBrush,
                                           new Point(Ev.PageBounds.Width / 2 - 100, Ev.PageBounds.Height - PTop), sf);
                    //附加内容
                    Ev.Graphics.DrawString(AddText2, AddFont, DrawBrush,
                                           new Point(Ev.PageBounds.Width / 2 + 200, Ev.PageBounds.Height - PTop + 30), sf);
                }
                else
                {
                    PageRecordNumber = FirstPrintRecordNumber;
                }
                rowOfTop = y = PTop + HeadFont.Height + 10;
                stringY = PTop + HeadFont.Height + 10 + (YUnit - TableFont.Height) / 2;
            }
            else
            {
                //计算,余下的记录条数是否还可以在一页打印,不满一页时为假
                if (DataTablePrint.Rows.Count - PrintRecordComplete >= PrintRecordNumber)
                {
                    PageRecordNumber = PrintRecordNumber;
                }
                else
                {
                    PageRecordNumber = DataTablePrint.Rows.Count - PrintRecordComplete;
                    //打印时间
                    Ev.Graphics.DrawString("打印时间：" + DateTime.Now.ToString(), TimeFont, DrawBrush,
                                           new Point(Ev.PageBounds.Width / 2 + 200, Ev.PageBounds.Height - PTop), sf);
                    //记录条数
                    Ev.Graphics.DrawString("记录条数：" + DataTablePrint.Rows.Count, TimeFont, DrawBrush,
                                           new Point(Ev.PageBounds.Width / 2 - 100, Ev.PageBounds.Height - PTop), sf);
                    //附加内容
                    Ev.Graphics.DrawString(AddText2, AddFont, DrawBrush,
                                           new Point(Ev.PageBounds.Width / 2 + 200, Ev.PageBounds.Height - PTop + 30), sf);
                }
            }
            //打印页码
            Ev.Graphics.DrawString("页码：" + PrintingPageNumber + "/" + TotalPage, TimeFont, DrawBrush,
                                   new Point(Ev.PageBounds.Width / 2 - 300, Ev.PageBounds.Height - PTop), sf);

            #region 列名

            if (PrintingPageNumber == 1 || PageRecordNumber > TotalNum) //最后一页只打印统计行时不打印列名
            {
                //得到datatable的所有列名
                for (int Cols = 0; Cols <= DataTablePrint.Columns.Count - 1; Cols++)
                {
                    ColumnText = DataTablePrint.Columns[Cols].ColumnName.Trim();

                    int colwidth = Convert.ToInt32(Ev.Graphics.MeasureString(ColumnText, TableFont).Width);
                    Ev.Graphics.DrawString(ColumnText, TableFont, DrawBrush, x, stringY);
                    x += XUnit[Cols];
                }
            }

            #endregion



            Ev.Graphics.DrawLine(Pen, PLeft, rowOfTop, x, rowOfTop);
            stringY += YUnit;
            y += YUnit;
            Ev.Graphics.DrawLine(Pen, PLeft, y, x, y);

            //当前页面已经打印的记录行数
            int PrintingLine = 0;
            while (PrintingLine < PageRecordNumber)
            {
                x = PLeft;
                //确定要当前要打印的记录的行号
                DataGridRow = DataTablePrint.Rows[PrintRecordComplete];
                for (int Cols = 0; Cols <= DataTablePrint.Columns.Count - 1; Cols++)
                {
                    Ev.Graphics.DrawString(DataGridRow[Cols].ToString().Trim(), TableFont, DrawBrush, x, stringY);
                    x += XUnit[Cols];
                }
                stringY += YUnit;
                y += YUnit;
                Ev.Graphics.DrawLine(Pen, PLeft, y, x, y);

                PrintingLine += 1;
                PrintRecordComplete += 1;
                if (PrintRecordComplete >= DataTablePrint.Rows.Count)
                {
                    Ev.HasMorePages = false;
                    PrintRecordComplete = 0;
                }
            }

            Ev.Graphics.DrawLine(Pen, PLeft, rowOfTop, PLeft, y);
            x = PLeft;
            for (int Cols = 0; Cols < DataTablePrint.Columns.Count; Cols++)
            {
                x += XUnit[Cols];
                Ev.Graphics.DrawLine(Pen, x, rowOfTop, x, y);
            }

            Ev.Graphics.DrawLine(Pen, 65, rowOfTop - 30, Ev.PageBounds.Width - 65, rowOfTop - 30); //上分界线
            Ev.Graphics.DrawLine(Pen, 65, Ev.PageBounds.Height - PBottom + 20, Ev.PageBounds.Width - 65,
                                 Ev.PageBounds.Height - PBottom + 20); //下分界线


            PrintingPageNumber += 1;

            if (PrintingPageNumber > TotalPage)
            {
                Ev.HasMorePages = false;
                PrintingPageNumber = 1;
                PrintRecordComplete = 0;
            }
            else
            {
                Ev.HasMorePages = true;
            }

        }

        public static string GetStr(string s, int l, string endStr)
        {
            string temp = s.Substring(0, (s.Length < l + 1) ? s.Length : l + 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(temp);

            string outputStr = "";
            int count = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                if ((int)encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= l - endStr.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > l)
                    break;
            }

            if (count <= l)
            {
                outputStr = temp;
                endStr = "";
            }

            outputStr += endStr;

            return outputStr;
        }

    }


}
