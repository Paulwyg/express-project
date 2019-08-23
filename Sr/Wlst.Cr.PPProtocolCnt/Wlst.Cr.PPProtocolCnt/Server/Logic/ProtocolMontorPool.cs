using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Wlst.Cr.PPProtocolSvrCnt.Server.Logic
{
    /// <summary>
    /// 协议池
    /// </summary>
    internal class ProtocolMontorPool
    {

        #region single instances

        private static object obj = 1;
        private static ProtocolMontorPool _mysel = null;

        public static ProtocolMontorPool MySelf
        {
            get
            {
                if (_mysel == null)
                {
                    lock (obj)
                    {
                        if (_mysel == null) _mysel = new ProtocolMontorPool();
                    }
                }
                return _mysel;
            }
        }

        private ProtocolMontorPool()
        {

        }

        #endregion


        /// <summary>
        /// 协议保存主体  第一个是编码  
        /// 第二个参数协议数据
        /// </summary>
        private readonly ConcurrentDictionary<string, List<PoolData>> _dictionary =
            new ConcurrentDictionary<string, List<PoolData>>();


        public Tuple<int, int> GetKeysValues()
        {
            int xcount = 0;
            foreach (var f in _dictionary) xcount += f.Value.Count;
            return new Tuple<int, int>(_dictionary.Keys.Count, xcount);
        }

        public void AddProtocol(string key, PoolData action)
        {
            try
            {
                if (!_dictionary.ContainsKey(key))
                {
                    if (!_dictionary.TryAdd(key, new List<PoolData>())) _dictionary.TryAdd(key, new List<PoolData>());
                    _dictionary[key].Add(action);
                }
                else
                {
                    if (_dictionary[key].Count > 0)
                    {
                        if (_dictionary[key][0].DataType == action.DataType)
                        {
                            _dictionary[key].Add(action);
                        }
                    }
                    else
                    {
                        _dictionary[key].Add(action);
                    }

                    //  Common.WriteError.WriteLogError("ProtocolVersion2 addProtocol counter an error : key:" + key);
                }
            }
            catch (Exception ex)
            {
                Common.WriteError.WriteLogError("ProtocolVersion2 addProtocol counter an error : key:" + key);
            }
        }




        public void DeleteProtocol(string key, object instancesClassOfActionIn = null)
        {
            if (!_dictionary.ContainsKey(key)) return;
            if (instancesClassOfActionIn == null) return;

            {

                foreach (var f in _dictionary[key])
                {
                    if (f.InstancesClassOfActionIn == instancesClassOfActionIn)
                    {
                        PoolData pd;
                        _dictionary[key].Remove(f);
                        break;
                    }
                }
                if (_dictionary[key].Count == 0)
                {
                    List<PoolData> od;
                    _dictionary.TryRemove(key, out od);
                }
            }
        }


        public void DeleteProtocol(object instancesClassOfActionIn)
        {
            var lst = new List<string>();
            foreach (var f in _dictionary)
            {
                foreach (var g in f.Value)
                {
                    if (g.InstancesClassOfActionIn == instancesClassOfActionIn)
                    {
                        lst.Add(f.Key);
                        break;
                    }
                }
            }

            foreach (var f in lst) DeleteProtocol(f, instancesClassOfActionIn);


        }





        /// <summary>
        /// 根据接收关键字获取 承载该数据data的类型 Func  无法查找到则返回null
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无法查找到则返回null</returns>
        public List<PoolData> GetAction(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            return null;
        }



        ///// <summary>
        ///// 根据接收关键字获取 承载该数据data的类型 Func  无法查找到则返回null
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns>无法查找到则返回null</returns>
        //public PoolData GetIsCheckSessionLoad(string key)
        //{
        //    if (_dictionary.ContainsKey(key))
        //    {
        //        return _dictionary[key];
        //    }
        //    return null;
        //}
    }
}
