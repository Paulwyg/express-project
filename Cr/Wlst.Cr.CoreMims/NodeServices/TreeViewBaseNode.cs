using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using Wlst.Cr.CoreOne.CoreInterface;


namespace Wlst.Cr.CoreMims.NodeServices
{
    /// <summary>
    /// 界面显示数据 带RaisePropertyChanged事件的  
    /// </summary>
    [Serializable]
    public partial class TreeViewBaseNode : ObservableObjectInfo
    {

        #region  ForeGround

        /// <summary>
        /// 前景颜色 1
        /// </summary>
        public string ForeGround1B
        {
            get { return GetZstring(() => ForeGround1B); }
            set { SetZ(() => ForeGround1B, value); }
        }





        /// <summary>
        ///  前景颜色 2
        /// </summary>
        public string ForeGround2B
        {
            get { return GetZstring(() => ForeGround2B); }
            set { SetZ(() => ForeGround2B, value); }
        }


        /// <summary>
        ///  前景颜色 3
        /// </summary>
        public string ForeGround3B
        {
            get { return GetZstring(() => ForeGround3B); }
            set { SetZ(() => ForeGround3B, value); }
        }

        /// <summary>
        ///  前景颜色 4
        /// </summary>
        public string ForeGround4B
        {
            get { return GetZstring(() => ForeGround4B); }
            set { SetZ(() => ForeGround4B, value); }
        }

        #endregion

        #region BackGround

        /// <summary>
        /// 背景色1
        /// </summary>
        public string BackGround1B
        {
            get { return GetZstring(() => BackGround1B); }
            set { SetZ(() => BackGround1B, value); }
        }



        /// <summary>
        /// 背景色2
        /// </summary>
        public string BackGround2B
        {
            get { return GetZstring(() => BackGround2B); }
            set { SetZ(() => BackGround2B, value); }
        }


        /// <summary>
        /// 背景色3
        /// </summary>
        public string BackGround3B
        {
            get { return GetZstring(() => BackGround3B); }
            set { SetZ(() => BackGround3B, value); }
        }


        /// <summary>
        /// 背景色4
        /// </summary>
        public string BackGround4B
        {
            get { return GetZstring(() => BackGround4B); }
            set { SetZ(() => BackGround4B, value); }
        }

        #endregion

        #region ImagesIcon

        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon1B
        {
            get { return GetZobject(() => ImagesIcon1B); }
            set { SetZ(() => ImagesIcon1B, value); }
        }

        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon2B
        {
            get { return GetZobject(() => ImagesIcon2B); }
            set { SetZ(() => ImagesIcon2B, value); }
        }


        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon3B
        {
            get { return GetZobject(() => ImagesIcon3B); }
            set { SetZ(() => ImagesIcon3B, value); }
        }


        /// <summary>
        /// 图标1
        /// </summary>
        public object ImagesIcon4B
        {
            get { return GetZobject(() => ImagesIcon4B); }
            set { SetZ(() => ImagesIcon4B, value); }
        }

        #endregion

        #region Id

        /// <summary>
        /// 节点id  1
        /// </summary>
        public int Id1B
        {
            get { return GetZint(() => Id1B); }
            set { SetZ(() => Id1B, value); }
        }

        /// <summary>
        /// 节点id  2
        /// </summary>
        public int Id2B
        {
            get { return GetZint(() => Id2B); }
            set { SetZ(() => Id2B, value); }
        }

        /// <summary>
        /// 节点id  3
        /// </summary>
        public int Id3B
        {
            get { return GetZint(() => Id3B); }
            set { SetZ(() => Id3B, value); }
        }

        /// <summary>
        /// 节点id  4
        /// </summary>
        public int Id4B
        {
            get { return GetZint(() => Id4B); }
            set { SetZ(() => Id4B, value); }
        }

        #endregion

        #region NodeName

        /// <summary>
        /// 节点名称  1
        /// </summary>
        public string NodeName1B
        {
            get { return GetZstring(() => NodeName1B); }
            set { SetZ(() => NodeName1B, value); }
        }

        /// <summary>
        /// 节点名称  2
        /// </summary>
        public string NodeName2B
        {
            get { return GetZstring(() => NodeName2B); }
            set { SetZ(() => NodeName2B, value); }
        }

        /// <summary>
        /// 节点名称  3
        /// </summary>
        public string NodeName3B
        {
            get { return GetZstring(() => NodeName3B); }
            set { SetZ(() => NodeName3B, value); }
        }

        /// <summary>
        /// 节点名称  4
        /// </summary>
        public string NodeName4B
        {
            get { return GetZstring(() => NodeName4B); }
            set { SetZ(() => NodeName4B, value); }
        }

        #endregion

        #region Is

        /// <summary>
        /// 备用Is 1
        /// 
        /// </summary>
        public bool Is1B
        {
            get { return GetZbool(() => Is1B); }
            set { SetZ(() => Is1B, value); }
        }

        /// <summary>
        /// 备用Is 2
        /// 
        /// </summary>
        public bool Is2B
        {
            get { return GetZbool(() => Is2B); }
            set { SetZ(() => Is2B, value); }
        }


        /// <summary>
        /// 备用Is 3
        /// 
        /// </summary>
        public bool Is3B
        {
            get { return GetZbool(() => Is3B); }
            set { SetZ(() => Is3B, value); }
        }

        /// <summary>
        /// 备用Is 4
        /// 
        /// </summary>
        public bool Is4B
        {
            get { return GetZbool(() => Is4B); }
            set { SetZ(() => Is4B, value); }
        }

        /// <summary>
        /// 备用Is 
        /// 
        /// </summary>
        public bool IsB
        {
            get { return GetZbool(() => IsB); }
            set { SetZ(() => IsB, value); }
        }
        #endregion

        #region IsVisi
        /// <summary>
        /// 显示1
        /// </summary>
        public Visibility IsVisi1
        {
            get { return GetZVisibility(() => IsVisi1); }
            set { SetZ(() => IsVisi1, value); }
        }
        /// <summary>
        /// 显示2
        /// </summary>
        public Visibility IsVisi2
        {
            get { return GetZVisibility(() => IsVisi2); }
            set { SetZ(() => IsVisi2, value); }
        }
        /// <summary>
        /// 显示3
        /// </summary>
        public Visibility IsVisi3
        {
            get { return GetZVisibility(() => IsVisi3); }
            set { SetZ(() => IsVisi3, value); }
        }
        /// <summary>
        /// 显示4
        /// </summary>
        public Visibility IsVisi4
        {
            get { return GetZVisibility(() => IsVisi4); }
            set { SetZ(() => IsVisi4, value); }
        }

        #endregion

        #region IsEnable

        /// <summary>
        /// 可使用1
        /// </summary>
        public bool IsEnable1
        {
            get { return GetZbool(() => IsEnable1); }
            set { SetZ(() => IsEnable1, value); }
        }

        /// <summary>
        /// 可使用2
        /// </summary>
        public bool IsEnable2
        {
            get { return GetZbool(() => IsEnable2); }
            set { SetZ(() => IsEnable2, value); }
        }
        /// <summary>
        /// 可使用3
        /// </summary>
        public bool IsEnable3
        {
            get { return GetZbool(() => IsEnable3); }
            set { SetZ(() => IsEnable3, value); }
        }
        /// <summary>
        /// 可使用4
        /// </summary>
        public bool IsEnable4
        {
            get { return GetZbool(() => IsEnable4); }
            set { SetZ(() => IsEnable4, value); }
        }
        #endregion

        #region IsSelectedB,IsExpandedB




        /// <summary>
        /// 当前节点是否为系统当前选中节点  选中触发选中事件  
        /// 
        /// </summary>
        public bool IsSelected
        {
            get { return GetZbool(() => IsSelected); }
            set
            {
                SetZ(() => IsSelected, value);
                if (OnNodeSelected != null) OnNodeSelected(IdfN, this);
            }
        }

        /// <summary>
        /// 当前节点是否展开 触发展开事件
        /// </summary>
        public bool IsExpanded
        {
            get { return GetZbool(() => IsExpanded); }
            set
            {
                SetZ(() => IsExpanded, value);
                if (OnExpanded != null) OnExpanded(IdfN, this);
            }
        }

        #endregion

        /// <summary>
        ///  子节点  未设置则初始化
        /// </summary>
        public ObservableCollection<TreeViewBaseNode> ChildItems
        {
            get { return GetZcollection(() => ChildItems); }
            set { SetZ(() => ChildItems, value); }
        }




    }


    /// <summary>
    /// 节点识别数据  系统自动与设置的
    /// </summary>
    public partial class TreeViewBaseNode
    {
        private TreeViewBaseNode _fatherBaseNode;

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeViewBaseNode FatherNodeN
        {
            get { return _fatherBaseNode; }
        }


        /// <summary>
        /// 当选中的时候触发该事件
        /// </summary>
        private  Action<long, TreeViewBaseNode> OnNodeSelected = null;



         
        /// <summary>
        /// 当展开的时候触发该事件 正常可以不用
        /// </summary>
        private  Action<long, TreeViewBaseNode> OnExpanded = null;


        /// <summary>
        /// 提供末端节点不重要的  节点自动生成不重复的key
        /// </summary>
        private static int objint = 1;

        /// <summary>
        /// 提供末端节点不重要的 节点自动生成不重复的key
        /// </summary>
        /// <returns></returns>
        internal  static int GetIdxN()
        {
            return Interlocked.Increment(ref objint);
        }


        /// <summary>
        /// 唯一识别
        /// </summary>
        private long _idfx;

        /// <summary>
        /// 唯一识别,系统分配唯一标识此节点的序号
        /// </summary>
        public long IdfN
        {
            get { return _idfx; }
        }
    }


    /// <summary>
    /// 节点识别数据  关键字等
    /// </summary>
    public partial class TreeViewBaseNode
    {



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
        /// 
        /// </summary>
        /// <param name="fatherNode">父节点</param>
        /// <param name="onNodeSelected">当选中的时候触发该事件</param>
        /// <param name="onExpanded">当展开的时候触发该事件</param>
        public TreeViewBaseNode(TreeViewBaseNode fatherNode,
            Action<long, TreeViewBaseNode> onNodeSelected,
             
            Action<long, TreeViewBaseNode> onExpanded, int key1TypeN, int key2, int key3, int key4, int key5,
            int key6, int zindexN, bool isCollapsedWhenChildItemsEmptyN )
        {
            _idfx = GetIdxN();
            OnNodeSelected = onNodeSelected;
            OnExpanded = onExpanded;
            _fatherBaseNode = fatherNode;
            Key1TypeN = key1TypeN;
            Key2 = key2;
            Key3 = key3;
            Key4 = key4;
            Key5 = key5;
            Key6 = key6;


            ZindexN = zindexN;
            IsCollapsedWhenChildItemsEmptyN = isCollapsedWhenChildItemsEmptyN;
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
        /// 顺序排序 ，越小越排前 
        /// </summary>
        public int ZindexN;

        /// <summary>
        /// 如果节点下无子节点是否隐藏该节点  ，请注意此设置  默认为 false，必须组节点才能设置为true
        /// </summary>
        public bool IsCollapsedWhenChildItemsEmptyN;
    }

    /// <summary>
    /// 用户存储数据
    /// </summary>
    public partial class TreeViewBaseNode
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

        private ObservableCollection<IIMenuItem> items;

        public ObservableCollection<IIMenuItem> CmItems
        {
            get
            {
                if (items == null) items = new ObservableCollection<IIMenuItem>();
                return items;
            }
            set
            {
                if (value == items) return;
                items = value;
                this.RaisePropertyChanged(() => this.CmItems);
            }
        }

    }
}