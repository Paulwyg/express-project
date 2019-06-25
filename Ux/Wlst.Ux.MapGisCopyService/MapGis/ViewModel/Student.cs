using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ArcGISWpfApplicationFive_Selection.ViewModel
{
    public class Student : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                //this.PropertyChanged("Name");
            }
        }



        private int _age;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public int Age
        {
            get { return _age; }
            set
            {
                if (value == _age) return;
                _age = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Age"));
            }
        }

    }
}
