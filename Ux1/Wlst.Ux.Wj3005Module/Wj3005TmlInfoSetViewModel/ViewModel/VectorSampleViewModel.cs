using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.ViewModel
{
    public class VectorSampleViewModel : ObservableObject 
    {
        /// <summary>
        /// 初始化显示矢量
        /// </summary>
        /// <param name="vectorId">序号</param>
        /// <param name="loopId">矢量绑定的回路地址</param>
        public VectorSampleViewModel(int vectorId, int loopId)
        {
            this.VectorId = vectorId;
            this.LoopId = -1;
            this.LoopId = loopId;
        }

        private int _id;

        /// <summary>
        /// 矢量地址 1到16
        /// </summary>
        public int VectorId
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.VectorId);
                    CalocatePoint();
                }
            }
        }

        private int _loopId;

        /// <summary>
        /// 此矢量绑定的回路地址
        /// </summary>
        public int LoopId
        {
            get
            {
                return _loopId;
            }
            set
            {
                if (_loopId != value)
                {
                    _loopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                    if (_loopId > 0)
                        BackgroundColor = "#FFEBCD";
                    else
                        BackgroundColor = "#00C0D1";
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
                return _backgroundColor;
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

        #region point

        private void CalocatePoint()
        {
            if (VectorId < 13)
            {
                X1onMap = 710;
                Y1onMap = (this.VectorId - 1) * 35 + 70;
            }
            else if (VectorId < 25)
            {
                X1onMap = 770;
                Y1onMap = (this.VectorId - 13) * 35 + 70;
            }
            else if (VectorId < 37)
            {
                X1onMap = 830;
                Y1onMap = (this.VectorId - 25) * 35 + 70;
            }
            else if (VectorId < 49)
            {
                X1onMap = 890;
                Y1onMap = (this.VectorId - 37) * 35 + 70;
            }
            //Y1onMap = this.VectorId*50 + 30;
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
                return 45;
            }
        }

        //int _heightControl;
        public int HeightControl
        {
            get
            {
                return 25;
            }
        }
        
        #endregion

    }
}