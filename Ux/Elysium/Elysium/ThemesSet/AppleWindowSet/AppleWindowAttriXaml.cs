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

namespace Elysium.ThemesSet.AppleWindowSet
{
    [PublicAPI]
    public static class AppleWindowAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "AppleWindowAttriXaml";

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

        public static SolidColorBrush Background
        {
            get
            {
                if (Info.ContainsKey("Background"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["Background"]);
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
                    if (Info.ContainsKey("Background")) Info["Background"] = value.Color.ToString();
                    else Info.Add("Background", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush Foreground
        {
            get
            {
                if (Info.ContainsKey("Foreground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["Foreground"]);
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
                    if (Info.ContainsKey("Foreground")) Info["Foreground"] = value.Color.ToString();
                    else Info.Add("Foreground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush HeaderBrush
        {
            get
            {
                if (Info.ContainsKey("HeaderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["HeaderBrush"]);
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
                    if (Info.ContainsKey("HeaderBrush")) Info["HeaderBrush"] = value.Color.ToString();
                    else Info.Add("HeaderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush BorderBrush
        {
            get
            {
                if (Info.ContainsKey("BorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["BorderBrush"]);
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
                    if (Info.ContainsKey("BorderBrush")) Info["BorderBrush"] = value.Color.ToString();
                    else Info.Add("BorderBrush", value.Color.ToString());
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
        [PublicAPI]
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(AppleWindowAttriXaml),
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


        #region Foreground

        [PublicAPI]
        public static DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(SolidColorBrush), typeof(AppleWindowAttriXaml),
                                    new FrameworkPropertyMetadata(Foreground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ForegroundProperty);
        }

        [PublicAPI]
        public static void SetForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ForegroundProperty, value);
        }

        #endregion

        #region Background

        [PublicAPI]
        public static DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(SolidColorBrush), typeof(AppleWindowAttriXaml),
                                    new FrameworkPropertyMetadata(Background,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(BackgroundProperty);
        }

        [PublicAPI]
        public static void SetBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(BackgroundProperty, value);
        }

        #endregion

        #region HeaderBrush

        [PublicAPI]
        public static DependencyProperty HeaderBrushProperty =
            DependencyProperty.RegisterAttached("HeaderBrush", typeof(SolidColorBrush), typeof(AppleWindowAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderBrushProperty);
        }

        [PublicAPI]
        public static void SetHeaderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderBrushProperty, value);
        }

        #endregion

        #region BorderBrush

        [PublicAPI]
        public static DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush", typeof(SolidColorBrush), typeof(AppleWindowAttriXaml),
                                    new FrameworkPropertyMetadata(BorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(BorderBrushProperty);
        }

        [PublicAPI]
        public static void SetBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(BorderBrushProperty, value);
        }

        #endregion

        #endregion
    }
}
