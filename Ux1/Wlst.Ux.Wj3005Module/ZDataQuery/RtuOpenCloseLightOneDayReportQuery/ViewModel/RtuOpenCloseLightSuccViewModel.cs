using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightOneDayReportQuery.Service;
using Wlst.client;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.RtuOpenCloseLightOneDayReportQuery.ViewModel
{
    [Export(typeof(IIRtuOpenCloseLightOneDayReportQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuOpenCloseLightSuccViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIRtuOpenCloseLightOneDayReportQuery
    {

        public RtuOpenCloseLightSuccViewModel()
        {
            this.InitAction();
            DtSelect = DateTime.Now.AddDays(-1);
        }

        #region Query

        #region CmdExport

        private DateTime _dtCmd_cmdCmdQuery;
        private DateTime _dtCmdSelectTime;
        private ICommand _cmdCmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdCmdQuery == null)
                    _cmdCmdQuery = new RelayCommand(ExCmdQuery, CanExCmdQuery, false);
                return _cmdCmdQuery;
            }
        }

        private void ExCmdQuery()
        {
            _dtCmd_cmdCmdQuery = DateTime.Now;
            _dtCmdSelectTime = DtSelect;

            var dtstart = new DateTime(DtSelect.Year, DtSelect.Month, DtSelect.Day, 0, 0, 1);
            this.Query(dtstart.Ticks, dtstart.AddDays(1).Ticks);
        }

        private bool CanExCmdQuery()
        {
            if (DtSelect.Year == _dtCmdSelectTime.Year && DtSelect.Month == _dtCmdSelectTime.Month &&
                _dtCmdSelectTime.Day == DtSelect.Day) return false;
            return DateTime.Now.Ticks - _dtCmd_cmdCmdQuery.Ticks > 30000000;
        }

        #endregion

        #endregion

        #region CmdExport

        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {

            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("终端地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("日期");
                lsttitle.Add("操作类型");
                lsttitle.Add("K1确认操作结果时间");
                lsttitle.Add("K1操作结果");
                lsttitle.Add("K2确认操作结果时间");
                lsttitle.Add("K2操作结果");
                lsttitle.Add("K3确认操作结果时间");
                lsttitle.Add("K3操作结果");
                lsttitle.Add("K4确认操作结果时间");
                lsttitle.Add("K4操作结果");
                lsttitle.Add("K5确认操作结果时间");
                lsttitle.Add("K5操作结果");
                lsttitle.Add("K6确认操作结果时间");
                lsttitle.Add("K6操作结果");

                var lstobj = new List<List<object>>();
                var gts = (from g in Items orderby g.PhyId ascending select g).ToList();
                foreach (var g in gts  )
                {
                    var tmp = new List<object>();
                    tmp.Add(g.PhyId );
                    tmp.Add(g.RtuName  );
                    tmp.Add(g.Date );
                    tmp.Add(g.Operator );
                    tmp.Add(g.LoopsItem [0].Time );
                    tmp.Add(g.LoopsItem [0].IsSucc );
                    tmp.Add(g.LoopsItem[1].Time);
                    tmp.Add(g.LoopsItem[1].IsSucc);
                    tmp.Add(g.LoopsItem[2].Time);
                    tmp.Add(g.LoopsItem[2].IsSucc);

                    tmp.Add(g.LoopsItem[3].Time);
                    tmp.Add(g.LoopsItem[3].IsSucc);
                    tmp.Add(g.LoopsItem[4].Time);
                    tmp.Add(g.LoopsItem[4].IsSucc);
                    tmp.Add(g.LoopsItem[5].Time);
                    tmp.Add(g.LoopsItem[5].IsSucc);
                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception e)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出巡测报表时报错:" + e);
            }
        }

        private bool CanExCmdExport()
        {
            return Items != null && Items.Count > 0;
            return false;
        }

        #endregion

        #region atrri


        private string  dRemindt;

        public string  Remind
        {
            get { return dRemindt; }
            set
            {
                if (dRemindt == value) return;
                dRemindt = value;
                this.RaisePropertyChanged(() => this.Remind);
            }
        }

        private DateTime dt;

        public DateTime DtSelect
        {
            get { return dt; }
            set
            {
                if (dt == value) return;
                dt = value;
                this.RaisePropertyChanged(() => this.DtSelect);
            }
        }
     
        private ObservableCollection<RtuOneDayOneOperatorItem> _items;


        public ObservableCollection<RtuOneDayOneOperatorItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<RtuOneDayOneOperatorItem>();
                }
                return _items;
            }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Items);
            }
        }

        #endregion


        public void NavOnLoad(params object[] parsObjects)
        {
            DtSelect = DateTime.Now.AddDays(-1);
            Remind = "请设置好查询日期进行查询...";
        }

        public void OnUserHideOrClosing()
        {
            this.Items.Clear();
        }

        #region tab iinterface
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get
            {
                return "开关灯报表查询";//I36N .Services.I36N .ConvertByCodingOne("11090001", "Setting");
                //return "Setting";
            }
        }

        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion


    }


    /// <summary>
    /// Event
    /// </summary>
    public partial class RtuOpenCloseLightSuccViewModel
    {

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_oneday_open_close_record ,// .wlst_svr_ans_cnt_wj3090_record_open_close_light_oneday_info  ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_request_openCloseOneDayReportRecord,
                RecordDataRequest,
                typeof (RtuOpenCloseLightSuccViewModel), this);
        }

        public void RecordDataRequest(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstRtuOnedayOpenCloseRecord   ;
            if (info == null) return;

          //  Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "查询数据已经返回，请查看.";
            var tmpitems = new ObservableCollection<RtuOneDayOneOperatorItem>();
            var gts = (from t in info.Rtus orderby t.RtuId ascending select t).ToList();
            foreach (var t in gts )
            {
                var tmp = SpriteInfo(t);
                foreach (var g in tmp) tmpitems.Add(g);
            }
            this.Items = tmpitems;
            // Remind = "数据已反馈，查询命令已结束，请查看数据！";
         //   Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "查询数据已经返回，请查看.";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + Items.Count + " 条数据.";
        }


        private List<RtuOneDayOneOperatorItem> SpriteInfo(ReportOpenCloseOneDayRecord.ReportOpenCloseOneDayOneRtuRecord info)
        {
            //long min = 0;
            //if (info.LoopCloseOper.Count > 0) min = info.LoopCloseOper[0].OperatorTime;
            //if (info.LoopOpenOper.Count > 0 && info.LoopOpenOper[0].OperatorTime < min)
            //    min = info.LoopOpenOper[0].OperatorTime;
            //var timessss = new DateTime(min);

            //var currentzero = new DateTime(timessss.Year, timessss.Month, timessss.Day, 0, 0, 1);
            //string datess = currentzero.Month.ToString("d2") + "-" + currentzero.Day.ToString("d2");

            var dirOpen =
                new Dictionary
                    <string, List<ReportOpenCloseOneDayRecord.ReportOpenCloseOneDayOneRtuRecord.ReportOpenCloseOneDayOneRtuOneLoopRecord>>();
            var dirClose =
                new Dictionary
                    <string, List<ReportOpenCloseOneDayRecord.ReportOpenCloseOneDayOneRtuRecord.ReportOpenCloseOneDayOneRtuOneLoopRecord>>();

            foreach (var g in info.LoopCloseOper)
            {
                var timessss = new DateTime(g.OperatorTime);
                var datess = timessss.Month.ToString("d2") + "-" + timessss.Day.ToString("d2");
                if (!dirClose.ContainsKey(datess))
                    dirClose.Add(datess, new List<ReportOpenCloseOneDayRecord.ReportOpenCloseOneDayOneRtuRecord.ReportOpenCloseOneDayOneRtuOneLoopRecord>());
                dirClose[datess].Add(g);
            }

            foreach (var g in info.LoopOpenOper)
            {
                var timessss = new DateTime(g.OperatorTime);
                var datess = timessss.Month.ToString("d2") + "-" + timessss.Day.ToString("d2");
                if (!dirOpen.ContainsKey(datess))
                    dirOpen.Add(datess, new List<ReportOpenCloseOneDayRecord.ReportOpenCloseOneDayOneRtuRecord.ReportOpenCloseOneDayOneRtuOneLoopRecord>());
                dirOpen[datess].Add(g);
            }

            var dirRtnOpen = new Dictionary<string, RtuOneDayOneOperatorItem>();
            foreach (var g in dirOpen)
            {
                if (g.Value.Count < 1) continue;
                RtuOneDayOneOperatorItem tmp = new RtuOneDayOneOperatorItem();
                tmp.RtuId = info.RtuId;
                tmp.Date = g.Key;
                tmp.Operator = "开灯";

                foreach (var m in g.Value)
                {
                    if (m.LoopId >17) continue;
                    var timesssss = new DateTime(m.OperatorTime);
                    tmp.LoopsItem[m.LoopId - 1].LoopId = m.LoopId;
                 
                    if (tmp.LoopsItem[m.LoopId - 1].IsSucc.Contains("--"))
                    {
                        tmp.LoopsItem[m.LoopId - 1].IsSucc = "";
                        tmp.LoopsItem[m.LoopId - 1].Time = "";
                    }

                    tmp.LoopsItem[m.LoopId - 1].Time += timesssss.Hour.ToString("d2") + ":" +
                                                        timesssss.Minute.ToString("d2") + "/";
                    tmp.LoopsItem[m.LoopId - 1].IsSucc += m.IsSucc ? "√/" : "X/";

                }
                foreach (var m in tmp.LoopsItem)
                {
                    if (!string.IsNullOrEmpty(m.IsSucc) && m.IsSucc.Length > 1)
                        m.IsSucc = m.IsSucc.Substring(0, m.IsSucc.Length - 1);

                    if (!string.IsNullOrEmpty(m.Time) && m.Time.Length > 1)
                        m.Time = m.Time.Substring(0, m.Time.Length - 1);

                }
                if (!dirRtnOpen.ContainsKey(g.Key))
                    dirRtnOpen.Add(g.Key, tmp);
                //     dirRtnOpen[g.Key].Add(tmp);
            }
            var dirRtnClose = new Dictionary<string, RtuOneDayOneOperatorItem>();
            foreach (var g in dirClose)
            {
                if (g.Value.Count < 1) continue;
                RtuOneDayOneOperatorItem tmp = new RtuOneDayOneOperatorItem();
                tmp.RtuId = info.RtuId;
                tmp.Date = g.Key;
                tmp.Operator = "关灯";

                foreach (var m in g.Value)
                {
                    if (m.LoopId > 17) continue;
                    var timesssss = new DateTime(m.OperatorTime);
                    tmp.LoopsItem[m.LoopId - 1].LoopId = m.LoopId;

                    if (tmp.LoopsItem[m.LoopId - 1].IsSucc.Contains("--"))
                    {
                        tmp.LoopsItem[m.LoopId - 1].IsSucc = "";
                        tmp.LoopsItem[m.LoopId - 1].Time = "";
                    }
                    tmp.LoopsItem[m.LoopId - 1].Time += timesssss.Hour.ToString("d2") + ":" +
                                                        timesssss.Minute.ToString("d2") + "/";
                    tmp.LoopsItem[m.LoopId - 1].IsSucc += m.IsSucc ? "√/" : "X/";

                }
                foreach (var m in tmp.LoopsItem)
                {
                    if (!string.IsNullOrEmpty(m.IsSucc) && m.IsSucc.Length > 1)
                        m.IsSucc = m.IsSucc.Substring(0, m.IsSucc.Length - 1);

                    if (!string.IsNullOrEmpty(m.Time) && m.Time.Length > 1)
                        m.Time = m.Time.Substring(0, m.Time.Length - 1);

                }
                if (!dirRtnClose.ContainsKey(g.Key))
                    dirRtnClose.Add(g.Key, tmp);

            }
            var lst = new List<string>();
            foreach (var g in dirRtnOpen.Keys) if (!lst.Contains(g)) lst.Add(g);
            foreach (var g in dirRtnClose.Keys) if (!lst.Contains(g)) lst.Add(g);
            var tmplst = (from g in lst orderby g ascending select g).ToList();

            List<RtuOneDayOneOperatorItem> rtn = new List<RtuOneDayOneOperatorItem>();
            foreach (var g in tmplst)
            {
                if (dirRtnOpen.ContainsKey(g)) rtn.Add(dirRtnOpen[g]);
                if (dirRtnClose.ContainsKey(g)) rtn.Add(dirRtnClose[g]);
            }
            return rtn;
        }

    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class RtuOpenCloseLightSuccViewModel
    {
        private void Query(long dtstarttime, long dtendtime)
        {
            var info =
                Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_oneday_open_close_record ;// .wlst_cnt_wj3090_record_open_close_light_oneday_info ;//.ServerPart.wlst_OpenCloseLight_clinet_request_openCloseOneDayReportRecord;
            info.WstRtuOnedayOpenCloseRecord  .DtStartTime = dtstarttime;
            info.WstRtuOnedayOpenCloseRecord.DtEndTime = dtendtime;
           
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }
    }
}
