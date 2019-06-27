using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elysium.ThemesSet.FontSet;
using Wlst.client;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.CoreMims.NodeServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj2090Module.Services;
 
using EventIdAssign = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign;

namespace Wlst.Ux.Wj2090Module.TreeTab.View.ViewModels
{
    /// <summary>
    /// 树的加载    
    /// </summary>
    public partial class TreeTabViewModel
    {
    

        /// <summary>
        /// 获取区域信息，返回  用户能查看的区域列表 
        /// </summary>
        /// <returns></returns>
        private List<int> LoadNode1GetArea()
        {
            var rtn = new List<int>();
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return new List<int>();


            var userProperty = UserInfo.UserLoginInfo;
            if (userProperty.D == true)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                    return new List<int>();
                return Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList();
            }
            else
            {
                List<int> areaLst = new List<int>();
                foreach (var t in userProperty.AreaX)
                {
                    if (t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }

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

                return areaLst;
            }

            return rtn;
        }

        /// <summary>
        /// 通过区域地址 获取区域的inputinfo
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        private InputInfo LoadNode2GetAreaInput(int areaId)
        {
            //区域信息
            var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef;
            foreach (var f in areaInfo.AreaInfo)
            {
                if (f.Value.AreaId == areaId)
                {
                    var inputinfo = new InputInfo(null, null, areaId, 1, areaId);

                    inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                    inputinfo.NodeName1B = f.Value.AreaId.ToString("d2") + "-" + f.Value.AreaName;
                    return inputinfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 通过区域信息 ，获取区域下的分组信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="rootinfo">如果系统只显示一个区域 那么填null</param>
        /// <returns></returns>
        private List<InputInfo> LoadNode2GetAreaGrpInput(int areaId, InputInfo rootinfo,TreeViewBaseNode rooTreeViewBaseNode=null )
        {
            var lstInput = new List<InputInfo>();

            //如果只有一个区域 那么不显示区域分组了 

            int idx = 1;

            var grp =
                (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                    where t.Key.Item1 == areaId
                    orderby t.Value.Index
                    select t.Value).ToList();

            {
                var inputinfo = new InputInfo(rootinfo, rooTreeViewBaseNode, 0, 2, areaId, 0);

                inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                inputinfo.NodeName1B = "全部集中器";
                lstInput.Add(inputinfo);
            }

            foreach (var f in grp)
            {
                var inputinfo = new InputInfo(rootinfo, rooTreeViewBaseNode, f.GroupId, 2, areaId, f.GroupId);
                inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                inputinfo.NodeName1B = f.GroupId.ToString("d2") + "-" + f.GroupName;
                lstInput.Add(inputinfo);
            }

            {
                var inputinfo = new InputInfo(rootinfo, rooTreeViewBaseNode, 99999, 2, areaId, -1);
                inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                inputinfo.NodeName1B = "特殊集中器";
                lstInput.Add(inputinfo);
            }
            return lstInput;
        }


        ///// <summary>
        ///// 输入区域信息  ，返回区域以及 区域下的
        ///// </summary>
        ///// <param name="areas"></param>
        ///// <returns></returns>
        ////加载区域 - 分组节点信息
        //private List<InputInfo> LoadNode2GetInfo(List<int> areas)
        //{
        //    var lstInput = new List<InputInfo>();
        //    foreach (var areaId in areas)
        //    {
        //        long areaKey = 0;
        //        InputInfo rootinfo = null;
        //        //区域信息
        //        var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef;
        //        foreach (var f in areaInfo.AreaInfo)
        //        {
        //            if (f.Value.AreaId == areaId)
        //            {
        //                var inputinfo = new InputInfo(null, null, areaId, 1, areaId);

        //                inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
        //                inputinfo.NodeName1B = f.Value.AreaId.ToString("d2") + "-" + f.Value.AreaName;
        //                lstInput.Add(inputinfo);

        //                rootinfo = inputinfo;
        //            }
        //        }

        //        if (rootinfo == null) continue;
        //        if (areas.Count == 1) rootinfo = null; //如果只有一个区域 那么不显示区域分组了 

        //        int idx = 1;

        //        var grp =
        //            (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
        //                where t.Key.Item1 == areaId
        //                orderby t.Value.Index
        //                select t.Value).ToList();

        //        {
        //            var inputinfo = new InputInfo(rootinfo, null, 0, 2, areaId, 0);

        //            inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
        //            inputinfo.NodeName1B = "全部集中器";
        //            lstInput.Add(inputinfo);
        //        }

        //        foreach (var f in grp)
        //        {
        //            var inputinfo = new InputInfo(rootinfo, null, f.GroupId, 2, areaId, f.GroupId);
        //            inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
        //            inputinfo.NodeName1B = f.GroupId.ToString("d2") + "-" + f.GroupName;
        //            lstInput.Add(inputinfo);
        //        }

        //        {
        //            var inputinfo = new InputInfo(rootinfo, null, 99999, 2, areaId, -1);
        //            inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
        //            inputinfo.NodeName1B = "特殊集中器";
        //            lstInput.Add(inputinfo);
        //        }
        //    }

        //    return lstInput;
        //}

        /// <summary>
        /// 通过分组获取 组下集中器节点
        /// </summary>
        /// <param name="grpInfo"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode3GetSluInfoByGrp(List<InputInfo> grpInfo)
        {
            int idx = 1;
            var lstInput = new List<InputInfo>();
            foreach (var fx in grpInfo)
            {

                if (fx.Key1TypeN != 2) continue;

                var lstRtu = new List<int>();

                int areaid = fx.Key2;
                int grpid = fx.Key3; // 0 全部  -1 特殊设备 其他组
                var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaid);
                if (grpid > 0) //  组
                {
                    var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(areaid,
                        grpid);
                    if (grp == null) continue;
                    lstRtu =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                }

                if (grpid == 0) // 0 全部  
                {
                    //var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(grpid);
                    //if (grp == null) continue;
                    lstRtu =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmlLstOfArea);
                }

                if (grpid == -1) //  -1 特殊设备  
                {

                    var grp =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(areaid);
                    if (grp.Count == 0) continue;
                    lstRtu = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp);
                }


                foreach (var f in lstRtu)
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    var inputinfo = LoadNode3GetSluInfo(fx, f, idx++);

                    if (inputinfo != null)
                        lstInput.Add(inputinfo);

                }
            }

            return lstInput;

        }

        /// <summary>
        /// 获取一个集中器的inputinfo
        /// </summary>
        /// <param name="fatherInputInfo"></param>
        /// <param name="sluid"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private InputInfo LoadNode3GetSluInfo(InputInfo fatherInputInfo, int sluid, int idx)
        {

            //foreach (var fx in grpInfo)
            {

                if (fatherInputInfo.Key1TypeN != 2) return null;

                int areaid = fatherInputInfo.Key2;
                int grpid = fatherInputInfo.Key3; // 0 全部  -1 特殊设备 其他组

                {
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluid) as Wj2090Slu ;
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Slu) return null;

                    // this.ChildTreeItems.Add(new TreeNodeItemSluViewModel(this, para));

                    var inputinfo = new InputInfo(fatherInputInfo, null, idx, 3, para.RtuId);

                    inputinfo.Id1StoreN = areaid;
                    inputinfo.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组
                    inputinfo.Id3StoreN = para.RtuPhyId;
                    inputinfo.Ld1StoreN = para.DateUpdate;

                    inputinfo.Str1StoreN = new System.Net.IPAddress(BitConverter.GetBytes(para.WjSlu.StaticIp)).ToString();
                    inputinfo.Str2StoreN = para.WjSlu.MobileNo + "";
                    inputinfo.Str3StoreN = para.RtuName;

                    inputinfo.ImagesIcon1B = ImageResources.SluIcon;
                    inputinfo.NodeName1B = para.RtuPhyId.ToString("d2") + "-" + para.RtuName;

                    return inputinfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 后续更新的时候 提供通过treenode生成inputinfo
        /// </summary>
        /// <param name="faTreeViewBaseNode"></param>
        /// <param name="sluid"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private InputInfo LoadNode3GetSluInfo(TreeViewBaseNode faTreeViewBaseNode, int sluid, int idx)
        {

            //foreach (var fx in grpInfo)
            {

                if (faTreeViewBaseNode.Key1TypeN != 2) return null;

                int areaid = faTreeViewBaseNode.Key2;
                int grpid = faTreeViewBaseNode.Key3; // 0 全部  -1 特殊设备 其他组

                {
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluid) as Wj2090Slu ;
                    if (para == null  ) return null;

                    // this.ChildTreeItems.Add(new TreeNodeItemSluViewModel(this, para));

                    var inputinfo = new InputInfo(null, faTreeViewBaseNode, idx, 3, para.RtuId);

                    inputinfo.Id1StoreN = areaid;
                    inputinfo.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组
                    inputinfo.Id3StoreN = para.RtuPhyId;
                    inputinfo.Ld1StoreN = para.DateUpdate;

                    inputinfo.Str1StoreN = new System.Net.IPAddress(BitConverter.GetBytes(para.WjSlu.StaticIp)).ToString(); 
                    inputinfo.Str2StoreN = para.WjSlu.MobileNo + "";
                    inputinfo.Str3StoreN = para.RtuName;




                    inputinfo.ImagesIcon1B = ImageResources.SluIcon;
                    inputinfo.NodeName1B = para.RtuPhyId.ToString("d2") + "-" + para.RtuName;

                    return inputinfo;
                }
            }

            return null;
        }


        /// <summary>
        /// 通过集中器节点 获取集中器下的控制器分组 与控制器节点
        /// </summary>
        /// <param name="sluinInputInfo"></param>
        /// <param name="sluTreeViewBaseNode"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode4GetCtrlInfo(InputInfo sluinInputInfo,
            TreeViewBaseNode sluTreeViewBaseNode = null)
        {
            int idx = 1;
            var lstInput = new List<InputInfo>();
            // var sluInfo = new List<InputInfo>();
            // sluInfo.Add(sluinInputInfo);
            //foreach (var fx in sluInfo)
            {
                if (sluinInputInfo != null && sluTreeViewBaseNode != null) sluTreeViewBaseNode = null;

                int areaid = 0; // fx.Id1StoreN;
                int grpid = 0; // fx.Id2StoreN; // 0 全部  -1 特殊设备 其他组
                int sluid = 0; // fx.Key2;

                if (sluTreeViewBaseNode == null && sluinInputInfo == null) return lstInput;
                if (sluTreeViewBaseNode != null)
                {
                    if (sluTreeViewBaseNode.Key1TypeN != 3) return new List<InputInfo>();
                    areaid = sluTreeViewBaseNode.Id1StoreN;
                    grpid = sluTreeViewBaseNode.Id2StoreN;
                    sluid = sluTreeViewBaseNode.Key2;
                }
                else
                {
                    if (sluinInputInfo.Key1TypeN != 3) return new List<InputInfo>();
                    areaid = sluinInputInfo.Id1StoreN;
                    grpid = sluinInputInfo.Id2StoreN;
                    sluid = sluinInputInfo.Key2;
                }


                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluid);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Slu) return lstInput;
                var sluPara = para as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                if (sluPara == null) return lstInput;





                if (Set.Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected)
                {
                    var lst = (from t in sluPara.WjSluCtrlGrps.Values orderby t.GrpId ascending select t).ToList();
                    var ctrlInfo = sluPara.WjSluCtrls.Values;
                    var concolls = new List<int>();
                    var lstcons = (from t in sluPara.WjSluCtrls select t.Value.CtrlPhyId).ToList();


                    if (lstcons.Count > 0)
                    {


                        var inputinfo = new InputInfo(sluinInputInfo, sluTreeViewBaseNode, idx++, 4, sluid, 0);

                        inputinfo.Id1StoreN = areaid;
                        inputinfo.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组


                        inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                        inputinfo.NodeName1B = "全部控制器";
                        lstInput.Add(inputinfo);

                        //获取组下的节点 

                        var ntx = (from t in lstcons orderby t ascending select t).ToList();
                        foreach (var g in ntx)
                        {
                            foreach (var f in ctrlInfo)
                            {
                                if (g == f.CtrlPhyId)
                                {
                                    int errorcode =
                                        Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(f.SluId,
                                            f.CtrlId, f.LightCount);
                                    int lampnum = 1;
                                    if (f.LightCount > 1) lampnum = 2;
                                    int errorIndex = 2090 * 1000 + lampnum * 100 + errorcode;

                                    var inputinfor = new InputInfo(inputinfo, null, idx++, 5, sluid, f.CtrlId);

                                    inputinfor.Id1StoreN = areaid;
                                    inputinfor.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组
                                    inputinfor.Id3StoreN = 0; //全部  ，控制器分组： 0 全部  -1 特殊设备 其他组
                                    inputinfor.Id4StoreN = f.CtrlPhyId;
                                    inputinfor.Id5StoreN = f.LightCount;
                                    inputinfor.Id6StoreN = errorIndex;

                                    inputinfor.Str1StoreN = f.BarCodeId +"";
                                    inputinfor.Str2StoreN = f.LampCode;
                                   

                                    inputinfor.ImagesIcon1B = Services.ImageResources.GetEquipmentIcon(errorIndex);
                                    inputinfor.NodeName1B = f.CtrlPhyId.ToString("d3") + "-" + f.LampCode;
                                    lstInput.Add(inputinfor);
                                }

                            }
                        }
                    }




                    foreach (var g in lst)
                    {
                        if (g.CtrlPhyLst.Count == 0) continue;

                        var inputinfo = new InputInfo(sluinInputInfo, sluTreeViewBaseNode, idx++, 4, sluid, g.GrpId);

                        inputinfo.Id1StoreN = areaid;
                        inputinfo.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组


                        inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                        inputinfo.NodeName1B = g.GrpId.ToString("d2") + "-" + g.GrpName;
                        lstInput.Add(inputinfo);


                        //获取组下的节点 

                        if (g.CtrlPhyLst.Count == 0) continue;
                        concolls.AddRange(g.CtrlPhyLst);
                        foreach (var fff in g.CtrlPhyLst)
                        {
                            foreach (var f in ctrlInfo)
                            {
                                if (fff == f.CtrlPhyId)
                                {
                                    int errorcode =
                                        Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(f.SluId,
                                            f.CtrlId, f.LightCount);
                                    int lampnum = 1;
                                    if (f.LightCount > 1) lampnum = 2;
                                    int errorIndex = 2090 * 1000 + lampnum * 100 + errorcode;

                                    var inputinfor = new InputInfo(inputinfo, null, idx++, 5, sluid, f.CtrlId);

                                    inputinfor.Id1StoreN = areaid;
                                    inputinfor.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组
                                    inputinfor.Id3StoreN = g.GrpId; //全部  ，控制器分组： 0 全部  -1 特殊设备 其他组
                                    inputinfor.Id4StoreN = f.CtrlPhyId;
                                    inputinfor.Id5StoreN = f.LightCount;
                                    inputinfor.Id6StoreN = errorIndex;


                                    inputinfor.Str1StoreN = f.BarCodeId + "";
                                    inputinfor.Str2StoreN = f.LampCode;

                                    inputinfor.ImagesIcon1B = Services.ImageResources.GetEquipmentIcon(errorIndex);
                                    inputinfor.NodeName1B = f.CtrlPhyId.ToString("d3") + "-" + f.LampCode;
                                    lstInput.Add(inputinfor);

                                }
                            }
                        }
                    }



                    {

                        var inputinfo = new InputInfo(sluinInputInfo, sluTreeViewBaseNode, idx++, 4, sluid, -1);

                        inputinfo.Id1StoreN = areaid;
                        inputinfo.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组



                        inputinfo.ImagesIcon1B = ImageResources.GroupIcon;
                        inputinfo.NodeName1B = "未分组控制器";
                        lstInput.Add(inputinfo);

                        //获取组下的节点 

                        var spe = new List<int>();
                        foreach (var g in lstcons)
                        {
                            if (!concolls.Contains(g)) spe.Add(g);
                        }

                        var ntx = (from t in spe orderby t ascending select t).ToList();
                        foreach (var g in ntx)
                        {
                            foreach (var f in ctrlInfo)
                            {
                                if (g == f.CtrlPhyId)
                                {
                                    int errorcode =
                                        Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlErrorCode(f.SluId,
                                            f.CtrlId, f.LightCount);
                                    int lampnum = 1;
                                    if (f.LightCount > 1) lampnum = 2;
                                    int errorIndex = 2090 * 1000 + lampnum * 100 + errorcode;

                                    var inputinfor = new InputInfo(inputinfo, null, idx++, 5, sluid, f.CtrlId);

                                    inputinfor.Id1StoreN = areaid;
                                    inputinfor.Id2StoreN = grpid; // 0 全部  -1 特殊设备 其他组
                                    inputinfor.Id3StoreN = -1; //全部  ，控制器分组： 0 全部  -1 特殊设备 其他组
                                    inputinfor.Id4StoreN = f.CtrlPhyId;
                                    inputinfor.Id5StoreN = f.LightCount;
                                    inputinfor.Id6StoreN = errorIndex;

                                    inputinfor.Str1StoreN = f.BarCodeId + "";
                                    inputinfor.Str2StoreN = f.LampCode;

                                    inputinfor.ImagesIcon1B = Services.ImageResources.GetEquipmentIcon(errorIndex);
                                    inputinfor.NodeName1B = f.CtrlPhyId.ToString("d3") + "-" + f.LampCode;
                                    lstInput.Add(inputinfor);

                                }
                            }


                        }
                    }


                }


            }

            return lstInput;
        }

        /// <summary>
        /// 系统是否仅加载了一个区域   ，后续区域更新的时候好处理
        /// </summary>
        private int  _onlyOneAreaLoad = -1;


        private Tuple<long, long, long> LstLoadversion = null;

        /// <summary>
        /// 加载节点
        /// </summary>
        private void LoadNode()
        {
            var  v1 = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetVersion;
            var v2 = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.Version;
            var v3 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.Version;
            if (v1 == 0 || v2 == 0 || v3 == 0) return;
            if (LstLoadversion == null)
            {
                LstLoadversion = new Tuple<long, long, long>(v1, v2, v3);
            }
            else
            {
                if (LstLoadversion.Item1 == v1 && LstLoadversion.Item2 == v2 && LstLoadversion.Item3 == v3) return;
                LstLoadversion = new Tuple<long, long, long>(v1, v2, v3);
            }



            var lst1 = LoadNode1GetArea();
            if (lst1.Count == 0) return;

            var rtn = new List<InputInfo>();
            var rootGrp = new List<InputInfo>();
            foreach (var f in lst1) //区域节点 -  集中器组节点
            {

                if (lst1.Count == 1) //只有一个区域  ，集中器分组即为 根节点了 
                {
                    var lst2 = LoadNode2GetAreaGrpInput(f, null);
                    rootGrp.AddRange(lst2);
                    _onlyOneAreaLoad = f ;
                }
                else //否则  区域为根节点
                {
                    var rootinfo = LoadNode2GetAreaInput(f);
                    var lst2 = LoadNode2GetAreaGrpInput(f, rootinfo);
                    rootGrp.Add(rootinfo);
                    rootGrp.AddRange(lst2);
                    _onlyOneAreaLoad = -1;
                }
            }

            rtn.AddRange(rootGrp);


            var lst3 = LoadNode3GetSluInfoByGrp(rootGrp); //通过集中器分组 获取所有的集中器input清单
            rtn.AddRange(lst3);

            foreach (var f in lst3) // 通过集中器的信息 加载控制器分组  - 控制器数据  ，注意此时  控制器分组与控制器 作为一个整体 ，后续参数变化也是如此
            {
                var lst4 = LoadNode4GetCtrlInfo(f, null);
                rtn.AddRange(lst4);
            }



            InitNode(rtn);

        }
    }

    /// <summary>
    /// OnNodeSelected
    /// </summary>
    public partial class TreeTabViewModel
    {
        public static  void OnNodeSelected(long idf, TreeViewBaseNode data)
        {
            if (data == null) return;
            if (data.IsSelected == false) return;
            if (data.Key1TypeN == 3)
            {

                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                };

                args.AddParams(data.Key2);
                //lvf  2018年5月22日14:40:48  记录当前点击终端
                Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = data.Key2;
                EventPublish.PublishEvent(args);


                var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(data.Key2);
                if (TerInfo == null) return;

                data.CmItems = MenuBuilding.BulidCm(((int) TerInfo.RtuModel).ToString(), false, TerInfo);
            }

            if (data.Key1TypeN == 5)
            {
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                };
      
                args.AddParams(data .Key2 );
                args.AddParams(data .Key3 );

                EventPublish.PublishEvent(args);
          

                data.CmItems = MenuBuilding.BulidCm("20900", false, new Tuple<int, int>(data.Key2, data.Key3));
            }

            if (data.Key1TypeN == 2)
            {
                int grpid = data.Key2;
                if (grpid < 1) return;

                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected,
                };
                args.AddParams(grpid);

                EventPublish.PublishEvent(args);
            }

        }


    }

    /// <summary>
    /// 树的动态更新
    /// </summary>
    public partial class TreeTabViewModel
    {

        ///// <summary>
        ///// 集中器分组索引   areaid  -  grpid  （ -1 特殊分组 ，0 所有设备 ，其他 组）
        ///// </summary>
        //private Dictionary<Tuple<int, int>, TreeViewBaseNode> SluGrpInfo =
        //    new Dictionary<Tuple<int, int>, TreeViewBaseNode>();

        ///// <summary>
        ///// 集中器索引 sluid
        ///// </summary>
        //private Dictionary<int, List<TreeViewBaseNode>> SluInfo = new Dictionary<int, List<TreeViewBaseNode>>();

        ///// <summary>
        ///// 控制器 索引   sluid-ctrlid
        ///// </summary>
        //private Dictionary<Tuple<int, int>, List<TreeViewBaseNode>> ConInfo =
        //    new Dictionary<Tuple<int, int>, List<TreeViewBaseNode>>();


        ///// <summary>
        ///// 当有节点删除、新增 时候重绘字典
        ///// </summary>
        //private void OnAddOrDeleteUpdateDic()
        //{
        //    SluGrpInfo.Clear();
        //    SluInfo.Clear();
        //    ConInfo.Clear();

        //    foreach (var f in DicItems)
        //    {
        //        // 2、分组
        //        if (f.Value.TypeNobanding == 2)
        //        {
        //            var tu = new Tuple<int, int>(f.Value.Id1StoreNoBanding, f.Value.Id2StoreNoBanding);
        //            if (SluGrpInfo.ContainsKey(tu) == false) SluGrpInfo.Add(tu, f.Value);
        //        }

        //        // Type = 3  为集中器 
        //        //inputinfo.IsGroup = false;
        //        //inputinfo.Id1StoreNoBanding = fx.Id1StoreNoBanding;
        //        //inputinfo.Id2StoreNoBanding = fx.Id2StoreNoBanding; // 0 全部  -1 特殊设备 其他组
        //        //inputinfo.Id3StoreNoBanding = para.RtuId;
        //        //inputinfo.Id3StoreNoBanding = para.RtuPhyId;
        //        //inputinfo.Lg1StoreNoBanding = para.DateUpdate;
        //        //inputinfo.Type = 3;
        //        if (f.Value.TypeNobanding == 3)
        //        {
        //            if (SluInfo.ContainsKey(f.Value.Id3StoreNoBanding) == false)
        //                SluInfo.Add(f.Value.Id3StoreNoBanding, new List<TreeViewBaseNode>());
        //            SluInfo[f.Value.Id3StoreNoBanding].Add(f.Value);
        //        }

        //        // Type = 5  为控制器
        //        //inputinfor.IsGroup = false;
        //        //inputinfor.Id1StoreNoBanding = areaid;
        //        //inputinfor.Id2StoreNoBanding = grpid; // 0 全部  -1 特殊设备 其他组
        //        //inputinfor.Id3StoreNoBanding = sluid;
        //        //inputinfor.Id4StoreNoBanding = 0; //全部  ，控制器分组： 0 全部  -1 特殊设备 其他组
        //        //inputinfor.Id5StoreNoBanding = f.CtrlId;
        //        //inputinfor.Id6StoreNoBanding = f.CtrlPhyId;
        //        //inputinfor.Id7StoreNoBanding = f.LightCount;
        //        //inputinfor.Id8StoreNoBanding = errorIndex;
        //        if (f.Value.TypeNobanding == 5)
        //        {
        //            var tu = new Tuple<int, int>(f.Value.Id3StoreNoBanding, f.Value.Id5StoreNoBanding);
        //            if (ConInfo.ContainsKey(tu) == false)
        //                ConInfo.Add(tu, new List<TreeViewBaseNode>());
        //            ConInfo[tu].Add(f.Value);
        //        }
        //    }

        //}


        private void OnAddRtu(List<int> rtuList)
        {
            var lst1 = LoadNode1GetArea();
            if (lst1.Count == 0) return;


            var rootGrp = new List<InputInfo>();
            foreach (var f in lst1) //区域节点 -  集中器组节点
            { 
                {
                    var lst2 = LoadNode2GetAreaGrpInput(f, null);
                    rootGrp.AddRange(lst2);
                }
            }

            foreach (var sluid in rtuList)
            {
                foreach (var f in rootGrp)
                {
                    bool isHas = IsThisGrpHasthisSlu(f.Key2, f, sluid);
                    if (isHas)
                    {
                        var root = GetNodeByKey(2, f.Key2, f.Key3);
                        foreach (var l in root)
                        {
                            var sluinputinfo = LoadNode3GetSluInfo(l, sluid, 1);
                            if (sluinputinfo != null)
                            {
                                var lst4 = LoadNode4GetCtrlInfo(sluinputinfo, null);
                                lst4.Add(sluinputinfo);
                                this.AddNode(l.IdfN, lst4, true);
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 通过分组获取 组下集中器节点
        /// </summary>
        /// <param name="areaid"></param>
        /// <param name="infoGrp"></param>
        /// <param name="sluid"></param>
        /// <returns></returns>
        private bool IsThisGrpHasthisSlu(int areaid, InputInfo infoGrp, int sluid)
        {


            int grpid = infoGrp.Key3; // 0 全部  -1 特殊设备 其他组
            var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaid);
            if (grpid > 0) //  组
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(areaid,
                    grpid);
                if (grp == null) return false;
                return grp.LstTml.Contains(sluid);

            }

            if (grpid == 0) // 0 全部  
            {

                var lstRtu =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmlLstOfArea);
                return lstRtu.Contains(sluid);
            }

            if (grpid == -1) //  -1 特殊设备  
            {

                var grp =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(areaid);
                if (grp.Count == 0) return false;
                return grp.Contains(sluid);
            }
            return false;
        }


        /// <summary>
        /// 删除集中器
        /// </summary>
        /// <param name="rtuList"></param>
        private void OnDeleteRtu(List<int> rtuList)
        {

            List<long> dlt = new List<long>();
            foreach (var f in rtuList)
            {
                var nodes = GetNodeByKey(3, f);
                foreach (var fx in nodes)
                {
                    dlt.Add(fx.IdfN);
                }
            }

            this.DeleteNode(dlt);
        }

        private void OnUpdateRtuPara(List<int> rtuList)
        {
            foreach (var f in rtuList)
            {
                var nodes = GetNodeByKey(3, f);
                foreach (var fx in nodes)
                {
                    var inputinfo = LoadNode3GetSluInfo(fx.FatherNodeN, f, fx.ZindexN);
                    UpdateNode(inputinfo); //更新集中器本身的显示数据

                    var child = LoadNode4GetCtrlInfo(null, fx);
                    UpdateNode(fx.IdfN, child);
                }
            }

        }

        /// <summary>
        /// 集中器图标更新
        /// </summary>
        /// <param name="rtuList"></param>
        private void OnUpdateRtuState(List<int> rtuList)
        {
            foreach (var f in rtuList)
            {
                var terInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                if (terInfo == null) continue;

                var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(f);

                int picIndex = 0;
                if (terInfo.EquipmentType == WjParaBase.EquType.Rtu)

                {
                    var s = terInfo.RtuStateCode;
                    if (s == 0)
                    {
                        picIndex = 3001;
                        OnUpdateRtuState(f, picIndex);
                        continue;
                    }

                    if (s == 1)
                    {
                        picIndex = 3002;
                        OnUpdateRtuState(f, picIndex);
                        continue;
                    }

                    var online = runninfo != null && runninfo.IsOnLine;
                    if (online == false)
                    {
                        picIndex = 3003;
                        OnUpdateRtuState(f, picIndex);
                        continue;
                    }

                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = runninfo.ErrorCount > 0;
                    var lighton = runninfo.IsLightHasElectric; // RtuNewDataService.IsRtuHasElectric(this.NodeId);
                    int errorindex = 0;
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton) errorindex = 2;

                    picIndex = 3005 + errorindex;
                    OnUpdateRtuState(f, picIndex);
                }
                else if (terInfo.EquipmentType == WjParaBase.EquType.Slu)
                {
                    if (terInfo.RtuStateCode != 2)
                    {
                        picIndex = (int) WjParaBase.EquType.Slu + 2;
                        OnUpdateRtuState(f, picIndex);
                        continue;
                    }

                    var online = runninfo != null && runninfo.IsOnLine;
                    if (online == false)
                    {
                        picIndex = (int) WjParaBase.EquType.Slu + 3;
                        OnUpdateRtuState(f, picIndex);
                        continue;
                    }

                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = runninfo.ErrorCount > 0;
                    if (haserror)
                    {
                        picIndex = (int) WjParaBase.EquType.Slu + 1;
                        OnUpdateRtuState(f, picIndex);
                    }
                    else
                    {
                        picIndex = (int) WjParaBase.EquType.Slu;
                        OnUpdateRtuState(f, picIndex);
                    }

                }
                else
                {
                    var tmp = runninfo != null && runninfo.ErrorCount > 0;
                    picIndex = (int) terInfo.EquipmentType + (tmp ? 1 : 0);
                    OnUpdateRtuState(f, picIndex);
                }
            }
        }

        /// <summary>
        /// 变换集中器图标 
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="imageCode"></param>
        private void OnUpdateRtuState(int rtuId, int imageCode)
        {
            // foreach (var f in rtuList)
            {
                var nodes = GetNodeByKey(3, rtuId);
                foreach (var f in nodes)
                {
                    if (f.Id4StoreN != imageCode)
                    {
                        f.Id4StoreN = imageCode;
                        f.ImagesIcon1B = ImageResources.GetEquipmentIcon(imageCode);
                    }
                }
            }
        }


        /// <summary>
        /// 更新控制器图标
        /// </summary>
        /// <param name="f"></param>
        private void OnUpdateConnState(long f)
        {
            int sluid = (int) (f / 10000);
            int ctrlid = (int) (f % 10000);


            int imagecode = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlImageCode(f);
            var imagesIcon = Services.ImageResources.GetEquipmentIcon(imagecode);


            var nodes = GetNodeByKey(5, sluid, ctrlid);
            foreach (var l in nodes)
            {
                if (l.Id6StoreN != imagecode)
                {
                    l.Id6StoreN = imagecode;
                    l.ImagesIcon1B = imagesIcon;
                }
            }



        }
        private void OnUpdateConnState(int sluid ,int ctrlid)
        {
   
            var f = sluid * 10000 + ctrlid;

            int imagecode = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetCtrlImageCode(f);
            var imagesIcon = Services.ImageResources.GetEquipmentIcon(imagecode);


            var nodes = GetNodeByKey(5, sluid, ctrlid);
            foreach (var l in nodes)
            {
                if (l.Id6StoreN != imagecode)
                {
                    l.Id6StoreN = imagecode;
                    l.ImagesIcon1B = imagesIcon;
                }
            }



        }


        private void OnAreaGrpChanged(int areaid)
        {

            if (_onlyOneAreaLoad == areaid)
            {
                this.LoadNode();
                return;
            }

            TreeViewBaseNode rooTreeViewBaseNode = null;
            foreach (var f in ChildItems)
            {
                if (f.Key2 == areaid)
                {
                    rooTreeViewBaseNode = f;
                    break;
                }
            }

            if (rooTreeViewBaseNode == null)
            {
                this.LoadNode();
                return;
            }


            var rtn = new List<InputInfo>();
            var rootGrp = new List<InputInfo>();

            //foreach (var f in lst1) //区域节点 -  集中器组节点
            {
                var lst2 = LoadNode2GetAreaGrpInput(areaid, null, rooTreeViewBaseNode);
                rootGrp.AddRange(lst2);
            }


            rtn.AddRange(rootGrp);
            var lst3 = LoadNode3GetSluInfoByGrp(rootGrp); //通过集中器分组 获取所有的集中器input清单
            rtn.AddRange(lst3);

            foreach (var f in lst3) // 通过集中器的信息 加载控制器分组  - 控制器数据  ，注意此时  控制器分组与控制器 作为一个整体 ，后续参数变化也是如此
            {
                var lst4 = LoadNode4GetCtrlInfo(f, null);
                rtn.AddRange(lst4);
            }

            UpdateNode(rooTreeViewBaseNode.IdfN, rtn);
        }


    }

    /// <summary>
    /// 事件 Event
    /// </summary>
    public partial class TreeTabViewModel
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
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                        return true;

                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                        return true;
                    if (args.EventId == EventIdAssign.RunningInfoUpdate1)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<int>;
                        if (lst == null) return false;
                      
                        foreach (var t in lst)
                        {
                            var key = new Tuple<int, int, int, int, int, int>(3, t, 0, 0, 0, 0);
                            
                            if (DicItems.ContainsKey(key)) return true;
 
                        }

                        return false;
                    }

                    if (args.EventId == EventIdAssign.RunningInfoUpdate2)
                    {
                        return false;
                    }

                    if (args.EventId == EventIdAssign.RunningInfoUpdate3)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.MapNeedChangeIcon)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }

            return false;
        }

        private void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv)
                {
                    if (DateTime.Now.Ticks - dtLoad.Ticks < 45 * 10000000) return;
                    LoadNode();
                    return;
                }

                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {
                        LoadNode();
                        Wlst.Cr.Coreb.Servers.WriteLog.WriteLogError("Load Node ");

                    }



                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (lst == null) return;
                        var lsr = (from t in lst select t.Item1).ToList();
                        OnAddRtu(lsr );
                    }

                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (lst == null) return;
                        var lsr = (from t in lst select t.Item1).ToList();
                        OnDeleteRtu(lsr);
                    }


                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (lst == null) return;
                        var lsr = (from t in lst where t.Item2 == 0 select t.Item1).ToList();
                        OnUpdateRtuPara( lsr);

                    }

                    if (args.EventId == EventIdAssign.RunningInfoUpdate1)
                    {
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null) return;
                        OnUpdateRtuState(lst);

                    }

                    if (args.EventId == EventIdAssign.RunningInfoUpdate3)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<long>;
                        if (lst == null) return;
                        foreach (var f in lst)
                        {

                            OnUpdateConnState(f);
                        }
                    }

                    if (args.EventId == EventIdAssign.RunningInfoUpdate2)
                    {
                        return;
        
                    }

                    if (args.EventId == EventIdAssign.MapNeedChangeIcon)
                    {
                        if (args.GetParams().Count < 2) return;
                        var sluId = (int)args.GetParams()[0];
                        var ctrlList = args.GetParams()[1] as IEnumerable<int>;
                        //if (lst == null) return;
                        if (ctrlList == null) return;

                        foreach (var f in ctrlList )
                        OnUpdateConnState(sluId ,f  );

                        //foreach (var t in lst)
                        //{
              

                        //}
                    }


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        try
                        {

                            int x = Convert.ToInt32(args.GetParams()[0]);
                            if (x > 0
                            ) //&& Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(x)
                            {
                                if (args.GetParams().Count > 1)
                                {
                                    int y = Convert.ToInt32(args.GetParams()[1]);
                                    OnCurrentSelectedCtrlNode(x, y);
                                }
                                else
                                {
                                    OnCurrentSelectedNode(x);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



        #endregion

        public bool IsVir
        {
            get { return GetZbool(() => IsVir); }
            set { SetZ(() => IsVir, value); }
        }
    }

}
