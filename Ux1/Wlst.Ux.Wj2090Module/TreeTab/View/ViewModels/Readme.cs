using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.TreeTab.View.ViewModels
{
    /// <summary>
    /// 按照设备的地址划分：
    /// Key1Type           Key2   Key3                       
    /// 1、区域           areaid   0                           
    /// 2、集中器分组     areaid  grpid（-1,0，...）
    /// 3、集中器节点     sluid   0
    /// 4、控制器分组     sluid   ctrlgrp（-1,0，...）
    /// 5、控制器节点     sluid   ctrlid
    ///
    ///
    ///
    ///                Id1StoreN   Id2StoreN   Id3StoreN - - -
    /// 1、区域           0         0                           
    /// 2、集中器分组     0         0
    /// 3、集中器节点     areaid -  grpid（-1,0，...）- sluphyid - erridx
    /// 4、控制器分组     areaid -  grpid（-1,0，...）
    /// 5、控制器节点     areaid -  grpid（-1,0，...）- ctrlgrp（-1,0，...） - 物理地址 - lightcount- erroridx
    /// </summary>
    class Readme
    {

        // <summary>
        // 关键数据
        // Key1Type  1、区域    ，2、集中器分组                       ，3、集中器                       ，4、控制器分组                             ，5 、控制器
        // Key2      区域地址        区域地址                              集中器地址                        集中器地址                                   集中器地址
        // Key3      0               1-X、组，0、所有，-1 未划分分组       0                                 1-X、组，0、所有，-1 未划分分组              控制器地址
        //
        //
        // Id1StoreN                                                       区域地址                           区域地址                                     区域地址
        // Id2StoreN                                                       1-X、组，0、所有，-1 未划分分组    1-X、组，0、所有，-1 未划分分组              1-X、组，0、所有，-1 未划分分组
        // Id3StoreN                                                       物理地址                                                                        1-X、组，0、所有，-1 未划分分组（控制器分组）
        // Id4StoreN                                                       erroridx                                                                        物理地址 - lightcount- erroridx
        // </summary>
        //
        //




        //

        
    }
}
