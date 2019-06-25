using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.CoreMims.NodeServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.SinglePlan.SingleGrp.Services;
using Wlst.Ux.SinglePlan.SingleGrp.View;
using Wlst.iifx;

namespace Wlst.Ux.SinglePlan.SingleGrp.ViewModel
{
    //[Export(typeof(SingleGrpViewModel))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SingleGrpViewModel : Wlst.Cr.CoreMims.NodeServices.TreeViewControl, IISingleGrp
    {
        public SingleGrpViewModel(): base(null, null)
        {  
        }
         public void NavOnLoad(params object[] parsObjects)
         {
             AreaName.Clear();

             if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
             {
                 foreach (var t in Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                 {
                     string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                     AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                 }
             }
             else
             {
                 foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                 {
                     if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                     {
                         string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                         AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                     }
                 }
             }

             if (AreaName.Count > 0)
                 AreaComboBoxSelected = AreaName[0];
             if (AreaName.Count > 1)
             {
                 Visi = Visibility.Visible;
             }
             else
             {
                 Visi = Visibility.Collapsed;
             }

             IsLeftSearchTreeVisi = Visibility.Collapsed;
             IsLeftTreeVisi = Visibility.Visible;
             Msg = "";
             //LoadNodeRight();

         }
         public void OnUserHideOrClosing()
         {
         }
         #region IITab

         public int Index
         {
             get { return 1; }
         }

         public string Title
         {
             get { return "新单灯分组"; }
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

    public partial class SingleGrpViewModel
    {
        private static ObservableCollection<AreaInt> _devices;

        /// <summary>
        /// 区域名称
        /// </summary>
        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }

        private Visibility _txtVisi;

        /// <summary>
        /// 
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

        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private AreaInt _areacomboboxselected;
        private int AreaId;

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;
                    RequestGroup();
                    RequestList();
                    RequestCtrlList(SluBref);
                    InitTvc();
                    LoadNodeLeft();
                }
            }
        }

        private ObservableCollection<SluGrpSet.GrpInfo> _grpItem;

        /// <summary>
        /// 分组信息
        /// </summary>
        public ObservableCollection<SluGrpSet.GrpInfo> GrpItem
        {
            get
            {
                if (_grpItem == null)
                {
                    _grpItem = new ObservableCollection<SluGrpSet.GrpInfo>();
                }
                return _grpItem;
            }

        }

        private ObservableCollection<SluGrpSet.SluInfoItem> _sluItem;

        /// <summary>
        /// 集中器详细信息
        /// </summary>
        public ObservableCollection<SluGrpSet.SluInfoItem> SluItem
        {
            get
            {
                if (_sluItem == null)
                {
                    _sluItem = new ObservableCollection<SluGrpSet.SluInfoItem>();
                }
                return _sluItem;
            }

        }

        private ObservableCollection<int> _sluBref;

        /// <summary>
        /// 集中器简要信息
        /// </summary>
        public ObservableCollection<int> SluBref
        {
            get
            {
                if (_sluBref == null)
                {
                    _sluBref = new ObservableCollection<int>();
                }
                return _sluBref;
            }

        }

        private string _msg;
        /// <summary>
        /// 通知
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value == _msg) return;
                _msg = value;
                RaisePropertyChanged(() => Msg);
            }
        }

        #region 去选全清
        private bool _ischecked1;
        /// <summary>
        /// 全选全清1
        /// </summary>
        public bool Ischecked1
        {
            get { return _ischecked1; }
            set
            {
                if (value == _ischecked1) return;
                _ischecked1 = value;
                RaisePropertyChanged(() => Ischecked1);
                foreach (var tvc in Tvc.ChildItems)
                {
                    foreach (var f in tvc.ChildItems)
                    {
                        var ff = f as TreeViewBaseNodeEx;
                        if (ff == null) return;
                        if (ff.IsSelected2B)
                            ff.Is1B = ff.IsEnable1 && ff.IsVisi1 == Visibility.Visible && _ischecked1;
                    }
                }
            }
        }

        private bool _ischecked2;
        /// <summary>
        /// 全选全清2
        /// </summary>
        public bool Ischecked2
        {
            get { return _ischecked2; }
            set
            {
                if (value == _ischecked2) return;
                _ischecked2 = value;
                RaisePropertyChanged(() => Ischecked2);
                foreach (var tvc in Tvc.ChildItems)
                {
                    foreach (var f in tvc.ChildItems)
                    {
                        var ff = f as TreeViewBaseNodeEx;
                        if (ff == null) return;
                        if (ff.IsSelected2B)
                            ff.Is2B = ff.IsEnable2 && ff.IsVisi2 == Visibility.Visible && _ischecked2;
                    }
                }
            }
        }

        private bool _ischecked3;
        /// <summary>
        /// 全选全清3
        /// </summary>
        public bool Ischecked3
        {
            get { return _ischecked3; }
            set
            {
                if (value == _ischecked3) return;
                _ischecked3 = value;
                RaisePropertyChanged(() => Ischecked3);
                foreach (var tvc in Tvc.ChildItems)
                {
                    foreach (var f in tvc.ChildItems)
                    {
                        var ff = f as TreeViewBaseNodeEx;
                        if (ff == null) return;
                        if (ff.IsSelected2B)
                            ff.Is3B = ff.IsEnable3 && ff.IsVisi3 == Visibility.Visible && _ischecked3;
                    }
                }
            }
        }

        private bool _ischecked4;
        /// <summary>
        /// 全选全清4
        /// </summary>
        public bool Ischecked4
        {
            get { return _ischecked4; }
            set
            {
                if (value == _ischecked4) return;
                _ischecked4 = value;
                RaisePropertyChanged(() => Ischecked4);
                foreach (var tvc in Tvc.ChildItems)
                {
                    foreach (var f in tvc.ChildItems)
                    {
                        var ff = f as TreeViewBaseNodeEx;
                        if (ff == null) return;
                        if (ff.IsSelected2B)
                            ff.Is4B = ff.IsEnable4 && ff.IsVisi4 == Visibility.Visible && _ischecked4;
                    }
                }
            }
        }
        #endregion

        #region Search Node

        private ObservableCollection<TreeViewBaseNode> _childTreeItemsLeftSearch;

        public ObservableCollection<TreeViewBaseNode> ChildTreeItemsLeftSearch
        {
            get
            {
                if (_childTreeItemsLeftSearch == null)
                    _childTreeItemsLeftSearch = new ObservableCollection<TreeViewBaseNode>();
                return _childTreeItemsLeftSearch;
            }
        }

        private string _searchLeft;

        public string SearchLeft
        {
            get { return _searchLeft; }
            set
            {
                if (_searchLeft != value)
                {
                    _searchLeft = value;
                    this.RaisePropertyChanged(() => this.SearchLeft);
                    SearchNode(_searchLeft);
                }
            }
        }

        private Visibility _isLeftSearchTreeVisi;

        public Visibility IsLeftSearchTreeVisi
        {
            get { return _isLeftSearchTreeVisi; }
            set
            {
                if (value == _isLeftSearchTreeVisi) return;
                _isLeftSearchTreeVisi = value;
                this.RaisePropertyChanged(() => this.IsLeftSearchTreeVisi);
            }
        }

        private Visibility _isLeftTreeVisi;

        public Visibility IsLeftTreeVisi
        {
            get { return _isLeftTreeVisi; }
            set
            {
                if (value == _isLeftTreeVisi) return;
                _isLeftTreeVisi = value;
                this.RaisePropertyChanged(() => this.IsLeftTreeVisi);
            }
        }
        #endregion
    }
    /// <summary>
    /// Icommand
    /// </summary>
    public partial class SingleGrpViewModel
    {
        /// <summary>
        /// 增加分组
        /// </summary>
        #region CmdAddGrp

        private DateTime _dtCmdAddGrp;
        private ICommand _cmdAddGrp;

        public ICommand CmdAddGrp
        {
            get
            {
                return _cmdAddGrp ??
                       (_cmdAddGrp = new RelayCommand(ExCmdAddGrp, CanCmdAddGrp, true));
            }

        }

        private bool CanCmdAddGrp()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddGrp.Ticks > 10000000;
        }

        private void ExCmdAddGrp()
        {
            _dtCmdAddGrp = DateTime.Now;
            var data = UMessageBoxWantSomefromUser.Show("新增分组", "请输入分组名称", "");
            if (data == UMessageBoxWantSomefromUser.CancelReturn) return;
            var id = GrpItem.Select(item => item.GrpId).Concat(new[] {0}).Max() + 1;
            var inputInfo = new InputInfo(null, null, id, 1, id);
            inputInfo.NodeName1B = id + "-" + data;
            inputInfo.Str1StoreN = data;
            AddRootNode(inputInfo);

        }

        #endregion

        /// <summary>
        /// 删除分组
        /// </summary>
        #region CmdDeleteGrp

        private DateTime _dtCmdDeleteGrp;
        private ICommand _cmdDeleteGrp;

        public ICommand CmdDeleteGrp
        {
            get
            {
                return _cmdDeleteGrp ??
                       (_cmdDeleteGrp = new RelayCommand(ExCmdDeleteGrp, CanCmdDeleteGrp, true));
            }

        }

        private bool CanCmdDeleteGrp()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdDeleteGrp.Ticks > 10000000;
        }

        private void ExCmdDeleteGrp()
        {
            _dtCmdDeleteGrp = DateTime.Now;
            var grpName = "";
            var idf = new List<long>();
            var grplist = new List<int>();
            //获取树中勾选的分组，并获取唯一标识码
            foreach (var item in ChildItems)
            {
                if(item.Key1TypeN!=1) continue;
                if (item.IsB)
                {
                    grpName = grpName + " " + item.NodeName1B;
                    grplist.Add(item.Key2);
                    var node=this.GetNodeByKey(1, item.Key2);
                    if (node.Count > 0)
                    {
                        idf.Add(node[0].IdfN);
                    }
                }
            }

            var data = WlstMessageBox.Show("删除分组", "确认删除" + grpName, WlstMessageBoxType.YesNo);
            if(data==WlstMessageBoxResults.No) return;
            foreach (var tvc in Tvc.ChildItems)
            {
                foreach (var f in tvc.ChildItems)
                {
                    if (grplist.Contains(f.Id2StoreN)) f.IsEnable1 = true;
                    if (grplist.Contains(f.Id3StoreN)) f.IsEnable2 = true;
                    if (grplist.Contains(f.Id4StoreN)) f.IsEnable3 = true;
                    if (grplist.Contains(f.Id5StoreN)) f.IsEnable4 = true;
                }
            }
            DeleteNode(idf);
        }

        #endregion

        /// <summary>
        /// 加入分组
        /// </summary>
        #region CmdAddToGrp

        private DateTime _dtCmdAddToGrp;
        private ICommand _cmdAddToGrp;

        public ICommand CmdAddToGrp
        {
            get
            {
                return _cmdAddToGrp ??
                       (_cmdAddToGrp = new RelayCommand(ExCmdAddToGrp, CanCmdAddToGrp, true));
            }

        }

        private bool CanCmdAddToGrp()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddToGrp.Ticks > 10000000;
        }

        private void ExCmdAddToGrp()
        {
            _dtCmdAddToGrp = DateTime.Now;
            //校验
            foreach (var t in Tvc.ChildItems)
            {
                var compare = new List<int>();
                var visiList = new List<int>();
                foreach (var f in t.ChildItems)
                {
                    var compare1 = new List<int>();
                    if (!f.Is1B && !f.Is2B && !f.Is3B && !f.Is4B && f.IsEnable1 && f.IsEnable2 && f.IsEnable3 && f.IsEnable4) continue;
                    var isOne = ChildItems.Count(xx => xx.IsB);
                    if (isOne == 0)
                    {
                        WlstMessageBox.Show("添加失败", "请选择一个分组", WlstMessageBoxType.Ok);
                        return;
                    }
                    if (isOne > 1)
                    {
                        WlstMessageBox.Show("添加失败", "只能选择一个分组", WlstMessageBoxType.Ok);
                        return;
                    }
                    if (f.Is1B) compare1.Add(0);
                    if (f.Is2B) compare1.Add(1);
                    if (f.Is3B) compare1.Add(2);
                    if (f.Is4B) compare1.Add(3);
                    
                    foreach (var item in ChildItems)
                    {
                        if (!item.IsB) continue;
                        foreach (var slu in item.ChildItems)
                        {
                            if (slu.Key2 != t.Key2) continue;
                            foreach (var ctrl in slu.ChildItems)
                            {
                                if (ctrl.Key2 != f.Key2) continue;
                                foreach (var lamp in ctrl.ChildItems)
                                {
                                    compare1.Add(lamp.Key2);
                                }
                            }
                        }
                    }

                    if (compare.Count != 0 && compare1.Count != 0 && !CompareArr(compare, compare1))
                    {
                        List<int> comp = compare;
                        comp.AddRange(visiList);
                        if (!CompareArr(comp, compare1))
                        {
                            WlstMessageBox.Show("添加失败", "同一集中器下控制器灯头需一致", WlstMessageBoxType.Ok);
                            return;
                        }
                    }
                    else
                    {
                        if (compare1.Count != 0)
                            compare = compare1;
                    }
                    if (f.IsVisi1 == Visibility.Collapsed) visiList.Add(0);
                    else visiList.Remove(0);
                    if (f.IsVisi2 == Visibility.Collapsed) visiList.Add(1);
                    else visiList.Remove(1);
                    if (f.IsVisi3 == Visibility.Collapsed) visiList.Add(2);
                    else visiList.Remove(2);
                    if (f.IsVisi4 == Visibility.Collapsed) visiList.Add(3);
                    else visiList.Remove(3);
                }
            }

            //将节点添加到树中
            foreach (var t in Tvc.ChildItems)
            {
                foreach (var f in t.ChildItems)
                {
                    if (!f.Is1B && !f.Is2B && !f.Is3B && !f.Is4B) continue;
                    foreach (var item in ChildItems)
                    {
                        if (!item.IsB) continue;
                        var rtn = new List<InputInfo>();
                        var rootGrp = new List<InputInfo>();
                        var grp = new SluGrpSet.GrpInfo();
                        grp.GrpId = item.Key2;
                        grp.GrpName = item.Str1StoreN;
                        var rootinfo = LoadNodeGetGrpInfo(grp);
                        var lst2 = LoadNode2GetSluInfoByGrp(rootinfo, grp.GrpId, t.Key2);
                        rootGrp.AddRange(lst2);
                        var lst3 = LoadNode3GetCtrlInfoBySlu(rootGrp, f.Key2, f.Id1StoreN, f.Str1StoreN);
                        foreach (var l in lst3)
                        {
                            var lamp = new List<int>();
                            lamp.Add(f.Is1B ? item.Key2 : 0);
                            lamp.Add(f.Is2B ? item.Key2 : 0);
                            lamp.Add(f.Is3B ? item.Key2 : 0);
                            lamp.Add(f.Is4B ? item.Key2 : 0);
                            var lst4 = LoadNode4GetLampByCtrl(l, lamp, f.Key2, t.Key2, item.Key2);
                            rtn.AddRange(lst4);
                            if (f.Is1B) f.Id2StoreN = item.Key2;
                            if (f.Is2B) f.Id3StoreN = item.Key2;
                            if (f.Is3B) f.Id4StoreN = item.Key2;
                            if (f.Is4B) f.Id5StoreN = item.Key2;
                            f.IsEnable1 = !f.Is1B && f.IsEnable1;
                            f.IsEnable2 = !f.Is2B && f.IsEnable2;
                            f.IsEnable3 = !f.Is3B && f.IsEnable3;
                            f.IsEnable4 = !f.Is4B && f.IsEnable4;
                            f.Is1B = f.Is2B = f.Is3B = f.Is4B = false;
                            
                        }
                        var node1 = GetNodeByKey(2, t.Key2, item.Key2);
                        if (node1.Count == 0)
                        {
                            //分组中新增集中器
                            rtn.AddRange(rootGrp);
                            rtn.AddRange(lst3);
                            AddNode(GetNodeByKey(1, item.Key2)[0].IdfN, rtn, false);
                            //TempAdd(1, rtn, treeNode);
                        }
                        else
                        {
                            //集中器中增加控制器
                            var node2 = GetNodeByKey(3, f.Key2, t.Key2, item.Key2);
                            if (node2.Count == 0)
                            {
                                rtn.AddRange(lst3);
                                AddNode(node1[0].IdfN, rtn, false);
                                //TempAdd(2, rtn, treeNode);
                            }
                            else
                            {
                                //控制器下增加灯头
                                AddNode(node2[0].IdfN, rtn, false);
                                //TempAdd(3, rtn, treeNode);

                            }
                        }    
                    }
                }
            }
           

        }

        public static bool CompareArr(List<int> arr1, List<int> arr2)
        {

            var q = from a in arr1 join b in arr2 on a equals b select a;

            bool flag = arr1.Count == arr2.Count && q.Count() == arr1.Count;

            return flag;//内容相同返回true,反之返回false。

        }

        #endregion

        /// <summary>
        /// 移出分组
        /// </summary>
        #region CmdRemoveFromGrp

        private DateTime _dtCmdRemoveFromGrp;
        private ICommand _cmdRemoveFromGrp;

        public ICommand CmdRemoveFromGrp
        {
            get
            {
                return _cmdRemoveFromGrp ??
                       (_cmdRemoveFromGrp = new RelayCommand(ExCmdRemoveFromGrp, CanCmdRemoveFromGrp, true));
            }

        }

        private bool CanCmdRemoveFromGrp()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdRemoveFromGrp.Ticks > 10000000;
        }

        private void ExCmdRemoveFromGrp()
        {
            _dtCmdRemoveFromGrp = DateTime.Now;
            var nodeList = new List<long>();
            var flag = true;
            foreach (var item in ChildItems)
            {
                foreach (var f in item.ChildItems)
                {
                    if (f.IsB)
                    {
                        //删除集中器
                        var node = GetNodeByKey(2, f.Key2, item.Key2);
                        if (node.Count > 0)
                        {
                            nodeList.Add(node[0].IdfN);
                            foreach (var tvc in Tvc.ChildItems)
                            {
                                if (tvc.Key2 != f.Key2) continue;
                                foreach (var x in tvc.ChildItems)
                                {
                                    foreach (var ff in f.ChildItems)
                                    {
                                        if(ff.Key2!=x.Key2) continue;
                                        foreach (var fff in ff.ChildItems)
                                        {
                                            if (fff.Key2 == 0) x.IsEnable1 = true;
                                            if (fff.Key2 == 1) x.IsEnable2 = true;
                                            if (fff.Key2 == 2) x.IsEnable3 = true;
                                            if (fff.Key2 == 3) x.IsEnable4 = true;
                                        }
                                    }
                                    //x.IsEnable1 = x.IsEnable2 = x.IsEnable3 = x.IsEnable4 = true;
                                }
                            }
                        }
                        continue;
                    }
                    foreach (var t in f.ChildItems)
                    {
                        if (t.IsB)
                        {
                            //删除控制器
                            var node1 = GetNodeByKey(3, t.Key2, f.Key2, item.Key2);
                            if (node1.Count > 0)
                            {
                                nodeList.Add(node1[0].IdfN);
                                foreach (var tvc in Tvc.ChildItems)
                                {
                                    if (tvc.Key2 != f.Key2) continue;
                                    foreach (var x in tvc.ChildItems)
                                    {
                                        if (x.Key2 != t.Key2) continue;
                                        foreach (var tt in t.ChildItems)
                                        {
                                            if (tt.Key2 == 0) x.IsEnable1 = true;
                                            if (tt.Key2 == 1) x.IsEnable2 = true;
                                            if (tt.Key2 == 2) x.IsEnable3 = true;
                                            if (tt.Key2 == 3) x.IsEnable4 = true;
                                        }
                                        //x.IsEnable1 = x.IsEnable2 = x.IsEnable3 = x.IsEnable4 = true;
                                    }
                                }
                            }
                            continue;
                        }
                        foreach (var l in t.ChildItems)
                        {
                            //删除灯头
                            if (!l.IsB) continue;
                            if (flag)
                            {
                                flag = false;
                                var data = WlstMessageBox.Show("提示", "将自动移除集中器下所有控制器的相同序号灯头", WlstMessageBoxType.YesNo);
                                if (data == WlstMessageBoxResults.No) return;
                            }
                            foreach (var xx in f.ChildItems)
                            {
                                var node22 = GetNodeByKey(4, l.Key2, xx.Key2, f.Key2, item.Key2);
                                if (node22.Count <= 0) continue;
                                nodeList.Add(node22[0].IdfN);
                                foreach (var tvc in Tvc.ChildItems)
                                {
                                    if (tvc.Key2 != f.Key2) continue;
                                    foreach (var x in tvc.ChildItems)
                                    {
                                        if (x.Key2 != xx.Key2) continue;
                                        switch (l.Key2)
                                        {
                                            case 0:
                                                x.IsEnable1 = true;
                                                x.Id2StoreN = 0;
                                                break;
                                            case 1:
                                                x.IsEnable2 = true;
                                                x.Id3StoreN = 0;
                                                break;
                                            case 2:
                                                x.IsEnable3 = true;
                                                x.Id4StoreN = 0;
                                                break;
                                            case 3:
                                                x.IsEnable4 = true;
                                                x.Id5StoreN = 0;
                                                break;
                                        }
                                    }
                                }
                            }
                            //var node2 = GetNodeByKey(4, l.Key2, t.Key2, f.Key2, item.Key2);
                            //if (node2.Count > 0)
                            //{
                            //    nodeList.Add(node2[0].IdfN);
                            //}
                            //foreach (var tvc in Tvc.ChildItems)
                            //{
                            //    if (tvc.Key2 != f.Key2) continue;
                            //    foreach (var x in tvc.ChildItems)
                            //    {
                            //        if (x.Key2 != t.Key2) continue;
                            //        switch (l.Key2)
                            //        {
                            //            case 0:
                            //                x.IsEnable1 = true;
                            //                x.Id2StoreN = 0;
                            //                break;
                            //            case 1:
                            //                x.IsEnable2 = true;
                            //                x.Id3StoreN = 0;
                            //                break;
                            //            case 2:
                            //                x.IsEnable3 = true;
                            //                x.Id4StoreN = 0;
                            //                break;
                            //            case 3:
                            //                x.IsEnable4 = true;
                            //                x.Id5StoreN = 0;
                            //                break;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
            DeleteNode(nodeList);
        }

        #endregion

         /// <summary>
        /// 保存分组
        /// </summary>
        #region CmdSaveGrp

        private DateTime _dtCmdSaveGrp;
        private ICommand _cmdSaveGrp;

        public ICommand CmdSaveGrp
        {
            get
            {
                return _cmdSaveGrp ??
                       (_cmdSaveGrp = new RelayCommand(ExCmdSaveGrp, CanCmdSaveGrp, true));
            }

        }

        private bool CanCmdSaveGrp()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSaveGrp.Ticks > 10000000;
        }

        private void ExCmdSaveGrp()
        {
            _dtCmdSaveGrp = DateTime.Now;
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存"  + " ...";
            //保存组信息
            var req = new MsgWithIif()
            {
                Post4078 = new SluGrpSet()
            };
            req.Post4078.AreaId = AreaId;
            req.Post4078.Op = 11;
            foreach (var f in ChildItems)
            {
                req.Post4078.ItemsGrp.Add(new SluGrpSet.GrpInfo()
                                              {
                                                  GrpId = f.Key2,
                                                  GrpName = f.Str1StoreN
                                              });
            }
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4078", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            if (data == null) return;
            var res = MsgWithIif.Deserialize(data);
            var x = res.Head;
            //保存集中器信息
            var req1 = new MsgWithIif()
            {
                Post4078 = new SluGrpSet()
            };
            req1.Post4078.AreaId = AreaId;
            req1.Post4078.Op = 13;
            foreach (var tvc in Tvc.ChildItems)
            {
                var ctrlItem = new List<SluGrpSet.SluInfoItem.SluCtrlInfo>();
                foreach (var f in tvc.ChildItems)
                {
                    var lamp = new List<int>();
                    if(f.IsVisi1==Visibility.Visible) lamp.Add(f.Id2StoreN);
                    if(f.IsVisi2==Visibility.Visible) lamp.Add(f.Id3StoreN);
                    if(f.IsVisi3==Visibility.Visible) lamp.Add(f.Id4StoreN);
                    if(f.IsVisi4==Visibility.Visible) lamp.Add(f.Id5StoreN);
                    ctrlItem.Add(new SluGrpSet.SluInfoItem.SluCtrlInfo()
                                     {
                                         CtrlId = f.Key2,
                                         CtrlPhyid = f.Id1StoreN,
                                         CtrlName = f.Str1StoreN,
                                         LampBelongGrp = lamp,
                                         LampCount = lamp.Count
                                     });
                }
                req1.Post4078.ItemsSlu.Add(new SluGrpSet.SluInfoItem()
                                               {
                                                   SluId = tvc.Key2,
                                                   SluPhyId = tvc.Id1StoreN,
                                                   SluName = tvc.Str1StoreN,
                                                   CtrlsInfo = ctrlItem
                                               });
            }
            var data1 = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndPostHttp("post4078", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req1)));
            //var url ="http://10.3.9.40:18088/mims/post4078";
            //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url, "?pb2=" + System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req1)));
                                                         
            if (data1 == null) return;
            var res1 = MsgWithIif.Deserialize(data1);
            var x1 = res1.Head;
            if (x.IfSt == 1 && x1.IfSt == 1)
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 保存成功";
            }
        }
        #endregion
    }


    /// <summary>
    /// 树的加载
    /// </summary>
    public partial class SingleGrpViewModel
    {

        #region 左侧终端树

        /// <summary>
        /// 加载左侧节点
        /// </summary>
        private void LoadNodeLeft()
        {
            //var lst = RequestGroup();
            var rtn = new List<InputInfo>();
            var rootGrp = new List<InputInfo>();
            foreach (var f in GrpItem)
            {
                var rootinfo = LoadNodeGetGrpInfo(f);
                var lst2 = LoadNode2GetSluInfoByGrp(rootinfo, f.GrpId);
                rootGrp.Add(rootinfo);
                rootGrp.AddRange(lst2);
            }
            rtn.AddRange(rootGrp);
            var lst3 = LoadNode3GetCtrlInfoBySlu(rootGrp);
            rtn.AddRange(lst3);
            foreach (var t in lst3)
            {
                var lst4 = LoadNode4GetLampByCtrl(t);
                rtn.AddRange(lst4);
            }
            InitNode(rtn);
        }

        /// <summary>
        /// 获取分组的InputInfo
        /// </summary>
        /// <returns></returns>
        private InputInfo LoadNodeGetGrpInfo(SluGrpSet.GrpInfo grp)
        {
            var inputinfo = new InputInfo(null, null, grp.GrpId, 1, grp.GrpId);
            inputinfo.NodeName1B = grp.GrpId + "-" + grp.GrpName;
            inputinfo.Str1StoreN = grp.GrpName;
            return inputinfo;
        }

        /// <summary>
        /// 获取分组下集中器节点
        /// </summary>
        /// <param name="grpInfo"></param>
        /// <param name="grpId"></param>
        /// <param name="rooTreeViewBaseNode"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode2GetSluInfoByGrp(InputInfo grpInfo, int grpId,
                                                         TreeViewBaseNode rooTreeViewBaseNode = null)
        {
            var lstInput = new List<InputInfo>();
            foreach (var slu in SluItem)
            {
                var inputinfo = new InputInfo(grpInfo, rooTreeViewBaseNode, slu.SluId, 2, slu.SluId, grpId);
                foreach (var ctrl in slu.CtrlsInfo)
                {
                    if (ctrl.LampBelongGrp.Contains(grpId))
                    {
                        inputinfo.NodeName1B = slu.SluPhyId + "-" + slu.SluName;
                        inputinfo.Id1StoreN = grpId;
                        //inputinfo.Id2StoreN = ctrl.CtrlId;
                        //inputinfo.Id3StoreN = ctrl.CtrlPhyid;
                        //inputinfo.Str1StoreN = ctrl.CtrlName;
                        inputinfo.Id4StoreN = slu.SluId;
                        lstInput.Add(inputinfo);
                        break;
                    }
                }
            }
            return lstInput;
        }

        /// <summary>
        /// 获取指定集中器节点
        /// </summary>
        /// <param name="grpInfo"></param>
        /// <param name="grpId"></param>
        /// <param name="sluId"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode2GetSluInfoByGrp(InputInfo grpInfo, int grpId, int sluId)
        {
            var lstInput = new List<InputInfo>();
            foreach (var slu in SluItem)
            {
                if (slu.SluId != sluId) continue;
                var inputinfo = new InputInfo(grpInfo, null, slu.SluId, 2, slu.SluId, grpId);

                inputinfo.NodeName1B = slu.SluPhyId + "-" + slu.SluName;
                inputinfo.Id1StoreN = grpId;
                inputinfo.Id4StoreN = slu.SluId;
                lstInput.Add(inputinfo);

            }
            return lstInput;
        }

        /// <summary>
        /// 获取集中器下控制器节点
        /// </summary>
        /// <param name="sluInfo"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode3GetCtrlInfoBySlu(List<InputInfo> sluInfo)
        {
            var lstInput = new List<InputInfo>();
            foreach (var info in sluInfo)
            {
                if (info.Key1TypeN != 2) continue;
                foreach (var slu in SluItem)
                {
                    if (info.Id4StoreN != slu.SluId) continue;
                    foreach (var ctrl in slu.CtrlsInfo)
                    {
                        if (ctrl.LampBelongGrp.Contains(info.Id1StoreN))
                        {
                            var inputinfo = new InputInfo(info, null, ctrl.CtrlPhyid, 3, ctrl.CtrlId, slu.SluId,
                                              info.Id1StoreN);
                            inputinfo.NodeName1B = ctrl.CtrlPhyid + "-" + ctrl.CtrlName;
                            inputinfo.Id1StoreN = info.Id1StoreN;
                            inputinfo.Id2StoreN = ctrl.CtrlId;
                            inputinfo.Id3StoreN = slu.SluId;
                            lstInput.Add(inputinfo);
                        }
                    }
                }
            }

            //foreach (var info in sluInfo)
            //{
            //    var inputinfo = new InputInfo(info, null, info.Id2StoreN, 3, info.Id2StoreN, info.Id4StoreN,
            //                                  info.Id1StoreN);
            //    if (info.Key1TypeN != 2) continue;
            //    inputinfo.NodeName1B = info.Id3StoreN + "-" + info.Str1StoreN;
            //    inputinfo.Id1StoreN = info.Id1StoreN;

            //    lstInput.Add(inputinfo);
            //}
            return lstInput;
        }

        /// <summary>
        /// 获取指定控制器节点
        /// </summary>
        /// <param name="sluInfo"></param>
        /// <param name="ctrlId"></param>
        /// <param name="ctrlPhyId"></param>
        /// <param name="ctrlName"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode3GetCtrlInfoBySlu(List<InputInfo> sluInfo, int ctrlId, int ctrlPhyId,
                                                          string ctrlName)
        {
            var lstInput = new List<InputInfo>();
            foreach (var info in sluInfo)
            {
                if (info.Key1TypeN != 2) continue;
                var inputinfo = new InputInfo(info, null, ctrlPhyId, 3, ctrlId, info.Id4StoreN, info.Id1StoreN);
                inputinfo.NodeName1B = ctrlPhyId + "-" + ctrlName;
                inputinfo.Id1StoreN = info.Id1StoreN;
                lstInput.Add(inputinfo);
            }
            return lstInput;
        }

        /// <summary>
        /// 获取控制器下灯头节点
        /// </summary>
        /// <param name="ctrlInfo"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode4GetLampByCtrl(InputInfo ctrlInfo)
        {
            var lstInput = new List<InputInfo>();
            foreach (var slu in SluItem)
            {
                if (ctrlInfo.Id3StoreN != slu.SluId) continue;
                foreach (var ctrl in slu.CtrlsInfo)
                {
                    if (ctrlInfo.Id2StoreN != ctrl.CtrlId) continue;
                    for (int i = 0; i < ctrl.LampCount; i++)
                    {
                        if (ctrl.LampBelongGrp[i] != ctrlInfo.Id1StoreN) continue;
                        var inputinfo = new InputInfo(ctrlInfo, null, i, 4, i, ctrl.CtrlId, slu.SluId,
                                                      ctrlInfo.Id1StoreN);
                        switch (i)
                        {
                            case 0:
                                inputinfo.NodeName1B = "灯头1";
                                break;
                            case 1:
                                inputinfo.NodeName1B = "灯头2";
                                break;
                            case 2:
                                inputinfo.NodeName1B = "灯头3";
                                break;
                            case 3:
                                inputinfo.NodeName1B = "灯头4";
                                break;
                        }
                        lstInput.Add(inputinfo);
                    }
                }
            }
            return lstInput;
        }

        /// <summary>
        /// 获取指定灯头节点
        /// </summary>
        /// <param name="ctrlInfo"></param>
        /// <param name="lamp"></param>
        /// <param name="ctrlId"></param>
        /// <param name="sluId"></param>
        /// <param name="grpId"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode4GetLampByCtrl(InputInfo ctrlInfo, List<int> lamp, int ctrlId, int sluId,
                                                       int grpId)
        {
            var lstInput = new List<InputInfo>();
            for (int i = 0; i < lamp.Count; i++)
            {
                var inputinfo = new InputInfo(ctrlInfo, null, i, 4, i, ctrlId, sluId, grpId);
                if (lamp[i] != ctrlInfo.Id1StoreN) continue;
                switch (i)
                {
                    case 0:
                        inputinfo.NodeName1B = "灯头1";
                        break;
                    case 1:
                        inputinfo.NodeName1B = "灯头2";
                        break;
                    case 2:
                        inputinfo.NodeName1B = "灯头3";
                        break;
                    case 3:
                        inputinfo.NodeName1B = "灯头4";
                        break;
                }
                lstInput.Add(inputinfo);
            }
            return lstInput;
        }

        #endregion
    }

    /// <summary>
    /// 函数
    /// </summary>
    public partial class SingleGrpViewModel
    {
        /// <summary>
        /// 获取区域下所有分组
        /// </summary>
        public void RequestGroup()
        {
            GrpItem.Clear();
            var req = new MsgWithIif()
                          {
                              Post4078 = new SluGrpSet()
                          };
            req.Post4078.AreaId = AreaId;
            req.Post4078.Op = 1;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4078", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            if (data == null) return;
            var res = MsgWithIif.Deserialize(data);
            foreach (var f in res.Back4078.ItemsGrp)
            {
                GrpItem.Add(f);
            }
        }

        /// <summary>
        /// 获取区域下所有集中器
        /// </summary>
        /// <returns></returns>
        public void RequestList()
        {
            SluBref.Clear();
            var req = new MsgWithIif()
                          {
                              Post4078 = new SluGrpSet()
                          };
            req.Post4078.AreaId = AreaId;
            req.Post4078.Op = 2;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4078", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
            if (data == null) return;
            var res = MsgWithIif.Deserialize(data);
            //foreach (var slu in res.Back4078.ItemsLst)
            //{
            //    var name = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(slu);

            //}
            
            foreach (var f in res.Back4078.ItemsLst)
            {
                SluBref.Add(f);
            }
        }

        /// <summary>
        /// 获取集中器和控制器详细信息
        /// </summary>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public void RequestCtrlList(ObservableCollection<int> itemList)
        {
            var req = new MsgWithIif()
            {
                Post4078 = new SluGrpSet()
            };
            req.Post4078.AreaId = AreaId;
            req.Post4078.Op = 3;
            //var sluItem = new List<SluGrpSet.SluInfoItem>();
            SluItem.Clear();
            var index = 0;
            for (int i = 0; i < itemList.Count; i = i + 10)
            {
                var list = new List<int>();
                for (int j = index * 10; j < (itemList.Count > i + 10 ? 10 : itemList.Count); j++)
                {
                    list.Add(itemList[j]);
                }
                req.Post4078.ItemsLst = list;
                var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4078", System.Convert.ToBase64String(MsgWithIif.SerializeToBytes(req)));
                if (data == null) return;
                var res = MsgWithIif.Deserialize(data);
                foreach (var slu in res.Back4078.ItemsSlu)
                {
                    SluItem.Add(slu);
                }
                index++;
            }
        }

        //查询左侧树
        private void SearchNode(string keyWord)
        {
            ChildTreeItemsLeftSearch.Clear();
            if (keyWord == "")
            {
                IsLeftSearchTreeVisi = Visibility.Collapsed;
                IsLeftTreeVisi=Visibility.Visible;
                ChildTreeItemsLeftSearch.Clear();
                return;
            }
            var items = (from i in DicItems orderby i.Key.Item1 select i).ToList();
            foreach (var l in items)

            IsLeftTreeVisi = Visibility.Collapsed;
            IsLeftSearchTreeVisi = Visibility.Visible;
        }
    }
}
