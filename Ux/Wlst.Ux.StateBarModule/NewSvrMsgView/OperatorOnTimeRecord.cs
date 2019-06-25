//using System;
//using Wlst.Ux.StateBarModule.NewSvrMsgView.Services;

//namespace Wlst.Ux.StateBarModule.NewSvrMsgView
//{
//    [Serializable]
//    public class OperatorOnTimeRecord
//    {
//        //private static IIOperatorOnTimeRecords _rrecordss;

//        //private static IIOperatorOnTimeRecords Records
//        //{
//        //    get
//        //    {
//        //        if (_rrecordss == null)
//        //        {
//        //            try
//        //            {
//        //                _rrecordss = ServiceLocator.Current.GetInstance<IIOperatorOnTimeRecords>();
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
//        //            }
//        //        }
//        //        return _rrecordss;
//        //    }
//        //}


//        #region Singl

//        private static OperatorOnTimeRecord _myself;

//        public static OperatorOnTimeRecord MySelf
//        {
//            get
//            {
//                if (_myself == null) new OperatorOnTimeRecord();
//                return _myself;
//            }
//        }

//        private OperatorOnTimeRecord()
//        {
//            _myself = this;
//        }

//        #endregion


//        /// <summary>
//        /// 
//        /// </summary>
//        public Action<int, string, OperatrType, string> OnNewRecordAdded;

//        /// <summary>
//        /// 新增加新的用户操作信息
//        /// </summary>
//        /// <param name="rtuId">操作的终端地址</param>
//        /// <param name="rtuName">终端 </param>
//        /// <param name="operatr">用户操作还是服务器应答 </param>
//        /// <param name="operatorContent">执行情况 如 完成或 等待 </param>
//        public static void AddNewRecordItem(int rtuId, string rtuName, OperatrType operatr, string operatorContent)
//        {
//            if (OperatorOnTimeRecord.MySelf.OnNewRecordAdded != null)
//            {
//                OperatorOnTimeRecord.MySelf.OnNewRecordAdded(rtuId, rtuName, operatr, operatorContent);
//            }
//        }

//    }
//}
