using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Cr.CoreMims.CoreInterface
{
    public interface IITreeNodeLoadExport
    {
        List<int> RtuModes { get;  }
        ObservableCollection<TreeNodeBaseViewModel> GetTreeNodeInfo(int rtuId);
    }
}
