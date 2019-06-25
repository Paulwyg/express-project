using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.DataGridSet
{
    [PublicAPI]
    public static class DataGridAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "DataGridAttriXaml";

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
        public static Thickness BorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("BorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["BorderThickness"]);
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
                    if (Info.ContainsKey("BorderThickness")) Info["BorderThickness"] = value.Left.ToString();
                    else Info.Add("BorderThickness", value.Left.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        public static SolidColorBrush NormalBackgrounBrush
        {
            get
            {
                if (Info.ContainsKey("NormalBackgrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalBackgrounBrush"]);
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
                    if (Info.ContainsKey("NormalBackgrounBrush")) Info["NormalBackgrounBrush"] = value.Color.ToString();
                    else Info.Add("NormalBackgrounBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush NormalBorderBrush
        {
            get
            {
                if (Info.ContainsKey("NormalBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalBorderBrush"]);
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
                    if (Info.ContainsKey("NormalBorderBrush")) Info["NormalBorderBrush"] = value.Color.ToString();
                    else Info.Add("NormalBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush DisableBackgrounBrush
        {
            get
            {
                if (Info.ContainsKey("DisableBackgrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DisableBackgrounBrush"]);
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
                    if (Info.ContainsKey("DisableBackgrounBrush")) Info["DisableBackgrounBrush"] = value.Color.ToString();
                    else Info.Add("DisableBackgrounBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush DisableBorderBrush
        {
            get
            {
                if (Info.ContainsKey("DisableBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DisableBorderBrush"]);
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
                    if (Info.ContainsKey("DisableBorderBrush")) Info["DisableBorderBrush"] = value.Color.ToString();
                    else Info.Add("DisableBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region Item项
            #region ItemNormalBackground
            public static SolidColorBrush ItemNormalBackground
            {
                get
                {
                    if (Info.ContainsKey("ItemNormalBackground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["ItemNormalBackground"]);
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
                        if (Info.ContainsKey("ItemNormalBackground")) Info["ItemNormalBackground"] = value.Color.ToString();
                        else Info.Add("ItemNormalBackground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

            #region ItemNormalForeground
        public static SolidColorBrush ItemNormalForeground
        {
            get
            {
                if (Info.ContainsKey("ItemNormalForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemNormalForeground"]);
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
                    if (Info.ContainsKey("ItemNormalForeground")) Info["ItemNormalForeground"] = value.Color.ToString();
                    else Info.Add("ItemNormalForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
            #endregion
        
            #region ItemNormalBorderBrush
        public static SolidColorBrush ItemNormalBorderBrush
        {
            get
            {
                if (Info.ContainsKey("ItemNormalBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemNormalBorderBrush"]);
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
                    if (Info.ContainsKey("ItemNormalBorderBrush")) Info["ItemNormalBorderBrush"] = value.Color.ToString();
                    else Info.Add("ItemNormalBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
            #endregion

            #region ItemBorderThickness
        public static Thickness ItemBorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("ItemBorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["ItemBorderThickness"]);
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
                    if (Info.ContainsKey("ItemBorderThickness")) Info["ItemBorderThickness"] = value.Left.ToString();
                    else Info.Add("ItemBorderThickness", value.Left.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
            #endregion

            #region ItemMouseOverBackground
            public static SolidColorBrush ItemMouseOverBackground
            {
                get
                {
                    if (Info.ContainsKey("ItemMouseOverBackground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["ItemMouseOverBackground"]);
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
                        if (Info.ContainsKey("ItemMouseOverBackground")) Info["ItemMouseOverBackground"] = value.Color.ToString();
                        else Info.Add("ItemMouseOverBackground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

            #region ItemMouseOverForeground
            public static SolidColorBrush ItemMouseOverForeground
            {
                get
                {
                    if (Info.ContainsKey("ItemMouseOverForeground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["ItemMouseOverForeground"]);
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
                        if (Info.ContainsKey("ItemMouseOverForeground")) Info["ItemMouseOverForeground"] = value.Color.ToString();
                        else Info.Add("ItemMouseOverForeground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

            #region ItemSelectedBackground
            public static SolidColorBrush ItemSelectedBackground
            {
                get
                {
                    if (Info.ContainsKey("ItemSelectedBackground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["ItemSelectedBackground"]);
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
                        if (Info.ContainsKey("ItemSelectedBackground")) Info["ItemSelectedBackground"] = value.Color.ToString();
                        else Info.Add("ItemSelectedBackground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

            #region ItemSelectedForeground
            public static SolidColorBrush ItemSelectedForeground
            {
                get
                {
                    if (Info.ContainsKey("ItemSelectedForeground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["ItemSelectedForeground"]);
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
                        if (Info.ContainsKey("ItemSelectedForeground")) Info["ItemSelectedForeground"] = value.Color.ToString();
                        else Info.Add("ItemSelectedForeground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

        #endregion

        #region Header项
        #region HeaderNormalBackground
            public static SolidColorBrush HeaderNormalBackground
            {
                get
                {
                    if (Info.ContainsKey("HeaderNormalBackground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderNormalBackground"]);
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
                        if (Info.ContainsKey("HeaderNormalBackground")) Info["HeaderNormalBackground"] = value.Color.ToString();
                        else Info.Add("HeaderNormalBackground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        #endregion
        #region HeaderNormalForeground
            public static SolidColorBrush HeaderNormalForeground
            {
                get
                {
                    if (Info.ContainsKey("HeaderNormalForeground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderNormalForeground"]);
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
                        if (Info.ContainsKey("HeaderNormalForeground")) Info["HeaderNormalForeground"] = value.Color.ToString();
                        else Info.Add("HeaderNormalForeground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        #endregion
        #region HeaderMouseOverBackground
            public static SolidColorBrush HeaderMouseOverBackground
            {
                get
                {
                    if (Info.ContainsKey("HeaderMouseOverBackground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderMouseOverBackground"]);
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
                        if (Info.ContainsKey("HeaderMouseOverBackground")) Info["HeaderMouseOverBackground"] = value.Color.ToString();
                        else Info.Add("HeaderMouseOverBackground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        #endregion
        #region HeaderMouseOverForeground
            public static SolidColorBrush HeaderMouseOverForeground
            {
                get
                {
                    if (Info.ContainsKey("HeaderMouseOverForeground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderMouseOverForeground"]);
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
                        if (Info.ContainsKey("HeaderMouseOverForeground")) Info["HeaderMouseOverForeground"] = value.Color.ToString();
                        else Info.Add("HeaderMouseOverForeground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        #endregion
        #region HeaderPressedBackground
            public static SolidColorBrush HeaderPressedBackground
            {
                get
                {
                    if (Info.ContainsKey("HeaderPressedBackground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderPressedBackground"]);
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
                        if (Info.ContainsKey("HeaderPressedBackground")) Info["HeaderPressedBackground"] = value.Color.ToString();
                        else Info.Add("HeaderPressedBackground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        #endregion
        #region HeaderPressedForeground
            public static SolidColorBrush HeaderPressedForeground
            {
                get
                {
                    if (Info.ContainsKey("HeaderPressedForeground"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderPressedForeground"]);
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
                        if (Info.ContainsKey("HeaderPressedForeground")) Info["HeaderPressedForeground"] = value.Color.ToString();
                        else Info.Add("HeaderPressedForeground", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        #endregion
        #endregion

        #region 表头分隔符

            #region HeaderSeparatedColor
            public static SolidColorBrush HeaderSeparatedColor
            {
                get
                {
                    if (Info.ContainsKey("HeaderSeparatedColor"))
                    {
                        try
                        {
                            var convertFromString = ColorConverter.ConvertFromString(Info["HeaderSeparatedColor"]);
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
                        if (Info.ContainsKey("HeaderSeparatedColor")) Info["HeaderSeparatedColor"] = value.Color.ToString();
                        else Info.Add("HeaderSeparatedColor", value.Color.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

            #region HeaderSeparatedWidth
            public static double HeaderSeparatedWidth
            {
                get
                {
                    if (Info.ContainsKey("HeaderSeparatedWidth"))
                    {
                        try
                        {
                            var width = Double.Parse(Info["HeaderSeparatedWidth"]);
                            return width;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    return 1.0;
                }
                set
                {
                    try
                    {
                        if (Info.ContainsKey("HeaderSeparatedWidth")) Info["HeaderSeparatedWidth"] = value.ToString();
                        else Info.Add("HeaderSeparatedWidth", value.ToString());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            #endregion

        #endregion

        #endregion

        #region Attach Propory

        #region 内容面板及其边框

        #region BorderThickness

        /// <summary>
/// Button边框厚度
/// </summary>
[PublicAPI]
public static readonly DependencyProperty BorderThicknessProperty =
    DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(DataGridAttriXaml),
                            new FrameworkPropertyMetadata(BorderThickness,
                                                            FrameworkPropertyMetadataOptions.
                                                                AffectsArrange |
                                                            FrameworkPropertyMetadataOptions.Inherits,
                                                            null, ThicknessUtil.CoerceNonNegative));

[PublicAPI]
[SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
public static Thickness GetBorderThickness([NotNull] DependencyObject obj)
{
    ValidationHelper.NotNull(obj, "obj");
    ThicknessUtil.EnsureNonNegative();
    return BoxingHelper<Thickness>.Unbox(obj.GetValue(BorderThicknessProperty));
}

[PublicAPI]
public static void SetBorderThickness([NotNull] DependencyObject obj, Thickness value)
{
    ValidationHelper.NotNull(obj, "obj");
    obj.SetValue(BorderThicknessProperty, value);
}

#endregion

        #region NormalBackgrounBrush

        [PublicAPI]
        public static DependencyProperty NormalBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("NormalBackgrounBrush", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBackgrounBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(NormalBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalBackgrounBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalBackgrounBrushProperty, value);
        }

        #endregion

        #region NormalBorderBrush

        [PublicAPI]
        public static DependencyProperty NormalBorderBrushProperty =
            DependencyProperty.RegisterAttached("NormalBorderBrush", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBorderBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(NormalBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalBorderBrushProperty, value);
        }

        #endregion

        #region DisableBackgrounBrush

        [PublicAPI]
        public static DependencyProperty DisableBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("DisableBackgrounBrush", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBackgrounBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(DisableBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBackgrounBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableBackgrounBrushProperty, value);
        }

        #endregion

        #region DisableBorderBrush

        [PublicAPI]
        public static DependencyProperty DisableBorderBrushProperty =
            DependencyProperty.RegisterAttached("DisableBorderBrush", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBorderBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(DisableBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableBorderBrushProperty, value);
        }

        #endregion

        #endregion
       
        #region Item项

        #region ItemNormalBackground
        [PublicAPI]
        public static DependencyProperty ItemNormalBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemNormalBackground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemNormalBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemNormalBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemNormalBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemNormalBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemNormalBackgroundProperty, value);
        }
        #endregion

        #region ItemNormalForeground
        [PublicAPI]
        public static DependencyProperty ItemNormalForegroundProperty =
            DependencyProperty.RegisterAttached("ItemNormalForeground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemNormalForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemNormalForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemNormalForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemNormalForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemNormalForegroundProperty, value);
        }
        #endregion

        #region ItemNormalBorderBrush
        [PublicAPI]
        public static DependencyProperty ItemNormalBorderBrushProperty =
            DependencyProperty.RegisterAttached("ItemNormalBorderBrush", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemNormalBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemNormalBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemNormalBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetItemNormalBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemNormalBorderBrushProperty, value);
        }
        #endregion

        #region ItemBorderThickness

        [PublicAPI]
        public static readonly DependencyProperty ItemBorderThicknessProperty =
            DependencyProperty.RegisterAttached("ItemBorderThickness", typeof(Thickness), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemBorderThickness,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsArrange |
                                                                  FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, ThicknessUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetItemBorderThickness([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<Thickness>.Unbox(obj.GetValue(ItemBorderThicknessProperty));
        }

        [PublicAPI]
        public static void SetItemBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemBorderThicknessProperty, value);
        }


        #endregion


        #region ItemMouseOverBackground
        [PublicAPI]
        public static DependencyProperty ItemMouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemMouseOverBackground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemMouseOverBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemMouseOverBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemMouseOverBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemMouseOverBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemMouseOverBackgroundProperty, value);
        }
        #endregion

        #region ItemMouseOverForeground
        [PublicAPI]
        public static DependencyProperty ItemMouseOverForegroundProperty =
            DependencyProperty.RegisterAttached("ItemMouseOverForeground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemMouseOverForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemMouseOverForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemMouseOverForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemMouseOverForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemMouseOverForegroundProperty, value);
        }
        #endregion

        #region ItemSelectedBackground
        [PublicAPI]
        public static DependencyProperty ItemSelectedBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemSelectedBackground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemSelectedBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemSelectedBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemSelectedBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemSelectedBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemSelectedBackgroundProperty, value);
        }
        #endregion

        #region ItemSelectedForeground
        [PublicAPI]
        public static DependencyProperty ItemSelectedForegroundProperty =
            DependencyProperty.RegisterAttached("ItemSelectedForeground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(ItemSelectedForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemSelectedForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemSelectedForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemSelectedForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemSelectedForegroundProperty, value);
        }
        #endregion

        #endregion

        #region Header项
        #region HeaderNormalBackground
        [PublicAPI]
        public static DependencyProperty HeaderNormalBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderNormalBackground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderNormalBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderNormalBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderNormalBackgroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderNormalBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderNormalBackgroundProperty, value);
        }
        #endregion
        #region HeaderNormalForeground
        [PublicAPI]
        public static DependencyProperty HeaderNormalForegroundProperty =
            DependencyProperty.RegisterAttached("HeaderNormalForeground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderNormalForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderNormalForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderNormalForegroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderNormalForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderNormalForegroundProperty, value);
        }
        #endregion
        #region HeaderMouseOverBackground
        [PublicAPI]
        public static DependencyProperty HeaderMouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderMouseOverBackground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderMouseOverBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderMouseOverBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderMouseOverBackgroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderMouseOverBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderMouseOverBackgroundProperty, value);
        }
        #endregion
        #region HeaderMouseOverForeground
        [PublicAPI]
        public static DependencyProperty HeaderMouseOverForegroundProperty =
            DependencyProperty.RegisterAttached("HeaderMouseOverForeground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderMouseOverForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderMouseOverForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderMouseOverForegroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderMouseOverForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderMouseOverForegroundProperty, value);
        }
        #endregion
        #region HeaderPressedBackground
        [PublicAPI]
        public static DependencyProperty HeaderPressedBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderPressedBackground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderPressedBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderPressedBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderPressedBackgroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderPressedBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderPressedBackgroundProperty, value);
        }
        #endregion
        #region HeaderPressedForeground
        [PublicAPI]
        public static DependencyProperty HeaderPressedForegroundProperty =
            DependencyProperty.RegisterAttached("HeaderPressedForeground", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderPressedForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderPressedForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderPressedForegroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderPressedForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderPressedForegroundProperty, value);
        }
        #endregion
        #endregion

        #region 表头分隔符

        #region HeaderSeparatedColor
        [PublicAPI]
        public static DependencyProperty HeaderSeparatedColorProperty =
            DependencyProperty.RegisterAttached("HeaderSeparatedColor", typeof(SolidColorBrush), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderSeparatedColor,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderSeparatedColor([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderSeparatedColorProperty);
        }

        [PublicAPI]
        public static void SetHeaderSeparatedColor([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderSeparatedColorProperty, value);
        }
        #endregion

        #region HeaderSeparatedWidth

        /// <summary>
        /// Scroll宽度
        /// </summary>

        [PublicAPI]
        public static readonly DependencyProperty HeaderSeparatedWidthProperty =
            DependencyProperty.RegisterAttached("HeaderSeparatedWidth", typeof(double), typeof(DataGridAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderSeparatedWidth,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetHeaderSeparatedWidth([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(HeaderSeparatedWidthProperty));
        }

        [PublicAPI]
        public static void SetHeaderSeparatedWidth([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderSeparatedWidthProperty, value);
        }

        #endregion
        #endregion

        #endregion
    }
}
