using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using System.Xml;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Statistics.UxStatistics.Services;
using Wlst.Ux.Statistics.UxStatistics.Views;
using Wlst.client;

namespace Wlst.Ux.Statistics.UxStatistics.ViewModel
{
    [Export(typeof (IIUxStatisticsModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class StatisticsViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITab, IIUxStatisticsModule
    {
        private Random rand = new Random(123456);
        private object _data;
        private object _lineData;
        private object _polarData;
        private object _barData;
        private ChartPalette _palette;
        private List<ChartPalette> _palettes;

        public void NavOnLoad(params object[] parsObjects)
        {
            //this.Records = GetData();
            this.LineData = this.CreateLineData();
        }

        public void OnUserHideOrClosing()
        {

        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "关于"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        #region Attris

        private string _smallTitle;
        public string SmallTitle
        {
            get { return this._smallTitle; }
            set
            {
                if (this._smallTitle != value)
                {
                    this._smallTitle = value;
                    RaisePropertyChanged(() => SmallTitle);
                }
            }
        }

        private string _charOneTitle;
        public string CharOneTitle
        {
            get { return this._charOneTitle; }
            set
            {
                if (this._charOneTitle != value)
                {
                    this._charOneTitle = value;
                    RaisePropertyChanged(() => CharOneTitle);
                }
            }
        }
        public object Data
        {
            get { return this._data; }
            set
            {
                if (this._data != value)
                {
                    this._data = value;
                    RaisePropertyChanged(() => Data);
                }
            }
        }

        public object BarData
        {
            get { return this._barData; }
            set
            {
                if (this._barData != value)
                {
                    this._barData = value;
                    RaisePropertyChanged(() => BarData);
                }
            }
        }

        public object LineData
        {
            get { return this._lineData; }
            set
            {
                if (this._lineData != value)
                {
                    this._lineData = value;
                    RaisePropertyChanged(() => LineData);
                }
            }
        }

        public ChartPalette Palette
        {
            get { return this._palette; }
            set
            {
                if (this._palette != value)
                {
                    this._palette = value;
                    RaisePropertyChanged(() => Palette);
                }
            }
        }

        public List<ChartPalette> Palettes
        {
            get { return this._palettes; }
            set
            {
                if (this._palettes != value)
                {
                    this._palettes = value;
                    RaisePropertyChanged(() => Palettes);
                }
            }
        }

        #endregion

        public StatisticsViewModel()
        {
            SmallTitle = "";
            this.InitializeData();
            this.InitializePalettePresets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartType">1- 饼图   2-柱状图</param>
        /// <param name="dataType">1- 终端故障 2- 单灯故障 3-上线率 4-耗电量 5-亮灯率 6-节能率</param>
        public StatisticsViewModel(int chartType, int dataType,string name)
        {
            SmallTitle = "";
            this.InitializePalettePresets();

            if (chartType == 1) //带率  饼图
            {
                List<Data> PieData = new List<Data>();
                if (dataType == 3) //在线率
                {
                    SmallTitle = name +"  在线率统计";


                    var sp = name.Split('-');
                    var month = Convert.ToInt32(sp[0]);
                    var day = Convert.ToInt32(sp[1]);

                    var d2 = new DateTime(2017, month, day, 12, 0, 1);
                    var d1 = d2.AddDays(-1);

                    var lst = new List<Tuple<int, int>>();
                    lst.Add(new Tuple<int, int>(98, 100));
                    lst.Add(new Tuple<int, int>(96, 98));
                    lst.Add(new Tuple<int, int>(94, 96));
                    lst.Add(new Tuple<int, int>(90, 94));
                    lst.Add(new Tuple<int, int>(0, 90));

                    int allcount = (from t in Wlst.Sr.tmphold.d5Hold.MySelf.Zxls
                                    where
                                        t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks
                                    select t).Count();

                    foreach (var f in lst)
                    {
                        double tmp =
                            (from t in Wlst.Sr.tmphold.d5Hold.MySelf.Zxls
                             where
                                 t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.zxl > f.Item1 * 0.01 &&
                                 t.zxl <= f.Item2 * 0.01
                             select t).Count() * 1.0 / allcount;


                        PieData.Add(new Data(f.Item1 + "%-" + f.Item2 + "%", tmp, 0, 5, name));
                    }

                    


                }
                if (dataType == 5) // 亮灯率
                {
                    SmallTitle = name + "  亮灯率统计";



                    var sp = name.Split('-');
                    var month = Convert.ToInt32(sp[0]);
                    var day = Convert.ToInt32(sp[1]);

                    var d2 = new DateTime(2017, month, day, 12, 0, 1);
                    var d1 = d2.AddDays(-1);

                    var lst = new List<Tuple<int, int>>();
                    lst.Add(new Tuple<int, int>(98, 100));
                    lst.Add(new Tuple<int, int>(96, 98));
                    lst.Add(new Tuple<int, int>(94, 96));
                    lst.Add(new Tuple<int, int>(90, 94));
                    lst.Add(new Tuple<int, int>(0, 90));

                    int allcount = (from t in Wlst.Sr.tmphold.d4Hold.MySelf.Ldls
                                    where
                                        t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks
                                    select t).Count();

                    foreach (var f in lst)
                    {
                        double tmp =
                            (from t in Wlst.Sr.tmphold.d4Hold.MySelf.Ldls
                             where
                                 t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.ldl > f.Item1 * 0.01 &&
                                 t.ldl <= f.Item2 * 0.01
                             select t).Count() * 1.0 / allcount;


                        PieData.Add(new Data(f.Item1 + "%-" + f.Item2 + "%", tmp, 0, 4, name));
                    }



                    //PieData.Add(new Data("90%+", 82.35));
                    //PieData.Add(new Data("80%-90%", 6.69));
                    //PieData.Add(new Data("70%-80%", 5.12));
                    //PieData.Add(new Data("60%-70%", 3.71));
                    //PieData.Add(new Data("-50%", 1));

                }
                if (dataType == 6) //节能率
                {
                    SmallTitle = name + "  节能率统计";
                    var sp = name.Split('-');
                    var month = Convert.ToInt32(sp[0]);
                    var day = Convert.ToInt32(sp[1]);

                    var d2 = new DateTime(2017, month, day, 12, 0, 1);
                    var d1 = d2.AddDays(-1);

                    var lst = new List<Tuple<int, int>>();
                    lst.Add(new Tuple<int, int>(25, 100));
                    lst.Add(new Tuple<int, int>(20, 25));
                    lst.Add(new Tuple<int, int>(15, 20));
                    lst.Add(new Tuple<int, int>(10, 15));
                    lst.Add(new Tuple<int, int>(0, 10));

                    int allcount = (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                                    where t.DateCreate>d1.Ticks  && t.DateCreate<=d2.Ticks
                                    select t).Count();
                    foreach (var f in lst)
                    {
                        double tmp =
                            (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                             where
                                 t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.jnl > f.Item1*0.01 &&
                                 t.jnl <= f.Item2*0.01
                             select t).Count()*1.0/allcount;


                        PieData.Add(new Data(f.Item1 + "%-"+ f.Item2+"%", tmp,0,3 ,name));
                    }

                    //  double tmp98 =
                   //(from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                   // where t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.jnl > 0.98
                   // select t.Lds).Sum();

                   // double tmp96 =
                   //     (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                   //      where t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.jnl > 0.96 && t.jnl < 0.98
                   //      select t.Lds).Sum();

                   // double tmp94 =
                   //     (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                   //      where t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.ldl > 0.94 && t.ldl < 0.96
                   //      select t.Lds).Sum();

                   // double tmp90 =
                   //     (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                   //      where t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.ldl > 0.90 && t.ldl < 0.94
                   //      select t.Lds).Sum();

                   // double tmp80 =
                   //     (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                   //      where t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks && t.ldl < 0.90
                   //      select t.Lds).Sum();
      
                }


                this.Data = PieData;

            }
            else if (chartType == 2) //数字 柱状图
            {
                DateTime today = DateTime.Today;
                List<IEnumerable<Data>> BarData = new List<IEnumerable<Data>>();

                if (dataType == 1) //终端故障
                {
                    SmallTitle = name + "  终端故障统计";
                    CharOneTitle = "按故障类型统计";
                    var sp = name.Split('-');
                    var month = Convert.ToInt32(sp[0]);
                    var day = Convert.ToInt32(sp[1]);

                    var d2 = new DateTime(2017, month, day, 12, 0, 1);
                    var d1 = d2.AddDays(-1);

                    List<Data> ErrTypeData = new List<Data>();
                    //faultid -  count
                    var dic = new Dictionary<int, int >();
  
                    for (int i = 1; i < 25; i++)    //按故障类型统计
                    {
                        var Errcount =
                            (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                             where t.FaultId == i  && t.DateCreate > d1.Ticks  && t.DateCreate < d2.Ticks 
                             select t).Count();


                        if (Errcount > 0)
                      {
                          dic.Add(i, Errcount);
                      }
                        //  TmlData.Add(new Data(name, count));
                    }
        

                    var ntg = (from t in dic orderby t.Value descending select t).ToList();
                    foreach (var f in ntg )
                    {
                        var errName = "未知";
                        if (Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary.ContainsKey(f.Key))
                            errName =
                                Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary[f.Key].FaultName;
                        ErrTypeData.Add(new Data(errName, f.Value,f.Key,1,name));
                    }
                    BarData.Add(ErrTypeData);


                    //////////////////////////////////////


                    List<Data> ErrTmlData = new List<Data>();
                    var dicTml = new Dictionary<int, int>();
                    foreach (var f in Wlst .Sr.tmphold .d1Hold .MySelf .Faults )
                    {
                        if(f.RtuId >1100000) continue;
                        if (f.DateCreate > d1.Ticks && f.DateCreate < d2.Ticks)
                        {
                            if (dicTml.ContainsKey(f.RtuId) == false) dicTml.Add(f.RtuId, 1);
                            else dicTml[f.RtuId] += 1;
                        }
                    }

                    //foreach (var g in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                    //{
                    //    if (g.Key >1000000  && g.Key<)
                    //        continue;
                    //    var tmlcount =
                    //       (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                    //        where t.RtuId==g.Value.RtuId && t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks
                    //        select t).Count();

                    //    if (tmlcount > 0)
                    //    {
                    //        dicTml.Add(g.Value.RtuId, tmlcount);
                    //    }
                    //}

                    int xcounttt = 0;
                    var ntgsss = (from t in dicTml orderby t.Value descending select t).ToList();
                    foreach (var f in ntgsss)
                    {
                        xcounttt ++;
                        if(xcounttt >15) break;
                   
                        var tmlName = "未知";
                        var tmlinfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Key);
                        if (tmlinfo == null) continue;
                        tmlName = tmlinfo.RtuName;
                        ErrTmlData.Add(new Data(tmlName, f.Value,f.Key,2,name ));
                    }

                    BarData.Add(ErrTmlData);
                }

                if (dataType == 2) //单灯故障
                {
                    SmallTitle = name + "  单灯故障统计";
                    CharOneTitle = "按故障类型统计";
                    var sp = name.Split('-');
                    var month = Convert.ToInt32(sp[0]);
                    var day = Convert.ToInt32(sp[1]);

                    var d2 = new DateTime(2017, month, day, 12, 0, 1);
                    var d1 = d2.AddDays(-1);

                    List<Data> ErrTypeData = new List<Data>();
                    //faultid -  count
                    var dic = new Dictionary<int, int>();

                    for (int i = 49; i < 65; i++)    //按故障类型统计  单灯
                    {
                        var Errcount =
                            (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                             where t.FaultId == i && t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks
                             select t).Count();


                        if (Errcount > 0)
                        {
                            dic.Add(i, Errcount);
                        }
                        //  TmlData.Add(new Data(name, count));
                    }


                    var ntg = (from t in dic orderby t.Value descending select t).ToList();
                    foreach (var f in ntg)
                    {
                        var errName = "未知";
                        if (!Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary.ContainsKey(f.Key))
                            continue;
   
                        errName =
                                Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary[f.Key].FaultName;

                        ErrTypeData.Add(new Data(errName, f.Value,f.Key,1,name));
                    }
                    BarData.Add(ErrTypeData);


                    //////////////////////////////////////


                    List<Data> ErrTmlData = new List<Data>();
                    var dicTml = new Dictionary<int, int>();
                    foreach (var f in Wlst.Sr.tmphold.d1Hold.MySelf.Faults)
                    {
                        if (f.RtuId < 1500000 || f.RtuId > 1600000) continue;
                        if (f.DateCreate > d1.Ticks && f.DateCreate < d2.Ticks)
                        {
                            if (dicTml.ContainsKey(f.RtuId) == false) dicTml.Add(f.RtuId, 1);
                            else dicTml[f.RtuId] += 1;
                        }
                    }
                    int xcounttt = 0;
                    var ntgsss = (from t in dicTml orderby t.Value descending select t).ToList();
                    foreach (var f in ntgsss)
                    {
                        xcounttt++;
                        if (xcounttt > 15) break;

                        var tmlName = "未知";
                        var tmlinfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Key);
                        if (tmlinfo == null) continue;
                        tmlName = tmlinfo.RtuName;
                        ErrTmlData.Add(new Data(tmlName, f.Value,f.Key,2,name));
                    }

                    BarData.Add(ErrTmlData);
                }

                if (dataType == 4) //耗电量
                {
                    SmallTitle = name + "  耗电量统计";
                    CharOneTitle = "按开关量输出统计";

                    var sp = name.Split('-');
                    var month = Convert.ToInt32(sp[0]);
                    var day = Convert.ToInt32(sp[1]);

                    var d2 = new DateTime(2017, month, day, 12, 0, 1);
                    var d1 = d2.AddDays(-1);

                    List<Data> KxPowerData = new List<Data>();
                    //faultid -  count
                    var dic = new Dictionary<int, double >();
                    
                    for (int i = 1; i < 7; i++)    //按故障类型统计  单灯
                    {
                        dic.Add(i,0);
                        var kxPower =
                            (from t in Wlst.Sr.tmphold.d2Hold.MySelf.Elecs
                             where t.LoopId == i && t.DateCreate > d1.Ticks && t.DateCreate < d2.Ticks
                             select t.Power).Sum();

                        if (kxPower > 0)
                        {
                            dic[i]= kxPower;
                        }
                        //  TmlData.Add(new Data(name, count));
                    }


                    var ntg = (from t in dic orderby t.Value descending select t).ToList();
                    foreach (var f in ntg)
                    {
                        KxPowerData.Add(new Data("K"+f.Key, f.Value));
                    }
                    BarData.Add(KxPowerData);


                    //////////////////////////////////////


                    List<Data> TmlPowerData = new List<Data>();
                    var dicTmlPower = new Dictionary<int, double>();
                    foreach (var f in Wlst.Sr.tmphold.d2Hold.MySelf.Elecs)
                    {
                        if (f.RtuId > 1100000) continue;
                        if (f.DateCreate > d1.Ticks && f.DateCreate < d2.Ticks)
                        {
                            if (dicTmlPower.ContainsKey(f.RtuId) == false) dicTmlPower.Add(f.RtuId, f.Power);
                            else dicTmlPower[f.RtuId] += f.Power;
                        }
                    }
                    int xcounttt = 0;
                    var ntgsss = (from t in dicTmlPower orderby t.Value descending select t).ToList();
                    foreach (var f in ntgsss)
                    {
                        xcounttt++;
                        if (xcounttt > 15) break;

                        var tmlName = "未知";
                        var tmlinfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Key);
                        if (tmlinfo == null) continue;
                        tmlName = tmlinfo.RtuName;
                        TmlPowerData.Add(new Data(tmlName, f.Value,f.Key,6,name ));
                    }

                    BarData.Add(TmlPowerData);



                }


                this.BarData = BarData;

            }

        }

        private void InitializePalettePresets()
        {
            List<ChartPalette> palettes = new List<ChartPalette>();
            palettes.Add(ChartPalettes.Arctic);
            palettes.Add(ChartPalettes.Autumn);
            palettes.Add(ChartPalettes.Cold);
            palettes.Add(ChartPalettes.Flower);
            palettes.Add(ChartPalettes.Forest);
            palettes.Add(ChartPalettes.Grayscale);
            palettes.Add(ChartPalettes.Grayscale);
            palettes.Add(ChartPalettes.Ground);
            palettes.Add(ChartPalettes.Lilac);
            palettes.Add(ChartPalettes.Natural);
            palettes.Add(ChartPalettes.Office2013);
            palettes.Add(ChartPalettes.Pastel);
            palettes.Add(ChartPalettes.Rainbow);
            palettes.Add(ChartPalettes.Spring);
            palettes.Add(ChartPalettes.Summer);
            palettes.Add(ChartPalettes.Warm);
            palettes.Add(ChartPalettes.Windows8);
            palettes.Add(ChartPalettes.VisualStudio2013);

            this.Palettes = palettes;
            this.Palette = ChartPalettes.Windows8;
        }


        private void InitializeData()
        {
            //this.BarData = this.CreateBarData();
            this.LineData = this.CreateLineData();
            //this.Data = this.CreatePieData();
        }

        //private IEnumerable<IEnumerable<Data>> CreateBarData()
        //{
        //    DateTime today = DateTime.Today;
        //    List<IEnumerable<Data>> BarData = new List<IEnumerable<Data>>();

        //    List<Data> FirefoxData = new List<Data>();
        //    FirefoxData.Add(new Data(today.AddYears(-2), 0.464));
        //    FirefoxData.Add(new Data(today.AddYears(-1), 0.435));
        //    FirefoxData.Add(new Data(today, 0.397));

        //    List<Data> ChromeData = new List<Data>();
        //    ChromeData.Add(new Data(today.AddYears(-2), 0.098));
        //    ChromeData.Add(new Data(today.AddYears(-1), 0.224));
        //    ChromeData.Add(new Data(today, 0.305));

        //    List<Data> IEdata = new List<Data>();
        //    IEdata.Add(new Data(today.AddYears(-2), 0.372));
        //    IEdata.Add(new Data(today.AddYears(-1), 0.275));
        //    IEdata.Add(new Data(today, 0.229));

        //    List<Data> SafariData = new List<Data>();
        //    SafariData.Add(new Data(today.AddYears(-2), 0.36));
        //    SafariData.Add(new Data(today.AddYears(-1), 0.38));
        //    SafariData.Add(new Data(today, 0.40));

        //    List<Data> OperaData = new List<Data>();
        //    OperaData.Add(new Data(today.AddYears(-2), 0.23));
        //    OperaData.Add(new Data(today.AddYears(-1), 0.22));
        //    OperaData.Add(new Data(today, 0.22));

        //    BarData.Add(FirefoxData);
        //    BarData.Add(ChromeData);
        //    BarData.Add(IEdata);
        //    BarData.Add(SafariData);
        //    BarData.Add(OperaData);

        //    return BarData;
        //}

        private IEnumerable<IEnumerable<Data>> CreateLineData()
        {
            DateTime today = DateTime.Today;
            List<IEnumerable<Data>> LineData = new List<IEnumerable<Data>>();
   

            //终端故障
            List<Data> TmlErrData = new List<Data>();
            var dayst = DateTime.Now.AddDays(-20);
            var dt1 = new DateTime(dayst.Year, dayst.Month, dayst.Day, 12, 0, 1);
            for (int i=0;i<20;i++)
            {
                var d1 = dt1.AddDays(i).Ticks;
                var d2 = dt1.AddDays(i + 1).Ticks;
                var name = dt1.AddDays(i).Month + "-" + dt1.AddDays(i).Day;
                var count =
                    (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                     where t.FaultId < 25 && t.DateCreate > d1 && t.DateCreate < d2
                     select t).Count();
                TmlErrData.Add(new Data(name, count));
            }
            //单灯故障
            List<Data> SluErrData = new List<Data>();
            for (int i = 0; i < 20; i++)
            {
                var d1 = dt1.AddDays(i).Ticks;
                var d2 = dt1.AddDays(i + 1).Ticks;
                var name = dt1.AddDays(i).Month + "-" + dt1.AddDays(i).Day;
                var count =
                    (from t in Wlst.Sr.tmphold.d1Hold.MySelf.Faults
                     where t.FaultId < 65 && t.FaultId>49 && t.DateCreate > d1 && t.DateCreate < d2
                     select t).Count();
                SluErrData.Add(new Data(name, count));
            }

            //在线率
            List<Data> OnlineData = new List<Data>();
            for (int i = 0; i < 20; i++)
            {
                var d1 = dt1.AddDays(i).Ticks;
                var d2 = dt1.AddDays(i + 1).Ticks;
                var name = dt1.AddDays(i).Month + "-" + dt1.AddDays(i).Day;
                double zxs = 0.0;
                double sum = 0.0;
                    //(from t in Wlst.Sr.tmphold.d5Hold.MySelf.Zxls
                    // where t.DateCreate > d1 && t.DateCreate < d2
                    // select t.Zxs).Sum();
                foreach (var g in Wlst.Sr.tmphold.d5Hold.MySelf.Zxls)
                {
                    if (g.DateCreate>d1 && g.DateCreate<d2)
                    {
                        zxs += g.Zxs;
                        sum += g.Sum;
                    }
                }



                //double  sum =
                //    (from t in Wlst.Sr.tmphold.d5Hold.MySelf.Zxls
                //     where t.DateCreate > d1 && t.DateCreate < d2
                //     select t.Sum).Sum();
                double tmp = zxs/sum;
                OnlineData.Add(new Data(name, tmp));
            }



            //耗电量
            List<Data> PowerData = new List<Data>();
            for (int i = 0; i < 20; i++)
            {
                var d1 = dt1.AddDays(i).Ticks;
                var d2 = dt1.AddDays(i + 1).Ticks;
                var name = dt1.AddDays(i).Month + "-" + dt1.AddDays(i).Day;
                var count =
                    (from t in Wlst.Sr.tmphold.d2Hold.MySelf.Elecs
                     where  t.DateCreate > d1 && t.DateCreate < d2
                     select t.Power ).Sum( );
                PowerData.Add(new Data(name, count));
            }


            //亮灯率
            List<Data> LdlData = new List<Data>();
            for (int i = 0; i < 20; i++)
            {
                var d1 = dt1.AddDays(i).Ticks;
                var d2 = dt1.AddDays(i + 1).Ticks;
                var name = dt1.AddDays(i).Month + "-" + dt1.AddDays(i).Day;
                double  lds =
                    (from t in Wlst.Sr.tmphold.d4Hold.MySelf.Ldls
                     where t.DateCreate > d1 && t.DateCreate < d2
                     select t.Lds).Sum();
                double  sum =
                    (from t in Wlst.Sr.tmphold.d4Hold.MySelf.Ldls
                     where t.DateCreate > d1 && t.DateCreate < d2
                     select t.Sum).Sum();
                double tmp = lds/sum;
                LdlData.Add(new Data(name, tmp));
            }

            //节能率
            List<Data> JnlData = new List<Data>();
            for (int i = 0; i < 20; i++)
            {
                var d1 = dt1.AddDays(i).Ticks;
                var d2 = dt1.AddDays(i + 1).Ticks;
                var name = dt1.AddDays(i).Month + "-" + dt1.AddDays(i).Day;
                double usePower=0.0;
                foreach (var g in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls)
                {
                    if (g.DateCreate > d1 && g.DateCreate < d2)
                    {
                        double usepowertmp = g.Power*(1 - g.jnl);
                        usePower += usepowertmp;
                    }
                }
                var sum =
                    (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                     where t.DateCreate > d1 && t.DateCreate < d2
                     select t.Power).Sum();
                double tmp =1- usePower/sum;
                JnlData.Add(new Data(name,tmp));
            }


            //WinXPData.Add(new Data("Q2,2010", 0.5809));
            //WinXPData.Add(new Data("Q3,2010", 0.5179));
            //WinXPData.Add(new Data("Q4,2010", 0.5532));
            //WinXPData.Add(new Data("Q1,2011", 0.4803));


            LineData.Add(TmlErrData);
            LineData.Add(SluErrData);
            LineData.Add(OnlineData);
            LineData.Add(PowerData);
            LineData.Add(LdlData);
            LineData.Add(JnlData);

            return LineData;
        }

        //private IEnumerable<Data> CreatePieData()
        //{
        //    List<Data> PieData = new List<Data>(4);
        //    PieData.Add(new Data("Google", 82.35));
        //    PieData.Add(new Data("Yahoo!", 6.69));
        //    PieData.Add(new Data("Baidu", 5.12));
        //    PieData.Add(new Data("Others", 4.71));

        //    return PieData;
        //}

        public void ShowDetailView(int chartType, int dataType,string name)
        {
            if (chartType == 1) //饼图
            {
                var tvx = new StatisticsViewModel(chartType, dataType,name);
                var tmp = new PieView();
                tmp.SetContext(tvx, name);
                tmp.ShowDialog();
            }
            if (chartType == 2) //柱状图
            {
                var tvx = new StatisticsViewModel(chartType, dataType, name);
                var tmp = new BarView();
                tmp.SetContext(tvx,name);
                tmp.ShowDialog();
            }

        }

        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {

                var lined = CreateLineData().ToList();
                if (lined.Count == 0) return;

                foreach (var g in lined)
                {

                    var lsttitle = new List<Object>();
                    lsttitle.Add("序号");
                    lsttitle.Add("统计类别");
                    lsttitle.Add("数值");
                    var lstobj = new List<List<object>>();
                    if (g== null) continue;
                    var da = g.ToList();
                    if (da.Count == 0) continue;
                    int i = 0;

                    foreach (var data in da)
                    {
                        i++;
                        var tmp = new List<object>();
                        tmp.Add(i);
                        tmp.Add(data.Category);
                        tmp.Add(data.Value);
                        lstobj.Add(tmp);
                    }
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                }

                //if (xVisi1 == true)
                //{
                //    lsttitle.Add("总灯头");
                //}

                //if (xVisi2 == true)
                //{
                //    lsttitle.Add(Name1);
                //    lsttitle.Add(Name2);
                //}

                //if (xVisi3 == true)
                //{
                //    lsttitle.Add("回路名称");
                //    lsttitle.Add("电量");
                //}

                

                //foreach (var g in Records)
                //{
                //    var tmp = new List<object>();
                //    tmp.Add(g.Index);
                //    tmp.Add(g.PhyId);
                //    tmp.Add(g.RtuName);

                //    if (xVisi1 == true)
                //    {
                //        tmp.Add(g.ZDT);
                //    }

                //    if (xVisi2 == true)
                //    {
                //        tmp.Add(g.Count);
                //        tmp.Add(g.Ratio);
                //    }

                //    if (xVisi3 == true)
                //    {
                //        tmp.Add(g.LoopName);
                //        tmp.Add(g.Power);
                //    }


                //    lstobj.Add(tmp);
                //}
                //Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            return true;
        }

        #endregion


    }
}
