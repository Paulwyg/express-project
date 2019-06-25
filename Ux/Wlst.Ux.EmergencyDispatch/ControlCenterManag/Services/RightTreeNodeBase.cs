using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services
{
    public partial class RightTreeNodeBase : TreeNodeBaseViewModel
    {

        private static readonly List<int> DeleteNodeIds=new List<int>();
        public RightTreeNodeBase()
        {
            ;
        }
        
        public List<int> GetNeedToBeDeletedNodeId()
        {
            return DeleteNodeIds;
        }

        public virtual void AddChild()
        {
            ;
        }

        #region IsChecked

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if(IsChecked==value) return;
                _isChecked = value;
                if(IsGroup)
                {
                    foreach (var item in ChildTreeItems)
                    {
                        item.IsChecked = IsChecked;
                    }
                }
                if(_isChecked && !DeleteNodeIds.Contains(NodeId) && !IsGroup)
                {
                    DeleteNodeIds.Add(NodeId);
                }
                RaisePropertyChanged(()=>IsChecked);
            }
        }
        #endregion

        #region IsGroup
        private bool _isGroup;

        public bool IsGroup
        {
            get { return _isGroup; }
            set
            {
                _isGroup = value;
                RaisePropertyChanged(() => IsGroup);
            }
        }
        #endregion

        #region Father
        /// <summary>
        /// 父节点
        /// </summary>
        protected RightTreeNodeBase _father;

        public RightTreeNodeBase Father
        {
            get { return _father; }
            set { _father = value; }
        }
        #endregion

        #region ChildTreeItems

        private ObservableCollection<RightTreeNodeBase> _childTreeItems;
        public ObservableCollection<RightTreeNodeBase>  ChildTreeItems
        {
            get { return _childTreeItems ?? (_childTreeItems = new ObservableCollection<RightTreeNodeBase>()); }
        }

        #endregion
    }
    public partial class RightTreeNodeBase
    {
        #region 状态
        #region State

        private string _state = "--";
        public string State
        {
            get { return _state; }
            set
            {
                if(_state==value) return;
                _state = value;
                RaisePropertyChanged(()=>State);
            }
        }
        #endregion

        #region ChangeState

        private bool _changeState;
        public bool ChangeState
        {
            get { return _changeState; }
            set
            {
                if (Switch1 || Switch2 || Switch3 || Switch4 || Switch5 || Switch6)
                {
                    State = value ? "开灯" : "关灯";
                }
                if(_changeState==value)return;
                _changeState = value;
                AnsOpenSwitch1 = "--";
                AnsOpenSwitch2 = "--";
                AnsOpenSwitch3 = "--";
                AnsOpenSwitch4 = "--";
                AnsOpenSwitch5 = "--";
                AnsOpenSwitch6 = "--";
                RaisePropertyChanged(()=>ChangeState);
            }
        }

        #endregion

        #endregion

        #region 对时

        private string _syncTime = "--";
        public string SyncTime
        {
            get { return _syncTime; }
            set
            {
                if(_syncTime==value) return;
                _syncTime = value;
                RaisePropertyChanged(()=>SyncTime);
            }
        }

        #endregion

        #region 发送周设置

        private string _sndWeekSet="--";
        public string SndWeekSet
        {
            get { return _sndWeekSet; }
            set
            {
                if(_sndWeekSet==value) return;
                _sndWeekSet = value;
                RaisePropertyChanged(()=>SndWeekSet);
            }
        }

        #endregion

        #region K0
        #region Switch0

        private bool _switch0;
        public bool Switch0
        {
            get { return _switch0; }
            set
            {
                if (_switch0 == value) return;
                _switch0 = value;
                Switch1 = Switch0;
                Switch2 = Switch0;
                Switch3 = Switch0;
                Switch4 = Switch0;
                Switch5 = Switch0;
                Switch6 = Switch0;
                if(IsGroup)
                {
                    foreach (var item in ChildTreeItems)
                    {
                        item.Switch0 = _switch0;
                    }
                    if(Father !=null && !(from item in Father.ChildTreeItems where item.Switch0 select item.NodeId).Any())
                    {
                        Father.Switch0 = false;
                    }
                }
                else
                {
                    if (!(from item in Father.ChildTreeItems where item.Switch0 select item.NodeId).Any())
                    {
                        Father.Switch0 = false;
                    }
                }
                RaisePropertyChanged(() => Switch0);
            }
        }

        #endregion
        #endregion

        #region K1

        #region Switch1

        private bool _switch1;
        public bool Switch1
        {
            get { return _switch1; }
            set
            {
                if(_switch1==value) return;
                _switch1 = value;
                if(IsGroup)
                {
                    foreach (var child in ChildTreeItems)
                    {
                        child.Switch1 = _switch1;
                    }
                    if (Father != null && !(from item in Father.ChildTreeItems where item.Switch1 select item.NodeId).Any())
                    {
                        Father.Switch1 = false;
                    }
                }
                else
                {
                    if(!(from item in Father.ChildTreeItems where item.Switch1 select item.NodeId).Any())
                    {
                        Father.Switch1 = false;
                    }
                }
                RaisePropertyChanged(()=>Switch1);
            }
        }

        #endregion

        #region AnsOpenSwitch1

        private string _ansOpenSwitch1 = "--";
        public string AnsOpenSwitch1
        {
            get { return _ansOpenSwitch1; }
            set
            {
                if(_ansOpenSwitch1==value) return;
                _ansOpenSwitch1 = value;
                RaisePropertyChanged(()=>AnsOpenSwitch1);
            }
        }

        #endregion

        #region OpenSwitch1Visi

        private Visibility _openSwitch1Visi=Visibility.Collapsed;
        public Visibility OpenSwitch1Visi
        {
            get { return _openSwitch1Visi; }
            set
            {
                if(_openSwitch1Visi==value) return;
                _openSwitch1Visi = value;
                RaisePropertyChanged(()=>OpenSwitch1Visi);
            }
        }

        #endregion

        #region AnsCloseSwitch1
        private string _ansCloseSwitch1 = "--";
        public string AnsCloseSwitch1
        {
            get { return _ansCloseSwitch1; }
            set
            {
                if (_ansCloseSwitch1 == value) return;
                _ansCloseSwitch1 = value;
                RaisePropertyChanged(() => AnsCloseSwitch1);
            }
        }
        #endregion

        #region CloseSwitch1Visi
        private Visibility _closeSwitch1Visi=Visibility.Collapsed;
        public Visibility CloseSwitch1Visi
        {
            get { return _closeSwitch1Visi; }
            set
            {
                if (_closeSwitch1Visi == value) return;
                _closeSwitch1Visi = value;
                RaisePropertyChanged(() => CloseSwitch1Visi);
            }
        }
        #endregion

        #region AnsPatrol1
        private string _ansPatrol1 = "--";
        public string AnsPatrol1
        {
            get { return _ansPatrol1; }
            set
            {
                if (_ansPatrol1 == value) return;
                _ansPatrol1 = value;
                RaisePropertyChanged(() => AnsPatrol1);
            }
        }
        #endregion

        #region Patrol1Visi
        private Visibility _patrol1Visi = Visibility.Collapsed;
        public Visibility Patrol1Visi
        {
            get { return _patrol1Visi; }
            set
            {
                if (_patrol1Visi == value) return;
                _patrol1Visi = value;
                RaisePropertyChanged(() => Patrol1Visi);
            }
        }
        #endregion

        #region K1Foreground

        private string _k1Foreground = Colors.Green.ToString();
        public string K1Foreground
        {
            get { return _k1Foreground; }
            set
            {
                if(_k1Foreground==value)return;
                _k1Foreground = value;
                RaisePropertyChanged(()=>K1Foreground);
            }
        }
        #endregion

        #endregion

        #region K2

        #region Switch2

        private bool _switch2;
        public bool Switch2
        {
            get { return _switch2; }
            set
            {
                if (_switch2 == value) return;
                _switch2 = value;
                if (IsGroup)
                {
                    foreach (var child in ChildTreeItems)
                    {
                        child.Switch2 = _switch2;
                    }
                    if (Father != null && !(from item in Father.ChildTreeItems where item.Switch2 select item.NodeId).Any())
                    {
                        Father.Switch2 = false;
                    }
                }
                else
                {
                    if (!(from item in Father.ChildTreeItems where item.Switch2 select item.NodeId).Any())
                    {
                        Father.Switch2 = false;
                    }
                }
                RaisePropertyChanged(() => Switch2);
            }
        }

        #endregion

        #region AnsOpenSwitch2

        private string _ansOpenSwitch2 = "--";
        public string AnsOpenSwitch2
        {
            get { return _ansOpenSwitch2; }
            set
            {
                if (_ansOpenSwitch2 == value) return;
                _ansOpenSwitch2 = value;
                RaisePropertyChanged(() => AnsOpenSwitch2);
            }
        }

        #endregion

        #region OpenSwitch2Visi

        private Visibility _openSwitch2Visi = Visibility.Collapsed;
        public Visibility OpenSwitch2Visi
        {
            get { return _openSwitch2Visi; }
            set
            {
                if (_openSwitch2Visi == value) return;
                _openSwitch2Visi = value;
                RaisePropertyChanged(() => OpenSwitch2Visi);
            }
        }

        #endregion

        #region AnsCloseSwitch2
        private string _ansCloseSwitch2 = "--";
        public string AnsCloseSwitch2
        {
            get { return _ansCloseSwitch2; }
            set
            {
                if (_ansCloseSwitch2 == value) return;
                _ansCloseSwitch2 = value;
                RaisePropertyChanged(() => AnsCloseSwitch2);
            }
        }
        #endregion

        #region CloseSwitch2Visi
        private Visibility _closeSwitch2Visi = Visibility.Collapsed;
        public Visibility CloseSwitch2Visi
        {
            get { return _closeSwitch2Visi; }
            set
            {
                if (_closeSwitch2Visi == value) return;
                _closeSwitch2Visi = value;
                RaisePropertyChanged(() => CloseSwitch2Visi);
            }
        }
        #endregion

        #region AnsPatrol2
        private string _ansPatrol2 = "--";
        public string AnsPatrol2
        {
            get { return _ansPatrol2; }
            set
            {
                if (_ansPatrol2 == value) return;
                _ansPatrol2 = value;
                RaisePropertyChanged(() => AnsPatrol2);
            }
        }
        #endregion

        #region Patrol2Visi
        private Visibility _patrol2Visi = Visibility.Collapsed;
        public Visibility Patrol2Visi
        {
            get { return _patrol2Visi; }
            set
            {
                if (_patrol2Visi == value) return;
                _patrol2Visi = value;
                RaisePropertyChanged(() => Patrol2Visi);
            }
        }
        #endregion

        #region K2Foreground

        private string _k2Foreground = Colors.Green.ToString();
        public string K2Foreground
        {
            get { return _k2Foreground; }
            set
            {
                if (_k2Foreground == value) return;
                _k2Foreground = value;
                RaisePropertyChanged(() => K2Foreground);
            }
        }
        #endregion

        #endregion

        #region K3

        #region Switch3

        private bool _switch3;
        public bool Switch3
        {
            get { return _switch3; }
            set
            {
                if (_switch3 == value) return;
                _switch3 = value;
                if (IsGroup)
                {
                    foreach (var child in ChildTreeItems)
                    {
                        child.Switch3 = _switch3;
                    }
                    if (Father != null && !(from item in Father.ChildTreeItems where item.Switch3 select item.NodeId).Any())
                    {
                        Father.Switch3 = false;
                    }
                }
                else
                {
                    if (!(from item in Father.ChildTreeItems where item.Switch3 select item.NodeId).Any())
                    {
                        Father.Switch3 = false;
                    }
                }
                RaisePropertyChanged(() => Switch3);
            }
        }

        #endregion

        #region AnsOpenSwitch3

        private string _ansOpenSwitch3 = "--";
        public string AnsOpenSwitch3
        {
            get { return _ansOpenSwitch3; }
            set
            {
                if (_ansOpenSwitch3 == value) return;
                _ansOpenSwitch3 = value;
                RaisePropertyChanged(() => AnsOpenSwitch3);
            }
        }

        #endregion

        #region OpenSwitch3Visi

        private Visibility _openSwitch3Visi = Visibility.Collapsed;
        public Visibility OpenSwitch3Visi
        {
            get { return _openSwitch3Visi; }
            set
            {
                if (_openSwitch3Visi == value) return;
                _openSwitch3Visi = value;
                RaisePropertyChanged(() => OpenSwitch3Visi);
            }
        }

        #endregion

        #region AnsCloseSwitch3
        private string _ansCloseSwitch3 = "--";
        public string AnsCloseSwitch3
        {
            get { return _ansCloseSwitch3; }
            set
            {
                if (_ansCloseSwitch3 == value) return;
                _ansCloseSwitch3 = value;
                RaisePropertyChanged(() => AnsCloseSwitch3);
            }
        }
        #endregion

        #region CloseSwitch3Visi
        private Visibility _closeSwitch3Visi = Visibility.Collapsed;
        public Visibility CloseSwitch3Visi
        {
            get { return _closeSwitch3Visi; }
            set
            {
                if (_closeSwitch3Visi == value) return;
                _closeSwitch3Visi = value;
                RaisePropertyChanged(() => CloseSwitch3Visi);
            }
        }
        #endregion

        #region AnsPatrol3
        private string _ansPatrol3 = "--";
        public string AnsPatrol3
        {
            get { return _ansPatrol3; }
            set
            {
                if (_ansPatrol3 == value) return;
                _ansPatrol3 = value;
                RaisePropertyChanged(() => AnsPatrol3);
            }
        }
        #endregion

        #region Patrol3Visi
        private Visibility _patrol3Visi = Visibility.Collapsed;
        public Visibility Patrol3Visi
        {
            get { return _patrol3Visi; }
            set
            {
                if (_patrol3Visi == value) return;
                _patrol3Visi = value;
                RaisePropertyChanged(() => Patrol3Visi);
            }
        }
        #endregion

        #region K3Foreground

        private string _k3Foreground = Colors.Green.ToString();
        public string K3Foreground
        {
            get { return _k3Foreground; }
            set
            {
                if (_k3Foreground == value) return;
                _k3Foreground = value;
                RaisePropertyChanged(() => K3Foreground);
            }
        }
        #endregion

        #endregion

        #region K4

        #region Switch4

        private bool _switch4;
        public bool Switch4
        {
            get { return _switch4; }
            set
            {
                if (_switch4 == value) return;
                _switch4 = value;
                if (IsGroup)
                {
                    foreach (var child in ChildTreeItems)
                    {
                        child.Switch4 = _switch4;
                    }
                    if (Father != null && !(from item in Father.ChildTreeItems where item.Switch4 select item.NodeId).Any())
                    {
                        Father.Switch4 = false;
                    }
                }
                else
                {
                    if (!(from item in Father.ChildTreeItems where item.Switch4 select item.NodeId).Any())
                    {
                        Father.Switch4 = false;
                    }
                }
                RaisePropertyChanged(() => Switch4);
            }
        }

        #endregion

        #region AnsOpenSwitch4

        private string _ansOpenSwitch4 = "--";
        public string AnsOpenSwitch4
        {
            get { return _ansOpenSwitch4; }
            set
            {
                if (_ansOpenSwitch4 == value) return;
                _ansOpenSwitch4 = value;
                RaisePropertyChanged(() => AnsOpenSwitch4);
            }
        }

        #endregion

        #region OpenSwitch4Visi

        private Visibility _openSwitch4Visi = Visibility.Collapsed;
        public Visibility OpenSwitch4Visi
        {
            get { return _openSwitch4Visi; }
            set
            {
                if (_openSwitch4Visi == value) return;
                _openSwitch4Visi = value;
                RaisePropertyChanged(() => OpenSwitch4Visi);
            }
        }

        #endregion

        #region AnsCloseSwitch4
        private string _ansCloseSwitch4 = "--";
        public string AnsCloseSwitch4
        {
            get { return _ansCloseSwitch4; }
            set
            {
                if (_ansCloseSwitch4 == value) return;
                _ansCloseSwitch4 = value;
                RaisePropertyChanged(() => AnsCloseSwitch4);
            }
        }
        #endregion

        #region CloseSwitch4Visi
        private Visibility _closeSwitch4Visi = Visibility.Collapsed;
        public Visibility CloseSwitch4Visi
        {
            get { return _closeSwitch4Visi; }
            set
            {
                if (_closeSwitch4Visi == value) return;
                _closeSwitch4Visi = value;
                RaisePropertyChanged(() => CloseSwitch4Visi);
            }
        }
        #endregion

        #region AnsPatrol4
        private string _ansPatrol4 = "--";
        public string AnsPatrol4
        {
            get { return _ansPatrol4; }
            set
            {
                if (_ansPatrol4 == value) return;
                _ansPatrol4 = value;
                RaisePropertyChanged(() => AnsPatrol4);
            }
        }
        #endregion

        #region Patrol4Visi
        private Visibility _patrol4Visi = Visibility.Collapsed;
        public Visibility Patrol4Visi
        {
            get { return _patrol4Visi; }
            set
            {
                if (_patrol4Visi == value) return;
                _patrol4Visi = value;
                RaisePropertyChanged(() => Patrol4Visi);
            }
        }
        #endregion

        #region K4Foreground

        private string _k4Foreground = Colors.Green.ToString();
        public string K4Foreground
        {
            get { return _k4Foreground; }
            set
            {
                if (_k4Foreground == value) return;
                _k4Foreground = value;
                RaisePropertyChanged(() => K4Foreground);
            }
        }
        #endregion

        #endregion

        #region K5

        #region Switch5

        private bool _switch5;
        public bool Switch5
        {
            get { return _switch5; }
            set
            {
                if (_switch5 == value) return;
                _switch5 = value;
                if (IsGroup)
                {
                    foreach (var child in ChildTreeItems)
                    {
                        child.Switch5 = _switch5;
                    }
                    if (Father != null && !(from item in Father.ChildTreeItems where item.Switch5 select item.NodeId).Any())
                    {
                        Father.Switch5 = false;
                    }
                }
                else
                {
                    if (!(from item in Father.ChildTreeItems where item.Switch5 select item.NodeId).Any())
                    {
                        Father.Switch5 = false;
                    }
                }
                RaisePropertyChanged(() => Switch5);
            }
        }

        #endregion

        #region AnsOpenSwitch5

        private string _ansOpenSwitch5 = "--";
        public string AnsOpenSwitch5
        {
            get { return _ansOpenSwitch5; }
            set
            {
                if (_ansOpenSwitch5 == value) return;
                _ansOpenSwitch5 = value;
                RaisePropertyChanged(() => AnsOpenSwitch5);
            }
        }

        #endregion

        #region OpenSwitch5Visi

        private Visibility _openSwitch5Visi = Visibility.Collapsed;
        public Visibility OpenSwitch5Visi
        {
            get { return _openSwitch5Visi; }
            set
            {
                if (_openSwitch5Visi == value) return;
                _openSwitch5Visi = value;
                RaisePropertyChanged(() => OpenSwitch5Visi);
            }
        }

        #endregion

        #region AnsCloseSwitch5
        private string _ansCloseSwitch5 = "--";
        public string AnsCloseSwitch5
        {
            get { return _ansCloseSwitch5; }
            set
            {
                if (_ansCloseSwitch5 == value) return;
                _ansCloseSwitch5 = value;
                RaisePropertyChanged(() => AnsCloseSwitch5);
            }
        }
        #endregion

        #region CloseSwitch5Visi
        private Visibility _closeSwitch5Visi = Visibility.Collapsed;
        public Visibility CloseSwitch5Visi
        {
            get { return _closeSwitch5Visi; }
            set
            {
                if (_closeSwitch5Visi == value) return;
                _closeSwitch5Visi = value;
                RaisePropertyChanged(() => CloseSwitch5Visi);
            }
        }
        #endregion

        #region AnsPatrol5
        private string _ansPatrol5 = "--";
        public string AnsPatrol5
        {
            get { return _ansPatrol5; }
            set
            {
                if (_ansPatrol5 == value) return;
                _ansPatrol5 = value;
                RaisePropertyChanged(() => AnsPatrol5);
            }
        }
        #endregion

        #region Patrol5Visi
        private Visibility _patrol5Visi = Visibility.Collapsed;
        public Visibility Patrol5Visi
        {
            get { return _patrol5Visi; }
            set
            {
                if (_patrol5Visi == value) return;
                _patrol5Visi = value;
                RaisePropertyChanged(() => Patrol5Visi);
            }
        }
        #endregion

        #region K5Foreground

        private string _k5Foreground = Colors.Green.ToString();
        public string K5Foreground
        {
            get { return _k5Foreground; }
            set
            {
                if (_k5Foreground == value) return;
                _k5Foreground = value;
                RaisePropertyChanged(() => K5Foreground);
            }
        }
        #endregion

        #endregion

        #region K6

        #region Switch6

        private bool _switch6;
        public bool Switch6
        {
            get { return _switch6; }
            set
            {
                if (_switch6 == value) return;
                _switch6 = value;
                if (IsGroup)
                {
                    foreach (var child in ChildTreeItems)
                    {
                        child.Switch6 = _switch6;
                    }
                    if (Father != null && !(from item in Father.ChildTreeItems where item.Switch6 select item.NodeId).Any())
                    {
                        Father.Switch6 = false;
                    }
                }
                else
                {
                    if (!(from item in Father.ChildTreeItems where item.Switch6 select item.NodeId).Any())
                    {
                        Father.Switch6 = false;
                    }
                }
                RaisePropertyChanged(() => Switch6);
            }
        }

        #endregion

        #region AnsOpenSwitch6

        private string _ansOpenSwitch6 = "--";
        public string AnsOpenSwitch6
        {
            get { return _ansOpenSwitch6; }
            set
            {
                if (_ansOpenSwitch6 == value) return;
                _ansOpenSwitch6 = value;
                RaisePropertyChanged(() => AnsOpenSwitch6);
            }
        }

        #endregion

        #region OpenSwitch6Visi

        private Visibility _openSwitch6Visi = Visibility.Collapsed;
        public Visibility OpenSwitch6Visi
        {
            get { return _openSwitch6Visi; }
            set
            {
                if (_openSwitch6Visi == value) return;
                _openSwitch6Visi = value;
                RaisePropertyChanged(() => OpenSwitch6Visi);
            }
        }

        #endregion

        #region AnsCloseSwitch6
        private string _ansCloseSwitch6 = "--";
        public string AnsCloseSwitch6
        {
            get { return _ansCloseSwitch6; }
            set
            {
                if (_ansCloseSwitch6 == value) return;
                _ansCloseSwitch6 = value;
                RaisePropertyChanged(() => AnsCloseSwitch6);
            }
        }
        #endregion

        #region CloseSwitch6Visi
        private Visibility _closeSwitch6Visi = Visibility.Collapsed;
        public Visibility CloseSwitch6Visi
        {
            get { return _closeSwitch6Visi; }
            set
            {
                if (_closeSwitch6Visi == value) return;
                _closeSwitch6Visi = value;
                RaisePropertyChanged(() => CloseSwitch6Visi);
            }
        }
        #endregion

        #region AnsPatrol6
        private string _ansPatrol6 = "--";
        public string AnsPatrol6
        {
            get { return _ansPatrol6; }
            set
            {
                if (_ansPatrol6 == value) return;
                _ansPatrol6 = value;
                RaisePropertyChanged(() => AnsPatrol6);
            }
        }
        #endregion

        #region Patrol6Visi
        private Visibility _patrol6Visi = Visibility.Collapsed;
        public Visibility Patrol6Visi
        {
            get { return _patrol6Visi; }
            set
            {
                if (_patrol6Visi == value) return;
                _patrol6Visi = value;
                RaisePropertyChanged(() => Patrol6Visi);
            }
        }
        #endregion

        #region K6Foreground

        private string _k6Foreground = Colors.Green.ToString();
        public string K6Foreground
        {
            get { return _k6Foreground; }
            set
            {
                if (_k6Foreground == value) return;
                _k6Foreground = value;
                RaisePropertyChanged(() => K6Foreground);
            }
        }
        #endregion

        #endregion


    }
}
