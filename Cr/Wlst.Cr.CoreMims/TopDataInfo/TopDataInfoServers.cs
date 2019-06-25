using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Wlst.Cr.CoreMims.TopDataInfo
{
    /// <summary>
    /// 为Top区域提供数据、事件支持
    /// </summary>
    public class TopDataInfoServers
    {
        #region SingleInstance

        private static TopDataInfoServers _myself;

        /// <summary>
        /// 入口
        /// </summary>
        public static TopDataInfoServers MySelf
        {
            get
            {
                if (_myself == null)
                    _myself = new TopDataInfoServers();
                return _myself;
            }
        }

        private TopDataInfoServers()
        {
            _myself = this;
        }

        #endregion


        private ConcurrentDictionary<int, Tuple<string, string>> _data = new ConcurrentDictionary<int, Tuple<string, string>>();

        /// <summary>
        /// 当保存的数据发生变化的时候 触发
        /// </summary>
        public event EventHandler OnTopDataChanged;

        /// <summary>
        /// 替换显示数据 0为用户登陆，1 为光控使用 ,2 为定时任务，3 为时间表任务使用， 9 日落 ， 10 日出
        /// </summary>
        /// <param name="data">需要显示的数据</param>
        /// <param name="tooltipdata">该数据的提示数据</param>
        /// <param name="index">序号  默认从右往左 0-15</param>
        public void UpdateDataInfo(string data, string tooltipdata, int index)
        {
            if (_data.ContainsKey(index))
            {
                _data[index]= new Tuple<string, string>(data, tooltipdata);
            }else
            {
                _data.TryAdd(index, new Tuple<string, string>(data, tooltipdata));
            }
            
            if (OnTopDataChanged != null)
            {
                OnTopDataChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 获取数据信息
        /// </summary>
        /// <param name="index">序号  默认从右往左 0-15</param>
        /// <returns>第一个数据为显示数据 第二个数据位提示数据</returns>
        public Tuple<string, string> GetDataInfo(int index)
        {
            if (_data.ContainsKey(index)) return _data[index];
            return new Tuple<string, string>("", "");
        }
    }
}
