using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.ViewModel
{
    public class MenuInstancesViewModel : ObservableObject
    {
        private int _id;

        /// <summary>
        /// 实例菜单Id值
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private string _name;

        /// <summary>
        /// 实例菜单名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }


        private string _key;

        /// <summary>
        /// 菜单关键字
        /// </summary>
        public string Key
        {
            get { return _key; }
            set
            {
                if (value != _key)
                {
                    _key = value;
                    this.RaisePropertyChanged(() => this.Key);
                }
            }
        }


        private int _idClassic;

        /// <summary>
        /// 使用的模板菜单Id
        /// </summary>
        public int IdClassic
        {
            get { return _idClassic; }
            set
            {
                if (value != _idClassic)
                {
                    _idClassic = value;
                    this.RaisePropertyChanged(() => this.IdClassic);
                    var ff = ServerClassic .GetClassicValue(_idClassic);
                    if (ff == null) IdClassicName = "None";
                    else IdClassicName = ff.Name;
                }
            }
        }


        private string _idClassicName;

        /// <summary>
        /// 使用的模板菜单Id
        /// </summary>
        public string IdClassicName
        {
            get { return _idClassicName; }
            set
            {
                if (value != _idClassicName)
                {
                    _idClassicName = value;
                    this.RaisePropertyChanged(() => this.IdClassicName);
                }
            }
        }

    }
}
