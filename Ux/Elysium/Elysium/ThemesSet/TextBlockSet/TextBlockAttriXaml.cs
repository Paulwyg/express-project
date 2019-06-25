using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.TextBlockSet
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class TextBlockAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "TextBlockAttriXaml";

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
        public static SolidColorBrush NormalBackground
        {
            get
            {
                if (Info.ContainsKey("NormalBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalBackground"]);
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
                    if (Info.ContainsKey("NormalBackground")) Info["NormalBackground"] = value.Color.ToString();
                    else Info.Add("NormalBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush NormalForeground
        {
            get
            {
                if (Info.ContainsKey("NormalForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalForeground"]);
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
                    if (Info.ContainsKey("NormalForeground")) Info["NormalForeground"] = value.Color.ToString();
                    else Info.Add("NormalForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush MouseOverBackground
        {
            get
            {
                if (Info.ContainsKey("MouseOverBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["MouseOverBackground"]);
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
                    if (Info.ContainsKey("MouseOverBackground")) Info["MouseOverBackground"] = value.Color.ToString();
                    else Info.Add("MouseOverBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush MouseOverForeground
        {
            get
            {
                if (Info.ContainsKey("MouseOverForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["MouseOverForeground"]);
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
                    if (Info.ContainsKey("MouseOverForeground"))
                        Info["MouseOverForeground"] = value.Color.ToString();
                    else Info.Add("MouseOverForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region Attach Propory

        #region NormalBackground

        [PublicAPI]
        public static DependencyProperty NormalBackgroundProperty =
            DependencyProperty.RegisterAttached("NormalBackground", typeof(SolidColorBrush), typeof(TextBlockAttriXaml),
                                                new FrameworkPropertyMetadata(NormalBackground,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(NormalBackgroundProperty);
        }

        [PublicAPI]
        public static void SetNormalBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalBackgroundProperty, value);
        }

        #endregion

        #region NormalForeground

        [PublicAPI]
        public static DependencyProperty NormalForegroundProperty =
            DependencyProperty.RegisterAttached("NormalForeground", typeof(SolidColorBrush), typeof(TextBlockAttriXaml),
                                                new FrameworkPropertyMetadata(NormalForeground,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(NormalForegroundProperty);
        }

        [PublicAPI]
        public static void SetNormalForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalForegroundProperty, value);
        }

        #endregion

        #region MouseOverBackground

        [PublicAPI]
        public static DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("MouseOverBackground", typeof(SolidColorBrush), typeof(TextBlockAttriXaml),
                                                new FrameworkPropertyMetadata(MouseOverBackground,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(MouseOverBackgroundProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverBackgroundProperty, value);
        }

        #endregion

        #region MouseOverForeground

        [PublicAPI]
        public static DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.RegisterAttached("MouseOverForeground", typeof(SolidColorBrush), typeof(TextBlockAttriXaml),
                                                new FrameworkPropertyMetadata(MouseOverForeground,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(MouseOverForegroundProperty);
        }

        [PublicAPI]
        public static void SetMouseOverForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MouseOverForegroundProperty, value);
        }

        #endregion

        #endregion
    }
}
