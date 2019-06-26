using System;
using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Cr.Core.Behavior
{
    /// <summary>
    /// 视图导出规格 如果未设置AttachRegion 则默认导出到 DocumentRegionName.DocumentRegion 区域
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [MetadataAttribute]
    
    public class ViewExportAttribute : ExportAttribute, IIViewExport
    {
 
        /// <summary>
        /// 视图导；导出类型为object
        /// </summary>
        /// <param name="id">视图唯一识别地址</param>
        /// <param name="attachRegion">视图需要显示的区域 默认DocumentRegion</param>
        /// <param name="attachNow"> 视图是否需要立即显示 默认false</param>
        public ViewExportAttribute(int id, string attachRegion = DocumentRegionName.DocumentRegion, bool attachNow = false)
            : base(typeof(object))
        {
            AttachRegion = attachRegion;
            Id = id + "";
            fatherId = -1;
            AttachNow = attachNow;
        }


        /// <summary>
        /// 视图导；导出类型为object
        /// </summary>
        /// <param name="viewType">视图类型 typeof(视图)</param>
        /// <param name="attachRegion">视图需要显示的区域 默认DocumentRegion</param>
        /// <param name="attachNow"> 视图是否需要立即显示 默认false</param>
        public ViewExportAttribute(Type viewType, string attachRegion = DocumentRegionName.DocumentRegion, bool attachNow = false)
            : base(typeof(object))
        {
            AttachRegion = attachRegion;
            Id = viewType.GUID + "";
            fatherId = -1;
            AttachNow = attachNow;
        }

        #region IIComponentInterface 成员

        /// <summary>
        /// View ID  如果提取的时候未int则转换为string即可
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否程序加载时立即显示
        /// </summary>
        public bool AttachNow { get; set; }

        /// <summary>
        /// 附属Region名称
        /// </summary>
        public string AttachRegion { get; set; }


        /// <summary>
        /// 该view如果需要附属到父view上则设置  父view Value
        /// </summary>
        public int fatherId { get; set; }

        #endregion
    };
}
