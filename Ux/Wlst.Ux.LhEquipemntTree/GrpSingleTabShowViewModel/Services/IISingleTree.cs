﻿using System;
using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.LhEquipemntTree.GrpComSingleMuliViewModel;

namespace Wlst.Ux.LhEquipemntTree.GrpSingleTabShowViewModel.Services
{
    public interface IISingleTree : IITab
    {
        ObservableCollection<TreeNodeBaseNode> ChildTreeItems { get; }

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
