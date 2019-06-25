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

namespace Wlst.Ux.Setting.SettingViewNew.baseview
{
    /// <summary>
    /// OptionView.xaml 的交互逻辑
    /// </summary>
    public partial class OptionView : UserControl
    {
        public OptionView()
        {
            InitializeComponent();

            DataContext = this;
        }

        public string _backGroundView = "Blue";
        public string _backGroundOther = "Red";

        public string BackGroundView
        {
            get { return _backGroundView; }
            set
            {
                _backGroundView = value;
                BackGroundOfList.BackGroundView = value;
                foreach (var f in Items)
                {
                    f.BackGroundView = value;
                }
            }

        }


        public string BackGroundOther
        {
            get { return _backGroundOther; }
            set
            {
                _backGroundOther = value;
                BackGroundOfList.BackGround = value;
                foreach (var f in Items)
                {
                    f.BackGround = value;

                }
            }
        }

        private int idx = 1;

        public void AddItem(string name, ContentControl view)
        {
            Items.Add(new OptionViewItem()
            {
                BackGround = BackGroundOther,
                BackGroundView = BackGroundView,
                Id = idx++,
                Content = view,
                Name = name

            });
            load = false;
        }



        private bool load = false;

        public void PrintVisualTree(DependencyObject obj)
        {

            if (obj is ContentControl)
            {
                var ns = obj as ContentControl;
                var xr = ns.DataContext as OptionViewItem;
                if (xr != null)
                {
                    int x = 0;
                    x += 1;
                    Console.WriteLine(x);

                    var pt = ns.TransformToAncestor(s1).Transform(new Point(0, 0));
                    xr.Point = pt;

                    load = true;
                }
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                PrintVisualTree(VisualTreeHelper.GetChild(obj, i));
            }
        }





        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (load == false)
                PrintVisualTree(itemsControl1);

            if (Items.Count < 1) return;

            foreach (var f in Items)
            {
                f.PointTmp = f.Content.TransformToAncestor(OptView).Transform(new Point(0, 0));
            }

            int selectid = 0;
            if (Items.Count == 1)
            {
                selectid = Items[0].Id;
            }
            else
            {
                var nts = (from t in Items orderby t.PointTmp.Y ascending select t).ToList();
                for (int i = 0; i < nts.Count - 1; i++)
                {
                    if (nts[i].PointTmp.Y <= 0 && nts[i + 1].PointTmp.Y > 0)
                    {
                        selectid = nts[i].Id;
                        break;
                    }
                }
            }

            SetSelect(selectid);

        }

        void SetSelect(int id)
        {
            foreach (var f in Items)
            {
                if (f.Id == id)
                {
                    f.BackGround = BackGroundView;
                }
                else
                {
                    f.BackGround = BackGroundOther;
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var snd = sender as Button;
            if (snd == null) return;
            var dx = snd.DataContext as OptionViewItem;
            if (dx == null) return;

            s1.ScrollToVerticalOffset(dx.Point.Y);

            SetSelect(dx.Id);
        }




        private ObservableCollection<OptionViewItem> items;

        public ObservableCollection<OptionViewItem> Items
        {
            get
            {
                if (items == null) items = new ObservableCollection<OptionViewItem>();
                return items;
            }
        }

        private OptionViewItem _backGround;
        public OptionViewItem BackGroundOfList
        {
            get
            {
                if (_backGround == null)
                {
                    _backGround = new OptionViewItem()
                    {
                        BackGround = BackGroundOther,
                        BackGroundView = BackGroundView
                    };
                }
                return _backGround;
            }
        }
    }
}
