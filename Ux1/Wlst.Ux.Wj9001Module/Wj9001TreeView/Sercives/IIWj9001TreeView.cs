using System;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj9001Module.Wj9001TreeView.Sercives
{
    public interface IIWj9001TreeView : IINavOnLoad,IITab 
    {

        string SearchText { get; set; }

        event EventHandler<NodeSelectedArgs> OnSelectedNodeByCode;

        event EventHandler<NodeSelectedArgs> OnClearSerchTest;

        void SearchNodeold(string keyWord);
    }
    public class NodeSelectedArgs : EventArgs
    {
        public int RtuIdSelected;
        public string SearchText;
    }
}
