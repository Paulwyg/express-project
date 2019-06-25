using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.EquipemntLightFault.Model
{
    /// <summary>
    /// 数据持有
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InfoDictionaryBase<T> where T : new()
    {
        /// <summary>
        /// 提供数据持有的数据结构
        /// </summary>
        protected ConcurrentDictionary<int, T> Info;

        /// <summary>
        /// 构造函数；执行事件注册
        /// </summary>
        public InfoDictionaryBase()
        {
            Info = new ConcurrentDictionary<int, T>();
        }

        /// <summary>
        /// <para>获取升序排列的列表 </para> 
        /// </summary>
        public ConcurrentDictionary<int, T> InfoDictionary
        {
            get { return Info; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 根据关键字信息；不存在返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回null</returns>
        public virtual  T GetInfoById(int id)
        {
            return Info.ContainsKey(id) ? Info[id] : default(T);
        }


        /// <summary>
        /// <para>获取升序排列的设备列表</para>
        /// </summary>
        public virtual  List<T> GetInfoList
        {
            get { return (from pair in Info orderby pair.Key select pair.Value).ToList(); }
        }
    }
}
