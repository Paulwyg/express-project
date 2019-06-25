using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.EmergencyDispatch.RtuLdlSet.ViewModels
{
    public partial class ItemOne : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private string xx1;
        private string xx2;
        private string xx3;
        private string xx4;
        private string xx5;
        private string xx6;
        private string xx7;
        private string xx8;
        private int indexxx;
        public int RtuId;

        #region
        public int  Index
        {
            get { return indexxx; }
            set
            {
                if (indexxx == value) return;
                indexxx = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }
        public string ShowId
        {
            get { return xx1; }
            set
            {
                if (xx1 == value) return;
                xx1 = value;
                this.RaisePropertyChanged(() => this.ShowId);
            }
        }

        public string Name
        {
            get { return xx2; }
            set
            {
                if (xx2 == value) return;
                xx2 = value;
                this.RaisePropertyChanged(() => this.Name);
            }
        }

        public string GrpName
        {
            get { return xx3; }
            set
            {
                if (xx3 == value) return;
                xx3 = value;
                this.RaisePropertyChanged(() => this.GrpName);
            }
        }


        public string X1
        {
            get { return xx4; }
            set
            {
                if (xx4 == value) return;
                xx4 = value;
                this.RaisePropertyChanged(() => this.X1);
            }
        }

        public string X2
        {
            get { return xx5; }
            set
            {
                if (xx5 == value) return;
                xx5 = value;
                this.RaisePropertyChanged(() => this.X2);
            }
        }

        public string X3
        {
            get { return xx8; }
            set
            {
                if (xx8 == value) return;
                xx8 = value;
                this.RaisePropertyChanged(() => this.X3);
            }
        }

        public string T
        {
            get { return xx6; }
            set
            {
                if (xx6 == value) return;
                xx6 = value;
                this.RaisePropertyChanged(() => this.T );
            }
        }

        public string Remark
        {
            get { return xx7; }
            set
            {
                if (xx7 == value) return;
                xx7 = value;
                this.RaisePropertyChanged(() => this.Remark);
            }
        }

        #endregion

        private Visibility visi;
        public Visibility Visi
        {
            get { return visi; }
            set
            {
                if (visi == value) return;
                visi = value;
                this.RaisePropertyChanged(() => this.Visi);
            }
        }
        public ItemOne(int rtuId, string grpName,int indexx)
        {
            Index = indexx;
            RtuId = rtuId;
            X1 = "--";
            X2 = "--";
            X3 = "--";
            T = "--";
            GrpName = grpName;

            var st = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById(rtuId);
            if (st != null)
            {
                ShowId = st.RtuPhyId .ToString("D4");
                Name = st.RtuName ;
            }
            else
            {
                ShowId = RtuId + "";
                Name = RtuId + "";
            }
            Visi = Visibility.Collapsed;
        }


        private ObservableCollection<ItemLoop> _measurePatrolData;

        public ObservableCollection<ItemLoop> Loops
        {
            get
            {
                if (_measurePatrolData == null)
                    _measurePatrolData = new ObservableCollection<ItemLoop>();

                return _measurePatrolData;
            }
            set
            {
                if (value == _measurePatrolData) return;
                _measurePatrolData = value;
                this.RaisePropertyChanged(() => this.Loops);
            }
        }
    }

    public partial class ItemOne
    {
        public long LastSetTime = 0;
        public void SetNewData(RtuNewDataInfo info)
        {
            LastSetTime = info.DateCreate.Ticks;
            bool seted = false;
            if (One == null)
            {
                One = info;
                seted = true;
                X1 = "√";
            }
            if (Two == null && seted == false)
            {
                Two = info;
                seted = true;
                X2 = "√";
            }
            if (Three == null && seted == false)
            {
                Three = info;
                seted = true;
                X3 = "√";
            }
            if (Target == null && seted == false)
            {
                Target = info;
                seted = true;
                T = "√";
            }

            if (One != null && Two != null && Three != null && Target != null)
            {
                var st = CanCalLdl();
                if (st == false) return;
                Remark = "";
                CalLdl();
            }
        }

        public bool NeedNewData()
        {
            if (One != null && Two != null && Three != null && Target != null)
            {
                return false;
            }
            return true;
        }

        public void CleanData(int index)
        {
            if (index == 1)
            {
                X1 = "--";
                One = null;
            }
            if (index == 2)
            {
                X2 = "--";
                Two = null;
            }
            if (index == 3)
            {
                X3 = "--";
                Three = null;
            }
            if (index == 4)
            {
                T = "--";
                Target = null;
            }
            this.Loops.Clear();
            Remark = "";
            Visi = Visibility.Collapsed;
        }

        public RtuNewDataInfo One = null;
        public RtuNewDataInfo Two = null;
        public RtuNewDataInfo Three = null;
        public RtuNewDataInfo Target = null;


        public bool CanCalLdl()
        {
            var loopx = GetLstAmpUsed();
            if (One != null && Two != null && Three != null && Target != null)
            {
                foreach (var f in One.LstNewLoopsData)
                {
                    if (!loopx.Contains(f.LoopId)) continue;
                    if (f.Power < 0.1 || f.V < 0.1)
                    {
                        X1 = "--";
                        One = null;
                        Remark = "回路" + f.LoopId + " 电压或功率小于0.1,已清除该数据，请重新选测...";
                        return false;
                    }
                }
                foreach (var f in Two.LstNewLoopsData)
                {
                    if (!loopx.Contains(f.LoopId)) continue;
                   
                    if (f.Power < 0.1 || f.V < 0.1)
                    {
                        Remark = "回路" + f.LoopId + " 电压或功率小于0.1,已清除该数据，请重新选测...";
                        X2 = "--";
                        Two = null;
                        return false;
                    }
                }
                foreach (var f in Three.LstNewLoopsData)
                {
                    if (!loopx.Contains(f.LoopId)) continue;
                   
                    if (f.Power < 0.1 || f.V < 0.1)
                    {
                        Remark = "回路" + f.LoopId + " 电压或功率小于0.1,已清除该数据，请重新选测...";
                        X3 = "--";
                        Three = null;
                        return false;
                    }
                }
                foreach (var f in Target.LstNewLoopsData)
                {
                    if (!loopx.Contains(f.LoopId)) continue;
                   
                    if (f.Power < 0.1 || f.V < 0.1)
                    {
                        Remark = "回路" + f.LoopId + " 电压或功率小于0.1,已清除该数据，请重新选测...";
                        T = "--";
                        Target = null;
                        return false;
                    }
                }

                return true;
            }
            Remark = "未采集到三次最新数据...";
            Visi = Visibility.Collapsed;
            return false;

        }


        private List< int > GetLstAmpUsed()
        {
            var pars = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( RtuId);
            if (pars == null) return new List<int>();
            var ntpars = pars as Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu ;
            if (ntpars == null) return new List<int>();
            var rtn = new List<int>();
            foreach (var f in ntpars.WjLoops   .Values  )
            {
                if (f.SwitchOutputId  < 1) continue;
                if (f.VectorMoniliang < 1) continue;
                if(f.CurrentRange==0 )continue;
                rtn.Add(f.LoopId);
            }
            return rtn;
        }


        public void CalLdl()
        {
            var rtn = CanCalLdl();
            if (rtn == false) return;

            Loops.Clear();
            try
            {
                var dirp = new Dictionary<int, double>();
                var loops = GetLstAmpUsed();

                var dirtar = new Dictionary<int, double>();
                foreach (var f in One.LstNewLoopsData)
                {
                    if (!loops.Contains( f.LoopId)) continue;
                    var tmp = f.V*f.V/f.Power;
                    if (!dirp.ContainsKey(f.LoopId)) dirp.Add(f.LoopId, tmp);
                    else dirp[f.LoopId] += tmp;
                }
                foreach (var f in Two.LstNewLoopsData)
                {
                    if (!loops.Contains( f.LoopId)) continue;
                    
                    var tmp = f.V*f.V/f.Power;
                    if (!dirp.ContainsKey(f.LoopId)) dirp.Add(f.LoopId, tmp);
                    else dirp[f.LoopId] += tmp;
                }
                foreach (var f in Three.LstNewLoopsData)
                {
                    if (!loops.Contains( f.LoopId)) continue;
                    
                    var tmp = f.V*f.V/f.Power;
                    if (!dirp.ContainsKey(f.LoopId)) dirp.Add(f.LoopId, tmp);
                    else dirp[f.LoopId] += tmp;
                }

                foreach (var f in Target.LstNewLoopsData)
                {
                    if (!loops.Contains( f.LoopId)) continue;
                    
                    var tmp = f.V*f.V/f.Power;
                    if (!dirtar.ContainsKey(f.LoopId)) dirtar.Add(f.LoopId, tmp);
                    else dirtar[f.LoopId] = tmp;
                }

                var nts = (from t in dirp orderby t.Key ascending select t).ToList();
                foreach (var f in nts)
                {
                    var xgt = Math.Round(f.Value/3, 3);
                    double tar = 0;
                    if (dirtar.ContainsKey(f.Key) && dirtar[f.Key] > 0)
                    {

                        tar = Math.Round(xgt/dirtar[f.Key], 2);
                    }
                    Loops.Add(new ItemLoop() {LdlValue = xgt, LoopId = f.Key, TargetTest = tar});
                }
                Remark = "";
                bool find = false;
                foreach (var f in Loops)
                {
                    if (f.TargetTest > 0.85) continue;
                    Remark += "回路：" + f.LoopId + "亮灯率为 " + (f.TargetTest*100) + " %;";
                    find = true;
                }
                if (find == false)
                {
                    Remark += "所有测试回路亮灯率均高于 85% .";
                }
            }
            catch (Exception ex)
            {
                One = null;
                Two = null;
                Three = null;
                X1 = "--";
                X2 = "--";
                X3 = "--";
                Remark = "计算基础数据出错，请重新采集进行计算...";
            }
            Visi = Visibility.Visible ;
        }


        public void CleanData()
        {
            Loops.Clear();
            X1 = "--";
            X2 = "--";
            X3 = "--";
            T = "--";
            One = null;
            Two = null;
            Three = null;
            Target = null;
            Visi = Visibility.Collapsed;
        }
    }



    public class ItemLoop : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region

        private int xx1;
        private double xx2;
        private double xx3;

        public int LoopId
        {
            get { return xx1; }
            set
            {
                if (xx1 == value) return;
                xx1 = value;
                this.RaisePropertyChanged(() => this.LoopId);
            }
        }

        public double LdlValue
        {
            get { return xx2; }
            set
            {
                if (xx2 == value) return;
                xx2 = value;
                this.RaisePropertyChanged(() => this.LdlValue);
            }
        }

        public double TargetTest
        {
            get { return xx3; }
            set
            {
                if (xx3 == value) return;
                xx3 = value;
                this.RaisePropertyChanged(() => this.TargetTest);
                this.RaisePropertyChanged(() => this.TargetTestX);
            }
        }

        public int TargetTestX
        {
            get { return Convert .ToInt32( TargetTest*100); }
        }

        #endregion
    }
}
