using System;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.Sercives
{
    public interface IIWj1050ManageViewModel : IINavOnLoad,IITab 
    {
        string SearchText { get; set; }

        event EventHandler<NodeSelectedArgs> OnClearSerchTest;

        void SearchNodeold(string keyWord);
    }

    public class NodeSelectedArgs : EventArgs
    {
        public int RtuIdSelected;
        public string SearchText;
    }
}
