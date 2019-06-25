using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using System.Collections.ObjectModel;
using Wlst.Ux.Wj2096Module.FieldInfoSet.ViewModel;
using System.Windows.Input;

namespace Wlst.Ux.Wj2096Module.FieldInfoSet.Services
{
    public interface IIConcentratorParaInformationViewModel : IINavOnLoad, IITab, IIOnHideOrClose
    {
        
        void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e);
        void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e);
        bool Is2096 { get; set; }
       // string ShowSndInfo { get; set; }
        //bool IsEnableCore { get; set; }
    }
}
