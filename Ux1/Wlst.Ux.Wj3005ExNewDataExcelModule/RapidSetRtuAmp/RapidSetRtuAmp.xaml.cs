using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.RapidSetRtuAmp
{
    /// <summary>
    /// RapidSetRtuAmp.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class RapidSetRtuAmp : Window
    {
        public RapidSetRtuAmp()
        {
            InitializeComponent();
            this.InitAction();
            this.DataContext = this;
            this.Loaded +=new RoutedEventHandler(WindowsLoaded);
          
        }


        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            LoadDisplayIndex();
            if (_newDataColumnsDisplayIndex == null || _newDataColumnsDisplayIndex.Count == 0) return;
            foreach (var g in rapidSetAmp.Columns)
            {
                foreach (var j in _newDataColumnsDisplayIndex)
                {
                    if (g.Header.ToString() == j.Key)
                    {
                        g.DisplayIndex = int.Parse(j.Value);
                        break;
                    }
                }
            }
        }
        private void LoadDisplayIndex()
        {
            _newDataColumnsDisplayIndex.Clear();

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            foreach (var g in info)
            {
                _newDataColumnsDisplayIndex.Add(g.Key, g.Value);
            }
        }
        public const string XmlConfigName = "Wlst.Ux.Wj3005ExNewDataExcelModule.RapidSetRtuAmp";
        public int RtuId;

        private bool isViewActive = false;
        public void InitWin(int index,int rtuid,
                            ObservableCollection
                                <Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel.LoopInfoLeft>
                                data)
        {
            var _color = "Black";
            isViewActive = true ;
            RtuId = rtuid;
            Items.Clear();
            var nt = (from t in data
                      where string.IsNullOrEmpty(t.V) == false && string.IsNullOrEmpty(t.A) == false
                      orderby t.LoopId ascending
                      select t).ToList();
            foreach (var f in nt)
            {
                _color = "Black";
                if (f.isShieldLoop ==0)
                {  
                    if (Getint(f.A) > Getint(f.Upper) || Getint(f.A) < Getint(f.Lower))
                    {
                        _color = "Red";
                    }

                }
                Items.Add(new Loop()
                              {
                                  A = f.A,
                                  V = f.V,
                                  isCanEdit = f.isShieldLoop == 0,
                                  ShowRed = _color,
                                  LoopId = f.LoopId,
                                  LoopName = f.LoopName,
                                  SwitchInState = f.SwitchInState,

                                  Lower = f.isShieldLoop==1 ? 0 : Getint(f.Lower),
                                  RefA = f.isShieldLoop ==1? 0 : Double.Parse(f.RefA),
                                  Upper = f.isShieldLoop ==1? 0 : Getint(f.Upper)
                });
            }
            if (index < 0) index = 0;
            if (rapidSetAmp.Items.Count < 1) return;
            rapidSetAmp.SelectedItem = Items[index];

            UpdateRules();
        }

        public void InitWin(int index,int rtuid,
                            ObservableCollection
                                <Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.ViewModel.LoopInfox>
                                data)
        {
            var _color = "";
            isViewActive = true ;
            RtuId = rtuid;
            Items.Clear();
            var nt = (from t in data
                      where string.IsNullOrEmpty(t.V) == false && string.IsNullOrEmpty(t.A) == false
                      orderby t.LoopId ascending
                      select t).ToList();
            foreach (var f in nt)
            {
                _color = "";
                if (f.isShieldLoop==0)
                {                  
                    if (Getint(f.A) > Getint(f.Upper) || Getint(f.A) <Getint(f.Lower))
                    {
                        _color = "Red";
                    }
                }
                Items.Add(new Loop()
                              {
                                  A = f.A,
                                  V = f.V,
                                  isCanEdit = f.isShieldLoop == 0,

                                  ShowRed = _color,
                                  LoopId = f.LoopId,
                                  LoopName = f.LoopName,
                                  SwitchInState = f.SwitchInState,
                                  Lower = f.isShieldLoop ==1 ? 0 :Getint(f.Lower),
                                  RefA = f.isShieldLoop ==1? 0 :Double.Parse(f.RefA), //Getint(f.RefA),
                                  Upper = f.isShieldLoop ==1 ? 0 :  Getint(f.Upper)
                                  
                              });
                
            }
            if (index < 0) index = 0;
            if (rapidSetAmp.Items.Count < 1) return;
            if (index > Items.Count) return;
            rapidSetAmp.SelectedItem = Items[index];

        //    Wlst .Cr .Core .CoreServices .SystemOption .GetOption( 3)
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            isViewActive = false;
            Items.Clear();
            this.Visibility = Visibility.Collapsed;
            e.Cancel = true;
            base.OnClosing(e);
        }


        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set,// .wlst_svr_ans_cnt_wj3090_request_ldl_loop_current_avg,//.ClientPart.wlst_Measures_server_ans_clinet_rqup_loop_sxx,
                RequestOrUpdateFaultTypeInfo,
                typeof(RapidSetRtuAmp), this,true );
        }

        private void RequestOrUpdateFaultTypeInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (isViewActive == false) return;
            var data = infos.WstRtuLdlSxxAvgSet;
            if (data == null) return;
            MyTextboxShow mtest = new MyTextboxShow();
            if (data.Op != 14) return;



            //var dic = new Dictionary<int, List<RtuSets.RtuLoopSxx>>();
            //foreach (var f in data .SxxItems ){
            //    if (dic.ContainsKey(f.RtuId) == false) dic.Add(f.RtuId, new List<RtuSets.RtuLoopSxx>());
            //    dic[f.RtuId].Add(f);
            //}

            //foreach (var f in dic)
            //{
            //    var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Key);
            //    var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            //    if (tmpequ2 == null) continue;
            //    foreach (var g in f.Value)
            //    {
            //        if (tmpequ2.WjLoops.ContainsKey(g.LoopId))
            //        {
            //            tmpequ2.WjLoops[g.LoopId].CurrentAlarmUpperlimit = g.SxDefault;
            //            tmpequ2.WjLoops[g.LoopId].CurrentAlarmLowerlimit = g.XxDefault;

            //        }
            //    }
            //}


            var ntg = (from t in data.SxxItems where t.RtuId == RtuId orderby t.LoopId select t).ToList();
            if (ntg.Count == 0) return;
            foreach (var g in ntg)
            {
                if (Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(g.RtuId) == false) return;

                //if (Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[g.RtuId].RtuNewData.LstNewLoopsData.Count <= g.LoopId - 1)
                //    return;

                //lvf 2019年3月27日08:44:40  更新最新数据中的上下限和额定电流
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[g.RtuId].RtuNewData.LstNewLoopsData)
                {
                    if ( t.LoopId == g.LoopId)
                    {
                        t.Upper = g.SxDefault;
                        t.Lower = g.XxDefault;
                        t.AvgOf7daysA = g.Avg;
                    }
                }



                //Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[g.RtuId].RtuNewData.LstNewLoopsData[ g.LoopId-1]
                //    .Upper = g.SxDefault;
                //Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[g.RtuId].RtuNewData.LstNewLoopsData[g.LoopId-1]
                //    .Lower = g.XxDefault;
                //Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[g.RtuId].RtuNewData.LstNewLoopsData[
                //    g.LoopId - 1]
                //    .AvgOf7daysA = g.Avg;
            }


              //var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
              //if (run != null && run.RtuNewData != null)
              //{
              //    foreach (var t in ntg)
              //    {
              //       run.RtuNewData.LstNewLoopsData[0].Upper 
              //    }
              //}
            if(DateTime .Now .Ticks - lastSavetime < 10*10000000L)
            {
                tbremark.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端额定电流、上下限设置成功.";
            }
            else
            {
                tbremark.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端额定电流、上下限设置已经更新，若需设置，请重新进入该界面"; 
                foreach (var f in Items) f.isCanEdit = false;
            }

          


                //var dirc = new Dictionary<Tuple<int, int>, Tuple<double, int, int>>();
                //var dirc2 = new Dictionary<Tuple<int, int>, Tuple<double, int, int>>();
                //foreach (var f in data.SxxItems)
                //{
                //    var tu = new Tuple<int, int>(f.RtuId, f.LoopId);
                //    if (dirc.ContainsKey(tu)) continue;
                //    var tu2 = new Tuple<double, int, int>(f.Avg, f.SxDefault, f.XxDefault);
                //    dirc.Add(tu, tu2);

                //    if (f.Avg2 > 0)
                //    {
                //        if (dirc2.ContainsKey(tu)) continue;
                //        var tu3 = new Tuple<double, int, int>(f.Avg2, f.Sx1, f.Xx1);
                //        dirc2.Add(tu, tu3);
                //    }
                //}

                //foreach (var f in this.Items)
                //{
                //    ////if (f.ValueString2.Contains("屏蔽")) continue;
                //    //var flg = false;
                //    //var flg2 = false;
                //    //foreach (var t in f.ChildTreeItems)
                //    //{
                //    //    var tu = new Tuple<int, int>(t.Rtuid, t.ValueInt1);
                //    //    if (dirc.ContainsKey(tu))
                //    //    {
                //    //        t.ValueString3 = dirc[tu].Item1.ToString("f2");
                //    //        t.ValueInt3 = dirc[tu].Item2;
                //    //        t.ValueInt2 = dirc[tu].Item3;
                //    //        flg = true;
                //    //        cancmdupdatecurrent = true;
                //    //    }
                //    //    if (dirc2.ContainsKey(tu))
                //    //    {
                //    //        t.ValueString4 = dirc2[tu].Item1.ToString("f2");
                //    //        t.ValueInt5 = dirc2[tu].Item2;
                //    //        t.ValueInt4 = dirc2[tu].Item3;
                //    //        flg2 = true;
                //    //    }
                //    //}
                //    //if (flg) f.ValueString3 = "√";
                //    //if (flg2) f.ValueString4 = "√";
                //}
               
             
        }

        private ObservableCollection<Loop> _item = null;

        public ObservableCollection<Loop> Items
        {
            get
            {
                if (_item == null) _item = new ObservableCollection<Loop>();
                return _item;
            }
        }


        private long lastSavetime = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RtuId < 0) return;
            ReqLowMaxInfo();
        }

        private int Getint(string data)
        {
            var sps = data.Split('.');
            if (sps.Count() > 0)
            {
                int rtn = 0;
                Int32.TryParse(sps[0], out rtn);
                return rtn;
            }
            return 0;
        }


        private void ReqLowMaxInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set;
            //.ServerListen.wlst_cnt_wj3090_request_ldl_loop_current_avg;
            info.WstRtuLdlSxxAvgSet.Op = 14;


            MyTextboxShow mtest = new MyTextboxShow();



            foreach (var f in Items)
            {

                if (f.isCanEdit == false) continue;
                info.WstRtuLdlSxxAvgSet.SxxItems.Add(new RtuSets.RtuLoopSxx()
                                                         {
                                                             RtuId = RtuId,
                                                             LoopId = f.LoopId,
                                                             SxDefault = f.Upper,
                                                             XxDefault = f.Lower,
                                                             Sx1 = 0,
                                                             Xx1 = 0,
                                                             Avg = f.RefA,
                                                             Avg2 = 0
                                                         });
            }




            SndOrderServer.OrderSnd(info, 10, 4);
            tbremark.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新已经提交，修改后的上下限信息将在客户端下次登录后更新...";
            lastSavetime = DateTime.Now.Ticks;
            //mtest.show = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新已经提交，修改后的上下限信息将在客户端下次登录后更新...";
            //tbremark.DataContext = mtest;
        }


        //    private string _remark = string.Empty;
        //    public string Remark
        //    {
        //        get
        //        {
        //            if (_remark.Length == 0)
        //                _remark = "";
        //            return _remark;
        //        }
        //        set
        //        {
        //            if (_remark != value)
        //            {
        //                _remark = value;
        //                OnPropertyChanged("Remark");
        //            }
        //        }
        //    }

        //    public event PropertyChangedEventHandler PropertyChanged;
        //    public virtual void OnPropertyChanged(string propertyName)
        //    {
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //        }
        //    }


        //}

        public class Loop : ObservableObject
        {

            private int _loopId;

            /// <summary>
            /// 回路序号
            /// </summary>
            public int LoopId
            {
                get { return _loopId; }
                set
                {
                    if (value != _loopId)
                    {
                        _loopId = value;
                        this.RaisePropertyChanged(() => this.LoopId);
                    }
                }
            }



            private string _showRed;

            /// <summary>
            /// 告警颜色为红
            /// </summary>
            public string ShowRed
            {
                get { return _showRed; }
                set
                {
                    if (value != _showRed)
                    {
                        _showRed = value;
                        this.RaisePropertyChanged(() => this.LoopId);
                    }
                }
            }


            private string _loopName;

            /// <summary>
            /// 回路名称
            /// </summary>
            public string LoopName
            {
                get { return _loopName; }
                set
                {
                    if (value != _loopName)
                    {
                        _loopName = value;
                        this.RaisePropertyChanged(() => this.LoopName);
                    }
                }
            }



            private string _v;

            /// <summary>
            /// 回路电压  或 所代表的门啥的状态
            /// </summary>
            public string V
            {
                get { return _v; }
                set
                {
                    if (value != _v)
                    {
                        _v = value;
                        this.RaisePropertyChanged(() => this.V);
                    }
                }
            }

            private string _a;

            /// <summary>
            /// 回路电流
            /// </summary>
            public string A
            {
                get { return _a; }
                set
                {
                    if (value != _a)
                    {
                        _a = value;
                        this.RaisePropertyChanged(() => this.A);
                    }
                }
            }






            private string _SwitchInState;

            /// <summary>
            /// ~~~
            /// </summary>
            public string SwitchInState
            {
                get { return _SwitchInState; }
                set
                {
                    if (value != _SwitchInState)
                    {
                        _SwitchInState = value;
                        this.RaisePropertyChanged(() => this.SwitchInState);
                    }
                }
            }


            private int _upper;

            /// <summary>
            /// 电流上限
            /// </summary>
            public int Upper
            {
                get { return _upper; }
                set
                {
                    if (value != _upper)
                    {
                        _upper = value;
                        this.RaisePropertyChanged(() => this.Upper);
                    }
                }
            }

            private int _lower;

            /// <summary>
            /// 电流下限
            /// </summary>
            public int Lower
            {
                get { return _lower; }
                set
                {
                    if (value != _lower)
                    {
                        _lower = value;
                        this.RaisePropertyChanged(() => this.Lower);
                    }
                }
            }



            private double _refA;

            /// <summary>
            /// 参考电流
            /// </summary>
            public double RefA
            {
                get { return _refA; }
                set
                {
                    if (value != _refA)
                    {
                        _refA = value;
                        this.RaisePropertyChanged(() => this.RefA);
                    }
                }
            }




            private bool _isShieldLoop;

            /// <summary>
            /// 是否屏蔽该回路
            /// </summary>
            public bool isCanEdit
            {
                get { return _isShieldLoop; }
                set
                {
                    if (value != _isShieldLoop)
                    {
                        _isShieldLoop = value;
                        this.RaisePropertyChanged(() => this.isCanEdit);
                    }
                }
            }











        }

        public class MyTextboxShow : INotifyPropertyChanged
        {

            public string show; //显示
            public event PropertyChangedEventHandler PropertyChanged;

            public string Show
            {
                get { return show; }
                set
                {
                    show = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Show"));
                }
            }
        }
        private Dictionary<string, string> _newDataColumnsDisplayIndex = new Dictionary<string, string>();
        private void RadGridView_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            _newDataColumnsDisplayIndex.Clear();
            foreach (var g in rapidSetAmp.Columns)
            {
                _newDataColumnsDisplayIndex.Add(g.Header.ToString(), g.DisplayIndex.ToString());
            }


            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(_newDataColumnsDisplayIndex, XmlConfigName);
        }

     
    }


    partial class RapidSetRtuAmp
    {   
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //上下限规则  lvf 2019年3月26日11:03:28
            var ntgs = (from t in ItemsRules orderby t.Index ascending select t).ToList();

            //回路数据
            foreach (var t in Items )
            {
                if (t.isCanEdit == false) continue;
                double a = 0;
                var atmp = t.A.Substring(0, t.A.Length - 1);
                if (Double.TryParse(atmp, out a) == false) continue;
                foreach (var f in ntgs)
                {
                    

                    if (f.Alow <= a && a  <= f.Amax &&a  >= 0)
                    {
                        var tAMin = (int)(a  * f.LowTimes);
                        var tamax = 0;

                        if (a  * f.MaxTimes - (int)(a  * f.MaxTimes) >= 0.5)
                            tamax = (int)(a  * f.MaxTimes) + 1;
                        else
                            tamax = (int)(a  * f.MaxTimes);

                        t.Upper = tamax;
                        t.Lower = tAMin;
                        t.RefA = (int ) a ;
                        //CurRuleItem.SelectedRtus.Add(tmp);
                    }
                }
            }
        }
        
        private void UpdateRules()
        {
            var AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(RtuId);

            this.ItemsRules.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.RtuSxxRuleInstancesHold.Myself.Rules.ContainsKey(AreaId))
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.RtuSxxRuleInstancesHold.Myself.Rules[AreaId])
                {
                    ItemsRules.Add(new RultItem()
                    {
                        Index = t.Index,
                        Alow = t.CurrentALow,
                        Amax = t.CurrentAMax,
                        LowTimes = Math.Round(t.LowerTimes, 2),
                        MaxTimes = Math.Round(t.MaxTimes, 2)
                    });

                }
            }

            if (ItemsRules.Count < 1)
            {
                ItemsRules.Add(new RultItem()
                {
                    Index = 1,
                    Alow = 0,
                    Amax = 1000,
                    LowTimes = 0.8,
                    MaxTimes = 1.2
                });
            }


        }      
        private ObservableCollection<RultItem> _itemItemsRuless;

        public ObservableCollection<RultItem> ItemsRules
        {
            get
            {
                if (_itemItemsRuless == null) _itemItemsRuless = new ObservableCollection<RultItem>();
                return _itemItemsRuless;
            }
        }
    }

    public class RultItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        int xxIndex;
        int xxAlow;
        int xxAmax;
        double xxLowTimes;
        double xxMaxTimes;


        public int Index
        {
            get { return xxIndex; }
            set
            {
                if (value == xxIndex) return;
                xxIndex = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }
        public int Alow
        {
            get { return xxAlow; }
            set
            {
                if (value == xxAlow) return;
                xxAlow = value;
                this.RaisePropertyChanged(() => this.Alow);
            }
        }
        public int Amax
        {
            get { return xxAmax; }
            set
            {
                if (value == xxAmax) return;
                xxAmax = value;
                this.RaisePropertyChanged(() => this.Amax);
            }
        }
        public double LowTimes
        {
            get { return xxLowTimes; }
            set
            {
                if (value == xxLowTimes) return;
                if (value >= 1) value = 0.99;
                if (value <= 0) value = 0;
                xxLowTimes = value;
                this.RaisePropertyChanged(() => this.LowTimes);
            }
        }
        public double MaxTimes
        {
            get { return xxMaxTimes; }
            set
            {
                if (value == xxMaxTimes) return;
                if (value <= 1) value = 1.01;
                xxMaxTimes = value;
                this.RaisePropertyChanged(() => this.MaxTimes);
            }
        }




    }

}


