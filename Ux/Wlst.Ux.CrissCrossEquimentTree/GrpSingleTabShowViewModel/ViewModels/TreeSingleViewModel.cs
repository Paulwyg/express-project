using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel;
using Wlst.Ux.CrissCrossEquipemntTree.GrpSingleTabShowViewModel.Services;
using Wlst.Ux.CrissCrossEquipemntTree.Models;
using Wlst.Ux.CrissCrossEquipemntTree.Resources;
using Wlst.client;
using Wlst.Sr.EquipmentInfoHolding.Model;
using System.IO;
using System.Text;

namespace Wlst.Ux.CrissCrossEquipemntTree.GrpSingleTabShowViewModel.ViewModels
{
    [Export(typeof(IISingleTree))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeSingleViewModel : ObservableObject, IISingleTree
    {
        //private  Dictionary<int, TreeNodeBaseNode> groupTreeNodes = new Dictionary<int, TreeNodeBaseNode>();
        // private Dictionary<int, TreeNodeBaseNode> tmlTreeNodes = new Dictionary<int, TreeNodeBaseNode>();
        public event EventHandler<NodeSelectedArgs> OnClearSerchTest;
        private int _hxxx = 0;
        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightTree;
                    if (_hxxx > 24) _hxxx = 24;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }

        public static TreeSingleViewModel MySelf;
        public TreeSingleViewModel()
        {

            if (MySelf == null) MySelf = this;
            IsLoadOnlyOneArea = true;
            EventPublish.AddEventTokener(
                 Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(LoadNode, 1);
            //LoadNode();
            //LoadNodeOther();
            LoadXml();
            IsSearchTreeVisi = Visibility.Collapsed;
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Update, 1, DelayEventHappen.EventOne);

            IsVir = true;
            IsUserX = Visibility.Collapsed;
            if (UserInfo.UserLoginInfo.D == true || UserInfo.UserLoginInfo.AreaX.Count != 0) IsUserX = Visibility.Visible;

            this.IsNotShowFastControl = UxTreeSetting.IsRutsNotShowNullK != 0 ? 80 : 0;
            this.IsNotShowFastControlToCenter = UxTreeSetting.IsRutsNotShowNullK != 0 ? 23 : 0;

            //Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("null", 8888, DateTime.Now.Ticks,  ExSearchNode,10);
        }

        public int timer_count = 0;

        void ExSearchNode(object obj)
        {
            if (StartSearch == true)
            {

                timer_count++;

                if (timer_count > 10)
                {
                    StartSearch = false;
                    timer_count = 0;
                    SearchNode(_searchText);
                }
            }
        }
        private void LoadXml()
        {
            var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("LdTabTreeSetConfg");
            if (infos.ContainsKey("SearchLimit"))
            {
                SearchLimit = Convert.ToInt32(infos["SearchLimit"]);
            }
            else SearchLimit = 0;


            var infoss = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("AreaIndex");
            if (infoss.ContainsKey("AreaIndex"))
            {
                areaIndex.Clear();
                //var areaCount = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count;
                var areatmp = infoss["AreaIndex"];
                string[] areaLstt = areatmp.Split('-');
                if (areaLstt.Count() == 0) return;
                foreach (var g in areaLstt)
                {

                    if (string.IsNullOrEmpty(g) || areaIndex.Contains(Convert.ToInt32(g))) continue;
                    areaIndex.Add(Convert.ToInt32(g));

                }
            }
            else
            {
                var info = new Dictionary<string, string>();
                info.Add("AreaIndex", "0-1-2-3");
                Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, "AreaIndex");
            }


            string path = Directory.GetCurrentDirectory() + "\\Config" + "\\" + "City.txt";
            string rrr = "";
            if (File.Exists(path))
            {
                var sr = new StreamReader(path, Encoding.Default);

                rrr = sr.ReadLine();

                sr.Close();
            }
            Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum = Convert.ToInt16(rrr);

        }
        private bool LoadXmldata() //crc
        {
            int x = 0;
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("SystemCommonSetConfg");
            if (info.ContainsKey("IsNotShowFastControl"))
            {
                try
                {
                    x = Convert.ToInt32(info["IsNotShowFastControl"]);
                }
                catch (Exception ex)
                {

                }
            }
            return x == 1;
        }

        List<int> areaLst = new List<int>();
        /// <summary>
        /// 区域呈现排序
        /// </summary>
        private List<int> areaIndex = new List<int>();
        #region load node

        public bool IsLoadOnlyOneArea = false;
        //加载终端节点
        private void LoadNode()
        {
            if (UserInfo.UserLoginInfo.D == true || UserInfo.UserLoginInfo.AreaX.Count != 0) IsUserX = Visibility.Visible;
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return;

            //if (Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.RegionRtuInfo.Count == 0) return;

            this.ChildTreeItems.Clear();


            var userProperty = UserInfo.UserLoginInfo;

            if (userProperty.D == true)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0) return;
                IsLoadOnlyOneArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2;
                if (IsLoadOnlyOneArea)
                {
                    int AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
                    var grp =
                        (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                         where t.Key.Item1 == AreaId
                         orderby t.Value.GroupId
                         select t.Value).ToList();

                    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));


                    foreach (var f in grp)
                    {
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f.AreaId, f.GroupId,
                                                                                     TypeOfTabTreeNode.IsGrp));
                    }
                    var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                    var rtuLst = new List<int>();
                    foreach (var v in sp)
                    {
                        if (v < 1099999) rtuLst.Add(v);
                    }
                    if (rtuLst.Count > 0)
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0,
                                                                                     TypeOfTabTreeNode.IsGrpSpecial));
                }
                else
                {
                    //var areaorder = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys;
                    //areaorder.Clear();
                    //foreach (var g in areaIndex)
                    //{
                    //    if (areaorder.Contains(g))
                    //}
                    var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys;
                    //if (areaIndex.Count != areaInfo.Count)
                    //{
                    foreach (var g in areaInfo)
                    {
                        if (!areaIndex.Contains(g)) areaIndex.Add(g);
                    }
                    //}
                    foreach (var f in areaIndex)//Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys
                    {
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            if (fff < 1099999) rtuLst.Add(fff);
                        }
                        if (rtuLst.Count == 0) continue;
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f, 0,
                                                                                     TypeOfTabTreeNode.IsArea));
                    }
                }
            }
            else
            {
                areaLst.Clear();
                foreach (var t in userProperty.AreaX)
                {
                    if (t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }
                //areaLst.AddRange(userProperty.AreaX);
                foreach (var t in userProperty.AreaW)
                {
                    if (!areaLst.Contains(t) && t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }
                foreach (var f in userProperty.AreaR)
                {
                    if (!areaLst.Contains(f) && f >= 0)
                    {
                        areaLst.Add(f);
                    }
                }

                IsLoadOnlyOneArea = areaLst.Count < 2;

                if (IsLoadOnlyOneArea)
                {
                    int AreaId = areaLst[0];
                    var grp =
                        (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                         where t.Key.Item1 == AreaId
                         orderby t.Value.GroupId
                         select t.Value).ToList();

                    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));
                    foreach (var f in grp)
                    {
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f.AreaId, f.GroupId,
                                                                                     TypeOfTabTreeNode.IsGrp));
                    }
                    var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                    var rtuLst = new List<int>();
                    foreach (var v in sp)
                    {
                        if (v < 1099999) rtuLst.Add(v);
                    }
                    if (rtuLst.Count > 0)
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0,
                                                                                     TypeOfTabTreeNode.IsGrpSpecial));
                }
                else
                {

                    foreach (var g in areaLst)
                    {
                        if (!areaIndex.Contains(g)) areaIndex.Add(g);
                    }

                    foreach (var f in areaIndex)//areaLst
                    {
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            if (fff < 1099999) rtuLst.Add(fff);
                        }
                        if (rtuLst.Count == 0) continue;
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                    }
                }
            }
            foreach (var f in ChildTreeItems)
                f.GetChildRtuCount();

            //for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            //{
            //    if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
            //    {
            //        this.ChildTreeItems.RemoveAt(i);
            //    }
            //}

            //删除0子节点的父节点
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
                else
                {
                    if (!IsLoadOnlyOneArea)
                    {
                        for (int j = this.ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                        {
                            if (this.ChildTreeItems[i].ChildTreeItems[j].RtuCount == 0)
                            {
                                this.ChildTreeItems[i].DeleteChild(j);
                                continue;
                            }
                            for (int k = this.ChildTreeItems[i].ChildTreeItems[j].ChildTreeItems.Count - 1; k >= 0; k--)
                            {
                                if (this.ChildTreeItems[i].ChildTreeItems[j].NodeType == TypeOfTabTreeNode.IsAll) continue;
                                if (this.ChildTreeItems[i].ChildTreeItems[j].ChildTreeItems[k].RtuCount == 0)
                                {
                                    if (this.ChildTreeItems[i].ChildTreeItems[j].ChildTreeItems[k].NodeType == TypeOfTabTreeNode.IsTml) continue;
                                    this.ChildTreeItems[i].ChildTreeItems[j].DeleteChild(k);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsAll) continue;
                        for (int j = this.ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                        {
                            if (this.ChildTreeItems[i].ChildTreeItems[j].NodeType == TypeOfTabTreeNode.IsTml) continue;
                            if (this.ChildTreeItems[i].ChildTreeItems[j].RtuCount == 0)
                            {
                                this.ChildTreeItems[i].DeleteChild(j);
                            }

                        }

                    }
                }
            }

            //Update();





            //////int AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
            //////this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));


            //////var normalGrps = ServiceGrpRegionInfoHold.GetGrpByType(AreaId,2);
            ////////先加载全局分组  再加载地区分组

            //////foreach (var f in normalGrps)
            //////{
            //////    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f.Key,f.Value,
            //////                                                                 TypeOfTabTreeNode.IsGrp));
            //////}

            //////var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            //////var rtuLst = new List<int>();
            //////foreach (var v in sp)
            //////{
            //////    if (v < 1099999) rtuLst.Add(v);
            //////}
            //////if (rtuLst.Count > 0)
            //////    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0,
            //////                                                                 TypeOfTabTreeNode.IsGrpSpecial));
        }

        //加载终端节点
        private void LoadNodeOther()
        {
            return;
            if (UserInfo.UserLoginInfo.D == true || UserInfo.UserLoginInfo.AreaX.Count != 0) IsUserX = Visibility.Visible;
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return;

            //if (Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.RegionRtuInfo.Count == 0) return;

            this.OtherChildTreeItems.Clear();


            var userProperty = UserInfo.UserLoginInfo;

            if (userProperty.D == true)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0) return;
                IsLoadOnlyOneArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2;
                if (IsLoadOnlyOneArea)
                {
                    int AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
                    var grp =
                        (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                         where t.Key.Item1 == AreaId
                         orderby t.Value.GroupId
                         select t.Value).ToList();

                    this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));

                    var regionGrps = ServiceGrpRegionInfoHold.GetGrpByType(AreaId, 1);
                    //先加载全局分组  再加载地区分组
                    if (regionGrps == null || regionGrps.Count == 0) return;

                    var regionsGrpsOrder = (from t in regionGrps orderby t.Key select t).ToList();

                    foreach (var f in regionsGrpsOrder)
                    {
                        this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, AreaId,f.Key,f.Value,
                                                                                     TypeOfTabTreeNode.IsRegion));
                    }
                    var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                    var rtuLst = new List<int>();
                    foreach (var v in sp)
                    {
                        if (v < 1099999) rtuLst.Add(v);
                    }
                    if (rtuLst.Count > 0)
                        this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, AreaId, 0,
                                                                                     TypeOfTabTreeNode.IsGrpSpecial));
                }
                else
                {
                    //var areaorder = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys;
                    //areaorder.Clear();
                    //foreach (var g in areaIndex)
                    //{
                    //    if (areaorder.Contains(g))
                    //}
                    var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys;
                    //if (areaIndex.Count != areaInfo.Count)
                    //{
                    foreach (var g in areaInfo)
                    {
                        if (!areaIndex.Contains(g)) areaIndex.Add(g);
                    }
                    //}
                    foreach (var f in areaIndex)//Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys
                    {
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            if (fff < 1099999) rtuLst.Add(fff);
                        }
                        if (rtuLst.Count == 0) continue;
                        this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, f, 0,
                                                                                     TypeOfTabTreeNode.IsArea));
                    }
                }
            }
            else
            {
                areaLst.Clear();
                foreach (var t in userProperty.AreaX)
                {
                    if (t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }
                //areaLst.AddRange(userProperty.AreaX);
                foreach (var t in userProperty.AreaW)
                {
                    if (!areaLst.Contains(t) && t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }
                foreach (var f in userProperty.AreaR)
                {
                    if (!areaLst.Contains(f) && f >= 0)
                    {
                        areaLst.Add(f);
                    }
                }

                IsLoadOnlyOneArea = areaLst.Count < 2;
                if (IsLoadOnlyOneArea)
                {
                    int AreaId = areaLst[0];
                    var grp =
                        (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                         where t.Key.Item1 == AreaId
                         orderby t.Value.GroupId
                         select t.Value).ToList();

                    this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));


                    var regionGrps = ServiceGrpRegionInfoHold.GetGrpByType(AreaId, 1);
                    //先加载全局分组  再加载地区分组
                    if (regionGrps == null || regionGrps.Count == 0) return;

                    var regionsGrpsOrder = (from t in regionGrps orderby t.Key select t).ToList();

                    foreach (var f in regionsGrpsOrder)
                    {
                        this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, AreaId, f.Key, f.Value,
                                                                                     TypeOfTabTreeNode.IsRegion));
                    }

                    //foreach (var f in grp)
                    //{
                    //    this.OtherChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f.AreaId, f.GroupId,
                    //                                                                 TypeOfTabTreeNode.IsGrp));
                    //}
                    var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                    var rtuLst = new List<int>();
                    foreach (var v in sp)
                    {
                        if (v < 1099999) rtuLst.Add(v);
                    }
                    if (rtuLst.Count > 0)
                        this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, AreaId, 0,
                                                                                     TypeOfTabTreeNode.IsGrpSpecial));
                }
                else
                {

                    foreach (var g in areaLst)
                    {
                        if (!areaIndex.Contains(g)) areaIndex.Add(g);
                    }

                    foreach (var f in areaIndex)//areaLst
                    {
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            if (fff < 1099999) rtuLst.Add(fff);
                        }
                        if (rtuLst.Count == 0) continue;
                        this.OtherChildTreeItems.Add(new TreeNodeItemRegionGroupViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                    }
                }
            }
            foreach (var f in OtherChildTreeItems)
                f.GetChildRtuCount();

            for (int i = this.OtherChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.OtherChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.OtherChildTreeItems.RemoveAt(i);
                }
            }

            Update();





            //////int AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
            //////this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));


            //////var normalGrps = ServiceGrpRegionInfoHold.GetGrpByType(AreaId,2);
            ////////先加载全局分组  再加载地区分组

            //////foreach (var f in normalGrps)
            //////{
            //////    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f.Key,f.Value,
            //////                                                                 TypeOfTabTreeNode.IsGrp));
            //////}

            //////var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            //////var rtuLst = new List<int>();
            //////foreach (var v in sp)
            //////{
            //////    if (v < 1099999) rtuLst.Add(v);
            //////}
            //////if (rtuLst.Count > 0)
            //////    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, AreaId, 0,
            //////                                                                 TypeOfTabTreeNode.IsGrpSpecial));
        }

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
        public void UpdateArea(int areaId)
        {

            var info = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(areaId);
            //.Values.ToList();
            if (info == null)
            {
                this.ChildTreeItems.Clear();
                //if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }

            //this.NodeName = info.AreaName;

            var gprlst = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                          where t.Key.Item1 == areaId
                          select t.Key.Item2).ToList();
            //var spe = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            //if(spe .Count >0) 
            //   gprlst.Add(0);


            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeId == 0) continue;
                if (gprlst.Contains(ChildTreeItems[i].NodeId) == false) ChildTreeItems.RemoveAt(i);
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsGrpSpecial &&
                    ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsGrp &&
                    ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsAll)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }


            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            var lstUp = new List<int>();
            foreach (var t in gprlst)
            {
                if (exist.Contains(t))
                {
                    lstUp.Add(t);
                    continue;
                }

                var para = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(areaId, t);
                if (para == null) continue;

                if (para.LstTml.Count == 0) continue;
                ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, areaId, t, TypeOfTabTreeNode.IsGrp));
            }

            foreach (var f in this.ChildTreeItems)
            {
                if (!lstUp.Contains(f.NodeId)) continue;
                f.ReUpdate(0);
            }

            TreeNodeBaseNode spe = null;
            var spelst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(areaId);
            foreach (var f in this.ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    f.ReUpdate(0);
                }
                if (f.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                {
                    if (spelst.Count == 0)
                    {
                        this.ChildTreeItems.Remove(f);
                        break;
                    }
                    f.ReUpdate(0);
                    spe = f;
                }
            }

            if (spe == null && spelst.Count > 0)
            {
                this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, areaId, 0,
                                                                             TypeOfTabTreeNode.IsGrpSpecial));
            }
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }
        }

        public void UpdateRtu()
        {
            if (this.ChildTreeItems.Count == 0)
            {
                this.LoadNode();
                this.LoadNodeOther();
                return;
            }
            //bool onlyone = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2;
            //if (IsLoadOnlyOneArea != onlyone)
            //{
            //    this.ChildTreeItems.Clear();
            //    this.LoadNode();
            //    return;
            //}
            //if (IsLoadOnlyOneArea)
            //{
            //    int areaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0
            //                     ? 0
            //                     : Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
            //    UpdateArea(areaId);
            //}
            //else
            //{
            //    var areas = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList();
            //    for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            //    {
            //        if (areas.Contains(ChildTreeItems[i].NodeId) ||
            //            ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsArea)
            //        {
            //            this.ChildTreeItems.RemoveAt(i);
            //        }
            //        else
            //        {
            //            ChildTreeItems[i].ReUpdate(0);
            //        }
            //    }

            //    var ars = (from t in ChildTreeItems select t.NodeId).ToList();
            //    foreach (var f in areas)
            //    {
            //        if (ars.Contains(f)) continue;
            //        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
            //    }

            //}

            foreach (var f in ChildTreeItems)
            {
                f.GetChildRtuCount();

            }
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
                else
                {
                    if (!IsLoadOnlyOneArea)
                    {
                        for (int j = this.ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                        {
                            if (this.ChildTreeItems[i].ChildTreeItems[j].RtuCount == 0)
                            {
                                this.ChildTreeItems[i].DeleteChild(j);
                            }
                        }
                    }
                }
            }
        }

        public void Update()
        {
            if (this.ChildTreeItems.Count == 0)
            {
                this.LoadNode();
                this.LoadNodeOther();
                return;
            }

            foreach (var f in ChildTreeItems)
            {
                f.GetChildRtuCount();

            }
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0 )
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
                else
                {
                    if (!IsLoadOnlyOneArea)
                    {
                        for (int j = this.ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                        {
                            if (this.ChildTreeItems[i].ChildTreeItems[j].RtuCount == 0)
                            {
                                this.ChildTreeItems[i].DeleteChild(j);
                                continue;
                            }
                            for (int k = this.ChildTreeItems[i].ChildTreeItems[j].ChildTreeItems.Count - 1; k >= 0; k--)
                            {
                                if (this.ChildTreeItems[i].ChildTreeItems[j].NodeType == TypeOfTabTreeNode.IsAll) continue;
                                if (this.ChildTreeItems[i].ChildTreeItems[j].ChildTreeItems[k].RtuCount == 0)
                                {
                                    if (this.ChildTreeItems[i].ChildTreeItems[j].ChildTreeItems[k].NodeType == TypeOfTabTreeNode.IsTml) continue;
                                    this.ChildTreeItems[i].ChildTreeItems[j].DeleteChild(k);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsAll) continue;
                        for (int j = this.ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                        {
                            if (this.ChildTreeItems[i].ChildTreeItems[j].NodeType == TypeOfTabTreeNode.IsTml) continue;
                            if (this.ChildTreeItems[i].ChildTreeItems[j].RtuCount == 0)
                            {
                                this.ChildTreeItems[i].DeleteChild(j);
                            }
                          
                        }

                    }
                }
            }
        }

        //private List<int> _grpList;
        ///// <summary>
        ///// 终端树中的所有分组
        ///// </summary>
        //public List<int> GrpList
        //{
        //    get
        //    {
        //        return _grpList;
        //    }
        //    set
        //    {
        //        if (value == _grpList) return;
        //        _grpList = value;
        //        this.RaisePropertyChanged(() => this.GrpList);
        //    }
        //}

        //private List<int> _areaList;
        ///// <summary>
        ///// 终端树中的所有区域
        ///// </summary>
        //public List<int> AreaList
        //{
        //    get
        //    {
        //        return _areaList;
        //    }
        //    set
        //    {
        //        if (value == _areaList) return;
        //        _areaList = value;
        //        this.RaisePropertyChanged(() => this.AreaList);
        //    }
        //}




        public void UpdateAreaGrp()
        {

            var areas = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo;
            var areaown = new Dictionary<int, AreaInfo.AreaItem>();
            foreach (var t in areas)
            {
                foreach (var f in areaLst)
                    if (t.Key == f)
                    {
                        areaown.Add(t.Key, t.Value);
                    }
            }
            var areaKeys = areas.Keys.ToList();
            var AreaList = new List<int>();

            var grpList = new List<Tuple<int, int>>();
            var LstGrp = new List<int>();
            var areagrp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups;
            var grpKeys = new List<int>();

            //更新分组名称
            foreach (var v in areagrp)
            {
                grpKeys.Add(v.Key.Item2);
                foreach (var vvv in ChildTreeItems)
                {
                    if (vvv.NodeType == TypeOfTabTreeNode.IsGrp)
                    {
                        if (!grpList.Contains(new Tuple<int, int>(vvv.Father.NodeId, vvv.NodeId))) grpList.Add(new Tuple<int, int>(vvv.Father.NodeId, vvv.NodeId));
                    }
                    foreach (var x in vvv.ChildTreeItems)
                    {

                        if (x.NodeId == v.Key.Item2 && x.NodeName != v.Value.GroupName)
                        {
                            x.NodeName = v.Value.GroupName;
                        }
                        if (x.NodeType == TypeOfTabTreeNode.IsGrp)
                        {
                            //if (GrpList.Contains(x.NodeId)) continue;
                            //GrpList.Add(x.NodeId);
                            if (!grpList.Contains(new Tuple<int, int>(x.Father.NodeId, x.NodeId))) grpList.Add(new Tuple<int, int>(x.Father.NodeId, x.NodeId));
                        }

                    }
                }
            }
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsArea)
                {
                    AreaList.Add(ChildTreeItems[i].NodeId);
                }
                //更新区域名称
                foreach (var f in areas)
                {

                    if (ChildTreeItems[i].NodeId == f.Value.AreaId &&
                        ChildTreeItems[i].NodeName != f.Value.AreaName &&
                        ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsArea)
                    {
                        ChildTreeItems[i].NodeName = f.Value.AreaName;
                    }
                }
                //删除已不存在的区域
                if (!areaKeys.Contains(ChildTreeItems[i].NodeId) &&
                    ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsArea)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }

                //删除已不存在的分组               
                for (int j = ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                {
                    if (!grpKeys.Contains(ChildTreeItems[i].ChildTreeItems[j].NodeId) &&
                        ChildTreeItems[i].ChildTreeItems[j].NodeType == TypeOfTabTreeNode.IsGrp)
                    {
                        ChildTreeItems[i].DeleteChild(j);
                        grpList.Remove(new Tuple<int, int>(ChildTreeItems[i].NodeId, ChildTreeItems[i].ChildTreeItems[j].NodeId));
                    }
                }
            }

        #endregion

            //增加新建的区域
            if (!IsLoadOnlyOneArea && Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count > 1)
            {
                foreach (var v in areas)
                {
                    if (!AreaList.Contains(v.Key))
                    {
                        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null, v.Key, 0,
                                                                                     TypeOfTabTreeNode.IsArea));
                        AreaList.Add(v.Key);
                    }
                }
            }



            //增加新建的分组
            foreach (var v in areagrp)
            {
                if (!grpList.Contains(v.Key))
                {
                    for (int i = ChildTreeItems.Count - 1; i >= 0; i--)
                    {
                        if (ChildTreeItems[i].NodeId == v.Key.Item1 && ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsArea)
                        {
                            ChildTreeItems[i].ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(
                                                                     ChildTreeItems[i], v.Key.Item1, v.Key.Item2,
                                                                     TypeOfTabTreeNode.IsGrp));
                        }
                        if (ChildTreeItems[i].NodeId == v.Key.Item2 && ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsGrp)
                        {
                            ChildTreeItems[i].ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(
                                                                     null, v.Key.Item1, v.Key.Item2,
                                                                     TypeOfTabTreeNode.IsGrp));
                        }
                    }
                }
            }

            //foreach (var v in grpKeys)
            //{
            //    if (!GrpList.Contains(v))
            //    {

            //        ChildTreeItems[v / 1000].ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(ChildTreeItems[v / 1000], v / 1000, v,
            //                                                                     TypeOfTabTreeNode.IsGrp));
            //        GrpList.Add(v);
            //    }
            //}

            //if (IsLoadOnlyOneArea)
            //{
            //    foreach (var v in grpKeys)
            //    {
            //        if (!GrpList.Contains(v))
            //        {
            //            ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null,UserInfo.UserLoginInfo.AreaR[0], v, TypeOfTabTreeNode.IsGrp));
            //        }
            //    }
            //}

            foreach (var f in ChildTreeItems) f.AddChild();


            foreach (var f in ChildTreeItems)
            {

                f.GetChildRtuCount();
            }
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
                else
                {
                    if (!IsLoadOnlyOneArea)
                    {
                        for (int j = this.ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--) // 清除空的分组
                        {
                            if (this.ChildTreeItems[i].ChildTreeItems[j].RtuCount == 0)
                            {
                                this.ChildTreeItems[i].DeleteChild(j);
                            }
                        }
                    }
                }
            }

        }


        #region tab iinterface
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get
            {
                return "类型地域";
            }
        }


        public bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        private ObservableCollection<TreeNodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                }
            }
        }



        private ObservableCollection<TreeNodeBaseNode> _otherChildTreeItemsInfo;
        /// <summary>
        /// 另一种交叉分组
        /// </summary>
        public ObservableCollection<TreeNodeBaseNode> OtherChildTreeItems
        {
            get
            {
                if (_otherChildTreeItemsInfo == null)
                    _otherChildTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _otherChildTreeItemsInfo;
            }
            set
            {
                if (value != _otherChildTreeItemsInfo)
                {
                    _otherChildTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.OtherChildTreeItems);
                }
            }
        }


        /// <summary>
        /// 屏蔽快速操作和控制
        /// </summary>
        private int _isNotShowFastControl;

        public int IsNotShowFastControl
        {
            get
            {
                return _isNotShowFastControl;
            }
            set
            {
                if (value != _isNotShowFastControl)
                {
                    _isNotShowFastControl = value;
                    this.RaisePropertyChanged(() => this.IsNotShowFastControl);
                }
            }
        }

        private int _isNotShowFastControltocenter;

        public int IsNotShowFastControlToCenter
        {
            get
            {
                return _isNotShowFastControltocenter;
            }
            set
            {
                if (value != _isNotShowFastControltocenter)
                {
                    _isNotShowFastControltocenter = value;
                    this.RaisePropertyChanged(() => this.IsNotShowFastControlToCenter);
                }
            }
        }


        private EventHandler<NodeSelectedArgs> OnSelectedNodeByCodeIns;
        event EventHandler<NodeSelectedArgs> IISingleTree.OnSelectedNodeByCode
        {
            add { OnSelectedNodeByCodeIns += value; }
            remove { if (OnSelectedNodeByCodeIns != null) OnSelectedNodeByCodeIns -= value; }
        }


    };

    //search
    public partial class TreeSingleViewModel
    {

        private ObservableCollection<TreeNodeBaseNode> _searchchildTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItemsSearch
        {
            get
            {
                if (_searchchildTreeItemsInfo == null)
                    _searchchildTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _searchchildTreeItemsInfo;
            }
            set
            {
                if (value != _searchchildTreeItemsInfo)
                {
                    _searchchildTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItemsSearch);
                }
            }
        }

        #region Search Node

        private bool StartSearch = false;
        private int SearchLimit = 0;

        private string _searchText;
        public delegate void SearchNodeInvoke(string text);

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    this.RaisePropertyChanged(() => this.SearchText);
                    if (SearchLimit == 1) return;
                    SearchNodeold(_searchText);

                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        var ins = new PublishEventArgs()
                        {
                            EventType = PublishEventType.Core,
                            EventId =
                                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                RtuGroupSelectdWantedMapUp
                        };
                        EventPublish.PublishEvent(ins);
                    }

                    //////StartSearch = true;
                    //////timer_count = 0;
                    //if (UxTreeSetting.IsShowRapidOp == 1)
                    //{

                    //SearchNode(_searchText);
                    //}
                    //else
                    //{
                    //    SearchNodeold(_searchText);
                    //}


                }
            }
        }

        //CmdClearUpSearchText
        #region CmdClearUpSearchText

        private ICommand _cmdCmdClearUpSearchText;

        public ICommand CmdClearUpSearchText
        {
            get
            {
                if (_cmdCmdClearUpSearchText == null)
                    _cmdCmdClearUpSearchText = new RelayCommand(ExCmdClearUpSearchText, CanCmdClearUpSearchText, false);

                return _cmdCmdClearUpSearchText;
            }
        }

        private void ExCmdClearUpSearchText()
        {
            SearchText = "";

            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };

            var info = new List<int>();
            ins.AddParams(info);

            EventPublish.PublishEvent(ins);
        }



        private bool CanCmdClearUpSearchText()
        {
            return !string.IsNullOrEmpty(SearchText);

        }



        #endregion


        #region CmdQuickQuery

        private ICommand _cmdQuickQuery;

        public ICommand CmdQuickQuery
        {
            get
            {
                if (_cmdQuickQuery == null)
                    _cmdQuickQuery = new RelayCommand<string>(ExCmdQuickQuery, CanCmdQuickQuery, false);
                return _cmdQuickQuery;
            }
        }

        private void ExCmdQuickQuery(string s)
        {

            int x = 0;
            if (Int32.TryParse(s, out x))
            {
                if (x == 1)
                {
                    SearchNodeold("掉线");
                }
                else if (x == 2)
                {
                    SearchNodeold("亮灯@-@");
                }
                else if (x == 3)
                {
                    SearchNodeold("灭灯");
                }
                else if (x == 4)
                {
                    SearchNodeold("停电");
                }
                else if (x == 5)
                {
                    SearchNodeold("不用");
                }
            }

        }

        private bool CanCmdQuickQuery(string s)
        {
            return true;
        }


        #endregion


        #region CmdControl

        private ICommand _cmdcontrol;

        public ICommand CmdControl
        {
            get
            {
                if (_cmdcontrol == null)
                    _cmdcontrol = new RelayCommand(ExCmdControl, CanCmdControl, false);
                return _cmdcontrol;
            }
        }

        private void ExCmdControl()
        {
            //Wlst.Cr.CoreOne.Models.MenuItemBase.ExNavWithArgs(
            //                   Wlst.Ux.EmergencyDispatch.Services.ViewIdAssign.NavToLdlViewId,
            //                   0);





            var lst = new List<int>();
            if (tmpList2.Count == 0) return;
            foreach (var t in tmpList2)//ChildTreeItemsSearch
            {
                if (t.NodeType == TypeOfTabTreeNode.IsTml)
                {
                    lst.Add(t.NodeId);
                }
            }

            if (lst.Count < 1)
            {
                //WlstMessageBox.Show("警告", "您未查询出终端，请查询出终端后再控制。", WlstMessageBoxType.Ok);
                //return;
                lst.Add(-1);
                RegionManage.ShowViewByIdAttachRegionWithArgu(Wlst.Ux.WJ3005Module.Services.ViewIdAssign.NavToControlCenterManagDemo2Id, lst);
            }
            else
            {
                RegionManage.ShowViewByIdAttachRegionWithArgu(Wlst.Ux.WJ3005Module.Services.ViewIdAssign.NavToControlCenterManagDemo2Id, lst);
            }

        }


        private bool CanCmdControl()
        {
            return true;
        }


        #endregion


        ObservableCollection<TreeNodeBaseNode> Getallnode(TreeNodeBaseNode node)
        {
            foreach (var f in node.ChildTreeItems) if (f.NodeType == TypeOfTabTreeNode.IsAll) return f.ChildTreeItems;
            return new ObservableCollection<TreeNodeBaseNode>();
        }
        //查询终端

        private ConcurrentQueue<string> searchKeys = new ConcurrentQueue<string>();
        private bool _running = false;
        private void SearchNode(string keyWord)
        {
            searchKeys.Enqueue(keyWord);
            Application.Current.Dispatcher.Invoke(
                new Action(() =>
                               {
                                   if (_running) return;
                                   _running = true;
                                   try
                                   {
                                       string key = string.Empty;
                                       while (searchKeys.TryDequeue(out key))
                                       {
                                           if (searchKeys.Count > 0) continue;
                                           if (string.IsNullOrEmpty(key))
                                           {
                                               IsSearchTreeVisi = Visibility.Collapsed;
                                               ChildTreeItemsSearch.Clear();
                                           }
                                           else
                                           {
                                               var tmpList2 = SearchNodepri(key);
                                               ChildTreeItemsSearch.Clear();
                                               int index = 0;

                                               bool needbreak = false;
                                               foreach (var t in tmpList2)
                                               {
                                                   if (searchKeys.Count > 0)
                                                   {
                                                       needbreak = true;
                                                       break;
                                                   }
                                                   index++;
                                                   ChildTreeItemsSearch.Add(t);
                                                   if (index%20 == 1)
                                                   {
                                                       Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();

                                                   }
                                               }
                                               if (needbreak) continue;
                                               IsSearchTreeVisi = Visibility.Visible;

                                           }


                                           var ins = new PublishEventArgs()
                                                         {
                                                             EventType = PublishEventType.Core,
                                                             EventId =
                                                                 Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign
                                                                 .RtuGroupSelectdWantedMapUp
                                                         };

                                           if (string.IsNullOrEmpty(key)) EventPublish.PublishEvent(ins);
                                           else
                                           {
                                               var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
                                               ins.AddParams(info);
                                               if (info.Count > 0)
                                               {
                                                   EventPublish.PublishEvent(ins);
                                               }
                                           }





                                           if (OnClearSerchTest != null)
                                               OnClearSerchTest(this, new NodeSelectedArgs() {SearchText = key});


                                       }
                                   }
                                   catch (Exception ex)
                                   {

                                   }
                               }), DispatcherPriority.DataBind);
            _running = false;
        }




        //private long searchtime = 0;
        private List<TreeNodeBaseNode> SearchNodepri(string keyWord)
        {
             
            
            if (keyWord == "")
            {
               // IsSearchTreeVisi = Visibility.Collapsed;
                return new List<TreeNodeBaseNode>( );
            }

            //var kesss =
            //    (from t in GrpComSingleMuliViewModel.BaseNodes.Nodess.Keys orderby t ascending select t).ToList();

            var lst = new List<TreeNodeBaseNode>();
            foreach (var f in ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    lst.AddRange(f.ChildTreeItems);
                }
                else if (f.NodeType == TypeOfTabTreeNode.IsArea)
                {
                    lst.AddRange(Getallnode(f));
                }

            }
            #region edit

            List<TreeNodeBaseNode> tmpList = new List<TreeNodeBaseNode>();
            if (keyWord.Length > 0)
            {
                if (keyWord.Contains(","))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split(',');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "")
                        {
                            MultiKeyLst.Add(new List<TreeNodeBaseNode>());
                            MultiKeyLst[i + 1].AddRange(MultiKeyLst[i]);
                            continue;
                        }
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
                                }

                                nodeId.Mark = "mark";
                            }



                            if (nodeId.RtuInstallAddr.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("设备安装位置")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：亮灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：亮灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：掉线")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：掉线";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：灭灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：灭灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "故障" &&
                                (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：故障")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：故障";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：不用")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：不用";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：停运")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：停运";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }
                else if (keyWord.Contains("，"))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split('，');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "") continue;
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
                                }

                                nodeId.Mark = "mark";
                            }



                            if (nodeId.RtuInstallAddr.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("设备安装位置")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：亮灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：亮灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：掉线")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：掉线";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：灭灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：灭灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "故障" &&
                                (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：故障")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：故障";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：不用")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：不用";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：停运")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：停运";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }

                else
                {
                    #region foreach

                    foreach (var nodeId in lst)
                    {
                        nodeId.ExtendSerachConten = null;
                        if (nodeId.PhyId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten = "物理地址-" + nodeId.PhyId;

                        }
                        if (nodeId.NodeId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;


                        }
                        if (nodeId.PhoneNumber.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;


                        }

                        if (StringContainKeyword(nodeId.NodeName, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端名称";

                        }


                        if (StringContainKeyword(nodeId.IpAddr, keyWord))
                        {
                            nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();

                        }



                        if (nodeId.RtuInstallAddr.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;

                        }

                        if (StringContainKeyword(nodeId.RtuOnly, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;

                        }

                        if (keyWord == "亮灯" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：亮灯";

                        }
                        if (keyWord == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：掉线";

                        }
                        if (keyWord == "灭灯" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：灭灯";

                        }
                        if (keyWord == "故障" &&
                            (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
                             nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：故障";

                        }

                        if (keyWord == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：不用";

                        }
                        if (keyWord == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：停运";
                        }
                        if (nodeId.ExtendSerachConten != null) tmpList.Add(nodeId);
                        // ChildTreeItemsSearch.Add(nodeId);

                    }

                    #endregion
                }

            }
            #endregion

            var tmpList2 = (from t in tmpList orderby t.NodeId ascending select t).ToList();
            return tmpList2;

            int index = 0;
            foreach (var t in tmpList2)
            {
 
                index++;
                ChildTreeItemsSearch.Add(t);
                if (index % 20 == 1)
                {
                    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
 
                }
            }


            IsSearchTreeVisi = Visibility.Visible;

            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };


            var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
            ins.AddParams(info);
            if (info.Count > 0)
            {
                EventPublish.PublishEvent(ins);
            }

        }

        private List<TreeNodeBaseNode> tmpList2 = new List<TreeNodeBaseNode>();
        //public void SearchNodeold(string keyWord)
        //{
        //    tmpList2.Clear();
        //    ChildTreeItemsSearch.Clear();
        //    if (keyWord == "")
        //    {
        //        IsSearchTreeVisi = Visibility.Collapsed;
        //        ChildTreeItemsSearch.Clear();
        //        return;
        //    }

        //    //var kesss =
        //    //    (from t in GrpComSingleMuliViewModel.BaseNodes.Nodess.Keys orderby t ascending select t).ToList();

        //    var lst = new List<TreeNodeBaseNode>();
        //    foreach (var f in ChildTreeItems)
        //    {
        //        if (f.NodeType == TypeOfTabTreeNode.IsAll)
        //        {
        //            lst.AddRange(f.ChildTreeItems);
        //        }
        //        else if (f.NodeType == TypeOfTabTreeNode.IsArea)
        //        {
        //            lst.AddRange(Getallnode(f));
        //        }

        //    }
        //    #region edit

        //    List<TreeNodeBaseNode> tmpList = new List<TreeNodeBaseNode>();
        //    if (keyWord.Length > 0)
        //    {
        //        if (keyWord.Contains(","))
        //        {
        //            List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
        //            MultiKeyLst[0].AddRange(lst);
        //            string[] keyLst = keyWord.Split(',');

        //            for (int i = 0; i < keyLst.Length; i++)
        //            {
        //                if (keyLst[i] == "")
        //                {
        //                    MultiKeyLst.Add(new List<TreeNodeBaseNode>());
        //                    MultiKeyLst[i + 1].AddRange(MultiKeyLst[i]);
        //                    continue;
        //                }
        //                MultiKeyLst.Add(new List<TreeNodeBaseNode>());

        //                #region foreach

        //                foreach (var nodeId in MultiKeyLst[i])
        //                {
        //                    //nodeId.ExtendSerachConten = null;
        //                    nodeId.Mark = null;
        //                    if (nodeId.PhyId.ToString().Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
        //                        }

        //                        nodeId.Mark = "mark";

        //                    }

        //                    if (nodeId.NodeId.ToString().Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
        //                        }

        //                        nodeId.Mark = "mark";

        //                    }

        //                    if (nodeId.PhoneNumber.Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
        //                        }

        //                        nodeId.Mark = "mark";

        //                    }

        //                    if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端名称";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }


        //                    if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }



        //                    if (nodeId.RtuInstallAddr.Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("设备安装位置")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }


        //                    if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }

        //                    if (keyLst[i] == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) ||
        //                         nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：亮灯")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：亮灯";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：掉线")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：掉线";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：灭灯")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：灭灯";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "故障" &&
        //                        (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
        //                         nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：故障")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：故障";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }

        //                    if (keyLst[i] == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：不用")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：不用";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：停运")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：停运";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

        //                    // ChildTreeItemsSearch.Add(nodeId);

        //                }


        //                #endregion
        //            }
        //            tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
        //        }
        //        else if (keyWord.Contains("，"))
        //        {
        //            List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
        //            MultiKeyLst[0].AddRange(lst);
        //            string[] keyLst = keyWord.Split('，');

        //            for (int i = 0; i < keyLst.Length; i++)
        //            {
        //                if (keyLst[i] == "") continue;
        //                MultiKeyLst.Add(new List<TreeNodeBaseNode>());

        //                #region foreach

        //                foreach (var nodeId in MultiKeyLst[i])
        //                {
        //                    //nodeId.ExtendSerachConten = null;
        //                    nodeId.Mark = null;
        //                    if (nodeId.PhyId.ToString().Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
        //                        }

        //                        nodeId.Mark = "mark";

        //                    }

        //                    if (nodeId.NodeId.ToString().Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
        //                        }

        //                        nodeId.Mark = "mark";

        //                    }

        //                    if (nodeId.PhoneNumber.Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
        //                        }

        //                        nodeId.Mark = "mark";

        //                    }

        //                    if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端名称";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }


        //                    if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }



        //                    if (nodeId.RtuInstallAddr.Contains(keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("设备安装位置")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }


        //                    if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }

        //                    if (keyLst[i] == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) ||
        //                         nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：亮灯")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：亮灯";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：掉线")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：掉线";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：灭灯")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：灭灯";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "故障" &&
        //                        (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
        //                         nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：故障")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：故障";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }

        //                    if (keyLst[i] == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：不用")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：不用";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (keyLst[i] == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
        //                    {
        //                        if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：停运")) || nodeId.ExtendSerachConten == null)
        //                        {
        //                            nodeId.ExtendSerachConten += " 终端状态：停运";
        //                        }

        //                        nodeId.Mark = "mark";
        //                    }
        //                    if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

        //                    // ChildTreeItemsSearch.Add(nodeId);

        //                }


        //                #endregion
        //            }
        //            tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
        //        }

        //        else
        //        {
        //            #region foreach

        //            foreach (var nodeId in lst)
        //            {
        //                nodeId.ExtendSerachConten = null;
        //                if (nodeId.PhyId.ToString().Contains(keyWord))
        //                {
        //                    nodeId.ExtendSerachConten = "物理地址-" + nodeId.PhyId;

        //                }
        //                if (nodeId.NodeId.ToString().Contains(keyWord))
        //                {
        //                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;


        //                }
        //                if (nodeId.PhoneNumber.Contains(keyWord))
        //                {
        //                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;


        //                }

        //                if (StringContainKeyword(nodeId.NodeName, keyWord))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端名称";

        //                }


        //                if (StringContainKeyword(nodeId.IpAddr, keyWord))
        //                {
        //                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();

        //                }



        //                if (nodeId.RtuInstallAddr.Contains(keyWord))
        //                {
        //                    nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;

        //                }

        //                if (StringContainKeyword(nodeId.RtuOnly, keyWord))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;

        //                }

        //                if (keyWord == "亮灯" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端状态：亮灯";

        //                }
        //                if (keyWord == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端状态：掉线";

        //                }
        //                if (keyWord == "灭灯" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端状态：灭灯";

        //                }
        //                if (keyWord == "故障" &&
        //                    (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
        //                     nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端状态：故障";

        //                }

        //                if (keyWord == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端状态：不用";

        //                }
        //                if (keyWord == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
        //                {
        //                    nodeId.ExtendSerachConten += " 终端状态：停运";
        //                }
        //                if (nodeId.ExtendSerachConten != null) tmpList.Add(nodeId);
        //                // ChildTreeItemsSearch.Add(nodeId);

        //            }

        //            #endregion
        //        }

        //    }
        //    #endregion

        //    tmpList2 = (from t in tmpList orderby t.NodeId ascending select t).ToList();
        //    int index = 0;
        //    foreach (var t in tmpList2)
        //    {

        //        index++;
        //        if (SearchLimit != 0 && SearchLimit != 1 && index > SearchLimit) break;//todo lvf test
        //        ChildTreeItemsSearch.Add(t);
        //        //if (index % 20 == 1)
        //        //{
        //        //    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
        //        //}
        //    }


        //    IsSearchTreeVisi = Visibility.Visible;

        //    var ins = new PublishEventArgs()
        //    {
        //        EventType = PublishEventType.Core,
        //        EventId =
        //            Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
        //    };


        //    var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
        //    ins.AddParams(info);
        //    if (info.Count > 0)
        //    {
        //        EventPublish.PublishEvent(ins);
        //    }

        //}

        public void SearchNodeold(string keyWord)
        {
            //if (checkedrtus.Count > 0) return;
            tmpList2.Clear();
            ChildTreeItemsSearch.Clear();
            //ChildTreeItemsMap.Clear();
            //IsSearchMapTreeVisi = Visibility.Collapsed;
            if (keyWord == "")
            {
                IsSearchTreeVisi = Visibility.Collapsed;
                ChildTreeItemsSearch.Clear();
                return;
            }

            //var kesss =
            //    (from t in GrpComSingleMuliViewModel.BaseNodes.Nodess.Keys orderby t ascending select t).ToList();

            var lst = new List<TreeNodeBaseNode>();
            foreach (var f in ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    lst.AddRange(f.ChildTreeItems);
                }
                else if (f.NodeType == TypeOfTabTreeNode.IsArea)
                {
                    lst.AddRange(Getallnode(f));
                }

            }
            #region edit

            List<TreeNodeBaseNode> tmpList = new List<TreeNodeBaseNode>();
            if (keyWord.Length > 0)
            {
                if (keyWord.Contains(","))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split(',');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "")
                        {
                            MultiKeyLst.Add(new List<TreeNodeBaseNode>());
                            MultiKeyLst[i + 1].AddRange(MultiKeyLst[i]);
                            continue;
                        }
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
                                }

                                nodeId.Mark = "mark";
                            }



                            if (nodeId.RtuInstallAddr.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("设备安装位置")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：亮灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：亮灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：掉线")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：掉线";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：灭灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：灭灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "故障" &&
                                (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：故障")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：故障";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：不用")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：不用";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：停运")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：停运";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }
                else if (keyWord.Contains("，"))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split('，');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "") continue;
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
                                }

                                nodeId.Mark = "mark";
                            }



                            if (nodeId.RtuInstallAddr.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("设备安装位置")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：亮灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：亮灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：掉线")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：掉线";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：灭灯")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：灭灯";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "故障" &&
                                (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
                                 nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：故障")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：故障";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (keyLst[i] == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：不用")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：不用";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (keyLst[i] == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端状态：停运")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端状态：停运";
                                }

                                nodeId.Mark = "mark";
                            }
                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }

                else
                {
                    #region foreach

                    foreach (var nodeId in lst)
                    {
                        nodeId.ExtendSerachConten = null;
                        if (nodeId.PhyId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten = "物理地址-" + nodeId.PhyId;

                        }
                        if (nodeId.NodeId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;


                        }
                        if (nodeId.PhoneNumber.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;


                        }

                        if (StringContainKeyword(nodeId.NodeName, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端名称";

                        }


                        if (StringContainKeyword(nodeId.IpAddr, keyWord))
                        {
                            nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();

                        }



                        if (nodeId.RtuInstallAddr.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 设备安装位置-" + nodeId.RtuInstallAddr;

                        }

                        if (StringContainKeyword(nodeId.RtuOnly, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;

                        }
                        if (keyWord == "亮灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：亮灯";

                        }
                        if (keyWord == "亮灯@-@" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3007) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：亮灯";

                        }
                        if (keyWord == "掉线" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3003))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：掉线";

                        }
                        if (keyWord == "灭灯" && (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3005) || nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006)))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：灭灯";

                        }
                        if (keyWord == "故障" &&
                            (nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3006) ||
                             nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3008)))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：故障";

                        }

                        if (keyWord == "停电")
                        {
                            //var ssss=  Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(
                            //    nodeId.NodeId);
                            //foreach (var g in ssss)
                            //{
                            //    if ( g.FaultId ==2)
                            //    {
                            //        nodeId.ExtendSerachConten += " 终端状态：停电";
                            //        break;
                            //    }
                            //}
                            var sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                                       where gg.RtuId == nodeId.NodeId && gg.FaultId < 6
                                       select gg.FaultId).ToList();
                            if (sss.Count == 0) continue;
                            string tmp = "";
                            if (sss[0] == 1) tmp = "供电停电";
                            if (sss[0] == 2) tmp = "终端断电";
                            if (sss[0] == 3) tmp = "A相断电";
                            if (sss[0] == 4) tmp = "B相断电";
                            if (sss[0] == 5) tmp = "C相断电";
                            nodeId.ExtendSerachConten += " 终端状态：" + tmp;

                        }

                        if (keyWord == "不用" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3001))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：不用";

                        }
                        if (keyWord == "停运" && nodeId.ImagesIcon == ImageResources.GetEquipmentIcon(3002))
                        {
                            nodeId.ExtendSerachConten += " 终端状态：停运";
                        }
                        if (nodeId.ExtendSerachConten != null) tmpList.Add(nodeId);
                        // ChildTreeItemsSearch.Add(nodeId);

                    }

                    #endregion
                }

            }
            #endregion

            tmpList2 = (from t in tmpList orderby t.PhyId ascending select t).ToList();
            int index = 0;
            foreach (var t in tmpList2)
            {

                index++;
                if (SearchLimit != 0 && SearchLimit != 1 && index > SearchLimit) break;//todo lvf test
                ChildTreeItemsSearch.Add(t);
                //if (index % 20 == 1)
                //{
                //    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
                //}
            }


            IsSearchTreeVisi = Visibility.Visible;

            if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };


            var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
            ins.AddParams(info);
            if (info.Count > 0)
            {
                EventPublish.PublishEvent(ins);
            }

        }

        private Visibility _isSearchTreeVisi;

        public Visibility IsSearchTreeVisi
        {
            get { return _isSearchTreeVisi; }
            set
            {
                if (value == _isSearchTreeVisi) return;
                _isSearchTreeVisi = value;
                this.RaisePropertyChanged(() => this.IsSearchTreeVisi);
            }
        }


        //private Visibility _isShowRapidOper;

        //public Visibility IsShowRapidOper
        //{
        //    get { return _isShowRapidOper; }
        //    set
        //    {
        //        if (value == _isShowRapidOper) return;
        //        _isShowRapidOper = value;
        //        this.RaisePropertyChanged(() => this.IsShowRapidOper);
        //    }
        //}





        /// <summary>
        /// 前者是否包含后者数据 
        /// </summary>
        /// <param name="containerStinng"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool StringContainKeyword(string containerStinng, string keyword)
        {
            if (containerStinng == null) return false;
            if (containerStinng.Contains(keyword)) return true;
            string conv = chinesecap(containerStinng);
            if (conv.Contains(keyword)) return true;
            if (containerStinng.ToUpper().Contains(keyword.ToUpper())) return true;
            return false;
        }


        /// <summary>
        /// 返回汉字字符串的拼音的首字母
        /// </summary>
        /// <param name="chinesestr">要转换的字符串</param>
        /// <returns></returns>
        public string chinesecap(string chinesestr)
        {
            byte[] zw = new byte[2];
            string charstr = "";
            string capstr = "";
            for (int i = 0; i <= chinesestr.Length - 1; i++)
            {
                charstr = chinesestr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
                zw = System.Text.Encoding.Default.GetBytes(charstr);
                // 得到汉字符的字节数组
                if (zw.Length == 2)
                {
                    int i1 = (short)(zw[0]);
                    int i2 = (short)(zw[1]);
                    long chinesestrInt = i1 * 256 + i2;
                    //table of the constant list
                    // a; //45217..45252
                    // z; //54481..55289
                    capstr += GetChinesefirst(chinesestrInt);
                }
                else
                {
                    capstr += charstr;
                }

                //capstr = capstr + chinastr;
            }

            return capstr;
        }

        private string GetChinesefirst(long chinesestrInt)
        {
            string chinastr = "";
            //table of the constant list
            // a; //45217..45252
            // b; //45253..45760
            // c; //45761..46317
            // d; //46318..46825
            // e; //46826..47009
            // f; //47010..47296
            // g; //47297..47613

            // h; //47614..48118
            // j; //48119..49061
            // k; //49062..49323
            // l; //49324..49895
            // m; //49896..50370
            // n; //50371..50613
            // o; //50614..50621
            // p; //50622..50905
            // q; //50906..51386

            // r; //51387..51445
            // s; //51446..52217
            // t; //52218..52697
            //没有u,v
            // w; //52698..52979
            // x; //52980..53640
            // y; //53689..54480
            // z; //54481..55289

            if ((chinesestrInt >= 45217) && (chinesestrInt <= 45252))
            {
                chinastr = "a";
            }
            else if ((chinesestrInt >= 45253) && (chinesestrInt <= 45760))
            {
                chinastr = "b";
            }
            else if ((chinesestrInt >= 45761) && (chinesestrInt <= 46317))
            {
                chinastr = "c";
            }
            else if ((chinesestrInt >= 46318) && (chinesestrInt <= 46825))
            {
                chinastr = "d";
            }
            else if ((chinesestrInt >= 46826) && (chinesestrInt <= 47009))
            {
                chinastr = "e";
            }
            else if ((chinesestrInt >= 47010) && (chinesestrInt <= 47296))
            {
                chinastr = "f";
            }
            else if ((chinesestrInt >= 47297) && (chinesestrInt <= 47613))
            {
                chinastr = "g";
            }
            else if ((chinesestrInt >= 47614) && (chinesestrInt <= 48118))
            {
                chinastr = "h";
            }

            else if ((chinesestrInt >= 48119) && (chinesestrInt <= 49061))
            {
                chinastr = "j";
            }
            else if ((chinesestrInt >= 49062) && (chinesestrInt <= 49323))
            {
                chinastr = "k";
            }
            else if ((chinesestrInt >= 49324) && (chinesestrInt <= 49895))
            {
                chinastr = "l";
            }
            else if ((chinesestrInt >= 49896) && (chinesestrInt <= 50370))
            {
                chinastr = "m";
            }

            else if ((chinesestrInt >= 50371) && (chinesestrInt <= 50613))
            {
                chinastr = "n";
            }
            else if ((chinesestrInt >= 50614) && (chinesestrInt <= 50621))
            {
                chinastr = "o";
            }
            else if ((chinesestrInt >= 50622) && (chinesestrInt <= 50905))
            {
                chinastr = "p";
            }
            else if ((chinesestrInt >= 50906) && (chinesestrInt <= 51386))
            {
                chinastr = "q";
            }

            else if ((chinesestrInt >= 51387) && (chinesestrInt <= 51445))
            {
                chinastr = "r";
            }
            else if ((chinesestrInt >= 51446) && (chinesestrInt <= 52217))
            {
                chinastr = "s";
            }
            else if ((chinesestrInt >= 52218) && (chinesestrInt <= 52697))
            {
                chinastr = "t";
            }
            else if ((chinesestrInt >= 52698) && (chinesestrInt <= 52979))
            {
                chinastr = "w";
            }
            else if ((chinesestrInt >= 52980) && (chinesestrInt <= 53640))
            {
                chinastr = "x";
            }
            else if ((chinesestrInt >= 53689) && (chinesestrInt <= 54480))
            {
                chinastr = "y";
            }
            else if ((chinesestrInt >= 54481) && (chinesestrInt <= 55289))
            {
                chinastr = "z";
            }
            return chinastr;
        }

        #endregion
    }

    //event
    public partial class TreeSingleViewModel
    {
        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// 目前只处理
        /// 1、系统当前选中的终端或分组变更，提供联动
        /// 2、终端参数发生变化的时候，即使更新显示数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType == PublishEventType.Core)
                {

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RegionNeedUpdate)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuRegionNeedUpdate)
                        return true;
                    if (args.EventId ==
   global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }



        private Dictionary<int, int> RtuImsgIconTmp = new Dictionary<int, int>();
        private void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv)
                {
                    Update();
                    //LoadNodeOther();
                    return;
                }

                if (args.EventType == PublishEventType.Core)
                {

                    if (args.EventId == EventIdAssign.RegionNeedUpdate)
                    {
                        LoadNode();
                        //LoadNodeOther();

                        Update();
                    }
                    if (args.EventId == EventIdAssign.RtuRegionNeedUpdate)
                    {

                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null) return;
                        foreach (var g in lst)
                        {
                            //变图标
                            if (args.EventAttachInfo == "ChangeImage")
                            {
                                return;



                                if (TreeNodeItemTmlViewModel.RtuItems.ContainsKey(g))
                                {
                                    foreach (var f in TreeNodeItemTmlViewModel.RtuItems[g])
                                    {
                                        if (f.Target != null)
                                        {
                                            var xg = f.Target as TreeNodeBaseNode;
                                            if (xg != null)
                                            {

                                                //停运3002，  不用3001，  关灯正常3005，  关灯故障3006 ， 开灯正常3007  ，开灯故障3008 ， 离线3003  ，默认 正常关灯3005
                                                var imageId = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetImageIdByRtuid(g);

                                                xg.ImagesIcon = ImageResources.GetEquipmentIcon(imageId);


                                                if (g == 1000002)
                                                {

                                                    var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(g);
                                                    var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                                    var iiid = 0;
                                                    // int modelId = (int) TerInfo.EquipmentType;
                                                    if (TerInfo.EquipmentType == WjParaBase.EquType.Rtu)
                                                    {
                                                        var s = TerInfo.RtuStateCode;
                                                        if (s == 0)
                                                        {
                                                            iiid = 3001;

                                                        }
                                                        if (s == 1)
                                                        {
                                                            iiid = 3002;

                                                        }

                                                        var online = runninfo != null && runninfo.IsOnLine;
                                                        if (online == false)
                                                        {
                                                            iiid = 3003;

                                                        }
                                                        var haserror = false;
                                                        if (UxTreeSetting.IsRutsNotShowError == false)
                                                            haserror = runninfo.ErrorCount > 0;
                                                        var lighton = runninfo.IsLightHasElectric; // RtuNewDataService.IsRtuHasElectric(this.NodeId);
                                                        int errorindex = 0;
                                                        // var ShieldAList = new Dictionary<int, Tuple<double, double>>();
                                                        if (haserror && lighton) errorindex = 3;
                                                        if (haserror && !lighton) errorindex = 1;
                                                        if (!haserror && lighton)
                                                        {
                                                            //foreach(var t in runninfo.RtuNewData.LstNewLoopsData   )
                                                            //{
                                                            //    ShieldAList.Add(t.LoopId, new Tuple<double, double>(t.A, t.ShieldLittleA));
                                                            //}
                                                            //foreach(var t in ShieldAList )
                                                            //{
                                                            //    int count = 0;
                                                            //    if (t.Value.Item1 < t.Value.Item2 || t.Value.Item1 == 0.0) count++;
                                                            //}

                                                            errorindex = 2;
                                                        }
                                                        if (!haserror && !lighton) errorindex = 0;

                                                        iiid = 3005 + errorindex;



                                                        WriteLog.WriteInfo("1000002初始化状态为：" + imageId + "     _ 应有状态为：" + iiid + "     " + DateTime.Now);
                                                    }








                                                }//xg.ReUpdate(1);
                                            }
                                        }

                                    }

                                }
                            }
                            else if (args.EventAttachInfo == "ChangeStructure")
                            {


                                LoadNode();
                                LoadNodeOther();
                                return;
                                if (TreeNodeItemTmlViewModel.RtuItems.ContainsKey(g))
                                {
                                    
                                    TreeNodeItemTmlViewModel.RtuItems.Remove(g);

                                    var tukey = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetGrpRegionByRtuid(g);


                                    //todo


                                    foreach (var k in this.ChildTreeItems)
                                    {
                                        if (k.NodeType == TypeOfTabTreeNode.IsGrp && k.NodeId == tukey.Item1)
                                        {
                                            foreach (var j in k.ChildTreeItems)
                                            {
                                                if (j.NodeId == g)
                                                {
                                                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                                                    j.AddChildren(j,para);

                                                }
                                            }

                                        }
                                    }
                                    

                                    foreach (var f in TreeNodeItemTmlViewModel.RtuItems[g])
                                    {
                                        if (f.Target != null)
                                        {
                                            var xg = f.Target as TreeNodeBaseNode;
                                            
                                            if (xg != null)
                                            {
                                                
                                                xg.ImagesIcon = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetImageIdByRtuid(g);
                                            }//xg.ReUpdate(1);
                                        }

                                    }

                                }

                            }



                        }
                 
                        Update();
                    }

                    if (args.EventId == EventIdAssign.RunningInfoUpdate1 || args.EventId == EventIdAssign.RunningInfoUpdate2)
                    {

                        var lst = args.GetParams()[0] as IEnumerable<int>;
                        if (lst == null) return;

                        foreach (var t in lst)
                        {
                            var id = GrpComSingleMuliViewModel.TreeNodeItemTmlViewModel.GetImageIconByState(t);
                            if (id == 0) continue;

                            if (RtuImsgIconTmp.ContainsKey(t))
                            {
                                if (RtuImsgIconTmp[t] == id) continue;
                                RtuImsgIconTmp[t] = id;
                            }
                            else
                            {
                                RtuImsgIconTmp.Add(t, id);
                            }

                            if (TreeNodeItemTmlViewModel.RtuItems.ContainsKey(t) == false) continue;
                            Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(Ac, new Tuple<int, int>(t, id));





                        }
                    }


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        try
                        {

                            //点击第二查询框中自身节点，则不处理
                            if (Convert.ToString(args.EventAttachInfo) == "TreeSelf") return;


                            int x = Convert.ToInt32(args.GetParams()[0]);
                            if (x > 0 && Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(x))
                            {
                                var tmp = GrpComSingleMuliViewModel.TreeNodeItemTmlViewModel.CurrentSelectedTreeNode;

                                //if (tmp !=null)  
                                //    OnCurrentSelectedNode(tmp.NodeId);
                                OnCurrentSelectedNode(x);
                                if (tmp == null || tmp.NodeId != x)
                                {
                                    if (OnSelectedNodeByCodeIns != null)
                                    {
                                        OnSelectedNodeByCodeIns(this, new NodeSelectedArgs() { RtuIdSelected = x });
                                    }

                                    //if (BaseNodes.AllTmpNodess.ContainsKey(x))
                                    //{
                                    //    BaseNodes.AllTmpNodess[x].IsSelected = true;
                                    //    BaseNodes.AllTmpNodess[x].is = true;
                                    //}
                                }

                                //////终端树中点击其他节点，则清空第二查询框
                                ////if (Convert.ToString(args.EventAttachInfo) == "Tree")
                                ////{

                                ////    ExCmdClearUpMapPiont();
                                ////    return;
                                ////}



                                //////判断选项里是否勾选联动，如果勾选则直接在终端树中呈现其他界面点击的设备
                                ////if (!Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 6, false)) return;
                                ////if (x < 1000000 || x > 1100000)
                                ////{
                                ////    ExCmdClearUpMapPiont();
                                ////    return;
                                ////}
                                ////SearchNodeMap(x.ToString());


                            }
                            //if (Convert.ToString(args.EventAttachInfo) == "gis")
                            //{
                            //    if (!Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 6, false)) return;
                            //    if (x < 1000000 || x > 1100000)
                            //    {
                            //        ExCmdClearUpMapPiont();
                            //        return;
                            //    }
                            //    SearchNodeMap(x.ToString());
                            //}

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (args.EventId ==
                      global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {

                        LoadNode();
                        return;
                        //Update();
                        if (IsLoadOnlyOneArea)
                        {
                            LoadNodeOther();
                        }
                        else
                        {
                            UpdateAreaGrp();
                        }


                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



        void Ac(object obj)
        {
            var tu = obj as Tuple<int, int>;
            if (tu == null) return;

            foreach (var f in TreeNodeItemTmlViewModel.RtuItems[tu.Item1])
            {
                if (f.Target != null)
                {
                    var xg = f.Target as TreeNodeBaseNode;
                    //if (xg != null) xg.ReUpdate(2);
                    if (xg != null) xg.ReUpdate(tu.Item2);

                    if (xg.NodeId == 1000002)
                    {
                        WriteLog.WriteInfo("1000002交叉分组更新图标：" + tu.Item2 + "  " + DateTime.Now);
                    }
                }

            }
        }


        void UpdateRtuInAreaNewAddToTop(TreeNodeBaseNode node)
        {
            if (node.NodeType == TypeOfTabTreeNode.IsAll)
            {
                node.AddChild();
            }
            else
            {
                foreach (var f in node.ChildTreeItems) UpdateRtuInAreaNewAddToTop(f);
            }
        }


        #endregion


        private bool isvier;

        public bool IsVir
        {
            get { return isvier; }
            set
            {
                if (isvier != value)
                {
                    isvier = value;
                    this.RaisePropertyChanged(() => this.IsVir);
                }
            }
        }


       



    }


    /// <summary>
    /// open close light
    /// </summary>
    public partial class TreeSingleViewModel
    {

        private int _curnetselectrtu = 0;
        private int _curstate = 0;
        private void OnCurrentSelectedNode(int nodeid)
        {
            var equip = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeid);
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[equip.RtuId]
                as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

            //如果没有操作权限则屏蔽界面
            var areaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaThatRtuIn(equip.RtuId);
            var userProperty = UserInfo.UserLoginInfo;
            if (userProperty.AreaX.Contains(areaId) == false)
            {
                this.IsNotShowFastControl = 0;
            }
            else
            {
                this.IsNotShowFastControl = UxTreeSetting.IsRutsNotShowNullK != 0 ? 90 : 0;
            }

            if (UxTreeSetting.IsRutsNotShowNullK == 1)
            {
                foreach (var v in KxInfo)
                {
                    v.IsEnable = false;
                    v.IsSelected = false;
                }
            }
            else
            {
                foreach (var v in KxInfo)
                {
                    v.IsEnable = true;

                }
            }

            //KxInfo[0].IsEnable = false ;
            //KxInfo[1].IsEnable = false;
            //KxInfo[2].IsEnable = false;
            //KxInfo[3].IsEnable = false;
            //KxInfo[4].IsEnable = false;
            //KxInfo[5].IsEnable = false;
            //KxInfo[6].IsEnable = false;
            //KxInfo[7].IsEnable = false;
            EnableK1 = false;
            EnableK2 = false;
            EnableK3 = false;
            EnableK4 = false;
            EnableK5 = false;
            EnableK6 = false;
            EnableK7 = false;
            EnableK8 = false;

            if (equip != null)
            {
                if (_curnetselectrtu == nodeid) return;
                _curnetselectrtu = nodeid;
                _curstate = equip.RtuStateCode;
                CurRtuInof = "快速操作: " + equip.RtuPhyId.ToString("d3") + " - " + equip.RtuName;
                if (equip.RtuModel == EnumRtuModel.Wj3006) { Visi = Visibility.Visible; }
                else
                {
                    Visi = Visibility.Collapsed;
                }
                if (UxTreeSetting.IsRutsNotShowNullK != 1) return;
                foreach (var f in t.WjLoops.Values)
                {
                    if (f.SwitchOutputId == 1) EnableK1 = true;
                    if (f.SwitchOutputId == 2) EnableK2 = true;
                    if (f.SwitchOutputId == 3) EnableK3 = true;
                    if (f.SwitchOutputId == 4) EnableK4 = true;
                    if (f.SwitchOutputId == 5) EnableK5 = true;
                    if (f.SwitchOutputId == 6) EnableK6 = true;
                    if (f.SwitchOutputId == 7) EnableK7 = true;
                    if (f.SwitchOutputId == 8) EnableK8 = true;
                }

                if (EnableK1) KxInfo[0].IsEnable = true;
                if (EnableK2) KxInfo[1].IsEnable = true;
                if (EnableK3) KxInfo[2].IsEnable = true;
                if (EnableK4) KxInfo[3].IsEnable = true;
                if (EnableK5) KxInfo[4].IsEnable = true;
                if (EnableK6) KxInfo[5].IsEnable = true;
                if (EnableK7) KxInfo[6].IsEnable = true;
                if (EnableK8) KxInfo[7].IsEnable = true;
            }
        }

        #region CmdOc

        private ICommand _cmdCmdOcrchText;

        public ICommand CmdOpenCloselight
        {
            get
            {
                if (_cmdCmdOcrchText == null)
                    _cmdCmdOcrchText = new RelayCommand<string>(ExCmdOpenCloselight, CanCmdOpenCloselight, true);
                return _cmdCmdOcrchText;
            }
        }

        private void ExCmdOpenCloselight(string s)
        {
            try
            {
                int x = 0;
                if (Int32.TryParse(s, out x))
                {
                    //if(x==1)//开灯
                    //{

                    //}
                    //else if (x == 2)//关灯
                    //{

                    //}


                    var loops = (from t in KxInfo where t.IsSelected && t.IsEnable orderby t.Value ascending select t.Value).ToList();
                    if (loops.Count == 0) return;

                    string tr = x == 1 ? "开灯" : "关灯";
                    if (x == 1)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 1)
                        {
                            if (
                                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                    "您将要进行" + tr + "操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                            {
                                return;
                            }
                        }
                        else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                        {
                            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                            if (sss == UMessageBoxWantPassWord.CancelReturn)
                            {
                                return;
                            }
                            if (sss != UserInfo.UserLoginInfo.UserPassword)
                            {
                                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                 UMessageBoxButton.Yes);
                                return;
                            }
                        }
                    }else
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 1)
                        {
                            if (
                                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                    "您将要进行" + tr + "操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                            {
                                return;
                            }
                        }
                        else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 2)
                        {
                            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                            if (sss == UMessageBoxWantPassWord.CancelReturn)
                            {
                                return;
                            }
                            if (sss != UserInfo.UserLoginInfo.UserPassword)
                            {
                                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                 UMessageBoxButton.Yes);
                                return;
                            }
                        }
                    }
                    OpenClsoelIGT(_curnetselectrtu, loops, x == 1);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OpenClsoelIGT(int rtuId, List<int> loops, bool isOpen)
        {

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_light;//.wlst_cnt_wj3090_order_open_close_light;
            //.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLight;
            info.WstRtuCntOrderOpenCloseLight.IsOpen = isOpen ? 1 : 2; //# 开关灯指令 0 关 1 开
            info.Args.Addr.Add(rtuId);
            info.WstRtuCntOrderOpenCloseLight.Loops.AddRange(loops);
            info.WstRtuCntOrderOpenCloseLight.RtuId = rtuId;
            SndOrderServer.OrderSnd(info, 0, 0, true);
        }


        private bool CanCmdOpenCloselight(string s)
        {
            if (_curnetselectrtu < 1000) return false;
            if ((from t in KxInfo where t.IsSelected && t.IsEnable select t).Count() == 0) return false;
            return _curstate == 2;
        }


        private ICommand _cmdCmdClear;

        public ICommand CmdClear
        {
            get
            {
                if (_cmdCmdClear == null)
                    _cmdCmdClear = new RelayCommand(ExCmdClear, CanCmdClear, false);
                return _cmdCmdClear;
            }
        }

        private void ExCmdClear()
        {
            KxInfo[0].IsSelected = false;
            KxInfo[1].IsSelected = false;
            KxInfo[2].IsSelected = false;
            KxInfo[3].IsSelected = false;
            KxInfo[4].IsSelected = false;
            KxInfo[5].IsSelected = false;
            KxInfo[6].IsSelected = false;
            KxInfo[7].IsSelected = false;
        }


        private bool CanCmdClear()
        {
            return true;

        }





        #endregion



        #region Refresh
        private ICommand _cmdRefresh;

        public ICommand CmdRefresh
        {
            get
            {
                if (_cmdRefresh == null)
                    _cmdRefresh = new RelayCommand(ExCmdRefresh, CanCmdRefresh, false);
                return _cmdRefresh;
            }
        }

        private void ExCmdRefresh()
        {
            LoadNode();
        }


        private bool CanCmdRefresh()
        {
            return true;

        }


        #endregion


        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _searchchsdfsdfsInfo;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> KxInfo
        {
            get
            {
                if (_searchchsdfsdfsInfo == null)
                {
                    _searchchsdfsdfsInfo = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool>();
                    if (UxTreeSetting.IsRutsNotShowNullK != 1)
                    {
                        for (int i = 1; i < 17; i++)
                            _searchchsdfsdfsInfo.Add(new NameIntBool() { Name = "K" + i, Value = i, IsSelected = false, IsEnable = true });
                    }
                    else
                    {
                        for (int i = 1; i < 17; i++)
                            _searchchsdfsdfsInfo.Add(new NameIntBool() { Name = "K" + i, Value = i, IsSelected = false, IsEnable = false });
                    }


                }
                return _searchchsdfsdfsInfo;
            }
        }

        private Visibility _visi;

        public Visibility Visi
        {
            get { return _visi; }
            set
            {
                if (_visi != value)
                {
                    _visi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }


        #region k1~8 是否可点击

        private bool _enablek1;
        public bool EnableK1
        {
            get { return _enablek1; }
            set
            {
                if (_enablek1 != value)
                {
                    _enablek1 = value;
                    this.RaisePropertyChanged(() => this.EnableK1);
                }
            }
        }

        private bool _enablek2;
        public bool EnableK2
        {
            get { return _enablek2; }
            set
            {
                if (_enablek2 != value)
                {
                    _enablek2 = value;
                    this.RaisePropertyChanged(() => this.EnableK2);
                }
            }
        }

        private bool _enablek3;
        public bool EnableK3
        {
            get { return _enablek3; }
            set
            {
                if (_enablek3 != value)
                {
                    _enablek3 = value;
                    this.RaisePropertyChanged(() => this.EnableK3);
                }
            }
        }

        private bool _enablek4;
        public bool EnableK4
        {
            get { return _enablek4; }
            set
            {
                if (_enablek4 != value)
                {
                    _enablek4 = value;
                    this.RaisePropertyChanged(() => this.EnableK4);
                }
            }
        }

        private bool _enablek5;
        public bool EnableK5
        {
            get { return _enablek5; }
            set
            {
                if (_enablek5 != value)
                {
                    _enablek5 = value;
                    this.RaisePropertyChanged(() => this.EnableK5);
                }
            }
        }

        private bool _enablek6;
        public bool EnableK6
        {
            get { return _enablek6; }
            set
            {
                if (_enablek6 != value)
                {
                    _enablek6 = value;
                    this.RaisePropertyChanged(() => this.EnableK6);
                }
            }
        }

        private bool _enablek7;
        public bool EnableK7
        {
            get { return _enablek7; }
            set
            {
                if (_enablek7 != value)
                {
                    _enablek7 = value;
                    this.RaisePropertyChanged(() => this.EnableK7);
                }
            }
        }

        private bool _enablek8;
        public bool EnableK8
        {
            get { return _enablek8; }
            set
            {
                if (_enablek8 != value)
                {
                    _enablek8 = value;
                    this.RaisePropertyChanged(() => this.EnableK8);
                }
            }
        }
        #endregion

        private Visibility _userX;

        public Visibility IsUserX
        {
            get { return _userX; }
            set
            {
                if (_userX != value)
                {
                    _userX = value;
                    this.RaisePropertyChanged(() => this.IsUserX);
                }
            }
        }

        private string _searchTextsdfd;

        public string CurRtuInof
        {
            get { return _searchTextsdfd; }
            set
            {
                if (_searchTextsdfd != value)
                {
                    _searchTextsdfd = value;
                    this.RaisePropertyChanged(() => this.CurRtuInof);
                }
            }
        }

    }
}