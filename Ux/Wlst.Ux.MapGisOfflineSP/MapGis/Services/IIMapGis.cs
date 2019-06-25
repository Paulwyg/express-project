using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Wlst.Ux.MapGisLocalSP.MapGis.Services
{
    public interface IIMapGis
    {
        void RaiseEvent();
        ContextMenu GetCm(List<int> rtulst, List<int> slulst, Dictionary<int, List<int>> sluctrllst);
        ContextMenu Cm { get; set; }
        void SetCmUnVisi();


        string RtuIdf
        {
            get;
            set;
        }
        string RtuName
        {
            get;
            set;
        }
        string GroupName
        {
            get;
            set;
        }
        string Remark
        {
            get;
            set;
        }
        string RtuPhyId
        {
            get;
            set;
        }
        string InstallAddr
        {
            get;
            set;
        }
        string DataCreate
        {
            get;
            set;
        }

        int CurrentRtuId { get; set; }
    }
}
