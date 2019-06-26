//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;
//using System.Linq;
//using System.Threading;
//using System.Windows;
//using Microsoft.Practices.Prism.Regions;
//using Wlst.Cr.Core.ComponentHold;
//using Wlst.Cr.Core.CoreInterface;
//using Wlst.Cr.Core.CoreServices;
//using Wlst.Cr.Core.UtilityFunction;

//namespace Wlst.Cr.Core.Behavior
//{
//    /// <summary>
//    /// prism region需要附加的Behavior信息，主程序识别
//    /// </summary>
//    [Export(typeof (AutoPopulateExportedViewsBehaviorThread))]
//    [PartCreationPolicy(CreationPolicy.NonShared)]
//    public class AutoPopulateExportedViewsBehaviorThread : RegionBehavior, IPartImportsSatisfiedNotification
//    {

//      //  public static AutoPopulateExportedViewsBehaviorThread MySelf;

       
           
//        /// <summary>
//        /// 
//        /// </summary>
//        public AutoPopulateExportedViewsBehaviorThread()
//        {
//            //if (MySelf != null) return;
//            //MySelf = this;
//        }

//        protected override void OnAttach()
//        {
//           // if (ModuleServices.ModuleManage.AutoLoading) return;
//            this.OnImportsSatisfiedViews();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void OnImportsSatisfied()
//        {
//           // if (ModuleServices.ModuleManage.AutoLoading) return;
//            this.OnImportsSatisfiedViews();
//        }


//        private void OnImportsSatisfiedViews()
//        {
//            try
//            {
//                var tmp = new List<Tuple<IIViewExport, object>>();
//                foreach (var t in RegisteredViews)
//                {
//                    tmp.Add(new Tuple<IIViewExport, object>(t.Metadata, t.Value));
//                }
//                if (tmp.Count == 0) return;
//                SenExp.MySelf.NewAdd(tmp);
//            }
//            catch (Exception ex)
//            {
//                UtilityFunction.WriteLog.WriteLogError("AutoPopulateExportedViewsBehavior Error in OnImportsSatisfiedViews" +
//                                                               ex );
//            }
//        }


//        private delegate void DoTask();
//        /// <summary>
//        /// 提供外部使用
//        /// </summary>
//        public void OnImportsSatisfiedViewsForOutUse()
//        {
//            try
//            {
//               // this.OnImportsSatisfiedViews();
//                Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
//                                                      new DoTask(OnImportsSatisfiedViews));
//            }
//            catch (Exception ex)
//            {
//                UtilityFunction.WriteLog.WriteLogError("AutoPopulateExportedViewsBehavior Error in OnImportsSatisfiedViewsForOutUse" +
//                                                           ex);
//            }
//        }



//        [ImportMany(AllowRecomposition = true)]
//        public Lazy<object, IIViewExport>[] RegisteredViews { get; set; }

//    }


//    internal class SenExp
//    {
//        private static SenExp _mySelf = null;
//        public static SenExp MySelf
//        {
//            get
//            {
//                if(_mySelf==null ) new SenExp();
//                return _mySelf;
//            }
//        }

//        private  SenExp()
//        {
//            if (_mySelf != null) return;
//            _mySelf = this;
//            _queue = new ConcurrentQueue<List<Tuple<IIViewExport, object>>>();
//            viewComponentHolding = new ViewComponentHolding();
//            _dtAdd = new Dictionary<int, long>();
//            _manevent = new ManualResetEvent(false);
//            _backThread = new Thread(ThRun);
//            _backThread.Start();
//        }

//        private ManualResetEvent _manevent;
//        private Thread _backThread;

//        private ConcurrentQueue<List<Tuple<IIViewExport, object>>> _queue;



//        public void NewAdd(List<Tuple<IIViewExport, object>> registeredViews)
//        {
//            try
//            {
//                _queue.Enqueue(registeredViews);
//                _manevent.Set();
//            }
//            catch (Exception ex)
//            {
//                UtilityFunction.WriteLog.WriteLogError("SenExp AutoPopulateExportedViewsBehavior Error in NewAdd" +
//                                                         ex);
//            }
//        }


//        private void ThRun()
//        {
//            while (true)
//            {
//                try
//                {
//                    List<Tuple<IIViewExport, object>> tmp = null;
//                    while (_queue.Count > 0)
//                    {
//                        _queue.TryDequeue(out tmp);
//                    }

//                    if (tmp != null)
//                    {
//                        OnImportsSatisfiedViews(tmp);
//                    }

//                    Thread.Sleep(100);
//                    _manevent.WaitOne();
//                }
//                catch (Exception ex)
//                {
//                    UtilityFunction.WriteLog.WriteLogError("SenExp AutoPopulateExportedViewsBehavior Error in ThRun" +
//                                           ex);
//                }

//            }

//        }

//        private ViewComponentHolding viewComponentHolding;


//        private void OnImportsSatisfiedViews(List<Tuple<IIViewExport, object>> RegisteredViews)
//        {
//            try
//            {
//                var lst = new List<int>();
//                foreach (var t in RegisteredViews)
//                {
//                    if (lst.Contains(t.Item1 .ID))
//                    {
//                        UtilityFunction.WriteLog.WriteLogError("Supper Error: Two View Has the Same Value:" +
//                                                               t.Item1 .ID);
//                        UtilityFunction.WriteLog.WriteLogError("System Will be Run in UnNormal State....");
//                    }
//                    else
//                    {
//                        lst.Add(t.Item1 .ID);
//                    }
//                }
//                //  Thread.Sleep(100);
//                if (ViewComponentHolding.Count < RegisteredViews.Count )
//                {
//                    //增加部件
//                    foreach (var t in RegisteredViews)
//                    {
//                        viewComponentHolding.AddViewItem(t.Item1, t.Item2, false);
//                        AddRegisteredViews(t.Item1, t.Item2);
//                    }
//                }
//                else if (ViewComponentHolding.Count > RegisteredViews.Count )
//                {
//                    var lstCollection = new List<int>();
//                    foreach (var t in ViewComponentHolding.GetAllViewsID)
//                    {
//                        lstCollection.Add(t);
//                    }
//                    //删除部件
//                    foreach (var t in lstCollection)
//                    {

//                        DeleteRegisteredViews(ViewComponentHolding.GetViewMetadataById(t),
//                                              ViewComponentHolding.GetViewById(t));
//                        viewComponentHolding.DeleteViewItem(t, false);

//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError("Core AutoPopulate happen an super error:" + ex.ToString());
//            }

//        }



//        #region 增加界面显示 AddRegisteredViews(IIViewExport metadata, object view)


//        private Dictionary<int, long> _dtAdd;// = new Dictionary<int, long>();

//        /// <summary>
//        /// 删除界面View
//        /// </summary>
//        /// <param name="metadata"></param>
//        /// <param name="view"></param>
//        private void AddRegisteredViews(IIViewExport metadata, object view)
//        {
//            try
//            {
//                if (metadata == null) return;
//                if (!metadata.AttachNow) return;
//                if (!RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(metadata.AttachRegion)) return;
//                var region = RegionManage.RegionManagerInstances.Regions[metadata.AttachRegion];


//                if (!region.Views.Contains(view))
//                {
//                    if (_dtAdd.ContainsKey(metadata.ID))
//                    {
//                        if (_dtAdd[metadata.ID] + 600000000 > DateTime.Now.Ticks) return;
//                    }
//                    else
//                    {
//                        _dtAdd.Add(metadata.ID, DateTime.Now.Ticks);
//                    }

//                    AddRegisterRegionView(metadata.AttachRegion, view);
//                }

//            }
//            catch (Exception ex)
//            {
//                ex.ToString();
//            }
//        }


//        //定义委托 
//        private delegate void DoTaskAdd(string reg, object obj);

//        private void AddRegisterRegionView(string regionName, object view)
//        {
//            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
//                                                  new DoTaskAdd(AddRegisterRegionViewDelegate), regionName, view);
//        }

//        private void AddRegisterRegionViewDelegate(string regionName, object view)
//        {
//            try
//            {
//                if (!RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(regionName)) return;
//                var region = RegionManage.RegionManagerInstances.Regions[regionName];

//                if (!region.Views.Contains(view))
//                {
//                    region.Add(view);
//                }

//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError(
//                    "AutoPopulateExportedViewsBehavior Occer An Big Error When resove view :" +
//                    ex.ToString());
//            }
//        }

//        #endregion

//        #region 删除界面View  DeleteRegisteredViews(IIViewExport metadata, object view)

//        /// <summary>
//        /// 删除界面View
//        /// </summary>
//        /// <param name="metadata"></param>
//        /// <param name="view"></param>
//        private void DeleteRegisteredViews(IIViewExport metadata, object view)
//        {
//            try
//            {
//                if (metadata == null) return;
//                if (!RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(metadata.AttachRegion)) return;
//                var region = RegionManage.RegionManagerInstances.Regions[metadata.AttachRegion];

//                //    var iv = region.Views;
//                if (region.Views.Contains(view))
//                {
//                    //region.Remove(view);
//                    DeleteRegisterRegionView(metadata.AttachRegion, view);
//                }

//            }
//            catch (Exception ex)
//            {
//                UtilityFunction.WriteLog.WriteLogError("SenExp AutoPopulateExportedViewsBehavior Error in DeleteRegisteredViews" +
//                                          ex);
//            }
//        }


//        //定义委托
//        private delegate void DoTaskRemove(string reg, object obj);

//        private void DeleteRegisterRegionView(string regionName, object view)
//        {
//            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
//                                                  new DoTaskRemove(DeleteRegisterRegionViewDelegate), regionName, view);
//        }

//        private void DeleteRegisterRegionViewDelegate(string regionName, object view)
//        {
//            try
//            {
//                if (!RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(regionName)) return;
//                var region = RegionManage.RegionManagerInstances.Regions[regionName];

//                if (region.Views.Contains(view))
//                {
//                    region.Remove(view);
//                }

//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError(
//                    "AutoPopulateExportedViewsBehavior Occer An Big Error When remove view :" +
//                    ex.ToString());
//            }
//        }

//        #endregion

//    }
//}
