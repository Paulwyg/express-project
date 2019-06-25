using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowForWlst;
using Wlst.client;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNewOne;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView;

namespace Wlst.Ux.StateBarModule.CommonSet.BaseView
{
    /// <summary>
    /// SetSystemRegion.xaml 的交互逻辑
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SetSystemRegion : CustomChromeWindow 
    {
        public SetSystemRegion()
        {
            InitializeComponent();

        }

        private ObservableCollection<RegionItem> dt;
        private SetSystemRegionStyle _setSystemRegionStyle = new SetSystemRegionStyle();
        public void SetContext(ObservableCollection<RegionItem> oit)
        {
            _setSystemRegionStyle.RegionItems = oit;


            DataContext = _setSystemRegionStyle;
        }

        public event EventHandler<EventArgsSetRegions> OnFormBtnOkClick;


        private bool _isViewActive = false;



        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _isViewActive = false;
            
            this.Visibility = Visibility.Collapsed;
            e.Cancel = true;
            base.OnClosing(e);
        }








        public class SetSystemRegionStyle : Wlst.Cr.Core.CoreServices.ObservableObject
        {

           // CurrentSelectRegionItem
           private RegionItem _currentSelectRegionItem;

           public RegionItem CurrentSelectRegionItem
           {
               get
               {
                   if (_currentSelectRegionItem == null) _currentSelectRegionItem = new RegionItem();
                   return _currentSelectRegionItem;
               }
               set
               {
                   if (_currentSelectRegionItem == value) return;
                   _currentSelectRegionItem = value;
                   this.RaisePropertyChanged(() => this.CurrentSelectRegionItem);
               }
           }



            private ObservableCollection<RegionItem> _regionItems;

            public ObservableCollection<RegionItem> RegionItems
            {
                get
                {
                    if (_regionItems == null) _regionItems = new ObservableCollection<RegionItem>();
                    return _regionItems;
                }
                set
                {
                    if (_regionItems == value) return;
                    _regionItems = value;
                    this.RaisePropertyChanged(() => this.RegionItems);
                }
            }
        }







    }

    /// <summary>
    /// method
    /// </summary>
    public partial class SetSystemRegion
    {



        private void AddRegion_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 0;
            index = _setSystemRegionStyle.RegionItems.Count + 1;
            _setSystemRegionStyle.RegionItems.Add(new RegionItem()
            {
                RegionId = index,
                RegionName = "新地区"
            });

        }




        private void Save_OnClick(object sender, RoutedEventArgs e)
        {


                if (OnFormBtnOkClick != null)
                {
                    OnFormBtnOkClick(this, new EventArgsSetRegions(_setSystemRegionStyle.RegionItems));
                }
                this.Close();
   
        }


        private void DelRegion_OnClick(object sender, RoutedEventArgs e)
        {
            if (_setSystemRegionStyle.CurrentSelectRegionItem == null)
            {
                UMessageBox.Show("提醒", "未选中地区", UMessageBoxButton.Ok);
                return;
            }

            _setSystemRegionStyle.RegionItems.Remove(_setSystemRegionStyle.CurrentSelectRegionItem);

        }
    }

    public class EventArgsSetRegions : EventArgs
    {
        public ObservableCollection<RegionItem> SetSystemRegion;

        public EventArgsSetRegions(ObservableCollection<RegionItem> info)
        {
            SetSystemRegion = info;
        }
    }


}
