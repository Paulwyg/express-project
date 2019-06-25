using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj2090Module.SrInfo;
using Wlst.Ux.Wj2090Module.TimeInfo.Services;
using Wlst.Ux.Wj2090Module.TimeInfo.TimeInfoSet.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.TimeInfo.ViewModel
{
    [Export(typeof (IITimeInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoSetVm : Wlst.Cr .Core .EventHandlerHelper .EventHandlerHelperExtendNotifyProperyChanged , Services.IITimeInfoSet
    {

        public TimeInfoSetVm ()
        {
            this.AddEventFilterInfo(Wj2090Module.Services.EventIdAssign.TimeInfoUpdateId, PublishEventType.Core, true);
            this.AddEventFilterInfo(Wj2090Module.Services.EventIdAssign.TimeInfoRequestId , PublishEventType.Core, true);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            if (DateTime.Now.Ticks - _dtCmdSaveTimeTable.Ticks < 100000000)
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "更新成功.");
            else
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "更新数据，界面执行刷新...");

            }
            this.NavOnLoad();
        }

        

        private List<string> CurrentModeName = new List<string>();
        private const string currentModeNameFile = "curModeName"; 
        private void LoadModeNameFromLocal(string xmlFileName)
        {
            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            //lvf  2018年5月16日16:47:07  采用根目录路径
            string dir = Directory.GetCurrentDirectory() + "\\FileSync\\SingleLamp";
            
            //string dir = "D:\\CETC50\\FileSync\\SingleLamp";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (File.Exists(xmlFileName))
                path = xmlFileName;

            CurrentModeName.Clear();

            info = Wlst.Cr.Coreb.Servers.XmlReadSave.Read(path);

            int AreaAmount = 0;

            if (info.ContainsKey("AreaAmount"))
            {
                AreaAmount = Convert.ToInt32(info["AreaAmount"]);

                if (AreaAmount< AreaName.Count )
                {
                    int num = AreaName.Count - AreaAmount;
                    for ( int i =num;i>0;i--)
                    {
                        CurrentModeName.Add("新模式1");
                    }
                }

                for (int i = 0; i < AreaAmount; i++)
                {
                    if (info.ContainsKey("ModeName" + i))
                    {
                        CurrentModeName.Add(info["ModeName" + i]);
                    }
                    else
                    {
                        CurrentModeName.Add("新模式1");
                    }
                }
            }
            else
            {
                for (int i = 0; i < AreaName.Count; i++)
                {
                    CurrentModeName.Add("新模式1");
                }
            }
        }

        private void Create_New_Time_TextFile()
        {
            //lvf  2018年5月16日16:47:07  采用根目录路径
            string dirtmp = Directory.GetCurrentDirectory() + "\\FileSync\\SingleLamp";
            var dir = new DirectoryInfo(dirtmp);

            foreach (FileInfo f in dir.GetFiles()) //查找文件
            {
                if (f.Name.Contains("EnsureThisFileHasContent_xx"))
                {
                    File.Delete(f.FullName);
                }
            }

            string pathString = dirtmp+"\\EnsureThisFileHasContent_xx" + DateTime.Now.Ticks +
                                ".txt";


            try
            {
                FileStream fs = new FileStream(pathString, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write("xxx");
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveModeNameToLocal(string xmlFileName)
        {
            var info = new Dictionary<string, string>();

            info.Add("AreaAmount", Convert.ToString(CurrentModeName.Count));

            for (int i = 0; i < CurrentModeName.Count; i++ )
            {
                info.Add("ModeName" + i, CurrentModeName[i]);
            }

            try
                {
                    if (!xmlFileName.EndsWith(".xml"))
                    {
                        xmlFileName += ".xml";
                    }

                    //string dir = "D:\\CETC50\\FileSync\\SingleLamp";
                    //lvf  2018年5月16日16:47:07  采用根目录路径
                    string dir = Directory.GetCurrentDirectory() + "\\FileSync\\SingleLamp";
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    string path = dir + "\\" + xmlFileName;
                    if (File.Exists(path)) File.Delete(path);

                    Wlst.Cr.Coreb.Servers.XmlReadSave.Save(info, path);
                }
                catch (Exception ex)
                {
                }
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            ModeVisi = Visibility.Collapsed;

            //lvf 2018年4月3日08:45:01   奎屯特殊功能   9999.xml - key:1
            //if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(9999, 1, false)) ModeVisi = Visibility.Visible;
            //if (File.Exists("Config\\奎屯.txt")) ModeVisi = Visibility.Visible;


            //string path = Directory.GetCurrentDirectory() + "\\Config" + "\\" + "City.txt";
            //string rrr = "";
            //if (File.Exists(path))
            //{
            //    var sr = new StreamReader(path, Encoding.Default);

            //    rrr = sr.ReadLine();

            //    sr.Close();
            //}
            //if (rrr == "3") ModeVisi = Visibility.Visible;

            //lvf 2018年4月12日10:07:41   
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 3) ModeVisi = Visibility.Visible;

            IsShowCtrlTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2090, 1, false);

            deleteing = true ;
            SluCtrls.Clear();
            AreaName.Clear();

            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                    AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }
            
            if (AreaName.Count > 0)
                AreaComboBoxSelected = AreaName[0];
            if (AreaName.Count > 1)
            {
                Visi = Visibility.Visible;
            }
            else
            {
                Visi = Visibility.Collapsed;
            }

            LoadModeNameFromLocal(currentModeNameFile);

            //if (string.IsNullOrEmpty(ModeName))
            //{
            //    ModeName = "新模式1";
            //}
            //LoadTimeItem(AreaId);
            //LoadTimeItems();

        }

        public void OnUserHideOrClosing()
        {
            TimeItems.Clear();
            foreach (var g in SluCtrls)
            {
                try
                {
                    g.OnSelectedSelfDefContrls -= OnUserSelectedSefDef;
                }
                catch (Exception ex)
                {

                }
            }
            SluCtrls.Clear();
        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "单灯方案设置"; }
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


    }

    /// <summary>
    /// 控制数显示
    /// </summary>
    public partial class TimeInfoSetVm
    {
        public event EventHandler OnBackNeedShowCtrlView;

        private void OnUserSelectedSefDef(object sender, EventArgs args)
        {
            var vm = sender as SluTimeCtrlSluOneVm;
            if (vm == null) return;

            if (CurrentSelectedSluCtrls == null || CurrentSelectedSluCtrls.SluId != vm.SluId)
            {
                CurrentSelectedSluCtrls = vm;
            }
            if (vm.Is485 == false)
                OnShowCtrlTree(vm.SluId, vm.AddrsCtrls);
            else OnShowCtrlGrpTree(vm.SluId, vm.AddrsCtrls);
            if (OnBackNeedShowCtrlView != null) OnBackNeedShowCtrlView(this, EventArgs.Empty);
        }


        private ObservableCollection<TreeNodeGrp> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeGrp> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeGrp>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                }
            }
        }

        protected void OnShowCtrlGrpTree(int sluId, List<int> selectGrps)
        {
            this.ChildTreeItems.Clear();
            var para =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluId) as
                Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (para == null) return;

            var nts = para .WjSluCtrlGrps .Values ;

            ////////////todo

            //if (nts.Count == 0)
            //{
            //    for (int i = 1; i < 20; i++)
            //    {
            //        var mtps = new SluGrpInfo() {GrpId = i, GrpName = "新分组" + i};
            //        for (int gg = 1; gg < 20; gg++)
            //        {
            //            mtps.RtuLst.Add(i*20 + gg);
            //        }
            //        nts.Add(mtps);
            //    }
            //}
            //////////////////////////////////////////////////

            foreach (var g in nts)
            {
                var tu = new Tuple<int, int>(AreaId , g.GrpId);
                //if (Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                //{
                    var nps = new TreeNodeGrp() { NodeId = g.GrpId, NodeName = g.GrpName, IsSelected = selectGrps.Contains(g.GrpId) };
                    this.ChildTreeItems.Add(nps);
                //}
               

            }
        }

        private bool _isShowCtrlTime;

        /// <summary>
        /// 单灯方案显示控制器方案
        /// </summary>
        public bool IsShowCtrlTime
        {
            get { return _isShowCtrlTime; }
            set
            {
                if (value != _isShowCtrlTime)
                {
                    _isShowCtrlTime = value;
                    this.RaisePropertyChanged(() => this.IsShowCtrlTime);
                }
            }
        }
        /// <summary>
        /// 显示终端数
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="selecttrls"></param>
        protected void OnShowCtrlTree(int sluId, List<int> selecttrls)
        {
            this.ChildTreeItems.Clear();

            var holdins = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId);
            if (holdins == null) return;
            var ips = holdins as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;
            if (ips == null) return;
            var dirs = new Dictionary<int,  string >();
            foreach (var g in ips.WjSluCtrls.Values )
            {
                var ntgsdf =string .IsNullOrEmpty(  g.LampCode)?"控制器"+g.CtrlPhyId :g.LampCode ;

                if (dirs.ContainsKey(g.CtrlPhyId)) dirs[g.CtrlPhyId] = ntgsdf;
                else dirs.Add(g.CtrlPhyId, ntgsdf);
            }
            var nts = ips .WjSluCtrlGrps .Values ;

            ////////////todo
            //if (dirs.Count == 0)
            //{
            //    for (int i = 1; i < 256; i++)
            //    {
            //        dirs.Add(i, "ctrls" + i);
            //    }
            //}
            //if (nts.Count == 0)
            //{
            //    for (int i = 1; i < 20; i++)
            //    {
            //        var mtps = new SluGrpInfo() {GrpId = i, GrpName = "新分组" + i};
            //        for (int gg = 1; gg < 20; gg++)
            //        {
            //            mtps.RtuLst.Add(i*20 + gg);
            //        }
            //        nts.Add(mtps);
            //    }
            //}
            //////////////////////////////////////////////////

            var lstall = (from t in dirs select t.Key).ToList();
            foreach (var g in nts)
            {
                var nps = new TreeNodeGrp() {NodeId = g.GrpId, NodeName = g.GrpName};
                foreach (var f in g.CtrlPhyLst)
                {
                    if (lstall.Contains(f)) lstall.Remove(f);
                    if (dirs.ContainsKey(f))
                    {
                        nps.ChildTreeItems.Add(new TreeNodeCtrl()
                                                   {IsSelected = selecttrls.Contains(f), NodeId = f, NodeName = dirs[f]});
                    }
                }
                if (nps.ChildTreeItems.Count > 0)
                {
                    nps.GetThisCheckByChild();
                    nps.SetCount();
                    this.ChildTreeItems.Add(nps);
                }
            }
            if (lstall.Count > 0)
            {
                var nps = new TreeNodeGrp() {NodeId = 0, NodeName = "未分组控制器"};
                foreach (var f in lstall)
                {
                    // if (lstall.Contains(f)) lstall.Remove(f);
                    if (dirs.ContainsKey(f))
                    {
                        nps.ChildTreeItems.Add(new TreeNodeCtrl()
                                                   {IsSelected = selecttrls.Contains(f), NodeId = f, NodeName = dirs[f]});
                    }
                }
                nps.GetThisCheckByChild();
                nps.SetCount();
                this.ChildTreeItems.Add(nps);
            }
        }


        /// <summary>
        /// 设置完成后 将控制器终端回归方案中
        /// </summary>
        public void OnUserSetOverSelectedSefDef()
        {
            if (CurrentSelectedSluCtrls == null) return;
            if (CurrentSelectedSluCtrls.OperatorTypeSelected == 4)
            {
                if (CurrentSelectedSluCtrls.Is485)
                {
                    var lst = new List<int>();
                    foreach (var f in this.ChildTreeItems)
                    {
                        if (f.IsSelected == false) continue;
                        if (lst.Contains(f.NodeId)) continue;
                        lst.Add(f.NodeId);
                    }
                    CurrentSelectedSluCtrls.UpdateAddrsCtrls(lst);
                }
                else
                {
                    var lst = new List<int>();
                    foreach (var g in this.ChildTreeItems)
                    {
                        foreach (var f in g.ChildTreeItems)
                        {
                            if (f.IsSelected == false) continue;
                            if (lst.Contains(f.NodeId)) continue;
                            lst.Add(f.NodeId);
                        }
                    }
                    CurrentSelectedSluCtrls.UpdateAddrsCtrls(lst);
                }
            }


        }


        /// <summary>
        /// 设置完成后 将集中器终端回归方案中
        /// </summary>
        public bool OnUserSetOverSlus()
        {
            // bool bolreturn = false;
            if (_currentselectTimeItems == null) return true;
            _currentselectTimeItems.Ctrls.Clear();

            int xCount = 0;
            foreach (var g in this.SluCtrls)
            {
                if (g.OperatorTypeSelected == 4 && g.AddrsCtrls.Count == 0)
                {
                    g.IsShowSelfSelected = false;
                }
                if (g.IsShowSelfSelected == false) g.OperatorTypeSelected = 101;

                if (g.OperatorTypeSelected == 101)
                {
                    g.IsShowSelfSelected = false;
                    continue;
                }
                
                xCount++; // = true;
                if (!_currentselectTimeItems.Ctrls.ContainsKey(g.SluId))
                    _currentselectTimeItems.Ctrls.Add(g.SluId, new SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne()
                                                                   {
                                                                       CtrlPhys  = g.AddrsCtrls,
                                                                       OperatorType = g.OperatorTypeSelected,
                                                                       SluId = g.SluId
                                                                   });
                else
                    _currentselectTimeItems.Ctrls[g.SluId] = new SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne()
                                                                 {
                                                                     CtrlPhys  = g.AddrsCtrls,
                                                                     OperatorType = g.OperatorTypeSelected,
                                                                     SluId = g.SluId
                                                                 };
            }
            _currentselectTimeItems.UsedSluCount = xCount;
            return xCount > 0;
        }
        // <summary>
        // 是否有集中器
        // </summary>
        public bool HaveSlu() 
        {
            // bool bolreturn = false;


            return this.SluCtrls.Count>0;
        }
    }


    /// <summary>
    /// Add  Delete
    /// </summary>
    public partial class TimeInfoSetVm
    {
        #region  CmdAddTimeTable

        private DateTime _dtCmdAddTimeTable;
        private ICommand _cmdAddTimeTable;

        public ICommand CmdAddTimeTable
        {
            get
            {
               return _cmdAddTimeTable ??
                       (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true));
            }
        }

        private bool CanCmdAddTimeTable()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 && DateTime.Now.Ticks - _dtCmdAddTimeTable.Ticks > 10000000;
        }

        private void ExCmdAddTimeTable()
        {
            _dtCmdAddTimeTable = DateTime.Now;

            var nts = new TimeInfoOneVm();
            this.TimeItems.Add(nts);
            CurrentSelectedTimeItem = nts;
        }

        #endregion



        #region  CmdDeleteTimeTable

        private DateTime _dtCmdDeleteTimeTable;
        private ICommand _cmdCmdDeleteTimeTable;

        public ICommand CmdDeleteTimeTable
        {
            get
            {
                return _cmdCmdDeleteTimeTable ??
                       (_cmdCmdDeleteTimeTable = new RelayCommand(ExCmdDeleteTimeTable, CanCmdDeleteTimeTable, true));
            }
        }

        private bool CanCmdDeleteTimeTable()
        {
            if (TimeItems.Count < 2) return false;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 && DateTime.Now.Ticks - _dtCmdDeleteTimeTable.Ticks > 10000000 && CurrentSelectedTimeItem != null;
        }


        private bool deleteing = false;

        private void ExCmdDeleteTimeTable()
        {
            _dtCmdDeleteTimeTable = DateTime.Now;

            if (TimeItems.Contains(CurrentSelectedTimeItem))
            {
                deleteing = true;
                TimeItems.Remove(CurrentSelectedTimeItem);
                if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
                else CurrentSelectedTimeItem = null;
                deleteing = false;
            }

        }

        #endregion



        #region  CmdSaveTimeTable

        private DateTime _dtCmdSaveTimeTable;
        private ICommand _cmdCmdSaveTimeTable;

        public ICommand CmdSaveTimeTable
        {
            get
            {
                return _cmdCmdSaveTimeTable ??
                       (_cmdCmdSaveTimeTable = new RelayCommand(ExCmdSaveTimeTable, CanCmdSaveTimeTable, true));
            }
        }

        private bool CanCmdSaveTimeTable()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 && DateTime.Now.Ticks - _dtCmdSaveTimeTable.Ticks > 50000000 && CurrentSelectedTimeItem != null;
        }

        private void Save_DDMS_to_Local(List<SluTimeScheme.SluTimeSchemeItem> ntg, string xmlFileName)
        {
            var info = new Dictionary<string, string>();

            info.Add("SluTimeSchemeItemCount", Convert.ToString(ntg.Count));

            int index = 1;

            foreach (var t  in ntg)
            {
                info.Add("SluTimeSchemeItem" + index + "AreaId", Convert.ToString(t.AreaId));
                info.Add("SluTimeSchemeItem" + index + "IsNotUsed", Convert.ToString(t.IsNotUsed));
                info.Add("SluTimeSchemeItem" + index + "IsSluOrCtrlScheme", Convert.ToString(t.IsSluOrCtrlScheme));
                info.Add("SluTimeSchemeItem" + index + "Nindex", Convert.ToString(t.Nindex));
                info.Add("SluTimeSchemeItem" + index + "SchemeDesc", Convert.ToString(t.SchemeDesc));
                info.Add("SluTimeSchemeItem" + index + "SchemeDescSec", Convert.ToString(t.SchemeDescSec));
                info.Add("SluTimeSchemeItem" + index + "SchemeId", Convert.ToString(t.SchemeId));
                info.Add("SluTimeSchemeItem" + index + "SchemeName", Convert.ToString(t.SchemeName));

                info.Add("SluTimeSchemeItem" + index + "CmdPwmScaleValue", Convert.ToString(t.SluTimePlanInfo.CmdPwmScaleValue));
                info.Add("SluTimeSchemeItem" + index + "CmdType", Convert.ToString(t.SluTimePlanInfo.CmdType));
                info.Add("SluTimeSchemeItem" + index + "LightEndEffect", Convert.ToString(t.SluTimePlanInfo.LightEndEffect));
                info.Add("SluTimeSchemeItem" + index + "LightStartEffect", Convert.ToString(t.SluTimePlanInfo.LightStartEffect));
                info.Add("SluTimeSchemeItem" + index + "LightUsedRtuId", Convert.ToString(t.SluTimePlanInfo.LightUsedRtuId));
                info.Add("SluTimeSchemeItem" + index + "OperationArgu", Convert.ToString(t.SluTimePlanInfo.OperationArgu));
                info.Add("SluTimeSchemeItem" + index + "OperationMethod", Convert.ToString(t.SluTimePlanInfo.OperationMethod));
                info.Add("SluTimeSchemeItem" + index + "OperationOrder", Convert.ToString(t.SluTimePlanInfo.OperationOrder));

                info.Add("SluTimeSchemeItem" + index + "CmdMixCount", Convert.ToString(t.SluTimePlanInfo.CmdMix.Count));

                int index1 = 1;
                foreach (var tt in t.SluTimePlanInfo.CmdMix)
                {
                    info.Add("SluTimeSchemeItem" + index + "CmdMix" + index1, Convert.ToString(tt));

                    index1++;
                }

                info.Add("SluTimeSchemeItem" + index + "CmdPwmScaleCount", Convert.ToString(t.SluTimePlanInfo.CmdPwmScale.Count));

                index1 = 1;
                foreach (var tt in t.SluTimePlanInfo.CmdPwmScale)
                {
                    info.Add("SluTimeSchemeItem" + index + "CmdPwmScale" + index1, Convert.ToString(tt));

                    index1++;
                }

                info.Add("SluTimeSchemeItem" + index + "OperationWeekSetCount", Convert.ToString(t.SluTimePlanInfo.OperationWeekSet.Count));

                index1 = 1;
                foreach (var tt in t.SluTimePlanInfo.OperationWeekSet)
                {
                    info.Add("SluTimeSchemeItem" + index + "OperationWeekSet" + index1, Convert.ToString(tt));

                    index1++;
                }

                info.Add("SluTimeSchemeItem" + index + "SluCtrlsCount", Convert.ToString(t.SluCtrls.Count));

                index1 = 1;
                foreach(var tt in t.SluCtrls)
                {
                    info.Add("SluTimeSchemeItem" + index + "SluCtrls" + index1 + "OperatorType", Convert.ToString(tt.OperatorType));
                    info.Add("SluTimeSchemeItem" + index + "SluCtrls" + index1 + "SluId", Convert.ToString(tt.SluId));

                    info.Add("SluTimeSchemeItem" + index + "SluCtrls" + index1 + "CtrlPhysCount", Convert.ToString(tt.CtrlPhys.Count));

                    int index2 = 1;

                    foreach (var ttt in tt.CtrlPhys)
                    {
                        info.Add("SluTimeSchemeItem" + index + "SluCtrls" + index1 + "CtrlPhys" + index2, Convert.ToString(ttt));

                        index2++;
                    }


                    index1++;
                }



                index++;
            }


            try
            {
                if (!xmlFileName.EndsWith(".xml"))
                {
                    xmlFileName += ".xml";
                }

                //string dir = "D:\\CETC50\\FileSync\\SingleLamp";
                //lvf  2018年5月16日16:47:07  采用根目录路径
                string dir = Directory.GetCurrentDirectory() + "\\FileSync\\SingleLamp";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                Wlst.Cr.Coreb.Servers.XmlReadSave.Save(info, path);
            }
            catch (Exception ex)
            {
            }

        }

        private void ExCmdSaveTimeTable()
        {

            var xg = _currentselectTimeItems.OnChanged();
            if (xg.Item1 == false)
            {

                WlstMessageBox.Show("方案设置有遗漏,请完善该方案", xg.Item2, WlstMessageBoxType.Ok);
                return;
            }
            if (OnUserSetOverSlus() == false)
            {
                WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "未设置任何使用该方案的集中器与控制器...", WlstMessageBoxType.Ok);
                return;
            }

            //var rtns = kgdx();
            //if(rtns .Count >0)
            //{
            //    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "光控操作仅支持开灯或关灯操作，混合操作与节能不支持...", WlstMessageBoxType.Ok);
            //    return;
                
            //}

            _dtCmdSaveTimeTable = DateTime.Now;

            var nts = new List<SluTimeScheme.SluTimeSchemeItem>();
            int i = 0;
            foreach (var t in this.TimeItems)
            {
                var xxg = t.OnChanged();
                if (xxg.Item1 == false)
                {

                    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", xg.Item2, WlstMessageBoxType.Ok);
                    this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                    return;
                }
                nts.Add(t.BackToSluTimeSchemeOne());
                nts[i].AreaId = AreaId;
                i = i + 1;

            }

            if (string.IsNullOrEmpty(ModeName))
            {
                ModeName = "新模式1";
            }

            Save_DDMS_to_Local(nts, ModeName);

            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 3)
            {
                CurrentModeName[AreaComboBoxSelected.Key] = ModeName;

                SaveModeNameToLocal(currentModeNameFile);

                Create_New_Time_TextFile();
            }


            SrInfo.TimeInfos.MySelf.UpdateTimeInfo(nts);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "提交服务器进行更新....");
        }


        List<Tuple<int,int>> kgdx()
        {
            List<Tuple<int, int>> rtn = new List<Tuple<int, int>>();
            //var rtn = new List<Tuple<int,int>>();
            foreach (var g in this.TimeItems)
            {
                if (g.OperationMethod == 11)
                {
                    if (g.CmdType != 4)
                    {
                        var tu = new Tuple<int, int>(AreaId ,g.SchemeId);
                        rtn.Add(tu);
                        continue;
                    }
                    var lst = new List<int>();
                    if (g.CmdMix1 != 0) lst.Add(g.CmdMix1);
                    if (g.CmdMix2 != 0) lst.Add(g.CmdMix2);
                    if (g.CmdMix3 != 0) lst.Add(g.CmdMix3);
                    if (g.CmdMix4 != 0) lst.Add(g.CmdMix4);

                    if(lst .Count ==0)
                    {
                        var tu = new Tuple<int, int>(AreaId, g.SchemeId);  //lvf
                        rtn.Add(tu);
                        continue;
                    }
                    int xg = lst[0];
                    bool allthesam = true;
                    foreach (var ff in lst )
                    {
                        if (ff != xg) allthesam = false;
                    }
                    if(allthesam==false  )
                    {
                        var tu = new Tuple<int, int>(AreaId, g.SchemeId);
                        rtn.Add(tu);
                        continue;
                        
                    }
                }
            }
            return rtn;
        }

        #endregion

        #region  CmdLoadTimeTable

        private DateTime _dtCmdLoadTimeTable;
        private ICommand _cmdCmdLoadTimeTable;

        public ICommand CmdLoadTimeTable
        {
            get
            {
                return _cmdCmdLoadTimeTable ??
                       (_cmdCmdLoadTimeTable = new RelayCommand(ExCmdLoadTimeTable, CanCmdLoadTimeTable, true));
            }
        }

        private bool CanCmdLoadTimeTable()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdLoadTimeTable.Ticks > 50000000;
        }

        private void ExCmdLoadTimeTable()
        {

            _dtCmdLoadTimeTable = DateTime.Now;

            //string dir = "D:\\CETC50\\FileSync\\SingleLamp";
            string dir = Directory.GetCurrentDirectory() + "\\FileSync\\SingleLamp";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string extension = "xml";
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = dir,
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                  extension,
                                  "Xml File"),
                FilterIndex = 1
            };

            var nts = new List<SluTimeScheme.SluTimeSchemeItem>();

            if (dialog.ShowDialog() == true)
            {
               nts = Load_DDMS_from_Local( dialog.SafeFileName);

                ModeName = dialog.SafeFileName.Substring(0, dialog.SafeFileName.Length - 4);

                CurrentModeName[nts[0].AreaId] = ModeName;

                SaveModeNameToLocal(currentModeNameFile);

                Create_New_Time_TextFile();

               SrInfo.TimeInfos.MySelf.UpdateTimeInfo(nts);
               Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "提交服务器进行更新....");
            }



        }

        private List<SluTimeScheme.SluTimeSchemeItem> Load_DDMS_from_Local(string xmlFileName)
        {
            var nts = new List<SluTimeScheme.SluTimeSchemeItem>();

            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            //string dir = "D:\\CETC50\\FileSync\\SingleLamp";
            string dir = Directory.GetCurrentDirectory() + "\\FileSync\\SingleLamp";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (File.Exists(xmlFileName))
                path = xmlFileName;

            info = Wlst.Cr.Coreb.Servers.XmlReadSave.Read(path);

            int SluTimeSchemeItemAmount = 0;

            if (info.ContainsKey("SluTimeSchemeItemCount"))
            {
                SluTimeSchemeItemAmount = Convert.ToInt32(info["SluTimeSchemeItemCount"]);
            }

            for (int index = 1; index <= SluTimeSchemeItemAmount; index++)
            {
                var schemeItem = new SluTimeScheme.SluTimeSchemeItem();

                if (info.ContainsKey("SluTimeSchemeItem" + index + "AreaId"))
                {
                    schemeItem.AreaId = Convert.ToInt32(info["SluTimeSchemeItem" + index + "AreaId"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "IsNotUsed"))
                {
                    schemeItem.IsNotUsed = Convert.ToBoolean(info["SluTimeSchemeItem" + index + "IsNotUsed"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "IsSluOrCtrlScheme"))
                {
                    schemeItem.IsSluOrCtrlScheme = Convert.ToInt32(info["SluTimeSchemeItem" + index + "IsSluOrCtrlScheme"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "Nindex"))
                {
                    schemeItem.Nindex = Convert.ToInt32(info["SluTimeSchemeItem" + index + "Nindex"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "SchemeDesc"))
                {
                    schemeItem.SchemeDesc = Convert.ToString(info["SluTimeSchemeItem" + index + "SchemeDesc"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "SchemeDescSec"))
                {
                    schemeItem.SchemeDescSec = Convert.ToString(info["SluTimeSchemeItem" + index + "SchemeDescSec"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "SchemeId"))
                {
                    schemeItem.SchemeId = Convert.ToInt32(info["SluTimeSchemeItem" + index + "SchemeId"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "SchemeName"))
                {
                    schemeItem.SchemeName = Convert.ToString(info["SluTimeSchemeItem" + index + "SchemeName"]);
                }

                schemeItem.SluTimePlanInfo = new SluTimeScheme.SluTimeSchemeItem.SluTimePlan();

                if (info.ContainsKey("SluTimeSchemeItem" + index + "CmdPwmScaleValue"))
                {
                    schemeItem.SluTimePlanInfo.CmdPwmScaleValue = Convert.ToInt32(info["SluTimeSchemeItem" + index + "CmdPwmScaleValue"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "CmdType"))
                {
                    schemeItem.SluTimePlanInfo.CmdType = Convert.ToInt32(info["SluTimeSchemeItem" + index + "CmdType"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "LightEndEffect"))
                {
                    schemeItem.SluTimePlanInfo.LightEndEffect = Convert.ToInt32(info["SluTimeSchemeItem" + index + "LightEndEffect"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "LightStartEffect"))
                {
                    schemeItem.SluTimePlanInfo.LightStartEffect = Convert.ToInt32(info["SluTimeSchemeItem" + index + "LightStartEffect"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "LightUsedRtuId"))
                {
                    schemeItem.SluTimePlanInfo.LightUsedRtuId = Convert.ToInt32(info["SluTimeSchemeItem" + index + "LightUsedRtuId"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "OperationArgu"))
                {
                    schemeItem.SluTimePlanInfo.OperationArgu = Convert.ToInt32(info["SluTimeSchemeItem" + index + "OperationArgu"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "OperationMethod"))
                {
                    schemeItem.SluTimePlanInfo.OperationMethod = Convert.ToInt32(info["SluTimeSchemeItem" + index + "OperationMethod"]);
                }

                if (info.ContainsKey("SluTimeSchemeItem" + index + "OperationOrder"))
                {
                    schemeItem.SluTimePlanInfo.OperationOrder = Convert.ToInt32(info["SluTimeSchemeItem" + index + "OperationOrder"]);
                }

                int cmdMixCount = 0;

                if (info.ContainsKey("SluTimeSchemeItem" + index + "CmdMixCount"))
                {
                    cmdMixCount = Convert.ToInt32(info["SluTimeSchemeItem" + index + "CmdMixCount"]);
                    schemeItem.SluTimePlanInfo.CmdMix = new List<int>();
                }

                

                for (int j = 1; j <= cmdMixCount; j++)
                {
                    if (info.ContainsKey("SluTimeSchemeItem" + index + "CmdMix" + j))
                    {
                        schemeItem.SluTimePlanInfo.CmdMix.Add(
                            Convert.ToInt32(info["SluTimeSchemeItem" + index + "CmdMix" + j]));
                    }
                }

                int cmdPwmScaleCount = 0;

                if (info.ContainsKey("SluTimeSchemeItem" + index + "CmdPwmScaleCount"))
                {
                    cmdPwmScaleCount = Convert.ToInt32(info["SluTimeSchemeItem" + index + "CmdPwmScaleCount"]);
                    schemeItem.SluTimePlanInfo.CmdPwmScale = new List<int>();
                }

                for (int j = 1; j <= cmdPwmScaleCount; j++)
                {
                    if (info.ContainsKey("SluTimeSchemeItem" + index + "CmdPwmScale" + j))
                    {
                        schemeItem.SluTimePlanInfo.CmdPwmScale.Add(
                            Convert.ToInt32(info["SluTimeSchemeItem" + index + "CmdPwmScale" + j]));
                    }
                }

                int operationWeekSetCount = 0;

                if (info.ContainsKey("SluTimeSchemeItem" + index + "OperationWeekSetCount"))
                {
                    operationWeekSetCount = Convert.ToInt32(info["SluTimeSchemeItem" + index + "OperationWeekSetCount"]);
                    schemeItem.SluTimePlanInfo.OperationWeekSet = new List<int>();
                }

                for (int j = 1; j <= operationWeekSetCount; j++)
                {
                    if (info.ContainsKey("SluTimeSchemeItem" + index + "OperationWeekSet" + j))
                    {
                        schemeItem.SluTimePlanInfo.OperationWeekSet.Add(
                            Convert.ToInt32(info["SluTimeSchemeItem" + index + "OperationWeekSet" + j]));
                    }
                }

                int sluCtrlsCount = 0;

                if (info.ContainsKey("SluTimeSchemeItem" + index + "SluCtrlsCount"))
                {
                    sluCtrlsCount = Convert.ToInt32(info["SluTimeSchemeItem" + index + "SluCtrlsCount"]);
                    schemeItem.SluCtrls = new List<SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne>();
                }

                for (int j = 1; j <= sluCtrlsCount; j++)
                {
                    int operatorType = 0;

                    if (info.ContainsKey("SluTimeSchemeItem" + index + "SluCtrls" + j + "OperatorType"))
                    {
                        operatorType =
                            Convert.ToInt32(info["SluTimeSchemeItem" + index + "SluCtrls" + j + "OperatorType"]);
                    }

                    int sluId = 0;

                    if (info.ContainsKey("SluTimeSchemeItem" + index + "SluCtrls" + j + "SluId"))
                    {
                        sluId =
                            Convert.ToInt32(info["SluTimeSchemeItem" + index + "SluCtrls" + j + "SluId"]);
                    }

                    int ctrlPhysCount = 0;

                    if (info.ContainsKey("SluTimeSchemeItem" + index + "SluCtrls" + j + "CtrlPhysCount"))
                    {
                        ctrlPhysCount =
                            Convert.ToInt32(info["SluTimeSchemeItem" + index + "SluCtrls" + j + "CtrlPhysCount"]);
                    }

                    var ctrlPhys = new List<int>();

                    for (int k = 1; k <= ctrlPhysCount; k++)
                    {
                        if (info.ContainsKey("SluTimeSchemeItem" + index + "SluCtrls" + j + "CtrlPhys" + k))
                        {
                            ctrlPhys.Add(
                                Convert.ToInt32(info["SluTimeSchemeItem" + index + "SluCtrls" + j + "CtrlPhys" + k]));
                        }
                    }

                    schemeItem.SluCtrls.Add(new SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne
                    {
                        OperatorType = operatorType,
                        SluId = sluId,
                        CtrlPhys = ctrlPhys
                    });
                }

                nts.Add(schemeItem);
            }

            return nts;
        }
        #endregion


        private string _cursdfsdf;

        public string Msg
        {
            get { return _cursdfsdf; }
            set
            {
                if (value != _cursdfsdf)
                {
                    _cursdfsdf = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }

            }
        }
    }

    /// <summary>
    /// 当前选中方案控制的控制器列表信息
    /// </summary>
    public partial class TimeInfoSetVm
    {
        private ObservableCollection<SluTimeCtrlSluOneVm> _sluCtrls;

        /// <summary>
        /// 操作参数信息
        /// </summary>
        // public SluTimePlan PlanOperator;
        /// <summary>
        /// 本次操作需要操作的集中器与控制器信息； 一次操作的所有的自定义控制器不得超过一万个；
        /// </summary>
        public ObservableCollection<SluTimeCtrlSluOneVm> SluCtrls
        {
            get
            {
                if (_sluCtrls == null) _sluCtrls = new ObservableCollection<SluTimeCtrlSluOneVm>();
                return _sluCtrls;
            }
        }

        private SluTimeCtrlSluOneVm _currentselectTSluCtrls;

        public SluTimeCtrlSluOneVm CurrentSelectedSluCtrls
        {
            get { return _currentselectTSluCtrls; }
            set
            {
                if (value != _currentselectTSluCtrls)
                {
                    if (_currentselectTSluCtrls != null)
                    {
                        _currentselectTSluCtrls.Marked = "";
                    }

                    _currentselectTSluCtrls = value;
                    if (_currentselectTSluCtrls != null)
                    {
                        _currentselectTSluCtrls.Marked = "设置中...";
                    }
                    

                    this.RaisePropertyChanged(() => this.CurrentSelectedSluCtrls);
                }

            }
        }





         private bool  _currIsSelectSluEnable;

        public bool IsSelectSluEnable
        {
            get { return _currIsSelectSluEnable; }
            set
            {
                if (value != _currIsSelectSluEnable)
                {
                    _currIsSelectSluEnable = value;
                    this.RaisePropertyChanged(() => this.IsSelectSluEnable);
                }

            }
        }


        private static ObservableCollection<AreaInt> _devices;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }

        private string _modeName;

        public string ModeName
        {
            get { return _modeName; }
            set
            {
                if (value != _modeName)
                {
                    _modeName = value;
                    this.RaisePropertyChanged(() => this.ModeName);
                }
            }

        }


        private Visibility _modeVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility ModeVisi
        {
            get { return _modeVisi; }
            set
            {
                if (value != _modeVisi)
                {
                    _modeVisi = value;
                    this.RaisePropertyChanged(() => this.ModeVisi);
                }
            }
        }


          private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private AreaInt _areacomboboxselected;
        private  int AreaId;

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;
                    if (AreaId < CurrentModeName.Count)
                    {
                        ModeName = CurrentModeName[AreaId];
                    }
                    LoadTimeItem(AreaId);

                    //LoadTimeTableInfoFromSr();
                    //this.LoadRtuOrGrpBandingInfo();
                }
            }
        }

        private ObservableCollection<TimeInfoOneVm> _sTimeItems;

        public ObservableCollection<TimeInfoOneVm> TimeItems
        {
            get
            {
                if (_sTimeItems == null) _sTimeItems = new ObservableCollection<TimeInfoOneVm>();
                return _sTimeItems;
            }
        }


        private TimeInfoOneVm _currentselectTimeItems;

        public TimeInfoOneVm CurrentSelectedTimeItem
        {
            get { return _currentselectTimeItems; }
            set
            {
                IsSelectSluEnable = value != null;

                if (value != _currentselectTimeItems)
                {

                    if (_currentselectTimeItems != null && deleteing == false)
                    {
                        ////////var xg = _currentselectTimeItems.OnChanged();
                        ////////if (xg.Item1 == false)
                        ////////{

                        ////////    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", xg.Item2, WlstMessageBoxType.Ok);
                        ////////    this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                        ////////    return;
                        ////////}
                        //if (OnUserSetOverSlus() == false)
                        //{
                        //    //_currentselectTimeItems = value;
                        //    ////WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "未设置任何使用该方案的集中器与控制器...", WlstMessageBoxType.Ok);
                        //    //this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                        //    OnSelectedTimeItemChanged();
                        //    return;
                        //}

                        _currentselectTimeItems.Marked = "";
                    }

                    _currentselectTimeItems = value;
                    if (_currentselectTimeItems != null) _currentselectTimeItems.Marked = "设置中...";
                    this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                    OnSelectedTimeItemChanged();
                }
                else
                {
                     
                    //todo
                }

            }
        }
        //private void LoadTimeItems()
        //{
        //    var nts =
        //           (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
        //            where t.Value.EquipmentType == WjParaBase.EquType.Slu && t.Value.AreaId==AreaId 
        //            orderby t.Key ascending
        //            select t.Key).ToList();
        //    foreach (var g in nts)
        //    {
                
        //        var gt = new SluTimeCtrlSluOneVm(g);
        //        gt.OnSelectedSelfDefContrls += OnUserSelectedSefDef;
                
        //        SluCtrls.Add(gt);
        //    }

        //    TimeItems.Clear();

        //    var bts =
        //        (from t in TimeInfos.MySelf.Info where t.Key.Item1==AreaId orderby t.Key ascending select t.Value).ToList();
        //    foreach (var g in bts)
        //    {
        //        TimeItems.Add(new TimeInfoOneVm(AreaId,g));
        //    }
        //    if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
        //    else CurrentSelectedTimeItem = null;
        //    deleteing = false;
        //}

        private void LoadTimeItem(int areaId) //lvff
        {
            SluCtrls.Clear();

            var lst = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(areaId);
            var nts =
                   (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                    where t.Value.EquipmentType == WjParaBase.EquType.Slu && lst .Contains( t.Value .RtuId )  
                    orderby t.Key ascending
                    select t.Key).ToList();
            foreach (var g in nts)
            {
                var gt = new SluTimeCtrlSluOneVm(g);
                gt.OnSelectedSelfDefContrls += OnUserSelectedSefDef;
                SluCtrls.Add(gt);
            }

            TimeItems.Clear();

            var bts =
                (from t in TimeInfos.MySelf.Info where t.Key.Item1==areaId  orderby t.Key ascending select t.Value).ToList();
            foreach (var g in bts)
            {
                TimeItems.Add(new TimeInfoOneVm(areaId,g));
            }
            if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
            else CurrentSelectedTimeItem = null;
            deleteing = false;
        }

        private void OnSelectedTimeItemChanged()
        {
             
            if (_currentselectTimeItems == null )
            {
                foreach (var g in this.SluCtrls)
                {
                    g.UpdateInfoBySluOne(0);
                }
            }
            else
            {
                foreach (var g in this.SluCtrls)
                {
                    if (_currentselectTimeItems.Ctrls.ContainsKey(g.SluId))
                    {
                        g.UpdateInfoBySluOne(_currentselectTimeItems.Ctrls[g.SluId]);
                    }
                    else
                    {
                        g.UpdateInfoBySluOne(0);
                    }
                }
            }
        }

    }
}
