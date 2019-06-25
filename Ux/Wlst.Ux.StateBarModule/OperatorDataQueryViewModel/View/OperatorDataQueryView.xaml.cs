using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;


using Microsoft.Win32;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.StateBarModule.OperatorDataQueryViewModel.Service;
using Telerik.Windows.Controls.GridView;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.StateBarModule.OperatorDataQueryViewModel.View
{
    /// <summary>
    /// DataQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Services.ViewIdAssign.OperatorDataQueryViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OperatorDataQueryView
    {
        public OperatorDataQueryView()
        {
            InitializeComponent();
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(),
                                                       FundEventHandlers, FundOrderFilters);

            //Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
        }

        [Import]
        public IIOperatorDataQueryViewModel Model
        {
            get { return DataContext as IIOperatorDataQueryViewModel; }
            set { DataContext = value; }
        }

        private bool _isdetailin;

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.None &&
                    args.EventId == Services.EventIdAssign.AnimationOperatorDataQueryViewModelEnterId)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.None &&
                    args.EventId == Services.EventIdAssign.AnimationOperatorDataQueryViewModelEnterId && !_isdetailin)
                {

                    Animations.Animation.EnterFromLeftAndTop(detail, 1, true);
                    _isdetailin = true;
                }

            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("ReSetAnimation error in FundEventHandlers:ex:" + xe);
            }
        }

        protected void AnimationLeaveClick(object sender, EventArgs e)
        {
            Animations.Animation.LeaveFromLeftAndBottom(detail, 1, true);
            _isdetailin = false;
        }

        private void BtnExportClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(gridview);
        }

        //private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    var list=new List<List<string>>();
        //    //表头
        //    var pHeader=new List<string>();
        //    foreach (var header in gridview.Columns)
        //    {
        //        pHeader.Add(header.Header.ToString());
        //    }
        //    list.Add(pHeader);
        //    //GridViewRow row;
        //    //foreach (var item in gridview.Items)
        //    //{
        //    //    var pDate = new List<string>();
        //    //    row = gridview.ItemContainerGenerator.ContainerFromItem(item) as GridViewRow;
        //    //    for (int i = 0; i < gridview.Columns.Count; i++)
        //    //    {
        //    //        var t = (row.Cells[i].Content as ).Text;
        //    //        pDate.Add(t);
        //    //    }
        //    //    list.Add(pDate);
        //    //}
        //}
        
       
        
    }
}
