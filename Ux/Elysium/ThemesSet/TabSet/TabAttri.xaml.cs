﻿using System;
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

namespace Elysium.ThemesSet.TabSet
{
    /// <summary>
    /// TabAttri.xaml 的交互逻辑
    /// </summary>
    public partial class TabAttri : UserControl
    {
        public TabAttri()
        {
            InitializeComponent();
        }

        public TabControl ShowTabControl
        {
            get { return tabControl1 ; }
        }

        public TabItem ShowTabItem1
        {
            get { return tabItem1; }
        }

        public TabItem ShowTabItem2
        {
            get { return tabItem2; }
        }
    }
}
