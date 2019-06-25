using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using System.Windows;
using System.Collections.Specialized;
using Telerik.Windows.Controls;
using Wlst.Cr.CoreOne.Services;

namespace Xboot.PopupWindow
{
    /// <summary>
    /// Defines a behavior that creates a Dialog to display the active view of the target <see cref="IRegion"/>.
    /// </summary>
    public  class DialogActivationBehavior : RegionBehavior, IHostAwareRegionBehavior
    {
        /// <summary>
        /// The key of this behavior
        /// </summary>
        public const string BehaviorKey = "DialogActivation";

    

       // public string DocumentReionName = "";



        static DocumentWindow _documentWindowIns;
        protected  static DocumentWindow  DocumentWindowIns
        {
            get
            {
                if(_documentWindowIns ==null )
                {
                    _documentWindowIns = new DocumentWindow();
                    _documentWindowIns.DocumentRegionName = RegionNames.DocumentRegion;
                    _documentWindowIns.OnDeletedUserControl += new System.EventHandler<DeleteUserControlArgs>(_documentWindowIns_OnDeletedUserControl);
                }
                return _documentWindowIns;
            }
        }

        static  void _documentWindowIns_OnDeletedUserControl(object sender, DeleteUserControlArgs e)
        {
            //throw new System.NotImplementedException();
            if (e.DeleteUserControl == null) return;


            foreach (var t in Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions)//in Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions)
            {

                if (t.Views.Contains(e.DeleteUserControl))
                {
                    t.Remove(e.DeleteUserControl);
                }
                else
                {
                    var fff = e.DeleteUserControl as RadPane;
                    if (fff != null)
                    {
                        var ggg = fff.Content;
                        if (ggg != null)
                        {
                            if (t.Views.Contains(ggg))
                            {
                                t.Remove(ggg);
                            }
                        }
                    }
                }

            }
        }


        /// <summary>
        /// Gets or sets the <see cref="DependencyObject"/> that the <see cref="IRegion"/> is attached to.
        /// </summary>
        /// <value>A <see cref="DependencyObject"/> that the <see cref="IRegion"/> is attached to.
        /// This is usually a <see cref="FrameworkElement"/> that is part of the tree.</value>
        public DependencyObject HostControl { get; set; }

        /// <summary>
        /// Performs the logic after the behavior has been attached.
        /// </summary>
        protected override void OnAttach()
        {
            this.Region.ActiveViews.CollectionChanged += this.ActiveViews_CollectionChanged;
            this.Region.OnViewActivated += new ActiveViewEventHandler(Region_OnViewActivated);
           
        }

        void Region_OnViewActivated(object sender, ActiveViewEventArgs e)
        {
            //throw new System.NotImplementedException();
            DocumentWindowIns.AddUserControl(e.View, true);
        }


        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (stopcount > 0)
                {
                    stopcount--;
                    foreach (var t in e.NewItems)
                        DocumentWindowIns.AddUserControl(t, false);
                }
                else
                    foreach (var t in e.NewItems)
                        DocumentWindowIns.AddUserControl(t, true);
                //DocumentWindowIns.RemovePanFromParent(e.NewItems[0]);

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var t in e.OldItems)
                    DocumentWindowIns.RemovePanFromParent(t);
            }
        }

        private int stopcount = 3;

    }
}
