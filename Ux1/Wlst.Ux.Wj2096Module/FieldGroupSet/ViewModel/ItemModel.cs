using System.Windows;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj2096Module.FieldGroupSet.ViewModel
{
    public class ItemModel : ObservableObject
    {

        public ItemModel()
        {
            GroupId = -1;
        }

        private bool _isChecked;

        /// <summary>
        /// 是否选中该条数据
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }


        private Visibility _itemVisi;
        /// <summary>
        /// 标记本条数据 是否显示或隐藏
        /// </summary>
        public Visibility ItemVisi
        {
            get { return _itemVisi; }
            set
            {
                if (value != _itemVisi)
                {
                    _itemVisi = value;
                    this.RaisePropertyChanged(() => this.ItemVisi);
                }
            }
        }

        private int _id;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.ID);
                }
            }
        }

        private int _areaId;
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (_areaId == value) return;
                _areaId = value;
                RaisePropertyChanged(() => AreaId);
            }
        }

        private int _physicalId;
        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if(_physicalId==value) return;
                _physicalId = value;
                RaisePropertyChanged(()=>PhysicalId);
            }
        }

        private string _name;


        /// <summary>
        /// 终端名称
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

        private string _type;

        /// <summary>
        /// 设备类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    this.RaisePropertyChanged(() => this.Type);
                }
            }
        }
        /// <summary>
        /// 组地址，隐藏 或排序使用；
        /// </summary>
        public int GroupId;

        private string _groupName;

        /// <summary>
        /// 本终端归属组名称
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (value != _groupName)
                {
                    _groupName = value;
                    this.RaisePropertyChanged(() => this.GroupName);
                }
            }
        }



    }
}
