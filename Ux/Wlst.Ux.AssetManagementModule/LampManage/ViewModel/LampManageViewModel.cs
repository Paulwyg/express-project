using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.AssetManageInfoHold.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.AssetManagementModule.LampManage.Services;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Ux.AssetManagementModule.LampManage.ViewModel
{
    [Export(typeof(IILampManage))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class LampManageViewModel: VmEventActionProperyChangedBase, IILampManage
    {
        public LampManageViewModel()
        {
            Title = "终端资产管理";
            InitEvent();
            InitAction();

        }

        private DateTime dtSnd;
        private Tuple<int, string> AddRtu=null;
        private ObservableCollection<NameValueInt> StateLst = new ObservableCollection<NameValueInt>( );
        private static ObservableCollection<NameValueInt> BureauLst = new ObservableCollection<NameValueInt>(); 

        #region IEventAggregator Subscription

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            try
            {

                //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                //{

                //    int id = Convert.ToInt32(args.GetParams()[0]);
                //    var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);

                //    if (tmps == null) return;
                //    AddRtu = new Tuple<int, string>(tmps.RtuPhyId, tmps.RtuName);
                //}

                if (args.EventId == Wlst.Sr.AssetManageInfoHold.Services.EventIdAssign.LampNeedUpdate)
                {
                    Msg = Convert.ToString(  args.EventAttachInfo);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }



        public void InitEvent()
        {

            this.AddEventFilterInfo(Wlst.Sr.AssetManageInfoHold.Services.EventIdAssign.LampNeedUpdate,
                                    PublishEventType.Core);
        }

        public void InitAction()
        {         
        }

       

        #endregion

        public override void NavOnLoadr(params object[] parsObjects)
        {
            Data.Clear();
            StateLst.Clear();
            BureauLst.Clear();
            Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.RequestLampInfo();
            StateLst.Add(new NameValueInt() { Name = "已移交", Value = 1 });
            StateLst.Add(new NameValueInt() { Name = "未移交", Value = 2 });


            //var lst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems;

            var xlst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            if (xlst != null)
            {
                var lst = (from t in xlst orderby t.Value.RtuId select t).ToList();

                int i = 0;
                int maxidx = 1;

                foreach (var t in lst)
                {
                    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.Value.RtuId);

                    if (tmp != null)
                    {
                        AddToBureauLstComboBox(t.Value.Cqj);

                        Data.Add(new LampItemModel()
                                     {
                                         //Index = t.Value.Id,
                                         Index = maxidx++,
                                         AmmeterNum = t.Value.Dbbh,
                                         BureauList = BureauLst,
                                         SelectedBureau = BureauLst[GetBureauLstComboBoxIndex(t.Value.Cqj)],
                                         //Bureau = t.Value.Cqj,
                                         NodeId = tmp.RtuPhyId,
                                         LogicalId = t.Value.RtuId,
                                         PowerNum = t.Value.Dygh,
                                         NodeName = tmp.RtuName,
                                         TransferState = StateLst,
                                         SelectedState = StateLst[t.Value.IsYj - 1]
                                     });
                    }
                }
            }
        }

        public override void OnUserHideOrClosingr()
        {
            Data.Clear();
        }

        public static void AddToBureauLstComboBox(string cqj)
        {
            bool flag = false;

            for (int j = 0; j < BureauLst.Count; j++)
            {
                if (BureauLst[j].Name == cqj)
                {
                    flag = true;
                    break;
                }
            }

            if (flag == false)
            {
                BureauLst.Add(new NameValueInt() { Name = cqj, Value = BureauLst.Count + 1 });
            }
        }

        private int GetBureauLstComboBoxIndex(string cqj)
        {
            for (int j = 0; j < BureauLst.Count; j++)
            {
                if (BureauLst[j].Name == cqj)
                {
                    return j;
                }
            }

            return -1;
        }

        #region attribute PropertyManage

       
        private ObservableCollection<LampItemModel> _data;
        /// <summary>
        /// 资产管理数据
        /// </summary>
        public ObservableCollection<LampItemModel> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new ObservableCollection<LampItemModel>();
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

        private LampItemModel _selectedData;
        /// <summary>
        /// 当前选中数据
        /// </summary>
        public LampItemModel SelectedData
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

        #region CmdAdd
        private ICommand _cmdAdd;
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public ICommand CmdAdd
        {
            get
            {
                if (_cmdAdd == null)
                {
                    _cmdAdd = new RelayCommand(AddEx, CanAdd, false);
                }
                return _cmdAdd;
            }
        }

        bool CanAdd()
        {           
            return true;
        }

        void AddEx()
        {
            //int id = 0;
            //string name = "";
            //if (AddRtu != null)
            //{
            //    id = AddRtu.Item1;
            //    name = AddRtu.Item2;
            //}
            //Data.Add(new LampItemModel() {Index = AddMaxId,NodeId=id,NodeName = name,TransferState = StateLst,SelectedState = StateLst[0]  });
            //AddMaxId += 1;
            Data.Clear();

            var lst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems;

            int index = 1;

            if (lst != null)
            {
                foreach (var t in lst)
                {
                    if((t.Value.RtuId >=EquipmentIdAssignRang.RtuStart) && (t.Value.RtuId <= EquipmentIdAssignRang.RtuEnd))
                    {
                        Data.Add(new LampItemModel()
                                     {
                                         Index = index++,
                                         AmmeterNum = string.Empty,
                                         BureauList = BureauLst,
                                         SelectedBureau = new NameValueInt(),
                                         NodeId = t.Value.RtuPhyId,
                                         LogicalId = t.Value.RtuId,
                                         PowerNum = string.Empty,
                                         NodeName = t.Value.RtuName,
                                         TransferState = StateLst,
                                         SelectedState = StateLst[1]
                                     });
                    }
                }
            }

            var xlst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            if (lst != null)
            {
                foreach (var t in xlst)
                {

                    for(int i = 0 ; i < Data.Count ; i++)
                    {
                        if(t.Value.RtuId == Data[i].LogicalId)
                        {
                            Data[i].AmmeterNum = t.Value.Dbbh;
                            Data[i].SelectedBureau = BureauLst[GetBureauLstComboBoxIndex(t.Value.Cqj)];
                            //Data[i].Bureau = t.Value.Cqj;
                            Data[i].PowerNum = t.Value.Dygh;
                            Data[i].SelectedState = StateLst[t.Value.IsYj - 1];
                            break;
                        }
                    }
                }
            }

            Modify_Mru();
        }

        void Modify_Mru()
        {
            var lst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems;

            if (lst != null)
            {
                foreach (var t in lst)
                {
                    if ((t.Value.RtuId >= EquipmentIdAssignRang.MruStart) && (t.Value.RtuId <= EquipmentIdAssignRang.MruEnd))
                    {
                        if ((t.Value.RtuFid >= EquipmentIdAssignRang.RtuStart) && (t.Value.RtuFid <= EquipmentIdAssignRang.RtuEnd))
                        {
                            for (int i = 0; i < Data.Count; i++)
                            {
                                if (t.Value.RtuFid == Data[i].LogicalId)
                                {
                                    Data[i].AmmeterNum = Convert.ToString(t.Value.RtuId);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
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
            //Data.Remove(SelectedData);
            bool flag = false;

            do
            {
                flag = false;
                for (int i = 0; i < Data.Count; i++)
                {
                    if (Data[i].SelectedBureau == null)
                    {
                        Data[i].SelectedBureau = new NameValueInt();
                    }

                    if ((Data[i].AmmeterNum == string.Empty) && string.IsNullOrEmpty(Data[i].SelectedBureau.Name) &&
                        (Data[i].PowerNum == string.Empty))
                    {
                        flag = true;
                        Data.RemoveAt(i);
                        break;

                    }
                }
            } while (flag == true);

            for (int i = 0; i < Data.Count; i++)
            {
                Data[i].Index = i + 1;
            }

        }
        #endregion
      
        #endregion

        #region 保存 导出

        /// <summary>
        /// 获取整棵树的分组信息
        /// </summary>
        /// <returns></returns>
        private List<LampInfo> GetLampLst()
        {
            var lis = new List<LampInfo>();

            int index = 1;
            foreach (var t in this.Data)
            {
                var tmp =
                    new LampInfo(new ZcDyxx.ZcDyxxItem
                                     {
                                         Id = t.Index,
                                         //Cqj = t.Bureau,
                                         Cqj = t.SelectedBureau.Name,
                                         Dbbh = t.AmmeterNum,
                                         Dygh = t.PowerNum,
                                         RtuId = t.LogicalId,
                                         IsYj = t.SelectedState.Value
                                     });
                lis.Add(tmp);
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
            DelEx();

            _dtSave = DateTime.Now;
            Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.UpdateData(GetLampLst() );
            Msg = DateTime.Now + " 已经提交更新信息到服务器，请等待...";
            dtSnd = DateTime.Now;
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
                titleinfo.Add("设备地址");
                titleinfo.Add("设备名称");
                titleinfo.Add("城区局");
                titleinfo.Add("电源杆号");
                titleinfo.Add("电表表号");
                titleinfo.Add("移交状态");
                 

                var tmllst = (from t in Data orderby t.Index  select t).ToList();
                foreach (var f in tmllst)
                {
                    var tmp = new List<object>();
                    tmp.Add(f.Index);
                    tmp.Add(f.NodeId );
                    tmp.Add(f.NodeName);
                    //tmp.Add(f.Bureau);
                    tmp.Add(f.SelectedBureau.Name);
                    tmp.Add(f.PowerNum );
                    tmp.Add(f.AmmeterNum );
                    tmp.Add(StateLst[f.SelectedState.Value - 1].Name);
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
