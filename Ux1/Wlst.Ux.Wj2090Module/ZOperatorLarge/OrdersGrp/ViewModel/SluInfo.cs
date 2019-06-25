using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Ux.Wj2090Module.TreeTab.View.ViewModels;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.ViewModel
{

    public partial class SluInfo : TreeNodeBase //,Wlst.Cr.Core.CoreServices.ObservableObject
    {

        private static Dictionary<int, List<TreeNodeBase>> _registerTmlNode = new Dictionary<int, List<TreeNodeBase>>();

        public static Dictionary<int, List<TreeNodeBase>> RegisterTmlNode
        {
            get { return _registerTmlNode; }
        }

        public static bool ClearRegisterTmlNodes()
        {
            try
            {
                _registerTmlNode.Clear();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }



        public static List<int> GetNodeChecked(bool isMeasure)
        {
            var rtn = new List<int>();
            foreach (var f in RegisterTmlNode)
            {
                foreach (var l in f.Value)
                {
                    if (l.IsChecked && rtn.Contains(l.NodeId) == false) rtn.Add(l.NodeId);
                }
            }
            return rtn;

        }



        private int _sSluId;

        public int SluId
        {
            get { return _sSluId; }
            set
            {
                if (value != _sSluId)
                {
                    _sSluId = value;
                    this.RaisePropertyChanged(() => this.SluId);

                    SluShowId = value + "";
                    SluName = value + "";

                    var sluinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                    if (sluinfo != null)
                    {
                        SluName = sluinfo.RtuName;
                        NodeName = SluName;
                        if (sluinfo.RtuFid  == 0)
                        {
                            SluShowId = sluinfo.RtuPhyId .ToString("D4");
                            PhysicalId = sluinfo.RtuPhyId ;
                           
                        }
                        else
                        {
                            var mtps =
                                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                                    sluinfo.RtuFid );
                            if (mtps != null)
                            {
                                SluShowId = mtps.RtuPhyId.ToString("D4");
                                PhysicalId = sluinfo.RtuPhyId;
                            }

                        }
                    }


                }
            }
        }

        private string _ssdfSluId;

        public string SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }


        private string _sSluName;

        public string SluName
        {
            get { return _sSluName; }
            set
            {
                if (value != _sSluName)
                {
                    _sSluName = value;
                    this.RaisePropertyChanged(() => this.SluName);
                }
            }
        }



        //private ObservableCollection<NameIntBoolx> _operationOperatorType = null;

        ///// <summary>
        /////  1 全部、2 单数、3 双数、4、自定义操作   其他11-99： 如32 地址除3余2的控制器操作 101 不操作
        ///// </summary>
        //public ObservableCollection<NameIntBoolx> OperatorType
        //{
        //    get
        //    {
        //        if (_operationOperatorType == null)
        //        {
        //            _operationOperatorType = new ObservableCollection<NameIntBoolx>();
        //            _operationOperatorType.Add(new NameIntBoolx()
        //                                           {Name = "全部", Value = 10, IsGrp = false,IsVisi = Visibility.Visible ,IsSelected = false});
        //            _operationOperatorType.Add(new NameIntBoolx() { Name = "单数", Value = 21, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });
        //            _operationOperatorType.Add(new NameIntBoolx() { Name = "双数", Value = 20, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });
        //            _operationOperatorType.Add(new NameIntBoolx() { Name = "隔二亮一", Value = 31, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });

        //        }
        //        return _operationOperatorType;
        //    }
        //}


        protected bool IsGprs = false;

        public SluInfo(TreeNodeBase father, int sluId)
        {
            this.SluId = sluId;
            NodeId = sluId;
            _father = father;
            IsGroup = false;

            LoadSluGrp();


            if (!_registerTmlNode.ContainsKey(NodeId))
            {
                _registerTmlNode.Add(NodeId, new List<TreeNodeBase>());
            }

            _registerTmlNode[NodeId].Add(this);

        }

        private void LoadSluGrp()
        {
            var sluinfo =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(SluId) as
                Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (sluinfo == null) return;
         
                var lst = (from t in sluinfo .WjSluCtrlGrps .Values  orderby t.GrpId ascending select t).ToList();
            //初始化了4个默认操作  组id从5开始
                int index = 4;
                foreach (var g in lst)
                {
                    var tmp = new NameIntBoolx()
                                  {
                                      Name = g.GrpId.ToString("d2") + g.GrpName,
                                      Value = g.GrpId,
                                      IsGrp = true,
                                      IsVisi = Visibility.Visible,
                                      IsSelected = false,
                                      SelfNode = this,
                                  };

                    //原代码为add，现更改为初始化10路，后面直接赋值，不用动态添加 lvf 2019年3月19日15:07:21
                    OperatorType[index]= new NameIntBoolx()
                                             {
                                                 Name =tmp.Name,
                                                 Value = tmp.Value,
                                                 IsGrp = tmp.IsGrp,
                                                 IsVisi = tmp.IsVisi,
                                                 IsSelected = tmp.IsSelected,
                                                 SelfNode = this,
                                             };
                    index++;
                }



            //初始化 已经扩展为10 该代码无意义 lvf 2019年3月19日15:02:45
            //int xcount = OperatorType.Count;
            //for (int i = xcount; i < 11; i++)
            //{
            //    OperatorType.Add(new NameIntBoolx()
            //                         {
            //                             Name = "无",
            //                             Value = -1,
            //                             IsGrp = true,
            //                             IsVisi = Visibility.Collapsed,
            //                             IsSelected = false
            //                         });
            //}
        }
    }


    public class NameIntBoolx : NameIntBool
    {
        public bool IsGrp;


        private Visibility _csdfdsheck;

        public Visibility IsVisi
        {
            get { return _csdfdsheck; }
            set
            {
                if (_csdfdsheck != value)
                {
                    _csdfdsheck = value;
                    this.RaisePropertyChanged(() => this.IsVisi);
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);

                    //如果是集中器下控制器分组上的点击，不处理
                    if (IsGrp) return;
                    //遍历 
                    if (SelfNode == null) return;
                    if (SelfNode.IsGroup)
                    {
                        foreach (var g in SelfNode.ChildTreeItems)
                        {
                            var op = g.OperatorType;

                            foreach (var t in op)
                            {
                                if ( t.Value == Value)
                                {
                                    t.IsSelected = IsSelected;
                                    break;
                                }
                            }
                           
                        }

                    }

                }
            }
        }



        protected TreeNodeBase _father;

        public TreeNodeBase SelfNode
        {
            get { return _father; }
            set
            {
                if (_father != value)
                {
                    _father = value;
                    this.RaisePropertyChanged(() => this.SelfNode);
                }
            }
        }


    }


}
