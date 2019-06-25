using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabRtuSet
{
    [Export(typeof (IITreeTabRtuSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeTabRtuSetvM : Wlst.Cr.Core.CoreServices.ObservableObject, IITreeTabRtuSet
    {
        public TreeTabRtuSetvM ()
        {
            InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Requestfirst,3,DelayEventHappen.EventOne );
        }   
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSpe  .wlst_request_or_update_group_class_info    ,//ProtocolCnt.ClientPart.wlst_RecordEvent_server_ans_clinet_request_data,
                Request,
                typeof(TreeTabRtuSetvM), this,true );



        }

        public void Requestfirst()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSpe .wlst_request_or_update_group_class_info ;
            info.WstSpeGroupClassInfo .Op  = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
        }

        public void Request(string session, Wlst.mobile.MsgWithMobile infos)
        {

            if (DateTime.Now.Ticks - dtsnd < 100000000) Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存成功.";

            TreeTabRtuSet.TabRtuHolding.Info.Clear();
            foreach (var f in infos.WstSpeGroupClassInfo.Item)
            {

                if (TabRtuHolding.Info.ContainsKey(f.GrpId)) TabRtuHolding.Info.Remove(f.GrpId);
                var mtpx = new TabRtuInfo() {Id = f.GrpId, Name = f.GrpName};
                mtpx.GrpOrRtus.AddRange(f.RtuOrGrpLst);
                TabRtuHolding.Info.Add(f.GrpId, mtpx);
            }
            TreeTabRtuSet.TabRtuHolding.OnUpdaetAll();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            LoadRtuOrGrpBandingInfo();
            LoadNamx();
        }

        public void OnUserHideOrClosing()
        {
            ChildTreeItems.Clear();
            Names.Clear();
        }

        #region IITab

        public string Title
        {
            get { return string.Format("终端分区管理"); }
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

        private string _msg;
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value == _msg) return;
                _msg = value;
                this.RaisePropertyChanged(() => this.Msg);
            }
        }
    }

    public partial class TreeTabRtuSetvM
    {

        //加载终端节点
        private void LoadRtuOrGrpBandingInfo()
        {
            ChildTreeItems.Clear();
            var ntg = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                       orderby t.Value.AreaId , t.Value.Index
                       select t).ToList();


                foreach (var t in ntg )
                {
                    this.ChildTreeItems.Add(new OneGrpOrRtuSet(t.Value .AreaId , t.Value .GroupId , true));
                }
            
            AddSepcialTmltoTree();

        }


        //加载无分组终端节点
        private void AddSepcialTmltoTree()
        {

            var lstContentTml = new List<int>();


            //if (Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(0))
            //{
            //    var tmp = Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[0].LstGrp;
            //    var atttmp = Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmp);
            //    foreach (var t in atttmp)
            //    {
            //        if (
            //            !Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(t))
            //            continue;
            //        lstContentTml.AddRange(
            //            Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(t));
            //    }
            //}

            //var lstAllSpecial = new List<int>();
            //foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems.Values  )
            //{
            //    if (t.EquipmentType !=WjParaBase.EquType.Rtu  ) continue;
            //    if (!lstContentTml.Contains(t.RtuId)) lstAllSpecial.Add(t.RtuId);
            //}

            var lstAllSpecial = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(0);

            if (lstAllSpecial.Count > 0)
            {
                foreach (var f in lstAllSpecial)
                    this.ChildTreeItems.Add(new OneGrpOrRtuSet(0,f, false));
            }





        }

        private ObservableCollection<OneGrpOrRtuSet> _childTreeItemsInfo;

        public ObservableCollection<OneGrpOrRtuSet> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<OneGrpOrRtuSet>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value == _childTreeItemsInfo) return;
                _childTreeItemsInfo = value;
                this.RaisePropertyChanged(() => ChildTreeItems);
            }
        }



        private void LoadNamx()
        {
            foreach (var f in TabRtuHolding.Info)
                this.Names.Add(new TabNamexInfo(f.Value.Id, f.Value.Name, f.Value.GrpOrRtus));
            SelectGrpByNames();

            if (Names.Count > 0) CurrentSelected = Names[0];
        }

        private ObservableCollection<TabNamexInfo> Namxsdf;

        public ObservableCollection<TabNamexInfo> Names
        {
            get
            {
                if (Namxsdf == null)
                    Namxsdf = new ObservableCollection<TabNamexInfo>();
                return Namxsdf;
            }
            set
            {
                if (value == Namxsdf) return;
                Namxsdf = value;
                this.RaisePropertyChanged(() => Names);
            }
        }

        private TabNamexInfo _currentSelectedx;

        public TabNamexInfo CurrentSelected
        {
            get { return _currentSelectedx; }
            set
            {
                if (value == _currentSelectedx) return;
                if (_currentSelectedx != null) PreOnSelectedChange();
                _currentSelectedx = value;
                this.RaisePropertyChanged(() => CurrentSelected);
                OnSelectedChanged();
            }
        }

        private void PreOnSelectedChange()
        {
            if (_currentSelectedx == null) return;
            var ntx = (from t in ChildTreeItems where t.IsSelected select t.RtuOrGrpId).ToList();

            foreach (var f in Names)
            {
                if (f.Id == CurrentSelected.Id) continue;
                for (int j = f.RtuOrGrps.Count - 1; j >= 0; j--)
                {
                    if (ntx.Contains(f.RtuOrGrps[j])) f.RtuOrGrps.RemoveAt(j);
                }
            }
            _currentSelectedx.RtuOrGrps.Clear();
            _currentSelectedx.RtuOrGrps.AddRange(ntx);

            SelectGrpByNames();
        }


        private void SelectGrpByNames()
        {

            var dicx = new Dictionary<int, string>();
            foreach (var f in Names) foreach (var g in f.RtuOrGrps) if (!dicx.ContainsKey(g)) dicx.Add(g, f.Name);
            foreach (var f in ChildTreeItems)
            {
                if (dicx.ContainsKey(f.RtuOrGrpId)) f.Belong = dicx[f.RtuOrGrpId];
                else f.Belong = "--";
            }
        }

        private void OnSelectedChanged()
        {
            foreach (var f in ChildTreeItems) f.IsSelected = false;
            if (_currentSelectedx == null) return;
            foreach (var f in ChildTreeItems)
                if (_currentSelectedx.RtuOrGrps.Contains(f.RtuOrGrpId))
                    f.IsSelected = true;
        }

        #region 增删改 CmdZsg

        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdZsg
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand<string>(ExCmdZcOrSnd, CanCmdZcOrSnd, true)); }
        }


        private long lastexute = 0;
        private int lastexutetpara = 0;

        private void ExCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            lastexute = DateTime.Now.Ticks;
            lastexutetpara = x;

            if (x == 1) AddNamex();
            if (x == 2) DeleteNamex();
            if (x == 3) Saveall();

        }

        private bool CanCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x == 1 && Names.Count > 4) return false;

            if (x == lastexutetpara)
            {
                return DateTime.Now.Ticks - lastexute > 20000000;
            }
            return true;
            // return x != lastexutetpara && DateTime.Now.Ticks - lastexute > 30000000;
        }

        #endregion


        private void AddNamex()
        {

            if (Names.Count > 4) return;

            int maxid = 1;
            foreach (var f in Names) if (f.Id >= maxid) maxid = f.Id + 1;
            var tmpx = new TabNamexInfo(maxid, "新增", new List<int>());
            Names.Add(tmpx);
            CurrentSelected = tmpx;
        }

        private void DeleteNamex()
        {
            if (CurrentSelected == null) return;
            Names.Remove(CurrentSelected);
            if (Names.Count > 0) CurrentSelected = Names[0];
        }

        private void Saveall()
        {

            PreOnSelectedChange();
           
            var info = Wlst.Sr.ProtocolPhone.LxSpe .wlst_request_or_update_group_class_info ;
            //.ProtocolCnt.ServerPart.wlst_RecordEvent_clinet_request_data;
            info.WstSpeGroupClassInfo.Op = 2;
            foreach (var f in Names)
            {


                var mtpx = new TabRtuInfo() {Id = f.Id, Name = f.Name};
                mtpx.GrpOrRtus.AddRange(f.RtuOrGrps);
                info.WstSpeGroupClassInfo.Item.Add(new RtuGroupClassInfo.RtuGroupClassInfoItem()
                                                                      {
                                                                          GrpName = f.Name,
                                                                          GrpId = f.Id,
                                                                          IsShow = 1,
                                                                          RtuOrGrpLst = f.RtuOrGrps
                                                                      });
            }

            SndOrderServer.OrderSnd(info, 10, 2);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  已经提交保存...";
            dtsnd = DateTime.Now.Ticks;
        }

        private long  dtsnd = DateTime.Now.Ticks;


    }

    public class OneGrpOrRtuSet : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public int AreaId;
        public bool IsGroup;

        public OneGrpOrRtuSet(int areaId, int grpOrRtuId, bool isGrp)
        {
            AreaId = areaId;
            IsGroup = isGrp;

            RtuOrGrpId = grpOrRtuId;
            if (isGrp == false) //rtu
            {
                Msg = "终端";
                var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(grpOrRtuId);
                if (tmp == null)
                {
                    PhyId = RtuOrGrpId + "";
                    RtuOrGrpName = RtuOrGrpId + "";

                }
                else
                {
                    PhyId = tmp.RtuPhyId.ToString("d4");
                    RtuOrGrpName = tmp.RtuName;
                }
            }
            else
            {
                Msg = "组";
                var xg = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(areaId,
                                                                                                            grpOrRtuId);
                if (xg != null)
                {

                    RtuOrGrpName = xg.GroupName;
                    PhyId = xg.GroupId + "";

                }
            }

        }


        #region


        private bool _iphIsSelectedyd;

        public bool IsSelected
        {
            get { return _iphIsSelectedyd; }
            set
            {
                if (_iphIsSelectedyd != value)
                {
                    _iphIsSelectedyd = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }

        private string _iphIsSeBelonglectedyd;

        public string Belong
        {
            get { return _iphIsSeBelonglectedyd; }
            set
            {
                if (_iphIsSeBelonglectedyd != value)
                {
                    _iphIsSeBelonglectedyd = value;
                    this.RaisePropertyChanged(() => this.Belong);
                }
            }
        }

        private string _iphyd;

        public string PhyId
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


        private int _rtuid;

        public int RtuOrGrpId
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid != value)
                {
                    _rtuid = value;
                    this.RaisePropertyChanged(() => this.RtuOrGrpId);
                }
            }
        }


        private string _rtuname;

        public string RtuOrGrpName
        {
            get { return _rtuname; }
            set
            {
                if (_rtuname != value)
                {
                    _rtuname = value;
                    this.RaisePropertyChanged(() => this.RtuOrGrpName);
                }
            }
        }


        private string _rmsg;

        public string Msg
        {
            get { return _rmsg; }
            set
            {
                if (_rmsg != value)
                {
                    _rmsg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }
            }
        }

        #endregion
    }

    public class TabNamexInfo : Wlst.Cr.Core.CoreServices.ObservableObject

    {
        public TabNamexInfo (int id,string name,List< int > xList )
        {
            Id = id;
            Name = name;
            RtuOrGrps = new List<int>();
            RtuOrGrps.AddRange(xList);
        }



        private int _rtuid;

        public int Id
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid != value)
                {
                    _rtuid = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }


        private string _rtuname;

        public string Name
        {
            get { return _rtuname; }
            set
            {
                if (_rtuname != value)
                {
                    _rtuname = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }


        public List<int> RtuOrGrps;


        
    }

}
