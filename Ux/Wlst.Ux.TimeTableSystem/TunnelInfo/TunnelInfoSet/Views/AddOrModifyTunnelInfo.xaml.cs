using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowForWlst;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Views
{
    /// <summary>
    /// AddOrModifyTunnelInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrModifyTunnelInfo : CustomChromeWindow
    {
        public AddOrModifyTunnelInfo()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "新增或修改隧道方案";
        }

        private TunnelInformation _dt;
        //private bool flag = false;
        private int _areaid1;

        public void SetContext(TunnelInformation oit,int area)
        {
            _dt = oit;
            _areaid1 = area;
            DataContext = oit;
        }

        public event EventHandler<EventArgsAddTunnel> OnFormBtnOkClick;

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    int x = 1;
        //    SelectTerminal _selectTerminal = new SelectTerminal();
        //    _selectTerminal.SetContext(x);
        //    _selectTerminal.ShowDialog();

        //}

        //是否关闭界面
        bool close = false;
        bool close1 = false;

        /// <summary>
        /// 点击确认按钮,将修改后的数据即时加载到前一界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _dt.SubOperationCount = _dt.OperationItems.Count;
            close1 = true;
            if (close1)
            {
                if(_dt.OperationItems.Count==0)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                    "当前方案中未添加操作", WlstMessageBoxType.Ok);
                    return;
                }
                if (_dt.SchemeName.Trim() == string.Empty)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                     "方案名称不能为空", WlstMessageBoxType.Ok);
                    return;
                }
                if (_dt.TunnelName.Trim() == string.Empty)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                     "隧道名称不能为空", WlstMessageBoxType.Ok);
                    return;
                }

                if (_dt.CurrentSelectLux.Id <= 0 && _dt.CurrentSelectLux2.Id > 0)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                     "未设置主光控时设置了备用光控！", WlstMessageBoxType.Ok);
                    return;
                }

                if (_dt.CurrentSelectLux.Id <= 0 && _dt.IsLuxOrTime == 1)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                     "未选择主光控！", WlstMessageBoxType.Ok);
                    return;

                }

                if (_dt.ProtectTime < 100 || _dt.ProtectTime > 500)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                     "保护时间超出范围，应在[100,500]之间！", WlstMessageBoxType.Ok);
                    return;
                }
                foreach (var t in _dt.OperationItems)
                {
                    if (t.OperationName.Trim() == string.Empty)
                    {
                        var infoss = WlstMessageBox.Show("警告",
                                                    "方案名称不能为空", WlstMessageBoxType.Ok);
                        return;
                    }
                    if (t.MaxLux < 0 || t.MaxLux > 10000)
                    {
                        var infoss = WlstMessageBox.Show("警告",
                                                         "最大光控值超出范围，应在[0,10000]之间！", WlstMessageBoxType.Ok);
                        return;
                    }

                 
                }
                if (_dt.IsLuxOrTime == 1)
                {
                    bool flag = true; //假设不重复   
                    for (int i = 0; i < _dt.OperationItems.Count - 1; i++)
                    {
                        //循环开始元素   
                        for (int j = i + 1; j < _dt.OperationItems.Count; j++)
                        {
                            //循环后续所有元素   
                            //如果相等，则重复   
                            if (_dt.OperationItems[i].MaxLux == _dt.OperationItems[j].MaxLux)
                            {
                                flag = false; //设置标志变量为重复   
                                break; //结束循环   
                            }
                        }
                    }
                    if (flag == false)
                    {
                        var infoss = WlstMessageBox.Show("警告",
                                                         "最大光控值出现重复！", WlstMessageBoxType.Ok);
                        return;
                    }
                }
                if (_dt.IsLuxOrTime == 2)
                {
                    bool flag1 = true; //假设不重复   
                    for (int i = 0; i < _dt.OperationItems.Count - 1; i++)
                    {
                        //循环开始元素   
                        for (int j = i + 1; j < _dt.OperationItems.Count; j++)
                        {
                            //循环后续所有元素   
                            //如果相等，则重复   
                            if (_dt.OperationItems[i].LastTimeHour == _dt.OperationItems[j].LastTimeHour &&
                                _dt.OperationItems[i].LastTimeMinute == _dt.OperationItems[j].LastTimeMinute)
                            {
                                flag1 = false; //设置标志变量为重复   
                                break; //结束循环   
                            }
                        }
                    }
                    if (flag1 == false)
                    {
                        var infoss = WlstMessageBox.Show("警告",
                                                         "最大操作时间出现重复！", WlstMessageBoxType.Ok);
                        return;
                    }
                }
                close = true;
            }
            if (close)
            {

                if (OnFormBtnOkClick != null)
                {
                    OnFormBtnOkClick(this, new EventArgsAddTunnel(_dt));
                }
                this.Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsAddTunnel(null));
            }
            this.Close();
        }


        private void Luxequipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var f = _dt.CurrentSelectedOperationItem;


            if (_dt.CurrentSelectLux.Id > 0 && _dt.LuxChanged == false)
                {
                    //var luxareaid =
                    //    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(
                    //        f.CurrentSelectLux.Id);
                    ////var luxareaid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value.Id].AreaId;

                    //if (luxareaid != _areaid1)
                    //{
                    //    string masg = "";
                    //    if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
                    //    {
                    //        masg = "当前区域";
                    //    }
                    //    else
                    //    {
                    //        foreach (var t in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                    //        {
                    //            if (luxareaid != t)
                    //                masg = "您所管理的区域";
                    //            else
                    //            {
                    //                masg = "当前区域";
                    //                break;
                    //            }
                    //        }
                    //    }

                    //    var infoss = WlstMessageBox.Show("警告",
                    //                                     "主光控 " + f.CurrentSelectLux.Name + " 不属于" + masg +
                    //                                     "，是否继续操作？ 是 继续，否 退出.", WlstMessageBoxType.YesNo);
                    //    if (infoss != WlstMessageBoxResults.Yes) f.CurrentSelectLux = f.LuxCollection[0];
                    //}

                //    var luxdelete = f.CurrentSelectLux;
                //    if (f.CurrentSelectLux.Id <= 0)
                //    {
                //        f.CurrentSelectLux2 = f.LuxCollection2[0];
                //        f.LuxCollection2.Clear();
                //        foreach (var t in f.LuxCollection)
                //        {
                //            f.LuxCollection2.Add(t);
                //        }
                //        f.CurrentSelectLux2 = null;
                //        f.LuxId2 = 0;
                //        f.LuxName2 = "";
                //    }
                //    else
                //    {
                //        flag = true;
                //        f.LuxCollection2.Clear();
                //        foreach (var t in f.LuxCollection)
                //        {
                //            f.LuxCollection2.Add(t);
                //        }
                //        f.LuxCollection2.Remove(luxdelete);
                //        //f.CurrentSelectLux2 = null ;
                //        //f.LuxId2 = 0 ;
                //        //f.LuxName2 =  "" ;
                //        bool flg = true;
                //        foreach (var t in f.LuxCollection2)
                //        {
                //            if (t.Id == f.LuxId2)
                //            {
                //                flg = false;
                //            }
                //        }
                //        if(flg)
                //        {
                //            f.CurrentSelectLux2 = null;
                //            f.LuxId2 = 0;
                //            f.LuxName2 = "";
                //        }

                //        flag = false;
                //    }
                }
                //else if (f.CurrentSelectLux.Id <= 0 && f.LuxChanged == false)
                //{
                //    f.LuxCollection2.Clear();
                //    foreach (var t in f.LuxCollection)
                //    {
                //        f.LuxCollection2.Add(t);
                //    }
                //    f.CurrentSelectLux2 =  null ;
                //    f.LuxId2 =  0 ;
                //    f.LuxName2 =  "" ;
                //    flag = false;
                //}

            

        }

        private void Luxequipment2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

                //var f = _dt.CurrentSelectedOperationItem;

              
                //if (f.CurrentSelectLux2.Id > 0)
                //{
                //    var luxareaid =
                //        Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(
                //            f.CurrentSelectLux2.Id);
                //    //var luxareaid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value.Id].AreaId;

                //    if (flag == false && luxareaid != _areaid1)
                //    {
                //        string masg = "";
                //        if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
                //        {
                //            masg = "当前区域";
                //        }
                //        else
                //        {
                //            foreach (var t in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                //            {
                //                if (luxareaid != t)
                //                    masg = "您所管理的区域";
                //                else
                //                {
                //                    masg = "当前区域";
                //                    break;
                //                }
                //            }
                //        }

                //        var infoss = WlstMessageBox.Show("警告",
                //                                         "备用光控 " + f.CurrentSelectLux2.Name + " 不属于" + masg +
                //                                         "，是否继续操作？ 是 继续，否 退出.", WlstMessageBoxType.YesNo);
                //        if (infoss == WlstMessageBoxResults.Yes)
                //        {
                //            return;
                //        }
                //        else
                //        {
                //            if (f.LuxCollection2.Count > 0)
                //            {
                //                f.CurrentSelectLux2 = f.LuxCollection2[0];
                //            }
                //        }

                //    }
                //}
            
        }


        public class EventArgsAddTunnel : EventArgs
        {
            public TunnelInformation AddTunnelInfo;

            public EventArgsAddTunnel(TunnelInformation info)
            {
                AddTunnelInfo = info;
            }
        }

        private Dictionary<int, string> dic = new Dictionary<int, string>();
        private bool _currentSelectAllStateTmp = false;

        //双击列头全选或全清
        private void GridViewDataColumn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (dic.Count == 0)
            {
                for (int i = 1; i < 32; i++)
                {
                    if (dic.ContainsKey(i) == false) dic.Add(i, "K" + i);
                }
            }


            var rx = e.OriginalSource as TextBlock;
            if (rx == null) return;

            var para = rx.Parent as Telerik.Windows.Controls.GridView.GridViewHeaderCell;
            if (para == null) return;
            var con = para.ToString();
            if (con == null) return;
            var keys = (from t in dic where con.Contains(t.Value) select t.Key).ToList();
            if (keys.Count == 0) return;
            int soutid = keys.Max();
            if (soutid == 0) return;
            _currentSelectAllStateTmp = !_currentSelectAllStateTmp;
            switch (soutid)
            {
                case 1:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[0].IsEnabledOn)
                            t.Items[0].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 2:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[1].IsEnabledOn)
                            t.Items[1].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 3:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[2].IsEnabledOn)
                            t.Items[2].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 4:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[3].IsEnabledOn)
                            t.Items[3].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 5:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[4].IsEnabledOn)
                            t.Items[4].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 6:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[5].IsEnabledOn)
                            t.Items[5].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 7:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[6].IsEnabledOn)
                            t.Items[6].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
                case 8:
                    foreach (var t in _dt.CurrentSelectedOperationItem.SelectedItems)
                    {
                        if (t.IsEnable && t.Items[7].IsEnabledOn)
                            t.Items[7].IsCheckSwitch = _currentSelectAllStateTmp;
                    }
                    break;
            }
        }

    }
}
