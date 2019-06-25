using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.ButtonSet;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.TextBoxsSet
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class TextBoxsXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "TextBoxsXaml";

        private static Dictionary<string, string> _infoinfo;

        private static Dictionary<string, string> Info
        {
            get
            {
                if (_infoinfo == null)
                {
                    _infoinfo = new Dictionary<string, string>();
                    var tmp = ReadSave.Read(ButtonSetXmlFileName);
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
            ReadSave.Save(Info, ButtonSetXmlFileName);
        }

        #region 背景色 等 static


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
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));

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
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x01, 0x7b, 0xcd));

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

        public static SolidColorBrush NormalForegroundBrush
        {
            get
            {
                if (Info.ContainsKey("NormalForegroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalForegroundBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x17, 0x17, 0x17));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("NormalForegroundBrush")) Info["NormalForegroundBrush"] = value.Color.ToString();
                    else Info.Add("NormalForegroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush MouseOverBackgroundBrush
        {
            get
            {
                if (Info.ContainsKey("MouseOverBackgroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["MouseOverBackgroundBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MouseOverBackgroundBrush"))
                        Info["MouseOverBackgroundBrush"] = value.Color.ToString();
                    else Info.Add("MouseOverBackgroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush MouseOverForegroundBrush
        {
            get
            {
                if (Info.ContainsKey("MouseOverForegroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["MouseOverForegroundBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MouseOverForegroundBrush"))
                        Info["MouseOverForegroundBrush"] = value.Color.ToString();
                    else Info.Add("MouseOverForegroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush MouseOverBorderBrush
        {
            get
            {
                if (Info.ContainsKey("MouseOverBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["MouseOverBorderBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x17, 0x17, 0x17));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MouseOverBorderBrush")) Info["MouseOverBorderBrush"] = value.Color.ToString();
                    else Info.Add("MouseOverBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush CaretBrush
        {
            get
            {
                if (Info.ContainsKey("CaretBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["CaretBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x4d, 0x4d, 0x4d));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("CaretBrush")) Info["CaretBrush"] = value.Color.ToString();
                    else Info.Add("CaretBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush SelectionBrush
        {
            get
            {
                if (Info.ContainsKey("SelectionBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["SelectionBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x77, 0x77, 0x77));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("SelectionBrush"))
                        Info["SelectionBrush"] = value.Color.ToString();
                    else Info.Add("SelectionBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

       

        public static SolidColorBrush DisableForegroundBrush
        {
            get
            {
                if (Info.ContainsKey("DisableForegroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DisableForegroundBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DisableForegroundBrush"))
                        Info["DisableForegroundBrush"] = value.Color.ToString();
                    else Info.Add("DisableForegroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush DisableBackgroundBrush
        {
            get
            {
                if (Info.ContainsKey("DisableBackgroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DisableBackgroundBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0xb8, 0xb8, 0xb8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DisableBackgroundBrush"))
                        Info["DisableBackgroundBrush"] = value.Color.ToString();
                    else Info.Add("DisableBackgroundBrush", value.Color.ToString());
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
                            var cl = (Color) convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));

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


        public static SolidColorBrush InputingForegroundBrush
        {
            get
            {
                if (Info.ContainsKey("InputingForegroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["InputingForegroundBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x01, 0x01, 0x01));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("InputingForegroundBrush"))
                        Info["InputingForegroundBrush"] = value.Color.ToString();
                    else Info.Add("InputingForegroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush InputingBackgroundBrush
        {
            get
            {
                if (Info.ContainsKey("InputingBackgroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["InputingBackgroundBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("InputingBackgroundBrush"))
                        Info["InputingBackgroundBrush"] = value.Color.ToString();
                    else Info.Add("InputingBackgroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush InputingBorderBrush
        {
            get
            {
                if (Info.ContainsKey("InputingBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["InputingBorderBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x01, 0x7b, 0xcd));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("InputingBorderBrush")) Info["InputingBorderBrush"] = value.Color.ToString();
                    else Info.Add("InputingBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region Attach Propory


        #region BorderThickness

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI] public static readonly  DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof (Thickness), typeof (TextBoxsXaml),
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

        [PublicAPI] public static DependencyProperty NormalBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("NormalBackgrounBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(NormalBackgrounBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(NormalBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalBackgrounBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalBackgrounBrushProperty, value);
        }

        #endregion

        #region NormalBorderBrush

        [PublicAPI] public static DependencyProperty NormalBorderBrushProperty =
            DependencyProperty.RegisterAttached("NormalBorderBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(NormalBorderBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(NormalBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalBorderBrushProperty, value);
        }

        #endregion

        #region NormalForegroundBrush

        [PublicAPI] public static DependencyProperty NormalForegroundBrushProperty =
            DependencyProperty.RegisterAttached("NormalForegroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(NormalForegroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalForegroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(NormalForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalForegroundBrushProperty, value);
        }

        #endregion




        #region MouseOverBackgroundBrush

        [PublicAPI] public static DependencyProperty MouseOverBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBackgroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(MouseOverBackgroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBackgroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(MouseOverBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverBackgroundBrushProperty, value);
        }

        #endregion

        #region MouseOverForegroundBrush

        [PublicAPI] public static DependencyProperty MouseOverForegroundBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverForegroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(MouseOverForegroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverForegroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(MouseOverForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverForegroundBrushProperty, value);
        }

        #endregion

        #region MouseOverBorderBrush

        [PublicAPI] public static DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(MouseOverBorderBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(MouseOverBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        #endregion



        #region InputingForegroundBrush

        [PublicAPI] public static DependencyProperty InputingForegroundBrushProperty =
            DependencyProperty.RegisterAttached("InputingForegroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(InputingForegroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetInputingForegroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(InputingForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetInputingForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(InputingForegroundBrushProperty, value);
        }

        #endregion

        #region InputingBackgroundBrush

        [PublicAPI] public static DependencyProperty InputingBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("InputingBackgroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(InputingBackgroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetInputingBackgroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(InputingBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetInputingBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(InputingBackgroundBrushProperty, value);
        }

        #endregion

        #region InputingBorderBrush

        [PublicAPI] public static DependencyProperty InputingBorderBrushProperty =
            DependencyProperty.RegisterAttached("InputingBorderBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(InputingBorderBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetInputingBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(InputingBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetInputingBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(InputingBorderBrushProperty, value);
        }

        #endregion



        #region DisableForegroundBrush

        [PublicAPI] public static DependencyProperty DisableForegroundBrushProperty =
            DependencyProperty.RegisterAttached("DisableForegroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(DisableForegroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableForegroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(DisableForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableForegroundBrushProperty, value);
        }

        #endregion

        #region DisableBackgroundBrush

        [PublicAPI] public static DependencyProperty DisableBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("DisableBackgroundBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(DisableBackgroundBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBackgroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(DisableBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableBackgroundBrushProperty, value);
        }

        #endregion

        #region DisableBorderBrush

        [PublicAPI] public static DependencyProperty DisableBorderBrushProperty =
            DependencyProperty.RegisterAttached("DisableBorderBrush", typeof (SolidColorBrush), typeof (TextBoxsXaml),
                                                new FrameworkPropertyMetadata(DisableBorderBrush,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush) obj.GetValue(DisableBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableBorderBrushProperty, value);
        }

        #endregion

        #region CaretBrush

        [PublicAPI]
        public static DependencyProperty CaretBrushProperty =
            DependencyProperty.RegisterAttached("CaretBrush", typeof(SolidColorBrush), typeof(TextBoxsXaml),
                                    new FrameworkPropertyMetadata(CaretBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetCaretBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(CaretBrushProperty);
        }

        [PublicAPI]
        public static void SetCaretBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(CaretBrushProperty, value);
        }

        #endregion

        #region SelectionBrush

        [PublicAPI]
        public static DependencyProperty SelectionBrushProperty =
            DependencyProperty.RegisterAttached("SelectionBrush", typeof(SolidColorBrush), typeof(TextBoxsXaml),
                                    new FrameworkPropertyMetadata(SelectionBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetSelectionBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(SelectionBrushProperty);
        }

        [PublicAPI]
        public static void SetSelectionBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(SelectionBrushProperty, value);
        }

        #endregion

        #endregion
    }
}
