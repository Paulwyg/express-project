using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.iifx;
using Wlst.Ux.SinglePlan.Services;

namespace Wlst.Ux.SinglePlan.SinglePlan.ViewModel
{
    public partial class SluOneGroup : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public SluOneGroup(int areaId)
        {
            AreaId = areaId;
            Id = 0;
            if (GroupItem.Count != 0) GroupItem[0].IsSelected = true;
            if (InstructionItem.Count != 0) InstructionItem[0].IsSelected = true;
        }
        public SluOneGroup(int areaId,Onegroup item)
        {
            AreaId = areaId;
            Id = item.Id;
            foreach (var group in GroupItem)
            {
                if (item.SluId == group.Value) group.IsSelected = true;
            }
            foreach (var instruction in InstructionItem)
            {
                if (item.InstructionId == instruction.Value) instruction.IsSelected = true;
            }
        }
        private ObservableCollection<NameIntBool> _instructionItem;
        /// <summary>
        /// 指令信息
        /// </summary>
        public ObservableCollection<NameIntBool> InstructionItem
        {
            get
            {
                if (_instructionItem == null)
                {
                    _instructionItem = new ObservableCollection<NameIntBool>();
                    var req = new InfoRq();
                    req.AreaId = AreaId;
                    var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1223", System.Convert.ToBase64String(InfoRq.SerializeToBytes(req)));
                    if (data == null) return null;
                    var res = SluPlanBriefInfo.Deserialize(data);
                    foreach (var item in res.Items)
                    {
                        _instructionItem.Add(new NameIntBool()
                                                 {
                                                     Value = item.TimePlanId,
                                                     Name = item.TimePlanName
                                                 });
                    }
                }
                return _instructionItem;
            }
        }

        private ObservableCollection<NameIntBool> _groupItem;
        /// <summary>
        /// 组信息
        /// </summary>
        public ObservableCollection<NameIntBool> GroupItem
        {
            get
            {
                if (_groupItem == null)
                {
                    _groupItem = new ObservableCollection<NameIntBool>();
                    //{
                    //    new NameIntBool {Name = "所有", IsSelected = true, Value = 0},
                    //    new NameIntBool {Name = "下发指令", IsSelected = false, Value = 1},
                    //    new NameIntBool {Name = "设置指令", IsSelected = false, Value = 2},
                    //    new NameIntBool {Name = "设备上传", IsSelected = false, Value = 3}
                    //};
                    var req = new InfoRq();
                    req.AreaId = AreaId;
                    var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("get1221", System.Convert.ToBase64String(InfoRq.SerializeToBytes(req)));
                    if (data == null) return null;
                    var res = SluPlanGrpInfoBk.Deserialize(data);
                    foreach (var item in res.Items)
                    {
                        _groupItem.Add(new NameIntBool()
                                           {
                                               Value = item.GrpId,
                                               Name = item.GrpName
                                           });
                    }

                }
                return _groupItem;
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
    }
}
