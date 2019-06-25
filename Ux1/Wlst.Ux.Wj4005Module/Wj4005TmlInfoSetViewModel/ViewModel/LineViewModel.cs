using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.ViewModel
{
    public class LineViewModel : ObservableObject 
    {
        public LineViewModel(double x1, double y1, double x2, double y2, int switchOutId, int switchInId)
        {
            X1onMap = x1;
            X2onMap = x2;
            Y1onMap = y1;
            Y2onMap = y2;
            if (switchInId > 0)
                SetSwitchInId(switchInId);
            else
                SetSwitchOutId(switchOutId);
        }

        public LineViewModel()
        {
        }

        private void SetSwitchOutId(int id)
        {
            switch (id )
            {
                case 1:
                    BackgroundColor = "#BA55D3";
                    break;
                case 2:
                    BackgroundColor = "#0000FF";
                    break;
                case 3:
                    BackgroundColor = "#00BFFF";
                    break;
                case 4:
                    BackgroundColor = "#7CFC00";
                    break;
                case 5:
                    BackgroundColor = "#CD5C5C";
                    break;
                case 6:
                    BackgroundColor = "#696969";
                    break;
                default :
                    BackgroundColor = "#FF0000";
                    break;
            }
        }

        private void SetSwitchInId(int id)
        {
            switch (id)
            {
                case 1:
                    BackgroundColor = "#BA55D3";
                    break;
                case 2:
                    BackgroundColor = "#0000FF";
                    break;
                case 3:
                    BackgroundColor = "#00BFFF";
                    break;
                case 4:
                    BackgroundColor = "#7CFC00";
                    break;
                case 5:
                    BackgroundColor = "#CD5C5C";
                    break;
                case 6:
                    BackgroundColor = "#696969";
                    break;
                case 7:
                    BackgroundColor = "#BA55D3";
                    break;
                case 8:
                    BackgroundColor = "#0000FF";
                    break;
                case 9:
                    BackgroundColor = "#00BFFF";
                    break;
                case 10:
                    BackgroundColor = "#7CFC00";
                    break;
                case 11:
                    BackgroundColor = "#CD5C5C";
                    break;
                case 12:
                    BackgroundColor = "#696969";
                    break;
                case 13:
                    BackgroundColor = "#BA55D3";
                    break;
                case 14:
                    BackgroundColor = "#0000FF";
                    break;
                case 15:
                    BackgroundColor = "#00BFFF";
                    break;
                case 16:
                    BackgroundColor = "#7CFC00";
                    break;
                default:
                    BackgroundColor = "#FF0000";
                    break;
            }
        }

        double _x1OnMap;

        public double X1onMap
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

        double _x2OnMap;

        public double X2onMap
        {
            get
            {
                return _x2OnMap;
            }
            set
            {
                if (_x2OnMap != value)
                {
                    _x2OnMap = value;
                    this.RaisePropertyChanged("X2onMap");
                }
            }
        }

        double _y1OnMap;

        public double Y1onMap
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

        double _y2OnMap;

        public double Y2onMap
        {
            get
            {
                return _y2OnMap;
            }
            set
            {
                if (_y2OnMap != value)
                {
                    _y2OnMap = value;
                    this.RaisePropertyChanged("Y2onMap");
                }
            }
        }

        private string _backgroundColor;

        /// <summary>
        /// 
        /// </summary>
        public string BackgroundColor
        {
            get
            {
                return _backgroundColor; //_backgroundColor;
            }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    this.RaisePropertyChanged(() => this.BackgroundColor);
                }
            }
        }


    }



}