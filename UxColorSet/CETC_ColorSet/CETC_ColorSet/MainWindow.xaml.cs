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
using Elysium.ThemesSet.AppleWindowSet;
using Elysium.ThemesSet.ButtonSet;
using Elysium.ThemesSet.CheckBoxRadioButtonSet;
using Elysium.ThemesSet.ComboBoxListBoxSet;
using Elysium.ThemesSet.DatePickerSet;
using Elysium.ThemesSet.FontSet;
using Elysium.ThemesSet.GroupBoxSet;
using Elysium.ThemesSet.LabelSet;
using Elysium.ThemesSet.ListViewSet;
using Elysium.ThemesSet.MenuSet;
using Elysium.ThemesSet.MessageBoxOverrideSet;
using Elysium.ThemesSet.RadGridViewSet;
using Elysium.ThemesSet.RadTreeViewSet;
using Elysium.ThemesSet.ScrollSet;
using Elysium.ThemesSet.ScrollViewSet;
using Elysium.ThemesSet.TabSet;
using Elysium.ThemesSet.TextBoxsSet;
using Elysium.ThemesSet.TreeViewSet;

namespace CETC_ColorSet
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);

            butts.DataContext = new ButtonAttriDataContext(butts.ShowButton);

            radion.DataContext = new CheckBoxRadioButtonAttriDataContext(radion.ShowCheckBox, radion.ShowRadioButton);

            textbox.DataContext = new TextBoxsAttriDataContext(textbox.ShowTextBox);

            tab.DataContext = new TabAttriDataContext(tab.ShowTabControl, tab.ShowTabItem1, tab.ShowTabItem2);

            ComboxListBoxtab.DataContext = new ComboBoxListBoxAttriDataContext(ComboxListBoxtab.ShowComboBox,
                                                                               ComboxListBoxtab.ShowListBox);
            Scroll.DataContext = new ScrollAttriDataContext(Scroll.ShowButton);
            ScrollView.DataContext = new ScrollViewerAttriDataContext(ScrollView.ShowView);

            menu.DataContext = new MenuAttriDataContext(menu.ShowMenu);

            listview.DataContext = new ListViewAttriDataContext(listview.ShowView, listview.ShowViewdisable);

            font.DataContext = new FontAttriDataContext(font.showFont);
            treeview.DataContext = new TreeViewAttriDataContext(treeview.ShowView);

            datePickerView.DataContext = new DatePickerAttriDataContext(datePickerView.ShowView);

            radGridView.DataContext = new RadGridViewAttriDataContext(radGridView.ShowView);

            RadTreeView.DataContext = new RadTreeViewAttriDataContext(RadTreeView.ShowView);
            GroupBox.DataContext = new GroupBoxAttriDataContext(GroupBox.ShowTextBox);
            AppleWindow.DataContext = new AppleWindowAttriDataContext();
            MyLabel.DataContext = new LabelAttriDataContext();
            MessageBoxOverride.DataContext = new MessageBoxOverrideAttriDataContext();
        }
    }
}
