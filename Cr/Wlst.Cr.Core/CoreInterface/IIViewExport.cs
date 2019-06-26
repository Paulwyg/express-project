namespace Wlst.Cr.Core.CoreInterface
{
    /// <summary>
    /// 任何视图部件需要导出则必须实现的接口
    /// </summary>
    public interface IIViewExport
    {

        /// <summary>
        /// 部件ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 视图是否在模块加载时立即显示到指定的附属Region上
        /// </summary>
        bool AttachNow { get; }

        /// <summary>
        /// 视图需要附属到的Region名称
        /// </summary>
        string AttachRegion { get; }

        /// <summary>
        /// 该view如果需要附属到父view上则设置  父view Value
        /// </summary>
        int fatherId { get; }
    };


}
