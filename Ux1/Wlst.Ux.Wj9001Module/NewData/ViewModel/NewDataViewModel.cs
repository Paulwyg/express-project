using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj9001Module.NewData.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj9001Module.NewData.ViewModel
{
    [Export(typeof(IINewData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataViewModel :Wlst .Cr .Core .CoreServices .ObservableObject , IINewData, Wlst.Cr.CoreMims.CoreInterface.IIShowData 
    {

        public NewDataViewModel ()
        {
            this.InitEvent();
        }
       


    }

    public partial class NewDataViewModel
    {
        private ObservableCollection<LeakLineViewModel> _selectDataItems;

        public ObservableCollection<LeakLineViewModel> LeakLineCollection
        {
            get { return _selectDataItems ?? (_selectDataItems = new ObservableCollection<LeakLineViewModel>()); }
        }
        private string _title; // todo
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    this.RaisePropertyChanged(() => this.Title);
                }
            }
        }
       
        private void ReDrewView()
        {

            LeakLineCollection.Clear();
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(_currentSelectedLeakId);
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_currentSelectedLeakId))
                return;
            var leak = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
[_currentSelectedLeakId] as Sr.EquipmentInfoHolding.Model.Wj9001Leak;
            if (leak == null) return;
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(leak.RtuFid)) return;
            var tml =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[leak.RtuFid] as
                Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (tml == null) return;

            Title = tml.RtuName;
            Title += "    ";
            Title += leak.RtuName;
            if (runninfo == null)
            {
                Title += "    无最新数据 ";
                return;
            }
            DateTime  dc = new DateTime();
            var ntg = (from t in runninfo.LeakLineNewData.Values orderby t.LeakLineId ascending select t).ToList();
            foreach (var item in ntg)
            {
                if (!leak.WjLeakLines.ContainsKey(item.LeakLineId)) continue;
                if (leak.WjLeakLines[item.LeakLineId].IsUsed == false) continue;
                LeakLineCollection.Add(new LeakLineViewModel(item));
                dc = new DateTime(item.DateCreate);// item.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
            }


            Title += "    采集时间： ";
            if(dc == new DateTime())
            {
                Title += "无";
            }else
            {
                Title += dc;
            }
            
        }

    }

    public partial class NewDataViewModel
    {


        private int _currentSelectedLeakId;

        public void InitEvent()
        {
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(),
                                                       FundEventHandlers, FundOrderFilters);
        }

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core &&
                    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    return true;
                }
                if (args.EventType == PublishEventType.Core &&
                    args.EventId == EventIdAssign.RunningInfoUpdate2)
                {
                    var rtuids = args.GetParams()[0] as List<int>;
                    if (rtuids == null) return false ;
                    if (rtuids.Contains(_currentSelectedLeakId) == false) return false ;
                    //if (rtuids.Contains(Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId) == false)
                    //    return false;


                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_currentSelectedLeakId))
                        return false ;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    int rtuid = Convert.ToInt32(args.GetParams()[0]);
                    _currentSelectedLeakId = rtuid;
                    if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLeak(rtuid)) return;

                    ReDrewView();
                    Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                        Wj9001Module  .Services.ViewIdAssign.NewDataViewId);
                }
                if (args.EventId == EventIdAssign.RunningInfoUpdate2)
                {
                    var rtuids = args.GetParams()[0] as List<int>;
                    if (rtuids == null) return;
                    if (rtuids.Contains(_currentSelectedLeakId) == false) return;

                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_currentSelectedLeakId))
                        return;
                    var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
[_currentSelectedLeakId] as Sr.EquipmentInfoHolding.Model.Wj9001Leak;
                    if (tml == null) return;
                    Title = tml.RtuName;
                    ReDrewView();
                    Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                        Wj9001Module.Services.ViewIdAssign.NewDataViewId);
                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("Ldu  showdata error in FundEventHandlers:ex:" + xe);
            }
        }




    }
}
