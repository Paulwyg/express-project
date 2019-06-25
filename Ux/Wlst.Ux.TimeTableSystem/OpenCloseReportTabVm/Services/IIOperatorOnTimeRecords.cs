namespace Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.Services
{
    public interface IIOpenCloseReportTabVm : Wlst.Cr.Core.CoreInterface.IITab
    {

        void OnSelectChanged(int timetableid);
    }
}
