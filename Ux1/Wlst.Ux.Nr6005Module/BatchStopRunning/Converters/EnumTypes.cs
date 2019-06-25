namespace Wlst.Ux.Nr6005Module.BatchStopRunning.Services
{
    public enum EnumOpenOrCloseAns
    {
       NoAnswer,  //无应答
       YesAnswer  //应答
    }
    public enum EnumSelectionTestAns
    {
        Ready, //准备等待
        Open, //开
        Close , //关
        Reply, //应答
    }
    public enum EnumTmlState
    {
        Use,  //使用
        Disable, //停用
        NotUse //不用
    }
    public enum  EnumWeekSndAns
    {
        Ready,
        K1K3Ans,
        K4K6Ans,
        AllAns
    }

    public enum EnumReportTypes
    {
        NoReport,
        OpenLightReport,
        CloseLightReport,
        SelectionTestReport,
        AsyncTimeReport,
        WeekSndReport
        
    }
}
