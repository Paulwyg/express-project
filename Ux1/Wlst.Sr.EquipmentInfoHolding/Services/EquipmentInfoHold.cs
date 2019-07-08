using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.EquipmentInfoHolding.Config;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    public partial class EquipmentDataInfoHold : EventHandlerHelperExtendNotifyProperyChanged
    {
        #region

        private static EquipmentDataInfoHold _mySlef;

        public static EquipmentDataInfoHold MySlef
        {
            get
            {
                if (_mySlef == null) new EquipmentDataInfoHold();
                return _mySlef;
            }
        }


        /// <summary>
        /// 执行初始化并向服务器请求数据
        /// </summary>
        public void InitStart()
        {
            //初始化清空 条形码与集中器id对应关系 lvf 2018年6月7日09:08:45
            CtrlSluInfo.Clear();

            long xxx = DateTime.Now.Ticks;
            var tmp = this.LoadRtufromXml();
            foreach (var t in tmp)
            {
                try
                {
                    var info = ConvertSvrdatatoLocaldata(t.Value);
                    if (info == null)
                    {
                        continue;
                    }
                    if (!Info.ContainsKey(t.Key))
                    {
                        Info.TryAdd(t.Key, info);
                        if (!LstLocaldata.ContainsKey(t.Key))
                            LstLocaldata.Add(t.Key, info.DateUpdate);
                    }
                }
                catch (Exception)
                {

                }

            }
            var longx = DateTime.Now.Ticks - xxx;
            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("Load local data waste:" + longx);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEquipmentInfoLstfromServer, 0);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestDataBaseBriefInfofromServer, 45);
            //RequestEquipmentInfoLstfromServer();
        }

        private EquipmentDataInfoHold()
        {
            if (_mySlef != null) return;
            _mySlef = this;
            this.InitEvent();
            this.InitAciotn();
            MaxPackageNumber = Config1.ConfigPackageRequestRtus;

            Xmlfilepath = Environment.CurrentDirectory + "\\rtudata";
        }

        #endregion

        /// <summary>
        /// 系统所有设备列表
        /// </summary>
        public ConcurrentDictionary<int, WjParaBase> Info = new ConcurrentDictionary<int, WjParaBase>();

        /// <summary>
        /// 需要置顶的终端列表
        /// </summary>
        public List<int> RtusNeedTopShow = new List<int>();

        //当前用户权限
        public List<int> UserPowerCanR = new List<int>();
        public List<int> UserPowerCanW = new List<int>();
        public List<int> UserPowerCanX = new List<int>();

        /// <summary>
        /// 系统所有控制器 条形码对应集中器编号sluid,ctrlid 字典
        /// </summary>
        public static ConcurrentDictionary<long, Tuple<int, int>> CtrlSluInfo = new ConcurrentDictionary<long, Tuple<int, int>>();

        public static ConcurrentDictionary<int, WjParaBase> InfoItems
        {
            get { return MySlef.Info; }
        }

        /// <summary>
        /// 根据设备逻辑地址获取设备信息；不存在返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回null</returns>
        public static WjParaBase GetInfoById(int id)
        {
            return MySlef.Info.ContainsKey(id) ? MySlef.Info[id] : null;
        }

        public static List<WjParaBase> GetInfoList()
        {
            return (from pair in MySlef.Info orderby pair.Key select pair.Value).ToList();
        }

        public static List<WjParaBase> GetInfoList(WjParaBase.EquType equType)
        {
            return
                (from pair in MySlef.Info
                 where pair.Value.EquipmentType == equType
                 orderby pair.Key
                 select pair.Value).ToList();
        }

        /// <summary>
        /// 根据控制器条形码获取集中器地址  lvf 2018年6月7日14:34:47
        /// </summary>
        /// <param name="barcodeId">条形码</param>
        /// <returns>item1:SluId;item2:CtrlId</returns>
        public static Tuple<int,int > GetSluIdByLampCode(long barcodeId)
        {
            return CtrlSluInfo.ContainsKey(barcodeId) ? CtrlSluInfo[barcodeId] :null;
        }

        /// <summary>
        /// 根据设备型号和物理地址获取逻辑地址 lvf 2018年6月7日14:34:54
        /// </summary>
        /// <param name="phyId"></param>
        /// <param name="equType"></param>
        /// <returns></returns>
        public static int GetLidByPhyId(int phyId,WjParaBase.EquType equType)
        {
            var list = GetInfoList(equType);
            foreach (var g in list)
            {
                if (g.RtuPhyId == phyId) return g.RtuId;
            }

            return 0;

        }


        /// <summary>
        /// 根据设备型号和物理地址获取逻辑地址 lvf 2018年6月7日14:34:54
        /// </summary>
        /// <param name="phyId"></param>
        /// <param name="equType"></param>
        /// <returns></returns>
        public static int GetBarCodeByPhyId(int phyId, WjParaBase.EquType equType)
        {
            var list = GetInfoList(equType);
            foreach (var g in list)
            {
                if (g.RtuPhyId == phyId) return g.RtuId;
            }

            return 0;

        }



        /// <summary>
        /// 根据设备逻辑地址获取设备信息；不存在返回-1; lvf 2019年5月23日14:24:09
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回-1</returns>
        public static int GetSluIdByRtuId(int id)
        {
            var list = GetInfoList(WjParaBase.EquType.Slu);
            foreach (var g in list)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                        ContainsKey(g.RtuId))
                    continue;
                var t =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g.RtuId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

                if (t.WjSlu.RelatedRtuId == id) return t.RtuId;

            }

            return -1;


        }

    }


    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentDataInfoHold
    {
        private void InitEvent()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);

        }

        /// <summary>
        /// 事件数据处理
        /// </summary>
        /// <param name="args"></param>
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                this.RequestEquipmentInfoLstfromServer();
                return;
            }
        }
        

        private void InitAciotn()
        {

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_svr_ans_cnt_request_add_equ,
                //.ProtocolCnt.ClientPart.wlst_equipment_server_ans_client_add_Equipment,
                addEquipment,
                typeof (EquipmentDataInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_update_equ,
                //.ProtocolCnt.ClientPart.wlst_equipment_server_ans_client_update_Equipment, 
                UpdateEquipment,
                typeof (EquipmentDataInfoHold), this);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_request_equ,
                //ProtocolCnt.ClientPart.wlst_equipment_server_ans_client_request_Equipment, 
                requestEquipment,
                typeof (EquipmentDataInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_delete_equ,
                //ProtocolCnt.ClientPart.wlst_equipment_server_ans_client_delete_Equipment, 
                deleteEquipment,
                typeof (EquipmentDataInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_request_equ_md5,
                //ProtocolCnt.ClientPart.wlst_equipment_server_ans_client_request_EquipentInfoList,
                requestEquipmentList,
                typeof (EquipmentDataInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEqu.wst_update_map_xy,
                //ProtocolCnt.ClientPart.wlst_equipment_server_ans_client_update_EquipentxyPositon,
                update_EquipentxyPositon,
                typeof (EquipmentDataInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wlst_svr_ans_cnt_request_database_brief_info,
                //ProtocolCnt.ClientPart.wlst_svr_to_clinet_database_biref, 
                OnGetDataBaseBriefInfo,
                typeof (EquipmentDataInfoHold), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wlst_svr_ans_cnt_request_database_package_info,
                //.ProtocolCnt.ClientPart.wlst_svr_to_clinet_database_info, 
                OnGetDatasInfo,
                typeof (EquipmentDataInfoHold), this);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxLogin.wst_super_admin_login,
                //.ProtocolCnt.ClientPart.wlst_svr_to_clinet_database_info, 
                OnSuperAdminLogin,
                typeof (EquipmentDataInfoHold), this);

        }


        #region 备份数据库

        private static Dictionary<int, Wlst.client.SvrAnsRquDataBaseInfo> DataBaseInfo =
            new Dictionary<int, Wlst.client.SvrAnsRquDataBaseInfo>();

        private static int PackageLength = 1024 * 40;
        private static long lastGetInfotime = 0;
        private static int PackageSum = 0;
        private static string FileNames = "database.rar";

        Thread threadona = new Thread(threadruns);

        public void OnGetDataBaseBriefInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            DataBaseInfo.Clear();
            if (EquipmentInfoHolding.Services.Others.CopyDataBaseFromSvr == false) return;
            var datax = infos.WstSysSvrAnsCntRequestDatabaseBiefInfo;
            if (datax.CanCntGet == false) return;
            if (string.IsNullOrEmpty(datax.Name)) return;
            if (!System.IO.Directory.Exists("DataBaseBackUp"))
            {
                System.IO.Directory.CreateDirectory("DataBaseBackUp");
            }
            string filefullname = @"DataBaseBackUp\" + datax.Name;
            if (System.IO.File.Exists(filefullname)) return;


            FileNames = datax.Name;
            //  int packageleng = 1024*40;
            PackageSum = 0;
            if (datax.Length % PackageLength == 0) PackageSum = datax.Length / PackageLength;
            else PackageSum = datax.Length / PackageLength + 1;

            var nts = Wlst.Sr.ProtocolPhone.LxSys.wlst_cnt_request_database_package_info;//.ServerPart.wlst_clinet_request_database_info;
            nts.WstSysCntRequestDatabaseInfo.PackageLength = PackageLength;
            nts.WstSysCntRequestDatabaseInfo.PackageSum = PackageSum;
            nts.WstSysCntRequestDatabaseInfo.RquPackages = new List<int>();

            for (int i = 0; i < PackageSum; i++)
            {
                nts.WstSysCntRequestDatabaseInfo.RquPackages.Add(i);
                if (nts.WstSysCntRequestDatabaseInfo.RquPackages.Count > 6) break;
            }
            SndOrderServer.OrderSnd(nts, 10, 3);

            lastGetInfotime = DateTime.Now.Ticks;
            try
            {
                if (threadona.IsAlive) threadona.Abort();
                threadona = null;
            }
            catch (Exception ex)
            {

            }
            try
            {
                threadona = new Thread(threadruns);
                threadona.Start();
            }
            catch (Exception ex)
            {

            }

        }

        static void threadruns()
        {
            while (true)
            {

                try
                {
                    if (DataBaseInfo.Count == PackageSum)
                    {
                        try
                        {
                            var str = "";
                            var ntssss = (from t in DataBaseInfo orderby t.Key ascending select t.Value).ToList();
                            foreach (var t in ntssss) str += t.Data;

                            if (!System.IO.Directory.Exists("DataBaseBackUp"))
                            {
                                System.IO.Directory.CreateDirectory("DataBaseBackUp");
                            }
                            string filefullname = @"DataBaseBackUp\" + FileNames;
                            var cvr = System.Convert.FromBase64String(str);

                            FileStream fsr = new FileStream(filefullname, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            BinaryWriter binaryWriter = new BinaryWriter(fsr);
                            binaryWriter.Write(cvr);
                            fsr.Close();
                        }
                        catch (Exception ex)
                        {
                            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("系统备份来自服务器的数据库出错.. ,代码:" + ex);
                        }
                        DataBaseInfo.Clear();
                        break;
                    }
                    else
                    {
                        if (DateTime.Now.Ticks - lastGetInfotime > 300000000)
                        {
                            var nts = Wlst.Sr.ProtocolPhone.LxSys.wlst_cnt_request_database_package_info;//.ServerPart.wlst_clinet_request_database_info;
                            nts.WstSysCntRequestDatabaseInfo.PackageLength = PackageLength;
                            nts.WstSysCntRequestDatabaseInfo.PackageSum = PackageSum;
                            nts.WstSysCntRequestDatabaseInfo.RquPackages = new List<int>();
                            for (int i = 0; i < PackageSum; i++)
                            {
                                if (!DataBaseInfo.ContainsKey(i))
                                    nts.WstSysCntRequestDatabaseInfo.RquPackages.Add(i);
                                if (nts.WstSysCntRequestDatabaseInfo.RquPackages.Count > 6) break;
                            }
                            SndOrderServer.OrderSnd(nts, 10, 3);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(3000);
            }
        }

        public void OnGetDatasInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos == null) return;
            if (infos.WstSysSvrAnsCntRequestDatabaseInfo == null) return;
            if (DataBaseInfo.ContainsKey(infos.WstSysSvrAnsCntRequestDatabaseInfo.CurrentPackageIndex)) return;
            lastGetInfotime = DateTime.Now.Ticks;

            DataBaseInfo.Add(infos.WstSysSvrAnsCntRequestDatabaseInfo.CurrentPackageIndex, infos.WstSysSvrAnsCntRequestDatabaseInfo);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "数据库备份", OperatrType.SystemInfo,
                                                                  (infos.WstSysSvrAnsCntRequestDatabaseInfo.CurrentPackageIndex + 1) + "/" +
                                                                  infos.WstSysSvrAnsCntRequestDatabaseInfo.PackageSum);

            if (infos.WstSysSvrAnsCntRequestDatabaseInfo.RquPackagesIndex + 1 == infos.WstSysSvrAnsCntRequestDatabaseInfo.RquPackagesSum)
            {
                var nts = Wlst.Sr.ProtocolPhone.LxSys.wlst_cnt_request_database_package_info;//.ServerPart.wlst_clinet_request_database_info;
                nts.WstSysCntRequestDatabaseInfo.PackageLength = PackageLength;
                nts.WstSysCntRequestDatabaseInfo.PackageSum = PackageSum;
                nts.WstSysCntRequestDatabaseInfo.RquPackages = new List<int>();
                for (int i = 0; i < PackageSum; i++)
                {
                    if (!DataBaseInfo.ContainsKey(i))
                        nts.WstSysCntRequestDatabaseInfo.RquPackages.Add(i);
                    if (nts.WstSysCntRequestDatabaseInfo.RquPackages.Count > 6) break;
                }
                SndOrderServer.OrderSnd(nts, 10, 3);
            }
        }




        public void OnSuperAdminLogin(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos == null) return;
            if (infos.Head.Idf == UserInfo.UserLoginInfo.UserName) return;
            if (infos.Head.Ret == 1)
            {
                if (UserInfo.UserLoginInfo.AreaW.Count == 0 || UserInfo.UserLoginInfo.AreaX.Count == 0)
                    return;
                UserPowerCanX.Clear();
                UserPowerCanX.AddRange(UserInfo.UserLoginInfo.AreaX);
                UserPowerCanW.Clear();
                UserPowerCanW.AddRange(UserInfo.UserLoginInfo.AreaW);


                UserInfo.UserLoginInfo.AreaW.Clear();
                UserInfo.UserLoginInfo.AreaX.Clear();
            }
            else
            {
                if (UserPowerCanW.Count == 0 && UserPowerCanX.Count == 0) return;
                UserInfo.UserLoginInfo.AreaW.Clear();
                UserInfo.UserLoginInfo.AreaW.AddRange(UserPowerCanW);
                UserInfo.UserLoginInfo.AreaX.Clear();
                UserInfo.UserLoginInfo.AreaX.AddRange(UserPowerCanX);
                UserPowerCanW.Clear();
                UserPowerCanX.Clear();
            }


        }

        #endregion

        //更新版本 
        public static long Version = 0;
        public void update_EquipentxyPositon(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstEquUpdateMapXy ;
            if (info != null)
            {
                if (Info.ContainsKey(info.RtuId))
                {
                    Info[info.RtuId].RtuMapX = info.Xmap;
                    Info[info.RtuId].RtuMapY = info.Ymap;

                    var ar = new PublishEventArgs()
                    {
                        EventId = EventIdAssign.EquipentxyPositonUpdateId,
                        EventType = PublishEventType.Core
                    };

                    ar.AddParams(info.RtuId);
                    //ar.AddParams(infos.Guid);
                    EventPublish.PublishEvent(ar);
                }

            }
        }

        public void requestEquipmentList(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstEquRequestMd5  ;
            if (info == null) return;

            var arg = new PublishEventArgs()
                          {
                              EventId = EventIdAssign.RequestNewRtuInAreas,
                              EventType = PublishEventType.Core
                          };
            if (info.Op == 1)
            {
                if (info.RtuIdsThatAreaNew == null) return;

                // 更新的终端与当前终端一致  不需要更新 退出
                if (info.RtuIdsThatAreaNew.Count == RtusNeedTopShow.Count)
                {
                    bool allthesame = true;
                    foreach (var f in info.RtuIdsThatAreaNew)
                        if (RtusNeedTopShow.Contains(f) == false)
                        {
                            allthesame = false;
                            break;
                        }
                    if (allthesame) return;
                }

                RtusNeedTopShow = info.RtuIdsThatAreaNew;

                EventPublish.PublishEvent(arg);
                return;
            }


            TmlInfoLstBack(info);

            if (info.RtuIdsThatAreaNew == null) RtusNeedTopShow = new List<int>();
            else
                RtusNeedTopShow = info.RtuIdsThatAreaNew;

            EventPublish.PublishEvent(arg);

        }

        public void addEquipment(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.Head.Ret>999)
            {
                int areaId = infos.Head.Ret - 1000;
                string mess = "设备物理地址已存在于 区域：" + areaId  + "   请更改物理地址";
                UMessageBox.Show("添加错误", mess, UMessageBoxButton.Ok);
                return;
            }
            UpdateInfo(infos.WstEquSvrAnsCntRequestAdd, infos.Head.Gid);
            Version = DateTime.Now.Ticks;
            try
            {
                var lstAdd = new List<Tuple<int, int>>();
                foreach (var t in infos.WstEquSvrAnsCntRequestAdd.EquLst)
                {
                    try
                    {
                        var info = ConvertSvrdatatoLocaldata(t);
                        if (info == null) continue;
                        lstAdd.Add(new Tuple<int, int>(info.RtuId, info.RtuFid ));
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add terinfo error:" + ex.ToString());
                    }
                }
                if (lstAdd.Count == 0) return;
                var ar = new PublishEventArgs()
                {
                    EventId = EventIdAssign.EquipmentUserAddEventId,
                    EventType = PublishEventType.Core
                };

                ar.AddParams(lstAdd);
                ar.AddParams(infos.Head.Gid);
                EventPublish.PublishEvent(ar);
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateEquipment(string session, Wlst.mobile.MsgWithMobile infos)
        {
            Version = DateTime.Now.Ticks;
            UpdateInfo(infos.WstEquUpdate  , infos.Head.Gid);
        }

        public void requestEquipment(string session, Wlst.mobile.MsgWithMobile infos)
        {
            // LogInfo.Log("服务器应答请求设备信息!!!");
            UpdateRequest(infos.WstEquRequest );
        }

        public void deleteEquipment(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstEquDelete ;
            if (info != null)
            {
                Version = DateTime.Now.Ticks;
                DeleteInfo(info);
            }
        }

       

        #region to from xml

        protected string Xmlfilepath = string.Empty;

        protected void SaveRtuToXml(EquipmnetInfoCnt cnt)
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                if (cnt.Equipment == null) return;
                string path = Xmlfilepath + "\\" + cnt.Equipment.RtuId;
                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.SaveAsXml(path, cnt);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("终端数据写入本地文本异常:" + ex);
            }
        }


        protected Dictionary<int, EquipmnetInfoCnt> LoadRtufromXml()
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                DirectoryInfo theFolder = new DirectoryInfo(Xmlfilepath);
                var fileInfo = theFolder.GetFiles();
                //遍历文件夹
                var lst = new Dictionary<int, EquipmnetInfoCnt>();
                foreach (FileInfo nextFile in fileInfo) //遍历文件
                {
                    var tmp = Wlst.Cr.CoreOne.Seridata.ClassToFileStream.LoadFromXml<EquipmnetInfoCnt>(nextFile.FullName);
                    // this.listBox2.Items.Add(nextFile.FullName );
                    if (tmp != null && tmp.Equipment != null)
                    {
                        if (!lst.ContainsKey(tmp.Equipment.RtuId))
                            lst.Add(tmp.Equipment.RtuId, tmp);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("读取本地终端数据异常:" + ex);
            }
            return new Dictionary<int, EquipmnetInfoCnt>();
        }

        #endregion

        #region 清单列表 TmlInfoLstBack

        /// <summary>
        /// 客户端需要请求的设备清单
        /// </summary>
        protected List<int> LstWantInfo = new List<int>();


        private int RequestLstCount = 0;
        protected bool firstback = true;
        protected void TmlInfoLstBack(ServerEquipmentMd5Info info)
        {
            LstWantInfo.Clear();

            if (info == null)
            {
                RequestLstCount++;
                if (RequestLstCount > 6) return;
                this.RequestEquipmentInfoLstfromServer();
                return;
            }
            //if (info.InfoList.Count < 1)
            //{
            //    return;
            //}
            var lstNeeddledte = new List<int>();
            var lstsvrall = (from t in info.InfoList select t.RtuId).ToList();
            var tmpsss = Info.Keys.ToList();
            foreach (var t in tmpsss)
            {
                if (!lstsvrall.Contains(t))
                {
                    WjParaBase data;
                    Info.TryRemove(t,out data );
                    lstNeeddledte.Add(t);
                }
            }
            if (lstNeeddledte.Count > 0)
            {
                DeleteRtuData(lstNeeddledte);
            }


            var lstInfo = new List<int>();
            foreach (var t in info.InfoList)
            {
                lstInfo.Add(t.RtuId);
                if (Info.ContainsKey(t.RtuId) && t.LastUpdateTime == Info[t.RtuId].DateUpdate )
                {
                    continue;
                }
                LstWantInfo.Add(t.RtuId);
            }


            if (LstWantInfo.Count == 0 && firstback)
            {
                UpdateRequestInThread();
            }
            firstback = false;

            var lstDelete =
                (from t in Info where !lstInfo.Contains(t.Key) select new Tuple<int, int>(t.Key, t.Value.RtuFid )).
                    ToList();
            if (lstDelete.Count > 0)
            {
                foreach (var t in lstDelete)
                {
                    if (Info.ContainsKey(t.Item1))
                    {
                        WjParaBase data;
                        Info.TryRemove( t.Item1,out data );
                    }
                }
                this.UpdateAttachLst();

                var ar = new PublishEventArgs()
                {
                    EventId = EventIdAssign.EquipmentDeleteEventId,
                    EventType = PublishEventType.Core
                };
                ar.AddParams(lstDelete);
                EventPublish.PublishEvent(ar);
            }

            var rtulst = info.RtuIdsThatAreaNew;

           
            

           
            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.RequestNewRtuInAreas,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);

            RequestNextPackageInfo();

        }

        private void DeleteRtuData(List<int> lst)
        {
            if (!Directory.Exists(Xmlfilepath)) return;
            foreach (var t in lst)
            {
                string path = Xmlfilepath + "\\" + t;
                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.DeleteXml(path);
            }
        }

        #endregion

        #region 删除设备  DeleteInfo

        /// <summary>
        /// 删除设备
        /// </summary>
        private void DeleteInfo(EquipmentInfo tmlInfoExchangeforUpdatetmlinfo)
        {
            if (tmlInfoExchangeforUpdatetmlinfo == null) return;

            var lstDelete = new List<Tuple<int, int>>();
            //如果型号发生变化 需要进一步修改 
            foreach (var t in tmlInfoExchangeforUpdatetmlinfo.LstInfo)
            {
                if (Info.ContainsKey(t))
                { WjParaBase data;
                    if (Info[t].RtuFid  == 0)
                    {
                        lstDelete.Add(new Tuple<int, int>(t, 0));

                        foreach (var gg in Info[t].EquipmentsThatAttachToThisRtu)
                        {
                            lstDelete.Add(new Tuple<int, int>(gg, t));
                           
                            if (Info.ContainsKey(gg)) Info.TryRemove( gg,out data );
                        }

                        //如果删除的是集中器   更新ctrlSluInfo字典 lvf 2018年6月7日09:39:07
                        if(t>1500000 && t<1600000)
                        {
                            var ctrlDelLst = new List<long>();
                            foreach (var g in CtrlSluInfo)
                            {
                                if (g.Value.Item1 == t && ctrlDelLst.Contains(g.Key) == false) ctrlDelLst.Add(g.Key);
                            }
                            foreach (var g in ctrlDelLst)
                            {
                                Tuple<int,int> x = new Tuple<int, int>(0,0);
                                CtrlSluInfo.TryRemove(g, out x);
                            }
                        
                        }



                    }
                    else
                    {
                        lstDelete.Add(new Tuple<int, int>(t, Info[t].RtuFid ));
                    }

                    Info.TryRemove( t,out data );
                }
            }

            UpdateAttachLst();

            if (lstDelete.Count > 0)
            {
                var ar = new PublishEventArgs()
                {
                    EventId = EventIdAssign.EquipmentDeleteEventId,
                    EventType = PublishEventType.Core
                };
                ar.AddParams(lstDelete);
                EventPublish.PublishEvent(ar);
            }

        }

        #endregion

        #region 更新设备 UpdateInfo

        /// <summary>
        /// 线程执行数据更新   需要预先赋值  _tmlInfoExchangeforUpdatetmlinfo
        /// </summary>
        private void UpdateInfo(Wlst .client .EquipmentInfo  tmlInfoExchangeforUpdatetmlinfo, long guid)
        {
            if (tmlInfoExchangeforUpdatetmlinfo == null) return;
            //如果型号发生变化 需要进一步修改 
            if (tmlInfoExchangeforUpdatetmlinfo.LstInfo == null) return;
            try
            {

                var lstAdd = new List<Tuple<int, int>>();
                var lstUpdate = new List<Tuple<int, int>>();
                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.EquLst )
                {
                    try
                    {
                        var info =ConvertSvrdatatoLocaldata(  t);
                        if (info == null) continue;
                        if (Info.ContainsKey(info.RtuId))
                        {
                            if (Info[info.RtuId].DateUpdate  == info.DateUpdate ) continue;
                            Info[info.RtuId] = info;
                            lstUpdate.Add(new Tuple<int, int>(info.RtuId, info.RtuFid ));
                            continue;
                        }
                        Info.TryAdd( info.RtuId, info);
                        lstAdd.Add(new Tuple<int, int>(info.RtuId, info.RtuFid ));

                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add terinfo error:" + ex.ToString());
                    }
                }

                UpdateAttachLst();

                if (lstUpdate.Count > 0)
                {
                    var ar = new PublishEventArgs()
                    {
                        EventId = EventIdAssign.EquipmentUpdateEventId,
                        EventType = PublishEventType.Core
                    };
                    ar.AddParams(lstUpdate);
                    EventPublish.PublishEvent(ar);
                }
                if (lstAdd.Count > 0)
                {
                    var ar = new PublishEventArgs()
                    {
                        EventId = EventIdAssign.EquipmentAddEventId,
                        EventType = PublishEventType.Core
                    };
                    ar.AddParams(lstAdd);
                    ar.AddParams(guid);
                    EventPublish.PublishEvent(ar);
                }


            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml core data ,ex:" + ex);
            }
        }

        #endregion

        #region 请求数据 UpdateRequest


        private Thread _threadChanged;
        private bool _isThreadRunning = false;
        private bool _updated = false;
        // private EqipentInfoExchange _requestedEquipment = new EqipentInfoExchange();
        private ConcurrentQueue<EquipmnetInfoCnt> _requestedEquipment = new ConcurrentQueue<EquipmnetInfoCnt>();
        private ConcurrentQueue<WjParaBase> changedInfo = new ConcurrentQueue<WjParaBase>();

        protected Dictionary<int, long> LstLocaldata = new Dictionary<int, long>();


        void UpdateRequest(Wlst .client .EquipmentInfo  tmlInfoExchangeforUpdatetmlinfo)
        {
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "终端参数", OperatrType.SystemInfo, "到达");
            if (_updated == false)
            {

                if (tmlInfoExchangeforUpdatetmlinfo.EquLst  != null)
                {
                    foreach (var t in tmlInfoExchangeforUpdatetmlinfo.EquLst )
                    {
                        _requestedEquipment.Enqueue(t);
                        if (LstWantInfo.Contains(t.RtuId)) LstWantInfo.Remove(t.RtuId);
                    }
                }
                if (_isThreadRunning == false)
                {
                    _isThreadRunning = true;
                    try
                    {
                        _threadChanged.Abort();
                    }
                    catch (Exception ex)
                    {
                    }

                    _threadChanged = null;
                    _threadChanged = new Thread(ChangeSntToCntInfo);
                    _threadChanged.Start();
                }
                if (LstWantInfo.Count == 0)
                {
                    Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "终端参数", OperatrType.SystemInfo, "到达完毕");
                    _updated = true;

                }
                else
                {
                    RequestNextPackageInfo();
                }
            }
            else
            {
                UpdateRequestWithOutThread(tmlInfoExchangeforUpdatetmlinfo);
            }

        }

        /// <summary>
        /// 正常数据更新
        /// </summary>
        private void UpdateRequestWithOutThread(Wlst .client .EquipmentInfo  tmlInfoExchangeforUpdatetmlinfo)
        {
            // var tmlInfoExchangeforUpdatetmlinfo = tmlInfoExchangeforUpdatetmlinfos as EqipentInfoExchange;
            if (tmlInfoExchangeforUpdatetmlinfo == null) return;
            //如果型号发生变化 需要进一步修改 
            //  Wlst .Cr .CoreMims .ShowMsgInfo .ShowNewMsg .AddNewShowMsg(0,"终端参数通信完成",OperatrType.SystemInfo ,"完成");

            if (tmlInfoExchangeforUpdatetmlinfo.EquLst  != null)
                try
                {
                    var lstAdd = new List<Tuple<int, int>>();
                    var lstUpdate = new List<Tuple<int, int>>();
                    foreach (var t in tmlInfoExchangeforUpdatetmlinfo.EquLst )
                    {
                        try
                        {
                            if (t.Equipment == null) continue;
                            WjParaBase data = ConvertSvrdatatoLocaldata( t );
                            if (data == null) continue;
                            if (LstWantInfo.Contains(t.RtuId)) LstWantInfo.Remove(t.RtuId);

                            if (Info.ContainsKey(t.RtuId))
                            {
                                if (Info[t.RtuId].DateUpdate == t.Equipment .DateUpdate ) continue;
                                Info[t.RtuId] = data ;
                                lstUpdate.Add(new Tuple<int, int>(t.RtuId, data .RtuFid ));

                                continue;
                            }
                            Info.TryAdd( t.RtuId, data );
                            lstAdd.Add(new Tuple<int, int>(t.RtuId, t.Equipment .RtuFid ));


                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteLogError("Add terinfo error:" + ex.ToString());
                        }
                        //Thread.Sleep(1);
                    }
                    // Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "终端参数加载完成", OperatrType.SystemInfo, "完成");
                    UpdateAttachLst();
                    if (lstUpdate.Count > 0)
                    {
                        var ar = new PublishEventArgs()
                        {
                            EventId = EventIdAssign.EquipmentUpdateEventId,
                            EventType = PublishEventType.Core
                        };
                        ar.AddParams(lstUpdate);
                        EventPublish.PublishEvent(ar);
                    }
                    if (lstAdd.Count > 0)
                    {
                        var ar = new PublishEventArgs()
                        {
                            EventId = EventIdAssign.EquipmentAddEventId,
                            EventType = PublishEventType.Core
                        };
                        ar.AddParams(lstAdd);
                        EventPublish.PublishEvent(ar);
                    }
                    tmlInfoExchangeforUpdatetmlinfo.LstInfo.Clear();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("Error to update tml core data ,ex:" + ex);
                }

            //mkx
            var arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.SingleInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);

            arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.MulityInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);
            //RequestNextPackageInfo();
            //Wlst.Cr.Core.ModuleServices.DelayEvent.RaiseEventHappen(DelayEventHappen.EventOne);
        }


        private WjParaBase ConvertSvrdatatoLocaldata(EquipmnetInfoCnt data)
        {

            if (data.Equipment.RtuModel == EnumRtuModel.Wj3090 || data.Equipment.RtuModel == EnumRtuModel.Wj3006 || data.Equipment.RtuModel ==EnumRtuModel.Wj4005 ||
                data.Equipment.RtuModel == EnumRtuModel.Wj3005 || data.Equipment.RtuModel == EnumRtuModel.Gz6005)
            {
                return new Wj3005Rtu(data.Equipment, data.Voltage, data.ParaGprs, data.AmpLoop, 
                                     data.SwitchOut);
            }
            if (data.Equipment.RtuModel == EnumRtuModel.Wj2090)
            {
                //记录  条形码 与 集中器id 对应关系 lvf 2018年6月7日09:16:00
                //先删除 原有的控制器
                var ctrlDelLst = new List<long >();
                foreach (var g in CtrlSluInfo)
                {
                    if (g.Value.Item1 == data.RtuId && ctrlDelLst.Contains(g.Key) == false) ctrlDelLst.Add(g.Key);
                }
                foreach (var g in ctrlDelLst)
                {
                    Tuple<int,int> x =new Tuple<int, int>(0,0);
                    CtrlSluInfo.TryRemove(g, out x);
                }
                //添加
                foreach (var g in data.SluCtrRegulator)
                {
                    CtrlSluInfo.TryAdd(g.BarCodeId,new Tuple<int, int>(data.RtuId,g.CtrlId));
                }
                return new Wj2090Slu(data.Equipment, data.Wj2090Slu, data.SluCtrRegulator, data.SluCtrRegulatorGrp);
            }

            if (data.Equipment.RtuModel == EnumRtuModel.Wj1050) return new Wj1050Mru(data.Equipment, data.Wj1050Mru);
            if (data.Equipment.RtuModel == EnumRtuModel.Jd601) return new Wj601Esu(data.Equipment, data.Jd601Esu);
            if (data.Equipment.RtuModel == EnumRtuModel.Wj1080) return new Wj1080Lux(data.Equipment,data.Wj1080Lux);
            if (data.Equipment.RtuModel == EnumRtuModel.Wj1090 || data.Equipment.RtuModel == EnumRtuModel.Wj30920 ||
                data.Equipment.RtuModel == EnumRtuModel.Wj30910) return new Wj1090Ldu(data.Equipment, data.LduLines);
            if (data.Equipment.RtuModel == EnumRtuModel.Wj9001) return new Wj9001Leak(data.Equipment, data.LeakLines);
            return null;
        }


        private EquipmnetInfoCnt ConvertLoacldatatoSvrdata(object data)
        {
            var basedata = data as WjParaBase;
            if (basedata == null) return null;
            var svrdata = new EquipmnetInfoCnt()
                              {Equipment = basedata, RtuId = basedata.RtuId, AttachRtuId = basedata.RtuFid};



            var wj3005rtu = data as Wj3005Rtu;
            if (wj3005rtu != null)
            {
                svrdata.AmpLoop = wj3005rtu.WjLoops.Values.ToList();

                svrdata.SwitchOut = wj3005rtu.WjSwitchOuts.Values.ToList();
                svrdata.Voltage = wj3005rtu.WjVoltage;
                svrdata.ParaGprs = wj3005rtu.WjGprs;
                return svrdata;
            }

            var wj601esu = data as Wj601Esu;
            if (wj601esu != null)
            {
                svrdata.Jd601Esu = wj601esu.WjEsu;
                return svrdata;
            }
            var wj2090slu = data as Wj2090Slu;
            if (wj2090slu != null)
            {
                svrdata.SluCtrRegulator = wj2090slu.WjSluCtrls.Values.ToList();
                svrdata.Wj2090Slu = wj2090slu.WjSlu;
                svrdata.SluCtrRegulatorGrp = (from t in wj2090slu.WjSluCtrlGrps select t.Value).ToList();
                return svrdata;
            }
            var wj1090lines = data as Wj1090Ldu;
            if (wj1090lines != null)
            {
                svrdata.LduLines = wj1090lines.WjLduLines.Values.ToList();
                return svrdata;
            }
            var wj1080 = data as Wj1080Lux;
            if (wj1080 != null)
            {
                svrdata.Wj1080Lux = wj1080.WjLux;
                return svrdata;
            }

            var wj1050 = data as Wj1050Mru;
            if (wj1050 != null)
            {
                svrdata.Wj1050Mru = wj1050.WjMru;
                return svrdata;
            }
            var wj9001 = data as Wj9001Leak;
            if (wj9001 != null)
            {
                svrdata.LeakLines = wj9001.WjLeakLines.Values.ToList();
                return svrdata;
            }
            return null;
        }

        /// <summary>
        /// 线程执行数据更新   需要预先赋值  _tmlInfoExchangeforUpdatetmlinfo
        /// </summary>
        private void ChangeSntToCntInfo()
        {
            int count = 0;
            while (true)
            {
                try
                {
                    EquipmnetInfoCnt tmp = null;
                    while (_requestedEquipment.TryDequeue(out tmp))
                    {
                        if (tmp == null) continue;
                        try
                        {

                            var info = ConvertSvrdatatoLocaldata(tmp);
                            if (info == null)
                            {
                                continue;
                            }
                            changedInfo.Enqueue(info);
                            if (!LstLocaldata.ContainsKey(info.RtuId))
                            {
                                this.SaveRtuToXml(tmp);
                                LstLocaldata.Add(info.RtuId, info.DateUpdate );
                            }
                            else
                            {
                                if (LstLocaldata[info.RtuId] != info.DateUpdate )
                                {
                                    this.SaveRtuToXml(tmp);

                                    LstLocaldata[info.RtuId] = info.DateUpdate ;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteLogError("Add terinfo error:" + ex.ToString());
                        }
                        // Thread.Sleep(1);
                        Wlst.Cr.CoreOne.Services.LogInfo.Log("正在解析数据：   " + _requestedEquipment.Count);
                    }
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(100);
                if (_updated)
                {

                    count++;
                    Thread.Sleep(100);
                    if (count > 2)
                    {
                        Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "参数解析", OperatrType.SystemInfo, "开始");
                        UpdateRequestInThread();
                        break;

                    }
                }
            }
        }

        /// <summary>
        /// 加速数据更新
        /// </summary>
        private void UpdateRequestInThread()
        {


            try
            {
                var lstAdd = new List<Tuple<int, int>>();
                var lstUpdate = new List<Tuple<int, int>>();

                WjParaBase  info = null;
                while (changedInfo.TryDequeue(out info))
                {
                    if (info == null)
                    {
                        continue;
                    }
                    if (LstWantInfo.Contains(info.RtuId)) LstWantInfo.Remove(info.RtuId);

                    if (Info.ContainsKey(info.RtuId))
                    {
                        if (Info[info.RtuId].DateUpdate  == info.DateUpdate ) continue;
                        Info[info.RtuId] = info;
                        lstUpdate.Add(new Tuple<int, int>(info.RtuId, info.RtuFid ));

                        continue;
                    }
                    Info.TryAdd( info.RtuId, info);
                    lstAdd.Add(new Tuple<int, int>(info.RtuId, info.RtuFid ));
                }
                // Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "终端参数加载完成", OperatrType.SystemInfo, "完成");
                UpdateAttachLst();
                if (lstUpdate.Count > 0)
                {
                    var ar = new PublishEventArgs()
                    {
                        EventId = EventIdAssign.EquipmentUpdateEventId,
                        EventType = PublishEventType.Core
                    };
                    ar.AddParams(lstUpdate);
                    EventPublish.PublishEvent(ar);
                }
                if (lstAdd.Count > 0)
                {
                    var ar = new PublishEventArgs()
                    {
                        EventId = EventIdAssign.EquipmentAddEventId,
                        EventType = PublishEventType.Core
                    };
                    ar.AddParams(lstAdd);
                    EventPublish.PublishEvent(ar);
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml core data ,ex:" + ex);
            }
            Version = DateTime.Now.Ticks;

            RequestNextPackageInfo();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RaiseEventHappen(DelayEventHappen.EventOne);
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "参数解析", OperatrType.SystemInfo, "完成");
            Wlst.Sr.PPPandSocketSvr.Server.ProtocolServices.ShieldCmd(false, new List<string>());
        }

        #endregion


        private void UpdateAttachLst()
        {
            //InfoAttachLstDictonary.Clear();
            //foreach (var t in InfoDictionary)
            //{
            //    if (t.Value.AttachRtuId > 0)
            //    {

            //        if (!InfoAttachLstDictonary.ContainsKey(t.Value.AttachRtuId))
            //            InfoAttachLstDictonary.Add(t.Value.AttachRtuId, new List<int>());
            //        if (!InfoAttachLstDictonary[t.Value.AttachRtuId].Contains(t.Key))
            //            InfoAttachLstDictonary[t.Value.AttachRtuId].Add(t.Key);
            //    }
            //}

            var ntg =
                (from t in Info where t.Value.RtuFid > 0 select new Tuple<int, int>(t.Value.RtuFid, t.Value.RtuId)).
                    ToList();
            foreach (var f in Info)
            {
                f.Value.EquipmentsThatAttachToThisRtu.Clear();
            }
            foreach (var f in ntg)
            {
                if (Info.ContainsKey(f.Item1) && !Info[f.Item1].EquipmentsThatAttachToThisRtu.Contains(f.Item2))
                    Info[f.Item1].EquipmentsThatAttachToThisRtu.Add(f.Item2);
            }
        }
    }

    /// <summary>
    /// Socket to Server
    /// </summary>
    public partial class EquipmentDataInfoHold
    {

        #region 请求设备列表信息

        /// <summary>
        /// 与服务器交互数据 请求所有终端列表扼要信息
        /// </summary>
        public void RequestEquipmentInfoLstfromServer()
        {
            var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_request_equ_md5 ;//.ServerPart.wlst_equipment_client_request_EquipentInfoList;
            SndOrderServer.OrderSnd(info, 10, 120);
        }

        protected void RequestDataBaseBriefInfofromServer()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSys.wlst_cnt_request_database_brief_info;//.ServerPart.wlst_clinet_request_database_briefinfo;
            SndOrderServer.OrderSnd(info, 10, 2);
        }

        /// <summary>
        /// 一次请求的终端最多数量
        /// </summary>
        public static int MaxPackageNumber = 99;

        protected void RequestNextPackageInfo()
        {
            if (LstWantInfo.Count < 1) return;
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //LogInfo.Log("正在请求设备信息!!!");
            //var info = new RequestEquipmentInfoLst();

            //foreach (var t in LstWantInfo)
            //{
            //    info.RequestLst.Add(t);
            //    if (info.RequestLst.Count >= MaxPackageNumber) break;
            //}
            //SndOrderServer.OrderSnd(EventIdAssign.EquipmentRequest, null,
            //                        info, waitId, 10, 60, true);
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0, "请求参数-共 " + LstWantInfo.Count, OperatrType.SystemInfo, "请求");

            var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_request_equ ;//.ServerPart.wlst_equipment_client_request_Equipment;
            foreach (var t in LstWantInfo)
            {
                info.WstEquRequest .LstInfo .Add(t);
                if (info.WstEquRequest.LstInfo.Count >= MaxPackageNumber) break;
            }

            SndOrderServer.OrderSnd(info, 20, 60);
        }


        #endregion


        #region 更新主设备信息

        public static void UpdateEquipmentInfo(WjParaBase  terminalInfo)
        {
            MySlef.UpdateMainEquipmentInfo(terminalInfo);
        }

        /// <summary>
        /// 更新终端信息到服务器;参数必须为终端参数 否则出错
        /// </summary>
        /// <param name="terminalInfo"></param>
        public void UpdateMainEquipmentInfo(WjParaBase  terminalInfo)
        {

            if (terminalInfo == null) return;
            var infos = ConvertLoacldatatoSvrdata( terminalInfo);
            if (infos == null)
            {
                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                              terminalInfo.RtuId, terminalInfo.RtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新出错，数据无法解析为服务器数据...");

                return;
            }



 

            var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_update_equ ;//.ServerPart.wlst_equipment_client_update_Equipment;
            info.WstEquUpdate .EquLst .Add(infos);

            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                          terminalInfo.RtuId, terminalInfo.RtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator,
                "终端信息更新");



        }

        #endregion

        #region 增加设备



        /// <summary>
        /// 增加主设备
        /// </summary>
        /// <param name="phyId">主设备物理地址</param>
        /// <param name="mode">设备类型</param>
        public static  long AddMainEquipment(int phyId, int mode,int areaId,int grpid)
        {

            if (mode == 0)
            {
                UMessageBox.Show("设备型号转换出错......,请重新添加", "未知错误", UMessageBoxButton.Ok);
                return 0;
            }

            var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_cnt_request_add_equ;//.ServerPart.wlst_equipment_client_add_Equipment;
        
            info.WstEquCntRequestAdd.EquipmentMode = mode;
            info.WstEquCntRequestAdd.PhyId = phyId;
         //   info.WstCntRequestAddEqu.EquipmentAddedType = 1;
            info.WstEquCntRequestAdd.ThisEquimentAttachRtuId = 0;
            info.WstEquCntRequestAdd.AreaId = areaId;
            info.WstEquCntRequestAdd.GroupId = grpid;
            //  info.Guid = TickCount.EnvironmentTickCount;

            SndOrderServer.OrderSnd(info, 10, 6);
            
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                        phyId, "" + "终端" + phyId, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "增加设备");

            return info.Head.Gid;
        }



        /// <summary>
        /// 增加附属设备
        /// </summary>
        /// <param name="rtuId">主设备地址</param>
        /// <param name="mode">附属设备类型</param>
        public static  void AddAttachEquipment(int rtuId, int mode,int areaId)
        {

            if (mode == 0)
            {
                UMessageBox.Show("设备型号转换出错......,请重新添加", "未知错误", UMessageBoxButton.Ok);
                return;
            }


            //EquipmentInfoAdd info = new EquipmentInfoAdd()
            //                            {
            //                                EquipmentMode = mode,
            //                                PhyId = 1,
            //                                EquipmentAddedType = 2,
            //                                ThisEquimentAttachRtuId = rtuId
            //                            };

            //int waitId = TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(EventIdAssign.EquipmentAddEventId, null,
            //                        info, waitId, 10, 3);

            var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_cnt_request_add_equ ;//.ServerPart.wlst_equipment_client_add_Equipment;
            info.WstEquCntRequestAdd.EquipmentMode = mode;
            info.WstEquCntRequestAdd.PhyId = 1;
           // info.WstCntRequestAddEqu.EquipmentAddedType = 2;
            info.WstEquCntRequestAdd.ThisEquimentAttachRtuId = rtuId;



            SndOrderServer.OrderSnd(info, 10, 6);


            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                rtuId, "" + "终端" + rtuId, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "增加设备");
        }



        #endregion

        #region 删除设备信息

        /// <summary>
        /// 删除附属设备信息
        /// </summary>
        /// <param name="equipmentId">设备地址 可以为主设备也可以为附属设备</param>
        public  static void DeleteEquipment(int equipmentId)
        {
            //var lInfoExchangeforServer = new EqipentInfoDelete();
            //lInfoExchangeforServer.LstInfo.Add(equipmentId);

            //int waitId = TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(EventIdAssign.EquipmentDeleteEventId, null,
            //                        lInfoExchangeforServer, waitId, 10, 3);

            var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_delete_equ ;//.ServerPart.wlst_equipment_client_delete_Equipment;
            info.WstEquDelete  .LstInfo.Add(equipmentId);

            SndOrderServer.OrderSnd(info, 10, 6);



            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                equipmentId, "" + equipmentId, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "删除设备");
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentMentRtuId"></param>
        /// <param name="locationX"></param>
        /// <param name="locationY"></param>
        public static  void OnEquipmentMentMapLocationChangeByMap(int equipmentMentRtuId, double locationX,
                                                          double locationY)
        {
            if (MySlef . Info.ContainsKey(equipmentMentRtuId))
            {
                var t =
                    MySlef.Info[equipmentMentRtuId];
                if (t == null) return;
                t.RtuMapX  = locationX;
                t.RtuMapY = locationY;


                //var infos = EquipmentInfoSvrConvert.ConvetWjInfoToSvrInfo(Info[equipmentMentRtuId]);
                //var tmlInfoExchangeforServer = new EqipentInfoExchange();
                //tmlInfoExchangeforServer.LstInfo.Add(infos);

                //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
                //SndOrderServer.OrderSnd(EventIdAssign.EquipmentUpdateEventId, null,
                //                        tmlInfoExchangeforServer, waitIdUpdate, 10, 6);

                var info = Wlst.Sr.ProtocolPhone.LxEqu .wst_update_map_xy ;//.ServerPart.wlst_equipment_client_update_EquipentxyPositon;
                info.WstEquUpdateMapXy .RtuId = equipmentMentRtuId;
                info.WstEquUpdateMapXy.Xmap = locationX;
                info.WstEquUpdateMapXy.Ymap = locationY;
                // info.Data.LstInfo.Add(EquipmentInfoSvrConvert.ConvetWjInfoToSvrInfo(Info[equipmentMentRtuId]));

                SndOrderServer.OrderSnd(info, 10, 6);



                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    t.RtuId, t.RtuName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新设备地图位置.");
            }
        }






    }
}
