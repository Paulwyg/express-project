using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.ViewModel
{
    public class OpenCloseReportRtuItem : Wlst.Cr.Core.CoreServices.ObservableObject
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
                    PhyId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                    if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_rtuId))
                    {
                        var sss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[_rtuId];
                        if (sss != null)
                        {
                            this.RtuName = sss.RtuName;
                            PhyId = sss.RtuPhyId;
                        }
                    }
                }
            }
        }

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
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

        private ObservableCollection<NameValueString> _records;


        public ObservableCollection<NameValueString> Records
        {
            get
            {
                if (_records == null)
                {
                    _records = new ObservableCollection<NameValueString>();
                    for (int i = 0; i < 9; i++)
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
