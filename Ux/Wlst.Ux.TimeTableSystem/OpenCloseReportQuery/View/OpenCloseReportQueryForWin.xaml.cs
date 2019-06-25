using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.Services;
using Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.ViewModel;

namespace Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.View
{
    /// <summary>
    /// OpenCloseReportQueryForWin.xaml 的交互逻辑
    /// </summary>
    public partial class OpenCloseReportQueryForWin : Window
    {
        public OpenCloseReportQueryForWin()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private ObservableCollection<OpenCloseReportItem> dt;
        private OpenCloseReportQueryForWinStyle openCloseReportQueryForWin = new OpenCloseReportQueryForWinStyle();


        public void SetContext(ObservableCollection<OpenCloseReportItem> oit)
        {
            openCloseReportQueryForWin.Records = oit;

            if (openCloseReportQueryForWin.Records.Count > 0) 
                openCloseReportQueryForWin.CurrentSelectItem = openCloseReportQueryForWin.Records.First();

            DataContext = openCloseReportQueryForWin;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public class OpenCloseReportQueryForWinStyle : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private OpenCloseReportItem _rCurrentSelectItem;

            public OpenCloseReportItem CurrentSelectItem
            {
                get { return _rCurrentSelectItem; }
                set
                {
                    if (value != _rCurrentSelectItem)
                    {
                        _rCurrentSelectItem = value;
                        this.RaisePropertyChanged(() => this.CurrentSelectItem);
                        if (_rCurrentSelectItem == null) return;

                        RtuOpenCloseItems.Clear();
                        foreach (var t in _rCurrentSelectItem.Records) RtuOpenCloseItems.Add(t);
                    }
                    if (value == null) btnExEnable = false;
                    else if (value.Records.Count == 0) btnExEnable = false;
                    else btnExEnable = true;
                }
            }

            private bool _btnExEnable;

            public bool btnExEnable
            {
                get { return _btnExEnable; }
                set
                {
                    if (_btnExEnable == value) return;
                    _btnExEnable = value;
                    this.RaisePropertyChanged(() => this.btnExEnable);
                }
            }

            private ObservableCollection<OpenCloseReportItem> _records;

            public ObservableCollection<OpenCloseReportItem> Records
            {
                get
                {
                    if (_records == null) _records = new ObservableCollection<OpenCloseReportItem>();
                    return _records;
                }
                set
                {
                    if (_records == value) return;
                    _records = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }

            private ObservableCollection<OpenCloseReportRtuItem> _recordsss;


            public ObservableCollection<OpenCloseReportRtuItem> RtuOpenCloseItems
            {
                get
                {
                    if (_recordsss == null)
                    {
                        _recordsss = new ObservableCollection<OpenCloseReportRtuItem>();
                    }
                    return _recordsss;
                }
                set
                {
                    if (value != _recordsss)
                    {
                        _recordsss = value;
                        this.RaisePropertyChanged(() => this.RtuOpenCloseItems);
                    }
                }
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sdr = sender as Telerik.Windows.Controls.RadGridView;
            if (sdr == null) return;
            var item = sdr.SelectedItem as OpenCloseReportRtuItem;
            if (item == null) return;
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(item.RtuId);
            EventPublish.PublishEvent(args);
        }
    }
}
