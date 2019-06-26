namespace Wlst.Cr.Core.CoreInterface
{

    /// <summary>
    /// 如果使用继承自ContentControl的控件来生成界面，但需要显示一些信息，如页面标题，页面在容器中是否可以关闭 是否可以悬浮等等信息
    /// </summary>
    public interface IITab
    {
        /// <summary>
        /// a title for this form
        /// </summary>
        string Title { get; }

        /// <summary>
        /// <c>True</c> if this instance can CanClose; otherwise, <c>False</c>.
        /// </summary>
        bool CanClose { get; }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// </summary>
        bool CanUserPin { get; }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// </summary>
        bool CanFloat { get; }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// </summary>
        bool CanDockInDocumentHost { get; }

        /// <summary>
        /// 需要设置的显示位置 针对终端树有效
        /// </summary>
        int Index { get; }
    }
}
