using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.SinglePlan.Services;
using Wlst.Ux.SinglePlan.SinglePlan.View;
using Wlst.iifx;

namespace Wlst.Ux.SinglePlan.SinglePlan.ViewModel
{
    public partial class SluOnePlan : EventHandlerHelperExtendNotifyProperyChanged
    {
        public SluOnePlan(int areaId)
        {
            AreaId = areaId;
            PlanId = 0;
            PlanName = "新方案";
            PlanDesc = "";
            IsUsed = true;
        }

        public SluOnePlan(Wlst.iifx.SluPlanBandingInfo.SluPlanBandingBriefInfo item)
        {
            PlanId = item.PlanId;
            PlanName = item.PlanName;
            PlanDesc = item.TimePlanDesc;
            IsUsed = item.IsUesd;
            PlanTime = new DateTime(wlst.sr.iif.UtcTime.GetCsharpTime(item.DateCreate)).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public SluOnePlan(Wlst.iifx.SluPlanBandingDetailInfo item)
        {
            PlanId = item.PlanId;
            PlanName = item.PlanName;
            PlanDesc = item.PlanDesc;
            IsUsed = item.IsUesd;
            var id = 1;
            foreach (var xx in item.Items)
            {
                Group.Add(new Onegroup
                              {
                                  Id = id++,
                                  SluId = xx.TimePlanCtrlGrpId,
                                  InstructionId = xx.TimePlanId,
                                  SluName = xx.TimePlanCtrlGrpName,
                                  InstructionName = xx.TimePlanName
                              });

            }
            if (Group.Count != 0) CurrentSelectGroup = Group[0];
        }

        private int _planId;

        /// <summary>
        ///方案Id  集中控制器的方案，方案地址由服务器设置，新增的方案地址全部为0提交服务器后服务器分配
        /// </summary>
        public int PlanId
        {
            get { return _planId; }
            set
            {
                if (value != _planId)
                {
                    _planId = value;
                    this.RaisePropertyChanged(() => this.PlanId);
                }
            }
        }

        private string _planName;

        /// <summary>
        /// 方案名称  
        /// </summary>

        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public string PlanName
        {
            get { return _planName; }
            set
            {
                if (value != _planName)
                {
                    _planName = value;
                    this.RaisePropertyChanged(() => this.PlanName);
                }
            }
        }

        [StringLength(30, ErrorMessage = "方案描述长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _planDesc;

        /// <summary>
        /// 方案描述
        /// </summary>
        public string PlanDesc
        {
            get { return _planDesc; }
            set
            {
                if (value != _planDesc)
                {
                    _planDesc = value;
                    this.RaisePropertyChanged(() => this.PlanDesc);
                }
            }
        }


        private bool _isUsed;
        /// <summary>
        /// 使用状态
        /// </summary>
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (value != _isUsed)
                {
                    _isUsed = value;
                    this.RaisePropertyChanged(() => this.IsUsed);
                    
                }
                State = value ? "使用" : "停用";
            }
        }

        private string _state;
        /// <summary>
        /// 使用状态
        /// </summary>
        public string State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    this.RaisePropertyChanged(() => this.State);

                }
            }
        }

        private string _planTime;
        /// <summary>
        /// 生成时间
        /// </summary>
        public string PlanTime
        {
            get { return _planTime; }
            set
            {
                if (value != _planTime)
                {
                    _planTime = value;
                    this.RaisePropertyChanged(() => this.PlanTime);
                }
            }
        }

        private ObservableCollection<Onegroup> _group;

        /// <summary>
        ///分组
        /// </summary>
        public ObservableCollection<Onegroup> Group
        {
            get { return _group ?? (_group = new ObservableCollection<Onegroup>()); }
        }

        private Onegroup _currentSelectGroup;
        /// <summary>
        /// 选中单灯方案
        /// </summary>
        public Onegroup CurrentSelectGroup
        {
            get { return _currentSelectGroup; }
            set
            {
                if (value != _currentSelectGroup)
                {
                    _currentSelectGroup = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectGroup);
                }

            }
        }

        private int _areaId;

        /// <summary>
        ///区域地址
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (value != _areaId)
                {
                    _areaId = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }
    }

    /// <summary>
    /// Icommand
    /// </summary>
    public partial class SluOnePlan
    {
        /// <summary>
        /// 增加分组和指令
        /// </summary>

        #region CmdAddGroup

        public static GroupAndInstruction _groupAndInstruction = null;
        private DateTime _dtCmdAddGroup;
        private ICommand _cmdAddGroup;

        public ICommand CmdAddGroup
        {
            get
            {
                return _cmdAddGroup ??
                       (_cmdAddGroup = new RelayCommand(ExCmdAddGroup, CanCmdAddGroup, true));
            }

        }

        private bool CanCmdAddGroup()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddGroup.Ticks > 10000000;
        }

        private void ExCmdAddGroup()
        {
            _dtCmdAddGroup = DateTime.Now;
            _groupAndInstruction=new GroupAndInstruction();
            _groupAndInstruction.OnBtnOk = OnBtnOk;
            var tvx = new SluOneGroup(AreaId);
            _groupAndInstruction.SetContext(tvx);
            _groupAndInstruction.ShowDialog();
        }


        public void OnBtnOk(SluOneGroup item)
        {
            int instructionId=0, groupId=0;
            string instruction="", group="";
            foreach (var xx in item.InstructionItem)
            {
                if(xx.IsSelected)
                {
                    instructionId = xx.Value;
                    instruction = xx.Name;
                }
            }
            foreach (var xx in item.GroupItem)
            {
                if (xx.IsSelected)
                {
                    groupId = xx.Value;
                    group = xx.Name;
                }
            }
            if(item.Id==0)
            {
                Group.Add(new Onegroup()
                              {
                                  Id = Group.Count+1,
                                  InstructionId = instructionId,
                                  InstructionName = instruction,
                                  SluId = groupId,
                                  SluName = group
                              });
            }
            else
            {
                foreach (var g in Group.Where(g => g.Id == item.Id))
                {
                    g.InstructionId = instructionId;
                    g.InstructionName = instruction;
                    g.SluId = groupId;
                    g.SluName = group;
                }
            }
        }


        #endregion

        /// <summary>
        /// 修改分组和指令
        /// </summary>
        #region CmdModifyGroup

        private DateTime _dtCmdModifyGroup;
        private ICommand _cmdModifyGroup;

        public ICommand CmdModifyGroup
        {
            get
            {
                return _cmdModifyGroup ??
                       (_cmdModifyGroup = new RelayCommand(ExCmdModifyGroup, CanCmdModifyGroup, true));
            }

        }

        private bool CanCmdModifyGroup()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdModifyGroup.Ticks > 10000000;
        }

        private void ExCmdModifyGroup()
        {
            _dtCmdModifyGroup = DateTime.Now;
            _groupAndInstruction = new GroupAndInstruction();
            _groupAndInstruction.OnBtnOk = OnBtnOk;
            var tvx = new SluOneGroup(AreaId,CurrentSelectGroup);
            _groupAndInstruction.SetContext(tvx);
            _groupAndInstruction.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 删除分组和指令
        /// </summary>
        #region CmdDeleteGroup

        private DateTime _dtCmdDeleteGroup;
        private ICommand _cmdDeleteGroup;

        public ICommand CmdDeleteGroup
        {
            get
            {
                return _cmdDeleteGroup ??
                       (_cmdDeleteGroup = new RelayCommand(ExCmdDeleteGroup, CanCmdDeleteGroup, true));
            }

        }

        private bool CanCmdDeleteGroup()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdDeleteGroup.Ticks > 10000000;
        }

        private void ExCmdDeleteGroup()
        {
            _dtCmdDeleteGroup = DateTime.Now;
            Group.Remove(CurrentSelectGroup);
        }

        #endregion

        /// <summary>
        /// 保存方案
        /// </summary>
        #region CmdSavePlan

        private DateTime _dtCmdSavePlan;
        private ICommand _cmdSavePlan;

        public ICommand CmdSavePlan
        {
            get
            {
                return _cmdSavePlan ??
                       (_cmdSavePlan = new RelayCommand(ExCmdSavePlan, CanCmdSavePlan, true));
            }

        }

        private bool CanCmdSavePlan()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSavePlan.Ticks > 10000000;
        }

        private void ExCmdSavePlan()
        {
            _dtCmdSavePlan = DateTime.Now;
            SavePlan();
        }

        #endregion
    }

    public partial class SluOnePlan
    {
        public void SavePlan()
        {
            var req = new SluPlanBandingDetailInfo();
            req.AreaId = AreaId;
            req.PlanId = PlanId;
            req.PlanDesc = PlanDesc;
            req.PlanName = PlanName;
            req.IsUesd = IsUsed;
            req.DateCreate = wlst.sr.iif.UtcTime.GetUtcTime(DateTime.Now.Ticks);
            foreach (var group in Group)
            {
                req.Items.Add(new SluPlanBandingDetailInfo.TimePlanBandingGrpItem()
                                           {
                                               TimePlanCtrlGrpId = group.SluId,
                                               TimePlanId = group.InstructionId
                                           });
            }
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4077", System.Convert.ToBase64String(SluPlanBandingDetailInfo.SerializeToBytes(req)));
            if (data == null) return;
            var res = CommAns.Deserialize(data);
            if (res.Head.IfSt != 1)
            {
                WlstMessageBox.Show("保存失败", "保存失败", WlstMessageBoxType.Ok);
                return;
            }
            WlstMessageBox.Show("保存成功", "保存成功", WlstMessageBoxType.Ok);
            SinglePlanViewModel._addOrModifyPlan.Close();
            var args = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId = EventIdAssign.SavePlanId
            };
            EventPublish.PublishEvent(args);
        }
    }

    public class Onegroup:Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _id;

        /// <summary>
        ///Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private int _sluId;

        /// <summary>
        ///集中器
        /// </summary>
        public int SluId
        {
            get { return _sluId; }
            set
            {
                if (value != _sluId)
                {
                    _sluId = value;
                    this.RaisePropertyChanged(() => this.SluId);
                }
            }
        }

        private string _sluName;

        /// <summary>
        ///集中器名称
        /// </summary>
        public string SluName
        {
            get { return _sluName; }
            set
            {
                if (value != _sluName)
                {
                    _sluName = value;
                    this.RaisePropertyChanged(() => this.SluName);
                }
            }
        }

        private int _instructionId;
        /// <summary>
        ///指令
        /// </summary>
        public int InstructionId
        {
            get { return _instructionId; }
            set
            {
                if (value != _instructionId)
                {
                    _instructionId = value;
                    this.RaisePropertyChanged(() => this.InstructionId);
                }
            }
        }

        private string _instructionName;
        /// <summary>
        ///指令名称
        /// </summary>
        public string InstructionName
        {
            get { return _instructionName; }
            set
            {
                if (value != _instructionName)
                {
                    _instructionName = value;
                    this.RaisePropertyChanged(() => this.InstructionName);
                }
            }
        }
    }
}
