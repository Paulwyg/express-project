using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{
    public class IdNameDesc : ObservableObject
    {
        private int _Id;

        /// <summary>
        /// 光控Id
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }


        private string _luxNsdfame;

        public string NameDesc
        {
            get { return _luxNsdfame; }
            set
            {
                if (_luxNsdfame != value)
                {
                    _luxNsdfame = value;
                    RaisePropertyChanged(() => NameDesc);
                }
            }
        }

        private ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> _mainruleitems;

        public ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> MainRuleItems
        {
            get
            {
                //    if (_mainruleitems == null)
                //    {
                //        _mainruleitems = new ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle>();
                //    }
                return _mainruleitems;
            }
            set
            {
                if (_mainruleitems != value)
                {
                    _mainruleitems = value;
                    this.RaisePropertyChanged(() => this.MainRuleItems);
                }
            }

        }


        private ObservableCollection<bool> _mainisoverone;
        public ObservableCollection<bool> MainIsOverOne
        {
            get
            {
                return _mainisoverone;
            }
            set
            {
                if (_mainisoverone != value)
                {
                    _mainisoverone = value;
                    this.RaisePropertyChanged(() => this.MainIsOverOne);
                }
            }
        }

        //private ObservableCollection<Visibility> _mainisovertwo;
        //public ObservableCollection<Visibility> MainIsOverTwo
        //{
        //    get
        //    {
        //        return _mainisovertwo;
        //    }
        //    set
        //    {
        //        if (_mainisovertwo != value)
        //        {
        //            _mainisovertwo = value;
        //            this.RaisePropertyChanged(() => this.MainIsOverTwo);
        //        }
        //    }
        //}
        private ObservableCollection<int> _maintype;
        public ObservableCollection<int> MainType
        {
            get
            {
                return _maintype;
            }
            set
            {
                if (_maintype != value)
                {
                    _maintype = value;
                    this.RaisePropertyChanged(() => this.MainType);
                }
            }
        }

        //private string _mainscrollbar;
        //public string MainScrollBar
        //{
        //    get
        //    {
        //        return _mainscrollbar;
        //    }
        //    set
        //    {
        //        if (_mainscrollbar != value)
        //        {
        //            _mainscrollbar = value;
        //            this.RaisePropertyChanged(() => this.MainScrollBar);
        //        }
        //    }
        //}

        private string _idname;

        public string IdName
        {
            get { return _idname; }
            set
            {
                if (_idname != value)
                {
                    _idname = value;
                    RaisePropertyChanged(() => IdName);
                }
            }
        }



    }
}
