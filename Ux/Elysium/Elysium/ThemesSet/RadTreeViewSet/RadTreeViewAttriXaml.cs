using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.RadTreeViewSet
{
    [PublicAPI]
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

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI]
        public static readonly DependencyProperty RadTreeViewBorderThicknessProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBorderThickness", typeof(Thickness), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBorderThickness,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsArrange |
                                                                    FrameworkPropertyMetadataOptions.Inherits,
                                                                    null, ThicknessUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetRadTreeViewBorderThickness([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<Thickness>.Unbox(obj.GetValue(RadTreeViewBorderThicknessProperty));
        }

        [PublicAPI]
        public static void SetRadTreeViewBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadTreeViewBorderThicknessProperty, value);
        }

        #endregion

        #region RadTreeViewBackground

        [PublicAPI]
        public static DependencyProperty RadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetRadTreeViewBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(RadTreeViewBackgroundProperty);
        }

        [PublicAPI]
        public static void SetRadTreeViewBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadTreeViewBackgroundProperty, value);
        }

        #endregion

        #region RadTreeViewBackgroundMouseOver

        [PublicAPI]
        public static DependencyProperty RadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(RadTreeViewBackgroundMouseOverProperty);
        }

        [PublicAPI]
        public static void SetRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region RadTreeViewBorderBrush

        [PublicAPI]
        public static DependencyProperty RadTreeViewBorderBrushProperty =
            DependencyProperty.RegisterAttached("RadTreeViewBorderBrush", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadTreeViewBorderBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetRadTreeViewBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(RadTreeViewBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetRadTreeViewBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadTreeViewBorderBrushProperty, value);
        }

        #endregion  

        #region ItemRadTreeViewBackground

        [PublicAPI]
        public static DependencyProperty ItemRadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemRadTreeViewBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemRadTreeViewBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemRadTreeViewBackgroundProperty, value);
        }

        #endregion

        #region ItemRadTreeViewForeground

        [PublicAPI]
        public static DependencyProperty ItemRadTreeViewForegroundProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewForeground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewForeground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemRadTreeViewForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemRadTreeViewForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemRadTreeViewForegroundProperty, value);
        }

        #endregion

        #region ItemRadTreeViewBackgroundMouseOver

        [PublicAPI]
        public static DependencyProperty ItemRadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewBackgroundMouseOverProperty);
        }

        [PublicAPI]
        public static void SetItemRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemRadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region ItemRadTreeViewForegroundMouseOver

        [PublicAPI]
        public static DependencyProperty ItemRadTreeViewForegroundMouseOverProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewForegroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewForegroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemRadTreeViewForegroundMouseOver([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewForegroundMouseOverProperty);
        }

        [PublicAPI]
        public static void SetItemRadTreeViewForegroundMouseOver([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemRadTreeViewForegroundMouseOverProperty, value);
        }

        #endregion

        #region ItemRadTreeViewBackgroundSelected

        [PublicAPI]
        public static DependencyProperty ItemRadTreeViewBackgroundSelectedProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewBackgroundSelected", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewBackgroundSelected,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemRadTreeViewBackgroundSelected([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewBackgroundSelectedProperty);
        }

        [PublicAPI]
        public static void SetItemRadTreeViewBackgroundSelected([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemRadTreeViewBackgroundSelectedProperty, value);
        }

        #endregion

        #region ItemRadTreeViewForegroundSelected

        [PublicAPI]
        public static DependencyProperty ItemRadTreeViewForegroundSelectedProperty =
            DependencyProperty.RegisterAttached("ItemRadTreeViewForegroundSelected", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemRadTreeViewForegroundSelected,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemRadTreeViewForegroundSelected([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemRadTreeViewForegroundSelectedProperty);
        }

        [PublicAPI]
        public static void SetItemRadTreeViewForegroundSelected([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemRadTreeViewForegroundSelectedProperty, value);
        }

        #endregion

        #region TitleExplandRadTreeViewBackground

        [PublicAPI]
        public static DependencyProperty TitleExplandRadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("TitleExplandRadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleExplandRadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleExplandRadTreeViewBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleExplandRadTreeViewBackgroundProperty);
        }

        [PublicAPI]
        public static void SetTitleExplandRadTreeViewBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleExplandRadTreeViewBackgroundProperty, value);
        }

        #endregion

        #region TitleExplandRadTreeViewBackgroundMouseOver

        [PublicAPI]
        public static DependencyProperty TitleExplandRadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("TitleExplandRadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleExplandRadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleExplandRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleExplandRadTreeViewBackgroundMouseOverProperty);
        }

        [PublicAPI]
        public static void SetTitleExplandRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleExplandRadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region TitleRadTreeViewBackgroundMouseOver

        [PublicAPI]
        public static DependencyProperty TitleRadTreeViewBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("TitleRadTreeViewBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleRadTreeViewBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleRadTreeViewBackgroundMouseOverProperty);
        }

        [PublicAPI]
        public static void SetTitleRadTreeViewBackgroundMouseOver([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleRadTreeViewBackgroundMouseOverProperty, value);
        }

        #endregion

        #region TitleRadTreeViewBackground

        [PublicAPI]
        public static DependencyProperty TitleRadTreeViewBackgroundProperty =
            DependencyProperty.RegisterAttached("TitleRadTreeViewBackground", typeof(SolidColorBrush), typeof(RadTreeViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleRadTreeViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleRadTreeViewBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleRadTreeViewBackgroundProperty);
        }

        [PublicAPI]
        public static void SetTitleRadTreeViewBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleRadTreeViewBackgroundProperty, value);
        }

        #endregion

        #endregion
        #endregion
    }
}
