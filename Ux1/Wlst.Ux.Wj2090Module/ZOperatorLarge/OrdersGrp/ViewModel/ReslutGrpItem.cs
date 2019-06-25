using System.Collections.Generic;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.ViewModel
{
    public class ReslutGrpItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public ReslutGrpItem ()
        {
            IsSuccessful = "--";
            ReplyTime = "--";
            IsSuccessful = "--";
            AttachInfo = "--";
        }

        public int sluId;
        public int addrtype;
        public int addr;

        public int OperatorX;


        #region Index

        private int _index;

        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index.Equals(value)) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }


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

                    SluShowId = value + "";
                    SluName = value + "";

                    var sluinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                    if (sluinfo != null)
                    {
                        SluName = sluinfo.RtuName;
                        if (sluinfo.RtuFid  == 0)
                        {
                            SluShowId = sluinfo.RtuPhyId.ToString("D4");
                        }
                        else
                        {
                            var mtps =
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                                    sluinfo.RtuFid );
                            if (mtps != null)
                            {
                                SluShowId = mtps.RtuPhyId.ToString("D4");
                            }

                        }
                    }


                }
            }
        }

        private string _ssdfSluId;

        public string SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
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



        private string _sampleTime;

        public string ReplyTime
        {
            get { return _sampleTime; }
            set
            {
                if (_sampleTime == value) return;
                _sampleTime = value;
                RaisePropertyChanged(() => ReplyTime);
            }
        }


        private string _sampsdfleTime;

        public string IsSuccessful
        {
            get { return _sampsdfleTime; }
            set
            {
                if (_sampsdfleTime == value) return;
                _sampsdfleTime = value;
                RaisePropertyChanged(() => IsSuccessful);
            }
        }

        private string _saattachInfo;

        public string AttachInfo
        {
            get { return _saattachInfo; }
            set
            {
                if (_saattachInfo == value) return;
                _saattachInfo = value;
                RaisePropertyChanged(() => AttachInfo);
            }
        }
    
        #endregion



        private int _indexxx;

        /// <summary>
        /// Nindex
        /// </summary>
        public int Nindex
        {
            get { return _indexxx; }
            set
            {
                if (_indexxx.Equals(value)) return;
                _indexxx = value;
                RaisePropertyChanged(() => Nindex);
            }
        }


        //private string _indexxxx;

        ///// <summary>
        ///// Nindexx 界面呈现
        ///// </summary>
        //public string Nindexx
        //{
        //    get { return _indexxxx; }
        //    set
        //    {
        //        if (_indexxxx.Equals(value)) return;
        //        _indexxxx = value;
        //        RaisePropertyChanged(() => Nindexx);
        //    }
        //}




        private string _saattachInfoXQ;

        public string GrpName
        {
            get { return _saattachInfoXQ; }
            set
            {
                if (_saattachInfoXQ == value) return;
                _saattachInfoXQ = value;
                RaisePropertyChanged(() => GrpName);
            }
        }
        private string _saattachInfo2;

        public string ShowSluId
        {
            get { return _saattachInfo2; }
            set
            {
                if (_saattachInfo2 == value) return;
                _saattachInfo2 = value;
                RaisePropertyChanged(() => ShowSluId);
            }
        }
 
    }
}
