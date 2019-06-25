using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;


namespace Wlst.Ux.Wj2090Module.TreeTab.View.Serivices
{
    public interface IIAreaTree : IITab
    {

        string SearchText { get; set; }

        event EventHandler<NodeSelectedArgs> OnSelectedNodeByCode;
    }

    public class NodeSelectedArgs : EventArgs
    {
        public int SluIdSelected;
    }
}
