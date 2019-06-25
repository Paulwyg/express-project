using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
 
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.EquipemntLightFault.ServicesHold;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntLightFault.SendOrderViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.EquipemntLightFault.SendOrderViewModel.ViewModel
{
    [Export(typeof(IISendOrderView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SendOrderViewModel : EventHandlerHelperExtendNotifyProperyChanged, IISendOrderView
    {
        public SendOrderViewModel()
        {

        }

        public ulong CurrentOrderID = 0;

        public static ObservableCollection<InputFaultRecord> FaultRecord = new ObservableCollection<InputFaultRecord>();

        private string Return_PriorityLevel_Desc(int priorityLevel)
        {
            if (priorityLevel == 3)
            {
                return "B";
            }

            return "A";
        }

        private bool Return_PriorityLevel_Enable(int priorityLevel)
        {
            if (priorityLevel == 3)
            {
                return true;
            }

            return false;
        }

        private static string ReturnStatusDesc(ulong orderID)
        {
            if (orderID == 0)
            {
                return "未派单";
            }

            return "已派单";
        }

        private string Get_Four_Number(int x)
        {
            string _num = Convert.ToString(x);

            int _len = _num.Length;

            if (_num.Length <= 4)
            {
                for (int i = 0; i < 4 - _len; i++)
                {
                    _num = "0" + _num;
                }
            }
            else
            {
                _num = "0001";
            }

            return _num;
        }

        private string GetOrderId()
        {
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + "OrderID.txt";

            try
            {

                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path, Encoding.Default);

                    String line = sr.ReadLine();

                    sr.Close();


                    string[] value = line.Split(',');

                    if (value.Length == 2)
                    {
                        if (value[0] == DateTime.Now.ToString("yyyyMMdd"))
                        {
                            if (value[1].Length == 4)
                            {
                                int x = Convert.ToInt32(value[1]);

                                //x++;

                                //if (x > 9999)
                                //{
                                //    x = 1;
                                //}

                                string _num = Get_Four_Number(x);


                                //File.Delete(path);
                                //Wlst.Ux.EquipemntLightFault.Services.fileread.Write(path, DateTime.Now.ToString("yyyyMMdd") + "," + _num);

                                return DateTime.Now.ToString("yyyyMMdd") + _num;
                            }
                        }
                    }
                }

                if (File.Exists(path)) File.Delete(path);

                Wlst.Ux.EquipemntLightFault.Services.fileread.Write(path, DateTime.Now.ToString("yyyyMMdd") + ",0001");

                return DateTime.Now.ToString("yyyyMMdd") + "0001";
            }
            catch (Exception)
            {

                if (File.Exists(path)) File.Delete(path);

                Wlst.Ux.EquipemntLightFault.Services.fileread.Write(path, DateTime.Now.ToString("yyyyMMdd") + ",0001");

                return DateTime.Now.ToString("yyyyMMdd") + "0001";
            }


        }

        private void WriteNewOrderID(string orderID)
        {
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + "OrderID.txt";

            if (File.Exists(path)) File.Delete(path);

            Wlst.Ux.EquipemntLightFault.Services.fileread.Write(path, orderID.Insert(8,","));
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            SendOrderList.Clear();

            int m = 0;

            CurrentOrderID = Convert.ToUInt64(GetOrderId());

            foreach (var ttt in FaultRecord)
            {
                var tmp = ServicesGrpSingleInfoHold.GetRtuBelongGrp(ttt.RtuId);

                var grpName = "特殊分组";

                if (tmp != null)
                {
                    grpName =
                        (ServicesGrpSingleInfoHold.InfoGroups[new Tuple<int, int>(tmp.Item1, tmp.Item2)]).GroupName;
                }


                var str  = ttt.RtuName + " " + ttt.FaultName;
                if (ttt.LoopID > 0)
                {
                    str = ttt.RtuName + "  回路" + ttt.LoopID + "   " + ttt.FaultName ;
                    var hld = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(ttt.RtuId);
                    if (hld != null)
                    {
                        var tmpsdfsd = hld as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                        if (tmpsdfsd != null)
                        {
                            if (tmpsdfsd.WjLoops.ContainsKey(ttt.LoopID))
                            {
                                str = ttt.RtuName + " " + tmpsdfsd.WjLoops[ttt.LoopID].LoopName + "   " + ttt.FaultName + "\r\n" + ttt.Remarks;
                            }
                        }

                    }

                }

                var orderInfo = new SendOrderItems
                                    {
                                        Id = m++,
                                        RtuId = ttt.RtuId,
                                        AdminName = UserInfo.UserLoginInfo.UserName,
                                        MergencyLocation = string.Empty,
                                        MergencyLocationEnable = Return_PriorityLevel_Enable(ttt.PriorityLevel),
                                        OrderId = ttt.OrderID,
                                        FaultId = ttt.FaultID,
                                        FaultName = ttt.FaultName,
                                        OrderTime = string.Empty,
                                        PriorityLevel = Return_PriorityLevel_Desc(ttt.PriorityLevel),
                                        RepairContent = str,
                                        RtuGroup = grpName,
                                        RtuName = ttt.RtuName,
                                        LoopId = ttt.LoopID,
                                        Status = ReturnStatusDesc(ttt.OrderID)
                                    };

                SendOrderList.Add(orderInfo);

            }

            if (SendOrderList.Count != 0)
            {
                CurrentSendOrderItem = SendOrderList[0];
                MessageShow = string.Empty;
            }


        }

        public void OnUserHideOrClosing()
        {

        }

        private string SendDatatoWebServices()
        {

            var data = new WsErrorModel()
                           {
                               ErrorId = Convert.ToString(CurrentOrderID),
                               PaperId = CurrentSendOrderItem.PriorityLevel,
                               Facility = CurrentSendOrderItem.RtuGroup,
                               Content = CurrentSendOrderItem.RepairContent,
                               Manager = CurrentSendOrderItem.AdminName,
                               Time = CurrentSendOrderItem.OrderTime,
                               Location = CurrentSendOrderItem.MergencyLocation,
                               Tml = CurrentSendOrderItem.RtuName
                           };
            return Wlst.Sr.EquipemntLightFault.ServicesHold.WebServiceHelper.Snddata(data);
        }

        public int SendOrderIndex = 0;
        public void SendOrder()
        {
            SendOrderIndex = CurrentSendOrderItem.Id;
            CurrentSendOrderItem.OrderTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            CurrentSendOrderItem.OrderId = CurrentOrderID;

            string sendDatatoWebServices = SendDatatoWebServices();
;

            if (sendDatatoWebServices.Contains( "成功") == true )
            {
                CurrentSendOrderItem = SendOrderList[SendOrderIndex];
                CurrentSendOrderItem.Status = ReturnStatusDesc(CurrentSendOrderItem.OrderId);

                var ntg = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_set_rtu_fault_pandan;

                ntg.WstFaultCurr.FaultItemsAdd.Add(new EquipmentFaultCurr.OneFaultItem
                                                       {
                                                           RtuId = CurrentSendOrderItem.RtuId,
                                                           FaultId = CurrentSendOrderItem.FaultId,
                                                           LoopId = CurrentSendOrderItem.LoopId,
                                                           PaiDan = Convert.ToString(CurrentOrderID)
                                                       });

                Wlst.Sr.PPPandSocketSvr.Server.SocketClient.SndData(ntg);


                CurrentOrderID++;

                WriteNewOrderID(Convert.ToString(CurrentOrderID));

                SendOrderIndex++;

                if (SendOrderIndex >= SendOrderList.Count)
                {
                    SendOrderIndex = SendOrderList.Count - 1;
                }

                CurrentSendOrderItem = SendOrderList[SendOrderIndex];


            }
            else
            {
                CurrentSendOrderItem = SendOrderList[SendOrderIndex];
                CurrentSendOrderItem.OrderTime = string.Empty;
            }

            MessageShow = sendDatatoWebServices;

        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public string Title
        {
            get { return "故障派单"; }
        }

        #endregion

        #region CmdSendOrder
        private ICommand _cmdSendOrder;

        public ICommand CmdSendOrder
        {
            get
            {
                if (_cmdSendOrder == null)
                    _cmdSendOrder = new RelayCommand(ExSendOrder, CanSendOrder, false);
                return _cmdSendOrder;
            }
        }

        private bool CanSendOrder()
        {
            return SendOrderList.Count > 0;
        }

        private void ExSendOrder()
        {
            SendOrder();
        }
        #endregion

        #region define

        private ObservableCollection<SendOrderItems> _sendOrderList;

        public ObservableCollection<SendOrderItems> SendOrderList
        {
            get
            {
                if (_sendOrderList == null)
                {
                    _sendOrderList = new ObservableCollection<SendOrderItems>();
                }
                return _sendOrderList;
            }
            set
            {
                if (value == _sendOrderList) return;
                _sendOrderList = value;
                this.RaisePropertyChanged(() => SendOrderList);
            }
        }

        private SendOrderItems _currentSendOrderItem;

        public SendOrderItems CurrentSendOrderItem
        {
            get
            {
                if (_currentSendOrderItem == null)
                {
                    _currentSendOrderItem = new SendOrderItems();
                }
                return _currentSendOrderItem;
            }
            set
            {
                if (value == _currentSendOrderItem) return;
                _currentSendOrderItem = value;
                this.RaisePropertyChanged(() => CurrentSendOrderItem);
            }
        }

        private string _messageShow;
        public string MessageShow
        {
            get { return _messageShow; }
            set
            {
                if (_messageShow == value) return;
                _messageShow = value;
                RaisePropertyChanged(() => MessageShow);
            }
        }

        #endregion
    }

}
