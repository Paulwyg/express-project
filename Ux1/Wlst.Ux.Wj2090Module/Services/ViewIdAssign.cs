using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj2090Module.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 60*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 60*100;
        

        //public const int SluInfoSetViewId = ViewIdAssignBaseId + 1;

        //public const string SluInfoSetViewAttachRegion =
        //    RegionNames.DocumentRegion;

        public const int TimeInfoSetViewId = ViewIdAssignBaseId + 2;

        public const int Wj2090SluInfoSetViewId = ViewIdAssignBaseId + 3;
   
        public const int NewDataViewId = ViewIdAssignBaseId + 4;
        public const string NewDataViewAttachRegion = RegionNames.DataRegion;



        public const int DataMiningViewId = ViewIdAssignBaseId + 5;
       

        public const int WeekSetQueryViewViewId = ViewIdAssignBaseId + 6;
       
        public const int Wj2090GrpManagerId = ViewIdAssignBaseId + 7;
        
        public const int Wj2090TreeViewId = ViewIdAssignBaseId + 8;
        public const string Wj2090TreeViewAttachRegion = RegionNames.LeftViewRegion;


        public const int Wj2090TreeSetViewId = ViewIdAssignBaseId + 9;
        
        public const int ZcSluTimeViewId = ViewIdAssignBaseId + 10;
       
        public const int ZcConnParasViewId = ViewIdAssignBaseId + 11;
        
        public const int SluPartolViewId = ViewIdAssignBaseId + 12;
        
        public const int ControlDataQueryViewId = ViewIdAssignBaseId + 13;
       
        public const int PartolsViewId = ViewIdAssignBaseId + 14;
        
        public const int OrderLargeViewId = ViewIdAssignBaseId + 15;
       
        public const int TimeInfoQueryViewId = ViewIdAssignBaseId + 16;
       




        public const int ZcConnArgsViewId = ViewIdAssignBaseId + 17;
       

        public const int ZcConnLocalArgsViewId = ViewIdAssignBaseId + 18;
        
        public const int PartolViewId = ViewIdAssignBaseId + 19;
       

        public const int Wj2090GrpManagerDelayId = ViewIdAssignBaseId + 20;
        
        public const int OrderLargeGrpViewId = ViewIdAssignBaseId + 21;

        public const int DataMining2ViewId = ViewIdAssignBaseId + 22;

        //MaxMinSluData查询界面  lvf 2018年11月15日09:45:10
        public const int DataSluAssisQuery = ViewIdAssignBaseId + 23;

    }
}
