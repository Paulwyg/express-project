using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;

namespace Wlst.Ux.Wj2090Module.TimeInfo.TimeInfoQuery
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
              Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId);
            if (holdinf != null)
            {
                if (holdinf.RtuFid  == 0)
                {
                    Is485 = "Gprs通信";
                }
                else
                {
                    Is485 = "485通信" ;
                }
               // PhyId = holdinf.PhyId;
                SluName = holdinf.RtuName;

            }
  
            var tmprs=Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluId);
            
            if(tmprs !=null )PhyId =tmprs .RtuPhyId .ToString( "d4");
  


            Items.Clear();
            foreach (var g in SrInfo.TimeInfos.MySelf.Info.Values)
            {
                foreach (var f in g.SluCtrls)
                {
                    if (f.SluId == sluId)
                    {
                        var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(f.SluId);
               
                        //var areaId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.SluId).AreaId;
                        Items.Add(new TimeInfoOneQueryVm(areaId ,g, f));
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


        private string _sIs485;

        public string Is485
        {
            get { return _sIs485; }
            set
            {
                if (value != _sIs485)
                {
                    _sIs485 = value;
                    this.RaisePropertyChanged(() => this.Is485);
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
            var info = Sr.ProtocolPhone.LxSlu.wst_slu_snd_order;
            // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            // info.WstCntOrderWj3090OpenClsoeCenter  = data;
            info.WstSluSndOrder.SluId = SluId;
            info.WstSluSndOrder.Op = 10;
            SndOrderServer.OrderSnd(info, 10, 2);

        }


        private bool CanCmdClearSluTime()
        {
            return DateTime.Now.Ticks - _dtCmdClearSluTime.Ticks > 30000000;
        }

        #endregion

    }
}
