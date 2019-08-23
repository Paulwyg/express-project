using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel
{
    /// <summary>
    /// 界面终端选择  类型选择
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {
        /// <summary>
        /// 全部则返回 空，多个则填写  包括终端下的附属设备
        /// </summary>
        /// <returns></returns>
        private List<int> GetSelectedRtuLst()
        {
            if (IsAdvancedQueryChecked == false) return new List<int>();
            var rtn = new List<int>();
            if (IsSingleEquipmentQuery) //单个终端查询 IsSingleEquipmentQuery
            {
                if (SelectedRtus.Count == 0)
                {
                    UMessageBox.Show("提醒", "未选择终端！", UMessageBoxButton.Ok);
                    return null;
                }
                rtn.AddRange(SelectedRtus);
            }

            else
            {
                //if (AreaId == -1)
                //{

                if (RtuTypeSelected.Value == -1)
                    return rtn; //空 全部 区域
                else
                {
                    rtn = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.MySlef.Info.Keys.ToList();
                }

                //}
                //else if (GrpId == -1) //整个区域的设备  
                //{
                //    rtn = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                //    if (rtn.Count == 0)
                //    {
                //        Remind = "请选择设备.";
                //        return null;
                //    }
                //    // return rtn;
                //}
                //else
                //{
                //    //一个分组下的设备 
                //    rtn = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                //             AreaId, GrpId);
                //    if (rtn.Count == 0)
                //    {
                //        Remind = "请选择设备.";
                //        return null;
                //    }
                //    // return rtn;
                //}
            }


            //如果 终端类型 需要判断 lvf 2018年12月11日13:48:19
            if (RtuTypeSelected.Value != -1)
            {
                var reme = new List<int>();
                foreach (var f in rtn)
                {
                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                            ContainsKey(f)) continue;

                    var tt =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f]
                            as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tt == null || tt.WjVoltage == null) continue;
                    if (tt.WjVoltage.RtuUsedType != RtuTypeSelected.Value) continue;
                    reme.Add(f);
                }
                if (reme.Count == 0)
                {
                    Remind = "类型筛选后无设备...";
                    return null;
                }
                return reme;

            }

            return rtn;
        }
    }

    /// <summary>
    /// 打印及打印预览
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {

        //wyg  打印预览
        #region CmdPrintPriview
        private ICommand _cmdPrintPriview;
        public ICommand CmdPrintPriview
        {
            get
            {
                if (_cmdPrintPriview == null)
                    _cmdPrintPriview = new RelayCommand(ExCmdPrintPriview, CanExPrintPriview, false);
                return _cmdPrintPriview;
            }
        }

        private void ExCmdPrintPriview()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("地址");
                tabletitle.Add("名称");
                tabletitle.Add("故障回路");
                tabletitle.Add("故障名称");
                tabletitle.Add("发生时间");
                if (IsOldFaultQuery) tabletitle.Add("消除时间");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in Records)
                {
                    createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuLoopName);
                    tem.Add(g.FaultName);
                    tem.Add(createtime.ToString("MM/dd HH:mm:ss"));
                    if (IsOldFaultQuery)
                    {
                        removetime = Convert.ToDateTime(g.DtRemoceTime, dtFormat);
                        tem.Add(removetime.ToString("MM/dd HH:mm:ss"));
                    }
                    tem.Add(g.Remark);
                    table.Add(tem);
                }
                print.Prints.PrintPriview(tabletitle, table, false, "故障统计表", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrintPriview()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }


        #endregion

        //打印
        #region CmdPrint
        private ICommand _cmdPrint;
        public ICommand CmdPrint
        {
            get
            {
                if (_cmdPrint == null)
                    _cmdPrint = new RelayCommand(ExCmdPrint, CanExPrint, false);
                return _cmdPrint;
            }
        }

        private void ExCmdPrint()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("地址");
                tabletitle.Add("名称");
                tabletitle.Add("故障回路");
                tabletitle.Add("故障名称");
                tabletitle.Add("发生时间");
                if (IsOldFaultQuery) tabletitle.Add("消除时间");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in Records)
                {
                    createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuLoopName);
                    tem.Add(g.FaultName);
                    tem.Add(createtime.ToString("MM/dd HH:mm:ss"));
                    if (IsOldFaultQuery)
                    {
                        removetime = Convert.ToDateTime(g.DtRemoceTime, dtFormat);
                        tem.Add(removetime.ToString("MM/dd HH:mm:ss"));
                    }
                    tem.Add(g.Remark);
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, false, "故障统计表", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }
        #endregion
    }



}
