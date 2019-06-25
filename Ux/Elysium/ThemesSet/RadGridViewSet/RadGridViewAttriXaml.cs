using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.RadGridViewSet
{
    [PublicAPI]
    public static class RadGridViewAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "RadGridViewAttriXaml";

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
        public static Thickness RadGridViewBorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("RadGridViewBorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["RadGridViewBorderThickness"]);
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
                    if (Info.ContainsKey("RadGridViewBorderThickness")) Info["RadGridViewBorderThickness"] = value.Left.ToString();
                    else Info.Add("RadGridViewBorderThickness", value.Left.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush RadGridViewBackground
        {
            get
            {
                if (Info.ContainsKey("RadGridViewBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["RadGridViewBackground"]);
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
                    if (Info.ContainsKey("RadGridViewBackground")) Info["RadGridViewBackground"] = value.Color.ToString();
                    else Info.Add("RadGridViewBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush RadGridViewBorderBrush
        {
            get
            {
                if (Info.ContainsKey("RadGridViewBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["RadGridViewBorderBrush"]);
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
                    if (Info.ContainsKey("RadGridViewBorderBrush")) Info["RadGridViewBorderBrush"] = value.Color.ToString();
                    else Info.Add("RadGridViewBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush RadGridViewForeground
        {
            get
            {
                if (Info.ContainsKey("RadGridViewForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["RadGridViewForeground"]);
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
                    if (Info.ContainsKey("RadGridViewForeground")) Info["RadGridViewForeground"] = value.Color.ToString();
                    else Info.Add("RadGridViewForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush SortIndicatorAscBrush
        {
            get
            {
                if (Info.ContainsKey("SortIndicatorAscBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["SortIndicatorAscBrush"]);
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
                    if (Info.ContainsKey("SortIndicatorAscBrush")) Info["SortIndicatorAscBrush"] = value.Color.ToString();
                    else Info.Add("SortIndicatorAscBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush SortIndicatorDesBrush
        {
            get
            {
                if (Info.ContainsKey("SortIndicatorDesBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["SortIndicatorDesBrush"]);
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
                    if (Info.ContainsKey("SortIndicatorDesBrush")) Info["SortIndicatorDesBrush"] = value.Color.ToString();
                    else Info.Add("SortIndicatorDesBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush TitleNormalBackground
        {
            get
            {
                if (Info.ContainsKey("TitleNormalBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleNormalBackground"]);
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
                    if (Info.ContainsKey("TitleNormalBackground")) Info["TitleNormalBackground"] = value.Color.ToString();
                    else Info.Add("TitleNormalBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush TitleCellSelectedBrush
        {
            get
            {
                if (Info.ContainsKey("TitleCellSelectedBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleCellSelectedBrush"]);
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
                    if (Info.ContainsKey("TitleCellSelectedBrush")) Info["TitleCellSelectedBrush"] = value.Color.ToString();
                    else Info.Add("TitleCellSelectedBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleCellMouseOverBrush
        {
            get
            {
                if (Info.ContainsKey("TitleCellMouseOverBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleCellMouseOverBrush"]);
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
                    if (Info.ContainsKey("TitleCellMouseOverBrush")) Info["TitleCellMouseOverBrush"] = value.Color.ToString();
                    else Info.Add("TitleCellMouseOverBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleNormalForeground
        {
            get
            {
                if (Info.ContainsKey("TitleNormalForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleNormalForeground"]);
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
                    if (Info.ContainsKey("TitleNormalForeground")) Info["TitleNormalForeground"] = value.Color.ToString();
                    else Info.Add("TitleNormalForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush TitleNormalForegroundSelected
        {
            get
            {
                if (Info.ContainsKey("TitleNormalForegroundSelected"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["TitleNormalForegroundSelected"]);
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
                    if (Info.ContainsKey("TitleNormalForegroundSelected")) Info["TitleNormalForegroundSelected"] = value.Color.ToString();
                    else Info.Add("TitleNormalForegroundSelected", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        
        public static SolidColorBrush ItemBackground
        {
            get
            {
                if (Info.ContainsKey("ItemBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemBackground"]);
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
                    if (Info.ContainsKey("ItemBackground")) Info["ItemBackground"] = value.Color.ToString();
                    else Info.Add("ItemBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemBackgroundMouseOver
        {
            get
            {
                if (Info.ContainsKey("ItemBackgroundMouseOver"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemBackgroundMouseOver"]);
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
                    if (Info.ContainsKey("ItemBackgroundMouseOver")) Info["ItemBackgroundMouseOver"] = value.Color.ToString();
                    else Info.Add("ItemBackgroundMouseOver", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush ItemBackgroundSelected
        {
            get
            {
                if (Info.ContainsKey("ItemBackgroundSelected"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemBackgroundSelected"]);
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
                    if (Info.ContainsKey("ItemBackgroundSelected")) Info["ItemBackgroundSelected"] = value.Color.ToString();
                    else Info.Add("ItemBackgroundSelected", value.Color.ToString());
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


        #region RadGridViewBorderThickness

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI]
        public static readonly DependencyProperty RadGridViewBorderThicknessProperty =
            DependencyProperty.RegisterAttached("RadGridViewBorderThickness", typeof(Thickness), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadGridViewBorderThickness,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsArrange |
                                                                    FrameworkPropertyMetadataOptions.Inherits,
                                                                    null, ThicknessUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetRadGridViewBorderThickness([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<Thickness>.Unbox(obj.GetValue(RadGridViewBorderThicknessProperty));
        }

        [PublicAPI]
        public static void SetRadGridViewBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadGridViewBorderThicknessProperty, value);
        }

        #endregion

        #region RadGridViewBackground

        [PublicAPI]
        public static DependencyProperty RadGridViewBackgroundProperty =
            DependencyProperty.RegisterAttached("RadGridViewBackground", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadGridViewBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetRadGridViewBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(RadGridViewBackgroundProperty);
        }

        [PublicAPI]
        public static void SetRadGridViewBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadGridViewBackgroundProperty, value);
        }

        #endregion

        #region RadGridViewBorderBrush

        [PublicAPI]
        public static DependencyProperty RadGridViewBorderBrushProperty =
            DependencyProperty.RegisterAttached("RadGridViewBorderBrush", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadGridViewBorderBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetRadGridViewBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(RadGridViewBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetRadGridViewBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadGridViewBorderBrushProperty, value);
        }

        #endregion

        #region RadGridViewForeground

        [PublicAPI]
        public static DependencyProperty RadGridViewForegroundProperty =
            DependencyProperty.RegisterAttached("RadGridViewForeground", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadGridViewForeground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetRadGridViewForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(RadGridViewForegroundProperty);
        }

        [PublicAPI]
        public static void SetRadGridViewForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RadGridViewForegroundProperty, value);
        }

        #endregion

        #region SortIndicatorAscBrush

        [PublicAPI]
        public static DependencyProperty SortIndicatorAscBrushProperty =
            DependencyProperty.RegisterAttached("SortIndicatorAscBrush", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(SortIndicatorAscBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetSortIndicatorAscBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(SortIndicatorAscBrushProperty);
        }

        [PublicAPI]
        public static void SetSortIndicatorAscBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(SortIndicatorAscBrushProperty, value);
        }

        #endregion

        #region SortIndicatorDesBrush

        [PublicAPI]
        public static DependencyProperty SortIndicatorDesBrushProperty =
            DependencyProperty.RegisterAttached("SortIndicatorDesBrush", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(RadGridViewForeground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetSortIndicatorDesBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(SortIndicatorDesBrushProperty);
        }

        [PublicAPI]
        public static void SetSortIndicatorDesBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(SortIndicatorDesBrushProperty, value);
        }

        #endregion

        #region TitleNormalBackground

        [PublicAPI]
        public static DependencyProperty TitleNormalBackgroundProperty =
            DependencyProperty.RegisterAttached("TitleNormalBackground", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleNormalBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleNormalBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleNormalBackgroundProperty);
        }

        [PublicAPI]
        public static void SetTitleNormalBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleNormalBackgroundProperty, value);
        }

        #endregion

        #region TitleCellSelectedBrush

        [PublicAPI]
        public static DependencyProperty TitleCellSelectedBrushProperty =
            DependencyProperty.RegisterAttached("TitleCellSelectedBrush", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleCellSelectedBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleCellSelectedBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleCellSelectedBrushProperty);
        }

        [PublicAPI]
        public static void SetTitleCellSelectedBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleCellSelectedBrushProperty, value);
        }

        #endregion

        #region TitleCellMouseOverBrush

        [PublicAPI]
        public static DependencyProperty TitleCellMouseOverBrushProperty =
            DependencyProperty.RegisterAttached("TitleCellMouseOverBrush", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleCellMouseOverBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleCellMouseOverBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleCellMouseOverBrushProperty);
        }

        [PublicAPI]
        public static void SetTitleCellMouseOverBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleCellMouseOverBrushProperty, value);
        }

    #endregion
        #region TitleNormalForeground

        [PublicAPI]
        public static DependencyProperty TitleNormalForegroundProperty =
            DependencyProperty.RegisterAttached("TitleNormalForeground", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleNormalForeground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleNormalForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleNormalForegroundProperty);
        }

        [PublicAPI]
        public static void SetTitleNormalForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleNormalForegroundProperty, value);
        }

    #endregion
        #region TitleNormalForegroundSelected

        [PublicAPI]
        public static DependencyProperty TitleNormalForegroundSelectedProperty =
            DependencyProperty.RegisterAttached("TitleNormalForegroundSelected", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(TitleNormalForegroundSelected,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetTitleNormalForegroundSelected([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(TitleNormalForegroundSelectedProperty);
        }

        [PublicAPI]
        public static void SetTitleNormalForegroundSelected([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TitleNormalForegroundSelectedProperty, value);
        }

    #endregion



        #region ItemBackground

        [PublicAPI]
        public static DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemBackground", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemBackground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemBackgroundProperty, value);
        }

        #endregion
        #region ItemBackgroundMouseOver

        [PublicAPI]
        public static DependencyProperty ItemBackgroundMouseOverProperty =
            DependencyProperty.RegisterAttached("ItemBackgroundMouseOver", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemBackgroundMouseOver,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemBackgroundMouseOver([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemBackgroundMouseOverProperty);
        }

        [PublicAPI]
        public static void SetItemBackgroundMouseOver([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemBackgroundMouseOverProperty, value);
        }

        #endregion
        #region ItemBackgroundSelected

        [PublicAPI]
        public static DependencyProperty ItemBackgroundSelectedProperty =
            DependencyProperty.RegisterAttached("ItemBackgroundSelected", typeof(SolidColorBrush), typeof(RadGridViewAttriXaml),
                                    new FrameworkPropertyMetadata(ItemBackgroundSelected,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemBackgroundSelected([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemBackgroundSelectedProperty);
        }

        [PublicAPI]
        public static void SetItemBackgroundSelected([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemBackgroundSelectedProperty, value);
        }

        #endregion
        
            
        #endregion
        #endregion
    }
}
