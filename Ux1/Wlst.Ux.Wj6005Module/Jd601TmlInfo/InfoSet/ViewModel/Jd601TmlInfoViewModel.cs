using System.ComponentModel.Composition;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.InfoSet.Services;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.InfoSet.ViewModel
{


    [Export(typeof(IIJd601TmlInfoView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Jd601TmlInfoViewModel : Cr.Core.CoreServices.ObservableObject, IIJd601TmlInfoView
    {
        #region tab
        public int Index
        {
            get { return 1; }
        }
        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public string Title
        {
            get { return "节电参数设置"; }
        }

        #endregion

        #region NavOnLoad

        public void NavOnLoad(params object[] parsObjects)
        {
            //GetEsuRtus();
            var tmlId = (int)parsObjects[0];
            if (tmlId < 1) return;
            //var setview =
            //   Cr.Core.CoreServices.RegionManage.GetViewById(
            //       Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoSetViewIdViewId, tmlId);
            //if (SettingControl == null) SettingControl = setview;

            //var dataview =
            //    Cr.Core.CoreServices.RegionManage.GetViewById(
            //        Wj6005Module.Services.ViewIdAssign.Jd601ControlAndDataViewId, tmlId);
            //if (DataControl == null) DataControl = dataview;

            //var getparview =
            //    Cr.Core.CoreServices.RegionManage.GetViewById(
            //        Wj6005Module.Services.ViewIdAssign.TmlInfoSetZcForjd601ViewId, tmlId);
            //if (GetParControl == null) GetParControl = getparview;

            var parametersSetview =
                Cr.Core.CoreServices.RegionManage.GetViewById(
                    Wj6005Module.Services.ViewIdAssign.TmlParmetersInfoSetForJd601ViewId, tmlId);
            if (ParametersSetControl == null) ParametersSetControl = parametersSetview;

            var instantDataView =
    Cr.Core.CoreServices.RegionManage.GetViewById(
        Wj6005Module.Services.ViewIdAssign.Jd601InstantDataViewId, tmlId);
            if (InstantData == null) InstantData = instantDataView;

            var operatorControl =
    Cr.Core.CoreServices.RegionManage.GetViewById(
        Wj6005Module.Services.ViewIdAssign.Jd601OperatorControlViewId, tmlId);
            if (OperatorControl == null) OperatorControl = operatorControl;
        }

        #endregion





        //private object _settingControl;

        //public object SettingControl
        //{
        //    get
        //    {
        //        return _settingControl;
        //    }
        //    set
        //    {
        //        if (value == _settingControl) return;
        //        _settingControl = value;
        //        RaisePropertyChanged(() => SettingControl);
        //    }
        //}

        private object _parametersSetControl;
        public object ParametersSetControl
        {
            get { return _parametersSetControl; }
            set
            {
                if (value == _parametersSetControl) return;
                _parametersSetControl = value;
                RaisePropertyChanged(() => ParametersSetControl);
            }
        }
        //private object _getParControl;
        //public object GetParControl
        //{
        //    get { return _getParControl; }
        //    set
        //    {
        //        if (value == _getParControl) return;
        //        _getParControl = value;
        //        RaisePropertyChanged(()=>GetParControl);
        //    }
        //}

        //private object _dataControl;

        //public object DataControl
        //{
        //    get
        //    {
        //        return _dataControl;
        //    }
        //    set
        //    {
        //        if (value == _dataControl) return;
        //        _dataControl = value;
        //        RaisePropertyChanged(() => DataControl);
        //    }

        //}

        private object _instantData;

        public object InstantData
        {
            get
            {
                return _instantData;
            }
            set
            {
                if (value == _instantData) return;
                _instantData = value;
                RaisePropertyChanged(() => InstantData);
            }

        }

        private object _operatorControl;

        public object OperatorControl
        {
            get
            {
                return _operatorControl;
            }
            set
            {
                if (value == _operatorControl) return;
                _operatorControl = value;
                RaisePropertyChanged(() => OperatorControl);
            }

        }
    }


}
