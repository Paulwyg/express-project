using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Ux.StateBarModule.UserOperateMsgView.ViewModel;

namespace Wlst.Ux.StateBarModule.UserOperateMsgView.Services
{
   public  interface IIUserOperatorRecords : Wlst.Cr.Core.CoreInterface.IITab 
    {
        ObservableCollection<OperateItem> Records { get; }
        void CurrentSelectItemDoubleClicked();
    }
}
