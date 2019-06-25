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

namespace Elysium.ThemesSet.ScrollSet
{
    [PublicAPI]
    public static class ScrollAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "ScrollBarXaml";

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

        public static double MyHeight
        {
            get
            {
                if (Info.ContainsKey("MyHeight"))
                {
                    try
                    {
                        var height = Double.Parse(Info["MyHeight"]);
                        return height;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                return 20.0;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyHeight")) Info["MyHeight"] = value.ToString();
                    else Info.Add("MyHeight", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static double MyWidth
        {
            get
            {
                if (Info.ContainsKey("MyWidth"))
                {
                    try
                    {
                        var width = Double.Parse(Info["MyWidth"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 20.0;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyWidth")) Info["MyWidth"] = value.ToString();
                    else Info.Add("MyWidth", value.ToString());
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x77, 0x74, 0x74));

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
                            var cl = (Color)convertFromString;
                            return new SolidColorBrush(cl);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new SolidColorBrush(Color.FromArgb(0xff, 0x4D, 0x4D, 0x4D));

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
                            var cl = (Color)convertFromString;
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
                    if (Info.ContainsKey("MouseOverBorderBrush")) Info["MouseOverBorderBrush"] = value.Color.ToString();
                    else Info.Add("MouseOverBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }


        public static SolidColorBrush PressedBorderBrush
        {
            get
            {
                if (Info.ContainsKey("PressedBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["PressedBorderBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("PressedBorderBrush")) Info["PressedBorderBrush"] = value.Color.ToString();
                    else Info.Add("PressedBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush PressedBackgroundBrush
        {
            get
            {
                if (Info.ContainsKey("PressedBackgroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["PressedBackgroundBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("PressedBackgroundBrush"))
                        Info["PressedBackgroundBrush"] = value.Color.ToString();
                    else Info.Add("PressedBackgroundBrush", value.Color.ToString());
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
                            var cl = (Color)convertFromString;
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
                            var cl = (Color)convertFromString;
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
                    if (Info.ContainsKey("DisableBorderBrush")) Info["DisableBorderBrush"] = value.Color.ToString();
                    else Info.Add("DisableBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region Attach Propory


        #region Height

        /// <summary>
        /// Scroll高度
        /// </summary>
        /// 


        [PublicAPI]
        public static readonly DependencyProperty MyHeightProperty =
            DependencyProperty.RegisterAttached("MyHeight", typeof(double), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(MyHeight,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));


        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetMyHeight([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            DoubleUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(MyHeightProperty));
        }

        [PublicAPI]
        public static void SetMyHeight([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyHeightProperty, value);
        }

        #endregion

        #region MyWidth

        /// <summary>
        /// Scroll宽度
        /// </summary>

        [PublicAPI]
        public static readonly DependencyProperty MyWidthProperty =
            DependencyProperty.RegisterAttached("MyWidth", typeof(double), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(MyWidth,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetMyWidth([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(MyWidthProperty));
        }

        [PublicAPI]
        public static void SetMyWidth([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyWidthProperty, value);
        }

        #endregion

        #region NormalBackgrounBrush

        [PublicAPI]
        public static DependencyProperty NormalBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("NormalBackgrounBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
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
            DependencyProperty.RegisterAttached("NormalBorderBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
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


        #region MouseOverBackgroundBrush

        [PublicAPI]
        public static DependencyProperty MouseOverBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBackgroundBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(MouseOverBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBackgroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(MouseOverBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverBackgroundBrushProperty, value);
        }

        #endregion


        #region MouseOverBorderBrush

        [PublicAPI]
        public static DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(MouseOverBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        #endregion



        #region PressedBorderBrush

        [PublicAPI]
        public static DependencyProperty PressedBorderBrushProperty =
            DependencyProperty.RegisterAttached("PressedBorderBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(PressedBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetPressedBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(PressedBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetPressedBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(PressedBorderBrushProperty, value);
        }

        #endregion

        #region PressedBackgroundBrush

        [PublicAPI]
        public static DependencyProperty PressedBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(PressedBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetPressedBackgroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(PressedBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetPressedBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(PressedBackgroundBrushProperty, value);
        }

        #endregion


        #region DisableBackgroundBrush

        [PublicAPI]
        public static DependencyProperty DisableBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("DisableBackgroundBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(DisableBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBackgroundBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(DisableBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableBackgroundBrushProperty, value);
        }

        #endregion

        #region DisableBorderBrush

        [PublicAPI]
        public static DependencyProperty DisableBorderBrushProperty =
            DependencyProperty.RegisterAttached("DisableBorderBrush", typeof(SolidColorBrush), typeof(ScrollAttriXaml),
                                    new FrameworkPropertyMetadata(DisableBorderBrush,
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
    }
}
