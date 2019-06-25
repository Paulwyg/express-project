using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
 

namespace Wlst.Cr.CoreMims.NodeServices
{
    /// <summary>
    /// 诸多显示数据
    /// </summary>
    public partial class InputInfo
    {

        #region  ForeGround

        /// <summary>
        /// 前景颜色 1
        /// </summary>
        public string ForeGround1B;


        /// <summary>
        ///  前景颜色 2
        /// </summary>
        public string ForeGround2B;


        /// <summary>
        ///  前景颜色 3
        /// </summary>
        public string ForeGround3B;

        /// <summary>
        ///  前景颜色 4
        /// </summary>
        public string ForeGround4B;

        #endregion

        #region BackGround

        /// <summary>
        /// 背景色1
        /// </summary>
        public string BackGround1B;



        /// <summary>
        /// 背景色2
        /// </summary>
        public string BackGround2B;


        /// <summary>
        /// 背景色3
        /// </summary>
        public string BackGround3B;


        /// <summary>
        /// 背景色4
        /// </summary>
        public string BackGround4B;

        #endregion

        #region ImagesIcon

        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon1B;

        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon2B;


        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon3B;


        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon4B;

        #endregion

        #region Id

        /// <summary>
        /// 节点id  1
        /// </summary>
        public int Id1B;

        /// <summary>
        /// 节点id  2
        /// </summary>
        public int Id2B;

        /// <summary>
        /// 节点id  3
        /// </summary>
        public int Id3B;

        /// <summary>
        /// 节点id  4
        /// </summary>
        public int Id4B;

        #endregion

        #region NodeName

        /// <summary>
        /// 节点名称  1
        /// </summary>
        public string NodeName1B;

        /// <summary>
        /// 节点名称  2
        /// </summary>
        public string NodeName2B;

        /// <summary>
        /// 节点名称  3
        /// </summary>
        public string NodeName3B;

        /// <summary>
        /// 节点名称  4
        /// </summary>
        public string NodeName4B;

        #endregion

        #region Is

        /// <summary>
        /// 备用Is 1
        /// 
        /// </summary>
        public bool Is1B;

        /// <summary>
        /// 备用Is 2
        /// 
        /// </summary>
        public bool Is2B;


        /// <summary>
        /// 备用Is 3
        /// 
        /// </summary>
        public bool Is3B;

        /// <summary>
        /// 备用Is 4
        /// 
        /// </summary>
        public bool Is4B;

        /// <summary>
        /// 备用Is 
        /// 
        /// </summary>
        public bool IsB;
        #endregion

        #region IsVisi
        /// <summary>
        /// 显示1
        /// </summary>
        public Visibility IsVisi1;
        /// <summary>
        /// 显示2
        /// </summary>
        public Visibility IsVisi2;
        /// <summary>
        /// 显示3
        /// </summary>
        public Visibility IsVisi3;
        /// <summary>
        /// 显示4
        /// </summary>
        public Visibility IsVisi4;

        #endregion

        #region IsEnable

        /// <summary>
        /// 可使用1
        /// </summary>
        public bool IsEnable1;
        /// <summary>
        /// 可使用2
        /// </summary>
        public bool IsEnable2;
        /// <summary>
        /// 可使用3
        /// </summary>
        public bool IsEnable3;
        /// <summary>
        /// 可使用4
        /// </summary>
        public bool IsEnable4;
        #endregion
    }

    /// <summary>
    /// 用户存储数据
    /// </summary>
    public partial class InputInfo
    {

        /// <summary>
        /// int存储
        /// </summary>
        public int Id1StoreN;

        public int Id2StoreN;
        public int Id3StoreN;
        public int Id4StoreN;
        public int Id5StoreN;
        public int Id6StoreN;
        public int Id7StoreN;
        public int Id8StoreN;


        /// <summary>
        /// long存储
        /// </summary>
        public long Ld1StoreN;

        public long Ld2StoreN;
        public long Ld3StoreN;
        public long Ld4StoreN;


        /// <summary>
        /// string存储
        /// </summary>
        public string  Str1StoreN;

        public string Str2StoreN;
        public string Str3StoreN;
        public string Str4StoreN;



    }

    /// <summary>
    /// 节点数据  构造函数
    /// </summary>
    public partial class InputInfo
    {
        /// <summary>
        /// 父节点信息 必须设置FatherInputInfo或者FatherTreeNodeInfo 中的一个  ， 都不设置 则为根节点
        /// </summary>
        public InputInfo FatherInputInfo = null;

        /// <summary>
        /// 父节点信息   必须设置FatherInputInfo或者FatherTreeNodeInfo 中的一个  ， 都不设置 则为根节点
        /// </summary>
        public TreeViewBaseNode FatherTreeNodeInfo = null;


        /// <summary>
        /// 节点类型 必须作为关键字 Key
        /// </summary>
        public int Key1TypeN;

        public int Key2;

        public int Key3;

        public int Key4;

        public int Key5;

        public int Key6;

        /// <summary>
        /// 顺序排序 ，越小越排前 
        /// </summary>
        public int ZindexN;

        /// <summary>
        /// 初始化节点信息你
        /// </summary>
        /// <param name="fatherInputInfo">父节点信息 必须设置FatherInputInfo或者FatherTreeNodeInfo 中的一个  ， 都不设置在构建树时 则为根节点 （树已经存在，更新节点信息不需要）</param>
        /// <param name="fatherTreeNodeInfo">父节点信息   必须设置FatherInputInfo或者FatherTreeNodeInfo 中的一个  ， 都不设置在构建树时 则为根节点 （树已经存在，更新节点信息不需要）</param>
        /// <param name="zindexN">顺序排序 ，越小越排前 </param>
        /// <param name="key1TypeN">节点类型 必须作为关键字 Key</param>
        /// <param name="key2"></param>
        /// <param name="key3"></param>
        /// <param name="key4"></param>
        /// <param name="key5"></param>
        /// <param name="key6"></param>
        public InputInfo(InputInfo fatherInputInfo, TreeViewBaseNode fatherTreeNodeInfo, int zindexN, int key1TypeN,
            int key2, int key3 = 0, int key4 = 0, int key5 = 0,
            int key6 = 0)
        {
            FatherInputInfo = fatherInputInfo;
            FatherTreeNodeInfo = fatherTreeNodeInfo;
            ZindexN = zindexN;
            Key1TypeN = key1TypeN;
            Key2 = key2;
            Key3 = key3;
            Key4 = key4;
            Key5 = key5;
            Key6 = key6;

            Idfn = TreeViewBaseNode.GetIdxN();
        }


        /// <summary>
        /// 获取节点复合节点关键字值
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int, int, int, int, int> GetKey()
        {
            return new Tuple<int, int, int, int, int, int>(Key1TypeN, Key2, Key3, Key4, Key5, Key6);
        }

        /// <summary>
        /// 唯一序号
        /// </summary>
        private long Idfn = 0;

        /// <summary>
        /// 获取能唯一识别本类的地址
        /// </summary>
        /// <returns></returns>
        public long GetIdf()
        {
            return Idfn;
        }

    }




}
