using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.SlusglInfoHold.Services;

namespace Wlst.Ux.Wj2096Module.TimeInfoQuery
{
    [Export(typeof (IITimeInfoQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoQueryVm : Wlst.Cr.Core.CoreServices.ObservableObject, IITimeInfoQuery
    {

      //  protected int SluId;

        public void NavOnLoad(params object[] parsObjects)
        {
            try
            {
                if (!parsObjects.Any()) return;
                SluId = Convert.ToInt32(parsObjects[0]);
                OnSelectedRtuChanged(SluId);
            }
            catch (Exception ex)
            {

            }
        }

        public void OnUserHideOrClosing()
        {
            //SluId = 0;
            Items.Clear();
        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "查询时间方案"; }
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

        private void OnSelectedRtuChanged(int sluId)
        {
            SluId = sluId;
            PhyId = "";
            SluName = "";
            var holdinf =
              Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId);
            if (holdinf != null)
            {
               // PhyId = holdinf.PhyId;
                SluName = holdinf.FieldName;

            }

            var tmprs = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId);

            if (tmprs != null) PhyId = tmprs.PhyId.ToString("d4");
  


            Items.Clear();
            foreach (var g in TimeInfos.MySelf.Info.Values)
            {
                var x = "";
                
                    foreach (var f in g.SluCtrls)
                    {
                        if (f.VSluId == sluId && f.OperatorType != 1)
                        {
                            var areaId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(f.VSluId).AreaId;
                            x = x + new TimeInfoOneQueryVm(areaId, g, f).CtrlInfos;
                        }
                    }
                    x = "自定义： " + x;

                

                foreach (var f in g.SluCtrls)
                {
                    if (f.VSluId == sluId)
                    {
                        var areaId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(f.VSluId).AreaId;
                        if (f.OperatorType != 1)
                            Items.Add(new TimeInfoOneQueryVm(areaId, g, f)
                                          {
                                              CtrlInfos = x
                                          });
                        else
                            Items.Add(new TimeInfoOneQueryVm(areaId, g, f));
                        break;
                    }
                }

            }
            if (Items.Count > 0) CurrentSelectedTimeItem = Items[0];
            else CurrentSelectedTimeItem = null;

        }





        private ObservableCollection<TimeInfoOneQueryVm> _childTreeItemsInfo;

        public ObservableCollection<TimeInfoOneQueryVm> Items
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TimeInfoOneQueryVm>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.Items);
                }
            }
        }

        private TimeInfoOneQueryVm _currentselectedvm;

        public TimeInfoOneQueryVm CurrentSelectedTimeItem
        {
            get { return _currentselectedvm; }
            set
            {
                if (value == _currentselectedvm) return;
                _currentselectedvm = value;
                this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
            }
        }


        #region


        private int _sSluId;

        public int SluId
        {
            get { return _sSluId; }
            set
            {
                if (value != _sSluId)
                {
                    _sSluId = value;
                    this.RaisePropertyChanged(() => this.SluId);
                }
            }
        }


        private string _ssdfSluId;

        public string PhyId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }


        private string _sSluName;

        public string SluName
        {
            get { return _sSluName; }
            set
            {
                if (value != _sSluName)
                {
                    _sSluName = value;
                    this.RaisePropertyChanged(() => this.SluName);
                }
            }
        }


        #endregion


        #region CmdClearSluTime

        private DateTime _dtCmdClearSluTime;
        private ICommand _cmdClearSluTime;

        public ICommand CmdClearSluTime
        {
            get { return _cmdClearSluTime ?? (_cmdClearSluTime = new RelayCommand(ExCmdClearSluTime, CanCmdClearSluTime, false)); }
        }



        private void ExCmdClearSluTime()
        {
            _dtCmdClearSluTime = DateTime.Now;
            //this.Items.Clear();
            var info = Sr.ProtocolPhone.LxSluSgl.wst_slusgl_read_time_plan_in_ctrl;
            // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            // info.WstCntOrderWj3090OpenClsoeCenter  = data;
            info.WstVsluTimeRead.SluId = 0;
            SndOrderServer.OrderSnd(info, 10, 2);

        }


        private bool CanCmdClearSluTime()
        {
            return DateTime.Now.Ticks - _dtCmdClearSluTime.Ticks > 30000000;
        }

        #endregion

    }
}
