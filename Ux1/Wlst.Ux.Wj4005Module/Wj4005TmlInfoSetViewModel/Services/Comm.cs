using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.Services
{
    public class Comm
    {
        public static ObservableCollection<StateBase> CollectionContactorState
        {
            get
            {
                var CollectionReturn = new ObservableCollection<StateBase>
                {
                    new StateBase("无", -1),
                    new StateBase("常开", 0),
                    new StateBase("常闭", 1),
                    
                };
                return CollectionReturn;
            }
        }

        public static ObservableCollection<StateBase> CollectionJumpAlarm
        {
            get
            {
                var CollectionReturn = new ObservableCollection<StateBase>
                {
                    new StateBase("被动报警", 0),
                    new StateBase("主动报警", 1)
                };
                return CollectionReturn;
            }
        }

        public static ObservableCollection<StateBase> CollectionContactorStateShow
        {
            get
            {
                var CollectionReturn = new ObservableCollection<StateBase>
                {
                    new StateBase("断", 0),
                    new StateBase("通", 1),
                     new StateBase("被盗", 2),
                    new StateBase("正常", 3),
                    new StateBase("打开", 4),
                    new StateBase("关闭", 5),
                };
                return CollectionReturn;
            }
        }

        public static ObservableCollection<StateBase> CollectionPhase
        {
            get
            {
                var CollectionReturn = new ObservableCollection<StateBase>
                {
                    new StateBase("A相", 0),
                    new StateBase("B相", 1),
                    new StateBase("C相", 2),
                };
                return CollectionReturn;
            }
        }

        public static ObservableCollection<StateBase> CollectionSwitchIn16
        {
            get
            {
                var CollectionRetrun = new ObservableCollection<StateBase>
                {
                    new StateBase("无", 0),
                    new StateBase("开关量输入1", 1),
                    new StateBase("开关量输入2", 2),
                    new StateBase("开关量输入3", 3),
                    new StateBase("开关量输入4", 4),
                    new StateBase("开关量输入5", 5),
                    new StateBase("开关量输入6", 6),
                    new StateBase("开关量输入7", 7),
                    new StateBase("开关量输入8", 8),
                    new StateBase("开关量输入9", 9),
                    new StateBase("开关量输入10", 10),
                    new StateBase("开关量输入11", 11),
                    new StateBase("开关量输入12", 12),
                    new StateBase("开关量输入13", 13),
                    new StateBase("开关量输入14", 14),
                    new StateBase("开关量输入15", 15),
                    new StateBase("开关量输入16", 16),
                };
                return CollectionRetrun;
            }
        }

        public static ObservableCollection<StateBase> CollectionSwitchOut6
        {
            get
            {
                var CollectionRetrun = new ObservableCollection<StateBase>
                {
                    new StateBase("无", 0),
                    new StateBase("开关量输出K1", 1),
                    new StateBase("开关量输出K2", 2),
                    new StateBase("开关量输出K3", 3),
                    new StateBase("开关量输出K4", 4),
                    new StateBase("开关量输出K5", 5),
                    new StateBase("开关量输出K6", 6),
                };
                return CollectionRetrun;
            }
        }

        public static ObservableCollection<StateBase> CollectionVector36
        {
            get
            {
                var CollectionRetrun = new ObservableCollection<StateBase>
                {
                    new StateBase("无", 0),
                    new StateBase("采样1", 1),
                    new StateBase("采样2", 2),
                    new StateBase("采样3", 3),
                    new StateBase("采样4", 4),
                    new StateBase("采样5", 5),
                    new StateBase("采样6", 6),
                    new StateBase("采样7", 7),
                    new StateBase("采样8", 8),
                    new StateBase("采样9", 9),
                    new StateBase("采样10", 10),
                    new StateBase("采样11", 11),
                    new StateBase("采样12", 12),
                    new StateBase("采样13", 13),
                    new StateBase("采样14", 14),
                    new StateBase("采样15", 15),
                    new StateBase("采样16", 16),
                    new StateBase("采样17", 17),
                    new StateBase("采样18", 18),
                    new StateBase("采样19", 19),
                    new StateBase("采样20", 20),
                    new StateBase("采样21", 21),
                    new StateBase("采样22", 22),
                    new StateBase("采样23", 23),
                    new StateBase("采样24", 24),
                    new StateBase("采样25", 25),
                    new StateBase("采样26", 26),
                    new StateBase("采样27", 27),
                    new StateBase("采样28", 28),
                    new StateBase("采样29", 29),
                    new StateBase("采样30", 30),
                    new StateBase("采样31", 31),
                    new StateBase("采样32", 32),
                    new StateBase("采样33", 33),
                    new StateBase("采样34", 34),
                    new StateBase("采样35", 35),
                    new StateBase("采样36", 36),
                };
                return CollectionRetrun;
            }
        }
    };

    public class StateBase : ObservableObject
    {
        public StateBase(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                this.RaisePropertyChanged(() => this.Name);
            }
        }

        private int _Value;

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value == value)
                    return;
                _Value = value;
                this.RaisePropertyChanged(() => this.Value);
            }
        }
    };

    public class NameXy : ObservableObject
    {
        public NameXy(string name)
        {
            this.Name = name;
        }

        private int x1onMap;

        public int X1onMap
        {
            get
            {
                return 300;
            }
            set
            {
                if (x1onMap == value)
                    return;
                x1onMap = value;
                this.RaisePropertyChanged(() => this.X1onMap);
            }
        }

        private int y1onMap;

        public int Y1onMap
        {
            get
            {
                return 0;
            }//540; }
            set
            {
                if (y1onMap == value)
                    return;
                y1onMap = value;
                this.RaisePropertyChanged(() => this.Y1onMap);
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                this.RaisePropertyChanged(() => this.Name);
            }
        }
    };
}