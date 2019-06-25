using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Cr.CoreOne.Models
{

    /// <summary>
    /// <para> 此类供继承 iMenuItem接口的部件进行继承，目的实现iMenuItem部分接口的初始化  </para>
    /// <para>避免继承该接口的所有部件均重复代码</para>
    /// <para>任何菜单部件如若需要导航到某界面同时某界面需要导航到时执行初始化则需视图Datacontent实现IINavOnLoad接口</para>
    /// <para>实现对iMenuItem中的参数进行 Get Set基本操作</para>
    /// <para>以及实现实例化一个MenuItem 对象并进行Name Tooltips简单设置</para>
    /// </summary>
    [Serializable]
    public class MenuItemBase : ObservableObject, IIMenuItem
    {

        #region  IIMenuItem参数

        /// <summary>
        /// 初始化参数
        /// </summary>
        public MenuItemBase()
        {
            Text = "Null";
            Id = -1;
            this.GroupName = string.Empty;
            this.Visibility = Visibility.Visible;

            this.Image = null;
            this.IsCheckable = false;
            this.IsChecked = false;
            this.IsEnabled = true ;
            this.Parent = null;
            this.ExText = "";
            this.Description = "Null";
            this.Classic = "Not Set So important Attr";
        }

        /// <summary>
        /// 如果执行Command事件，需要参数则置放于此
        /// </summary>
        public object Argu { get; set; }

        /// <summary>
        /// 当生成菜单的时候需要调用该函数来对Argu参数赋值
        /// </summary>
        public virtual void InitDataWhenBeforeUse(object argu)
        {
            Argu = argu;
        }

        /// <summary>
        /// 本菜单 是否 在生成的时候 动态调用 是否可以显示
        /// </summary>
        public virtual   bool IsCanBeShowRwx()
        {
            return false;
        }

        public event EventHandler OnVisibilityChanged;
    
        private string _text;

        /// <summary>
        /// 标题内容 初始化
        /// </summary>
        public string Text
        {
            get { return this._text; }
            set
            {
                if (value != this._text)
                {
                    this._text = value;
                    this.RaisePropertyChanged(() => this.Text);
                }
            }
        }


        private string _teTextTmpxt;

        /// <summary>
        /// 临时变量 可以任意修改的  整合显示 test与 extest
        /// </summary>
        public string TextTmp
        {
            get { return this._teTextTmpxt; }
            set
            {
                if (value != this._teTextTmpxt)
                {
                    this._teTextTmpxt = value;
                    this.RaisePropertyChanged(() => this.TextTmp);
                }
            }
        }


        private object _exTCommandParameterext;

        /// <summary>
        /// 扩展名称，用于如 开K1-全夜灯 立即运算的
        /// </summary>
        public object  CommandParameter
        {
            get { return this._exTCommandParameterext; }
            set
            {
                if (value != _exTCommandParameterext)
                {
                    this._exTCommandParameterext = value;
                    this.RaisePropertyChanged(() => this.CommandParameter);
                }
            }
        }

        private string _exText;

        /// <summary>
        /// 扩展名称，用于如 开K1-全夜灯 立即运算的
        /// </summary>
        public string ExText
        {
            get { return this._exText; }
            set
            {
                if (value != this._exText)
                {
                    this._exText = value;
                    this.RaisePropertyChanged(() => this.ExText);
                }
            }
        }

        private string _classic;

        public string Classic
        {
            get { return _classic; }
            set
            {
                if (_classic != value)
                {
                    _classic = value;
                    this.RaisePropertyChanged(() => this.Classic);

                }
            }
        }

        private bool _isCheckable;

        /// <summary>
        /// 是否可被选中，默认false
        /// </summary>
        public bool IsCheckable
        {
            get { return this._isCheckable; }
            set
            {
                if (value != this._isCheckable)
                {
                    this._isCheckable = value;
                    this.RaisePropertyChanged(() => this.IsCheckable);
                }
            }
        }

        private bool _isSeparator;

        /// <summary>
        /// 是否具有分隔符  默认不具有
        /// </summary>
        public bool IsSeparator
        {
            get { return this._isSeparator; }
            set
            {
                if (value != this._isSeparator)
                {
                    this._isSeparator = value;
                    this.RaisePropertyChanged(() => this.IsSeparator);
                }
            }
        }


        private bool _staysOpenOnClick;

        /// <summary>
        /// 当用户点击后 菜单是否依然保留，默认不保留  退去
        /// </summary>
        public bool StaysOpenOnClick
        {
            get { return this._staysOpenOnClick; }
            set
            {

                if (value != this._staysOpenOnClick)
                {
                    this._staysOpenOnClick = value;
                    this.RaisePropertyChanged(() => this.StaysOpenOnClick);
                }
            }
        }

        private bool _isEnabled = true;

        /// <summary>
        /// 菜单是否可用  默认true
        /// </summary>
        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set
            {
                if (value != this._isEnabled)
                {
                    this._isEnabled = value;
                    this.RaisePropertyChanged(() => this.IsEnabled);
                }
            }
        }

        private bool _isChecked;

        /// <summary>
        /// 菜单是否被点击  默认false;如果重写此参数请调用基类
        /// </summary>
        public virtual bool IsChecked
        {
            get { return this._isChecked; }
            set
            {
                if (value != this._isChecked)
                {
                    this._isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                    if (!string.IsNullOrEmpty(this.GroupName))
                    {
                        if (this.IsChecked)
                        {
                            this.UncheckOtherItemsInGroup();
                        }
                    }
                }
            }
        }

        private void UncheckOtherItemsInGroup()
        {
            if (this.Parent == null) return;
            if (string.IsNullOrEmpty(this.GroupName)) return;
            IEnumerable<IIMenuItem> groupItems =
                this.Parent.CmItems.Where((IIMenuItem item) => item.GroupName == this.GroupName);
            foreach (IIMenuItem item in groupItems)
            {
                if (item != this)
                {
                    item.IsChecked = false;
                    // item.OnPropertyChanged("IsChecked");
                }
            }
        }



        private object _image;

        /// <summary>
        /// 菜单图片
        /// </summary>
        public object Image
        {
            get { return this._image; }
            set
            {
                if (value != this._image)
                {
                    this._image = value;
                    this.RaisePropertyChanged(() => this.Image);
                }
            }
        }


        private int _id;

        /// <summary>
        /// 菜单全局唯一值
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set
            {
                if (value != this._id)
                {
                    this._id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private string _description;

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string Description
        {
            get { return this._description; }
            set
            {
                if (value != this._description)
                {
                    this._description = value;
                    this.RaisePropertyChanged(() => this.Description);
                }
            }
        }

        private string _tooltips;

        /// <summary>
        /// 菜单提示
        /// </summary>
        public string Tooltips
        {
            get { return this._tooltips; }
            set
            {
                if (value != this._tooltips)
                {
                    this._tooltips = value;
                    this.RaisePropertyChanged(() => this.Tooltips);
                }
            }
        }

        private string _shortCuts;

        /// <summary>
        /// 快捷键信息
        /// </summary>
        public string ShortCuts
        {
            get { return this._shortCuts; }
            set
            {
                if (value != this._shortCuts)
                {
                    this._shortCuts = value;
                    this.RaisePropertyChanged(() => this.ShortCuts);
                }
            }
        }

        private object _tag;

        /// <summary>
        /// 其他未知数据
        /// </summary>
        public object Tag
        {
            get { return this._tag; }
            set
            {
                if (value != this._tag)
                {
                    this._tag = value;
                    this.RaisePropertyChanged(() => this.Tag);
                }
            }
        }



        private System.Windows.Input.ICommand _command;

        /// <summary>
        /// 菜单命令 如果有的话
        /// </summary>
        public virtual System.Windows.Input.ICommand Command
        {
            get { return this._command; }
            set
            {
                if (value != this._command)
                {
                    this._command = value;
                    this.RaisePropertyChanged(() => this.Command);
                }
            }
        }


        private string _groupName;

        /// <summary>
        /// 菜单属于分组的名称，默认不属于任何分组，如果属于分组 则在选中时其他默认非选中
        /// </summary>
        public string GroupName
        {
            get { return this._groupName; }
            set
            {
                if (value == this._groupName) return;
                this._groupName = value;
                this.RaisePropertyChanged(() => this.GroupName);
            }
        }

        private Visibility _visibility;

        /// <summary>
        /// 菜单是否可见
        /// </summary>
        public Visibility Visibility
        {
            get { return this._visibility; }
            set
            {
                if (value != this._visibility)
                {
                    this._visibility = value;
                    this.RaisePropertyChanged(() => this.Visibility);
                    if (OnVisibilityChanged != null)
                    {
                        OnVisibilityChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        //private bool _isPrivilegLeave;
        ///// <summary>
        ///// 是否本菜单为用户权限验证的。请在函数中最后一个设置；请在函数中最后一个设置；请在函数中最后一个设置；请在函数中最后一个设置；
        ///// 请在函数中最后一个设置；请在函数中最后一个设置
        ///// </summary>
        //public bool IsPrivilegLeave
        //{
        //    get { return this._isPrivilegLeave; }
        //    set
        //    {
        //        if (value != this._isPrivilegLeave)
        //        {
        //            this._isPrivilegLeave = value;
        //            this.OnPrivilegChange();
        //            if (value)
        //                PrivilegsControls.forMenu.MenuPrivilegMonitor.MySelf.Nothingtodo();
        //        }
        //    }
        //}

        private ObservableCollection<IIMenuItem> _items;

        /// <summary>
        /// 本菜单下的子菜单 ，不允许操作;系统执行操作
        /// </summary>
        public ObservableCollection<IIMenuItem> CmItems
        {
            get { return _items ?? (_items = new ObservableCollection<IIMenuItem>()); }
             set
             {
                 if (_items == value) return;
                 _items = value;
                 this.RaisePropertyChanged(() => this.CmItems);
             }
        }

        /// <summary>
        /// 本菜单父类，不允许操作，系统执行操作
        /// </summary>
        public IIMenuItem Parent { get; set; }

        #endregion



        /// <summary>
        /// 导航 此导航不对页面后台MVVM执行初始化  不执行IINavOnLoad接口下的NavOnLoad函数执行
        /// </summary>
        /// <param name="viewId">导航页面Id值</param>
        public void ExNavNoArgs( int viewId)
        {
            try
            {
                RegionManage.ShowViewByIdAttachRegion(viewId, true);
                var ar = new PublishEventArgs
                {
                    EventId = CoreIdAssign.EventIdAssign.ShowViewInstructionEventId,
                    EventType = PublishEventType.Core
                };
                ar.AddParams(viewId);
                EventPublish.PublishEvent(ar);
                EventPublish.PublishEvent(new PublishEventArgs()
                                                {EventId = 10, EventType = "MainWindow.update.msgisvisi"});
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "CoreMenu MenuItemBase Nav to viewId: " + viewId + " Counter an Err :" + ex.ToString());
            }
        }



        /// <summary>
        /// 导航 此导航对页面后台MVVM执行初始化  将执行IINavOnLoad接口下的NavOnLoad函数执行
        /// </summary>
      
        /// <param name="viewId">导航页面Id值</param>
        /// <param name="parsObjects">IINavOnLoad接口下的NavOnLoad函数 参数</param>
        public void ExNavWithArgs(int viewId, params object[] parsObjects)
        {
            try
            {
                RegionManage.ShowViewByIdAttachRegionWithArgu(viewId, parsObjects);
                var ar = new PublishEventArgs
                {
                    EventId = CoreIdAssign.EventIdAssign.ShowViewInstructionEventId,
                    EventType = PublishEventType.Core
                };
                ar.AddParams(viewId);
                EventPublish.PublishEvent(ar);
                EventPublish.PublishEvent(new PublishEventArgs() { EventId = 10, EventType = "MainWindow.update.msgisvisi" });
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "CoreMenu MenuItemBase Nav to viewId: " + viewId + " Counter an Err :" + ex.ToString());

                try
                {
                    RegionManage.ShowViewByIdAttachRegionWithArgu(viewId, parsObjects);
                    var ar = new PublishEventArgs
                    {
                        EventId = CoreIdAssign.EventIdAssign.ShowViewInstructionEventId,
                        EventType = PublishEventType.Core
                    };
                    ar.AddParams(viewId);
                    EventPublish.PublishEvent(ar);
                    EventPublish.PublishEvent(new PublishEventArgs() { EventId = 10, EventType = "MainWindow.update.msgisvisi" });
                }
                catch (Exception exx)
                {
                 
                }
            }
        }



        public string ControlType
        {
            get { return "Menu"; }
        }
    }
}
