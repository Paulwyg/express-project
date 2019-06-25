using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Elysium.ThemesSet.Common;
using Microsoft.Practices.Prism;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingLnViewModel.Services;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingLnViewModel.ViewModel
{

    public class NameValueDouble : ObservableObject
    {
    
        private string  _value2;

        public string  Value2
        {
            get { return _value2; }
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.Value2);
                }
            }
        }
    }


    [Export(typeof(IIEquipmentFaultManageSettingLnViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EquipmentFaultManageSettingViewModel : ObservableObject, IIEquipmentFaultManageSettingLnViewModel
    {
        public EquipmentFaultManageSettingViewModel()
        {
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(delayEvent, 1);
            this.NavOnLoad();

           
            

        }

        private void InitAction()
        {
            //todo
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wst_fault_hlbph_level,//.ClientPart.wlst_server_ans_clinet_request_sys_title,
                ActionRcvLnSetting,
                typeof(EquipmentFaultManageSettingViewModel), this);
        }
        /// <summary>
        /// 请求火零不平衡设置
        /// </summary>
        private void delayEvent()
        {
            var xxxinfo = Wlst.Sr.ProtocolPhone.LxFault.wst_fault_hlbph_level;//.ServerPart.wlst_clinet_request_sys_title;
            xxxinfo.WstFaultHlbphLevel.Op = 1;
            SndOrderServer.OrderSnd(xxxinfo, 1, 1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        public void ActionRcvLnSetting(string session, Wlst.mobile.MsgWithMobile infos)
        {

            if (infos.WstFaultHlbphLevel == null) return;
            try
            {
                if (infos.WstFaultHlbphLevel.Op == 1)
                {
                    foreach (var f in HLbphUpper) f.Value2 = "--";

                    var strlst = "";
                    //List<double > tmp = new List<double>();
                    var ntg =
                        (from t in infos.WstFaultHlbphLevel.Levels where t > 0.001 orderby t ascending select t).ToList();
                    for (int i = 0; i < ntg.Count;i++ )
                    {
                        if (i > 9) break;
                        HLbphUpper[i] .Value2 = ntg[i].ToString("f2");
                        strlst = string.IsNullOrEmpty(strlst) ? ntg[i].ToString("f2") + "" : strlst + "," + ntg[i].ToString("f2");
                    }

                       
                    this.HLbphLower = infos.WstFaultHlbphLevel.Axj;
                  //  this.HLbphUpper = tmp;
                    this.HlbphUpdateAlarm = infos.WstFaultHlbphLevel.AupdateDiff;
                    this.HLbphTimer = infos.WstFaultHlbphLevel.TimesCheckBeforAlarm;



                    var dicOp = new Dictionary<int, string>();
                    var dicDesc = new Dictionary<int, string>();
                    dicOp.Add(4, strlst);
                    dicDesc.Add(4, "火零不平衡档位");

                    Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3601, dicOp, dicDesc);
               
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        public const string XmlConfigName = "EquipmentFaultSetting";

        public void NavOnLoad(params object[] parsObjects)
        {
            this.HLbphShieldTimer = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3601, 3, 24);
            IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;

            delayEvent();

            //for (int i = 0; i < 10; i++)
            //{
            //    HLbphUpper.Add(0);
            //}
        }



        #region IsShowHLbph


        private bool _IsShowHLbph;

        public bool IsShowHLbph
        {
            get { return _IsShowHLbph; }
            set
            {
                if (value != _IsShowHLbph)
                {
                    _IsShowHLbph = value;
                    RaisePropertyChanged(() => IsShowHLbph);
                }
            }
        }



        #endregion

        #region HLbphUpper  终端火零不平衡报警电流差值上限

        private ObservableCollection<NameValueDouble> _HLbphUpper=null ;
        /// <summary>
        /// 默认10个
        /// </summary>
        public ObservableCollection<NameValueDouble> HLbphUpper
        {
            get
            {
                if(_HLbphUpper ==null )
                {
                    _HLbphUpper = new ObservableCollection<NameValueDouble>();
                    for (int i = 0; i < 10; i++)
                        _HLbphUpper.Add(new NameValueDouble()
                                            {
                                                Value2 = "--"
                                            });
                }
                return _HLbphUpper;
            }
            set
            {
                if (value != _HLbphUpper)
                {
                    _HLbphUpper = value;
                    RaisePropertyChanged(() => this.HLbphUpper);
                }
            }
        }

        #endregion

        #region HLbphLower 终端火零不平衡消警电流差值下限

        private double _HLbphLower;

        public double HLbphLower
        {
            get { return _HLbphLower; }
            set
            {
                if (value != _HLbphLower)
                {
                    _HLbphLower = value;
                    RaisePropertyChanged(() => this.HLbphLower);
                }
            }
        }

        #endregion

        #region HlbphUpdateAlarm 终端火零不平衡更新报警值

        private double _HlbphUpdateAlarm;

        public double HlbphUpdateAlarm
        {
            get { return _HlbphUpdateAlarm; }
            set
            {
                if (value != _HlbphUpdateAlarm)
                {
                    _HlbphUpdateAlarm = value;
                    RaisePropertyChanged(() => this.HlbphUpdateAlarm);
                }
            }
        }

        #endregion

        #region HLbphTimer 终端火零不平衡检测报警次数

        private int _hLbphTimer;

        public int HLbphTimer
        {
            get { return _hLbphTimer; }
            set
            {
                if (value != _hLbphTimer)
                {
                    _hLbphTimer = value;
                    RaisePropertyChanged(() => this.HLbphTimer);
                }
            }
        }
        #endregion

        #region HLbphShieldTimer 终端火零不平衡应急关灯 屏蔽时间 默认为24小时

        private int _hLbphShieldTimer;

        public int HLbphShieldTimer
        {
            get { return _hLbphShieldTimer; }
            set
            {
                if (value != _hLbphShieldTimer)
                {
                    _hLbphShieldTimer = value;
                    RaisePropertyChanged(() => this.HLbphShieldTimer);
                }
            }
        }
        #endregion


        #region IsD


        private bool _cheIsD;

        public bool IsD
        {
            get { return _cheIsD; }
            set
            {
                if (value != _cheIsD)
                {
                    _cheIsD = value;
                    RaisePropertyChanged(() => IsD);
                }
            }
        }



        #endregion



        private DateTime _dtApply;
        private ICommand _cmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_cmdApply == null) _cmdApply = new RelayCommand(Ex, CanEx, false);
                return _cmdApply;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Ex()
        {
            _dtApply = DateTime.Now;


            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();
            dicOp.Add(3, HLbphShieldTimer + "");
            dicDesc.Add(3, "火零不平衡应急关灯屏蔽时间表时间");




            var lst = new List<double>();
            var strlst = "";
            foreach (var f in HLbphUpper )
            {
                double r = 0;
                if(Double .TryParse( f.Value2 ,out r))
                {
                    if (lst.Contains(r)) continue;
                    lst.Add(r);
                    strlst = string.IsNullOrEmpty(strlst) ? r+"": strlst+","+ r;
                }
            }

            dicOp.Add(4, strlst);
            dicDesc.Add(4, "火零不平衡档位");

            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3601, dicOp, dicDesc);
            //todo
            var xxxinfo = Wlst.Sr.ProtocolPhone.LxFault.wst_fault_hlbph_level;//.ServerPart.wlst_clinet_request_sys_title;
            xxxinfo.WstFaultHlbphLevel.Op = 2;
            xxxinfo.WstFaultHlbphLevel.AupdateDiff = HlbphUpdateAlarm;
            xxxinfo.WstFaultHlbphLevel.Axj = HLbphLower;
            xxxinfo.WstFaultHlbphLevel.Levels = lst ;
            xxxinfo.WstFaultHlbphLevel.TimesCheckBeforAlarm = HLbphTimer;



            SndOrderServer.OrderSnd(xxxinfo, 1, 1);



        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 10000000;
        }

    }
}
