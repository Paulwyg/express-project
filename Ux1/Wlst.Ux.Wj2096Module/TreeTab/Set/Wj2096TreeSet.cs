using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;

namespace Wlst.Ux.Wj2096Module.TreeTab.Set
{
    [Export(typeof (IIWj2096TreeSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Wj2096TreeSet : Wlst.Cr.Core.CoreServices.ObservableObject, IIWj2096TreeSet
    {
        public Wj2096TreeSet()
        {
            //IsShowGrpInTreeModelShowId = Wj2096TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            //IsShowConOnNodeSelected = Wj2096TreeSetLoad.Myself.IsShowConOnNodeSelected;

            IsShowTreeOnTab = Wj2096TreeSetLoad.Myself.IsShowTreeOnTab;
            //IsNoRtu = Wj2096TreeSetLoad.Myself.IsNoRtu;

           // IsShowTreeOnTab = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2096, 1, false);
           // IsNoRtu = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2096, 2, false);

            //IsIconFollowTheRtu = Wj2096TreeSetLoad.Myself.IsIconFollowTheRtu;
            _dtApply = DateTime.Now.AddDays(-1);
            //this.NavOnLoad();
        }

        #region  define


        private bool _isShowTreeOnTab;

        /// <summary>
        /// 是否在主界面显示 
        /// </summary>
        public bool IsShowTreeOnTab
        {
            get { return _isShowTreeOnTab; }
            set
            {
                if (value != _isShowTreeOnTab)
                {
                    _isShowTreeOnTab = value;
                    this.RaisePropertyChanged(() => this.IsShowTreeOnTab);
                }
            }
        }


        #endregion

        
        //private bool _isNoRtu;

        ///// <summary>
        ///// 是否有终端
        ///// </summary>
        //public bool IsNoRtu
        //{
        //    get { return _isNoRtu; }
        //    set
        //    {
        //        if (value != _isNoRtu)
        //        {
        //            _isNoRtu = value;
        //            this.RaisePropertyChanged(() => this.IsNoRtu);
        //        }
        //    }
        //}




        private DateTime _dtApply;
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
            _dtApply = DateTime.Now;
            //Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected = IsShowConOnNodeSelected;
            Wj2096TreeSetLoad.Myself.IsShowTreeOnTab = IsShowTreeOnTab;
            //Wj2096TreeSetLoad.Myself.IsNoRtu = IsNoRtu;
            //Wj2090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId = IsShowGrpInTreeModelShowId;
            //Wj2090TreeSetLoad.Myself.IsIconFollowTheRtu = IsIconFollowTheRtu;
            Wj2096TreeSetLoad.Myself.SavConfig();
        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //LoadConfig();

            //IsShowConOnNodeSelected = Wj2096TreeSetLoad.Myself.IsShowConOnNodeSelected;
            IsShowTreeOnTab = Wj2096TreeSetLoad.Myself.IsShowTreeOnTab;
            //IsNoRtu = Wj2096TreeSetLoad.Myself.IsNoRtu;


            //IsShowGrpInTreeModelShowId = Wj2096TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            //IsIconFollowTheRtu = Wj2096TreeSetLoad.Myself.IsIconFollowTheRtu;
        }
    }



}
