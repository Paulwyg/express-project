using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using Elysium.Extensions;
using Elysium.ThemesSet.Common;
using JetBrains.Annotations;

namespace Elysium.ThemesSet.Common
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class CommonXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "CommonXaml";

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

        public static double ContentFontSize //= 9d*(96d/72d);
        {
            get
            {
                if (Info.ContainsKey("ContentFontSize"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["ContentFontSize"]);
                        return convertFromString;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return 9d*(96d/72d);

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("ContentFontSize")) Info["ContentFontSize"] = value.ToString();
                    else Info.Add("ContentFontSize", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

     
        #endregion

        #region Attach Propory

        #region ContentFontSize

        /// <summary>
        /// 字体大小
        /// </summary>
        [PublicAPI] public static readonly DependencyProperty ContentFontSizeProperty =
            DependencyProperty.RegisterAttached("ContentFontSize", typeof(double), typeof(CommonXaml),
                                                new FrameworkPropertyMetadata(ContentFontSize,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsMeasure |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsArrange |
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.Inherits,
                                                                              null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetContentFontSize([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            DoubleUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(ContentFontSizeProperty));
        }

        [PublicAPI]
        public static void SetContentFontSize([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ContentFontSizeProperty, value);
        }


        #endregion

        #endregion
    }
}
