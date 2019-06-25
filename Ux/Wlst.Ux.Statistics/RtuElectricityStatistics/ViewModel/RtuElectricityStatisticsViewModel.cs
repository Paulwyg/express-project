using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Statistics.RtuElectricityStatistics.Services;

namespace Wlst.Ux.Statistics.RtuElectricityStatistics.ViewModel
{
    [Export(typeof(IIRtuElectricityStatisticsViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RtuElectricityStatistics : EventHandlerHelperExtendNotifyProperyChanged, IITab, IIRtuElectricityStatisticsViewModel
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

        public RtuElectricityStatistics()
        {
            SmallTitle = "";
            this.InitializeData();
            this.InitializePalettePresets();
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
        private IEnumerable<IEnumerable<RtuElectricityData>> CreateLineData()
        {
            DateTime today = DateTime.Today;
            List<IEnumerable<RtuElectricityData>> LineData = new List<IEnumerable<RtuElectricityData>>();


            List<RtuElectricityData> k1Data = new List<RtuElectricityData>();
            k1Data.Add(new RtuElectricityData("Q2,2010", 0.5809));
            k1Data.Add(new RtuElectricityData("Q3,2010", 1.5179));
            k1Data.Add(new RtuElectricityData("Q4,2010", 2.5532));
            k1Data.Add(new RtuElectricityData("Q1,2011", 3.4803));

            List<RtuElectricityData> k2Data = new List<RtuElectricityData>();
            k2Data.Add(new RtuElectricityData("Q2,2010", 4.5809));
            k2Data.Add(new RtuElectricityData("Q3,2010", 5.5179));
            k2Data.Add(new RtuElectricityData("Q4,2010", 6.5532));
            k2Data.Add(new RtuElectricityData("Q1,2011", 7.4803));

            List<RtuElectricityData> k3Data = new List<RtuElectricityData>();
            k3Data.Add(new RtuElectricityData("Q2,2010", 8.5809));
            k3Data.Add(new RtuElectricityData("Q3,2010", 9.5179));
            k3Data.Add(new RtuElectricityData("Q4,2010", 10.5532));
            k3Data.Add(new RtuElectricityData("Q1,2011", 11.4803));

            List<RtuElectricityData> k4Data = new List<RtuElectricityData>();
            k4Data.Add(new RtuElectricityData("Q2,2010", 12.5809));
            k4Data.Add(new RtuElectricityData("Q3,2010", 13.5179));
            k4Data.Add(new RtuElectricityData("Q4,2010", 14.5532));
            k4Data.Add(new RtuElectricityData("Q1,2011", 15.4803));

            List<RtuElectricityData> k5Data = new List<RtuElectricityData>();
            k5Data.Add(new RtuElectricityData("Q2,2010", 16.5809));
            k5Data.Add(new RtuElectricityData("Q3,2010", 17.5179));
            k5Data.Add(new RtuElectricityData("Q4,2010", 18.5532));
            k5Data.Add(new RtuElectricityData("Q1,2011", 19.4803));

            List<RtuElectricityData> k6Data = new List<RtuElectricityData>();
            k6Data.Add(new RtuElectricityData("Q2,2010", 20.5809));
            k6Data.Add(new RtuElectricityData("Q3,2010", 21.5179));
            k6Data.Add(new RtuElectricityData("Q4,2010", 22.5532));
            k6Data.Add(new RtuElectricityData("Q1,2011", 23.4803));

            LineData.Add(k1Data);
            LineData.Add(k2Data);
            LineData.Add(k3Data);
            LineData.Add(k4Data);
            LineData.Add(k5Data);
            LineData.Add(k6Data);

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

        public void ShowDetailView(int chartType, int dataType, string name)
        {

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
                    if (g == null) continue;
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
