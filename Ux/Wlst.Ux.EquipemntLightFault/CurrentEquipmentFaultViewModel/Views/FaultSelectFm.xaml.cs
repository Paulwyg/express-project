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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views
{
    /// <summary>
    /// FaultSelectFm.xaml 的交互逻辑
    /// </summary>
    public partial class FaultSelectFm : Window
    {
        public event EventHandler<EventArgsAddUpdate> OnFormBtnOkClick;

        private ObservableCollection<NameIntBool> types = null;

        public ObservableCollection<NameIntBool> Types
        {
            get
            {
                if (types == null)
                {
                    types = new ObservableCollection<NameIntBool>();
                }
                return types;
            }
        }

        public FaultSelectFm()
        {
            InitializeComponent();
            DataContext = this;


        }

        private int Id;
        private int fIndex;
        //private List<int> Selected = new List<int>();

        public void OnLoad(int id, int faltindx, List<int> selectfault, List<int> cantBeselected)
        {
            Id = id;
            fIndex = faltindx;
            //Selected = selectfault;

            Types.Clear();
            if (cantBeselected == null) cantBeselected = new List<int>();
            if (selectfault == null) selectfault = new List<int>();

            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;

                var ff = Sr.EquipemntLightFault.Services.UserDisplayErrorSelfSetInfoHold.GetInfoById(t.Value.FaultId); //二次判断
                if (ff ==null || !ff.IsDisplay) continue;

                Types.Add(new NameIntBool()
                              {
                                  IsSelected = selectfault.Contains(t.Key),
                                  Name =
                                      string.IsNullOrEmpty(t.Value.FaultNameByDefine)
                                          ? t.Value.FaultName
                                          : t.Value.FaultNameByDefine,
                                  Value = t.Key,
                                  IsEnable = cantBeselected.Contains(t.Key) == false
                              });
            }
            tbx.Text = "当前正在设置时间段:" + Id + "  ,报警故障:" + fIndex + " 显示故障.";


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {

                var Name = "";
                foreach (var f in Types)
                {
                    if (f.IsSelected) Name += f.Name + "-";
                }
                if (Name.Length > 1) Name = Name.Substring(0, Name.Length - 1);
                else Name = "无";
                OnFormBtnOkClick(this,
                                 new EventArgsAddUpdate(Id, fIndex,
                                                        (from t in Types where t.IsSelected select t.Value).ToList(),
                                                        Name));
            }
            this.Visibility = Visibility.Collapsed;
        }
    }


    public class EventArgsAddUpdate : EventArgs
    {
        public int Id;
        public int FaultIndex;
        public string Name;
        public List<int> FaultsSelected;

        public EventArgsAddUpdate(int id, int faultIndex, List<int> info,string name)
        {
            Id = id;
            FaultIndex = faultIndex;
            FaultsSelected = info;
            Name = name;
        }
    }
}
