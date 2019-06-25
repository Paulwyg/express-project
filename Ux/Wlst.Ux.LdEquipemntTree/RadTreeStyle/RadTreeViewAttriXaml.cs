using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Wlst.Ux.EquipemntTree.RadTreeStyle
{
    public static class RadTreeViewAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "RadTreeViewAttriXaml";

        private static Dictionary<string, string> _infoinfo;

        private static Dictionary<string, string> Info
        {
            get
            {
                if (_infoinfo == null)
                {
                    _infoinfo = new Dictionary<string, string>();
                    var tmp = ReadSave.Read(ScrollSetXmlFileName);
                    foreach (var t in tmp)
                    {
                        if (!_infoinfo.ContainsKey(t.Key))
                            _infoinfo.Add(t.Key, t.Value);
                    }
                }
                return _infoinfo;
            }
        }

        /// <summary>
        /// 保存设置的样式
        /// </summary>
        public static void SaveStyle()
        {
            ReadSave.Save(Info, ScrollSetXmlFileName);
        }

        #region 背景色 等 static

        #region  内容面板及其边框

        public static Thickness RadTreeViewBorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("RadTreeViewBorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["RadTreeViewBorderThickness"]);
                        return new Thickness(convertFromString);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new Thickness(1d);

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RadTreeViewBorderThickness")) Info["RadTreeViewBorderThickness"] = value.Left.ToString();
                    else Info.Add("RadTreeViewBorderThickness", value.Left.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush RadTreeViewBackground
        {
            get
            {
                if (Info.ContainsKey("RadTreeViewBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["RadTreeViewBackground"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0x0, 0xFF, 0xFF, 0xFF));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RadTreeViewBackground")) Info["RadTreeViewBackground"] = value.Color.ToString();
                    else Info.Add("RadTreeViewBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush RadTreeViewBackgroundMouseOver
        {
            get
            {
                if (Info.ContainsKey("RadTreeViewBackgroundMouseOver"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["RadTreeViewBackgroundMouseOver"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RadTreeViewBackgroundMouseOver")) Info["RadTreeViewBackgroundMouseOver"] = value.Color.ToString();
                    else Info.Add("RadTreeViewBackgroundMouseOver", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush RadTreeViewBorderBrush
        {
            get
            {
                if (Info.ContainsKey("RadTreeViewBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["RadTreeViewBorderBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RadTreeViewBorderBrush")) Info["RadTreeViewBorderBrush"] = value.Color.ToString();
                    else Info.Add("RadTreeViewBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
                

        public static SolidColorBrush ItemRadTreeViewBackground
        {
            get
            {
                if (Info.ContainsKey("ItemRadTreeViewBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemRadTreeViewBackground"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ItemRadTreeViewBackground")) Info["ItemRadTreeViewBackground"] = value.Color.ToString();
                    else Info.Add("ItemRadTreeViewBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemRadTreeViewForeground
        {
            get
            {
                if (Info.ContainsKey("ItemRadTreeViewForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemRadTreeViewForeground"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ItemRadTreeViewForeground")) Info["ItemRadTreeViewForeground"] = value.Color.ToString();
                    else Info.Add("ItemRadTreeViewForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemRadTreeViewBackgroundMouseOver
        {
            get
            {
                if (Info.ContainsKey("ItemRadTreeViewBackgroundMouseOver"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemRadTreeViewBackgroundMouseOver"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ItemRadTreeViewBackgroundMouseOver")) Info["ItemRadTreeViewBackgroundMouseOver"] = value.Color.ToString();
                    else Info.Add("ItemRadTreeViewBackgroundMouseOver", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemRadTreeViewForegroundMouseOver
        {
            get
            {
                if (Info.ContainsKey("ItemRadTreeViewForegroundMouseOver"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemRadTreeViewForegroundMouseOver"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ItemRadTreeViewForegroundMouseOver")) Info["ItemRadTreeViewForegroundMouseOver"] = value.Color.ToString();
                    else Info.Add("ItemRadTreeViewForegroundMouseOver", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemRadTreeViewBackgroundSelected
        {
            get
            {
                if (Info.ContainsKey("ItemRadTreeViewBackgroundSelected"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemRadTreeViewBackgroundSelected"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ItemRadTreeViewBackgroundSelected")) Info["ItemRadTreeViewBackgroundSelected"] = value.Color.ToString();
                    else Info.Add("ItemRadTreeViewBackgroundSelected", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemRadTreeViewForegroundSelected
        {
            get
            {
                if (Info.ContainsKey("ItemRadTreeViewForegroundSelected"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemRadTreeViewForegroundSelected"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ItemRadTreeViewForegroundSelected")) Info["ItemRadTreeViewForegroundSelected"] = value.Color.ToString();
                    else Info.Add("ItemRadTreeViewForegroundSelected", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleExplandRadTreeViewBackground
        {
            get
            {
                if (Info.ContainsKey("TitleExplandRadTreeViewBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleExplandRadTreeViewBackground"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("TitleExplandRadTreeViewBackground")) Info["TitleExplandRadTreeViewBackground"] = value.Color.ToString();
                    else Info.Add("TitleExplandRadTreeViewBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleExplandRadTreeViewBackgroundMouseOver
        {
            get
            {
                if (Info.ContainsKey("TitleExplandRadTreeViewBackgroundMouseOver"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleExplandRadTreeViewBackgroundMouseOver"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("TitleExplandRadTreeViewBackgroundMouseOver")) Info["TitleExplandRadTreeViewBackgroundMouseOver"] = value.Color.ToString();
                    else Info.Add("TitleExplandRadTreeViewBackgroundMouseOver", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleRadTreeViewBackgroundMouseOver
        {
            get
            {
                if (Info.ContainsKey("TitleRadTreeViewBackgroundMouseOver"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleRadTreeViewBackgroundMouseOver"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("TitleRadTreeViewBackgroundMouseOver")) Info["TitleRadTreeViewBackgroundMouseOver"] = value.Color.ToString();
                    else Info.Add("TitleRadTreeViewBackgroundMouseOver", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleRadTreeViewBackground
        {
            get
            {
                if (Info.ContainsKey("TitleRadTreeViewBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleRadTreeViewBackground"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("TitleRadTreeViewBackground")) Info["TitleRadTreeViewBackground"] = value.Color.ToString();
                    else Info.Add("TitleRadTreeViewBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #endregion

        #region Attach Propory

        #region 内容面板及其边框


        #region RadTreeViewBorderThickness

        public static readonly DependencyProperty RadTreeViewBorderThicknessProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBorderThickness", typeof(Thickness), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBorderThickness,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsArrange |
                                                                    FrameworkPropertyMetadataOptions.Inherits,
                                                                    null));

        public static Thickness GetRadTreeViewBorderThickness( DependencyObject obj)
        {
            return (Thickness)obj.GetValue(RadTreeViewBorderThicknessProperty);
        }

        public static void SetRadTreeViewBorderThickness( DependencyObject obj, Thickness value)
        {
            obj.SetValue(RadTreeViewBorderThicknessProperty, value);
        }

        #endregion

        #region RadTreeViewBackground

        public static DependencyProperty RadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetRadTreeViewBackground( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(RadTreeViewBackgroundProperty);
        }

        public static void SetRadTreeViewBackground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(RadTreeViewBackgroundProperty, value);
        }

        #endregion

        #region RadTreeViewBackgroundMouseOver

        public static DependencyProperty RadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetRadTreeViewBackgroundMouseOver( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(RadTreeViewBackgroundMouseOverProperty);
        }

        public static void SetRadTreeViewBackgroundMouseOver(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(RadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region RadTreeViewBorderBrush
        public static DependencyProperty RadTreeViewBorderBrushProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBorderBrush", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBorderBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetRadTreeViewBorderBrush(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(RadTreeViewBorderBrushProperty);
        }

        public static void SetRadTreeViewBorderBrush(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(RadTreeViewBorderBrushProperty, value);
        }

        #endregion  

        #region ItemRadTreeViewBackground

        public static DependencyProperty ItemRadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetItemRadTreeViewBackground(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewBackgroundProperty);
        }

        public static void SetItemRadTreeViewBackground( DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ItemRadTreeViewBackgroundProperty, value);
        }

        #endregion

        #region ItemRadTreeViewForeground

        public static DependencyProperty ItemRadTreeViewForegroundProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewForeground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewForeground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetItemRadTreeViewForeground( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewForegroundProperty);
        }

        public static void SetItemRadTreeViewForeground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ItemRadTreeViewForegroundProperty, value);
        }

        #endregion

        #region ItemRadTreeViewBackgroundMouseOver

        public static DependencyProperty ItemRadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetItemRadTreeViewBackgroundMouseOver(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewBackgroundMouseOverProperty);
        }

        public static void SetItemRadTreeViewBackgroundMouseOver(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ItemRadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region ItemRadTreeViewForegroundMouseOver

        public static DependencyProperty ItemRadTreeViewForegroundMouseOverProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewForegroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewForegroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetItemRadTreeViewForegroundMouseOver( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewForegroundMouseOverProperty);
        }

        public static void SetItemRadTreeViewForegroundMouseOver( DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ItemRadTreeViewForegroundMouseOverProperty, value);
        }

        #endregion

        #region ItemRadTreeViewBackgroundSelected

        public static DependencyProperty ItemRadTreeViewBackgroundSelectedProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewBackgroundSelected", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewBackgroundSelected,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetItemRadTreeViewBackgroundSelected( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewBackgroundSelectedProperty);
        }

        public static void SetItemRadTreeViewBackgroundSelected(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ItemRadTreeViewBackgroundSelectedProperty, value);
        }

        #endregion

        #region ItemRadTreeViewForegroundSelected

        public static DependencyProperty ItemRadTreeViewForegroundSelectedProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewForegroundSelected", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewForegroundSelected,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetItemRadTreeViewForegroundSelected( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewForegroundSelectedProperty);
        }

        public static void SetItemRadTreeViewForegroundSelected(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(ItemRadTreeViewForegroundSelectedProperty, value);
        }

        #endregion

        #region TitleExplandRadTreeViewBackground

        public static DependencyProperty TitleExplandRadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("TitleExplandRadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleExplandRadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetTitleExplandRadTreeViewBackground(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(TitleExplandRadTreeViewBackgroundProperty);
        }
        public static void SetTitleExplandRadTreeViewBackground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(TitleExplandRadTreeViewBackgroundProperty, value);
        }

        #endregion

        #region TitleExplandRadTreeViewBackgroundMouseOver

        public static DependencyProperty TitleExplandRadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("TitleExplandRadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleExplandRadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetTitleExplandRadTreeViewBackgroundMouseOver( DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(TitleExplandRadTreeViewBackgroundMouseOverProperty);
        }

        public static void SetTitleExplandRadTreeViewBackgroundMouseOver(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(TitleExplandRadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region TitleRadTreeViewBackgroundMouseOver

        public static DependencyProperty TitleRadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("TitleRadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleRadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetTitleRadTreeViewBackgroundMouseOver(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(TitleRadTreeViewBackgroundMouseOverProperty);
        }
        public static void SetTitleRadTreeViewBackgroundMouseOver( DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(TitleRadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region TitleRadTreeViewBackground
        public static DependencyProperty TitleRadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("TitleRadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleRadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        public static SolidColorBrush GetTitleRadTreeViewBackground(DependencyObject obj)
        {
            return (SolidColorBrush)obj.GetValue(TitleRadTreeViewBackgroundProperty);
        }

        public static void SetTitleRadTreeViewBackground(DependencyObject obj, SolidColorBrush value)
        {
            obj.SetValue(TitleRadTreeViewBackgroundProperty, value);
        }

        #endregion

        #endregion
        #endregion
    }

    public class ReadSave
    {
        public static void Save(Dictionary<string, string> info, string xmlFileName)
        {
            try
            {
                if (!xmlFileName.EndsWith(".xml"))
                {
                    xmlFileName += ".xml";
                }

                string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                try
                {
                    XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                    writer.WriteStartDocument(); //XML声明
                    writer.WriteStartElement("Root"); //书写根元素
                    foreach (var t in info)
                    {
                        if (string.IsNullOrEmpty(t.Key) || string.IsNullOrEmpty(t.Value)) continue;

                        writer.WriteStartElement("elysium"); //开始一个元素
                        writer.WriteAttributeString("key", t.Key); //向先前创建的元素中添加一个属性
                        writer.WriteAttributeString("value", t.Value); //向先前创建的元素中添加一个属性
                        writer.WriteEndElement(); // 关闭元素
                    }
                    //在节点间添加一些空

                    writer.Close();

                }
                catch
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }

        }


        public static Dictionary<string, string> Read(string xmlFileName)
        {
            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (!File.Exists(path)) return info;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNode root = xmlDoc.SelectSingleNode("Root");
                if (root != null)
                {
                    var nodelist = root.ChildNodes;

                    foreach (XmlNode nodeType in nodelist)
                    {
                        XmlElement element = (XmlElement)nodeType;
                        if (element != null)
                        {
                            try
                            {
                                string key = element.GetAttribute("key");
                                string value = element.GetAttribute("value");
                                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
                                {
                                    info.Add(key, value);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
                //                       ", nodeName :" + nodename + "; Ex:" + ex);
            }
            return info;
        }
    }
}
