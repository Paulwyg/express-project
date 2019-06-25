using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using WindowForWlst;
using Wlst.Cr.Core.CoreInterface;

namespace Xboot.PopupWindow
{
    /// <summary>
    /// DocumentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentWindow : CustomChromeWindow
    {

        public string DocumentRegionName = "Document";

        public DocumentWindow()
        {
            InitializeComponent();

            
            Panes.RadPaneGroupOverride .OnItemsChangeded += new EventHandler(_radPaneGroup_OnItemsChangeded);
            this.DataContext = this;
            //this.Topmost = true;

        }

        public event EventHandler<DeleteUserControlArgs> OnDeletedUserControl;

        /// <summary>
        /// addd or  select
        /// </summary>
        /// <param name="obj"></param>
        public void AddUserControl(object obj,bool show)
        {
            AddValueToRadPaneGroup(obj,show );
        }


        private void _radPaneGroup_OnItemsChangeded(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (Panes == null) return;
            try
            {
                var ee = e as System.Collections.Specialized.NotifyCollectionChangedEventArgs;
                if (ee == null) return;
                if (ee.OldItems == null) return;
                if (ee.OldItems.Count < 1) return;
                if (ee.Action == NotifyCollectionChangedAction.Remove)
                {
                    //if (
                    //    !Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions.ContainsRegionWithName(
                    //        DocumentRegionName))
                    //    return;

                    //var regionRegion =
                    //    Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions[DocumentRegionName];


                    //foreach (var t in Wlst.Cr.Core.CoreServices.RegionManage.RegionManagerInstances.Regions)
                    //{
                    //    foreach (var f in ee.OldItems)
                    //    {
                    //        if (t.Views.Contains(f))
                    //        {
                    //            t.Remove(f);
                    //        }
                    //    }
                    //}


                    foreach (var f in ee.OldItems)
                    {
                        if (Panes.RadPaneGroupOverride.Items.Contains(f))
                        {
                            this.Panes.RadPaneGroupOverride.Items.Remove(f);
                            this.OnDeletedUserControl(this, new DeleteUserControlArgs() {DeleteUserControl = f});
                        }


                        foreach (var fff in Panes.RadPaneGroupOverride.EnumeratePanes())
                        {
                            if (fff == f)
                            {
                                this.Panes.RadPaneGroupOverride.Items.Remove(fff); //.RemovePane(fff);
                                this.OnDeletedUserControl(this, new DeleteUserControlArgs() { DeleteUserControl = f });
                                break;
                            }
                            var ff = fff.Content;
                            if (ff != null && f == ff)
                            {
                                this.Panes.RadPaneGroupOverride.Items.Remove(fff); //.RemovePane(fff); // = t;
                                this.OnDeletedUserControl(this, new DeleteUserControlArgs() { DeleteUserControl = f });
                                break;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private void AddValueToRadPaneGroup(object item,bool show)
        {
            var lst = new List<object>();
            foreach (var t in Panes.EnumeratePanes())
            {
                lst.Add(t);
                var ff = t.Content;
                if (ff != null)
                    lst.Add(ff);

            }

            if (lst.Contains(item))
            {
                Panes.RadPaneGroupOverride.SelectedItem = item ;
                if(show )
                {
                    this.Visibility = Visibility.Visible;
                    this.Show();
                    this.Focus();
                }
                return;
            }



            var radPane = item as RadPane;

            if (radPane != null)
            {

                radPane.IsHidden = false;
                if (radPane.Parent != null)
                {
                    radPane.RemoveFromParent();
                }
                Panes.RadPaneGroupOverride.Items.Add(item);
                return;
            }

            var userControl = item as UserControl;
            if (userControl != null)
            {
                try
                {
                    var radDocumnetPane = new RadDocumentPane();


                    radDocumnetPane.IsHidden = false;
                    var parent = userControl.Parent;
                    if (parent != null)
                    {

                        parent.SetValue(ContentPresenter.ContentProperty, null);
                    }

                    var mvvm = userControl.DataContext as IITab;
                    if (mvvm != null)
                    {
                        radDocumnetPane.Header = mvvm.Title;
                        radDocumnetPane.CanUserClose = mvvm.CanClose;
                        radDocumnetPane.CanDockInDocumentHost = mvvm.CanDockInDocumentHost;
                        radDocumnetPane.CanUserPin = mvvm.CanUserPin;
                        radDocumnetPane.CanFloat = mvvm.CanFloat;
                    }
                    else
                    {
                        radDocumnetPane.Header = userControl.Name;
                        radDocumnetPane.CanUserClose = true;
                        radDocumnetPane.CanDockInDocumentHost = true;
                        radDocumnetPane.CanUserPin = true;
                        radDocumnetPane.CanFloat = true;
                    }
                    radDocumnetPane.Content = item;


                    Panes.RadPaneGroupOverride.Items.Add(radDocumnetPane);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (show)
                {
                    this.Visibility = Visibility.Visible;
                    this.Show();
                    this.Focus();
                }

            }
        }




        /// <summary>
        /// 从_radPaneGroup中删除指定view
        /// </summary>
        /// <param name="value"></param>
        public void RemovePanFromParent(object value)
        {
            try
            {
                var lst = new List<Tuple<RadPane, object>>();
                foreach (var t in Panes.RadPaneGroupOverride.EnumeratePanes())
                {
                    var ff = t.Content;
                    if (ff != null)
                    {
                        lst.Add(new Tuple<RadPane, object>(t, ff));
                    }
                    else
                    {
                        lst.Add(new Tuple<RadPane, object>(t, 1));
                    }
                }
                foreach (var t in lst)
                {
                    if (t.Item1 == value || t.Item2 == value)
                    {
                        t.Item1.RemoveFromParent();
                    }
                }

                if (this.Panes.RadPaneGroupOverride.Items.Count == 0) this.Hide();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var dic = new List<object>();
                foreach (var t in Panes.RadPaneGroupOverride.Items) dic.Add(t);
                // var dic = (from t in Panes.Items select t).ToList();

                foreach (var t in dic)
                {
                    this.Panes.RadPaneGroupOverride.Items.Remove(t);
                    this.OnDeletedUserControl(this, new DeleteUserControlArgs() {DeleteUserControl = t});
                    if (this.Panes.RadPaneGroupOverride.Items.Count == 1) break;
                }
            }
            catch (Exception ex)
            {

            }
            e.Cancel = true;
            this.Hide();
            //base.OnClosing(e);
        }

    };

    public partial class DocumentWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _isTopMost;

        public bool IsTopMost
        {
            get { return _isTopMost; }
            set
            {
                if (value != _isTopMost)
                {
                    _isTopMost = value;
                    if (_isTopMost)
                    {
                        this.Topmost = true;
                    }
                    else
                    {
                        this.Topmost = false;
                    }
                    this.OnPropertyChanged("IsTopMost");
                }
            }
        }
    }

    public class DeleteUserControlArgs : EventArgs
    {
        public object DeleteUserControl;
    }
}
