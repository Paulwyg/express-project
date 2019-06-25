using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.About.UxShowErr.Sevices;

namespace Wlst.Ux.About.UxShowErr.ViewModel
{
    [Export(typeof(IIUxShowErrModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShowErrViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITab, IIUxShowErrModule
    {



        public void NavOnLoad(params object[] parsObjects)
        {
            if (parsObjects.Length == 3)
            {


                string[] _value = parsObjects[0].ToString().Split('-');
                int cFaultId = 0;
                int cRtuId = 0;

                var dt1 = new DateTime(DateTime.Now.Year, Convert.ToInt32(_value[0]), Convert.ToInt32(_value[1]), 12, 0, 1);
                xTitle = dt1.ToString("yyyy-MM-dd");

                if (Convert.ToInt32(parsObjects[1]) == 1)
                {
                    cFaultId = Convert.ToInt32(parsObjects[2]);
                    cRtuId = -1;

                    string _faultName = "";

                    foreach (var tt in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoList)
                    {
                        if (tt.FaultId == cFaultId)
                        {
                            _faultName = tt.FaultName;
                            break;
                        }
                    }

                    xTitle += "   故障名称：" + _faultName;
                }
                else if (Convert.ToInt32(parsObjects[1]) == 2)
                {
                    cRtuId = Convert.ToInt32(parsObjects[2]);
                    cFaultId = -1;

                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(cRtuId);

                    if (info != null)
                    {
                        xTitle += "   终端名称：" + info.RtuName;
                    }
                }

                Update_Record(dt1, cFaultId, cRtuId);
            }
        }


        public void OnUserHideOrClosing()
        {

        }

        private void Update_Record(DateTime _dt, int _faultID, int _rtuID)
        {
            Records.Clear();


            var d2 = _dt.Ticks;
            var d1 = _dt.AddDays(-1).Ticks;

            if (_faultID != -1)
            {
                var lst =
                    (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                     where t.FaultId == _faultID && t.DateCreate > d1 && t.DateCreate < d2
                     select t).ToList();

                int m = 1;

                foreach (var t in lst)
                {


                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);

                    string _faultName = "";

                    foreach (var tt in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoList)
                    {
                        if (tt.FaultId == t.FaultId)
                        {
                            _faultName = tt.FaultName;
                            break;
                        }
                    }


                    if (info != null)
                    {
                        Records.Add(new EquipmentFaultViewModel
                        {

                            Index = m++,
                            PhyId = info.RtuPhyId,
                            RtuName = info.RtuName,
                            RtuLoopName = info.GetLoopName(t.LoopId),
                            FaultName = _faultName,
                            DtCreateTime = new DateTime(t.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                            DtRemoceTime = new DateTime(t.DateRemove).ToString("yyyy-MM-dd HH:mm:ss")

                        });
                    }
                }
            }

            if(_rtuID != - 1)
            {
                var lst =
                    (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                     where t.RtuId == _rtuID && t.DateCreate > d1 && t.DateCreate < d2
                     select t).ToList();

                int m = 1;

                foreach (var t in lst)
                {


                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);

                    string _faultName = "";

                    foreach (var tt in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoList)
                    {
                        if (tt.FaultId == t.FaultId)
                        {
                            _faultName = tt.FaultName;
                            break;
                        }
                    }


                    if (info != null)
                    {
                        Records.Add(new EquipmentFaultViewModel
                        {

                            Index = m++,
                            PhyId = info.RtuPhyId,
                            RtuName = info.RtuName,
                            RtuLoopName = info.GetLoopName(t.LoopId),
                            FaultName = _faultName,
                            DtCreateTime = new DateTime(t.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                            DtRemoceTime = new DateTime(t.DateRemove).ToString("yyyy-MM-dd HH:mm:ss")

                        });
                    }
                }
            }


        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "故障细节"; }
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

        #region Records

        private ObservableCollection<EquipmentFaultViewModel> _record;

        public ObservableCollection<EquipmentFaultViewModel> Records
        {
            get { return _record ?? (_record = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_record != value)
                {
                    _record = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }
        }

        #endregion

        private string _title;

        public string xTitle
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    this.RaisePropertyChanged(() => this.xTitle);
                }
            }
        }

        #region CmdExport
        private DateTime _dtCmdExport;
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
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("终端地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("故障回路");
                lsttitle.Add("故障名称");
                lsttitle.Add("发生时间");
                lsttitle.Add("消除时间");


                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.RtuLoopName);
                    tmp.Add(g.FaultName);
                    tmp.Add(g.DtCreateTime);
                    tmp.Add(g.DtRemoceTime);


                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            return true;
        }

        #endregion
    }
}
