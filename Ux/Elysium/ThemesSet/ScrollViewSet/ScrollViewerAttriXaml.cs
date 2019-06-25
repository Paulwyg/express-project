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

namespace Elysium.ThemesSet.ScrollViewSet
{
    [PublicAPI]
    public static class ScrollViewerAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "ScrollViewerXaml";

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

        public static double MyOpacity
        {
            get
            {
                if (Info.ContainsKey("MyOpacity"))
                {
                    try
                    {
                        var myopacity = Double.Parse(Info["MyOpacity"]);
                        return myopacity;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 0.1;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyOpacity")) Info["MyOpacity"] = value.ToString();
                    else Info.Add("MyOpacity", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region Attach Propory

        #region NormalBackgrounBrush

        [PublicAPI]
        public static DependencyProperty NormalBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("NormalBackgrounBrush", typeof(SolidColorBrush), typeof(ScrollViewerAttriXaml),
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

        #region MyOpacity


        [PublicAPI]
        public static readonly DependencyProperty MyOpacityProperty =
            DependencyProperty.RegisterAttached("MyHeight", typeof(double), typeof(ScrollViewerAttriXaml),
                                    new FrameworkPropertyMetadata(MyOpacity,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));


        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetMyOpacity([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            DoubleUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(MyOpacityProperty));
        }

        [PublicAPI]
        public static void SetMyOpacity([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyOpacityProperty, value);
        }

        #endregion

        #endregion
    }
}
