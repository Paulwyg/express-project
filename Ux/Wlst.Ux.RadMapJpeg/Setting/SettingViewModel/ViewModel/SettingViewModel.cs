using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.RadMapJpeg.Setting.SettingViewModel.Services;

namespace Wlst.Ux.RadMapJpeg.Setting.SettingViewModel.ViewModel
{
    [Export(typeof (IISettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SettingViewModel : ObservableObject, IISettingViewModel
    {
        public SettingViewModel()
        {
            //_dtApply = DateTime.Now.AddDays(-1);
            this.NavOnLoad();
        }

        #region  define



        private bool _iIsAllowZoom;

        /// <summary>
        /// 是否允许地图放大缩小 
        /// </summary>
        public bool IsAllowZoom
        {
            get { return _iIsAllowZoom; }
            set
            {
                if (value != _iIsAllowZoom)
                {
                    _iIsAllowZoom = value;
                    this.RaisePropertyChanged(() => this.IsAllowZoom);
                }
            }
        }

        private bool _iIsAllowNavToEquImage;

        /// <summary>
        /// 是否允许地图联动
        /// </summary>
        public bool IsAllowNavToEquImage
        {
            get { return _iIsAllowNavToEquImage; }
            set
            {
                if (value != _iIsAllowNavToEquImage)
                {
                    _iIsAllowNavToEquImage = value;
                    this.RaisePropertyChanged(() => this.IsAllowNavToEquImage);
                }
            }
        }

        #endregion

        //private DateTime _dtApply;
        private ICommand _cmdApply;

        public ICommand CmdApply
        {
            get
            {
                if (_cmdApply == null) _cmdApply = new RelayCommand(Ex, CanEx, false);
                return _cmdApply;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Ex()
        {
            // _dtApply = DateTime.Now;
            //if (MapJepg.ViewModels.RadMapJpegViewModel.MySelf == null) return;
            MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.IsUserNetMap = this.IsAllowZoom;
            MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.CanNavToEquImage = IsAllowNavToEquImage;
            MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.SavConfig();
        }

        private bool CanEx()
        {
            if (this.IsAllowNavToEquImage != MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.CanNavToEquImage ||
                this.IsAllowZoom != MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.IsUserNetMap)
                return true;
            else return false;
            //return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            this.IsAllowNavToEquImage = MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.CanNavToEquImage;
            this.IsAllowZoom = MapJepg.ViewModels.RadMapJpegViewModelSet.MySelf.IsUserNetMap;
        }
    };
}