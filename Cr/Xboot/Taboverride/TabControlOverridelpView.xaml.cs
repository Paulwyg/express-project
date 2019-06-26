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

namespace Xboot.Taboverride
{
    /// <summary>
    /// TabControlOverridelpView.xaml 的交互逻辑
    /// </summary>
    public partial class TabControlOverridelpView : UserControl
    {
        public TabControlOverridelpView()
        {
            InitializeComponent();
            TabControlr.OnItemsChangeded += new EventHandler(TabControlr_OnItemsChangeded);
            //TabControlr.SelectionChanged += new SelectionChangedEventHandler(TabControlr_SelectionChanged);
        }

        //private void TabControlr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (sender != null)
        //    {
        //        var tb = sender as TabControl;
        //        if (tb == null) return;
        //        int ind = tb.SelectedIndex;
        //        if (ind == 0) return;

        //    }
        //    //throw new NotImplementedException();
        //}

        private bool running = false;


        private void TabControlr_OnItemsChangeded(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (running) return;
            running = true;

            var tmp = new List<object>();
            var ls = new List<Tuple<int, object>>();
            if (TabControlr.Items.Count == 0)
            {
                running = false;
                return;
            }

            foreach (var f in TabControlr.Items)
            {
                var g = f as ContentControl;
                if (g == null)
                {
                    tmp.Add(f);
                    continue;
                }

                var its = g.DataContext as Wlst.Cr.Core.CoreInterface.IITab;
                if (its == null)
                {
                    tmp.Add(f);
                    continue;
                }
                ls.Add(new Tuple<int, object>(its.Index, f));
               // ls.Add(new Tuple<int, object>(0, f));
                //if (its.Title.Contains("终端"))
                //{
                //    ls.Add(new Tuple<int, object>(0, f));
                //}
                //else if (its.Title.Contains("线路"))
                //{
                //    ls.Add(new Tuple<int, object>(1, f));
                //}
                //else
                //    ls.Add(new Tuple<int, object>(2, f));
                // ls.Add(new Tuple<int, object>(its.TabIndex, f));

            }

            var ntg = (from t in ls orderby t.Item1 ascending select t.Item2).ToList();
            TabControlr.Items.Clear();


            foreach (var f in ntg)
            {
                TabControlr.Items.Add(f);
            }
            foreach (var f in tmp)
            {
                TabControlr.Items.Add(f);
            }
            Wlst.Cr.Coreb.AsyncTask .Qtz .AddQtz("0", 999, DateTime.Now.AddSeconds(1).Ticks, RunInUiThread, 0, null, 1);


 
            //TabControlr.SelectedIndex = 0;
            running = false;
        }


        //定义委托
        private delegate void DoTask();

        private void RunInUiThread(object obj)
        {
            Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal, new DoTask(As));

        }

        private void As()
        {
            TabControlr.SelectedIndex = 0;
        }

    }
}
