using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowForWlst;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView
{
    /// <summary>
    /// AddTimeTable.xaml 的交互逻辑
    /// </summary>
    public partial class AddTimeTable : CustomChromeWindow
    {
        public AddTimeTable()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "新增或修改时间表";
        }


        private TimeTableInfomationItem dt;
        private int areaid;
        private bool flag = false;

        public void SetContext(TimeTableInfomationItem oit,int area,int tableid)
        {
            dt = oit;
            DataContext = oit;
            areaid = area;
            dt.IsEdit = true;
            if (dt.CurrentSelectLux!=null) dt.ShowCurrentSelectLux2 =22;
        }


        public event EventHandler<EventArgsAddTimeTable> OnFormBtnOkClick;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dt.CurrentSelectLux.Name == null)
            {
                var newlst = new List<int>();
                foreach (var t in dt.LuxCollection)
                {
                    newlst.Add(t.Id);
                }
                foreach (var t in dt.LuxCollection2)
                {
                    if (newlst.Contains(t.Id))
                    {
                        newlst.Remove(t.Id);
                    }
                }
                if (newlst.Count > 0)
                {
                    foreach (var t in dt.LuxCollection)
                    {
                        if (t.Id == newlst.First())
                        {
                            dt.CurrentSelectLux = t;
                            continue;
                        }
                    }
                    
                }
            }


            bool close = false;
            bool close1 = true;

            List<string> msag = new List<string>();
            foreach (var t in Wlst.Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBangdingToThisTimeTablesTmls(areaid,dt.TimeId))
            {
                if (t.Item1 > 999999)
                {
                    var tmp =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Item1];

                    if (tmp.RtuModel == EnumRtuModel.Wj3005 || tmp.RtuModel == EnumRtuModel.Wj3090)
                    {
                        msag.Add("终端 " + t.Item1 + " " +
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Item1].RtuName); }
                    
                }
                else
                {
                    var tu = new Tuple<int, int>(areaid, t.Item1);
                    if (Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                    {
                        var tmp =
                            Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[tu];

                        foreach (var tt in tmp.LstTml)
                        {
                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tt) == false)
                                continue;
                            var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tt];
                            if (info != null)
                            {
                                if (info.RtuModel == EnumRtuModel.Wj3005 || info.RtuModel == EnumRtuModel.Wj3090)
                                {
                                    msag.Add("分组 " + t.Item1 + " " +
                                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[tu].GroupName);break;
                                }
                            }
                        }
                    }
                }
            }
           
            if (dt.MainIsOverOne[0] && msag.Count>0 && Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection == false)
            {
                string msg = "";
                if (msag.Count == 1)
                {
                    msg = msag[0];
                }
                else if (msag.Count == 2)
                {
                    msg = msag[0] + "，" +msag[1];
                }
                else if (msag.Count > 2)
                {
                    msg = msag[0] + "，" + msag[1];
                }
                var information= WlstMessageBox.Show("无法保存", "该时间表为多段开关灯时间表，"+msg+" 不支持多段开关灯功能，请解除绑定后再修改该时间表！", WlstMessageBoxType.Ok);
                close1 = false;
            }


            if (close1)
            {
                var dic = new Dictionary<int, List<int>>();
                int max1 = 1;
                //int LightOpenCloseProtect = 5;
                for (int i = 0; i < 3; i++)
                {
                    if (dt.MainIsOverOne[i]) max1 = max1 + 1;
                }
                if (dt.RuleItems.Count == 7 * max1)
                {

                    var lst = new List<int>(); //光控开，开，光控关，关

                    //var ruleitemslistorder =
                    //    (from t in dt.RuleItems orderby t.DayOfWeekUsed , t.TimeOn select t).ToList();
                    var ruleitemslistorder = (from t in dt.RuleItems orderby t.DayOfWeekUsed, t.TimetableSectionId select t).ToList();
                    var RuleItemsOnce = new ObservableCollection<TimeTableOneDayInfomationItem>();
                    foreach (var f in ruleitemslistorder) RuleItemsOnce.Add(f);


                    foreach (var t in RuleItemsOnce)
                    {

                        lst = new List<int>();

                        if (t.IsUsedLuxOn) lst.Add(t.TimeOn - dt.LuxEffective);
                        else
                        {
                            if (t.TimeOn == 1500) lst.Add(1500);
                            else if (t.TimeOn > 1440)
                                lst.Add(t.TimeOn - 1440);
                            else if (t.TimeOn == 1440)
                                lst.Add(t.TimeOn - 1440 + 1);
                            else lst.Add(t.TimeOn);
                        }

                        if (t.TimeOn == 1500) lst.Add(1500);
                        else if (t.TimeOn > 1440)
                            lst.Add(t.TimeOn - 1440);
                        else if (t.TimeOn == 1440)
                            lst.Add(t.TimeOn - 1440 + 1);
                        else lst.Add(t.TimeOn);

                        if (t.IsUsedLuxOff) lst.Add(t.TimeOff - dt.LuxEffective);
                        else
                        {
                            if (t.TimeOff == 1500) lst.Add(1500);
                            else if (t.TimeOff > 1440)
                                lst.Add(t.TimeOff - 1440);
                            else if (t.TimeOff == 1440)
                                lst.Add(t.TimeOff - 1440 + 1);
                            else lst.Add(t.TimeOff);
                        }

                        if (t.TimeOff == 1500) lst.Add(1500);
                        else if (t.TimeOff > 1440)
                            lst.Add(t.TimeOff - 1440);
                        else if (t.TimeOff == 1440)
                            lst.Add(t.TimeOff - 1440 + 1);
                        else lst.Add(t.TimeOff);

                        if (dic.ContainsKey(t.DayOfWeekUsed))
                        {
                            var lit = new List<int>(dic[t.DayOfWeekUsed]);
                            for (int i = 0; i < lst.Count; i++)
                            {
                                lit.Add(lst[i]);
                            }
                            dic[t.DayOfWeekUsed] = new List<int>(lit);
                        }
                        else
                        {
                            dic.Add(t.DayOfWeekUsed, lst);
                        }
                    }


                    var listall = new List<int>();
                    var listallint = 0;
                    var first = 0;

                    for (int i = 0; i < 7; i++)
                    {
                        var listnow = new List<int>(dic[i]);
                        int weekafter = new int();
                        if (i == 0) weekafter = 6;
                        else weekafter = i - 1;
                        int weekbefore = new int();
                        if (i == 0) weekbefore = 6;
                        else weekbefore = i - 1;
                        var listbefore = new List<int>(dic[weekbefore]);
                        var listafter = new List<int>(dic[weekafter]);

                        var is1500 = 0;
                        listall.Add(0);
                        for (int j = 0; j < listnow.Count; j++)
                        {
                            listall[i] = listall[i] + listnow[j];
                            if (listnow[j] == 1500) is1500 = is1500 + 1;
                            else is1500 = 0;
                        }
                        if (is1500 == 4 || is1500 == 8 || is1500 == 12 || is1500 == 16) { }
                        else is1500 = 0;

                        if (listall[i] / listnow.Count != 1500)
                        {
                            for (int j = 2; j < listnow.Count - 2; j = j + 2)
                            {
                                if (j > 2 && j + is1500 < listnow.Count - 2)
                                {
                                    if (listnow[j - 1] < listnow[j + 1] && listnow[j - 2] < listnow[j + 1] &&
                                        listnow[j - 1] < listnow[j] && listnow[j - 2] < listnow[j])
                                    {
                                    }
                                    else
                                    {
                                        var information = WlstMessageBox.Show
                                            ("无法保存", "存在同一时间段内关灯时间大于开灯时间的情况，请重新修改！", WlstMessageBoxType.Ok);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (listnow[1] != 1500 && listnow[1] < listnow[3] && listnow[0] < listnow[3] &&
                                        listnow[1] < listnow[2] && listnow[0] < listnow[2])
                                    {
                                    }
                                    else if (listnow[1] == 1500 && listbefore[listbefore.Count - 1] < listnow[3] &&
                                                listbefore[listbefore.Count - 2] < listnow[3]
                                                && listbefore[listbefore.Count - 2] < listnow[2] &&
                                                listbefore[listbefore.Count - 1] < listnow[2])
                                    {
                                    }
                                    else
                                    {
                                        var information = WlstMessageBox.Show
                                            ("无法保存", "存在同一时间段内关灯时间大于开灯时间的情况，请重新修改！", WlstMessageBoxType.Ok);
                                        return;
                                    }
                                }

                                if (j == listnow.Count - 4)
                                {
                                    if ((listnow[j + 3] < listnow[j + 1] && listnow[j + 3] < listafter[1] &&
                                            listnow[j + 3] < listafter[0] && listnow[j + 2] < listafter[1] &&
                                            listnow[j + 2] < listafter[0] && listafter[1] != 1500) ||
                                        (listnow[j + 3] > listnow[j + 1] && listnow[j + 3] > listnow[j] &&
                                            listnow[j + 2] > listnow[j + 1] && listnow[j + 2] > listnow[j] && listnow[j + 3] != 1500) ||
                                        (listnow[j + 3] == 1500 && listnow[j + 2] == 1500 && listnow[j + 1] == 1500 && listnow[j] == 1500))
                                    {
                                    }
                                    else
                                    {
                                        var information = WlstMessageBox.Show
                                            ("无法保存", "最后时间段存在关灯时间大于开灯时间的情况，请重新修改！", WlstMessageBoxType.Ok);
                                        return;
                                    }
                                }
                            }

                            for (int j = 2; j < listnow.Count - 4; j = j + 4)
                            {
                                if (listnow[j] > listnow[j + 2] && listnow[j] > listnow[j + 1] && listnow[j - 1] > listnow[j + 1] && listnow[j - 1] > listnow[j + 2])
                                {
                                    var information = WlstMessageBox.Show
                                            ("无法保存", "存在非最后段跨天的情况，请重新修改！", WlstMessageBoxType.Ok);
                                    return;
                                }

                                if (listnow[j + 2] > listnow[j + 4] && listnow[j + 2] > listnow[j + 3] && listnow[j + 1] > listnow[j + 4] && listnow[j + 1] > listnow[j + 3])
                                {
                                    var information = WlstMessageBox.Show
                                           ("无法保存", "存在同一时间段时间顺序设置错误的情况，请重新修改！", WlstMessageBoxType.Ok);
                                    return;
                                }
                            
                            }

                           
                        }
                        else
                        {
                            listallint = listallint + 1;
                        }
                    }
                    if (listall.Count == 7 && listallint == 7)
                    {
                        var information = WlstMessageBox.Show
                                               ("无法保存", "没有设置时间，请重新修改！", WlstMessageBoxType.Ok);
                        return;
                    }

                }

                if (dt.CurrentSelectLux.Id > 0 && dt.LuxChanged == false)
                {
                    var luxareaid =
                        Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(dt.CurrentSelectLux.Id);
                    //var luxareaid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value.Id].AreaId;

                    if (luxareaid != areaid)
                    {
                        string masg = "";
                        if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
                        {
                            masg = "当前区域";
                        }
                        else
                        {
                            foreach (var t in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                            {
                                if (luxareaid != t)
                                    masg = "您所管理的区域";
                                else
                                {
                                    masg = "当前区域";
                                    break;
                                }
                            }
                        }

                        var infoss = WlstMessageBox.Show("警告",
                                                         "该光控 " + dt.CurrentSelectLux.Name + " 不属于" + masg +
                                                         "，是否继续操作？ 是 继续，否 退出.", WlstMessageBoxType.YesNo);
                        if (infoss == WlstMessageBoxResults.No) return;
                    }
                }

                if (dt.CurrentSelectLux.Id <= 0 && dt.CurrentSelectLux2.Id > 0)
                {
                    var infoss = WlstMessageBox.Show("警告",
                                                        "未设置主光控时设置了备用光控！", WlstMessageBoxType.YesNo);
                    return;
                }

                if (dt.CurrentSelectLux.Id <= 0)
                {
                    foreach (var t in dt.RuleItems)
                    {
                        if (t.IsUsedLuxOn == true || t.IsUsedLuxOff == true)
                        {
                            var infoss = WlstMessageBox.Show("警告",
                                                        "设置了光控开关灯，但未选择主光控！", WlstMessageBoxType.YesNo);
                            return;
                        }
                        
                    }
                }


                close = true;

 
            }
            if (close)
            {
                dt.IsEdit = false;
                if (dt.LuxId2 == 0) dt.ShowCurrentSelectLux2 = 0;

                if (OnFormBtnOkClick != null)
                {
                    OnFormBtnOkClick(this, new EventArgsAddTimeTable(dt));
                }
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsAddTimeTable(null));
            }
            this.Close();
        }

        private void Button_Click_LightHelp(object sender, RoutedEventArgs e)
        {
            string luxequipment = null;
            var luxon = 15;
            var luxoff = 15;
            var lightonset = 15;
            var lightoffset = -15;
            var luxtime = 30;

            try
            {
                lightonset =  Convert.ToInt32(Lightonset.Text);
                lightoffset = Convert.ToInt32(Lightoffset.Text);
                luxtime = Convert.ToInt32(Luxtime.Text);
                luxon = Convert.ToInt32(Luxon.Text);
                luxoff = Convert.ToInt32(Luxoff.Text);
            }
            catch (Exception)
            {

            }
            AddTimeTableHelp _addTimeTableHelp = new AddTimeTableHelp();
            _addTimeTableHelp.SetDataContext(luxon,luxoff,luxequipment,lightonset,lightoffset,luxtime);



            _addTimeTableHelp.ShowDialog();


        }

        private void Luxequipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dt.CurrentSelectLux.Id > 0 && dt.LuxChanged == false)
            {
                var luxareaid =
                    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(dt.CurrentSelectLux.Id);
                //var luxareaid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value.Id].AreaId;

                if (luxareaid != areaid)
                {
                    string masg = "";
                    if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
                    {
                        masg = "当前区域";
                    }
                    else
                    {
                        foreach (var t in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                        {
                            if (luxareaid != t)
                                masg = "您所管理的区域";
                            else
                            {
                                masg = "当前区域";
                                break;
                            }
                        }
                    }

                    var infoss = WlstMessageBox.Show("警告",
                                                     "主光控 " + dt.CurrentSelectLux.Name + " 不属于" + masg +
                                                     "，是否继续操作？ 是 继续，否 退出.", WlstMessageBoxType.YesNo);
                    if (infoss != WlstMessageBoxResults.Yes) dt.CurrentSelectLux = dt.LuxCollection[0];
                }

                var luxdelete = dt.CurrentSelectLux;
                if (dt.CurrentSelectLux.Id <= 0)
                {
                    dt.CurrentSelectLux2 = dt.LuxCollection2[0];
                    dt.ShowCurrentSelectLux2 = 0;
                    dt.LuxCollection2.Clear();
                    foreach (var t in dt.LuxCollection)
                    {
                        dt.LuxCollection2.Add(t);
                    }
                    dt.CurrentSelectLux2 = null;
                    dt.LuxId2 = 0;
                    dt.LuxName2 = "";
                }
                else
                {
                    dt.ShowCurrentSelectLux2 = 22;
                    flag = true;
                    dt.LuxCollection2.Clear();
                    foreach (var t in dt.LuxCollection)
                    {
                        dt.LuxCollection2.Add(t);
                    }
                    dt.LuxCollection2.Remove(luxdelete);
                    dt.CurrentSelectLux2 = null;
                    dt.LuxId2 = 0;
                    dt.LuxName2 = "";
                    //foreach (var t in dt.LuxCollection2)
                    //{
                    //    if (t.Id == dt.LuxId2)
                    //    {
                    //        dt.CurrentSelectLux2 = t;
                    //    }
                    //}
                    flag = false;
                }
            }
            else if (dt.CurrentSelectLux.Id <= 0 && dt.LuxChanged == false)
            {
                dt.LuxCollection2.Clear();
                foreach (var t in dt.LuxCollection)
                {
                    dt.LuxCollection2.Add(t);
                }
                dt.CurrentSelectLux2 = null;
                dt.LuxId2 = 0;
                dt.LuxName2 = "";
                flag = false;
            }



        }

        private void Luxequipment2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dt.CurrentSelectLux2.Id > 0 )
            {
                var luxareaid =
                    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(dt.CurrentSelectLux2.Id);
                //var luxareaid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value.Id].AreaId;

                if (flag == false && luxareaid != areaid)
                {
                    string masg = "";
                    if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
                    {
                        masg = "当前区域";
                    }
                    else
                    {
                        foreach (var t in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                        {
                            if (luxareaid != t)
                                masg = "您所管理的区域";
                            else
                            {
                                masg = "当前区域";
                                break;
                            }
                        }
                    }

                    var infoss = WlstMessageBox.Show("警告",
                                                     "备用光控 " + dt.CurrentSelectLux2.Name + " 不属于" + masg +
                                                     "，是否继续操作？ 是 继续，否 退出.", WlstMessageBoxType.YesNo);
                    if (infoss == WlstMessageBoxResults.Yes)
                    {
                        return;
                    }
                    else
                    {
                        if (dt.LuxCollection2.Count > 0)
                        {
                            dt.CurrentSelectLux2 = dt.LuxCollection2[0];
                        }
                    }

                    dt.ShowCurrentSelectLux2 = 22;
                }
            }
            else
            {
                dt.ShowCurrentSelectLux2 = 0;
            }
        }


    }

    public class EventArgsAddTimeTable : EventArgs
    {
        public TimeTableInfomationItem AddTimeTableInfo;

        public EventArgsAddTimeTable(TimeTableInfomationItem info)
        {
            AddTimeTableInfo = info;
        }
    }
}
