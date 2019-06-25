using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.ViewModel
{
    public class AttachEquipmentModuleViewModel : ObservableObject
    {
        public AttachEquipmentModuleViewModel(int modulekey, string moduleName, int index)
        {
            this.ModuleKey = modulekey;
            this.MouduleName = moduleName;
            this.Index = index;
        }

        private int _modulekey;

        public int ModuleKey
        {
            get
            {
                return _modulekey;
            }
            set
            {
                if (_modulekey == value)
                    return;
                _modulekey = value;
                this.RaisePropertyChanged(() => this.ModuleKey);
            }
        }

        private string _moduleName;

        public string MouduleName
        {
            get
            {
                return _moduleName;
            }
            set
            {
                if (_moduleName == value)
                    return;
                _moduleName = value;
                this.RaisePropertyChanged(() => this.MouduleName);
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
            X1onMap = 20;
            Y1onMap = (this.Index - 1) * 45 + 20;
        }

        int _x1OnMap;

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

        int _y1OnMap;

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