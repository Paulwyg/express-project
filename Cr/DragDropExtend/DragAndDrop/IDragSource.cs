using System.Windows;

namespace DragDropExtend.DragAndDrop
{
    /// <summary>
    /// 源端
    /// 任何需要Drag and  drop的实例  必须实现该接口
    /// </summary>
    public interface IDragSource
    {
        /// <summary>
        /// 拖动效果
        /// </summary>
        /// <param name="Data"></param>
        DragDropEffects StartDrag(object Data);

        /// <summary>
        /// 提取拖动时需要携带的数据 
        /// 传递数据
        /// </summary>
        /// <returns></returns>
        object DragData();
    }
}
