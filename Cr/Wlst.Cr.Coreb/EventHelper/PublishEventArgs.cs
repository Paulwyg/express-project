using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.Coreb.EventHelper
{
    /// <summary>
    /// 事件发布 需要携带的结构体
    /// </summary>
    public class PublishEventArgs
    {
        protected List<object> LstArgs;

        public PublishEventArgs()
        {
            EventType = string.Empty;
            EventId = -1;
            EventAttachInfo = null;
            LstArgs = new List<object>();
        }

        /// <summary>
        /// <param >初始化为 string.Empty；</param>
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        ///  <param >初始化值为-1</param>
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// 事件其他需要携带的非参数的某些信息
        /// </summary>
        public object EventAttachInfo { get; set; }

        /// <summary>
        ///  <param >添加事件携带的参数，参数可无限制添加，如AddParams("fff", 8, 23, 555, Arg1)；</param>
        /// <param > 事件处理函数将获取到出入参数的一个List类型，其中参数排列与事件发布一致</param>
        /// </summary>
        /// <param name="parsObjects"></param>
        public void AddParams(params object[] parsObjects)
        {
            try
            {
                if (parsObjects == null) return;
                foreach (var t in parsObjects)
                    LstArgs.Add(t);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 获取事件携带的参数，数据顺序为加入时的顺序
        /// </summary>
        /// <returns>参数列表</returns>
        public List<object> GetParams()
        {
            if (LstArgs == null) return new List<object>();
            var lstReturn = new List<object>();
            foreach (var t in LstArgs) lstReturn.Add(t);
            return lstReturn;
        }
    }
}
