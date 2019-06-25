using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.About.UxAbout
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToHelp : MenuItemBase
    {
        public NavToHelp()
        {
            Id = Wlst.Ux.About.Services.MenuIdAssgin.NavToHelpViewModelMainId;
            Text = "帮助";
            Tag = "帮助";
            Classic = "主菜单";
            Description = "关于，ID 为" + Wlst.Ux.About.Services.MenuIdAssgin.NavToHelpViewModelMainId;
            Tooltips = "帮助";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        private static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            //string v_OpenFolderPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Help" ; 
            //System.Diagnostics.Process.Start("explorer.exe", v_OpenFolderPath);   


            var path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Split('\\');
            int num = path.Count();
            string pathTmp = "";
            for (int i = 0; i < num - 3; i++)
            {
                if (i == 0)
                {
                    pathTmp = path[i];
                }
                else
                {
                    pathTmp = pathTmp + "\\" + path[i];
                }

            }
            pathTmp = pathTmp + "\\docs";
            //string v_OpenFolderPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../../docs";
            System.Diagnostics.Process.Start("explorer.exe", pathTmp);   
        }
    }
}
