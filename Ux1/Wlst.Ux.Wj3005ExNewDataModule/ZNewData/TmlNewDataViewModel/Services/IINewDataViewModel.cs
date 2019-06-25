using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj3005ExNewDataModule.ZNewData.TmlNewDataViewModel.Services
{
    /// <summary>
    /// 终端最新窗口接口
    /// </summary>
    public interface IINewDataViewModel:IITab ,IINavOnLoad
    {
        void MeasureRtu();


        void RequestNearData();
        ///// <summary>
        ///// 终端地址
        ///// </summary>
        //int RtuId { get; set; }

        //string RtuName { get; set; }

        ///// <summary>
        ///// 客户的接收到数据的时间
        ///// </summary>
        //DateTime DtGetDataTime { get; set; }

        ///// <summary>
        ///// 回路最新数据
        ///// 其回路信息存放结构为 NewDataforOneLoop
        ///// </summary>
        //ObservableCollection<TmlAmpLoopViewModel> LstNewLoopsData { get; }

        /////// <summary>
        /////// 开关量输入状态数据  是否每个开关量输入吸合； 默认36路依次为0~35；
        /////// <para>false 开 ，true 吸合</para>
        /////// </summary>
        ////ObservableCollection<bool> LstIsSwithInAttraction { get; }

        ///// <summary>
        ///// 开关量输出状态数据 是否每个开关量输出吸合连接 
        ///// </summary>
        //ObservableCollection<KeyValueName> LstIsSwitchOutAttraction { get; }

        ///// <summary>
        ///// 更新最新数据时 使用书节点焦点转移
        ///// </summary>
        //bool OnlyTreeNodeChangeCanActiveNewData { get; set; }

    }
}
