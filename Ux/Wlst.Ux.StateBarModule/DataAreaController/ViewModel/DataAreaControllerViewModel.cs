using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Ux.StateBarModule.DataAreaController.Services;

namespace Wlst.Ux.StateBarModule.DataAreaController.ViewModel
{

    [Export(typeof (IIDataAreaController))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DataAreaControllerViewModel :Wlst .Cr .Core .CoreServices .ObservableObject , IIDataAreaController, IPartImportsSatisfiedNotification
    {

        public DataAreaControllerViewModel()
        {
            Wlst.Cr.CoreMims.Services.ShowNewDataServices.OnUserWantShowNewDataView +=
                new EventHandler<Cr.CoreMims.Services.ShowNewDataEventArgs>(
                    ShowNewDataServices_OnUserWantShowNewDataView);


            Wlst.Cr.CoreMims.Services.ShowNewDataServices.OnUserWantCloseNewDataView +=
             new EventHandler<Cr.CoreMims.Services.ShowNewDataEventArgs>(
                 ShowNewDataServices_OnUserWantCloseNewDataView);
        }

        void ShowNewDataServices_OnUserWantShowNewDataView(object sender, Cr.CoreMims.Services.ShowNewDataEventArgs e)
        {
            //throw new NotImplementedException();
            string  vid = e.ViewId+"";
            bool find = false;
            foreach (var g in this.Items)
            {
                if (g.ViewId == vid)
                {
                    find = true;
                    g.Vuc = Visibility.Visible;
                }
                else
                {
                    g.Vuc = Visibility.Collapsed;
                }
            }
            if (find == false)
            {
                txt = "无法找到该设备的数据驱动页面";
            }
            else
            {
                txt = "";
            }
        }


        void ShowNewDataServices_OnUserWantCloseNewDataView(object sender, Cr.CoreMims.Services.ShowNewDataEventArgs e)
        {
            //throw new NotImplementedException();
            string vid = e.ViewId + "";
            bool find = false;
            foreach (var g in this.Items)
            {
                if (g.ViewId == vid)
                {
                    //find = true;
                    g.Vuc = Visibility.Collapsed ;
                }
                //else
                //{
                //    g.Vuc = Visibility.Collapsed;
                //}
            }
            //if (find == false)
            //{
            //    txt = "无法找到该设备的数据驱动页面";
            //}
            //else
            {
                txt = "";
            }
        }

        private ObservableCollection<UserControlObject> _lineItemss;

        public ObservableCollection<UserControlObject> Items
        {
            get
            {
                if (_lineItemss == null) _lineItemss = new ObservableCollection<UserControlObject>();
                return _lineItemss;
            }
        }


        private string _txt;

        public string txt
        {
            get { return _txt; }
            set
            {
                if (value == _txt) return;
                _txt = value;
                this.RaisePropertyChanged(() => this.txt);
            }
        }

        public void OnImportsSatisfied()
        {
            this.OnImportsSatisfiedViews();
        }


        private void OnImportsSatisfiedViews()
        {
            try
            {
                foreach (var t in RegisteredViews)
                {
                    try
                    {
                        if (t.Metadata.AttachRegion == Wlst.Cr.CoreOne.Services.RegionNames.DataRegion)
                        {
                            if (t.Metadata.Id == Ux.StateBarModule.Services.ViewIdAssign.DataAreaControllerViewId+"")
                                continue;
                            var userControlMvvm = t.Value as ContentControl;
                            if (userControlMvvm != null)
                            {
                                var mvvm = userControlMvvm.DataContext as Wlst.Cr.CoreMims.CoreInterface.IIShowData;
                                //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                                if (mvvm != null)
                                {
                                    //mvvm.NavOnLoad(args.Item3);
                                    //bolNavOnLoad = true;
                                    bool find = false;
                                    foreach (var g in this.Items)
                                    {
                                        if (g.ViewId == t.Metadata.Id )
                                        {
                                            find = true;
                                            break;
                                        }
                                    }
                                    if (find == false)
                                    {
                                        this.Items.Add(new UserControlObject()
                                                           {
                                                               ViewId = t.Metadata.Id,
                                                               Uc = t.Value,
                                                               Vuc = Visibility.Collapsed
                                                           });
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("最新数据控制器提取最新数据导出页面出错:" + ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core AutoPopulate happen an super error:" + ex.ToString());
            }

        }



        [ImportMany(AllowRecomposition = true)]
        public Lazy<object, IIViewExport>[] RegisteredViews { get; set; }
    }
}
