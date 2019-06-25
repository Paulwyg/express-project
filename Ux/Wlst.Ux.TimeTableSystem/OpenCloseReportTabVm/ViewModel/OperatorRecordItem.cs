using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.ViewModel
{
    public class OpenCloseReportItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {

    
        private int _rtuId;

        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                    var sss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[_rtuId];
                    if (sss != null)
                    {
                        PhyId = sss.RtuPhyId;
                        this.RtuName = sss.RtuName;
                    }
                    else
                    {
                        PhyId = value;
                    }
                }
            }
        }


        private int _rtuphyId;

        public int PhyId
        {
            get { return _rtuphyId; }
            set
            {
                if (value != _rtuphyId)
                {
                    _rtuphyId = value;
                    this.RaisePropertyChanged(() => this.PhyId);

                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private ObservableCollection<NameValueString  > _records;


        public ObservableCollection<NameValueString> Records
        {
            get
            {
                if (_records == null)
                {
                    _records = new ObservableCollection<NameValueString>();
                    for (int i = 0; i < 7; i++)
                        _records.Add(new NameValueString()
                                         {
                                             Name = "--",
                                             Value = "--",

                                         });
                }
                return _records;
            }
        }




    };


}
