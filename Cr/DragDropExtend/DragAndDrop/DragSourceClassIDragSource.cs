using System;
using System.Windows;

namespace DragDropExtend.DragAndDrop
{
    /// <summary>
    /// 本类实现IDragSource接口 
    /// 在构造函数中需要传入IDragSource需要的两个函数
    /// </summary>
    public class DragSourceClassIDragSource : IDragSource
    {
        private Func<object, DragDropEffects> funcStargDrag;
        private Func<object> funcDragData;

        /// <summary>
        /// 实现IDragSource接口
        /// </summary>
        /// <param name="funStargDrag">IDragSource的StargDrag</param>
        /// <param name="funDragData">IDragSource的DragData</param>
        public DragSourceClassIDragSource(Func<object, DragDropEffects> funStargDrag, Func<object> funDragData)
        {
            this.funcStargDrag = funStargDrag;
            this.funcDragData = funDragData;
        }

        #region IDragSource 成员

        public DragDropEffects StartDrag(object Data)
        {
            if (funcStargDrag != null)
            {
                return funcStargDrag(Data);
            }
            return DragDropEffects.None;
        }

        public object DragData()
        {
            if (funcDragData != null)
            {
                return funcDragData();
            }
            return null;
        }

        #endregion
    }
}
