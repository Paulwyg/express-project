using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Wlst.Ux.StateBarModule.DataAreaController.ViewModel
{
    public class UserControlObject : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public string  ViewId;


        public object uc;

        public object Uc
        {
            get { return uc; }
            set
            {
                if (uc != value)
                {
                    uc = value;
                    this.RaisePropertyChanged(() => this.Uc);
                }
            }
        }


        public Visibility vuc;

        public Visibility Vuc
        {
            get { return vuc; }
            set
            {
                if (vuc != value)
                {
                    vuc = value;
                    this.RaisePropertyChanged(() => this.Vuc);
                }
            }
        }
    }
}
