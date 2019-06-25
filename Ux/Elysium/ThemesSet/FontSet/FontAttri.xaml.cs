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

namespace Elysium.ThemesSet.FontSet
{
    /// <summary>
    /// FontAttri.xaml 的交互逻辑
    /// </summary>
    public partial class FontAttri : UserControl
    {
       List<string> FontWeightSet=new List<string>();
        List<string> FontStyleSet=new List<string>();
        List<string> FontStretchSet= new List<string>();
        List<string> FontFamilySet=new List<string>();
        public FontAttri()
        {
            InitializeComponent();
            #region FontWeight
            FontWeightSet.Add("Black");
            FontWeightSet.Add("Bold");
            FontWeightSet.Add("DemiBold");
            FontWeightSet.Add("ExtraBlack");
            FontWeightSet.Add("ExtraBold");
            FontWeightSet.Add("ExtraLight");
            FontWeightSet.Add("Heavy");
            FontWeightSet.Add("Light");
            FontWeightSet.Add("Medium");
            FontWeightSet.Add("Normal");
            FontWeightSet.Add("Regular");
            FontWeightSet.Add("SemiBold");
            FontWeightSet.Add("Thin");
            FontWeightSet.Add("UltraBlack");
            FontWeightSet.Add("UltraBold");
            FontWeightSet.Add("UltraLight");

            foreach (var item in FontWeightSet)
            {
                comboBox1.Items.Add(item);
            }
            #endregion

            #region FontStyle
            FontStyleSet.Add(FontStyles.Italic.ToString());
            FontStyleSet.Add(FontStyles.Normal.ToString());
            FontStyleSet.Add(FontStyles.Oblique.ToString());

            foreach (var item in FontStyleSet)
            {
                fontstyle.Items.Add(item);
            }
            #endregion

            #region FontStretchSet
            FontStretchSet.Add(FontStretches.Condensed.ToString());
            FontStretchSet.Add(FontStretches.Expanded.ToString());
            FontStretchSet.Add(FontStretches.ExtraCondensed.ToString());
            FontStretchSet.Add(FontStretches.ExtraExpanded.ToString());
            FontStretchSet.Add(FontStretches.Medium.ToString());
            FontStretchSet.Add(FontStretches.Normal.ToString());
            FontStretchSet.Add(FontStretches.SemiCondensed.ToString());
            FontStretchSet.Add(FontStretches.SemiExpanded.ToString());
            FontStretchSet.Add(FontStretches.UltraCondensed.ToString());
            FontStretchSet.Add(FontStretches.UltraExpanded.ToString());

            foreach (var item in FontStretchSet)
            {
                fontstretch.Items.Add(item);
            }
            #endregion

            #region FontFamilySet
            FontFamilySet.Add("Arial");
            FontFamilySet.Add("Times New Roman");
            FontFamilySet.Add("宋体");
            foreach (var item in FontFamilySet)
            {
                fontfamily.Items.Add(item);
            }
            #endregion


        }
        public Button showFont
        {
            get { return button1; }
        }

        
    }
}
