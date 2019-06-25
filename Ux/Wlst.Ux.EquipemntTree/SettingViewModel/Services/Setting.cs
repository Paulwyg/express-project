using System.Collections.Generic;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;

namespace Wlst.Ux.EquipemntTree.SettingViewModel.Services
{
    /// <summary>
    /// 本模块的设置信息
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// 在分组显示时  是否显示id值
        /// </summary>
        public bool IsShowGrpInTreeModelShowIdOrNot
        {
            set { UxTreeSetting.IsShowGrpInTreeModelShowId = value; }
            get { return UxTreeSetting.IsShowGrpInTreeModelShowId; }
        }



        /// <summary>
        /// 在分组显示终端级别时  是否显示终端下的设备以及回路
        /// </summary>
        public bool IsShowGrpInTreeModelShowTmlChildNodeOrNot
        {
            set { UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode = value; }
            get { return UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode; }
        }



        /// <summary>
        /// 是否在主界面显示 单终端树
        /// </summary>
        public bool IsShowSingleTreeOnTabOrNot
        {
            set
            {
                if (UxTreeSetting.IsShowSingleTreeOnTab == value) return;
                UxTreeSetting.IsShowSingleTreeOnTab = value;
                RegionManage.ShowViewByIdAttachRegion(
                    EquipemntTree.Services.ViewIdAssign.GrpSingleTabShowViewId,
             
                    UxTreeSetting.IsShowSingleTreeOnTab);

                //CETC50_Core .UtilityFunction .SQLiteHelper.ExecuteNonQuery( )
            }
            get { return UxTreeSetting.IsShowSingleTreeOnTab; }
        }


        /// <summary>
        /// 是否在主界面显示多终端树
        /// </summary>
        public bool IsShowMulTreeOnTabOrNot
        {
            set
            {
                if (UxTreeSetting.IsShowMulTreeOnTab == value) return;
                UxTreeSetting.IsShowMulTreeOnTab = value;
                RegionManage.ShowViewByIdAttachRegion(
                    //Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleMulTreeViewId,
                    //Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleMulTreeViewAttachRegion,
                    EquipemntTree.Services.ViewIdAssign.GrpMulityTabShowViewId,
             
                    UxTreeSetting.IsShowMulTreeOnTab);
            }
            get { return UxTreeSetting.IsShowMulTreeOnTab; }
        }

        public bool IsSelectGrpMapOnlyShow
        {
            set
            {
                if (UxTreeSetting.IsSelectGrpMapOnlyShow == value) return;
                UxTreeSetting.IsSelectGrpMapOnlyShow = value;
                if (value == false)
                {
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
            }
            get { return UxTreeSetting.IsSelectGrpMapOnlyShow; }
        }


        private bool isShowTheSelectdNodeInTree;

        public bool IsShowTheSelectdNodeInTree
        {
            get { return isShowTheSelectdNodeInTree; }
            set
            {
                isShowTheSelectdNodeInTree = value;
                //if (Views.GrpSingleTabShowView.MySelf != null)
                //{
                //    Views.GrpSingleTabShowView.MySelf.tvProperties.IsVirtualizing = !value;
                //}
            }
        }


     
        /// <summary>
        /// </summary>
        public int TreeSortBy
        {
            set { UxTreeSetting.TreeSortBy = value; }
            get { return UxTreeSetting.TreeSortBy; }
        }

          /// <summary>
        /// </summary>
        public bool  IsRutsNotShowError
        {
            set { UxTreeSetting.IsRutsNotShowError = value; }
            get { return UxTreeSetting.IsRutsNotShowError; }
        }
      
        /// <summary>
        ///终端树 快速操作中不显示无效开关量 0 不显示 1 屏蔽 2 全部
        /// </summary>
        public int IsRutsNotShowNullK
        {
            set { UxTreeSetting.IsRutsNotShowNullK = value; }
            get { return UxTreeSetting.IsRutsNotShowNullK; }
        }
        /// <summary>
        ///终端树 显示快速操作  0 普通模式 1 快速模式
        /// </summary>
        public int IsShowRapidOp
        {
            set { UxTreeSetting.IsShowRapidOp = value; }
            get { return UxTreeSetting.IsShowRapidOp; }
        }
        /// <summary>
        /// 快速查询显示上限
        /// </summary>
        public int SearchLimit
        {
            set { UxTreeSetting.SearchLimit = value; }
            get { return UxTreeSetting.SearchLimit; }
        }
    }
}
