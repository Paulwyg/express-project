using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipmentNewData.InfoHold;
using Wlst.Sr.EquipmentNewData.Model;

namespace Wlst.Sr.EquipmentNewData.Services
{
    public  class LduNewDataServices
    {
        private static LduNewDataInfoHold _info = new LduNewDataInfoHold();

        public LduNewDataServices()
        {
            _info.InitStartService();
        }


        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public static Dictionary<int, Dictionary<int, LduNewData>> InfoDictionary
        {
            get { return _info.InfoDictionary; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 根据设备逻辑地址获取设备信息；不存在返回null
        /// </summary>
        /// <param name="id">设备地址 终端地址</param>
        /// <returns>不存在返回null</returns>
        public static LduNewData GetInfoById(int rtuId, int lineLoopId)
        {
            return _info.GetInfoById(rtuId, lineLoopId);
        }

        /// <summary>
        /// <para>该集中控制器下的线路地址和数据</para>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  </para>
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public static  Dictionary<int, LduNewData> GetInfoById(int rtuId)
        {
            return _info.GetInfoById(rtuId);
        }


        ///// <summary>
        ///// 通过输入照明终端地址来获取该终端绑定的防盗状态信息
        ///// 回路 状态  1正常，2 被盗 3 短路 4 被盗与短路
        ///// </summary>
        ///// <param name="rtu"></param>
        ///// <returns></returns>
        //public static List<Tuple<int, int>> GetRtuLoopLduInfo(int rtu)
        //{
        //    var infos = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(rtu);

        //    var lst = new List<Tuple<int, int, int>>(); //终端回路，集中控制器地址，控制器地址序号
        //    foreach (var t in infos)
        //    {
        //        var tmp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetAttachEquipmentInfo(t);
        //        if (tmp == null) continue;

        //        var info = tmp as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
        //        if (info == null) continue;
        //        foreach (var tt in info.LduLines)
        //        {
        //            var tst = new Tuple<int, int, int>(tt.LduLoopID, tmp.RtuId, tt.LduLineID);
        //            if (!lst.Contains(tst)) lst.Add(tst);
        //        }
        //    }


        //    var lstReturn = new List<Tuple<int, int>>();
        //    foreach (var t in lst)
        //    {
        //        var tmp = LduNewDataServices.GetInfoById(t.Item2, t.Item3);
        //        if (tmp != null)
        //        {
        //            var index = 1;
        //            if (tmp.IsShortCircuit && tmp.IsStolen) index = 4;
        //            else if (tmp.IsShortCircuit) index = 3;
        //            else if (tmp.IsStolen) index = 2;
        //            lstReturn.Add(new Tuple<int, int>(t.Item1, index));
        //        }
        //    }
        //    return lstReturn;
        //}
    }
}
