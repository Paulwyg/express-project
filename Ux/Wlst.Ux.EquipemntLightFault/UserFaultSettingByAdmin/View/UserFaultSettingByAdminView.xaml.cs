using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.Services;

namespace Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.View
{
    /// <summary>
    /// UserFaultSettingByAdminView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(EquipemntLightFault.Services.ViewIdAssign.UserFaultSettingByAdminViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserFaultSettingByAdminView : UserControl
    {
        public UserFaultSettingByAdminView()
        {
            InitializeComponent();

            //GridViewDataColumn gd=new GridViewDataColumn() ;
            //var x = gd.IsVisible;

        }

        [Import]
        public IIUserFaultSettingByAdminVm Model
        {
            get { return DataContext as IIUserFaultSettingByAdminVm; }
            set
            {
                DataContext = value;
                value.OnChanged += new EventHandler(value_OnChanged);
            }
        }

        void value_OnChanged(object sender, EventArgs e)
        {
            return;
            if (sender == null) return;
            int r = 0;
            if (Int32.TryParse(sender.ToString(), out r))
            {
                gv.IsVisible = r > 0;
            }

            //throw new NotImplementedException();
        }
    }
}
