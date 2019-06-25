using System;
using System.ComponentModel.Composition;
using System.Windows;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.AddMainEquipment.Services;

namespace Wlst.Ux.AddMainEquipment.Views
{
    /// <summary>
    /// AddMainEquipmentView2.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.AddMainEquipmentViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AddMainEquipmentView
    {
        public AddMainEquipmentView()
        {
            InitializeComponent();
            gtx.Visibility = Visibility.Collapsed;
        }

        [Import]
        public IIAddMainEquipment Model
        {
            get { return DataContext as IIAddMainEquipment; }
            set
            {
                value.SetButton(gtx);
                DataContext = value; 
            }
        }

        private void gtx_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            gtx.Visibility = Visibility.Collapsed;
            count = 0;

            try
            {
                int x = Convert.ToInt32(tbphy.Text);

                //lvf nb单灯删除 2018年5月28日13:57:37

                if (x>1700000 && x<1800000)
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.DeleteEquipment(x);

                    return; 
                }



                if (x > 0 &&
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                        x))
                {

                    if (WlstMessageBox.Show("确认不可恢复删除",
                                            "删除设备，逻辑地址为:" + x + ",删除将不可恢复，请确认...", WlstMessageBoxType.YesNo) ==
                        WlstMessageBoxResults.Yes)
                    {
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.DeleteEquipment(x);

                    }

                }
                else
                {
                    WlstMessageBox.Show("设备不存在",
                                        "您想删除的设备" + x + "不存在,请确认...");
                }
            }
            catch (Exception ex)
            {

            }
        }


        private int count = 0;

        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            count++;
            if (count >= 5)
            {
                gtx.Visibility = Visibility.Visible;
            }
        }


        private long dt = 0;

        private void tbphy_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dt == 0) dt = DateTime.Now.Ticks;
            if (DateTime.Now.Ticks - dt < 100000000)
                count++;
            else
            {
                count = 0;
                dt = 0;
            }

            if (count >= 5)
            {
                gtx.Visibility = Visibility.Visible;
            }
        }

        //private void RadTreeListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    return;
        //}
    }
}
