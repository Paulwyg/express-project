using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.GroupBoxSet
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class GroupBoxAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "GroupBoxAttriXaml";

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

        public static Thickness BorderThickness
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
                return new Thickness(0.5d);

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
        #endregion

        #region Attach Propory

        #region NormalForeground

        [PublicAPI]
        public static DependencyProperty NormalForegroundProperty =
            DependencyProperty.RegisterAttached("NormalForeground", typeof(SolidColorBrush), typeof(GroupBoxAttriXaml),
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

        #region BorderThickness

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI]
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(GroupBoxAttriXaml),
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

        #region NormalBorderBrush

        [PublicAPI]
        public static DependencyProperty NormalBorderBrushProperty =
            DependencyProperty.RegisterAttached("NormalBorderBrush", typeof(SolidColorBrush), typeof(GroupBoxAttriXaml),
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

        #endregion
    }
}
