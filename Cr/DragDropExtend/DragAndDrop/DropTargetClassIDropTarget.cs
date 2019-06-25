using System;
using System.Windows;

namespace DragDropExtend.DragAndDrop
{
    public  class DropTargetClassIDropTarget : IDropTarget
    {
        private Func<object, DragDropEffects> funcDragOver;
        private Action<object, DragEventArgs, object> funcDrop;

        /// <summary>
        /// 实现IDropTarget接口
        /// </summary>
        /// <param name="funDragOver">IDropTarget的DragOver</param>
        /// <param name="funDrop">IDropTarget的Drop</param>
        public DropTargetClassIDropTarget(Func<object, DragDropEffects> funDragOver,
                                          Action<object, DragEventArgs, object> funDrop)
        {
            this.funcDragOver = funDragOver;
            this.funcDrop = funDrop;
        }


        #region IDropTarget 成员

        public DragDropEffects DragOver(object Data)
        {
            if (funcDragOver != null)
            {
                return funcDragOver(Data);
            }
            return DragDropEffects.None;
        }

        public void Drop(object sender, DragEventArgs e, object Data)
        {
            if (funcDrop != null)
            {
                funcDrop(sender, e, Data);
            }
        }

        #endregion
    }
}
