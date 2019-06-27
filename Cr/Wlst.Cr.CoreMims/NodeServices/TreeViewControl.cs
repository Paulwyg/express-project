using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace Wlst.Cr.CoreMims.NodeServices
{
    public partial class TreeViewControl : ObservableObjectInfo
    {

        /// <summary>
        ///  子节点  未设置则初始化
        /// </summary>
        public ObservableCollection<TreeViewBaseNode> ChildItems
        {
            get { return GetZcollection(() => ChildItems); }
            set { SetZ(() => ChildItems, value); }
        }


        /// <summary>
        /// 可视化树对应的 的字典  ，还有一个可不见的字段 无子节点的节点字典
        /// </summary>
        public Dictionary<Tuple<int, int, int, int, int, int>, List<TreeViewBaseNode>> DicItems =
            new Dictionary<Tuple<int, int, int, int, int, int>, List<TreeViewBaseNode>>();

        /// <summary>
        /// 组无设备 隐藏的组
        /// </summary>
        private Dictionary<Tuple<int, int, int, int, int, int>, List<TreeViewBaseNode>> DicHideItems =
            new Dictionary<Tuple<int, int, int, int, int, int>, List<TreeViewBaseNode>>();

        #region  树节点的 属性预设置

        protected  Dictionary<int, bool> _dicIsCollapsedWhenChildItemsEmptyN = new Dictionary<int, bool>();

        /// <summary>
        /// 预先设置该类型的节点是否在 节点下无其他子节点的情况下 隐藏,默认false 全部显示 
        /// </summary>
        /// <param name="keyTypeN"></param>
        /// <param name="isCollapsedWhenChildItemsEmptyN"></param>
        public void SetIsCollapsedWhenChildItemsEmptyNByKeyType(int keyTypeN, bool isCollapsedWhenChildItemsEmptyN)
        {
            if (_dicIsCollapsedWhenChildItemsEmptyN.ContainsKey(keyTypeN))
                _dicIsCollapsedWhenChildItemsEmptyN[keyTypeN] = isCollapsedWhenChildItemsEmptyN;
            else _dicIsCollapsedWhenChildItemsEmptyN.Add(keyTypeN, isCollapsedWhenChildItemsEmptyN);
        }


        /// <summary>
        /// 当树节点中的   IsSelected 属性变化的时候 调用
        /// </summary>
        protected  Action<long, TreeViewBaseNode> OnNodeSelected = null;

        /// <summary>
        /// 当树节点中的   IsExpanded 属性变化的时候 调用
        /// </summary>
        protected Action<long, TreeViewBaseNode> OnNodeExpanded = null;
  
        #endregion

        /// <summary>
        /// 返回查找的  ，不存在返回 空集合
        /// </summary>
        /// <param name="key1TypeN"></param>
        /// <param name="key2"></param>
        /// <param name="key3"></param>
        /// <param name="key4"></param>
        /// <param name="key5"></param>
        /// <param name="key6"></param>
        /// <returns></returns>
        public List<TreeViewBaseNode> GetNodeByKey(int key1TypeN, int key2, int key3 = 0, int key4 = 0, int key5 = 0,
            int key6 = 0)
        {
            var t = new Tuple<int, int, int, int, int, int>(key1TypeN, key2, key3, key4, key5, key6);
            if (DicItems.ContainsKey(t)) return DicItems[t];
            if (DicHideItems.ContainsKey(t)) return DicHideItems[t];
            return new List<TreeViewBaseNode>();
        }

        

        public TreeViewControl(Action<long, TreeViewBaseNode> onNodeSelected,
            Action<long, TreeViewBaseNode> onExpanded = null )
        {
            OnNodeSelected = onNodeSelected;
            OnNodeExpanded = onExpanded;
        }

    }

    public partial class TreeViewControl
    {
        public Dictionary<long, TreeViewBaseNode> DicIdfNs = new Dictionary<long, TreeViewBaseNode>();

        /// <summary>
        /// 当DicItems，DicHideItems 内容变化的还是 重新加载 DicIdfNs的数据 ，DicIdfNs数据是上诉2个集合的合集
        /// </summary>
        private void ReLoadIdfn()
        {
            DicIdfNs.Clear();
            foreach (var f in DicItems)
            {
                foreach (var l in f.Value)
                {
                    if (DicIdfNs.ContainsKey(l.IdfN) == false) DicIdfNs.Add(l.IdfN, l);
                }
            }

            foreach (var f in DicHideItems)
            {
                foreach (var l in f.Value)
                {
                    if (DicIdfNs.ContainsKey(l.IdfN) == false) DicIdfNs.Add(l.IdfN, l);
                }
            }
        }

        #region  初始化显示树  输入List<InputInfo> data

        /// <summary>
        /// 初始化显示树，
        /// 排序按照给定的顺序进行排序，
        /// 返回 添加失败的节点 即该节点父节点不在树中呈现的
        /// </summary>
        /// <param name="data"></param>
        public List<InputInfo> InitNode(List<InputInfo> data)
        {
            DicHideItems.Clear();
            DicItems.Clear();
            ChildItems.Clear();


            var root = (from t in data
                where t!=null && t.FatherTreeNodeInfo == null && t.FatherInputInfo == null
                orderby t.ZindexN ascending
                select t).ToList();

            //将数据转换为字典 加速后续呈现
            var dictmp = new Dictionary<long , List<InputInfo>>();
            
            foreach (var f in data)
            {
                if (f.FatherInputInfo == null && f.FatherTreeNodeInfo == null) continue;
                if (f.FatherInputInfo != null)
                {
                    if (dictmp.ContainsKey(f.FatherInputInfo.GetIdf()) == false)
                        dictmp.Add(f.FatherInputInfo.GetIdf(), new List<InputInfo>());
                    dictmp[f.FatherInputInfo.GetIdf()].Add(f);

                }
                //else if (f.FatherTreeNodeInfo != null)
                //{
                //    if (dictmp.ContainsKey(f.FatherTreeNodeInfo.IdfN) == false)
                //        dictmp.Add(f.FatherTreeNodeInfo.IdfN, new List<InputInfo>());
                //    dictmp[f.FatherTreeNodeInfo.IdfN].Add(f);

                //    if (dicfather.ContainsKey(f.FatherTreeNodeInfo.IdfN) == false)
                //        dicfather.Add(f.FatherTreeNodeInfo.IdfN,
                //            new Tuple<InputInfo, TreeViewBaseNode>(null, f.FatherTreeNodeInfo));
                //}
            }


            foreach (var l in root)
            {
                var c1 = GetNewTreeViewBaseNode(null, l);

                InitNode(ref c1,l.GetIdf() ,  dictmp );
                //组下无节点 则删除
                if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
                {
                    if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                        DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicHideItems[c1.GetKey()].Add(c1);
                }
                else
                {
                    if (DicItems.ContainsKey(c1.GetKey()) == false)
                        DicItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicItems[c1.GetKey()].Add(c1);

                    ChildItems.Add(c1);
                }

            }

            ReLoadIdfn();
            return (from t in data
                where DicItems.ContainsKey(t.GetKey()) == false && DicHideItems.ContainsKey(t.GetKey()) == false
                select t).ToList();
        }


        private void InitNode(ref TreeViewBaseNode father, long fatheridf,
            Dictionary<long, List<InputInfo>> data)
        {
            if (father == null) return;
            // var keyfa = father.GetKey();
            if (data.ContainsKey(fatheridf) == false) return;

            var childs = (from t in data[fatheridf] orderby t.ZindexN ascending select t).ToList();

            foreach (var l in childs)
            {
                var c1 = GetNewTreeViewBaseNode(father, l);
                if (data.ContainsKey(l.GetIdf()))
                    InitNode(ref c1, l.GetIdf(), data);



                //组下无节点 则删除
                if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
                {
                    if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                        DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicHideItems[c1.GetKey()].Add(c1);
                }
                else
                {
                    if (DicItems.ContainsKey(c1.GetKey()) == false)
                        DicItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicItems[c1.GetKey()].Add(c1);

                    father.ChildItems.Add(c1);
                }


            }
        }

        #endregion


        #region 删除节点 List<long> idfs

        /// <summary>
        /// 删除节点    提供通过唯一识别码来删除 ，不提供通过KEY的
        /// </summary>
        /// <param name="idfs"></param>
        public void DeleteNode(List<long> idfs)
        {
            //提取需要删除的节点信息 ,明面上的  即通过idfs知道的  
            var lstDlt = new List<TreeViewBaseNode>();
            foreach (var l in idfs)
            {
                if (DicIdfNs.ContainsKey(l))
                {
                    lstDlt.Add(DicIdfNs[l]);
                }
            }

            //提取需要删的节点信息，通过idfs递归查询的所有的需要删除的节点信息
            var lstDltAll = new List<TreeViewBaseNode>();
            foreach (var l in lstDlt)
            {
                GetSelfAndChildItems(l, ref lstDltAll);
            }

            //所有删除了子节点的父节点 信息
            var lstFatherNode = new List<TreeViewBaseNode>();
            foreach (var l in lstDltAll)
            {
                //显示树的 obsvr删除本节点
                if (l.FatherNodeN == null && ChildItems.Contains(l))
                {
                    ChildItems.Remove(l);
                }

                //显示树的 obsvr删除本节点
                if (l.FatherNodeN != null && l.FatherNodeN.ChildItems.Contains(l))
                {
                    l.FatherNodeN.ChildItems.Remove(l);
                    lstFatherNode.Add(l.FatherNodeN);
                }

                //显示字典删除
                if (DicItems.ContainsKey(l.GetKey()) && DicItems[l.GetKey()].Contains(l))
                {
                    DicItems[l.GetKey()].Remove(l);
                    if (DicItems[l.GetKey()].Count == 0) DicItems.Remove(l.GetKey());
                }


                //隐藏字典删除
                if (DicHideItems.ContainsKey(l.GetKey()) && DicHideItems[l.GetKey()].Contains(l))
                {
                    DicHideItems[l.GetKey()].Remove(l);
                    if (DicHideItems[l.GetKey()].Count == 0) DicHideItems.Remove(l.GetKey());
                }

                //idfn字典删除
                if (DicIdfNs.ContainsKey(l.IdfN))
                {
                    DicIdfNs.Remove(l.IdfN);
                }
            }

            //移动空组
            foreach (var l in lstFatherNode)
            {
                var father = l.FatherNodeN;
                CheckVisiAndHidforChild(l);
                CheckVisiAndHidforFather(father);
            }
        }

        /// <summary>
        /// 递归检查节点下的节点是否需要移走
        /// </summary>
        /// <param name="c1"></param>
        private void CheckVisiAndHidforChild(TreeViewBaseNode c1)
        {
            if (c1 == null) return;
           // if (c1.IsCollapsedWhenChildItemsEmptyN == false) return;

            if (c1.ChildItems.Count > 0)
            {
                foreach (var l in c1.ChildItems)
                    CheckVisiAndHidforChild(l);
            }


            //组下无节点 则删除
            if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
            {
                //显示节点字典删除
                if (DicItems.ContainsKey(c1.GetKey()) && DicItems[c1.GetKey()].Contains(c1))
                {
                    DicItems[c1.GetKey()].Remove(c1);
                }

                //隐藏节点增加
                if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                    DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                DicHideItems[c1.GetKey()].Add(c1);


                //显示树的 obsvr删除本节点
                if (c1.FatherNodeN == null && ChildItems.Contains(c1))
                {
                    ChildItems.Remove(c1);
                }

                //显示树的 obsvr删除本节点
                if (c1.FatherNodeN != null && c1.FatherNodeN.ChildItems.Contains(c1))
                {
                    c1.FatherNodeN.ChildItems.Remove(c1);
                }
            }
        }

        /// <summary>
        /// 递归检查节点下的节点是否需要移走
        /// </summary>
        /// <param name="c1"></param>
        private void CheckVisiAndHidforFather(TreeViewBaseNode c1)
        {
            if (c1 == null) return;
            //if (c1.IsCollapsedWhenChildItemsEmptyN == false) return;

            //组下无节点 则删除
            if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
            {
                //显示节点字典删除
                if (DicItems.ContainsKey(c1.GetKey()) && DicItems[c1.GetKey()].Contains(c1))
                {
                    DicItems[c1.GetKey()].Remove(c1);
                }

                //隐藏节点增加
                if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                    DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                DicHideItems[c1.GetKey()].Add(c1);



                var father = c1.FatherNodeN;

                //显示树的 obsvr删除本节点
                if (c1.FatherNodeN == null && ChildItems.Contains(c1))
                {
                    ChildItems.Remove(c1);
                }

                //显示树的 obsvr删除本节点
                if (c1.FatherNodeN != null && c1.FatherNodeN.ChildItems.Contains(c1))
                {
                    c1.FatherNodeN.ChildItems.Remove(c1);
                }

                if (father != null) CheckVisiAndHidforFather(father);

            }
        }


        /// <summary>
        /// 递归获取 某一个节点以及下的所有的节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rtn"></param>
        private void GetSelfAndChildItems(TreeViewBaseNode key, ref List<TreeViewBaseNode> rtn)
        {
            rtn.Add(key);

            foreach (var f in key.ChildItems)
            {
                GetSelfAndChildItems(f, ref rtn);
            }
        }

        private void GetSelfAndChildItemsIdf(TreeViewBaseNode key, ref List<long> rtn)
        {
            rtn.Add(key.IdfN);

            foreach (var f in key.ChildItems)
            {
                GetSelfAndChildItemsIdf(f, ref rtn);
            }
        }

        #endregion


        #region AddNodeTofather(TreeViewBaseNode f, ObservableCollection<TreeViewBaseNode> childs)

        /// <summary>
        /// 动态添加节点到指定的父节点中去 ,同时添加更新到  DicItems
        /// </summary>
        /// <param name="f"></param>
        /// <param name="childs"></param>
        private void AddNodeTofather(TreeViewBaseNode f, ObservableCollection<TreeViewBaseNode> childs)
        {

            //插入
            bool added = false;
            for (int j = 0; j < childs.Count; j++)
            {
                if (f.ZindexN < 100) //插入第一个 
                {
                    if (f.ZindexN <= childs[j].ZindexN)
                    {
                        childs.Insert(j, f);
                        added = true;
                        break;
                    }
                }
                else //插入最后一个
                {
                    if (f.ZindexN < childs[j].ZindexN)
                    {
                        childs.Insert(j, f);
                        added = true;
                        break;
                    }
                }
            }

            //序号最大  最后写入
            if (added == false) childs.Add(f);
        }


        /// <summary>
        /// 更新数据  更新idfN下的所有节点信息  ，动态更新 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idfN"></param>
        public void UpdateNode(long idfN, List<InputInfo> data)
        {

            if (DicIdfNs.ContainsKey(idfN) == false) return;
            var key = DicIdfNs[idfN].GetKey();
            //若父节点在 隐藏字典中 ，需将父节点移动到显示树与显示字典中
            if (DicHideItems.ContainsKey(key))
            {
                for (int j = DicHideItems[key].Count - 1; j >= 0; j--)
                {
                    if (DicHideItems[key][j].IdfN == idfN)
                    {
                        var d = DicHideItems[key][j];
                        //移出隐藏字典
                        DicHideItems[key].RemoveAt(j);
                        if (DicHideItems[key].Count == 0) DicHideItems.Remove(key);

                        //添加到可视化字典
                        if (DicItems.ContainsKey(key) == false) DicItems.Add(key, new List<TreeViewBaseNode>());
                        DicItems[key].Add(d);

                        //在树上显示
                        if (d.FatherNodeN == null) AddNodeTofather(d, ChildItems);
                        else AddNodeTofather(d, d.FatherNodeN.ChildItems);
                        break;
                    }
                }
            }

            if (DicItems.ContainsKey(key) == false) return;
            //查找父节点 
            TreeViewBaseNode father = null;
            foreach (var l in DicItems[key])
            {
                if (l.IdfN == idfN)
                {
                    father = l;
                    break;
                }
            }

            if (father == null) return;
            //执行动态更新 子节点

            var dic = new Dictionary<Tuple<int, int, int, int, int, int>, List<InputInfo>>();
            foreach (var l in data)
            {
                Tuple<int, int, int, int, int, int> keyx = null;
                if (l.FatherInputInfo != null) keyx = l.FatherInputInfo.GetKey();
                else if (l.FatherTreeNodeInfo != null) keyx = l.FatherTreeNodeInfo.GetKey();
                if (keyx == null) continue;

                if (dic.ContainsKey(keyx) == false) dic.Add(keyx, new List<InputInfo>());
                dic[keyx].Add(l);
            }

            List<long> dltLongs = new List<long>();
            UpdateNode(father, dic, ref dltLongs); //更新子节点信息
            ReLoadIdfn(); //重新加载 idfn  ，因为添加进去的 点的idfn未加载进去  
            DeleteNode(dltLongs); //执行删除计划

            CheckVisiAndHidforChild(father);

        }

        /// <summary>
        /// 执行动态更新
        /// </summary>
        /// <param name="fatherNode">一定在可视化树中且在可视化字典中的</param>
        /// <param name="data">数据均为该节点下的数据</param>
        /// <param name="dltIdfs"></param>
        private void UpdateNode(TreeViewBaseNode fatherNode,
            Dictionary<Tuple<int, int, int, int, int, int>, List<InputInfo>> data, ref List<long> dltIdfs)
        {


            if (data.ContainsKey(fatherNode.GetKey()) == false)
            {
                if (fatherNode.ChildItems.Count > 0)
                {
                    GetSelfAndChildItemsIdf(fatherNode, ref dltIdfs);
                }

                if (dltIdfs.Contains(fatherNode.IdfN)) dltIdfs.Remove(fatherNode.IdfN);
                return;
            }

            //获取本节点下已经不存在的  清单
            var keys = (from t in data[fatherNode.GetKey()] select t.GetKey()).ToList();
            foreach (var l in fatherNode.ChildItems)
            {
                if (keys.Contains(l.GetKey())) continue;
                dltIdfs.Add(l.IdfN);
            }

            //更新节点信息
            foreach (var l in fatherNode.ChildItems)
            {

                foreach (var lx in data[fatherNode.GetKey()])
                {
                    if (lx.GetKey().Equals(l.GetKey()))
                    {
                        UpdateTreeViewNode(l, lx);
                        UpdateNode(l, data, ref dltIdfs);
                        break;
                    }
                }
            }

            //添加新增的清单
            keys = (from t in fatherNode.ChildItems select t.GetKey()).ToList();
            foreach (var l in data[fatherNode.GetKey()])
            {
                if (keys.Contains(l.GetKey())) continue;

                TreeViewBaseNode c1 = null;
                if (DicHideItems.ContainsKey(l.GetKey()))
                {
                    foreach (var lx in DicHideItems[l.GetKey()])
                    {
                        if (lx.FatherNodeN != null && lx.FatherNodeN.GetKey().Equals(fatherNode.GetKey()))
                        {
                            //查找隐藏字典中存在的数据  并移出
                            c1 = lx;
                            DicHideItems[l.GetKey()].Remove(lx);
                            break;
                        }
                    }
                }

                if (c1 == null)
                    c1 = GetNewTreeViewBaseNode(fatherNode, l);
                if (data.ContainsKey(c1.GetKey()))
                    DoInit(ref c1, data);


                //组下无节点 则删除
                if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
                {
                    if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                        DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicHideItems[c1.GetKey()].Add(c1);
                }
                else
                {
                    if (DicItems.ContainsKey(c1.GetKey()) == false)
                        DicItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicItems[c1.GetKey()].Add(c1);

                    AddNodeTofather(c1, fatherNode.ChildItems);
                }
            }
        }

        private void DoInit(ref TreeViewBaseNode father,
            Dictionary<Tuple<int, int, int, int, int, int>, List<InputInfo>> data)
        {
            if (father == null) return;
            var keyfa = father.GetKey();
            if (data.ContainsKey(keyfa) == false) return;

            var childs = (from t in data[keyfa] orderby t.ZindexN ascending select t).ToList();

            foreach (var l in childs)
            {
                var c1 = GetNewTreeViewBaseNode(father, l);
                if (data.ContainsKey(c1.GetKey()))
                    DoInit(ref c1, data);


                //组下无节点 则删除
                if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
                {
                    if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                        DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicHideItems[c1.GetKey()].Add(c1);
                }
                else
                {
                    if (DicItems.ContainsKey(c1.GetKey()) == false)
                        DicItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicItems[c1.GetKey()].Add(c1);

                    father.ChildItems.Add(c1);
                }


            }
        }

        #endregion

        #region 更新数据 InputInfo updateData

        /// <summary>
        /// 更新数据 更新数据的 updateData的 父节点信息可以为null的 
        /// </summary>
        /// <param name="f"></param>
        public void UpdateNode(InputInfo f)
        {
            // var lstUpdated = new List<InputInfo>();

            if (DicItems.ContainsKey(f.GetKey()))
            {
                foreach (var l in DicItems[f.GetKey()])
                {
                    UpdateTreeViewNode(l, f);
                }

                //更新数据
                //  lstUpdated.Add(f);
            }

            //todo
            //var otherdata = (from t in updateData where lstUpdated.Contains(t) == false select t).ToList();

            //AddNode(otherdata);
        }

        #endregion


        #region 更新数据 UpdateNode(long idfN, List<InputInfo> data)

        /// <summary>
        /// 更新数据  更新idfN下的所有节点信息  ，动态更新 
        /// </summary>
        /// <param name="idfN"></param>
        /// <param name="dataChilds"></param>
        /// <param name="first"></param>
        public void AddNode(long idfN, List<InputInfo> dataChilds, bool first)
        {

            if (DicIdfNs.ContainsKey(idfN) == false) return;
            var key = DicIdfNs[idfN].GetKey();
            //若父节点在 隐藏字典中 ，需将父节点移动到显示树与显示字典中
            if (DicHideItems.ContainsKey(key))
            {
                for (int j = DicHideItems[key].Count - 1; j >= 0; j--)
                {
                    if (DicHideItems[key][j].IdfN == idfN)
                    {
                        var d = DicHideItems[key][j];
                        //移出隐藏字典
                        DicHideItems[key].RemoveAt(j);
                        if (DicHideItems[key].Count == 0) DicHideItems.Remove(key);

                        //添加到可视化字典
                        if (DicItems.ContainsKey(key) == false) DicItems.Add(key, new List<TreeViewBaseNode>());
                        DicItems[key].Add(d);

                        //在树上显示
                        if (d.FatherNodeN == null) AddNodeTofather(d, ChildItems);
                        else AddNodeTofather(d, d.FatherNodeN.ChildItems);
                        break;
                    }
                }
            }

            if (DicItems.ContainsKey(key) == false) return;
            //查找父节点 
            TreeViewBaseNode father = null;
            foreach (var l in DicItems[key])
            {
                if (l.IdfN == idfN)
                {
                    father = l;
                    break;
                }
            }

            if (father == null) return;
            //执行动态更新 子节点

            var dic = new Dictionary<Tuple<int, int, int, int, int, int>, List<InputInfo>>();
            foreach (var l in dataChilds)
            {
                Tuple<int, int, int, int, int, int> keyx = null;
                if (l.FatherInputInfo != null) keyx = l.FatherInputInfo.GetKey();
                else if (l.FatherTreeNodeInfo != null) keyx = l.FatherTreeNodeInfo.GetKey();
                if (keyx == null) continue;

                if (dic.ContainsKey(keyx) == false) dic.Add(keyx, new List<InputInfo>());
                dic[keyx].Add(l);
            }


            AddNode(father, dic, first); //更新子节点信息


            CheckVisiAndHidforChild(father);

        }

        /// <summary>
        /// 增加父节点
        /// </summary>
        /// <param name="data"></param>
        public void AddRootNode(InputInfo data)
        {
            var c1 = GetNewTreeViewBaseNode(null, data);
            ChildItems.Add(c1);

            var key = c1.GetKey();
            if (DicItems.ContainsKey(key) == false) DicItems.Add(key, new List<TreeViewBaseNode>());
            DicItems[key].Add(c1);

            CheckVisiAndHidforChild(c1);
            ReLoadIdfn();

        }


        /// <summary>
        /// 执行动态更新
        /// </summary>
        /// <param name="fatherNode">一定在可视化树中且在可视化字典中的</param>
        /// <param name="data">数据均为该节点下的数据</param>
        /// <param name="isInseratfrist"></param>
        private void AddNode(TreeViewBaseNode fatherNode,
            Dictionary<Tuple<int, int, int, int, int, int>, List<InputInfo>> data, bool isInseratfrist)
        {
            if (data.ContainsKey(fatherNode.GetKey()) == false) return;

            //获取本节点下已经不存在的  清单
            var keys = (from t in data[fatherNode.GetKey()] select t.GetKey()).ToList();
            foreach (var l in fatherNode.ChildItems)
            {
                if (keys.Contains(l.GetKey()))
                {
                    foreach (var lx in data[fatherNode.GetKey()])
                    {
                        if (lx.GetKey().Equals(l.GetKey()))
                        {
                            data[fatherNode.GetKey()].Remove(lx);
                            break;
                        }
                    }

                    //原来的树中已经包含了需要增加的节点 不再增加
                }
            }

            foreach (var l in data[fatherNode.GetKey()])
            {
                var c1 = GetNewTreeViewBaseNode(fatherNode, l);
                if (data.ContainsKey(c1.GetKey()))
                    DoInit(ref c1, data);


                //组下无节点 则删除
                if (c1.IsCollapsedWhenChildItemsEmptyN && c1.ChildItems.Count == 0)
                {
                    if (DicHideItems.ContainsKey(c1.GetKey()) == false)
                        DicHideItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicHideItems[c1.GetKey()].Add(c1);
                }
                else
                {
                    if (DicItems.ContainsKey(c1.GetKey()) == false)
                        DicItems.Add(c1.GetKey(), new List<TreeViewBaseNode>());
                    DicItems[c1.GetKey()].Add(c1);

                    if (isInseratfrist) c1.ZindexN = 1;
                    else c1.ZindexN = 9999999;
                    AddNodeTofather(c1, fatherNode.ChildItems);
                }
            }

            ReLoadIdfn();
        }


        #endregion

    }

    /// <summary>
    /// 注释的代码
    /// </summary>
    public partial class TreeViewControl
    {

        ///// <summary>
        ///// 动态增加节点 已经存在于父节点的节点不再添加，只做更新操作
        ///// 不存在的增加
        ///// </summary>
        ///// <param name="addedDatas">需要添加的节点数据</param>
        ///// <returns></returns>
        //public List<long> AddNode(List<InputInfo> addedDatas)
        //{
        //    var lstUpdate = new List<InputInfo>();

        //    //对已经存在的节点 执行数据更新即可
        //    foreach (var f in addedDatas)
        //    {
        //        //根节点已经存在了 执行更新
        //        if (f.FatherInputInfo == null && f.FatherTreeNodeInfo == null)
        //        {
        //            foreach (var l in ChildItems)
        //            {
        //                if (l.GetKey() == f.GetKey())
        //                {
        //                    UpdateTreeViewNode(l, f);
        //                    lstUpdate.Add(f);
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {

        //            Tuple<int, int, int, int, int, int> fatherkey = null ;      
        //            if (f.FatherInputInfo != null) fatherkey = f.FatherInputInfo.GetKey();
        //            if (f.FatherTreeNodeInfo != null) fatherkey = f.FatherTreeNodeInfo.GetKey();
        //            //子节点已经存在 更新子节点
        //            if (DicItems.ContainsKey(f.GetKey() ))
        //            {
        //                foreach (var l in DicItems[f.GetKey()])
        //                {
        //                    if (l.FatherNodeN != null && l.FatherNodeN.GetKey() == fatherkey)
        //                    {
        //                        UpdateTreeViewNode(l, f);
        //                        lstUpdate.Add(f);
        //                        break;
        //                    }
        //                }
        //            }
        //            if (DicHideItems .ContainsKey(f.GetKey()))
        //            {
        //                foreach (var l in DicHideItems[f.GetKey()])
        //                {
        //                    if (l.FatherNodeN != null && l.FatherNodeN.GetKey() == fatherkey)
        //                    {
        //                        UpdateTreeViewNode(l, f);
        //                        lstUpdate.Add(f);
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }





        //  var addedData = (from t in addedDatas where lstUpdate.Contains(t) == false select t).ToList();

        //  var lstAdded = new List<InputInfo>();
        //  var lstaddedNoed = new List<TreeViewBaseNode>();

        //    //新增根节点 
        //    var rootnode = (from t in addedData where t.FatherInputInfo  == null && t.FatherTreeNodeInfo ==null  orderby t.ZindexN  ascending select t).ToList();
        //    foreach (var f in rootnode)
        //    {
        //        var c1 = GetNewTreeViewBaseNode(null, f);
        //        AddNodeTofather(c1, ChildItems);
        //        lstAdded.Add(f );
        //        lstaddedNoed.Add(c1);
        //    }

        //    addedData = (from t in addedData where lstAdded.Contains(t) == false select t).ToList();

        //    //增加其他子节点
        //    int lastCount = addedData.Count;
        //    while (true)
        //    {
        //        foreach (var l in addedData)
        //        {
        //            if (l.FatherInputInfo == null && l.FatherTreeNodeInfo == null) continue;

        //            if (l.FatherInputInfo != null)
        //            {
        //                Tuple<int, int, int, int, int, int> fatherkey = l.FatherInputInfo.GetKey();
        //                if (DicHideItems.ContainsKey(fatherkey))
        //                {
        //                    var x = DicHideItems[fatherkey].;
        //                    DicHideItems.Remove(fatherkey);

        //                    if (DicItems.ContainsKey(fatherkey)) DicItems[fatherkey].Add(x);
        //                }
        //                if (DicItems.ContainsKey(fatherkey))
        //                {

        //                }
        //            }
        //        }


        //        addedData = (from t in addedData where lstAdded.Contains(t) == false select t).ToList();
        //        if (lastCount == addedData.Count) break;
        //        lastCount = addedData.Count;
        //    }

        //    //增加其他子节点

        //    //添加节点到隐藏树节点中  将隐藏的节点还原到显示节点
        //    var needaddnodeInHidedic =
        //        (from t in addedData where DicHideItems.ContainsKey(t.FatherKey) select t.FatherKey).ToList()
        //        .Distinct();
        //    foreach (var f in needaddnodeInHidedic)
        //    {
        //        if (DicHideItems.ContainsKey(f) == false) continue;
        //        var tmp = DicHideItems[f];

        //        //移动到显示区域
        //        if (tmp.FatherNode == null)
        //        {
        //            AddNodeTofather(tmp, ChildItems);
        //        }
        //        else
        //        {
        //            if (DicItems.ContainsKey(f) == false)
        //            {
        //                AddNodeTofather(tmp, DicItems[f].ChildItems);
        //            }
        //        }

        //        //隐藏节点中删除该节点
        //        DicHideItems.Remove(f);

        //    }



        //    //添加节点到树中
        //    var needaddnode = (from t in addedData
        //        where lstAdded.Contains(t.Key) == false
        //              && DicItems.ContainsKey(t.FatherKey)
        //        select t).ToList();

        //    while (needaddnode.Count > 0)
        //    {
        //        foreach (var f in needaddnode)
        //        {
        //            if (DicItems.ContainsKey(f.FatherKey) == false) continue;
        //            var c1 = f.GetTreeViewBaseNode(DicItems[f.FatherKey], OnNodeSelected, OnNodeExpanded);

        //            //插入
        //            AddNodeTofather(c1, DicItems[f.FatherKey].ChildItems);
        //            lstAdded.Add(f.Key);
        //        }

        //        needaddnode =
        //            (from t in addedData
        //                where lstAdded.Contains(t.Key) == false && DicItems.ContainsKey(t.FatherKey)
        //                select t)
        //            .ToList();
        //    }




        //    //多判断几次  可能出现的情况是  刚好一个组下组的组下组 没节点  一次只能删除一级
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());
        //    AddNodeThenHideNoChildGrp((from t in addedData select t.Key).ToList());



        //    return (from t in addedData where DicItems.ContainsKey(t.Key) == false select t.Key).ToList();

        //}

        ///// <summary>
        ///// 添加节点时候  隐藏无子节点的组节点
        ///// </summary>
        ///// <param name="addKey"></param>
        //private void AddNodeThenHideNoChildGrp(List<long> addKey)
        //{

        //    ////组下无节点 则删除
        //    foreach (var f in addKey)
        //    {
        //        if (DicItems.ContainsKey(f) == false) continue;
        //        //非组节点 不管
        //        if (DicItems[f].IsGroupNobanding == false) continue;

        //        //提取该数据信息
        //        var tmp = DicItems[f];

        //        //字典先移出
        //        DicItems.Remove(f);

        //        //查找该节点的父节点  ，在父节点中移出该显示信息
        //        if (tmp.FatherNode != null && DicItems.ContainsKey(tmp.FatherNode.Key))
        //        {
        //            if (DicItems[tmp.FatherNode.Key].ChildItems.Contains(tmp))
        //            {
        //                DicItems[tmp.FatherNode.Key].ChildItems.Remove(tmp);
        //            }
        //        }

        //        //隐藏节点中添加该节点
        //        if (DicHideItems.ContainsKey(tmp.Key) == false)
        //        {
        //            DicHideItems.Add(tmp.Key, tmp);
        //        }
        //    }
        //}


    }

    /// <summary>
    /// 逻辑部分 TreeViewBaseNode 生成
    /// </summary>
    public partial class TreeViewControl
    {

        private Tuple<int, int, int, int, int, int> GetKeyTuple(int a1, int a2, int a3, int a4, int a5, int a6)
        {
            return new Tuple<int, int, int, int, int, int>(a1, a2, a3, a4, a5, a6);
        }

        public virtual TreeViewBaseNode GetTreeViewBaseNodeExtend(TreeViewBaseNode fatherNode, InputInfo data)
        {
            return null;
        }

        /// <summary>
        /// 用给定的数据初始化节点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fatherNode"></param>
        /// <returns></returns>
        protected TreeViewBaseNode GetNewTreeViewBaseNode(TreeViewBaseNode fatherNode, InputInfo data)
        {
            var nodertn = GetTreeViewBaseNodeExtend(fatherNode, data);

            if (nodertn != null) return nodertn;


            bool isCollapsedWhenChildItemsEmptyN = _dicIsCollapsedWhenChildItemsEmptyN.ContainsKey(data.Key1TypeN) &&
                                                   _dicIsCollapsedWhenChildItemsEmptyN[data.Key1TypeN];

            var rtn = new TreeViewBaseNode(fatherNode, OnNodeSelected, OnNodeExpanded, data.Key1TypeN, data.Key2,
                                           data.Key3, data.Key4, data.Key5, data.Key6, data.ZindexN,
                                           isCollapsedWhenChildItemsEmptyN);

            return GetTreeViewBaseNode(rtn, data);

        }


        /// <summary>
        /// 用给定的数据初始化节点 rtn为新建的node节点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rtn"></param>
        /// <returns></returns>
        protected TreeViewBaseNode GetTreeViewBaseNode(TreeViewBaseNode rtn, InputInfo data)
        {

            rtn.IsRaisePropertyChanged = false;
            //加上判断 速度会更快的
            if (string.IsNullOrEmpty(data.BackGround1B) == false) rtn.BackGround1B = data.BackGround1B;
            if (string.IsNullOrEmpty(data.BackGround2B) == false) rtn.BackGround2B = data.BackGround2B;
            if (string.IsNullOrEmpty(data.BackGround3B) == false) rtn.BackGround3B = data.BackGround3B;
            if (string.IsNullOrEmpty(data.BackGround4B) == false) rtn.BackGround4B = data.BackGround4B;

            if (string.IsNullOrEmpty(data.ForeGround1B) == false) rtn.ForeGround1B = data.ForeGround1B;
            if (string.IsNullOrEmpty(data.ForeGround2B) == false) rtn.ForeGround2B = data.ForeGround2B;
            if (string.IsNullOrEmpty(data.ForeGround3B) == false) rtn.ForeGround3B = data.ForeGround3B;
            if (string.IsNullOrEmpty(data.ForeGround4B) == false) rtn.ForeGround4B = data.ForeGround4B;

            if (data.ImagesIcon1B != null) rtn.ImagesIcon1B = data.ImagesIcon1B;
            if (data.ImagesIcon2B != null) rtn.ImagesIcon2B = data.ImagesIcon2B;
            if (data.ImagesIcon3B != null) rtn.ImagesIcon3B = data.ImagesIcon3B;
            if (data.ImagesIcon4B != null) rtn.ImagesIcon4B = data.ImagesIcon4B;

            if (data.Id1B != 0) rtn.Id1B = data.Id1B;
            if (data.Id2B != 0) rtn.Id2B = data.Id2B;
            if (data.Id3B != 0) rtn.Id3B = data.Id3B;
            if (data.Id4B != 0) rtn.Id4B = data.Id4B;

            if (string.IsNullOrEmpty(data.NodeName1B) == false) rtn.NodeName1B = data.NodeName1B;
            if (string.IsNullOrEmpty(data.NodeName2B) == false) rtn.NodeName2B = data.NodeName2B;
            if (string.IsNullOrEmpty(data.NodeName3B) == false) rtn.NodeName3B = data.NodeName3B;
            if (string.IsNullOrEmpty(data.NodeName4B) == false) rtn.NodeName4B = data.NodeName4B;

            if (data.Is1B) rtn.Is1B = data.Is1B;
            if (data.Is2B) rtn.Is2B = data.Is2B;
            if (data.Is3B) rtn.Is3B = data.Is3B;
            if (data.Is4B) rtn.Is4B = data.Is4B;
            if (data.IsB) rtn.IsB = data.IsB;

            if (data.IsEnable1) rtn.IsEnable1 = data.IsEnable1;
            if (data.IsEnable2) rtn.IsEnable2 = data.IsEnable2;
            if (data.IsEnable3) rtn.IsEnable3 = data.IsEnable3;
            if (data.IsEnable4) rtn.IsEnable4 = data.IsEnable4;

            if (data.IsVisi1 == Visibility.Visible) rtn.IsVisi1 = data.IsVisi1;
            if (data.IsVisi2 == Visibility.Visible) rtn.IsVisi2 = data.IsVisi2;
            if (data.IsVisi3 == Visibility.Visible) rtn.IsVisi3 = data.IsVisi3;
            if (data.IsVisi4 == Visibility.Visible) rtn.IsVisi4 = data.IsVisi4;

            rtn.IsRaisePropertyChanged = true;

            rtn.Id1StoreN = data.Id1StoreN;
            rtn.Id2StoreN = data.Id2StoreN;
            rtn.Id3StoreN = data.Id3StoreN;
            rtn.Id4StoreN = data.Id4StoreN;
            rtn.Id5StoreN = data.Id5StoreN;
            rtn.Id6StoreN = data.Id6StoreN;
            rtn.Id7StoreN = data.Id7StoreN;
            rtn.Id8StoreN = data.Id8StoreN;


            rtn.Ld1StoreN = data.Ld1StoreN;
            rtn.Ld2StoreN = data.Ld2StoreN;
            rtn.Ld3StoreN = data.Ld3StoreN;
            rtn.Ld4StoreN = data.Ld4StoreN;


            rtn.Str1StoreN = data.Str1StoreN;
            rtn.Str2StoreN = data.Str2StoreN;
            rtn.Str3StoreN = data.Str3StoreN;
            rtn.Str4StoreN = data.Str4StoreN;
            return rtn;
        }

        /// <summary>
        /// 更新节点信息 ，不更新父节点、关键字、排位 ，只更新显示的可触发RaisePropertyChanged 更新的属性和用户存储的属性
        /// 具体包括： 背景颜色、前景颜色、图标、id、Name、Is，以及用户存储的数据
        /// </summary>
        /// <param name="rtn"></param>
        /// <param name="data"></param>
        internal void UpdateTreeViewNode(TreeViewBaseNode rtn, InputInfo data)
        {

            //加上判断 速度会更快的
            if (rtn.BackGround1B != data.BackGround1B) rtn.BackGround1B = data.BackGround1B;
            if (rtn.BackGround2B != data.BackGround2B) rtn.BackGround2B = data.BackGround2B;
            if (rtn.BackGround3B != data.BackGround3B) rtn.BackGround3B = data.BackGround3B;
            if (rtn.BackGround4B != data.BackGround4B) rtn.BackGround4B = data.BackGround4B;

            if (rtn.ForeGround1B != data.ForeGround1B) rtn.ForeGround1B = data.ForeGround1B;
            if (rtn.ForeGround2B != data.ForeGround2B) rtn.ForeGround2B = data.ForeGround2B;
            if (rtn.ForeGround3B != data.ForeGround3B) rtn.ForeGround3B = data.ForeGround3B;
            if (rtn.ForeGround4B != data.ForeGround4B) rtn.ForeGround4B = data.ForeGround4B;

            if (rtn.ImagesIcon1B != data.ImagesIcon1B) rtn.ImagesIcon1B = data.ImagesIcon1B;
            if (rtn.ImagesIcon2B != data.ImagesIcon2B) rtn.ImagesIcon2B = data.ImagesIcon2B;
            if (rtn.ImagesIcon3B != data.ImagesIcon3B) rtn.ImagesIcon3B = data.ImagesIcon3B;
            if (rtn.ImagesIcon4B != data.ImagesIcon4B) rtn.ImagesIcon4B = data.ImagesIcon4B;

            if (rtn.Id1B != data.Id1B) rtn.Id1B = data.Id1B;
            if (rtn.Id2B != data.Id2B) rtn.Id2B = data.Id2B;
            if (rtn.Id3B != data.Id3B) rtn.Id3B = data.Id3B;
            if (rtn.Id4B != data.Id4B) rtn.Id4B = data.Id4B;

            if (rtn.NodeName1B != data.NodeName1B) rtn.NodeName1B = data.NodeName1B;
            if (rtn.NodeName2B != data.NodeName2B) rtn.NodeName2B = data.NodeName2B;
            if (rtn.NodeName3B != data.NodeName3B) rtn.NodeName3B = data.NodeName3B;
            if (rtn.NodeName4B != data.NodeName4B) rtn.NodeName4B = data.NodeName4B;

            if (rtn.Is1B != data.Is1B) rtn.Is1B = data.Is1B;
            if (rtn.Is2B != data.Is2B) rtn.Is2B = data.Is2B;
            if (rtn.Is3B != data.Is3B) rtn.Is3B = data.Is3B;
            if (rtn.Is4B != data.Is4B) rtn.Is4B = data.Is4B;
            if (rtn.IsB != data.IsB) rtn.IsB = data.IsB;

            if (rtn.IsEnable1 != data.IsEnable1) rtn.IsEnable1 = data.IsEnable1;
            if (rtn.IsEnable2 != data.IsEnable2) rtn.IsEnable2 = data.IsEnable2;
            if (rtn.IsEnable3 != data.IsEnable3) rtn.IsEnable3 = data.IsEnable3;
            if (rtn.IsEnable4 != data.IsEnable4) rtn.IsEnable4 = data.IsEnable4;

            if (rtn.IsVisi1 != data.IsVisi1) rtn.IsVisi1 = data.IsVisi1;
            if (rtn.IsVisi2 != data.IsVisi2) rtn.IsVisi2 = data.IsVisi2;
            if (rtn.IsVisi3 != data.IsVisi3) rtn.IsVisi3 = data.IsVisi3;
            if (rtn.IsVisi4 != data.IsVisi4) rtn.IsVisi4 = data.IsVisi4;

            rtn.Id1StoreN = data.Id1StoreN;
            rtn.Id2StoreN = data.Id2StoreN;
            rtn.Id3StoreN = data.Id3StoreN;
            rtn.Id4StoreN = data.Id4StoreN;
            rtn.Id5StoreN = data.Id5StoreN;
            rtn.Id6StoreN = data.Id6StoreN;
            rtn.Id7StoreN = data.Id7StoreN;
            rtn.Id8StoreN = data.Id8StoreN;


            rtn.Ld1StoreN = data.Ld1StoreN;
            rtn.Ld2StoreN = data.Ld2StoreN;
            rtn.Ld3StoreN = data.Ld3StoreN;
            rtn.Ld4StoreN = data.Ld4StoreN;


            rtn.Str1StoreN = data.Str1StoreN;
            rtn.Str2StoreN = data.Str2StoreN;
            rtn.Str3StoreN = data.Str3StoreN;
            rtn.Str4StoreN = data.Str4StoreN;

        }


        //public static InputInfo GetInputInfo(TreeViewBaseNode data)
        //{
        //    long fakey = 0;
        //    if (data.FatherNode != null) fakey = data.FatherNode.Key;

        //    var rtn = new InputInfo(fakey);

        //    //加上判断 速度会更快的
        //    rtn.BackGround1 = data.BackGround1;
        //    rtn.BackGround2 = data.BackGround2;
        //    rtn.BackGround3 = data.BackGround3;
        //    rtn.BackGround4 = data.BackGround4;

        //    rtn.ForeGround1 = data.ForeGround1;
        //    rtn.ForeGround2 = data.ForeGround2;
        //    rtn.ForeGround3 = data.ForeGround3;
        //    rtn.ForeGround4 = data.ForeGround4;

        //    rtn.ImagesIcon1 = data.ImagesIcon1;
        //    rtn.ImagesIcon2 = data.ImagesIcon2;
        //    rtn.ImagesIcon3 = data.ImagesIcon3;
        //    rtn.ImagesIcon4 = data.ImagesIcon4;

        //    rtn.Id1 = data.Id1;
        //    rtn.Id2 = data.Id2;
        //    rtn.Id3 = data.Id3;
        //    rtn.Id4 = data.Id4;

        //    rtn.NodeName1 = data.NodeName1;
        //    rtn.NodeName2 = data.NodeName2;
        //    rtn.NodeName3 = data.NodeName3;
        //    rtn.NodeName4 = data.NodeName4;

        //    rtn.Is1 = data.Is1;
        //    rtn.Is2 = data.Is2;
        //    rtn.Is3 = data.Is3;
        //    rtn.Is4 = data.Is4;

        //    rtn.Type = data.TypeNobanding;
        //    rtn.OrderIdx = data.OrderIdxNobanding;
        //    rtn.IsGroup = data.IsGroupNobanding;


        //    rtn.Id1StoreNoBanding = data.Id1StoreNoBanding;
        //    rtn.Id2StoreNoBanding = data.Id2StoreNoBanding;
        //    rtn.Id3StoreNoBanding = data.Id3StoreNoBanding;
        //    rtn.Id4StoreNoBanding = data.Id4StoreNoBanding;


        //    rtn.Id5StoreNoBanding = data.Id5StoreNoBanding;
        //    rtn.Id6StoreNoBanding = data.Id6StoreNoBanding;
        //    rtn.Id7StoreNoBanding = data.Id7StoreNoBanding;
        //    rtn.Id8StoreNoBanding = data.Id8StoreNoBanding;


        //    rtn.Lg1StoreNoBanding = data.Lg1StoreNoBanding;
        //    rtn.Lg2StoreNoBanding = data.Lg2StoreNoBanding;
        //    rtn.Lg3StoreNoBanding = data.Lg3StoreNoBanding;
        //    rtn.Lg4StoreNoBanding = data.Lg4StoreNoBanding;

        //    rtn._key = data.Key;
        //    return rtn;
        //}



    }
}
