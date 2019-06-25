using System;
using System.Collections.Generic;
using System.Data;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Ux.EquipemntTree.SettingViewModel.Services
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






        private bool _loadCOnfig = false;
        public const string XmlConfigName = "TabTreeSetConfg";

        private void InitLoad()
        {
            if (_loadCOnfig) return;

            _loadCOnfig = true;


            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("IsShowGrpInTreeModelShowIdOrNot"))
            {
                IsShowGrpInTreeModelShowIdOrNot = info["IsShowGrpInTreeModelShowIdOrNot"].Contains("yes");
            }
            else IsShowGrpInTreeModelShowIdOrNot = true;

            if (info.ContainsKey("IsShowGrpInTreeModelShowTmlChildNodeOrNot"))
            {
                IsShowGrpInTreeModelShowTmlChildNodeOrNot =
                    info["IsShowGrpInTreeModelShowTmlChildNodeOrNot"].Contains("yes");
            }
            else IsShowGrpInTreeModelShowTmlChildNodeOrNot = true;

            if (info.ContainsKey("IsShowMulTreeOnTabOrNot"))
            {
                IsShowMulTreeOnTabOrNot = info["IsShowMulTreeOnTabOrNot"].Contains("yes");
            }
            else IsShowMulTreeOnTabOrNot = true;


            if (info.ContainsKey("IsShowSingleTreeOnTabOrNot"))
            {
                IsShowSingleTreeOnTabOrNot = info["IsShowSingleTreeOnTabOrNot"].Contains("yes");
            }
            else IsShowSingleTreeOnTabOrNot = true;


            if (info.ContainsKey("IsSelectGrpMapOnlyShow"))
            {
                IsSelectGrpMapOnlyShow = info["IsSelectGrpMapOnlyShow"].Contains("yes");
            }
            else IsSelectGrpMapOnlyShow = true ;

            if (info.ContainsKey("IsShowTheSelectdNodeInTree"))
            {
                IsShowTheSelectdNodeInTree = info["IsShowTheSelectdNodeInTree"].Contains("yes");
            }
            else IsShowTheSelectdNodeInTree = false  ;


            if (info.ContainsKey("IsRutsNotShowError"))
            {
                IsRutsNotShowError = info["IsRutsNotShowError"].Contains("yes");
            }
            else IsRutsNotShowError  = false;

            //if (info.ContainsKey("IsRutsNotShowNullK"))
            //{
            //    IsRutsNotShowNullK = info["IsRutsNotShowNullK"].Contains("yes");
            //}
            //else IsRutsNotShowNullK = false;
            if (info.ContainsKey("IsRutsNotShowNullK"))
            {
                try
                {
                    IsRutsNotShowNullK = Convert.ToInt32(info["IsRutsNotShowNullK"]);
                }
                catch (Exception ex)
                {
                    IsRutsNotShowNullK = 1;
                }
            }

            //if (info.ContainsKey("IsShowRapidOp"))
            //{
            //    IsShowRapidOp = info["IsShowRapidOp"].Contains("yes");
            //}
            //else IsShowRapidOp = false;
            if (info.ContainsKey("IsShowRapidOp"))
            {
                try
                {
                    IsShowRapidOp = Convert.ToInt32(info["IsShowRapidOp"]);
                }
                catch (Exception ex)
                {
                    IsShowRapidOp = 0;
                }
            }

            if (info.ContainsKey("SearchLimit"))
            {
                SearchLimit = Convert.ToInt32(info["SearchLimit"]);
            }
            else SearchLimit = 0;

            TreeSortBy = 1;
            if (info.ContainsKey("TreeSortBy"))
            {
                try
                {
                  
                    TreeSortBy = Convert.ToInt32(info["TreeSortBy"]);
                }
                catch(Exception ex)
                {
                    
                }
            }
           // else IsShowTheSelectdNodeInTree = false;
        }




        private void SavConfig()
        {
            var info = new Dictionary<string, string>();
            if (IsShowGrpInTreeModelShowIdOrNot) info.Add("IsShowGrpInTreeModelShowIdOrNot", "yes");
            else info.Add("IsShowGrpInTreeModelShowIdOrNot", "no");

            if (IsShowGrpInTreeModelShowTmlChildNodeOrNot) info.Add("IsShowGrpInTreeModelShowTmlChildNodeOrNot", "yes");
            else info.Add("IsShowGrpInTreeModelShowTmlChildNodeOrNot", "no");

            if (IsShowMulTreeOnTabOrNot) info.Add("IsShowMulTreeOnTabOrNot", "yes");
            else info.Add("IsShowMulTreeOnTabOrNot", "no");

            if (IsShowSingleTreeOnTabOrNot) info.Add("IsShowSingleTreeOnTabOrNot", "yes");
            else info.Add("IsShowSingleTreeOnTabOrNot", "no");

            if (IsSelectGrpMapOnlyShow) info.Add("IsSelectGrpMapOnlyShow", "yes");
            else info.Add("IsSelectGrpMapOnlyShow", "no");
            if (IsShowTheSelectdNodeInTree) info.Add("IsShowTheSelectdNodeInTree", "yes");
            else info.Add("IsShowTheSelectdNodeInTree", "no");

            if (IsRutsNotShowError) info.Add("IsRutsNotShowError", "yes");
            else info.Add("IsRutsNotShowError", "no");

            //if (IsRutsNotShowNullK) info.Add("IsRutsNotShowNullK", "yes");
            //else info.Add("IsRutsNotShowNullK", "no");
            info.Add("IsRutsNotShowNullK", IsRutsNotShowNullK + "");

            //if (IsShowRapidOp) info.Add("IsShowRapidOp", "yes");
            //else info.Add("IsShowRapidOp", "no");
            info.Add("IsShowRapidOp", IsShowRapidOp + "");

            info.Add("TreeSortBy", TreeSortBy +"");

            info.Add("SearchLimit", SearchLimit + "");
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);


        }

    }
}
