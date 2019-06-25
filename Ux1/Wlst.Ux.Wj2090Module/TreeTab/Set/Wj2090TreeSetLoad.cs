using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj2090Module.TreeTab.Set
{


    public class Wj2090TreeSetLoad
    {
        public bool IsShowConOnNodeSelected;
        public bool IsShowTreeOnTab;
        public bool IsShowGrpInTreeModelShowId;
        public bool IsIconFollowTheRtu;


        static Wj2090TreeSetLoad myself;
        public static Wj2090TreeSetLoad Myself
        {
            get
            {
                if (myself == null) new Wj2090TreeSetLoad();
                return myself;
            }
        }


        public const string XmlConfigName = "Wj2090SetConfg";

        private Wj2090TreeSetLoad()
        {

            if (myself == null) myself = this;



            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("IsShowConOnNodeSelected"))
            {
                IsShowConOnNodeSelected = info["IsShowConOnNodeSelected"].Contains("yes");
            }
            else IsShowConOnNodeSelected = true;

            if (info.ContainsKey("IsShowGrpInTreeModelShowId"))
            {
                IsShowGrpInTreeModelShowId = info["IsShowGrpInTreeModelShowId"].Contains("yes");
            }
            else IsShowGrpInTreeModelShowId = true;

            if (info.ContainsKey("IsShowTreeOnTab"))
            {
                IsShowTreeOnTab = info["IsShowTreeOnTab"].Contains("yes");
            }
            else IsShowTreeOnTab = true;

            if (info.ContainsKey("IsIconFollowTheRtu"))
            {
                IsIconFollowTheRtu = info["IsIconFollowTheRtu"].Contains("yes");
            }
            else IsIconFollowTheRtu = true;

        }




        public void SavConfig()
        {
            var info = new Dictionary<string, string>();
            if (IsShowConOnNodeSelected) info.Add("IsShowConOnNodeSelected", "yes");
            else info.Add("IsShowConOnNodeSelected", "no");

            if (IsShowGrpInTreeModelShowId) info.Add("IsShowGrpInTreeModelShowId", "yes");
            else info.Add("IsShowGrpInTreeModelShowId", "no");

            if (IsShowTreeOnTab) info.Add("IsShowTreeOnTab", "yes");
            else info.Add("IsShowTreeOnTab", "no");

            if (IsIconFollowTheRtu) info.Add("IsIconFollowTheRtu", "yes");
            else info.Add("IsIconFollowTheRtu", "no");

            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);


            RegionManage.ShowViewByIdAttachRegion(
                Ux.Wj2090Module.Services.ViewIdAssign.Wj2090TreeViewId,
           
                IsShowTreeOnTab);
        }

    };
}
