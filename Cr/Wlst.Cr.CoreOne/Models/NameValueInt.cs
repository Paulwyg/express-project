using System;
using System.Windows;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Cr.CoreOne.Models
{
    [Serializable]
    public class NameValueInt : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private string _dtame;

        public string DtName
        {
            get { return _dtame; }
            set
            {
                if (_dtame != value)
                {
                    _dtame = value;
                    this.RaisePropertyChanged(() => this.DtName);
                }
            }
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.Value);
                }
            }
        }

        private int _value2;

        public int Value2
        {
            get { return _value2; }
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.Value2);
                }
            }
        }
    }



    public class NameIntBool : ObservableObject
    {


        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private int _areaid;

        public int AreaId
        {
            get { return _areaid; }
            set
            {
                if (_areaid != value)
                {
                    _areaid = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.Value);
                }
            }
        }

        /// <summary>
        /// 当IsSelected状态发生变化的时候出发  如果本函数被注册了
        /// </summary>
        public event EventHandler OnIsSelectedChanged;

        private bool _check;

        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnIsSelectedChanged != null) OnIsSelectedChanged(this, EventArgs.Empty);
                }
            }
        }

        private bool _isEnable;

        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    this.RaisePropertyChanged(() => this.IsEnable);
                }
            }
        }

        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged(() => this.IsShow);
                }
            }
        }

        //~NameIntBool()
        //{

        //}
    }


    public class IntStringBoolDoubleKey : ObservableObject
    {


        private string _name;

        public string ValueString
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.ValueString);
                }
            }
        }

        private int _areaid;

        public int Key
        {
            get { return _areaid; }
            set
            {
                if (_areaid != value)
                {
                    _areaid = value;
                    this.RaisePropertyChanged(() => this.Key);
                }
            }
        }

        private int _value;

        public int ValueInt
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.ValueInt);
                }
            }
        }


        private double _valuedouble;

        public double ValueDouble
        {
            get { return _valuedouble; }
            set
            {
                if (_valuedouble != value)
                {
                    _valuedouble = value;
                    this.RaisePropertyChanged(() => this.ValueDouble);
                }
            }
        }

        private bool _check;

        public bool ValueBool
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.ValueBool);
                }
            }
        }

        private bool _isEnable;

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    this.RaisePropertyChanged(() => this.IsEnable);
                }
            }
        }

        private Visibility _isShow;
        /// <summary>
        /// 是否显示
        /// </summary>
        public Visibility IsVisi
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged(() => this.IsVisi);
                }
            }
        }

    }
}
