using System;
using System.Collections.Generic;
using System.Data;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Ux.CrissCrossEquipemntTree.SettingViewModel.Services
{
    public class SettingExtend : Setting
    {

        private static readonly object Synobj = new object();

        private static SettingExtend _myself;

        public static SettingExtend Myself
        {
            get
            {
                if (_myself == null)
                {
                    lock (Synobj)
                    {
                        if (_myself == null)
                        {
                            new SettingExtend();
                        }
                    }
                }
                return _myself;
            }
        }

        private SettingExtend()
        {
            if (_myself == null)
            {
                _myself = this;
                this.InitLoad();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isShowGrpInTreeModelShowId"></param>
        /// <param name="isShowGrpInTreeModelShowTmlChildNode"></param>
        /// <param name="isShowSingleTreeOnTab"></param>
        /// <param name="isShowMulTreeOnTab"></param>
        /// <param name="isSelectGrpMapOnlyShow"> </param>
        /// <param name="isShowTheSelectdNodeInTree"> </param>
        public void UpdateSetting(bool isShowGrpInTreeModelShowId, bool isShowGrpInTreeModelShowTmlChildNode,
                                  bool isShowSingleTreeOnTab, bool isShowMulTreeOnTab, bool isSelectGrpMapOnlyShow, bool isShowTheSelectdNodeInTree, int sortby, bool isRutsNotShowError, int isRutsNotShowNullK,int isShowRapidOp,int searchLimit)
        {
            try
            {
                this.IsShowGrpInTreeModelShowIdOrNot = isShowGrpInTreeModelShowId;
                this.IsShowGrpInTreeModelShowTmlChildNodeOrNot = isShowGrpInTreeModelShowTmlChildNode;
                this.IsShowMulTreeOnTabOrNot = isShowMulTreeOnTab;
                this.IsShowSingleTreeOnTabOrNot = isShowSingleTreeOnTab;
                this.IsSelectGrpMapOnlyShow = isSelectGrpMapOnlyShow;
                this.IsShowTheSelectdNodeInTree = isShowTheSelectdNodeInTree;
                this.TreeSortBy = sortby;
                this.IsRutsNotShowError = isRutsNotShowError;
                this.IsRutsNotShowNullK = isRutsNotShowNullK;
                this.IsShowRapidOp = isShowRapidOp;
                this.SearchLimit = searchLimit;
                SavConfig();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        public void UpdateSetting(bool isShowSingleTreeOnTab)
        {
            try
            {
                this.IsShowSingleTreeOnTabOrNot = isShowSingleTreeOnTab;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }






        private bool _loadCOnfig = false;
        public const string XmlConfigName = "CrissCrossTabTreeSetConfig";

        private void InitLoad()
        {
            if (_loadCOnfig) return;

            _loadCOnfig = true;


            IsShowSingleTreeOnTabOrNot = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4002, 1, false);


        }




        private void SavConfig()
        {
            //    var info = new Dictionary<string, string>();
            //    if (IsShowSingleTreeOnTabOrNot) info.Add("IsShowSingleTreeOnTabOrNot", "yes");
            //    else info.Add("IsShowSingleTreeOnTabOrNot", "no");

            //    info.Add("IsRutsNotShowNullK", IsRutsNotShowNullK + "");

            //    info.Add("IsShowRapidOp", IsShowRapidOp + "");

            //    info.Add("SearchLimit", SearchLimit + "");
            //    Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);



            var dic = new Dictionary<int, string>();
            var desc = new Dictionary<int, string>();

            dic.Add(1, IsShowSingleTreeOnTabOrNot?  "1" : "0");
            desc.Add(1, "是否显示交叉分组");


            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(4002, dic, desc);




        }

    }
}
