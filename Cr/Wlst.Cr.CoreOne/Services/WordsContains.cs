using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreOne.Services
{
   public  class WordsContains
    {    /// <summary>
        /// 前者是否包含后者数据 字符串是否包含  以及转化为拼音第一个字母是否包含
        /// </summary>
        /// <param name="containerStinng"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public  static  bool StringContainKeyword(string containerStinng, string keyword)
    {
        return Wlst.Cr.Coreb.Servers.WordsContains.StringContainKeyword(containerStinng, keyword);
    }


        /// <summary>
        /// 返回汉字字符串的拼音的首字母  如：我是谁 输入为：wss
        /// </summary>
        /// <param name="chinesestr">要转换的字符串</param>
        /// <returns></returns>
        public static string Chinesecap(string chinesestr)
        {
            return Wlst.Cr.Coreb.Servers.WordsContains.Chinesecap(chinesestr);
        }

       /// <summary>
       /// 通过输入数字 输出中文匹配的母  
       /// </summary>
       /// <param name="chinesestrInt"></param>
       /// <returns></returns>
        public static string GetChinesefirst(long chinesestrInt)
        {
            return Wlst.Cr.Coreb.Servers.WordsContains.GetChinesefirst(chinesestrInt);
        }

    }
}
