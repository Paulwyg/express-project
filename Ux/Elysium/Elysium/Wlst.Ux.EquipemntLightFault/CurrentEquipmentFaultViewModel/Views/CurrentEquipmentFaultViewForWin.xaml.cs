using System;
using System.Collections.Generic;
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
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views
{
    /// <summary>
    /// CurrentEquipmentFaultViewForWin.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentEquipmentFaultViewForWin : Window
    {
        private CurrentEquipmentFaultViewModel.ViewModel.CurrentEquipmentFaultViewModel Mdl;
        // private IICurrentEquipmentFaultView Model;
        public CurrentEquipmentFaultViewForWin()
        {
            InitializeComponent();

            Mdl = new CurrentEquipmentFaultViewModel.ViewModel.CurrentEquipmentFaultViewModel();
            Mdl.OnRuleChanged += new EventHandler<EventArgsRule>(Mdl_OnRuleChanged);

            this.DataContext = Mdl;
            MdlOnRuleChanged(Mdl.GetCurrentSelected());
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        private void Mdl_OnRuleChanged(object sender, EventArgsRule e)
        {
            //throw new NotImplementedException();
            var x = (from t in e.LstSixHave where t.Value select t.Key).ToList();
            MdlOnRuleChanged(x);
            return;
            if (x.Count == 0)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(6, 0, 3, 0, 2);
            }
            if (x.Count == 1)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 3, 0, 1);
                SetGrd(6, 0, 3, 1, 1);
            }
            if (x.Count == 2)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 1, 0, 1);
                SetGrd(x[1], 1, 2, 0, 1);
                SetGrd(6, 0, 3, 1, 1);
            }
            if (x.Count == 3)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 1, 0, 1);
                SetGrd(x[1], 0, 1, 1, 1);
                SetGrd(x[2], 1, 2, 0, 1);
                SetGrd(6, 1, 2, 1, 1);
            }
            if (x.Count == 4)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 1, 0, 1);
                SetGrd(x[1], 0, 1, 1, 1);
                SetGrd(x[2], 1, 1, 0, 1);
                SetGrd(x[3], 2, 1, 0, 1);
                SetGrd(6, 1, 2, 1, 1);
            }
            if (x.Count == 5)
            {
                SetGrd(1, 0, 1, 0, 1);
                SetGrd(2, 0, 1, 1, 1);
                SetGrd(3, 1, 1, 0, 1);
                SetGrd(4, 1, 1, 1, 1);
                SetGrd(5, 2, 1, 0, 1);
                SetGrd(6, 2, 1, 1, 1);
            }


        }


        private void MdlOnRuleChanged(List<int> x)
        {
            //throw new NotImplementedException();
            // var x = (from t in e.LstSixHave where t.Value select t.Key).ToList();
            if (x.Count == 0)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(6, 0, 3, 0, 2);
            }
            if (x.Count == 1)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 3, 0, 1);
                SetGrd(6, 0, 3, 1, 1);
            }
            if (x.Count == 2)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 1, 0, 1);
                SetGrd(x[1], 1, 2, 0, 1);
                SetGrd(6, 0, 3, 1, 1);
            }
            if (x.Count == 3)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 1, 0, 1);
                SetGrd(x[1], 0, 1, 1, 1);
                SetGrd(x[2], 1, 2, 0, 1);
                SetGrd(6, 1, 2, 1, 1);
            }
            if (x.Count == 4)
            {
                for (int i = 1; i < 6; i++) SetGrd(i, false);
                SetGrd(x[0], 0, 1, 0, 1);
                SetGrd(x[1], 0, 1, 1, 1);
                SetGrd(x[2], 1, 1, 0, 1);
                SetGrd(x[3], 2, 1, 0, 1);
                SetGrd(6, 1, 2, 1, 1);
            }
            if (x.Count == 5)
            {
                SetGrd(1, 0, 1, 0, 1);
                SetGrd(2, 0, 1, 1, 1);
                SetGrd(3, 1, 1, 0, 1);
                SetGrd(4, 1, 1, 1, 1);
                SetGrd(5, 2, 1, 0, 1);
                SetGrd(6, 2, 1, 1, 1);
            }


        }

        private void SetGrd(int index, bool visi)
        {
            if (index == 1) g1.Visibility = visi ? Visibility.Visible : Visibility.Collapsed;
            if (index == 2) g2.Visibility = visi ? Visibility.Visible : Visibility.Collapsed;
            if (index == 3) g3.Visibility = visi ? Visibility.Visible : Visibility.Collapsed;
            if (index == 4) g4.Visibility = visi ? Visibility.Visible : Visibility.Collapsed;
            if (index == 5) g5.Visibility = visi ? Visibility.Visible : Visibility.Collapsed;
            // if (index == 1) g1.Visibility = visi ? Visibility.Visible : Visibility.Collapsed;

        }

        private void SetGrd(int id, int stRow, int spRow, int stColum, int spColum)
        {
            if (id == 1)
            {
                g1.Visibility = Visibility.Visible;
                g1.SetValue(Grid.RowProperty, stRow);
                g1.SetValue(Grid.RowSpanProperty, spRow);
                g1.SetValue(Grid.ColumnProperty, stColum);
                g1.SetValue(Grid.ColumnSpanProperty, spColum);
            }
            if (id == 2)
            {
                g2.Visibility = Visibility.Visible;
                g2.SetValue(Grid.RowProperty, stRow);
                g2.SetValue(Grid.RowSpanProperty, spRow);
                g2.SetValue(Grid.ColumnProperty, stColum);
                g2.SetValue(Grid.ColumnSpanProperty, spColum);
            }
            if (id == 3)
            {
                g3.Visibility = Visibility.Visible;
                g3.SetValue(Grid.RowProperty, stRow);
                g3.SetValue(Grid.RowSpanProperty, spRow);
                g3.SetValue(Grid.ColumnProperty, stColum);
                g3.SetValue(Grid.ColumnSpanProperty, spColum);
            }
            if (id == 4)
            {
                g4.Visibility = Visibility.Visible;
                g4.SetValue(Grid.RowProperty, stRow);
                g4.SetValue(Grid.RowSpanProperty, spRow);
                g4.SetValue(Grid.ColumnProperty, stColum);
                g4.SetValue(Grid.ColumnSpanProperty, spColum);
            }
            if (id == 5)
            {
                g5.Visibility = Visibility.Visible;
                g5.SetValue(Grid.RowProperty, stRow);
                g5.SetValue(Grid.RowSpanProperty, spRow);
                g5.SetValue(Grid.ColumnProperty, stColum);
                g5.SetValue(Grid.ColumnSpanProperty, spColum);
            }
            if (id == 6)
            {
                g6.Visibility = Visibility.Visible;
                g6.SetValue(Grid.RowProperty, stRow);
                g6.SetValue(Grid.RowSpanProperty, spRow);
                g6.SetValue(Grid.ColumnProperty, stColum);
                g6.SetValue(Grid.ColumnSpanProperty, spColum);
            }
        }

        //[Import]
        //public IICurrentEquipmentFaultView Model
        //{
        //    get { return DataContext as IICurrentEquipmentFaultView; }
        //    set { DataContext = value; }
        //}
        protected override void OnClosed(EventArgs e)
        {
            this.Hide();

            //base.OnClosed(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            //base.OnClosing(e);
        }

        private FaultSelectFm frm = null;

        private void treeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as Telerik.Windows.Controls.RadGridView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            var mvvm = ggg.Item as CurrentItemViewModel;
            if (mvvm == null) return;

            var cellIndex = ggg.Column.DisplayIndex - 2;
            var nodeId = mvvm.Id;

            if (mvvm.IsEnable == false) return;



            if (frm == null)
            {
                frm = new FaultSelectFm();
                frm.OnFormBtnOkClick += new EventHandler<EventArgsAddUpdate>(frm_OnFormBtnOkClick);
                frm.Visibility = Visibility.Visible;
            }
            else
            {
                frm.Visibility = Visibility.Visible;
            }



            var s3 = new List<int>();
            foreach (var f in Mdl.FaultRules)
            {
                if (f.Id == nodeId)
                {
                    foreach (var g in f.SelectedFaults)
                    {
                        if (g.Key == cellIndex) continue;
                        s3.AddRange(g.Value);
                    }
                }
            }

            frm.OnLoad(nodeId, cellIndex, mvvm.SelectedFaults[cellIndex], s3);

        }

        private void frm_OnFormBtnOkClick(object sender, EventArgsAddUpdate e)
        {
            //throw new NotImplementedException();
            foreach (var f in Mdl.FaultRules)
            {
                if (f.Id == e.Id)
                {
                    if (f.SelectedFaults.ContainsKey(e.FaultIndex))
                    {
                        f.SelectedFaults[e.FaultIndex] = e.FaultsSelected;
                        f.Names[e.FaultIndex - 1].Name = e.Name;
                    }

                }
            }

           
        }

        private   void RadGridView_MouseDoubleClick  (object sender, MouseButtonEventArgs e)
            {
                var listView = sender as Telerik.Windows.Controls.RadGridView;
                if (listView == null) return;
                var ggg = listView.CurrentCellInfo;
                var mvvm = ggg.Item as FaultRecordViewModel;
                if (mvvm == null) return;
                // if(e.LeftButton  ==MouseButtonState.Pressed )

                Mdl.OnSelectedFaults(mvvm, e.LeftButton == MouseButtonState.Pressed);
            }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;
            this.Topmost = cb.IsChecked==true ;
        }

        }
    
}
