using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.CoreInterface;
 
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.ViewModel
{
    public class AttachEquipmentViewModel : ObservableObject
    {
        public AttachEquipmentViewModel(int attachEquipmentId, string attachEquipmentName, int index, object attachEquipmentInstance)
        {
            this.AttachEquipmentId = attachEquipmentId;
            this.AttachEquipmentName = attachEquipmentName;
            this.Index = index;
            this.AttachEquipmentInstance = attachEquipmentInstance;

            var info =
             Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( attachEquipmentId);
            if (info == null) return;
            if ((int ) info.RtuModel == 30910)
            {
                CanBeDelete = false;
            }
        }



        private int _attachEquipmentId;

        public int AttachEquipmentId
        {
            get
            {
                return _attachEquipmentId;
            }
            set
            {
                if (_attachEquipmentId == value)
                    return;
                _attachEquipmentId = value;
                this.RaisePropertyChanged(() => this.AttachEquipmentId);
            }
        }


        private bool canBeDelete=true ;
        public bool CanBeDelete
        {
            get
            {
                return canBeDelete;
            }
            set
            {
                if (canBeDelete == value)return;
                canBeDelete = value;
                this.RaisePropertyChanged(() => this.CanBeDelete);
            }
        }
        private string _attachEquipmentName;

        public string AttachEquipmentName
        {
            get
            {
                return _attachEquipmentName;
            }
            set
            {
                if (_attachEquipmentName == value)
                    return;
                _attachEquipmentName = value;
                this.RaisePropertyChanged(() => this.AttachEquipmentName);
            }
        }

        private int _index;

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (_index == value)
                    return;
                _index = value;
                this.RaisePropertyChanged(() => this.Index);
                CalocatePoint();
            }
        }

        public object AttachEquipmentInstance;

        private ContextMenu _cm;

        /// <summary>
        /// 菜单
        /// </summary>
        public ContextMenu Cm
        {
            get
            {
                var t = ResetCm();
                _cm = new ContextMenu();
                if (t == null)
                    return null;

                foreach (var tt in t)
                {
                    //if (tt.Text.Contains("设置"))
                    //{
                        var f = new MenuItem();
                        f.Header = tt.Text;
                        f.IsEnabled = tt.IsEnabled;
                        f.Command = tt.Command;
                        _cm.Items.Add(f);
                   // }
                }
                return _cm;
            }
        }

        public ObservableCollection<IIMenuItem> ResetCm()
        {
            //return base.ResetCm();
            if (AttachEquipmentInstance == null)
                return null;
            var att = AttachEquipmentInstance as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (att == null)
                return null;
            ObservableCollection<IIMenuItem> t = null;
            t =MenuBuilding.BulidCm(((int )att.RtuModel).ToString(CultureInfo.InvariantCulture), false, AttachEquipmentInstance);
            return t;
        }

        #region point

        public string BackgroundColor
        {
            get
            {
                return "#FFB0E0E6";
            }
        }

        private void CalocatePoint()
        {
            if (Index < 9)
            {
                X1onMap = 260;
                Y1onMap = (this.Index - 1) * 45 + 25;
            }
            else if (Index < 18)
            {
                X1onMap = 480;
                Y1onMap = (this.Index - 9) * 45 + 25;
            }
            else if (Index < 27)
            {
                X1onMap = 700;
                Y1onMap = (this.Index - 18) * 45 + 25;
            }
        }

        private int _x1OnMap;

        public int X1onMap
        {
            get
            {
                return _x1OnMap;
            }
            set
            {
                if (_x1OnMap != value)
                {
                    _x1OnMap = value;
                    this.RaisePropertyChanged("X1onMap");
                }
            }
        }

        private int _y1OnMap;

        public int Y1onMap
        {
            get
            {
                return _y1OnMap;
            }
            set
            {
                if (_y1OnMap != value)
                {
                    _y1OnMap = value;
                    this.RaisePropertyChanged("Y1onMap");
                }
            }
        }

        //int _widthControl;
        public int WidthControl
        {
            get
            {
                return 120;
            }
        }

        //int _heightControl;
        public int HeightControl
        {
            get
            {
                return 30;
            }
        }

        public int HeightDes
        {
            get
            {
                return 45;
            }
        }
        
        #endregion

    }
}