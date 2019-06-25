using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.ViewModel;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel;


namespace Wlst.Ux.Wj3005ExNewDataExcelModule.RapidSetRtuAmp
{
    public class NavToRapidSetRtuAmp
    {



        public static RapidSetRtuAmp rapidSetRtuAmp = null;

        public static void InitWin(int index, int rtuid, ObservableCollection<LoopInfoLeft> data)
        {
            if (rapidSetRtuAmp == null)
                rapidSetRtuAmp = new RapidSetRtuAmp();

            rapidSetRtuAmp.Visibility = Visibility.Visible;
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid)) return;
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];
            if (info == null) return;

            var tmp = info as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            if (tmp == null) return;
            var loops = (from t in tmp.WjLoops where t.Value.SwitchOutputId > 0 select t.Value.LoopId).ToList();
            var da = (from t in data where loops.Contains(t.LoopId) select t).ToList();
            var sdfsdfsd = new ObservableCollection<LoopInfoLeft>();
            foreach (var f in da) sdfsdfsd.Add(f);


            rapidSetRtuAmp.Title = info.RtuPhyId.ToString("d4") + " - " + info.RtuName;
          
            rapidSetRtuAmp.Show();
            rapidSetRtuAmp.Focus();  
            rapidSetRtuAmp.InitWin(index ,rtuid ,sdfsdfsd );

        }

        public static void InitWin(int index, int rtuid, ObservableCollection<LoopInfox> data)
        {
            if (rapidSetRtuAmp == null)
                rapidSetRtuAmp = new RapidSetRtuAmp();

            rapidSetRtuAmp.Visibility = Visibility.Visible;
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid)) return;
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];
            if (info == null) return;
            var tmp=info as   Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            if (tmp == null) return;
            var loops = (from t in tmp.WjLoops where t.Value.SwitchOutputId > 0 select t.Value.LoopId).ToList();
            var da = (from t in data where loops.Contains(t.LoopId) select t).ToList();
            var sdfsdfsd = new ObservableCollection<LoopInfox>();
            foreach (var f in da) sdfsdfsd.Add(f);

            rapidSetRtuAmp.Title = info.RtuPhyId.ToString("d4" ) + " - " + info.RtuName;
         
            rapidSetRtuAmp.Show();
            rapidSetRtuAmp.Focus();   
            rapidSetRtuAmp.InitWin(index,rtuid, sdfsdfsd);

        }

    }
}

