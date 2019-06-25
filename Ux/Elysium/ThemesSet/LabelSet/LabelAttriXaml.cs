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

namespace Elysium.ThemesSet.LabelSet
{
    [PublicAPI]
    public static class LabelAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "LabelAttriXaml";

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
                return new SolidColorBrush(Color.FromArgb(0x00, 0xff, 0xff, 0xff));

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

        #endregion

        #region Attach Propory

        #region Foreground

        [PublicAPI]
        public static DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached("Foreground", typeof(SolidColorBrush), typeof(LabelAttriXaml),
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

        #endregion
    }
}
