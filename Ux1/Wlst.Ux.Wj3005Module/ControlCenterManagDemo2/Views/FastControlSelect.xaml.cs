using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism;
using WindowForWlst;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Views
{
    /// <summary>
    /// FastControlSelect.xaml 的交互逻辑
    /// </summary>
    public partial class FastControlSelect :  CustomChromeWindow 
    {
        public FastControlSelect(Tuple<List<Tuple<int, int>>, List<string>> NowSelected)
        {
            //this.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;


            //var hwnd = new WindowInteropHelper(this).Handle;
            //SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            Groups.Clear();
           
            areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo .AreaX )
            {
                if (areas.Contains(f) == false && f != -1) areas.Add(f);
            }
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false && f != -1) areas.Add(f);
                }
            }

            if (areas.Count > 1)
            {
                AreaView.Add(new NameIntBool()
                             {
                                 IsShow = true
                             });
            }
            else
            {
                AreaView.Add(new NameIntBool()
                                 {
                                     IsShow = false
                                 });
            }
            
            foreach (var t in areas)
            {
                var grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(t);
                var area = "未知";
                if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                {
                    area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                }
                foreach (var tt in grps)
                {
                     Groups.Add(new GroupsItem()
                               {
                                   AreaId = t,
                                   AreaName = tt.AreaId + "-" + area,
                                   Id = tt.GroupId,
                                   Name = tt.GroupName,
                                   IsChecked = false
                               });
                }

                Groups.Add(new GroupsItem()
                {
                    AreaId = t,
                    AreaName = t + "-" + area,
                    Id = -1,
                    Name = "特殊终端",
                    IsChecked = false
                });

            }

            var lststring = LoadXmldata();
            foreach (var t in lststring)
            {
                Types.Add(new NameIntBool()
                              {
                                  IsSelected = false,
                                  Name = t,
                                  IsEnable = true
                              });
            }

            if (NowSelected != new Tuple<List<Tuple<int, int>>, List<string>>(new List<Tuple<int, int>>(), new List<string>()))
            {
                foreach (var t in Groups)
                {
                    if (NowSelected.Item1.Contains(new Tuple<int, int>(t.AreaId,t.Id)))
                    {
                        t.IsChecked = true;
                    }
                }
                foreach (var t in Types)
                {
                    if (NowSelected.Item2.Contains(t.Name))
                    {
                        t.IsSelected = true;
                    }
                }
            }

            group.ItemsSource = Groups;
            lstbox.ItemsSource = Types;
        }

        //private const int GWL_STYLE = -16;
        //private const int WS_SYSMENU = 0x80000;
        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        //[DllImport("user32.dll")]
        //private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        

        public event EventHandler<EventArgsFrmFastSelectTmlList> OnFormBtnOkClick;
        private List<int> areas = new List<int>();

        private List<string> LoadXmldata() //crc
        {
            List<string> x = new List<string>();
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("FastSearchConfig");
            foreach (var t in info)
            {
                x.Add(t.Value);
            }
            return x;
        }

        private void WriteXmldata(Dictionary<string, string> info) //crc
        {
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info,"FastSearchConfig");
        }

        private ObservableCollection<NameIntBool> _types = null;
        public ObservableCollection<NameIntBool> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new ObservableCollection<NameIntBool>();
                }
                return _types;
            }
            
        }

        private ObservableCollection<GroupsItem> _groups = null;
        public ObservableCollection<GroupsItem> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = new ObservableCollection<GroupsItem>();
                }
                return _groups;
            }

        }


        private ObservableCollection<NameIntBool> _areaview = null;

        public ObservableCollection<NameIntBool> AreaView
        {
            get
            {
                if (_areaview == null)
                {
                    _areaview = new ObservableCollection<NameIntBool>();
                }
                return _areaview;
            }
        }

        public class GroupsItem : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private bool _ischecked;

            public bool IsChecked
            {
                get { return _ischecked; }
                set
                {
                    if (_ischecked != value)
                    {
                        _ischecked = value;
                        this.RaisePropertyChanged(() => this.IsChecked);
                    }
                }
            }

            private int _id;

            public int Id
            {
                get { return _id; }
                set
                {
                    if (value != _id)
                    {
                        _id = value;
                        this.RaisePropertyChanged(() => this.Id);
                    }
                }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != _name)
                    {
                        _name = value;
                        this.RaisePropertyChanged(() => this.Name);
                    }
                }
            }

            private string _area;

            public string AreaName
            {
                get { return _area; }
                set
                {
                    if (value != _area)
                    {
                        _area = value;
                        this.RaisePropertyChanged(() => this.AreaName);
                    }
                }
            }

            private int _areaid;

            public int AreaId
            {
                get { return _areaid; }
                set
                {
                    if (value != _areaid)
                    {
                        _areaid = value;
                        this.RaisePropertyChanged(() => this.AreaId);
                    }
                }
            }

        }


        public class EventArgsFrmFastSelectTmlList : EventArgs
        {
            public Tuple<List <int>,List<Tuple<int,int>>,List<string>> Info;
            public EventArgsFrmFastSelectTmlList(Tuple<List<int>, List<Tuple<int, int>>, List<string>> tmp)
            {
                Info = tmp;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Types.Count>8)
            {
                UMessageBox.Show("提示", "关键词数量已达上限！", UMessageBoxButton.Ok);
                return;
            }
            else
            {
                var type = new NameIntBool()
                               {
                                   IsSelected = false,
                                   Name = txt.Text,
                                   IsEnable = true
                               };
                foreach (var t in Types)
                {
                    if (t.Name == type.Name)
                    {
                        UMessageBox.Show("提示", "已有该关键词！", UMessageBoxButton.Ok);
                        return;
                    }
                }
                
                Types.Add(type);
                

            }
            lstbox.ItemsSource = Types;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var t in Types)
            {
                if (t.Name == txt.Text)
                {
                    Types.Remove(t);
                    lstbox.ItemsSource = Types;
                    return;
                }
            }
            UMessageBox.Show("提示", "没有该关键词，请重新输入！", UMessageBoxButton.Ok);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var dic = new Dictionary<string, string>();
            var list = new List< int>();
            int index = 1;
            foreach (var t in Types)
            {
                dic.Add(index.ToString(""),t.Name);
                index++;
            }
            WriteXmldata(dic);

            var lstgrp = new List<Tuple<int,int>>();
            var lstnogrp = new List<int>();

            foreach (var i in Groups)
            {
                if (i.IsChecked)
                {
                    if (i.Id != -1)
                    {
                        lstgrp.Add(new Tuple<int, int>(i.AreaId, i.Id));
                    }
                    else
                    {
                        var lsttt =
                            Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(i.AreaId);
                        foreach (var i1 in lsttt)
                        {
                            var tu = i1;
                            lstnogrp.Add(tu);
                        }
                    }
                }
            }
            var lsttype = new List<string>();
            foreach (var i in Types)
            {
                if (i.IsSelected)
                {
                    lsttype.Add(i.Name);
                }
            }

            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.RtuModel == EnumRtuModel.Wj3005 || t.Value.RtuModel == EnumRtuModel.Wj3006 || t.Value.RtuModel == EnumRtuModel.Wj4005)
                {
                    foreach (var tt in lsttype)
                    {
                        if (t.Value.RtuName.Contains(tt))
                        {
                            var tu =
                                Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(t.Value.RtuId);
                            if (tu != null)
                            {
                                if (lstgrp.Contains(tu))
                                {
                                    list.Add(t.Value.RtuId);
                                }
                            }
                            else
                            {
                                if (lstnogrp.Contains(t.Value.RtuId))
                                {
                                    list.Add(t.Value.RtuId);
                                }
                            }
                        }
                    }
                }
            }

            if (list.Count == 0)
            {
                UMessageBox.Show("提示", "没有筛选出终端！", UMessageBoxButton.Ok);
                return;
            }

            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsFrmFastSelectTmlList(new Tuple<List<int>, List<Tuple<int, int>>, List<string>>(list,lstgrp,lsttype)));
            }
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsFrmFastSelectTmlList(null));
            }
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Groups.Count > 0)
            {
                if (Groups.First().IsChecked)
                {
                    foreach (var t in Groups)
                    {
                        t.IsChecked = false;
                    }
                }
                else
                {
                    foreach (var t in Groups)
                    {
                        t.IsChecked = true;
                    }
                }
            }
            group.ItemsSource = Groups;
        }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //} 


    }
}
