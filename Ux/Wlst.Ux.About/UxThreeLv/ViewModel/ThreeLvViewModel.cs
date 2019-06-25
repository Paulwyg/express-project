using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.About.UxShowErr.Sevices;
using Wlst.Ux.About.UxShowErr.ViewModel;
using Wlst.Ux.About.UxThreeLv.Services;

namespace Wlst.Ux.About.UxThreeLv.ViewModel
{
    [Export(typeof(IIUxThreeLvModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ThreeLvViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITab, IIUxThreeLvModule
    {



        public void NavOnLoad(params object[] parsObjects)
        {
            if (parsObjects.Length == 3)
            {
                string[] _value = parsObjects[0].ToString().Split('-');

                var dt1 = new DateTime(DateTime.Now.Year, Convert.ToInt32(_value[0]), Convert.ToInt32(_value[1]), 12, 0, 1);
                xTitle = dt1.ToString("yyyy-MM-dd");
                int _op = Convert.ToInt32(parsObjects[1]);
                
                if ((_op == 3) || (_op == 4) || (_op == 5))
                {
                    string[] _value1 = parsObjects[2].ToString().Split('-');

                    double _min = Convert.ToDouble(_value1[0].Substring(0, _value1[0].Length - 1))/100;
                    double _max = Convert.ToDouble(_value1[1].Substring(0, _value1[1].Length - 1))/100;

                    if (_op == 3)
                    {
                        xTitle += "   节能率";
                        xVisi1 = false;
                        xVisi2 = true;
                        xVisi3 = false;
                        Name1 = "电量";
                        Name2 = "节能率";
                    }
                    else if (_op == 4)
                    {
                        xTitle += "   亮灯率";
                        xVisi1 = true;
                        xVisi2 = true;
                        xVisi3 = false;
                        Name1 = "亮灯数";
                        Name2 = "亮灯率";
                    }
                    else if (_op == 5)
                    {
                        xTitle += "   在线率";
                        xVisi1 = true;
                        xVisi2 = true;
                        xVisi3 = false;
                        Name1 = "在线数";
                        Name2 = "在线率";
                    }

                    Update_Record(_op, dt1, _min, _max);
                }
                else if (_op == 6)
                {
                    xTitle += "   耗电量";
                    xVisi1 = false;
                    xVisi2 = false;
                    xVisi3 = true ;
                    Name1 = "";
                    Name2 = "";

                    int cRtuId = Convert.ToInt32(parsObjects[2]);

                    Update_Record(_op, dt1, cRtuId);
                }



            }
        }


        public void OnUserHideOrClosing()
        {

        }

        private void Update_Record(int _op, DateTime _dt, int rtuId)
        {
            Records.Clear();


            var d2 = _dt.Ticks;
            var d1 = _dt.AddDays(-1).Ticks;


            if (rtuId != -1)
            {
                var lst =
                    (from t in Wlst.Sr.tmphold.d2Hold.MySelf.Elecs
                     where t.RtuId == rtuId && t.DateCreate > d1 && t.DateCreate < d2
                     select t).ToList();

                int m = 1;

                foreach (var t in lst)
                {


                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);


                    if (info != null)
                    {
                        Records.Add(new ThreeLvDefine
                        {

                            Index = m++,
                            PhyId = info.RtuPhyId,
                            RtuName = info.RtuName,
                            LoopName = info.GetLoopName(t.LoopId),
                            Power = t.Power.ToString("0.##")
                        });
                    }
                }
            }
        }

        private void Update_Record(int _op, DateTime _dt, double _min, double _max)
        {
            var d2 = _dt.Ticks;
            var d1 = _dt.AddDays(-1).Ticks;

            Records.Clear();

            int m = 1;
            
            if (_op == 3)
            
            {
                var lst =
                    (from t in Wlst.Sr.tmphold.d3Hold.MySelf.Jnls
                     where t.jnl > _min && t.jnl < _max && t.DateCreate > d1 && t.DateCreate < d2
                     select t).ToList();
                
              

                foreach (var t in lst)
                {

                     var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);

                    Records.Add(new ThreeLvDefine
                                    {
                                        Index = m++,
                                        PhyId = info.RtuPhyId,
                                       RtuName = info.RtuName,
                                       Count = t.Power.ToString("0.##"),
                                       Ratio = t.jnl.ToString("0.##")
                                    });

                }
            }


            if (_op == 4)
            {
                var lst =
                    (from t in Wlst.Sr.tmphold.d4Hold.MySelf.Ldls
                     where t.ldl > _min && t.ldl < _max && t.DateCreate > d1 && t.DateCreate < d2
                     select t).ToList();



                foreach (var t in lst)
                {

                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);

                    Records.Add(new ThreeLvDefine
                    {
                        Index = m++,
                        PhyId = info.RtuPhyId,
                        RtuName = info.RtuName,
                        ZDT = Convert.ToString(  t.Sum),
                        Count = t.Lds.ToString("0.##"),
                        Ratio = t.ldl.ToString("0.##")
                    });

                }
            }

            if (_op == 5)
            {
                var lst =
                    (from t in Wlst.Sr.tmphold.d5Hold.MySelf.Zxls
                     where t.zxl > _min && t.zxl < _max && t.DateCreate > d1 && t.DateCreate < d2
                     select t).ToList();



                foreach (var t in lst)
                {

                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);

                    Records.Add(new ThreeLvDefine
                    {
                        Index = m++,
                        PhyId = info.RtuPhyId,
                        RtuName = info.RtuName,
                        ZDT = Convert.ToString(t.Sum),
                        Count = t.Zxs.ToString("0.##"),
                        Ratio = t.zxl.ToString("0.##")
                    });

                }
            }
        }


        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "细节显示"; }
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



        private string _title;

        public string xTitle
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    this.RaisePropertyChanged(() => this.xTitle);
                }
            }
        }


        private string _name1;

        public string Name1
        {
            get { return _name1; }
            set
            {
                if (value != _name1)
                {
                    _name1 = value;
                    this.RaisePropertyChanged(() => this.Name1);
                }
            }
        }

        private string _name2;

        public string Name2
        {
            get { return _name2; }
            set
            {
                if (value != _name2)
                {
                    _name2 = value;
                    this.RaisePropertyChanged(() => this.Name2);
                }
            }
        }

        private bool _xVisi1 = false;

        public bool xVisi1
        {
            get { return _xVisi1; }
            set
            {
                if (value == _xVisi1) return;
                _xVisi1 = value;
                RaisePropertyChanged(() => xVisi1);
            }
        }

        private bool _xVisi2 = false;

        public bool xVisi2
        {
            get { return _xVisi2; }
            set
            {
                if (value == _xVisi2) return;
                _xVisi2 = value;
                RaisePropertyChanged(() => xVisi2);
            }
        }

        private bool _xVisi3 = false;

        public bool xVisi3
        {
            get { return _xVisi3; }
            set
            {
                if (value == _xVisi3) return;
                _xVisi3 = value;
                RaisePropertyChanged(() => xVisi3);
            }
        }

        #region Records

        private ObservableCollection<ThreeLvDefine> _record;

        public ObservableCollection<ThreeLvDefine> Records
        {
            get { return _record ?? (_record = new ObservableCollection<ThreeLvDefine>()); }
            set
            {
                if (_record != value)
                {
                    _record = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }
        }

        #endregion


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
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("终端地址");
                lsttitle.Add("终端名称");

                if (xVisi1 == true)
                {
                    lsttitle.Add("总灯头");
                }

                if (xVisi2 == true)
                {
                    lsttitle.Add(Name1);
                    lsttitle.Add(Name2);
                }

                if (xVisi3 == true)
                {
                    lsttitle.Add("回路名称");
                    lsttitle.Add("电量");
                }

                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);

                    if (xVisi1 == true)
                    {
                        tmp.Add(g.ZDT);
                    }

                    if (xVisi2 == true)
                    {
                        tmp.Add(g.Count);
                        tmp.Add(g.Ratio);
                    }

                    if (xVisi3 == true)
                    {
                        tmp.Add(g.LoopName);
                        tmp.Add(g.Power);
                    }


                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
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
