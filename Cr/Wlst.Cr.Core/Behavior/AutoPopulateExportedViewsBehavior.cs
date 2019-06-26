using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Cr.Core.Behavior
{
    /// <summary>
    /// prism region需要附加的Behavior信息，主程序识别 为了加速数据显示
    /// </summary>
    [Export(typeof(AutoPopulateExportedViewsBehavior))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AutoPopulateExportedViewsBehavior : RegionBehavior, IPartImportsSatisfiedNotification
    {

        //protected static  List<AutoPopulateExportedViewsBehavior> MySelf = new List<AutoPopulateExportedViewsBehavior>();

        protected static AutoPopulateExportedViewsBehavior MySelfOfSatisfied = null;
        //private static bool isNotShieldLoading = false;
        ///// <summary>
        ///// 手动设置屏蔽自动加载页面
        ///// </summary>
        //public static void SetLoadInner()
        //{
        //    isNotShieldLoading = true;
        //}
        ///// <summary>
        ///// 手动加载页面
        ///// </summary>
        //public static void RunningLoadViewToWindows()
        //{
        //    isNotShieldLoading = false;
        //    //if(MySelf .Count >0)MySelf [0].OnImportsSatisfiedViews();
        //    //if (MySelfOfSatisfied != null) MySelfOfSatisfied.OnImportsSatisfiedViews();
        //    AddRegisteredViews();
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public AutoPopulateExportedViewsBehavior ()
        //{
        //    MySelf.Add(this);
        //}

        private static  bool fsld = false;
        protected override void OnAttach()
        {
         //   if (isNotShieldLoading == false)
            if (fsld)
                AddRegisteredViews();
        }


        public void OnImportsSatisfied()
        {
            MySelfOfSatisfied = this;

            //OnImportsSatisfiedViews();
        }

        public static void OmOnImportsSatisfied()
        {
      
            if (MySelfOfSatisfied != null)
                MySelfOfSatisfied.OnImportsSatisfiedViews();

            if(fsld ==false )
            {
                fsld = true;
                AddRegisteredViews();
            }
        }

        private static readonly ViewComponentHolding ViewComponentHold = new ViewComponentHolding();

        [ImportMany(AllowRecomposition = true)]
        public Lazy<object, IIViewExport>[] RegisteredViews { get; set; }

        #region IPartImportsSatisfiedNotification Views



        ////定义委托
        //private delegate void DoTask();
        ///// <summary>
        ///// 提供外部使用
        ///// </summary>
        //public void OnImportsSatisfiedViewsForOutUse()
        //{
        //    try
        //    {
        //        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
        //                                              new DoTask(OnImportsSatisfiedViews));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}


        private void OnImportsSatisfiedViews()
        {
            try
            {
                CheckSameViewId();
                //  Thread.Sleep(100);
                if (ViewComponentHolding.Count < RegisteredViews.Length)
                {
                    //增加部件
                    foreach (var t in RegisteredViews)
                    {
                        ViewComponentHold.AddViewItem(t.Metadata, t.Value, false);
                    }
                    AddRegisteredViews();
                }
                else if (ViewComponentHolding.Count > RegisteredViews.Length)
                {
                    var lstCollection = new List<string >();
                    foreach (var t in ViewComponentHolding.GetAllViewsID)
                    {
                        lstCollection.Add(t);
                    }
                    //删除部件
                    foreach (var t in lstCollection)
                    {
                        if (!RegisteredViewsContainsId(t))
                        {
                            DeleteRegisteredViews(ViewComponentHolding.GetViewMetadataById(t),
                                                  ViewComponentHolding.GetViewById(t));
                            ViewComponentHold.DeleteViewItem(t, false);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core AutoPopulate happen an super error:" + ex.ToString());
            }

        }

        private void CheckSameViewId()
        {
            //Dictionary<int, IIViewExport> dic = new Dictionary<int, IIViewExport>();
            var lst = new List<string >();
            foreach (var t in RegisteredViews)
            {
                if (lst.Contains(t.Metadata.Id))
                {
                    UtilityFunction.WriteLog.WriteLogError("Supper Error: Two View Has the Same Value:" + t.Metadata.Id);
                    UtilityFunction.WriteLog.WriteLogError("System Will be Run in UnNormal State....");
                }
                else
                {
                    lst.Add(t.Metadata.Id);
                }
            }
        }


        ///// <summary>
        ///// 添加界面view
        ///// </summary>
        //private static  void AddRegisteredViews()
        //{
        //    try
        //    {
        //        foreach (var viewEntry in RegisteredViews)
        //        {
        //            if (!viewEntry.Metadata.AttachNow) continue;
        //            if (
        //                !RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(
        //                    viewEntry.Metadata.AttachRegion)) continue;
        //            var region = RegionManage.RegionManagerInstances.Regions[viewEntry.Metadata.AttachRegion];

        //            try
        //            {
        //                var view = viewEntry.Value;
        //                if (!region.Views.Contains(view))
        //                {

        //                    region.Add(view);
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                WriteLog.WriteLogError(
        //                    "AutoPopulateExportedViewsBehavior Occer An Big Error When resove view :" +
        //                    exception.ToString());
        //                throw;
        //            }
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        WriteLog.WriteLogError(
        //            "AutoPopulateExportedViewsBehavior Occer An Big Error When resove view :" + exception.ToString());
        //    }
        //}

        /// <summary>
        /// 删除界面View
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="view"></param>
        private void DeleteRegisteredViews(IIViewExport metadata, object view)
        {
            try
            {
                if (!RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(metadata.AttachRegion)) return;
                var region = RegionManage.RegionManagerInstances.Regions[metadata.AttachRegion];

                var iv = region.Views;
                if (region.Views.Contains(view))
                {
                    region.Remove(view);
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }



        /// <summary>
        /// 检测重组后的View中是否包含特定ID的View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool RegisteredViewsContainsId(string  id)
        {
            foreach (var t in RegisteredViews)
            {
                if (t.Metadata.Id == id)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 添加界面view
        /// </summary>
        private static void AddRegisteredViews()
        {
            try
            {
                foreach (var viewEntry in ViewComponentHolding.DictionaryViewInterfaceItems  )
                {
                   
                    if (!viewEntry.Value .AttachNow) continue;
                    if (!ViewComponentHolding.DictionaryViewItems.ContainsKey(viewEntry.Key)) continue;

                    if (
                        !RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(
                            viewEntry.Value.AttachRegion)) continue;

                    var region = RegionManage.RegionManagerInstances.Regions[viewEntry.Value.AttachRegion];

                    try
                    {
                       // if (!ViewComponentHolding.DictionaryViewItems.ContainsKey(viewEntry.Key)) continue;
                        var view = ViewComponentHolding.DictionaryViewItems[viewEntry.Key];
                        if (!region.Views.Contains(view))
                        {
                            region.Add(view);
                        }
                    }
                    catch (Exception exception)
                    {
                        WriteLog.WriteLogError(
                            "AutoPopulateExportedViewsBehavior Occer An Big Error When resove view :" +
                            exception.ToString());
                        throw;
                    }
                }

            }
            catch (Exception exception)
            {
                WriteLog.WriteLogError(
                    "AutoPopulateExportedViewsBehavior Occer An Big Error When resove view :" + exception.ToString());
            }
        }
        #endregion

    }
}
