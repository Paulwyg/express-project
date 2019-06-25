using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.AddMainEquipment.ViewModels
{
    public class EquipmentViewItem : ObservableObject
    {
        private int _moduleKey;
        /// <summary>
        /// 设备模块关键字
        /// </summary>
        public int ModuleKey
        {
            get { return _moduleKey; }
            set
            {
                if (value != _moduleKey)
                {
                    _moduleKey = value;
                    this.RaisePropertyChanged(() => this.ModuleKey);
                }
            }
        }


        private string _moduleDes;
        /// <summary>
        /// 设备模块描述
        /// </summary>
        public string ModuleDes
        {
            get { return _moduleDes; }
            set
            {
                if (value != _moduleDes)
                {
                    _moduleDes = value;
                    this.RaisePropertyChanged(() => this.ModuleDes);
                }
            }
        }



        private string _name;
        /// <summary>
        /// 设备模块描述
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

        private object _imageIcon;
        public object ImageIcon
        {
            get { return _imageIcon; }
            set
            {
                if(value !=_imageIcon)
                {
                    _imageIcon = value;
                    this.RaisePropertyChanged(()=>this.ImageIcon);
                }
            }
        }

        private int _moduleInfoSetViewId;
        public int ModuleInfoSetViewId
        {
            get { return _moduleInfoSetViewId; }
            set
            {
                if(value !=_moduleInfoSetViewId)
                {
                    _moduleInfoSetViewId = value;
                    this.RaisePropertyChanged(()=>this.ModuleInfoSetViewId);
                }
            }
        }
        private string _moduleInfoSetViewAttachRegion;
        public string ModuleInfoSetViewAttachRegion
        {
            get { return _moduleInfoSetViewAttachRegion; }
            set
            {
                if(value !=_moduleInfoSetViewAttachRegion)
                {
                    _moduleInfoSetViewAttachRegion = value;
                    this.RaisePropertyChanged(()=>this.ModuleInfoSetViewAttachRegion);
                }
            }
        }
    }
}
