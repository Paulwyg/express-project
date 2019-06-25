using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.SettingHolding;
using Wlst.Ux.Setting.SettingViewModel.Services;
using Wlst.Ux.Setting.SettingViewModel.ViewModel;
using Wlst.Ux.Setting.SettingViewNew.baseview;

namespace Wlst.Ux.Setting.SettingViewNew
{

    //[Export(typeof(IISettingViewModel))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public class SettingViewNewModel : ObservableObject, IISettingViewModel
    {


        public OptionView ShowView = null;



        //public void FundEventHandler()
        //{
        //    int viewId = 0;

        //    var obs = Wlst.Cr.Core.CoreServices.RegionManage.GetViewById(viewId);

        //    if (obs != null)
        //        View = obs;
        //}



        public void NavOnLoad(params object[] parsObjects)
        {
            ShowView.Items.Clear();
            ShowView.BackGroundView = "#ffffff";
            ShowView.BackGroundOther= "#f0f0f1";

            var userProperty = UserInfo.UserLoginInfo;
            //this.ChildTreeItems.Clear();
            foreach (var t in SettingComponentHolding.GetAllComponentIDs)
            {
                var tmp = SettingComponentHolding.GetMenuItemById(t);
                if (tmp == null) continue;
                if (string.IsNullOrEmpty(tmp.Describle) == false && tmp.Describle.Contains("admin") &&
                    userProperty.D == false)
                {
                    continue;
                }
                //this.AddNode(tmp);
                var obs = Wlst.Cr.Core.CoreServices.RegionManage.GetViewById(tmp .ViewId );
                var vis = obs as ContentControl;
                if (vis == null) continue;

                if (tmp.Key.Contains("Setting"))
                {
                    ShowView.AddItem(tmp.PathSetting, vis);
                }
            }
        }

        //private void AddNode(IISetting keyValue)
        //{
        //    if (keyValue.Key.Contains("Setting"))
        //    {
        //        //string path =
        //        //    I36N .Services.I36N .ConvertByCodingOne(keyValue.Id.ToString(CultureInfo.InvariantCulture), "Null");
        //        //if (path.Contains("Missing"))
        //        //{
        //        string path = keyValue.PathSetting;


        //        string[] sp = path.Split('#');
        //        if (sp.Length == 1)
        //        {
        //            this.ChildTreeItems.Add(new TreeNodeViewModel(keyValue.ViewId, sp[0]));
        //        }
        //        else
        //        {
        //            if (sp.Length < 2) return;
        //            foreach (var t in this.ChildTreeItems)
        //            {
        //                if (t.NodeName.Equals(sp[0].Trim()))
        //                {
        //                    t.ChildTreeItems.Add(new TreeNodeViewModel(keyValue.ViewId, sp[1]));
        //                    return;
        //                }
        //            }
        //            var f = new TreeNodeViewModel(0, sp[0].Trim());
        //            this.ChildTreeItems.Add(f);
        //            f.ChildTreeItems.Add(new TreeNodeViewModel(keyValue.ViewId, sp[1]));
        //        }
        //    }
        //}

        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                return "选项";
                //return "Setting";
            }
        }

        public bool CanClose
        {
            get { return true; }
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
    }
}
