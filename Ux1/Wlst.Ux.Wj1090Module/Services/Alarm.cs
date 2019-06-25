using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj1090Module.Services
{
    public class Alarm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fslagAlarm">上传数据 终端的值</param>
        /// <param name="fslagDetection">检测数据 设置的值</param>
        /// <returns>第一个值是否被盗，第二个值 是否短路</returns>
        public static Tuple<bool, bool> GetInfo(int fslagAlarm, int fslagDetection)
        {
            bool beidao = false;
            bool duanlu = false;
            string rtn = "正常";
            if ((fslagAlarm >> 3 & 1) == 1) //1 关灯 0 开灯
            {
                if ((fslagAlarm >> 6 & 1) == 1 && (fslagDetection >> 6 & 1) == 1)
                {
                    //if (tmp.Contains(42))
                    {
                        //   occrrors.Add(41);
                        duanlu = true;
                    }
                }

                if ((fslagDetection >> 4 & 1) == 1 && (fslagDetection >> 5 & 1) == 1)
                {
                    if (((fslagAlarm >> 4 & 1) == 1 && (fslagDetection >> 4 & 1) == 1) &&
                        ((fslagAlarm >> 5 & 1) == 1 && (fslagDetection >> 5 & 1) == 1))
                    {
                        // if (tmp.Contains(41))
                        {
                            // occrrors.Add(41);
                            beidao = true;
                        }
                    }
                }
                else
                {
                    if (((fslagAlarm >> 4 & 1) == 1 && (fslagDetection >> 4 & 1) == 1) ||
                        ((fslagAlarm >> 5 & 1) == 1 && (fslagDetection >> 5 & 1) == 1))
                    {
                        // if (tmp.Contains(41))
                        {
                            beidao = true;
                        }
                    }
                }
            }
            else
            {
                bool checkeded = false;
                bool bolerror = false;
                if ((fslagDetection & 1) == 1)
                {
                    checkeded = true;
                    if ((fslagAlarm & 1) == 0) bolerror = true;
                }
                if ((fslagDetection >> 1 & 1) == 1)
                {
                    checkeded = true;
                    if ((fslagAlarm >> 1 & 1) == 0) bolerror = true;
                }
                if ((fslagDetection >> 2 & 1) == 1)
                {
                    checkeded = true;
                    if ((fslagAlarm >> 2 & 1) == 0) bolerror = true;
                }

                if (bolerror == false && checkeded)
                {
                    // if (tmp.Contains(41))
                    {
                        beidao = true;
                    }
                }
            }
            return new Tuple<bool, bool>(beidao, duanlu);
        }

    }
}
