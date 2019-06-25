using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;


namespace Wlst.Ux.Wj2090Module.HisDataQuery.WeekSetQuery.ViewModels
{
    public class WeekSetItemVm : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region attri



        private int _isdfsdfndexsdf;
        public int Index
        {
            get { return _isdfsdfndexsdf; }
            set
            {
                if (_isdfsdfndexsdf == value) return;
                _isdfsdfndexsdf = value;
                RaisePropertyChanged(() => Index);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>

        #region SluId

        private int _indexsdf;

        public int SluId
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => SluId);

                var infos = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                if (infos != null)
                {
                    SluName = infos.RtuName;
                    if(infos .RtuFid  ==0)
                    {
                        SluShowId = infos.RtuPhyId .ToString("D4");
                    }
                   else
                    {
                        var ntg =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                                infos.RtuFid );
                        if(ntg !=null )
                        {
                            SluShowId = ntg.RtuPhyId .ToString("D4");
                        }
                      else   SluShowId =  value.ToString("D7");
                    }
                }
                else
                {
                    if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info.ContainsKey(value) == false) return;
                    var info = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info[value];
                    if (info != null)
                    {

                        SluShowId = info.PhyId+"";// info.FieldId;
                        SluName = info.FieldName;
                    }
                    else
                    {
                        SluShowId = value+"";
                        SluName = "" + value;
                    }
                    //SluShowId = value.ToString("D7");
                    //SluName = "--";
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
        private string  _indsdfsdfdf;

        public string  SluName
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => SluName);
            }
        }
        private string   _index;

        public string CtrlId
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => CtrlId);
            }
        }

        #endregion


        private string _lDateCreate;

        public string DateCreate
        {
            get { return _lDateCreate; }
            set
            {
                if (_lDateCreate == value) return;
                _lDateCreate = value;
                RaisePropertyChanged(() => DateCreate);
            }
        }


        private string _lDateReply;

        public string DateReply
        {
            get { return _lDateReply; }
            set
            {
                if (_lDateReply == value) return;
                _lDateReply = value;
                RaisePropertyChanged(() => DateReply);
            }
        }


        private string _liUserName;

        public string UserName
        {
            get { return _liUserName; }
            set
            {
                if (_liUserName == value) return;
                _liUserName = value;
                RaisePropertyChanged(() => UserName);
            }
        }


        private string _lRemark;

        public string Remark
        {
            get { return _lRemark; }
            set
            {
                if (_lRemark == value) return;
                _lRemark = value;
                RaisePropertyChanged(() => Remark);
            }
        }

        private int _lNIndex;

        public int NIndex
        {
            get { return _lNIndex; }
            set
            {
                if (_lNIndex == value) return;
                _lNIndex = value;
                RaisePropertyChanged(() => NIndex);
            }
        }
        #endregion

        public WeekSetItemVm(Wlst .client .SluWeekSetRecord .SluWeekSetRecordItem   info)
        {
            //var infos = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(info.SluId);
            //if (infos != null) SluName = infos.RtuName;
            SluId = info.SluId;
            if (info.CtrlId == 0) CtrlId = "--";
            else
            {
              //  Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(info.SluId, info.CtrlId);
                if (SluId > 1700000 && SluId < 1800000)
                {
                    var infos = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(info.CtrlId);
                    if (infos != null)
                    {

                        CtrlId = infos.OrderId + "";// info.FieldId;
                        SluName = infos.CtrlName;  //显示 控制器名称  lvf  2019年2月22日10:57:35
                    }
                    else
                    {
                        CtrlId = info.CtrlId + "";
                   
                    }
                   
                }
                else
                {
                     CtrlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(info.SluId, info.CtrlId).ToString("D2");
                     if (CtrlId == "00") CtrlId = "--";
                }
               
            }
            NIndex = info.NIndex;
            UserName = info.UserName;
            Remark = info.Remark;
            DateCreate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            DateReply = info.DateReply == 0 ? "--" : new DateTime(info.DateReply).ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
