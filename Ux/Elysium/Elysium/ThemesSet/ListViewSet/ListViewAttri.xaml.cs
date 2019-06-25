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

namespace Elysium.ThemesSet.ListViewSet
{
    /// <summary>
    /// ListViewAttri.xaml 的交互逻辑
    /// </summary>
    public partial class ListViewAttri : UserControl
    {
        public ListViewAttri()
        {
            InitializeComponent();
            List<Person> ListPersons=new List<Person>();
            ListPersons.Add(new Person(){UserId ="1",UserName ="msl",MobilePhone ="1523333",Address ="shanghai"});
            ListPersons.Add(new Person() { UserId = "2", UserName = "yanan", MobilePhone = "1333", Address = "shanghai" });
            ListPersons.Add(new Person() { UserId = "3", UserName = "msl", MobilePhone = "3333", Address = "shanghai" });
            ListPersons.Add(new Person() { UserId = "4", UserName = "yanan", MobilePhone = "3333", Address = "shanghai" });
            this.showview.ItemsSource = ListPersons;
            showviewdisable.ItemsSource = ListPersons;
        }
        public ListView ShowView
        {
            get { return showview; }
        }

        public ListView ShowViewdisable
        {
            get { return showviewdisable; }
        }
    }

    public class Person
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string MobilePhone { get; set; }
        public string Address { get; set; }

    }
}
