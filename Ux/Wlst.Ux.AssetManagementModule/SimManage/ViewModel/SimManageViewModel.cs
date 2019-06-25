using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.AssetManageInfoHold.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.AssetManagementModule.LampManage.ViewModel;
using Wlst.Ux.AssetManagementModule.SimManage.Services;
using Wlst.client;
using System.Threading.Tasks;  
using System.Reflection;  
using Microsoft.Office.Interop.Excel;  


namespace Wlst.Ux.AssetManagementModule.SimManage.ViewModel
{
    [Export(typeof(IISimManage))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SimManageViewModel : VmEventActionProperyChangedBase, IISimManage
    {
        public SimManageViewModel ()
        {
            Title = "Sim卡资产管理";

            StateLst.Clear();

            StateLst.Add(new NameValueInt() { Name = "正常", Value = 1 });
            StateLst.Add(new NameValueInt() { Name = "遗失", Value = 2 });
            StateLst.Add(new NameValueInt() { Name = "过期", Value = 3 });
            StateLst.Add(new NameValueInt() { Name = "停用", Value = 4 });
            StateLst.Add(new NameValueInt() { Name = "限制", Value = 5 });
            StateLst.Add(new NameValueInt() { Name = "其他", Value = 6 });

            InitEvent();
            InitAction();
        }

        private ObservableCollection<NameValueInt> StateLst =  new ObservableCollection<NameValueInt>( ); 

        #region IEventAggregator Subscription

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {

                    int id = Convert.ToInt32(args.GetParams()[0]);
                    var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                    if (tmps == null) return;

                    var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[id] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                    string IpAddr = string.Empty;

                    if (tt != null)
                        IpAddr = new System.Net.IPAddress(BitConverter.GetBytes(tt.WjGprs.StaticIp)).ToString();


                    new Tuple<int, string,string>(id, tmps.RtuName, IpAddr);

                }
            }
            catch (Exception)
            {

                throw;
            }

        }



        public void InitEvent()
        {

            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_zc_sim,
               UpdateSimAck,
               typeof(SimManageViewModel), this);
        }

        public void UpdateSimAck(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.WstSpeZcSim.Op == 5)
            {
                Msg = DateTime.Now + " 更新成功！";

                Data.Clear();

                int index = 1;
                foreach (var t in infos.WstSpeZcSim.Items)
                {
                    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                    //var d = ConvertLongDateTime(t.Value.DtKt);
                    //var x = d;
                    Data.Add(new SimItemModel()
                    {
                        ActivateTime = new DateTime(t.DtKt),
                        ChargeTime = new DateTime(t.DtXf),
                        Id = index++,
                        IP = t.IpAddr.Trim(),
                        NodeId = t.RtuId,
                        NodeName = tmp != null ? tmp.RtuName : "",
                        State = StateLst,
                        TelNum = t.SimNum.ToString("D1"),
                        SelectedState = StateLst[t.State - 1]
                    });
                }
            }
            else if (infos.WstSpeZcSim.Op == 1)
            {
                if (infos.WstSpeZcSim.Items != null)
                {
                    Data.Clear();

                    int index = 1;
                    foreach (var t in infos.WstSpeZcSim.Items)
                    {
                        var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                        //var d = ConvertLongDateTime(t.Value.DtKt);
                        //var x = d;
                        Data.Add(new SimItemModel()
                        {
                            ActivateTime = new DateTime(t.DtKt),
                            ChargeTime = new DateTime(t.DtXf),
                            Id = index++,
                            IP = t.IpAddr.Trim(),
                            NodeId = t.RtuId,
                            NodeName = tmp != null ? tmp.RtuName : "",
                            State = StateLst,
                            TelNum = t.SimNum.ToString("D1"),
                            SelectedState = StateLst[t.State - 1]
                        });
                    }

                    int maxidx = 1;
                    if (Data != null)
                    {
                        foreach (var f in this.Data)
                        {
                            if (f.Id >= maxidx) maxidx = f.Id + 1;
                        }
                    }
                }
            }
        }


        #endregion

        #region Methods

        public override void NavOnLoadr(params object[] parsObjects)
        {
            Data.Clear();



            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_zc_sim;

            info.WstSpeZcSim.Op = 1;

            SndOrderServer.OrderSnd(info, 10, 6);


            //var lst = Wlst.Sr.AssetManageInfoHold.Services.SimInfoHold.GetData();


        }

      

        private bool IsDataValidate(ObservableCollection<SimItemModel > allData)
        {
            foreach (var t in allData)
            {
                if (string.IsNullOrEmpty(t.TelNum) == false)
                {
                    if (t.TelNum.Length != 11)
                    {
                        UMessageBox.Show("设置错误", "序号为" + t.Id + "的卡号长度不对，请重新设置", UMessageBoxButton.Yes);
                        return false;
                    }
                    if (t.ActivateTime > t.ChargeTime)
                    {

                        UMessageBox.Show("设置错误", "序号为" + t.Id + "的续费时间大于开通时间，请重新设置", UMessageBoxButton.Yes);
                        return false;

                    }
                    IPAddress ip;
                    if (!IPAddress.TryParse(t.IP, out ip))
                    {
                        UMessageBox.Show("设置错误", "序号为" + t.Id + "的Ip地址不合法，请重新设置", UMessageBoxButton.Yes);
                        return false;
                    }

                }
            }
            return true;
        }

        
        #endregion

        #region attribute



        private ObservableCollection<SimItemModel > _data;
        /// <summary>
        /// sim卡管理数据
        /// </summary>
        public ObservableCollection<SimItemModel> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new ObservableCollection<SimItemModel>();
                }
                return _data;
            }
            set
            {
                if (value == _data) return;
                _data = value;
                this.RaisePropertyChanged(() => Data);
            }
        }

        private SimItemModel _selectedData;
        /// <summary>
        /// 当前选中数据
        /// </summary>
        public SimItemModel SelectedData
        {
            get { return _selectedData; }
            set
            {
                if (value != _selectedData)
                {
                    _selectedData = value;
                    this.RaisePropertyChanged(() => this.SelectedData);
                }
            }
        }

        private string _msg;
        /// <summary>
        /// 当前信息
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }
            }
        }

        #endregion

        #region Command

        #region CmdAddAll
        private ICommand _cmdAddAll;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public ICommand CmdAddAll
        {
            get
            {
                if (_cmdAddAll == null)
                {
                    _cmdAddAll = new RelayCommand(ExAddAll, CanAddAll, false);
                }
                return _cmdAddAll;
            }
        }

        bool CanAddAll()
        {
            return true;
        }

        void ExAddAll()
        {
            Data.Clear();

            var lst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems;

            int index = 1;

            if (lst != null)
            {
                foreach (var t in lst)
                {
                    if ((t.Value.RtuId >= EquipmentIdAssignRang.RtuStart) && (t.Value.RtuId <= EquipmentIdAssignRang.RtuEnd))
                    {
                        var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Value.RtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        if (tt != null)
                            Data.Add(new SimItemModel()
                                         {
                                             ActivateTime  = DateTime.Now,
                                             ChargeTime = DateTime.Now,
                                             Id = index++,
                                             IP = new System.Net.IPAddress(BitConverter.GetBytes(tt.WjGprs.StaticIp)).ToString(),
                                             NodeId = t.Value.RtuId,
                                             NodeName = t.Value.RtuName,
                                             State = StateLst,
                                             TelNum = string.Empty,
                                             SelectedState = StateLst[0]
                                         });
                    }
                }
            }

            var xlst = Wlst.Sr.AssetManageInfoHold.Services.SimInfoHold.GetData();

            if (lst != null)
            {
                foreach (var t in xlst)
                {

                    for (int i = 0; i < Data.Count; i++)
                    {
                        if (t.Value.RtuId == Data[i].NodeId)
                        {
                            Data[i].ActivateTime = new DateTime(t.Value.DtKt);
                            Data[i].ChargeTime = new DateTime(t.Value.DtXf);
                            Data[i].SelectedState = StateLst[t.Value.State - 1];
                            Data[i].TelNum = Convert.ToString(t.Value.SimNum);
                            break;
                        }
                    }
                }
            }



            //int maxidx = 1;
            //if (Data != null)
            //{
            //    foreach (var f in this.Data)
            //    {
            //        if (f.Id >= maxidx) maxidx = f.Id + 1;
            //    }
            //    AddMaxId = maxidx;
            //}
            //else
            //{
            //    AddMaxId = 1;
            //}

            //int id = 0;
            //string name = "";
            //string ip = string.Empty;
            //if(AddRtu !=null)
            //{
            //    id = AddRtu.Item1;
            //    name = AddRtu.Item2;
            //    ip = AddRtu.Item3;
            //}
            //Data.Add(new SimItemModel() { Id = AddMaxId, NodeId = id, NodeName = name,  IP = ip, State = StateLst, SelectedState = StateLst[0],ActivateTime = DateTime.Now,ChargeTime = DateTime.Now  });
            //AddMaxId += 1;
        }
        #endregion

        #region CmdDel
        private ICommand _cmdDel;
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand CmdDel
        {
            get
            {
                if (_cmdDel == null)
                {
                    _cmdDel = new RelayCommand(DelEx, CanDel, false);
                }
                return _cmdDel;
            }
        }

        bool CanDel()
        {
            return true;
        }

        void DelEx()
        {
            Data.Remove(SelectedData);

            int index = 1;
            foreach (var t in Data )
            {
                t.Id = index++;
            }
        }
        #endregion


        #endregion

        #region 保存 导出

        /// <summary>
        /// 获取整棵树的分组信息
        /// </summary>
        /// <returns></returns>
        private List<SimInfo> GetSimLst()
        {
            var lis = new List<SimInfo>();

            int index = 1;
            foreach (var t in this.Data)
            {
                if (string.IsNullOrEmpty(t.TelNum) == false)
                {
                    var tmp =
                        new SimInfo(new ZcSim.ZcSimItem()
                                        {
                                            Id = t.Id,
                                            IpAddr = t.IP,
                                            DtKt = t.ActivateTime.Ticks,
                                            DtXf = t.ChargeTime.Ticks,
                                            RtuId = t.NodeId,
                                            SimNum = long.Parse(t.TelNum),
                                            State = t.SelectedState.Value
                                        });
                    lis.Add(tmp);
                }
            }
            return lis;
        }

        #region CmdSave

        private DateTime _dtSave;
        private ICommand _cmdSave;

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new RelayCommand(SaveEx, CanSave, false);
                }
                return _cmdSave;
            }
        }

        bool CanSave()
        {
            return true;
        }

        void SaveEx()
        {
            _dtSave = DateTime.Now;
            if (!IsDataValidate(Data)) return;
            Wlst.Sr.AssetManageInfoHold.Services.SimInfoHold.UpdateSimInfo( GetSimLst());
            Msg = DateTime.Now + " 已经提交更新信息到服务器，请等待...";
        }
        #endregion

        #region CmdInput

        private ICommand _CmdInput;

        public ICommand CmdInput
        {
            get
            {
                if (_CmdInput == null) _CmdInput = new RelayCommand(ExCmdInput, CanCmdInput, false);
                return _CmdInput;
            }
        }

        private NameValueInt Get_SelectedState(string _name)
        {
            foreach(var t in StateLst)
            {
                if(t.Name == _name)
                {
                    return t;
                }
            }

            return StateLst[0];
        }

        public static Array ReadXls(string filename, int index)//读取第index个sheet的数据
        {
            //启动Excel应用程序
            var xls = new Microsoft.Office.Interop.Excel.Application();
            //打开filename表
            _Workbook book = xls.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            _Worksheet sheet;//定义sheet变量
            xls.Visible = false;//设置Excel后台运行
            xls.DisplayAlerts = false;//设置不显示确认修改提示

            try
            {
                sheet = (_Worksheet)book.Worksheets.get_Item(index);//获得第index个sheet，准备读取
            }
            catch (Exception ex)//不存在就退出
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            Console.WriteLine(sheet.Name);
            int row = sheet.UsedRange.Rows.Count;//获取不为空的行数
            int col = sheet.UsedRange.Columns.Count;//获取不为空的列数

            // Array value = (Array)sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[row, col]).Cells.Value2;//获得区域数据赋值给Array数组，方便读取

            Microsoft.Office.Interop.Excel.Range range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[row, col]];
            var value = (Array)range.Value2;

            book.Save();//保存
            book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
            xls.Quit();//Excel程序退出
            //sheet,book,xls设置为null，防止内存泄露
            sheet = null;
            book = null;
            xls = null;
            GC.Collect();//系统回收资源
            return value;
        }


        private void ExCmdInput()
        {
            try
            {
                string extension = "xls";
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    DefaultExt = extension,
                    Filter =
                        String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                      extension,
                                      "Excel"),
                    FilterIndex = 1
                };
                if (dialog.ShowDialog() == true)
                {

                    bool res = true;

                    string[] columns = new string[] { "序号", "卡号", "设备地址", "设备名称", "IP地址", "开通时间", "续费时间", "状态" };

                    Array simData = ReadXls(dialog.FileName, 1);

                    int index = 0;
                    foreach (string temp in simData)
                    {
                        if (index < columns.Length)
                        {
                            if (temp != columns[index++])
                            {
                                res = false;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }


                   

                    if (res == true)
                    { 
                        var inputData = new ObservableCollection<SimItemModel>();

                        int id = 0;
                        string telNum = string.Empty;
                        int nodeID = 0;
                        string nodeName = string.Empty;
                        string ip = string.Empty;
                        var activateTime = new DateTime();
                        var chargeTime = new DateTime();
                        var state = StateLst;
                        var selectedState = new NameValueInt();

                        int i = 0;

                        foreach (string temp in simData)
                        {
                            if(i >= columns.Length)
                            {
                                if(i % columns.Length == 0)
                                {
                                    id = Convert.ToInt32(temp);
                                }
                                else if (i % columns.Length == 1)
                                {
                                    telNum = Convert.ToString(temp);
                                }
                                else if (i % columns.Length == 2)
                                {
                                    nodeID = Convert.ToInt32(temp);
                                }
                                else if (i % columns.Length == 3)
                                {
                                    nodeName = Convert.ToString(temp);
                                }
                                else if (i % columns.Length == 4)
                                {
                                    ip = Convert.ToString(temp);
                                }
                                else if (i % columns.Length == 5)
                                {
                                    activateTime = Convert.ToDateTime(temp);
                                }
                                else if (i % columns.Length == 6)
                                {
                                    chargeTime = Convert.ToDateTime(temp);
                                }
                                else if (i % columns.Length == 7)
                                {
                                    selectedState = Get_SelectedState(Convert.ToString(temp));

                                    inputData.Add(
                                        new SimItemModel
                                            {
                                                Id = id,
                                                TelNum = telNum,
                                                NodeId = nodeID,
                                                NodeName = nodeName,
                                                IP = ip,
                                                ActivateTime = activateTime,
                                                ChargeTime = chargeTime,
                                                State = StateLst,
                                                SelectedState = selectedState
                                            }

                                        );
                                }
                            }

                            i++;

                        }

                        Data.Clear();


                        Data = inputData;

                        int maxidx = 1;
                        if (Data != null)
                        {
                            foreach (var f in this.Data)
                            {
                                if (f.Id >= maxidx) maxidx = f.Id + 1;
                            }
                        }

                        Msg = DateTime.Now + " 导入成功！";
                    }
                    else
                    {
                        Msg = DateTime.Now + " 导入失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导入报表出错，异常为:" + ex);

                Msg = DateTime.Now + " 导入失败！";

            }
        }

        private bool CanCmdInput()
        {
            return true;
        }
        #endregion

        #region CmdReport

        private DateTime _dtReport;
        private ICommand _CmdReport;

        public ICommand CmdReport
        {
            get
            {
                if (_CmdReport == null) _CmdReport = new RelayCommand(ExCmdReport, CanCmdReport, false);
                return _CmdReport;
            }
        }


        private void ExCmdReport()
        {
            _lastExCmdReport = DateTime.Now.Ticks;
            WriteData();
        }

        private long _lastExCmdReport = DateTime.Now.AddDays(-1).Ticks;

        private bool CanCmdReport()
        {
            if (DateTime.Now.Ticks - _dtReport.Ticks < 60000000) return false;
            return true;
        }

        #endregion

        private void WriteData()
        {
            try
            {
                var writeinfo = new List<List<object>>();
                var titleinfo = new List<object>();
                titleinfo.Add("序号");
                titleinfo.Add("卡号");
                titleinfo.Add("设备地址");
                titleinfo.Add("设备名称");
                titleinfo.Add("IP地址");
                titleinfo.Add("开通时间");
                titleinfo.Add("续费时间");
                titleinfo.Add("状态");


                var tmllst = (from t in Data orderby t.Id select t).ToList();
                foreach (var f in tmllst)
                {
                    var tmp = new List<object>();
                    tmp.Add(f.Id);
                    tmp.Add(f.TelNum);
                    tmp.Add(f.NodeId);
                    tmp.Add(f.NodeName);
                    tmp.Add(f.IP);  
                    tmp.Add(f.ActivateTime);
                    tmp.Add(f.ChargeTime);
                    tmp.Add(f.SelectedState.Name);
                    writeinfo.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

                Msg = DateTime.Now + " 导出成功！";
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出资产信息报表时报错:" + ex);

                Msg = DateTime.Now + " 导出失败！";
            }
        }

        #endregion
    }
}
