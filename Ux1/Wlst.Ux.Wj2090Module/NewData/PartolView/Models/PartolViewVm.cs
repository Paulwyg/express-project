using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Wj2090Module.NewData.PartolView.Services;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Models
{
    [Export(typeof (IIPartolView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class PartolViewVm : Wlst.Cr.Core.CoreServices.ObservableObject, IIPartolView
    {

        public PartolViewVm ()
        {
            this.InitAciton();
        }
      //  public static bool UserManalOperator = false;

        /// <summary>
        /// 2 未知控制器，4物理信息，5巡测数据，6 辅助数据
        /// </summary>
        /// <param name="dataview"></param>
        public static void ShowThisView(int dataview)
        {
            //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
            //    Wj2090Module.Services.ViewIdAssign.PartolViewId,
            //    Wj2090Module.Services.ViewIdAssign.PartolViewAttachRegion, true);

            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                Wj2090Module.Services.ViewIdAssign.PartolViewId, dataview);
        }

        private static int CurrentSluId = 0;
        public static void SetCurrentSluId(int sluId)
        {
            CurrentSluId = sluId;
        }

        private bool _thisViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            _thisViewActive = true;

            try
            {
                DataItem2.Clear();
                DataItem4.Clear();
                DataItem5.Clear();
                DataItem6.Clear();

                IndexView = 5;
                if (parsObjects.Any())
                {

                    IndexView = Convert.ToInt32(parsObjects[0]);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            DataItem2.Clear();
            DataItem4.Clear();
            DataItem5.Clear();
            DataItem6.Clear();
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯巡测数据"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        #region attri

        private int _isdfsdsSome;

        /// <summary>
        /// 显示页面
        /// </summary>
        public int IndexView
        {
            get { return _isdfsdsSome; }
            set
            {
                if (_isdfsdsSome == value) return;
                _isdfsdsSome = value;
                this.RaisePropertyChanged(() => this.IndexView);
                if(value ==2)
                {
                    xRemind = "未知控制器数据。";
                }
                if (value == 4)
                {
                    xRemind = "控制器物理信息。";
                }
                if (value == 5)
                {
                    xRemind = "控制器巡测数据。";
                }
                if (value == 6)
                {
                    xRemind = "控制器辅助数据。";
                }
            }
        }

        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (_remind == value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }


        private string _xremind;

        public string xRemind
        {
            get { return _xremind; }
            set
            {
                if (_xremind == value) return;
                _xremind = value;
                RaisePropertyChanged(() => xRemind);
            }
        }
        #endregion

        #region Items

        private ObservableCollection<DataSluUnknow2> _dataItem2;

        public ObservableCollection<DataSluUnknow2> DataItem2
        {
            get
            {
                if (_dataItem2 == null)
                {
                    _dataItem2 = new ObservableCollection<DataSluUnknow2>();

                }
                return _dataItem2;
            }
            set
            {
                if (_dataItem2 == value) return;
                _dataItem2 = value;
                RaisePropertyChanged(() => _dataItem2);
            }
        }

        private ObservableCollection<DataSluPhy4> _dataItem4;

        public ObservableCollection<DataSluPhy4> DataItem4
        {
            get
            {
                if (_dataItem4 == null)
                {
                    _dataItem4 = new ObservableCollection<DataSluPhy4>();

                }
                return _dataItem4;
            }
            set
            {
                if (_dataItem4 == value) return;
                _dataItem4 = value;
                RaisePropertyChanged(() => DataItem4);
            }
        }

        private ObservableCollection<DataSluLamp5> _dataItem5;

        public ObservableCollection<DataSluLamp5> DataItem5
        {
            get
            {
                if (_dataItem5 == null)
                {
                    _dataItem5 = new ObservableCollection<DataSluLamp5>();

                }
                return _dataItem5;
            }
            set
            {
                if (_dataItem5 == value) return;
                _dataItem5 = value;
                RaisePropertyChanged(() => DataItem5);
            }
        }

        private ObservableCollection<DataSluAssis6> _dataItem6;

        public ObservableCollection<DataSluAssis6> DataItem6
        {
            get
            {
                if (_dataItem6 == null)
                {
                    _dataItem6 = new ObservableCollection<DataSluAssis6>();

                }
                return _dataItem6;
            }
            set
            {
                if (_dataItem6 == value) return;
                _dataItem6 = value;
                RaisePropertyChanged(() => DataItem6);
            }
        }

        #endregion

    }

    //Action
    public partial class PartolViewVm
    {
        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu  .wst_svr_ans_slu_ctrl_measure  , OnSluMeasure,
                typeof (PartolViewVm), this);
        }

        private void OnSluMeasure(string sessionid,Wlst .mobile .MsgWithMobile info)
        {
            if (info == null) return;
            if (_thisViewActive == false) return;
            var data = info.WstSluSvrAnsSluMeasure ;
            if (data == null) return;

            int sluId = data.SluId;
            if (sluId < 1) return;
            if (sluId != CurrentSluId) return;

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  返回数据：";
            if (data.UnknowCtrls != null && data.UnknowCtrls.Count > 0)
            {
                foreach (var g in data.UnknowCtrls)
                {
                    if (g == null) continue;
                    this.DataItem2.Add(new DataSluUnknow2(g, sluId));
                }
                Remind += "未知控制器;";
            }

            if (data.InfoAssis6 != null && data.InfoAssis6.Count > 0)
            {
                foreach (var g in data.InfoAssis6)
                {
                    if (g == null) continue;
                   // int index = 1;
                    foreach (var f in g.LightDataField)
                    {
                        if (f == null) continue;
                        this.DataItem6.Add(new DataSluAssis6(f, sluId, g.CtrlId , g.LeakageCurrent));
                        //index++;
                    }
                }
                Remind += "控制器辅助数据;";
            }

            if (data.InfoPhy4 != null && data.InfoPhy4.Count > 0)
            {
                foreach (var g in data.InfoPhy4)
                {
                    if (g == null) continue;
                    this.DataItem4.Add(new DataSluPhy4(g, sluId, g.CtrlId));
                }
                Remind += "控制器物理数据;";
            }

            if (data.InfoBaseic5 != null && data.InfoBaseic5.Count > 0)
            {
                foreach (var g in data.InfoBaseic5)
                {
                    
                    if (g.Info == null) continue;
                    var dts = new DateTime(g.Info.DateTimeCtrl).ToString("yyyy-MM-dd HH:mm:ss");
                    foreach (var f in g.Items)
                    {
                        if (f == null) continue;
                        this.DataItem5.Add(new DataSluLamp5(f, sluId, g.Info.CtrlId,g.Info .Status ){DateCtrlCreate =dts  });
                    }

                }

                Remind += "控制器数据;";

              
                IndexView = 5;
            }
            Remind += "请手动翻阅。";
        }




    }


    public partial class PartolViewVm
    {

        #region CmdIndex

        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdIndex
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand<string>(ExCmdZcOrSnd, CanCmdZcOrSnd, false)); }
        }

        private void ExCmdZcOrSnd(string str)
        {
            int x = 2;
            try
            {
                var xgt = Convert.ToInt32(str);
                if (xgt == 8)
                {
                    if (IndexView == 2) DataItem2.Clear();
                    if (IndexView == 4) DataItem4.Clear();
                    if (IndexView == 5) DataItem5.Clear();
                    if (IndexView == 6) DataItem6.Clear();

                    return;
                }

                IndexView = xgt;
            }
            catch (Exception ex)
            {

            }

        }

        private bool CanCmdZcOrSnd(string str)
        {
            int x = 2;
            try
            {
                x = Convert.ToInt32(str);
                if (x == 8)
                {
                    if (IndexView == 2) return DataItem2.Count > 0;
                    if (IndexView == 4) return DataItem4.Count > 0;
                    if (IndexView == 5) return DataItem5.Count > 0;
                    if (IndexView == 6) return DataItem6.Count > 0;
                    return false;
                }
                //if (x == IndexView) return false;
                if (x == 2) return DataItem2.Count > 0;
                if (x == 4) return DataItem4.Count > 0;
                if (x == 5) return DataItem5.Count > 0;
                if (x == 6) return DataItem6.Count > 0;

                return false;





            }
            catch (Exception ex)
            {

            }
            return false;
            // return x != lastexutetpara && DateTime.Now.Ticks - lastexute > 30000000;
        }

        #endregion

        //#region CmdReport

        //private ICommand _cmCmdReport;

        //public ICommand CmdReport
        //{
        //    get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand(ExCmdReport, CanCmdReport, false)); }
        //}

        //private void ExCmdReport()
        //{
        //    if (IndexView == 2)
        //    {
        //        var writeinfo = new List<List<object>>();
        //        var titleinfo = new List<object>();

        //        titleinfo.Add("");

        //        var tmp = new List<object>();


        //        writeinfo.Add(tmp);
                

        //        Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
        //    }
        //    else if (IndexView == 4)
        //    {
               
        //    }
        //    else if (IndexView == 5)
        //    {
        //        var writeinfo = new List<List<object>>();
        //        var titleinfo = new List<object>();

        //        titleinfo.Add("物理地址");
               
        //        foreach (var t in lst)
        //        {
        //            var tmp = new List<object>();

        //            tmp.Add(t.PhysicalId);


        //            writeinfo.Add(tmp);
        //        }

        //        Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
        //    }
        //    else if (IndexView == 6)
        //    {

        //    }
        //    else
        //    {
        //        WlstMessageBox.Show("警告", "导出失败！", WlstMessageBoxType.Ok);
        //    }

        //}

        //private bool CanCmdReport()
        //{
        //    return true;
        //}

        //#endregion
    }
}
