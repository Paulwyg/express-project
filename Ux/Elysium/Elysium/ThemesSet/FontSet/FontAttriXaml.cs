using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;
using FontStyle = System.Windows.FontStyle;

namespace Elysium.ThemesSet.FontSet
{
    [PublicAPI]
    public static class FontAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "FontAttriXaml";

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

        #region 字体大小 等 static
        #region RowHeadHeightt
        public static double RowHeadHeightt
        {
            get
            {
                if (Info.ContainsKey("RowHeadHeightt"))
                {
                    try
                    {
                        var width = Double.Parse(Info["RowHeadHeightt"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 16;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RowHeadHeightt")) Info["RowHeadHeightt"] = value.ToString();
                    else Info.Add("RowHeadHeightt", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region RowHeightt
        public static double   RowHeightt
        {
            get
            {
                if (Info.ContainsKey("RowHeightt"))
                {
                    try
                    {
                        var width = Double.Parse(Info["RowHeightt"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 16;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RowHeightt")) Info["RowHeightt"] = value.ToString();
                    else Info.Add("RowHeightt", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }


        public static double RowHeighttDouble
        {
            get
            {
                if (Info.ContainsKey("RowHeightt"))
                {
                    try
                    {
                        var width = Double.Parse(Info["RowHeightt"]);
                        return width*2;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 32;
            }
        }

        public static double RowHeighttD1
        {
            get
            {
                if (Info.ContainsKey("RowHeightt"))
                {
                    try
                    {
                        var width = Double.Parse(Info["RowHeightt"]);
                        return width-1;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 15;
            }
        }
        #endregion


        #region RowHeightTree
        public static double RowHeightTree
        {
            get
            {
                if (Info.ContainsKey("RowHeightTree"))
                {
                    try
                    {
                        var width = Double.Parse(Info["RowHeightTree"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 16;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("RowHeightTree")) Info["RowHeightTree"] = value.ToString();
                    else Info.Add("RowHeightTree", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region MyFontSize
        public static double MyFontSize
        {
            get
            {
                if (Info.ContainsKey("MyFontSize"))
                {
                    try
                    {
                        var width = Double.Parse(Info["MyFontSize"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return 12;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyFontSize")) Info["MyFontSize"] = value.ToString();
                    else Info.Add("MyFontSize", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region MyFontWeight
        public static FontWeight MyFontWeight
        {
            get
            {
                if (Info.ContainsKey("MyFontWeight"))
                {
                    try
                    {
                        var width = GetFontWeight(Info["MyFontWeight"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return FontWeights.Normal;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyFontWeight")) Info["MyFontWeight"] = value.ToString();
                    else Info.Add("MyFontWeight", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static FontWeight GetFontWeight(string weight)
        {
            FontWeight fw;
            switch (weight)
            {
                case "Black":
                    fw = FontWeights.Black;
                    break;
                case "Bold":
                    fw = FontWeights.Bold;
                    break;
                case "DemiBold":
                    fw = FontWeights.DemiBold;
                    break;
                case "ExtraBlack":
                    fw = FontWeights.ExtraBlack;
                    break;
                case "ExtraBold":
                    fw = FontWeights.ExtraBold;
                    break;
                case "ExtraLight":
                    fw = FontWeights.ExtraLight;
                    break;
                case "Heavy":
                    fw = FontWeights.Heavy;
                    break;
                case "Light":
                    fw = FontWeights.Light;
                    break;
                case "Medium":
                    fw = FontWeights.Medium;
                    break;
                case "Normal":
                    fw = FontWeights.Normal;
                    break;
                case "Regular":
                    fw = FontWeights.Regular;
                    break;
                case "SemiBold":
                    fw = FontWeights.SemiBold;
                    break;
                case "Thin":
                    fw = FontWeights.Thin;
                    break;
                case "UltraBlack":
                    fw = FontWeights.UltraBlack;
                    break;

                case "UltraBold":
                    fw = FontWeights.UltraBold;
                    break;
                case "UltraLight":
                    fw = FontWeights.UltraLight;
                    break;

                default:
                    fw = FontWeights.Normal;
                    break;
            }

            return fw;
        }

        #endregion

        #region MyFontStyle
        public static FontStyle MyFontStyle
        {
            get
            {
                if (Info.ContainsKey("MyFontStyle"))
                {
                    try
                    {
                        var width = GetFontStyle(Info["MyFontStyle"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return FontStyles.Normal;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyFontStyle")) Info["MyFontStyle"] = value.ToString();
                    else Info.Add("MyFontStyle", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static FontStyle GetFontStyle(string style)
        {
            FontStyle fw;
            switch (style)
            {
                case "Normal":
                    fw = FontStyles.Normal;
                    break;
                case "Italic":
                    fw = FontStyles.Italic;
                    break;
                case "Oblique":
                    fw = FontStyles.Oblique;
                    break;
                default:
                    fw = FontStyles.Normal;
                    break;
            }

            return fw;
        }
        #endregion

        #region MyFontStretch
        public static FontStretch MyFontStretch
        {
            get
            {
                if (Info.ContainsKey("MyFontStretch"))
                {
                    try
                    {
                        var width = GetFontStretch(Info["MyFontStretch"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return FontStretches.Normal;
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyFontStretch")) Info["MyFontStretch"] = value.ToString();
                    else Info.Add("MyFontStretch", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static FontStretch GetFontStretch(string style)
        {
            FontStretch fw;
            switch (style)
            {
                case "Condensed":
                    fw = FontStretches.Condensed;
                    break;
                case "Expanded":
                    fw = FontStretches.Expanded;
                    break;
                case "ExtraCondensed":
                    fw = FontStretches.ExtraCondensed;
                    break;
                case "ExtraExpanded":
                    fw = FontStretches.ExtraExpanded;
                    break;
                case "Medium":
                    fw = FontStretches.Medium;
                    break;
                case "Normal":
                    fw = FontStretches.Normal;
                    break;
                case "SemiCondensed":
                    fw = FontStretches.SemiCondensed;
                    break;
                case "SemiExpanded":
                    fw = FontStretches.SemiExpanded;
                    break;
                case "UltraCondensed":
                    fw = FontStretches.UltraCondensed;
                    break;
                case "UltraExpanded":
                    fw = FontStretches.UltraExpanded;
                    break;
                default:
                    fw = FontStretches.Normal;
                    break;
            }

            return fw;
        }
        #endregion

        #region MyFontFamily
        public static FontFamily MyFontFamily
        {
            get
            {
                if (Info.ContainsKey("MyFontFamily"))
                {
                    try
                    {
                        var width = new FontFamily(Info["MyFontFamily"]);
                        return width;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return new FontFamily("Arial");
            }
            set
            {
                try
                {
                    if (Info.ContainsKey("MyFontFamily")) Info["MyFontFamily"] = value.ToString();
                    else Info.Add("MyFontFamily", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion
        #endregion

        #region Attach Propory
        #region RowHeadHeightt

        [PublicAPI]
        public static readonly DependencyProperty RowHeadHeighttProperty =
            DependencyProperty.RegisterAttached("RowHeadHeightt", typeof(double), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(RowHeadHeightt,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetRowHeadHeightt([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(RowHeadHeighttProperty));
        }

        [PublicAPI]
        public static void SetRowHeadHeightt([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RowHeadHeighttProperty, value);
        }

        #endregion

        #region RowHeightt

        [PublicAPI]
        public static readonly DependencyProperty RowHeighttProperty =
            DependencyProperty.RegisterAttached("RowHeightt", typeof(double  ), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(RowHeightt,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetRowHeightt([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(RowHeighttProperty));
        }

        [PublicAPI]
        public static void SetRowHeightt([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RowHeighttProperty, value);
        }

        #endregion

        #region RowHeightTree

        [PublicAPI]
        public static readonly DependencyProperty RowHeightTreeProperty =
            DependencyProperty.RegisterAttached("RowHeightTree", typeof(double), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(RowHeightTree,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetRowHeightTree([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(RowHeightTreeProperty));
        }

        [PublicAPI]
        public static void SetRowHeightTree([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(RowHeightTreeProperty, value);
        }

        #endregion


        #region MyFontSize

        [PublicAPI]
        public static readonly DependencyProperty MyFontSizeProperty =
            DependencyProperty.RegisterAttached("MyFontSize", typeof(double), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(MyFontSize,
                                                                   FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetMyFontSize([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(MyFontSizeProperty));
        }

        [PublicAPI]
        public static void SetMyFontSize([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyFontSizeProperty, value);
        }

        #endregion

        #region MyFontWeight

        [PublicAPI]
        public static readonly DependencyProperty MyFontWeightProperty =
            DependencyProperty.RegisterAttached("MyFontWeight", typeof(FontWeight), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(MyFontWeight));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static FontWeight GetMyFontWeight([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<FontWeight>.Unbox(obj.GetValue(MyFontWeightProperty));
        }

        [PublicAPI]
        public static void SetMyFontWeight([NotNull] DependencyObject obj, FontWeight value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyFontWeightProperty, value);
        }

        #endregion

        #region MyFontStyle

        [PublicAPI]
        public static readonly DependencyProperty MyFontStyleProperty =
            DependencyProperty.RegisterAttached("MyFontStyle", typeof(FontStyle), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(MyFontStyle));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static FontStyle GetMyFontStyle([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<FontStyle>.Unbox(obj.GetValue(MyFontStyleProperty));
        }

        [PublicAPI]
        public static void SetMyFontStyle([NotNull] DependencyObject obj, FontStyle value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyFontStyleProperty, value);
        }

        #endregion

        #region MyFontStretch

        [PublicAPI]
        public static readonly DependencyProperty MyFontStretchProperty =
            DependencyProperty.RegisterAttached("MyFontStretch", typeof(FontStretch), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(MyFontStretch));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static FontStretch GetMyFontStretch([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<FontStretch>.Unbox(obj.GetValue(MyFontStretchProperty));
        }

        [PublicAPI]
        public static void SetMyFontStretch([NotNull] DependencyObject obj, FontStretch value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyFontStretchProperty, value);
        }

        #endregion

        #region MyFontFamily

        [PublicAPI]
        public static readonly DependencyProperty MyFontFamilyProperty =
            DependencyProperty.RegisterAttached("MyFontFamily", typeof(FontFamily), typeof(FontAttriXaml),
                                    new FrameworkPropertyMetadata(MyFontFamily));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static FontFamily GetMyFontFamily([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return new FontFamily(obj.GetValue(MyFontFamilyProperty).ToString());
        }

        [PublicAPI]
        public static void SetMyFontFamily([NotNull] DependencyObject obj, FontFamily value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(MyFontFamilyProperty, value);
        }

        #endregion

        #endregion
    }
}
