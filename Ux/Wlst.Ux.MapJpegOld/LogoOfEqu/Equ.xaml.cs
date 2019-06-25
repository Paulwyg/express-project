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
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.RadMapJpeg.LogoOfEqu
{
    /// <summary>
    /// Equ.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = true, ID = RadMapJpeg.Services.ViewIdAssign.EquViewId,
        AttachRegion = RadMapJpeg.Services.ViewIdAssign.EquViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Equ : UserControl
    {
        public Equ()
        {
            InitializeComponent();
            DataContext = new EquViewModel();
        }
    };


    public class EquViewModel
    {

        public const int Jd601 = 1103004;
        public const int Mru1050 = 1102502;
        public const int Lux1080 = 1102602;


        private bool IsExistValue(int id)
        {
            return  Wlst.Cr.Core.ComponentHold.ViewComponentHolding.ContainsComponent(id);
            //if (sss.Contains(id)) return true;
            //return false;
        }

        #region CmdJd601

        private void ExUpdateEsuTime()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(Jd601,
                                                                            RegionNames.
                                                                                DocumentRegion, 1);
        }

        private bool CanUpdateEsuTime()
        {
            return IsExistValue(Jd601);
        }

        private ICommand _UpdateEsuTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdJd601
        {
            get { return _UpdateEsuTime ?? (_UpdateEsuTime = new RelayCommand(ExUpdateEsuTime, CanUpdateEsuTime, false)); }
        }

        #endregion

        #region CmdMru1050

        private void ExCmdOpenEsu()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(Mru1050,
                                                                            RegionNames.
                                                                                DocumentRegion, 1);
        }

        private bool CanCmdOpenEsu()
        {
            return IsExistValue(Mru1050);
        }

        private ICommand _CmdOpenEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdMru1050
        {
            get { return _CmdOpenEsu ?? (_CmdOpenEsu = new RelayCommand(ExCmdOpenEsu, CanCmdOpenEsu, false)); }
        }

        #endregion

        #region CmdLux1080

        private void ExCmdCloseEsu()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(Lux1080,
                                                                            RegionNames.
                                                                                DocumentRegion, 1);
        }

        private bool CanCmdCloseEsu()
        {
            return IsExistValue(Lux1080);
        }

        private ICommand _CmdCloseEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdLux1080
        {
            get { return _CmdCloseEsu ?? (_CmdCloseEsu = new RelayCommand(ExCmdCloseEsu, CanCmdCloseEsu, true)); }
        }

        #endregion

    }
}
