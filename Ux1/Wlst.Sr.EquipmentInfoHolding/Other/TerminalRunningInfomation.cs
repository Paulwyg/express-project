namespace Wlst.Sr.EquipmentInfoHolding.Other
{
    public class TerminalRunningInfomation
    {
        public int RtuId { get; set; }
        public bool IsConnected { get; set; }

        public int ImageCode { get; set; }
    }

    //public class BasicTerminalInfomation : Infrastructure.Interface.IIBasicTmlInfomation
    //{
    //    #region 基本参数


    //    public int RtuId { get; set; }
    //    /// <summary>
    //    /// 终端名称
    //    /// </summary>
    //    public string RtuName { get; set; }

    //    /// <summary>
    //    /// 终端工作状态
    //    ///  0-不用，1-停运，2-使用
    //    /// </summary>
    //    public int State { get; set; }

    //    /// <summary>
    //    /// 终端型号 默认3005
    //    /// </summary>
    //    public int Model { get; set; }

    //    /// <summary>
    //    /// 地图X坐标 仅JPG
    //    /// </summary>
    //    public int X { get; set; }

    //    /// <summary>
    //    /// 地图Y坐标仅JPG
    //    /// </summary>
    //    public int Y { get; set; }

    //    /// <summary>
    //    /// 地图X坐标 GIS以及其他矢量地图
    //    /// </summary>
    //    public double GisX { get; set; }

    //    /// <summary>
    //    /// 地图Y坐标 GIS以及其他矢量地图
    //    /// </summary>
    //    public double GisY { get; set; }

    //    /// <summary>
    //    /// 终端是否已经上线
    //    /// </summary>
    //    public bool IsConnected { get; set; }


    //    public long RecentUpdateTime { get; set; }

    //    public int SwitchOutLoops { get; set; }

    //    public int SwitchInLoops { get; set; }

    //    /// <summary>
    //    /// 终端通信方式
    //    /// </summary>
    //    public int CommType { get; set; }

    //    /// <summary>
    //    /// 终端图标代码 0 连接断开 1关灯正常 2关灯故障  3 开灯正常 4开灯故障
    //    /// </summary>
    //    public int ImageCode { get; set; }


    //    #endregion

    //    public BasicTerminalInfomation(Infrastructure.Interface.IIBasicTmlInfomation basicTmlInfomation)
    //    {
    //        this.RtuId = basicTmlInfomation.RtuId;
    //        this.RtuName = basicTmlInfomation.RtuName;
    //        this.State = basicTmlInfomation.State;
    //        this.X = basicTmlInfomation.X;
    //        this.Y = basicTmlInfomation.Y;
    //        this.GisX = basicTmlInfomation.GisX;
    //        this.GisY = basicTmlInfomation.GisY;
    //        this.Model = basicTmlInfomation.Model;
    //        this.RecentUpdateTime = basicTmlInfomation.RecentUpdateTime;

    //        ImageCode = RtuId%10;
    //    }

    //    public void SetOrUpdateBaseTerminalInfo(Infrastructure.Interface.IIBasicTmlInfomation basicTmlInfomation, bool publisEvent)
    //    {
    //        bool bolChangeValue = false;
    //        if (this.RtuId != basicTmlInfomation.RtuId)
    //        {
    //            return;
    //        }
    //        if (this.RtuName != basicTmlInfomation.RtuName)
    //        {
    //            this.RtuName = basicTmlInfomation.RtuName;
    //            bolChangeValue = true;
    //        }
    //        if (this.State != basicTmlInfomation.State)
    //        {
    //            this.State = basicTmlInfomation.State;
    //            bolChangeValue = true;
    //        }
    //        if (this.X != basicTmlInfomation.X)
    //        {
    //            this.X = basicTmlInfomation.X;
    //            bolChangeValue = true;
    //        }
    //        if (this.Y != basicTmlInfomation.Y)
    //        {
    //            this.Y = basicTmlInfomation.Y;
    //            bolChangeValue = true;
    //        }
    //        if (this.GisX.Equals(basicTmlInfomation.GisX))
    //        {
    //            this.GisX = basicTmlInfomation.GisX;
    //            bolChangeValue = true;
    //        }
    //        if (this.GisY.Equals(basicTmlInfomation.GisY))
    //        {
    //            this.GisY = basicTmlInfomation.GisY;
    //            bolChangeValue = true;
    //        }
    //        if (this.Model != basicTmlInfomation.Model)
    //        {

    //            this.Model = basicTmlInfomation.Model;
    //            bolChangeValue = true;
    //        }
    //        this.RecentUpdateTime = basicTmlInfomation.RecentUpdateTime;
    //        if (bolChangeValue && publisEvent) PublishUpdateTerminalInfoEvenet();
    //    }


    //    public  void PublishUpdateTerminalInfoEvenet()
    //    {
    //        //发布事件  update
    //        var args = new PublishEventArgs()
    //                       {
    //                           EventType = PublishEventType.Core,
    //                           EventId = 52303,
    //                       };
    //        args.AddParams(RtuId);
    //        EventPublisher.EventPublish(args);
    //    }

    //    public void PublishAddTerminalInfoEvent()
    //    {
    //        //发布事件  add
    //        var args = new PublishEventArgs()
    //        {
    //            EventType = PublishEventType.Core,
    //            EventId = 52301,
    //        };
    //        args.AddParams(RtuId);
    //        EventPublisher.EventPublish(args);
    //    }


    //    public void PublishDeleteTerminalInfoEvent()
    //    {
    //        //发布事件  delete
    //        var args = new PublishEventArgs()
    //        {
    //            EventType = PublishEventType.Core,
    //            EventId = 52303,
    //        };
    //        args.AddParams(RtuId);
    //        EventPublisher.EventPublish(args);
    //    }

    //}
}