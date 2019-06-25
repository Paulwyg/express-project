using System;
using System.Collections.Generic;
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
using Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingViewModel.ViewModel
{
    [Export(typeof(IIEquipmentFaultManageSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EquipmentFaultManageSettingViewModel : ObservableObject, IIEquipmentFaultManageSettingViewModel
    {
        public EquipmentFaultManageSettingViewModel()
        {

            this.NavOnLoad();

        }


        public const string XmlConfigName = "EquipmentFaultSetting";

        public void NavOnLoad(params object[] parsObjects)
        {

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);

            if (info.ContainsKey("IsShowCQJandDGGH"))
            {
                IsShowCQJandDGGH = info["IsShowCQJandDGGH"].Contains("yes");
            }
            else IsShowCQJandDGGH = false;

            if (info.ContainsKey("IsShowVAPHL"))
            {
                IsShowVAPHL = info["IsShowVAPHL"].Contains("yes");
            }
            else IsShowVAPHL = false;

            if (info.ContainsKey("EnablePaidan"))
            {
                EnablePaidan = info["EnablePaidan"].Contains("yes");
            }
            else EnablePaidan = false;

            EquipemntLightFaultSetting.IsShowCQJandDGGH = IsShowCQJandDGGH;
            EquipemntLightFaultSetting.IsShowVAPHL = IsShowVAPHL;

            IsShowBlackground = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3101, 1, false,"\\SystemColorAndFont");

            IsShowDelFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 1, false);

            IsCalcFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 2, false);

            IsShowCalcFaultDetail = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 3, false);

            IsCopyFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 4, false);

            IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;


        }

        private bool _enablePaidan;

        /// <summary>
        /// 是否启用派单功能
        /// </summary>
        public bool EnablePaidan
        {
            get { return _enablePaidan; }
            set
            {
                if (value != _enablePaidan)
                {
                    _enablePaidan = value;
                    this.RaisePropertyChanged(() => this.EnablePaidan);
                }
            }
        }

        private bool _isShowCQJandDGGH;

        /// <summary>
        /// 分组显示是否显示城区局和灯杆号
        /// </summary>
        public bool IsShowCQJandDGGH
        {
            get { return _isShowCQJandDGGH; }
            set
            {
                if (value != _isShowCQJandDGGH)
                {
                    _isShowCQJandDGGH = value;
                    this.RaisePropertyChanged(() => this.IsShowCQJandDGGH);
                }
            }
        }


        private bool _isShowVAPHL;

        /// <summary>
        /// 分组终端是否显示电压电流上限下限 
        /// </summary>
        public bool IsShowVAPHL
        {
            get { return _isShowVAPHL; }
            set
            {
                if (value != _isShowVAPHL)
                {
                    _isShowVAPHL = value;
                    this.RaisePropertyChanged(() => this.IsShowVAPHL);
                }
            }
        }

        private bool _isShowBlackground;

        /// <summary>
        /// 最新故障界面显示背景色
        /// </summary>
        public bool IsShowBlackground
        {
            get { return _isShowBlackground; }
            set
            {
                if (value != _isShowBlackground)
                {
                    _isShowBlackground = value;
                    this.RaisePropertyChanged(() => this.IsShowBlackground);
                }
            }
        }

        #region IsCalcFault
        private bool _isCalcFault;

        /// <summary>
        /// 历史故障显示统计数据  lvf 2018年5月9日09:37:13
        /// </summary>
        public bool IsCalcFault
        {
            get { return _isCalcFault; }
            set
            {
                if (value != _isCalcFault)
                {
                    _isCalcFault = value;
                    this.RaisePropertyChanged(() => this.IsCalcFault);

                    if (_isCalcFault == false) IsShowCalcFaultDetail = false;
                }
            }
        }
        #endregion


        #region IsCopyFault
        private bool _isCopyFault;

        /// <summary>
        /// 双击故障复制信息到剪贴板  lvf 2018年10月8日09:03:35
        /// </summary>
        public bool IsCopyFault
        {
            get { return _isCopyFault; }
            set
            {
                if (value != _isCopyFault)
                {
                    _isCopyFault = value;
                    this.RaisePropertyChanged(() => this.IsCopyFault);

                }
            }
        }
        #endregion

        #region IsShowCalcFaultDetail
        private bool _isShowCalcFaultDetail;

        /// <summary>
        /// 历史故障下方显示详细故障数据  lvf 2018年5月9日09:37:13
        /// </summary>
        public bool IsShowCalcFaultDetail
        {
            get { return _isShowCalcFaultDetail; }
            set
            {
                if (value != _isShowCalcFaultDetail)
                {
                    _isShowCalcFaultDetail = value;
                    this.RaisePropertyChanged(() => this.IsShowCalcFaultDetail);
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


        #region IsShowDelFault
        private bool _isShowDelFault;

        /// <summary>
        /// 显示删除故障按钮
        /// </summary>
        public bool IsShowDelFault
        {
            get { return _isShowDelFault; }
            set
            {
                if (value != _isShowDelFault)
                {
                    _isShowDelFault = value;
                    this.RaisePropertyChanged(() => this.IsShowDelFault);
                }
            }
        }
        #endregion


        #region SystemName

        private string _backgroundColor;

        public string SystemName
        {
            get { return _backgroundColor; }
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    RaisePropertyChanged(() => this.SystemName);
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

            Dictionary<string, string> info = new Dictionary<string, string>();

            if (IsShowCQJandDGGH) info.Add("IsShowCQJandDGGH", "yes");
            else info.Add("IsShowCQJandDGGH", "no");

            if (IsShowVAPHL) info.Add("IsShowVAPHL", "yes");
            else info.Add("IsShowVAPHL", "no");

            if (EnablePaidan) info.Add("EnablePaidan", "yes");
            else info.Add("EnablePaidan", "no");

            EquipemntLightFaultSetting.IsShowCQJandDGGH = IsShowCQJandDGGH;
            EquipemntLightFaultSetting.IsShowVAPHL = IsShowVAPHL;

            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);

            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();
            dicOp.Add(1, IsShowBlackground ? "1" : "0");
            dicDesc.Add(1, "最新故障界面显示背景色");
            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3101, dicOp, dicDesc, "\\SystemColorAndFont"); 

            var dicOp1 = new Dictionary<int, string>();
            var dicDesc1 = new Dictionary<int, string>();
            dicOp1.Add(1, IsShowDelFault ? "1" : "0");
            dicDesc1.Add(1, "是否显示删除故障按钮");

            dicOp1.Add(2, IsCalcFault ? "1" : "0");
            dicDesc1.Add(2, "历史故障开启统计功能");

            dicOp1.Add(3, IsShowCalcFaultDetail ? "1" : "0");
            dicDesc1.Add(3, "统计时下方显示详细信息");

            dicOp1.Add(4, IsCopyFault ? "1" : "0");
            dicDesc1.Add(4, "双击复制故障信息至剪贴板");

            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3102, dicOp1, dicDesc1);

        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 10000000;
        }

    }
}
