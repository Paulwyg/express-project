namespace Wlst.Ux.Statistics.UxStatistics.Services
{

    public interface IIUxStatisticsModule : Wlst.Cr.Core.CoreInterface.IINavOnLoad, Wlst.Cr.Core.CoreInterface.IIOnHideOrClose
    {
        void ShowDetailView(int x, int y,string name );
    }
}
