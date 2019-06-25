using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;

using Wlst.Ux.Wj6005Module.Jd601ManageViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.Wj6005Module.Jd601ManageViewModel.ViewModel
{
    [Export(typeof (IIJd601ManageView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601ManageViewModel : 
        Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, 
        IIJd601ManageView
    {
        #region tab
        public int Index
        {
            get { return 8; }
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
            get { return "节电节能"; }
        }

        #endregion

        private int _hxxx = 0;
        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightTree;
                    if (_hxxx > 24) _hxxx = 24;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }

        public Jd601ManageViewModel()
        {
            MySelf = this;
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Load,1,DelayEventHappen.EventOne);
            //Load();
        }

        #region NavOnLoad

        public void NavOnLoad(params object[] parsObjects)
        {
            //GetEsuRtus();

        }

        #endregion


        public static bool OnSelectNodeChangeNavToParsSet = true ;

        private void UpdateViewByTmlId(int rtuId)
        {
            if (rtuId < 1) return;
            if (!OnSelectNodeChangeNavToParsSet) return;
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                Wj6005Module.Services.ViewIdAssign.Jd601TmlInfoViewId, rtuId);

        }


        private static Jd601ManageViewModel MySelf;
        private static TreeNodeWj1090ViewModel _currentSelectedTreeNode;

        public static TreeNodeWj1090ViewModel CurrentSelectedTreeNode
        {
            get { return _currentSelectedTreeNode; }
            set
            {
                if (_currentSelectedTreeNode != value)
                {
                    _currentSelectedTreeNode = value;
                    if (_currentSelectedTreeNode != null)
                    {
                        //  var view = CoreRun.CoreService.CoreServices.GetViewById(0, _currentSelectedTreeNode.NodeId);
                        if (MySelf != null)
                        {
                            MySelf.UpdateViewByTmlId(_currentSelectedTreeNode.NodeId);
                        }
                    }
                }
            }
        }

        #region Reflesh

        private DateTime _dtReflesh;
        public ICommand _reflesh;

        public ICommand Reflesh
        {
            get
            {
                if (_reflesh == null) _reflesh = new RelayCommand(ExReflesh,CanReflesh,true);
                return _reflesh;
            }
        }

        private bool CanReflesh()
        {
            return DateTime.Now.Ticks - _dtReflesh.Ticks > 30000000;

        }
        private void ExReflesh()
        {
            _dtReflesh = DateTime.Now;
            this.Load();
        }

        #endregion

    }



    /// <summary>
    /// data  attri
    /// </summary>
    public partial class Jd601ManageViewModel 
    {
        private ObservableCollection<TreeNodeWj1090ViewModel> _collectionWj1090;

     

        public ObservableCollection<TreeNodeWj1090ViewModel> CollectionJd601
        {
            get
            {
                if (_collectionWj1090 == null)
                    _collectionWj1090 = new ObservableCollection<TreeNodeWj1090ViewModel>();
                return _collectionWj1090;
            }
        }

        private void Load()
        {
            CollectionJd601.Clear();
            foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                )
            {
      
                if (t.Key < 1200000 || t.Key > 1300000) continue;
                var fff = t.Value as Sr.EquipmentInfoHolding.Model.Wj601Esu;
                if (fff == null) continue;


                CollectionJd601.Add(new TreeNodeWj1090ViewModel(fff.RtuId, fff.RtuName, t.Value.RtuFid));
            }


            //if (CollectionJd601.Count > 0)
            //{
            //    CurrentSelectedTreeNode = CollectionJd601[0];
            //}

        }
    }
}
