using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using DragDropExtend.DragAndDrop;
using DragDropExtend.ExtensionsHelper;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Models;
using Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Services;
using Wlst.Ux.EquipmentGroupManage.Models;
using Wlst.Ux.EquipmentGroupManage.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.ViewModel
{
    [Export(typeof (IIGrpMulitManageView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class GrpMulitManageViewModel : VmEventActionProperyChangedBase, IIGrpMulitManageView
    {
 
        //private EventSubScriptionTokener _eventSubScriptionTokener;

        public GrpMulitManageViewModel()
        {
            Visi = Visibility.Hidden;
            Title = "本地分组";
            IsAreaMulti = false ;
            InitEvent();
            _ChildTreeItems = new ObservableCollection<TreeItemViewModel>();
            _itemsRtu = new ObservableCollection<RtuItem>();
            path = new Dictionary<int, List<Tuple<int, string>>>();
        }

        #region IEventAggregator Subscription

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            
            if (args.EventId == PublishEventTypeLocal.RcvGrpInfofromServerAndNeedUpdateGroupInfo)
            {
                this.NavOnLoad();
            }
            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.MulityInfoGroupAllNeedUpdate)
            {
                if (DateTime.Now.Ticks - dtSnd.Ticks < 600000000)
                {

                    Msg = DateTime.Now + " 更新成功!";
                }
                else
                {
                    Msg = DateTime.Now + " 收到更新数据!";
                }
                this.LoadItems();
            }
            if (args.EventType == PublishEventTypeLocal.Name &&
                args.EventId == PublishEventTypeLocal.DeleteGrpPathByCommandBasicShowGroupManage)
            {
                int id = -1;
                int grpId = -1;
                int treeId = -1;
                string  grpName = "";
                try
                {
                    grpName = Convert.ToString(args.GetParams()[2]);
                    foreach(var t in ChildTreeItems)
                    {
                        if (t.NodeName == grpName) treeId = t.NodeId;
                    }
                    id = Convert.ToInt32(args.GetParams()[0]);
                    grpId = Convert.ToInt32(args.GetParams()[1]);
                }
                catch (Exception ex)
                {
                    return;
                }
                if (id == -1 || grpId==-1)
                {
                    return;
                }
 

                DeletePath(id, grpId);
                foreach (var t in _ChildTreeItems)
                {
                    t.DeleteTmlNodeByFather(id, treeId);
                }
            }
        }
        

        void InitEvent()
        {
            this.AddEventFilterInfo(0, PublishEventTypeLocal.Name);
            this.AddEventFilterInfo(
                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.MulityInfoGroupAllNeedUpdate, "Core.CoreServices.PublishEventArgs.Core", true);
        }

        #endregion
        private DateTime dtSnd;
        private int AddMaxId = 1;
        //public event EventHandler AreaVisiChanged;
        public override  void NavOnLoadr(params object[] parsObjects)
        {
            Msg = " ";
            this.ItemsArea.Clear();
            if (UserInfo.UserLoginInfo.D == true)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count > 1)
                {
                    this.ItemsArea.Add(new NameValueInt()
                                           {
                                               Name = "全部区域",
                                               Value = -1
                                           });
                }
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                   this.ItemsArea.Add(new NameValueInt()
                    {
                        Name = f.Key.ToString("d2") + "-" + f.Value.AreaName,
                        Value = f.Key
                    }); 
                }
                

            }
            else
            {
                List<int> areaLst = new List<int>();
                areaLst.AddRange(UserInfo.UserLoginInfo.AreaX);
                foreach (var t in UserInfo.UserLoginInfo.AreaW)
                {
                    if (!areaLst.Contains(t))
                    {
                        areaLst.Add(t);
                    }
                }
                if (areaLst.Count > 1)
                {
                    this.ItemsArea.Add(new NameValueInt()
                                           {
                                               Name = "全部区域",
                                               Value = -1
                                           });
                }

                foreach (var f in areaLst)
                {
                    var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(f);
                    this.ItemsArea.Add(new NameValueInt()
                    {
                        Name = areaInfo.AreaId.ToString("d2") + "-" + areaInfo.AreaName,
                        Value = areaInfo.AreaId
                    });
                }
                
            }

            if (ItemsArea.Count > 0) CurrentSelectArea = ItemsArea[0];
            Visi = ItemsArea.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            if (ItemsArea.Count > 1) IsAreaMulti = true;
            else IsAreaMulti = false;

            LoadItems();
        }

        public override void OnUserHideOrClosingr()
        {
            this.ChildTreeItems.Clear();
            _itemsRtu.Clear();
            //base.OnUserHideOrClosingr();
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _fItemsArea;
        /// <summary>
        /// 区域选择combobox
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> ItemsArea
        {
            get
            {
                if (_fItemsArea == null)
                {
                    _fItemsArea = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _fItemsArea;
            }
            set
            {
                if (value == _fItemsArea) return;
                _fItemsArea = value;
                this.RaisePropertyChanged(() => ItemsArea);
            }
        }

        private Visibility _txtVisi;

        /// <summary>
        /// 若该用户能管理多个区域，则显示区域选择栏
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        private string _msg;

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }
            }
        }

        private Wlst.Cr.CoreOne.Models.NameValueInt _cur;
        public Wlst.Cr.CoreOne.Models.NameValueInt CurrentSelectArea
        {
            get { return _cur; }
            set
            {
                if (value == _cur) return;
                _cur = value;
                this.RaisePropertyChanged(() => this.CurrentSelectArea);
                LoadItems();

            }
        }

        public void LoadItems()
        {
            int maxidx = 1; 
            _ChildTreeItems.Clear();
            //添加根节点到树中
            var grps = new List<GroupInformation>();
            if (CurrentSelectArea == null) return;           
            if (CurrentSelectArea.Value != -1)
            {
                var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(CurrentSelectArea.Value);
                if (area == null) return;
            }

            grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GrpInfoList(CurrentSelectArea.Value);


            if (grps.Count > 0)
            {
                var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
                foreach (var f in grpsTmp)
                {
                   
                    this.AddChild(f);

                }
                //对分组子节点 进行数据加载
                foreach (var t in ChildTreeItems)
                {
                    if (t.NodeType != TreeNodeType.IsGrp) continue;
                    
                    t.AddChild();
                }
            }

            LoadRtus();
            ReLoadTmlsRelfectGrps();

            foreach (var f in this.ChildTreeItems)
            {
                if (f.NodeId >= maxidx) maxidx = f.NodeId + 1;
                f.GetChildRtuCount();
            }
            AddMaxId = maxidx;
        }


 
        #region tree

        public string HeaderInfo
        {
            get { return "Terminal"; }
        }

        #region TreeView

        public void AddChild(GroupItemsInfo.GroupItem gi)
        {
            if(CurrentSelectArea==null) return;
            var t = new TreeItemViewModel(null, CurrentSelectArea.Value,gi, true, this.ChildTreeItems);
            ChildTreeItems.Add(t);
        }

        private ObservableCollection<TreeItemViewModel> _ChildTreeItems;

        public ObservableCollection<TreeItemViewModel> ChildTreeItems
        {
            get { return _ChildTreeItems; }
        }

        #endregion

        #region TreeView 事件

        public void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
        }

        public void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = VisualTreeExtensions.GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        /// <summary>
        /// 获取菜单树当前选择的菜单
        /// </summary>
        /// <returns></returns>
        public string GetSelectMVVMPath()
        {
            for (int i = 0; i < _ChildTreeItems.Count; i++)
            {
                string strReturn = _ChildTreeItems[i].GetSelectMVVMPath("");
                if (!strReturn.Equals(""))
                {
                    return strReturn;
                }
            }

            return "";
        }

        /// <summary>
        /// 获取菜单树当前选择的菜单
        /// </summary>
        /// <returns></returns>
        public TreeItemViewModel GetSelectMvvm()
        {
            for (int i = 0; i < ChildTreeItems.Count; i++)
            {
                var strReturn = ChildTreeItems[i].GetSelectMvvm();
                if (strReturn != null)
                {
                    return strReturn;
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region ListView

        private ObservableCollection<RtuItem> _itemsRtu;

        public ObservableCollection<RtuItem> ItemsRtu
        {
            get { return _itemsRtu; }
        }

        private RtuItem _currentListViewItem;

        public RtuItem CurrentListViewItem
        {
            get
            {
                if (_currentListViewItem == null) _currentListViewItem = new RtuItem();
                return _currentListViewItem;
            }
            set
            {
                if (value != _currentListViewItem)
                {
                    _currentListViewItem = value;
                    this.RaisePropertyChanged(() => this.CurrentListViewItem);
                }
            }
        }

        private bool _isAreaMulti;

        public bool IsAreaMulti
        {
            get { return _isAreaMulti; }
            set
            {
                if (_isAreaMulti != value)
                {
                    _isAreaMulti = value;
                    this.RaisePropertyChanged(() => this.IsAreaMulti);
                    //if (CompareVisiChanged != null) CompareVisiChanged(value, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 用来保存每个终端所有归属显示分组的路径
        /// </summary>
        private Dictionary<int, List<Tuple< int ,string >>> path;

        /// <summary>
        /// 加载终端信息到_tmlsRelfectGrps中
        /// 并将终端的归属分组路径进行计算
        /// </summary>
        private void LoadRtus()
        {
            path.Clear();

            ItemsRtu.Clear();
            string type = "";
            var tmpLst = new List<RtuItem>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.RtuFid != 0) continue;
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;

                int id = t.Value.RtuId;
                string name = t.Value.RtuName;
                var tmlReflectItem = new RtuItem();
                tmlReflectItem.ID = id;
                tmlReflectItem.PhyId = t.Value.RtuPhyId;
                if (t.Value.RtuModel == EnumRtuModel.Wj3090) type = "3090终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj3005) type = "3005终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj3006) type = "3006终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj2090) type = "单灯设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1080) type = "光控设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1050) type = "电表设备";
                else if (t.Value.RtuModel == EnumRtuModel.Jd601) type = "节电设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1090) type = "线路检测";
                else if (t.Value.RtuModel == EnumRtuModel.Wj4005) type = "4005终端";
                tmlReflectItem.IsSelected = false;
                tmlReflectItem.Name = name;
                tmlReflectItem.Type = type;
                tmlReflectItem.AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(id);
                tmlReflectItem.Area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(tmlReflectItem.AreaId).AreaName;
                
                
                tmlReflectItem.PathCounte = 0;
                tmlReflectItem.OnCmdWatchDetailInfo += OnCmdWatchDetailInfo;
                tmlReflectItem.BtnDetailVisible = Visibility.Collapsed;
                if (path.ContainsKey(id))
                {
                    tmlReflectItem.PathCounte = path[id].Count;
                    foreach (var strPath in path[id])
                    {
                        tmlReflectItem.PathItemsforthistml.Add(new PathItem() {GrpName  = strPath.Item2 ,GrpId =strPath .Item1 , Id = id});
                    }
                    string temp = "";
                    foreach (var item in tmlReflectItem.PathItemsforthistml)
                    {
                        temp += item.GrpName;
                    }
                    if (temp.Length > 20)
                    {
                        tmlReflectItem.BtnDetailVisible = Visibility.Visible;
                    }
                    else
                    {
                        tmlReflectItem.BtnDetailVisible = Visibility.Collapsed;
                    }
                    if (tmlReflectItem.PathItemsforthistml.Count > 0)
                        tmlReflectItem.CurrentItem = tmlReflectItem.PathItemsforthistml[0];
                }
                if (tmlReflectItem.AreaId == this.CurrentSelectArea.Value)
                {
                   // tmpLst.Add(tmlReflectItem);
                    ItemsRtu.Add(tmlReflectItem);
                }
                if(this.CurrentSelectArea.Value == -1)
                {
                    //tmpLst.Add(tmlReflectItem);
                    ItemsRtu.Add(tmlReflectItem);
                }
                ////ItemsRtu.Add(tmlReflectItem);

            }
            //var tmpLst2 = (from t in tmpLst orderby t.Type, t.ID ascending select t).ToList();
            //foreach (var t in tmpLst2)
            //{
            //    ItemsRtu.Add(t);
            //}
        }

        private ObservableCollection<PathItem> _detailInfo;
        public ObservableCollection<PathItem> DetailInfo
        {
            get { return _detailInfo; }
            set
            {
                if(value!=_detailInfo)
                {
                    _detailInfo = value;
                    RaisePropertyChanged(()=>this.DetailInfo);
                }
            }
        }

        private void OnCmdWatchDetailInfo(object sender, EventArgs e)
        {
            var oneRecord = sender as RtuItem;
            if (oneRecord == null) return;
            DetailInfo = oneRecord.PathItemsforthistml;
            var ar = new PublishEventArgs()
            {
                EventId =EquipmentGroupManage.Services.EventIdAssign.AnimationGrpMulitManange,
                EventType = PublishEventType.None
            };
            EventPublish.PublishEvent(ar);

        }

        void GetPath()
        {
            path.Clear();
            foreach (var f in ChildTreeItems)
            {
                foreach (var g in f.ChildTreeItems)
                {
                    if (g.IsGroup) continue;
                    if (!path.ContainsKey(g.NodeId)) path.Add(g.NodeId, new List<Tuple< int ,string >>());
                    var sps = f.NodeName.Split('-');

                    var gpname = "";
                    int grpId = 0;
                    foreach (var fg in sps)
                    {
                        //Int32.TryParse(fg, out grpId);
                        grpId = f.NodeId;
                        if (string.IsNullOrEmpty(fg) == false) gpname = fg;
                    }

                    path[g.NodeId].Add(new Tuple<int, string>(grpId,gpname  ));
                }
            }
        }

        /// <summary>
        /// 加载终端信息到_tmlsRelfectGrps中
        /// 并将终端的归属分组路径进行计算
        /// </summary>
        private void ReLoadTmlsRelfectGrps()
        {
            path.Clear();
            GetPath();

            foreach (var t in ItemsRtu )
            {
                int id = t.ID;
                t.PathCounte = 0;
                t.PathItemsforthistml.Clear();
                if (!path.ContainsKey(id)) continue;
                t.PathCounte = path[id].Count;
                foreach (var strPath in path[id])
                {
                    t.PathItemsforthistml.Add(new PathItem() {GrpName   = strPath.Item2 ,GrpId =strPath .Item1, Id = id});
                }
                string temp = "";
                foreach (var item in t.PathItemsforthistml)
                {
                    temp += item.GrpName;
                }
                if (temp.Length > 20)
                {
                    t.BtnDetailVisible = Visibility.Visible;
                }
                else
                {
                    t.BtnDetailVisible = Visibility.Collapsed;
                }
                if (t.PathItemsforthistml.Count > 0)
                    t.CurrentItem = t.PathItemsforthistml[0];
            }
        }

   
        /// <summary>
        /// 界面点击删除按钮后发布事件后的处理函数
        /// 删除在终端显示中的路径 并更新count
        /// </summary>
        /// <param name="tmlId"></param>
        /// <param name="grpId"></param>
        private void DeletePath(int tmlId, int  grpId)
        {
            foreach (var t in _itemsRtu)
            {
                if (t.ID == tmlId)
                {
                    bool bolfind = false;
                    foreach (var pathItem in t.PathItemsforthistml)
                    {
                        if (pathItem.GrpId  == grpId )
                        {
                            t.PathItemsforthistml.Remove(pathItem);
                            bolfind = true;
                            break;
                        }
                    }
                    if (bolfind)
                    {
                        t.PathCounte = t.PathItemsforthistml.Count;
                        if (t.PathItemsforthistml.Count > 0)
                        {
                            t.CurrentItem = t.PathItemsforthistml[0];
                        }
                        string temp = "";
                        foreach (var item in t.PathItemsforthistml)
                        {
                            temp += item.GrpName;
                        }
                        if (temp.Length > 20)
                        {
                            t.BtnDetailVisible = Visibility.Visible;
                        }
                        else
                        {
                            t.BtnDetailVisible = Visibility.Collapsed;
                        }
                        //删除在path中注册的路径信息
                
                        
                        if (path.ContainsKey(tmlId) && path[tmlId] != null  )
                        {
                            foreach (var f in path [tmlId ])
                            {
                                if(f.Item1 ==grpId )
                                {
                                    path[tmlId].Remove(f);
                                    break;
                                }
                            }
 
                        }

                        break;
                    }
                }
            }
           
        }

        #endregion

        #region top command  CmdCleanSelected

        private string _LookUpKey;

        public string LookUpKey
        {
            get { return _LookUpKey; }
            set
            {
                if (value != _LookUpKey)
                {
                    _LookUpKey = value;
                    this.RaisePropertyChanged(() => this.LookUpKey);
                }
            }
        }

        private DateTime [] _dateTimes=new DateTime[3];

        private ICommand _CmdCleanSelected;

        public ICommand CmdCleanSelected
        {
            get
            {
                if (_CmdCleanSelected == null)
                {
                    _CmdCleanSelected = new RelayCommand(ExCleanAllSelected,CanExCleanAllSelected,true );
                }
                return _CmdCleanSelected;
            }
        }


    

        private void ExCleanAllSelected()
        {
            _dateTimes[0] = DateTime.Now;
            foreach (var t in _itemsRtu)
            {
                t.IsSelected = false;
            }
        }
        private bool CanExCleanAllSelected()
        {
            return DateTime.Now.Ticks - _dateTimes[0].Ticks > 30000000;
        }

        private ICommand _CmdAllSelected;

        public ICommand CmdAllSelected
        {
            get
            {
                if (_CmdAllSelected == null)
                {
                    _CmdAllSelected = new RelayCommand(ExCmdAllSelected, CanCmdAllSelected, true);
                }
                return _CmdAllSelected;
            }
        }
        private void ExCmdAllSelected()
        {
           
            foreach (var t in _itemsRtu)
            {
                t.IsSelected = true ;
            }
        }
        private bool CanCmdAllSelected()
        {
            return true;
        }


             

       #region 按钮 增改删

        private ICommand _CmdSCmdAddGrpave;

        public ICommand CmdAddGrp
        {
            get
            {
                if (_CmdSCmdAddGrpave == null)
                {
                    _CmdSCmdAddGrpave = new RelayCommand(AddCmdEx, CanAdd, false);
                }
                return _CmdSCmdAddGrpave;
            }
        }

        private bool CanAdd()
        {
            return true;
        }
        private void AddCmdEx()
        {
          
            
            var tmp = new Wlst.Sr.EquipmentInfoHolding.Model.GroupInformation(new GroupItemsInfo.GroupItem()
                                                                                  {
                                                                                      AreaId = CurrentSelectArea.Value,
                                                                                      GroupName = "新分组"+AddMaxId ,
                                                                                      GroupId = AddMaxId,
                                                                                      LstTml = new List<int>()
                                                                                  });
            AddMaxId += 1;
            this.ChildTreeItems.Add(new TreeItemViewModel(null,CurrentSelectArea .Value, tmp, true,this .ChildTreeItems ));
        }

        /// <summary>
        /// 当修改树节点名称前需要备份前节点名称
        /// 若用户输入不合法则恢复原名称
        /// </summary>
        private string BackUpName;

        private ICommand _cmdeditGrp;

        public ICommand CmdEditGrp
        {
            get
            {
                if (_cmdeditGrp == null)
                {
                    _cmdeditGrp = new RelayCommand(EditCmdEx, CanEdit, false);
                }
                return _cmdeditGrp;
            }
        }

        private bool CanEdit()
        {
            return true;
        }
        private void EditCmdEx()
        {
            foreach (var t in this.ChildTreeItems)
            {
                if (t.IsSelected == true)
                {
                    t.BackUpName = t.NodeName;
                    t.LabNameVisi = Visibility.Collapsed;
                    t.TxbNameVisi = Visibility.Visible;

                }
                else
                {
                    t.BackUpName = t.NodeName;
                    t.LabNameVisi = Visibility.Visible;
                    t.TxbNameVisi = Visibility.Collapsed;
                }
            }
        }

        private ICommand _cmddelGrp;

        public ICommand CmdDelGrp
        {
            get
            {
                if (_cmddelGrp == null)
                {
                    _cmddelGrp = new RelayCommand(DelCmdEx, CanDel, false);
                }
                return _cmddelGrp;
            }
        }

        private bool CanDel()
        {
            return true;
        }
        private void DelCmdEx()
        {
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].IsSelected == true && this.ChildTreeItems[i].IsGroup == true)
                {
                    //if (this.ChildTreeItems[i].ChildTreeItems.Count != 0)
                    //{
                        var infoss = WlstMessageBox.Show("确认删除", "是否删除该分组，是 继续删除，否 退出.", WlstMessageBoxType.YesNo);
                        if (infoss != WlstMessageBoxResults.Yes) return;
                        this.ChildTreeItems.RemoveAt(i);
                    //}
                    //else
                    //{
                    //    this.ChildTreeItems.RemoveAt(i);
                    //}
                }
            }
            ReLoadTmlsRelfectGrps();
        }

        #endregion

        /// <summary>
        /// 获取整棵树的分组信息
        /// </summary>
        /// <returns></returns>
        private List<GroupInformation> GetGrpsBelong()
        {
            var lis = new List<Wlst.Sr.EquipmentInfoHolding.Model.GroupInformation>();

            int index = 1;
            foreach (var t in this.ChildTreeItems)
            {
                if (t.IsGroup == false) continue;
                var tmp =
                    new GroupInformation(new GroupItemsInfo.GroupItem()
                    {
                        AreaId = CurrentSelectArea.Value,
                        GroupName = t.NodeName,
                        GroupId = t.NodeId,
                        LstTml = new List<int>()
                    }
                        )
                    {
                        Index = index++
                    };
                foreach (var g in t.ChildTreeItems)
                {
                    if (g.IsGroup) continue;
                    tmp.LstTml.Add(g.NodeId);
                }

                lis.Add(tmp);
            }
            return lis;
        }

        private ICommand _CmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_CmdSave == null)
                {
                    _CmdSave = new RelayCommand(ExSave, CanSave,false );
                }
                return _CmdSave;
            }
        }

        private void ExSave()
        {
            _dateTimes[1] = DateTime.Now;
            if (CurrentSelectArea == null) return;
            int araid = CurrentSelectArea.Value;
            var tmp = GetGrpsBelong();
            if(tmp .Count ==0)
            {
                tmp.Add(new GroupInformation(new GroupItemsInfo.GroupItem()
                                                 {

                                                     AreaId = araid,
                                                     GroupName = "-100",
                                                     GroupId = -100,
                                                     LstTml = new List<int>()

                                                 }));
                ;

            }
            Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.UpdateGroupsInfo(tmp);
            Msg = DateTime.Now + " 已提交信息到服务器！";
           // Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.RequestGroupInfo();
            //var arg = new PublishEventArgs()
            //{
            //    EventId = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate,
            //    EventType = PublishEventType.Core
            //};
            //EventPublish.PublishEvent(arg);
            dtSnd = DateTime.Now;
           // ReLoadTmlsRelfectGrps();

        }

        private bool CanSave()
        {
            if (DateTime.Now.Ticks - _dateTimes[1].Ticks < 60000000) return false;
            return true;

            //return DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
        }

        #endregion

        #region DragSourceTree 成员

        private DragSourceClassIDragSource DragSourceClassTree;

        public IDragSource DragSourceTree
        {
            get
            {
                if (DragSourceClassTree == null)
                {
                    DragSourceClassTree = new DragSourceClassIDragSource(DragSourceTreeStartDrag, DragSourceTreeDragData);
                }
                return DragSourceClassTree;
            }
        }

        public DragDropEffects DragSourceTreeStartDrag(object Data)
        {
            try
            {
                var dataAsPath = Data as HelpDragAndDrop;
                if (dataAsPath == null) return DragDropEffects.None;
                if (dataAsPath.CarrayData.Equals("")) return DragDropEffects.None;
                return DragDropEffects.Copy | DragDropEffects.Move;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public object DragSourceTreeDragData()
        {
            string strGetSeletcTreeNodePath = GetSelectMVVMPath();
            if (strGetSeletcTreeNodePath.Equals("")) return null;

            return new HelpDragAndDrop(HelpDragAndDrop.enumType.TreeToListView, strGetSeletcTreeNodePath);
        }

        #endregion

        #region DropTargetTree

        private DropTargetClassIDropTarget DropTargetClassTree;

        public IDropTarget DropTargetTree
        {
            get
            {
                if (DropTargetClassTree == null)
                {
                    DropTargetClassTree = new DropTargetClassIDropTarget(DropTargetTreeDragOver, DropTargetTreeDrop);
                }
                return DropTargetClassTree;
            }
        }

        public DragDropEffects DropTargetTreeDragOver(object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return DragDropEffects.None;
            return DragDropEffects.Copy | DragDropEffects.Move;
        }

        public void DropTargetTreeDrop(object sender, DragEventArgs e, object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return;
            if (t.type == HelpDragAndDrop.enumType.TreeToListView) return;
            if (t.type == HelpDragAndDrop.enumType.ListViewToTree)
            {
                //组增加终端
                //todo
                var dropInfomation = new DropTargetInfomation(sender, e);
                var mvvm = dropInfomation.TargetItem as TreeItemViewModel;
                if (mvvm == null) return;


                var lstNeedAddTml = new List<int>();
                string[] strTmls = t.CarrayData.Split('/');
                try
                {
                    foreach (var strTmlId in strTmls)
                    {
                        int tempId = Convert.ToInt32(strTmlId);
                        if (tempId > 0) lstNeedAddTml.Add(tempId);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("iGrpsHelpShowManageViewModel DropTargetTreeDrop  err path is :" +
                                           t.CarrayData + ",Err is " + ex.ToString());
                    return;
                }

                if(mvvm .IsGroup ==false && mvvm .Father !=null  )
                {
                    mvvm = mvvm.Father;
                }
                if (mvvm.IsGroup == false) return;
                foreach (var f in lstNeedAddTml )
                {
                    bool find = false;
                    foreach (var g in mvvm .ChildTreeItems )
                    {
                        if(g.NodeId ==f )
                        {
                            find = true;
                            break;
                        }
                    }
                    if (find) continue;

                    var data = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if(data ==null ) continue;
                    mvvm.ChildTreeItems.Add(new TreeItemViewModel(mvvm, CurrentSelectArea.Value,data, false, null));
                }
    
                ReLoadTmlsRelfectGrps();
            }

            ExCleanAllSelected();
        }

        #endregion

        #region DragSourceListView 成员

        private DragSourceClassIDragSource DragSourceClassListView;

        public IDragSource DragSourceListView
        {
            get
            {
                if (DragSourceClassListView == null)
                {
                    DragSourceClassListView = new DragSourceClassIDragSource(DragSourceListViewStartDrag,
                                                                             DragSourceListViewDragData);
                }
                return DragSourceClassListView;
            }
        }

        public DragDropEffects DragSourceListViewStartDrag(object Data)
        {
            try
            {
                var dataAsPath = Data as HelpDragAndDrop;
                if (dataAsPath == null) return DragDropEffects.None;
                if (dataAsPath.CarrayData.Equals("")) return DragDropEffects.None;
                return DragDropEffects.Copy | DragDropEffects.Move;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public object DragSourceListViewDragData()
        {
            string strGetSeletcTmlIds = "";
            foreach (var t in ItemsRtu)
            {
                if (t.IsSelected)
                {
                    strGetSeletcTmlIds = strGetSeletcTmlIds + t.ID + "/";
                }
            }
            if (strGetSeletcTmlIds.Length > 0)
                strGetSeletcTmlIds = strGetSeletcTmlIds.Substring(0, strGetSeletcTmlIds.Length - 1);
            if (strGetSeletcTmlIds.Equals("")) return null;

            return new HelpDragAndDrop(HelpDragAndDrop.enumType.ListViewToTree, strGetSeletcTmlIds);
        }

        #endregion

        #region DropTargetListView

        private DropTargetClassIDropTarget DropTargetClassListView;

        public IDropTarget DropTargetListView
        {
            get
            {
                if (DropTargetClassListView == null)
                {
                    DropTargetClassListView = new DropTargetClassIDropTarget(DropTargetListViewDragOver,
                                                                             DropTargetListViewDrop);
                }
                return DropTargetClassListView;
            }
        }

        public DragDropEffects DropTargetListViewDragOver(object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return DragDropEffects.None;
            return DragDropEffects.Copy | DragDropEffects.Move;
        }

        /// <summary>
        /// 仅终端树 拖动到lv中使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="Data"></param>
        public void DropTargetListViewDrop(object sender, DragEventArgs e, object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return;
            if (t.type == HelpDragAndDrop.enumType.TreeToListView)
            {
                try
                {
                    string[] strSp = t.CarrayData.Split('/');
                    string strId = strSp[strSp.Length - 1];
                    string grpInfo = strSp[0];
                    string[] grpInfoSp = grpInfo.Split('-');
                    string grpName = grpInfoSp[grpInfoSp.Length -1];
                    string grpid = grpInfoSp[0];
                    int Id = Convert.ToInt32((strId));
                    int grpId = Convert.ToInt32(grpid);

                    if (Id == 0) return;

                    var args = new PublishEventArgs()
                                   {
                                       EventType = PublishEventTypeLocal.Name,
                                       EventId =
                                           PublishEventTypeLocal.DeleteGrpPathByCommandBasicShowGroupManage
                                   };
                    args.AddParams(Id, grpId,grpName);
                    EventPublish.PublishEvent(args);

                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("iGrpsHelpShowManageViewModel DropTargetListViewDrop  err path is :" +
                                           t.CarrayData + ",Err is " + ex.ToString());
                    return;
                }
            }
        }

        #endregion
    };

    #region HelpDragAndDrop
    public class HelpDragAndDrop
    {
        public enum enumType
        {
            /// <summary>
            /// 终端树到终端 即删除树种的终端
            /// </summary>
            TreeToListView = 1,

            /// <summary>
            /// 终端到树 即组增加终端
            /// </summary>
            ListViewToTree
        };

        public HelpDragAndDrop(enumType et, string data)
        {
            type = et;
            CarrayData = data;
        }

        public enumType type;

        /// <summary>
        /// 拖转携带的数据
        /// 树到终端大致为  id-grp-name/id
        /// 终端到树大致为  id/id/id/id
        /// </summary>
        public string CarrayData;
    }
    #endregion

    //public class HelpDragAndDrop
    //{
    //    public HelpDragAndDrop(TreeItemViewModel tv)
    //    {
    //        treeData = tv;
    //        listData = new List<int>();
    //    }

    //    public HelpDragAndDrop(List<int> lst)
    //    {
    //        treeData = null;
    //        listData = new List<int>();
    //        foreach (var t in lst)
    //        {
    //            listData.Add(t);
    //        }
    //    }

    //    public TreeItemViewModel treeData;

    //    /// <summary>
    //    /// 拖转携带的数据
    //    /// 树到终端大致为  id-grp-name/id
    //    /// 终端到树大致为  id/id/id/id
    //    /// </summary>
    //    public List<int> listData;
    //}
}