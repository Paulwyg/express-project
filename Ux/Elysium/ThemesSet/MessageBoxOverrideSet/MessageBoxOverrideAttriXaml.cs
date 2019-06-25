using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.MessageBoxOverrideSet
{
    [PublicAPI]
    public static class MessageBoxOverrideAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "MessageBoxOverrideAttriXaml";

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
                return new SolidColorBrush(Color.FromArgb(0x00, 0xff, 0xff, 0xff));

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

        #endregion

        #region Attach Propory

        #region Background

        [PublicAPI]
        public static DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(SolidColorBrush), typeof(MessageBoxOverrideAttriXaml),
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
            DependencyProperty.RegisterAttached("HeaderBrush", typeof(SolidColorBrush), typeof(MessageBoxOverrideAttriXaml),
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

        #endregion
    }
}
