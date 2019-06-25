using System;

namespace Wlst.Cr.CoreMims.CoreInterface
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IIEquipmentModel
    {
        /// <summary>
        /// 可添加设备名称附属 型号
        /// </summary>
        int ModelKey { get; }

        /// <summary>
        /// 可添加设备的名称
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// 描述
        /// </summary>
        string ModuleDescription { get; }

        /// <summary>
        /// 是否可以为附属设备
        /// </summary>
        bool CanBeAttachEquipmnet { get; }

        /// <summary>
        /// 是否可以为主设备
        /// </summary>
        bool CanBeMainEquipment { get; }

        ///// <summary>
        ///// 设备增加后是否可以在客户端界面中删除
        ///// </summary>
        //bool CanBeDelete { get; }

        //Uri ModuleImageUri { get; }

        //int ModuleInfoSetViewId { get; }
        //string ModuleInfoSetViewAttachRegion { get; }

        EquipmentModelArgs Args { get; }
    };


    public class EquipmentModelArgs
    {
        /// <summary>
        /// 该设备的图形图像路径
        /// </summary>
        public string ImageSourcePath;

        public string Name;

        /// <summary>
        /// 该设备参数设置所在页面的页面地址
        /// </summary>
        public int ModelInfoSetViewId;

        /// <summary>
        /// 该设备参数设置所在页面需要呈现到的区域名称
        /// </summary>
        public string ModelInfoSetViewAttachRegion;
    }
}
