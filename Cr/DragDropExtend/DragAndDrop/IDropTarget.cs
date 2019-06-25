using System.Windows;

namespace DragDropExtend.DragAndDrop
{
    /// <summary>
    /// 目的端
    /// 任何需要Drag and  drop的实例  必须实现该接口
    /// </summary>
    public interface IDropTarget
    {
        /// <summary>
        /// 进入目标区域后 判断Data数据是否合法 合法后鼠标移动效果
        /// </summary>
        /// <param name="Data">携带的移动数据</param>
        /// <returns></returns>
        DragDropEffects DragOver(object Data);

        /// <summary>
        /// 移动函数 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">包含与所有拖放事件（System.Windows.DragDrop.DragEnter、System.Windows.DragDrop.DragLeave、System.Windows.DragDrop.DragOver和 System.Windows.DragDrop.Drop）相关的参数。</param>
        /// <param name="Data">携带的移动数据</param>
        void Drop(object sender, DragEventArgs e, object Data);
    }
}
