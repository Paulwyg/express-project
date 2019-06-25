using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.Services;
using Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel;

namespace Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.Views
{
    /// <summary>
    /// Wj3005TmlInfoSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Nr6005Module .Services .ViewIdAssign .Wj3005TmlInfoSetViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj3005TmlInfoSetView : UserControl
    {
        public Wj3005TmlInfoSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IITmlInformationViewModel Model
        {
            get
            {
                return DataContext as IITmlInformationViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

  
   
        //private void AttachEquipmentModule_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var t = DragDropExtend.DragAndDrop.DropTargetInfomation.GetSelectItemByUiElement(sender, e);
        //    if (t != null)
        //    {
        //        var ff = t as AttachEquipmentModuleViewModel;
        //        if (ff != null)
        //        {
        //            Model.CurrentSelectAttachEquimentModule = ff;
        //            Model.CurrentSelectAttachEquiment = null;
        //            return;
        //        }
        //    }
        //    Model.CurrentSelectAttachEquimentModule = null;
        //    Model.CurrentSelectAttachEquiment = null;
        //}

        //private void CanvasTabAttach_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    Model.CurrentSelectAttachEquimentModule = null;
        //    Model.CurrentSelectAttachEquiment = null;
        //}

        //private void AttachEquipment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var t = DragDropExtend.DragAndDrop.DropTargetInfomation.GetSelectItemByUiElement(sender, e);
        //    if (t != null)
        //    {
        //        var ff = t as AttachEquipmentViewModel ;
        //        if (ff != null)
        //        {
        //            Model.CurrentSelectAttachEquiment = ff;
        //            Model.CurrentSelectAttachEquimentModule = null;
        //            return;
        //        }
        //    }
        //    Model.CurrentSelectAttachEquiment = null;
        //    Model.CurrentSelectAttachEquimentModule = null;
        //}

        //private void ItemsControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //}

        //private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{

        //}

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var sdr = sender as Button;
                if (sdr == null) return;
                var modulekey = Convert.ToInt32(sdr.ToolTip.ToString());
                if(modulekey >0)
                {
                    Model.AddModule(modulekey);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var sdr = sender as Button;
                if (sdr == null) return;
                var ids = Convert.ToInt32(sdr.ToolTip.ToString());
                if (ids > 0)
                {
                    Model.DeleteAttachInstances(ids);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var sd = sender as TextBlock;
            if (sd != null)
            {
                var xxx = (int)sd.ToolTip;
                Model.ResetCm(xxx);
            }
        }
    };
}