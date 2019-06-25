using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Setting.SystemInformationViewModel.Services;

namespace Wlst.Ux.Setting.SystemInformationViewModel.ViewModel
{

    [Export(typeof (IISystemInformationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SystemInformationViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IISystemInformationViewModel
    {
         public SystemInformationViewModel()
         {
             this.InitAction();
             ClientTime = DateTime.Now;
             MiddleTime = DateTime.Now.AddDays(1);
             Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("none", 8888, DateTime.Now.Ticks + 5000, 1, Updatetime);
         }

         public void NavOnLoad(params object[] parsObjects)
         {
             updateflg = false;
             ServerIpAddr = Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + "   " + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverPort;
             ClientVer = "V5.1.2 _ 20190322";


             ReqInfo();
         }

         #region tab

         public bool CanClose
         {
             get { return true; }
         }

         public bool CanDockInDocumentHost
         {
             get { return true; }
         }

         public bool CanFloat
         {
             get { return true; }
         }

         public bool CanUserPin
         {
             get { return true; }
         }

         public string Title
         {
             get { return "系统信息"; }
         }

         public int Index
         {
             get { return 1; }
         }


         #endregion

         #region 绑定信息

        /// <summary>
        /// 项目名称
        /// </summary>
         private string _projectname;
         public string ProjectName
         {
             get { return _projectname; }
             set
             {
                 if (value == _projectname) return;
                 _projectname = value;
                 this.RaisePropertyChanged(() => this.ProjectName);
             }
         }

        /// <summary>
        /// 详细地址
        /// </summary>
         private string _projectdeaddr;
         public string ProjectDeAddr
         {
             get { return _projectdeaddr; }
             set
             {
                 if (value == _projectdeaddr) return;
                 _projectdeaddr = value;
                 this.RaisePropertyChanged(() => this.ProjectDeAddr);
             }
         }

        /// <summary>
        /// 联系电话
        /// </summary>
         private string _phonethatprojectcat;
         public string PhoneThatProjectCat
         {
             get { return _phonethatprojectcat; }
             set
             {
                 if (value == _phonethatprojectcat) return;
                 _phonethatprojectcat = value;
                 this.RaisePropertyChanged(() => this.PhoneThatProjectCat);
             }
         }

        /// <summary>
        /// 系统安装日期
        /// </summary>
         private string _dtsysteminstal;
         public string DtSystemInstal
         {
             get { return _dtsysteminstal; }
             set
             {
                 if (value == _dtsysteminstal) return;
                 _dtsysteminstal = value;
                 this.RaisePropertyChanged(() => this.DtSystemInstal);
             }
         }

        /// <summary>
        /// 服务器tv
        /// </summary>
         private string _tvsvr;
         public string TvSvr
         {
             get { return _tvsvr; }
             set
             {
                 if (value == _tvsvr) return;
                 _tvsvr = value;
                 this.RaisePropertyChanged(() => this.TvSvr);
             }
         }


         /// <summary>
         /// 前台机tv
         /// </summary>
         private string _tvcnt1;
         public string TvCnt1
         {
             get { return _tvcnt1; }
             set
             {
                 if (value == _tvcnt1) return;
                 _tvcnt1 = value;
                 this.RaisePropertyChanged(() => this.TvCnt1);
             }
         }

         /// <summary>
         /// 通讯协议版本
         /// </summary>
         private string _verprocom4;
         public string VerProCom4
         {
             get { return _verprocom4; }
             set
             {
                 if (value == _verprocom4) return;
                 _verprocom4 = value;
                 this.RaisePropertyChanged(() => this.VerProCom4);
             }
         }

         /// <summary>
         /// 交互协议版本
         /// </summary>
         private string _verprocnt5;
         public string VerProCnt5
         {
             get { return _verprocnt5; }
             set
             {
                 if (value == _verprocnt5) return;
                 _verprocnt5 = value;
                 this.RaisePropertyChanged(() => this.VerProCnt5);
             }
         }

         /// <summary>
         /// 中间层运行时间
         /// </summary>
         private string _dtmiddlestart;
         public string DtMiddleStart
         {
             get { return _dtmiddlestart; }
             set
             {
                 if (value == _dtmiddlestart) return;
                 _dtmiddlestart = value;
                 this.RaisePropertyChanged(() => this.DtMiddleStart);
             }
         }

         /// <summary>
         /// 客户端运行时间
         /// </summary>
         private string _dtclientstart;
         public string DtClientStart
         {
             get { return _dtclientstart; }
             set
             {
                 if (value == _dtclientstart) return;
                 _dtclientstart = value;
                 this.RaisePropertyChanged(() => this.DtClientStart);
             }
         }
        

         /// <summary>
         /// 客户端版本   主版本号+打包日期
         /// </summary>
         private string _clientVer;
         public string ClientVer
         {
             get { return _clientVer; }
             set
             {
                 if (value == _clientVer) return;
                 _clientVer = value;
                 this.RaisePropertyChanged(() => this.ClientVer);
             }
         }


         /// <summary>
         /// 
         /// </summary>
         private string _serverIpAddr;
         public string ServerIpAddr
         {
             get { return _serverIpAddr; }
             set
             {
                 if (value == _serverIpAddr) return;
                 _serverIpAddr = value;
                 this.RaisePropertyChanged(() => this.ServerIpAddr);
             }
         }


         /// <summary>
         /// 客户端识别码
         /// </summary>
         private string _idf;
         public string Idf
         {
             get { return _idf; }
             set
             {
                 if (value == _idf) return;
                 _idf = value;
                 this.RaisePropertyChanged(() => this.Idf);
             }
         }

         #endregion

         #region 辅助

        private DateTime _clienttime;
        public DateTime ClientTime
         {
             get { return _clienttime; }
             set
             {
                 if (value == _clienttime) return;
                 _clienttime = value;
                 this.RaisePropertyChanged(() => this.ClientTime);
             }
         }

        private DateTime _middletime;
        public DateTime MiddleTime
        {
            get { return _middletime; }
            set
            {
                if (value == _middletime) return;
                _middletime = value;
                this.RaisePropertyChanged(() => this.MiddleTime);
            }
        }

         void Updatetime(object obj)
         {
             if (DateTime.Now > ClientTime)
             {
                 TimeSpan dttime = DateTime.Now - ClientTime;
                 DtClientStart = dttime.Days + "天" + dttime.Hours.ToString("00") + "小时" + dttime.Minutes.ToString("00") + "分" + dttime.Seconds.ToString("00") + "秒";
             }
             else
             {
                 DtClientStart = "未知";
             }

             if (DateTime.Now > MiddleTime)
             {
                 TimeSpan dttime = DateTime.Now - MiddleTime;
                 DtMiddleStart = dttime.Days + "天" + dttime.Hours.ToString("00") + "小时" + dttime.Minutes.ToString("00") + "分" + dttime.Seconds.ToString("00") + "秒";
             }
             else
             {
                 DtMiddleStart = "未知";
             }
         }

        

         #endregion

         private void ReqInfo()
         {
             var info = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_rep_sh;
             info.SysRepSh.Op = 1;
             SndOrderServer.OrderSnd(info, 10, 2);
         }


         #region CmdSave

        private bool updateflg = false;
         private DateTime dtde = DateTime.Now.AddDays(-1);

         private ICommand _cmdCmdSave;

         public ICommand CmdSave
         {
             get { return _cmdCmdSave ?? (_cmdCmdSave = new RelayCommand(ExCmdSave, CanCmdSave, true)); }
         }


         private void ExCmdSave()
         {
             dtde = DateTime.Now;
             var info = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_rep_sh;

             var sysinfo =new client.SysReportToShInfo.RepToSh1();

             sysinfo.ProjectName = ProjectName;
             sysinfo.ProjectDeAddr = ProjectDeAddr;
             sysinfo.PhoneThatProjectCat = PhoneThatProjectCat;
             sysinfo.TvCnt1 = TvCnt1;
             sysinfo.TvSvr = TvSvr;

             info.SysRepSh.Info1 = sysinfo;
             info.SysRepSh.Op = 11;
             SndOrderServer.OrderSnd(info, 10, 2);

             updateflg = true;
         }


         private bool CanCmdSave()
         {
             return DateTime.Now.Ticks - dtde.Ticks > 30000000;
         }

         #endregion

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wst_sys_rep_sh,
                RequestSystemInfo,
                typeof(SystemInformationViewModel), this, true);
        }

        private void RequestSystemInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var data = infos.SysRepSh;
            if (data.Op == 1)
            {
                if (updateflg) WlstMessageBox.Show("提示", "设置信息更新成功。", WlstMessageBoxType.Ok);
                updateflg = false;

                Idf = data.Idf.ToString();
                ProjectDeAddr = data.Info1.ProjectDeAddr;
                ProjectName = data.Info1.ProjectName;
                PhoneThatProjectCat = data.Info1.PhoneThatProjectCat;
                DtSystemInstal = (new DateTime(data.Idf)).ToString("yyyy年MM月dd日");
                TvCnt1 = data.Info1.TvCnt1;
                TvSvr = data.Info1.TvSvr;
                VerProCom4 = "v" + data.Info1.VerProCom4;
                VerProCnt5 = data.Info1.VerProCnt5;
                MiddleTime = new DateTime(data.Info1.DtMiddleStart);
            }


        }
    }
}
