using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.DataValidation;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.ExtendYixinEsu.JnRtuSet
{
    [Export(typeof (IIJnRtuSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class JnRtuSetVm : Wlst.Cr.Core.CoreServices.ObservableObject, IIJnRtuSet
    {

        public JnRtuSetVm()
        {
            InitAction();
        }

        private bool _isViewShow = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            Isaveed = false;
            _isViewShow = true;
            InitItems();
        }

        public void OnUserHideOrClosing()
        {

            _isViewShow = false;

            if (JnDataQuery.JnQueryVm._myself != null && Isaveed)
                JnDataQuery.JnQueryVm._myself.OnRtuSetOver(); // = !JnDataQuery.JnQueryVm.IsRtuSetOver;
        }

        #region IITab

        public string Title
        {
            get { return string.Format("节能终端设置"); }
        }

        public int Index
        {
            get { return 1; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion
    }

    public partial class JnRtuSetVm
    {
        private void InitItems()
        {
            Items.Clear();
            ItemsCatlog.Clear();

            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .Values )
            {
                if (t.EquipmentType !=WjParaBase.EquType.Rtu ) continue;
 
                Items.Add(new RtuItem()
                              {
                                  IsSelectd = false,
                                  RtuId = t.RtuId ,
                                  RtuPhyId = t.RtuPhyId,
                                  RtuShowId = t.RtuPhyId .ToString("D4"),
                                  RtuShowName = t.RtuName
                              });
            }
            int index = 1;
            foreach (var t in Sr.RtuBelongDataHold.MySlef.Info)
            {
                ItemsCatlog.Add(new RtuCatlog(index++, t.Key, t.Value));
            }

            if (ItemsCatlog.Count > 0) CurrentSelectRule = ItemsCatlog[0];
        }



        private ObservableCollection<RtuItem> _items;

        public ObservableCollection<RtuItem> Items
        {
            get
            {
                if (_items == null) _items = new ObservableCollection<RtuItem>();
                return _items;
            }
        }

        private ObservableCollection<RtuCatlog> _itemItemsRuless;

        public ObservableCollection<RtuCatlog> ItemsCatlog
        {
            get
            {
                if (_itemItemsRuless == null) _itemItemsRuless = new ObservableCollection<RtuCatlog>();
                return _itemItemsRuless;
            }
        }

        private RtuCatlog _rCurrentSelectRule;

        public RtuCatlog CurrentSelectRule
        {
            get { return _rCurrentSelectRule; }
            set
            {
                if (value == _rCurrentSelectRule) return;
                UpdateCurent();
                _rCurrentSelectRule = value;
                this.RaisePropertyChanged(() => this.CurrentSelectRule);
                UpdateShow();
            }
        }

        private void UpdateCurent()
        {
            if (CurrentSelectRule == null) return;
            CurrentSelectRule.Rtus.Clear();
            CurrentSelectRule.Rtus = (from t in Items where t.IsSelectd select t.RtuPhyId).ToList();
            CurrentSelectRule.Xcount = CurrentSelectRule.Rtus.Count;

        }

        private void UpdateShow()
        {
            if (CurrentSelectRule == null)
            {
                foreach (var t in Items)
                {
                    t.IsSelectd = false;
                }
                return;
            }

            foreach (var t in Items)
            {
                t.IsSelectd = CurrentSelectRule.Rtus.Contains(t.RtuPhyId);
            }
        }
    }



    public partial class JnRtuSetVm
    {
        private const string CmdInfo = "request.yxjn.rtus";

        private void RequstInfoFromOtherClient()
        {

            var info = Wlst.Sr.ProtocolPhone .LxSys  .wlst_request_exchange_data_cnt_self  ;//.ServerPart.wlst_svr_clinet_exchang_info_self;
            info.WstSysRequestExchangeInfoSelf.CmdInfo = CmdInfo;
            info.WstSysRequestExchangeInfoSelf.Indentify = 1;
            SndOrderServer.OrderSnd(info);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在从其他客户端请求终端设置信息...";
        }


        private void Save()
        {
            var dic = new Dictionary<string, List<int>>();
            foreach (var g in ItemsCatlog)
            {
                if (!string.IsNullOrEmpty(g.NewName) && g.Rtus.Count > 0 && !dic.ContainsKey(g.NewName))
                    dic.Add(g.NewName, g.Rtus);
            }
            Sr.RtuBelongDataHold.MySlef.UpdateRtus(dic, DateTime.Now.Ticks);



            var nt = Wlst.Sr.ProtocolPhone .LxSys  .wlst_request_exchange_data_cnt_self  ;//.ServerPart.wlst_svr_clinet_exchang_info_self;
            var strs = "";
            nt.WstSysRequestExchangeInfoSelf .Indentify = 2;
            nt.WstSysRequestExchangeInfoSelf .CmdInfo = CmdInfo;
            nt.WstSysRequestExchangeInfoSelf .Data1 = Sr.RtuBelongDataHold.MySlef.LastUpdateTime + "";
            foreach (var g in Sr.RtuBelongDataHold.MySlef.Info)
            {
                strs += g.Key + "*";
                foreach (var f in g.Value) strs += f + "-";
                strs += "#";
            }
            nt.WstSysRequestExchangeInfoSelf .Data2 = strs;
            SndOrderServer.OrderSnd(nt);

            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存成功.";
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSys  .wlst_request_exchange_data_cnt_self ,//.ClientPart.wlst_svr_to_clinet_exchang_info_self,
                OnRcvSvrSelfInfo,
                typeof (JnRtuSetVm), this);
        }

        private void OnRcvSvrSelfInfo(string session,Wlst .mobile .MsgWithMobile  infos)
        {

            if (infos == null || infos.WstSysRequestExchangeInfoSelf   == null) return;
            if (infos.WstSysRequestExchangeInfoSelf .CmdInfo != CmdInfo) return;
            if (infos.WstSysRequestExchangeInfoSelf .Indentify == 1) //其他客户端请求
            {
                if (Sr.RtuBelongDataHold.MySlef.LastUpdateTime == 0) return;

                var nt = Wlst.Sr.ProtocolPhone .LxSys  .wlst_request_exchange_data_cnt_self ;//.ServerPart.wlst_svr_clinet_exchang_info_self;
                var strs = "";
                nt.WstSysRequestExchangeInfoSelf .Indentify = 2;
                nt.WstSysRequestExchangeInfoSelf .CmdInfo = CmdInfo;
                nt.WstSysRequestExchangeInfoSelf .Data1 = Sr.RtuBelongDataHold.MySlef.LastUpdateTime + "";
                foreach (var g in Sr.RtuBelongDataHold.MySlef.Info)
                {
                    strs += g.Key + "*";
                    foreach (var f in g.Value) strs += f + "-";
                    strs += "#";
                }
                nt.WstSysRequestExchangeInfoSelf .Data2 = strs;
                SndOrderServer.OrderSnd(nt);
                return;
            }
            else
            {
                if (infos.WstSysRequestExchangeInfoSelf .Data1 == null || infos.WstSysRequestExchangeInfoSelf .Data2 == null) return;
                long gx = 0;
                Int64.TryParse(infos.WstSysRequestExchangeInfoSelf .Data1, out gx);
                if (gx <= Sr.RtuBelongDataHold.MySlef.LastUpdateTime) return;
                var sps = infos.WstSysRequestExchangeInfoSelf .Data2.Split('#');
                var dic = new Dictionary<string, List<int>>();
                foreach (var t in sps)
                {
                    var nps = t.Split('*');
                    if (nps.Count() < 2) continue;
                    var names = nps[0].Trim();
                    var lst = new List<int>();
                    var gps = nps[1].Split('-');
                    foreach (var gg in gps)
                    {
                        int r = 0;
                        if (Int32.TryParse(gg, out r))
                        {
                            lst.Add(r);
                        }
                    }
                    if (!string.IsNullOrEmpty(names) && lst.Count > 0 && !dic.ContainsKey(names)) dic.Add(names, lst);
                }
                if (dic.Count == 0) return;
                Sr.RtuBelongDataHold.MySlef.UpdateRtus(dic, gx);
                if (_isViewShow)
                {
                    ItemsCatlog.Clear();
                    int index = 1;
                    foreach (var t in Sr.RtuBelongDataHold.MySlef.Info)
                    {
                        ItemsCatlog.Add(new RtuCatlog(index++, t.Key, t.Value));
                    }

                    if (ItemsCatlog.Count > 0) CurrentSelectRule = ItemsCatlog[0];
                    Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  接受到来自其他客户端的更新的设置信息，界面同步更新...";
                }


            }

        }




        #region CmdAddCatalog

        private ICommand _cCmdCmdUpdateCurrent;

        public ICommand CmdAddCatalog
        {
            get
            {
                return _cCmdCmdUpdateCurrent ??
                       (_cCmdCmdUpdateCurrent = new RelayCommand(ExCmdUpdateCurrent, CanCmdUpdateCurrent, true));
            }
        }


        private void ExCmdUpdateCurrent()
        {

            ItemsCatlog.Add(new RtuCatlog(ItemsCatlog.Count + 1, "新厂家", new List<int>()));
        }

        private bool CanCmdUpdateCurrent()
        {
            return true;
        }

        #endregion


        #region CmdDeleteCatalog

        private ICommand _cCmdCmdAddRule;

        public ICommand CmdDeleteCatalog
        {
            get { return _cCmdCmdAddRule ?? (_cCmdCmdAddRule = new RelayCommand(ExCmdAddRule, CanCmdAddRule, true)); }
        }


        private void ExCmdAddRule()
        {

            if (ItemsCatlog.Contains(CurrentSelectRule)) ItemsCatlog.Remove(CurrentSelectRule);

        }


        private bool CanCmdAddRule()
        {
            return CurrentSelectRule != null && ItemsCatlog.Count > 1;
        }

        #endregion


        #region CmdSave

        private ICommand _cCmdDeleteRules;

        public ICommand CmdSave
        {
            get
            {
                return _cCmdDeleteRules ??
                       (_cCmdDeleteRules = new RelayCommand(ExCmdDeleteRules, CanCmdDeleteRules, true));
            }
        }

        private bool Isaveed = false;
        private void ExCmdDeleteRules()
        {
            Isaveed = true;

            dtde = DateTime.Now;
            UpdateCurent();
            this.Save(); //  this.ItemsRules.Remove(CurrentSelectRule);

        }

        private DateTime dtde = DateTime.Now.AddDays(-1);

        private bool CanCmdDeleteRules()
        {
            return DateTime.Now.Ticks - dtde.Ticks > 30000000 && ItemsCatlog.Count > 0;
        }

        #endregion

        #region CmdRequestInfoFromOtrCnt

        private ICommand _cCmdUpdate;

        public ICommand CmdRequestInfoFromOtrCnt
        {
            get { return _cCmdUpdate ?? (_cCmdUpdate = new RelayCommand(ExCmdUpdate, CanCmdUpdate, true)); }
        }

        private DateTime _dtUpdate = DateTime.Now.AddDays(-1);

        private void ExCmdUpdate()
        {
            _dtUpdate = DateTime.Now;

            this.RequstInfoFromOtherClient();
        }

        private bool CanCmdUpdate()
        {
            return DateTime.Now.Ticks - _dtUpdate.Ticks > 200000000;
        }

        #endregion

        private string newName;
        public string Remark
        {
            get { return newName; }
            set
            {
                if (value == newName) return;
                newName = value;
                this.RaisePropertyChanged(() => this.Remark);
            }
        }

    }

    public class RtuItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int xxIndex;
        private string xxAlow;
        private string xxAmax;
        private bool xxLowTimes;

        public int RtuPhyId;

        public int RtuId
        {
            get { return xxIndex; }
            set
            {
                if (value == xxIndex) return;
                xxIndex = value;
                this.RaisePropertyChanged(() => this.RtuId);
            }
        }

        public string RtuShowId
        {
            get { return xxAlow; }
            set
            {
                if (value == xxAlow) return;
                xxAlow = value;
                this.RaisePropertyChanged(() => this.RtuShowId);
            }
        }

        public string RtuShowName
        {
            get { return xxAmax; }
            set
            {
                if (value == xxAmax) return;
                xxAmax = value;
                this.RaisePropertyChanged(() => this.RtuShowName);
            }
        }

        public bool IsSelectd
        {
            get { return xxLowTimes; }
            set
            {
                if (value == xxLowTimes) return;
                xxLowTimes = value;
                this.RaisePropertyChanged(() => this.IsSelectd);
            }
        }


    }

    public class RtuCatlog : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int xxIndex;
        public string OldName;
        private string newName;
        public List<int> Rtus;

        public RtuCatlog(int index, string name, List<int> rtus)
        {
            Index = index;
            OldName = name;
            NewName = name;
            Rtus = new List<int>();
            Rtus.AddRange(rtus);
            Xcount = Rtus.Count;
        }


        public int Index
        {
            get { return xxIndex; }
            set
            {
                if (value == xxIndex) return;
                xxIndex = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }

    [StringLength(30,ErrorMessage="输入名称不能大于30个字符" )]
    [Required(ErrorMessage = "[厂家名称]内容不能为空！")]  
        public string NewName
        {
            get { return newName; }
            set
            {
                if (value == newName) return;
                newName = value;
                this.RaisePropertyChanged(() => this.NewName);
            }
        }

        private int Xcoun;
        public int Xcount
        {
            get { return Xcoun; }
            set
            {
                if (value == Xcoun) return;
                Xcoun = value;
                this.RaisePropertyChanged(() => this.Xcount);
            }
        }

    }

}
