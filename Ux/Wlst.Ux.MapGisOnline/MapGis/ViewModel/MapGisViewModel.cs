using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ArcGISWpfApplicationFive_Selection.ViewModel;

using System.Reflection;

using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.Menu.Services;
using System.Windows;
using System.Globalization;
using System.Runtime.InteropServices;
using Wlst.Ux.MapGisOnline.MapGis.Services;
using Wlst.Ux.MapGisOnline.MapGis.ViewModel;
using Wlst.Ux.MapGisOnline.Resources;

namespace Wlst.Ux.MapGisOnline.MapGis.ViewModel
{
    [Export(typeof(IIMapGis))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MapGisViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIMapGis, Wlst.Cr.Core.CoreInterface.IITab
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region tab iinterface
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get
            {
                return "地图";
            }
        }

        public bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion
        #region IsBusy
        private bool _isBusy = true;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
                }
            }
        }
        #endregion

        #region IniExend
        //private string  _iniexend ;
        //Wlst.Ux.MapGis.MapGis.View.MapGis.INIClass iniC = new Wlst.Ux.MapGis.MapGis.View.MapGis.INIClass(@".\MapGis.ini");
        //public string  IniExend
        //{
        //    get
        //    {
        //        return _iniexend;
        //    }
        //    set
        //    {
        //        _iniexend =  iniC.IniReadValue("MapGis", "Exend");

        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("IniExend"));
        //        }
        //    }
        //}
        #endregion
        public void RaiseEvent()
        {
            this.RaisePropertyChanged(() => this.IsEditMapGisEnable);
        }

        // private bool _isBusy = true;

        public bool IsEditMapGisEnable { get; set; }


        #region 右键菜单  在节点被选中的时候显示刷新右键菜单


        public ContextMenu Cm
        {
            get
            {
                return _cm;
            }
            set
            {
                if (value == _cm) return;
                _cm = value;
                this.RaisePropertyChanged(() => this.Cm);
            }
        }


        int CurrentRtuIds;
        public int CurrentRtuId
        {
            get { return CurrentRtuIds; }
            set
            {
                // if (CurrentRtuIds == value) return;
                CurrentRtuIds = value;

                GetCm(value, 0);
                // Cm.Visibility = Visibility.Visible;
                this.RaisePropertyChanged(() => this.Cm);
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("Cm"));
                //}
            }
        }

        int CurrentCtrlIds;
        public int CurrentCtrlId
        {
            get { return CurrentCtrlIds; }
            set
            {
                // if (CurrentRtuIds == value) return;
                CurrentCtrlIds = value;

                GetCm(CurrentRtuIds, value);
                // Cm.Visibility = Visibility.Visible;
                this.RaisePropertyChanged(() => this.Cm);
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("Cm"));
                //}
            }
        }


        public void SetCmUnVisi()
        {
            //if (Cm != null) Cm.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// 菜单
        /// </summary>
        public ContextMenu GetCm(List<int> rtulst, List<int> slulst, Dictionary<int, List<int>> sluctrllst)
        {

            if (Cm == null)
            {
                Cm = new ContextMenu();
                Cm.BorderThickness = new Thickness(0);
            }
            Cm.Items.Clear();
            if (rtulst != null && rtulst.Count > 0)
            {
                var t = MenuBuilding.BulidCm("30500", false, rtulst);
                var gg = MenuBuilding.HelpCmm(t);

                foreach (var f in gg)
                {
                    Cm.Items.Add(f);
                }
            }

            if (slulst != null && slulst.Count > 0)
            {
                var t = MenuBuilding.BulidCm("20901", false, slulst);
                var gg = MenuBuilding.HelpCmm(t);

                foreach (var f in gg)
                {
                    Cm.Items.Add(f);
                }
            }

            if (sluctrllst != null && sluctrllst.Count > 0)
            {
                var t = MenuBuilding.BulidCm("20902", false, sluctrllst);
                var gg = MenuBuilding.HelpCmm(t);

                foreach (var f in gg)
                {
                    Cm.Items.Add(f);
                }
            }


            var mi = new MenuItem();
            mi.Header = "混合菜单";
            mi.IsEnabled = false;
            if (Cm.Items.Count > 0)
                Cm.Items.Insert(0, mi);
            else Cm.Items.Add(mi);
            return Cm;
        }


        /// <summary>
        /// 菜单
        /// </summary>
        public ContextMenu GetCm(int rtuId, int ctrlid = 0)
        {

            if (Cm == null)
            {
                Cm = new ContextMenu();
                Cm.BorderThickness = new Thickness(0);
            }
            Cm.Items.Clear();
            string rtuName = "" + rtuId;
            if (ctrlid < 1)
            {
                if (
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems. //).EquipmentInfoDictionary.
                        ContainsKey(rtuId))
                {
                    var s =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[ //.EquipmentInfoDictionary[
                            rtuId];
                    rtuName = s.RtuName;
                    var t = MenuBuilding.BulidCm(((int)s.RtuModel).ToString(), false, s);
                    //(s.RtuModel.ToString(CultureInfo.InvariantCulture), false, s);
                    HelpCmm(t);
                }
                //else if (
                //    Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(rtuId) != null)
                //{
                //    var s =
                //        Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(rtuId);
                //    rtuName = s.FieldName;
                //    var t = MenuBuilding.BulidCm("20960", false, s.FieldId);

                //    HelpCmm(t);
                //}
                else if (rtuId < 1600000000 && rtuId > 1500000000)
                {
                    int sluId = Convert.ToInt32(rtuId / 1000);
                    int CtrlId = rtuId % 1000;
                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems. //).EquipmentInfoDictionary.
                            ContainsKey(sluId))
                    {
                        var s =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            //.EquipmentInfoDictionary[
                                sluId];
                        rtuName = s.RtuName + "--" + CtrlId;
                        var t = MenuBuilding.BulidCm("20900", false, new Tuple<int, int>(sluId, CtrlId));
                        HelpCmm(t);
                    }
                }
            }
            else    //nb单灯
            {
                var sluinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(rtuId,ctrlid);
                if (sluinfo != null) 
                {                    
                    rtuName = ctrlid + "--" + sluinfo.CtrlName  ;
                    var t = MenuBuilding.BulidCm("20962", false, new Tuple<int, int>(rtuId, ctrlid));
                    HelpCmm(t);
                }
            }

            var mi = new MenuItem();
            mi.Header = rtuName;
            mi.IsEnabled = false;
            if (Cm.Items.Count > 0)
                Cm.Items.Insert(0, mi);
            else Cm.Items.Add(mi);
            return Cm;
        }

        private ContextMenu _cm = null;



        private void HelpCmm(ObservableCollection<IIMenuItem> t)
        {
            // ContextMenu Cm = new ContextMenu();
            // Cm.Items.Clear();
            var gg = MenuBuilding.HelpCmm(t);
            foreach (var f in gg)
            {
                Cm.Items.Add(f);
            }
            return;
            foreach (var f in t)
            {
                if (f.CmItems != null && f.CmItems.Count > 0)
                {
                    var ggg = helpCmmm(f);
                    ggg.Header = f.Text;
                    Cm.Items.Add(ggg);
                }
                else
                {
                    MenuItem mii = new MenuItem();
                    mii.Header = f.Text;
                    mii.IsCheckable = f.IsCheckable;
                    mii.IsEnabled = f.IsEnabled;
                    mii.ToolTip = f.Tooltips;
                    mii.Command = f.Command;

                    Cm.Items.Add(mii);
                }
            }
            return;
        }

        private MenuItem helpCmmm(IIMenuItem g)
        {
            MenuItem mi = new MenuItem();
            foreach (var f in g.CmItems)
            {
                if (f.CmItems != null && f.CmItems.Count > 0)
                {
                    var ggg = helpCmmm(f);
                    ggg.Header = f.Text;
                    mi.Items.Add(ggg);
                }
                else
                {
                    var mii = new MenuItem();
                    mii.Header = f.Text;
                    mii.IsCheckable = f.IsCheckable;
                    mii.IsEnabled = f.IsEnabled;
                    mii.ToolTip = f.Tooltips;
                    mii.Command = f.Command;

                    mi.Items.Add(mii);
                }
            }
            return mi;
        }



        #endregion



        #region SymbolsGrapic
        private object _imagesIcon;

        /// <summary>
        /// 前台界面绑定的图标
        /// </summary>
        public object ImagesIcon
        {
            get { return _imagesIcon; }
            set
            {
                if (_imagesIcon != value)
                {
                    _imagesIcon = value;
                    this.RaisePropertyChanged(() => this.ImagesIcon);
                }
            }
        }


        private int _equipmentImageState;

        public int EquipmentImageState
        {
            get { return _equipmentImageState; }
            set
            {
                if (_equipmentImageState != value)
                {
                    _equipmentImageState = value;
                    ImagesIcon = ImageResources.GetEquipmentIcon(_equipmentImageState);// ImageResource.GetEquipmentIcon(_equipmentImageState);
                    //this.RaisePropertyChanged(() => this.ImagesIcon);
                }
            }
        }



        #endregion

        //public ObservableCollection<FeatureData> SelectedFeaturesData;



        //private bool GetPrivilege()
        //{
        //    ////int userLevel = PrivilegsList.MySelf.GetPrivileg(Wlst.Ux.MapGis.Services.MenuIdAssign.MapEditId); //rwx
        //    ////bool excute = (userLevel & 1) == 1;
        //    ////bool wirte = ((userLevel >> 1) & 1) == 1;
        //    ////bool read = ((userLevel >> 2) & 1) == 1;

        //    ////if (excute || wirte) //具有操作权限
        //    ////{
        //    ////    return true;
        //    ////}
        //    ////return false;
        //}


    }



}
