//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Wlst.Ux.EquipmentDataQuery.EquipmentDailyDataQueryViewModel.Models
//{
//    /// <summary>
//    /// 终端设备最新数据；与服务器交互数据
//    /// </summary>
//    public class TmlNewData
//    {
//        /// <summary>
//        /// 终端地址
//        /// </summary>
//        public int RtuId;

//        /// <summary>
//        /// 电压
//        /// </summary>
//        public double RtuVoltageA;/// <summary>
//        /// 电压
//        /// </summary>
//        public double RtuVoltageB;/// <summary>
//        /// 电压
//        /// </summary>
//        public double RtuVoltageC;
//        /// <summary>
//        /// 电流
//        /// </summary>
//        public Double RtuCurrentSumA;/// <summary>
//        /// 电流
//        /// </summary>
//        public Double RtuCurrentSumB;/// <summary>
//        /// 电流
//        /// </summary>
//        public Double RtuCurrentSumC;

//        /// <summary>
//        /// 数据发生时间
//        /// </summary>
//        public DateTime DateCreate;

//        /// <summary>
//        /// 回路最新数据
//        /// 其回路信息存放结构为 NewDataforOneLoop
//        /// </summary>
//        public List<TmlNewDataforOneLoop> LstNewLoopsData;

//        ///// <summary>
//        ///// 开关量输入状态数据  是否每个开关量输入吸合； 默认36路依次为0~35；
//        ///// <para>false 开 ，true 吸合</para>
//        ///// </summary>
//        //public List<bool> IsSwithInAttraction;
//        /// <summary>
//        /// 开关量输出状态数据 是否每个开关量输出吸合连接 0~
//        /// </summary>
//        public List<bool> IsSwitchOutAttraction;


//        /// <summary>
//        /// 构造函数；需要知道终端地址
//        /// </summary>
//        /// <param name="rtuId"></param>
//        public TmlNewData(int rtuId)
//        {
//            RtuId = rtuId;
//            LstNewLoopsData = new List<TmlNewDataforOneLoop>();
//            IsSwitchOutAttraction = new List<bool>();
//        }

//        /// <summary>
//        /// 构造函数；需要知道终端地址
//        /// </summary>
//        public TmlNewData()
//        {
//            RtuId = 0;
//            LstNewLoopsData = new List<TmlNewDataforOneLoop>();
//            IsSwitchOutAttraction = new List<bool>();
//        }

//        public TmlNewData(TmlNewData tmlNewData)
//        {
//            this.DateCreate = tmlNewData.DateCreate;
//            this.RtuCurrentSumA = tmlNewData.RtuCurrentSumA;
//            this.RtuCurrentSumB = tmlNewData.RtuCurrentSumB;
//            this.RtuCurrentSumC = tmlNewData.RtuCurrentSumC;
//            this.RtuId = tmlNewData.RtuId;
//            this.RtuVoltageA = tmlNewData.RtuVoltageA;
//            this.RtuVoltageB = tmlNewData.RtuVoltageB;
//            this.RtuVoltageC = tmlNewData.RtuVoltageC;
//            foreach (var t in tmlNewData.LstNewLoopsData) this.LstNewLoopsData.Add(t);
//            foreach (var t in tmlNewData.IsSwitchOutAttraction) this.IsSwitchOutAttraction.Add(t);
//        }
//        ///// <summary>
//        ///// 克隆本类的实例 即创建了一个原对象的深表副本
//        ///// </summary>
//        ///// <returns></returns>
//        //public object Clone()
//        //{
//        //    using (MemoryStream ms = new MemoryStream())
//        //    {
//        //        object CloneObject;
//        //        BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
//        //        bf.Serialize(ms, this);
//        //        ms.Seek(0, SeekOrigin.Begin);
//        //        // 反序列化至另一个对象(即创建了一个原对象的深表副本) 
//        //        CloneObject = bf.Deserialize(ms);
//        //        // 关闭流 
//        //        ms.Close();
//        //        return CloneObject;
//        //    }
//        //}
//    };

//    public class TmlNewDataforOneLoop
//    {
//        public int LoopId;
//        /// <summary>
//        /// 仅此为string 如果为门的话 此处填写 开 或关；如果为回路的话 此处填写 电流 ~~~~
//        /// </summary>
//        public string V;
//        /// <summary>
//        /// 电压
//        /// </summary>
//        public double A;
//        /// <summary>
//        /// 功率
//        /// </summary>
//        public double Power;
//        /// <summary>
//        /// 功率因数
//        /// </summary>
//        public double PowerFactor;
//        /// <summary>
//        /// 亮灯率
//        /// </summary>
//        public double BrightRate;

//        /// <summary>
//        /// 开关量输入状态
//        /// </summary>
//        public bool SwitchInState;
//    }
//}
