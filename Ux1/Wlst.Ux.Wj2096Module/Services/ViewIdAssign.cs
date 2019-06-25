using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj2096Module.Services
{
   public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 75*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 75 * 100;

       public const int TreeTabVeiwId = ViewIdAssignBaseId + 1;
       public const string TreeTabVeiwAttachRegion = RegionNames.LeftViewRegion;

       public const int Wj2096TreeSetViewId = ViewIdAssignBaseId + 2; //选项设置

       public const int NewDataViewId = ViewIdAssignBaseId + 3;  //最新数据
       public const string NewDataViewAttachRegion = RegionNames.DataRegion;

       public const int Wj2096SluInfoSetViewId = ViewIdAssignBaseId + 4;  //参数设置

       public const int FieldGroupSetViewId = ViewIdAssignBaseId + 5;  //域分组

       public const int FieldCtrlGroupSetViewId = ViewIdAssignBaseId + 6;  //域控制器分组

       public const int NewDataGridViewId = ViewIdAssignBaseId + 7;  //巡测域数据

       public const int TimeInfoSetViewId = ViewIdAssignBaseId + 8;

       public const int TimeInfoQueryViewId = ViewIdAssignBaseId + 9;

       public const int ZcConnLocalArgsViewId = ViewIdAssignBaseId + 10;


       public const int ZcConnArgsViewId = ViewIdAssignBaseId + 11;

       //2018.9.10
       public const int SetYearTimeViewId = ViewIdAssignBaseId + 12;
    }
}
