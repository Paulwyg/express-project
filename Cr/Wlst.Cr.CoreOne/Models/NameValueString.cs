using System;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Cr.CoreOne.Models
{
    [Serializable]
  public   class NameValueString:ObservableObject 
    {
        private string  _name;
        public string  Name
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



        private string  _value;
        public string  Value
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

    }
}
