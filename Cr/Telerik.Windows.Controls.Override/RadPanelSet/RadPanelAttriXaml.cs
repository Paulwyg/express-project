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

namespace Telerik.Windows.Controls.Override.RadPanelSet
{

    [PublicAPI]
    public static class RadPanelAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ButtonSetXmlFileName = "TabAttriXaml";

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
                return new Thickness(0d);

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
                            var cl = (Color)convertFromString;
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
                            var cl = (Color)convertFromString;
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
                            var cl = (Color)convertFromString;
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x4d, 0x4d, 0x4d));

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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x4d, 0x4d, 0x4d));

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

        public static SolidColorBrush PressedForegroundBrush
        {
            get
            {
                if (Info.ContainsKey("PressedForegroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["PressedForegroundBrush"]);
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
                    if (Info.ContainsKey("PressedForegroundBrush"))
                        Info["PressedForegroundBrush"] = value.Color.ToString();
                    else Info.Add("PressedForegroundBrush", value.Color.ToString());
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



        public static SolidColorBrush SelectedIndicateBrush
        {
            get
            {
                if (Info.ContainsKey("SelectedIndicateBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["SelectedIndicateBrush"]);
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
                    if (Info.ContainsKey("SelectedIndicateBrush"))
                        Info["SelectedIndicateBrush"] = value.Color.ToString();
                    else Info.Add("SelectedIndicateBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush UnSelectedIndicateBrush
        {
            get
            {
                if (Info.ContainsKey("UnSelectedIndicateBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["UnSelectedIndicateBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xff, 0x01, 0xff, 0xff));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("UnSelectedIndicateBrush"))
                        Info["UnSelectedIndicateBrush"] = value.Color.ToString();
                    else Info.Add("UnSelectedIndicateBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush NormalTabForegroundBrush
        {
            get
            {
                if (Info.ContainsKey("NormalTabForegroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalTabForegroundBrush"]);
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
                    if (Info.ContainsKey("NormalTabForegroundBrush"))
                        Info["NormalTabForegroundBrush"] = value.Color.ToString();
                    else Info.Add("NormalTabForegroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush NormalTabBackgroundBrush
        {
            get
            {
                if (Info.ContainsKey("NormalTabBackgroundBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalTabBackgroundBrush"]);
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
                    if (Info.ContainsKey("NormalTabBackgroundBrush"))
                        Info["NormalTabBackgroundBrush"] = value.Color.ToString();
                    else Info.Add("NormalTabBackgroundBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static SolidColorBrush NormalTabBorderBrush
        {
            get
            {
                if (Info.ContainsKey("NormalTabBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["NormalTabBorderBrush"]);
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
                    if (Info.ContainsKey("NormalTabBorderBrush")) Info["NormalTabBorderBrush"] = value.Color.ToString();
                    else Info.Add("NormalTabBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static Thickness TabBorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("TabBorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["TabBorderThickness"]);
                        return new Thickness(convertFromString);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return new Thickness(0d);

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("TabBorderThickness")) Info["TabBorderThickness"] = value.Left.ToString();
                    else Info.Add("TabBorderThickness", value.Left.ToString());
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
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(BorderThickness,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsArrange |
                                                                  FrameworkPropertyMetadataOptions.Inherits,
                                                                  null));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetBorderThickness([NotNull] DependencyObject obj)
        {

            return (Thickness)obj.GetValue(BorderThicknessProperty);
        }

        [PublicAPI]
        public static void SetBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {

            obj.SetValue(BorderThicknessProperty, value);
        }

        #endregion



        #region NormalBackgrounBrush

        [PublicAPI]
        public static DependencyProperty NormalBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("NormalBackgrounBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBackgrounBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBackgrounBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(NormalBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalBackgrounBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(NormalBackgrounBrushProperty, value);
        }

        #endregion

        #region NormalBorderBrush

        [PublicAPI]
        public static DependencyProperty NormalBorderBrushProperty =
            DependencyProperty.RegisterAttached("NormalBorderBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalBorderBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(NormalBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(NormalBorderBrushProperty, value);
        }

        #endregion

        #region NormalForegroundBrush

        [PublicAPI]
        public static DependencyProperty NormalForegroundBrushProperty =
            DependencyProperty.RegisterAttached("NormalForegroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(NormalForegroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalForegroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(NormalForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(NormalForegroundBrushProperty, value);
        }

        #endregion




        #region MouseOverBackgroundBrush

        [PublicAPI]
        public static DependencyProperty MouseOverBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBackgroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(MouseOverBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBackgroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(MouseOverBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(MouseOverBackgroundBrushProperty, value);
        }

        #endregion

        #region MouseOverForegroundBrush

        [PublicAPI]
        public static DependencyProperty MouseOverForegroundBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverForegroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(MouseOverForegroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverForegroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(MouseOverForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(MouseOverForegroundBrushProperty, value);
        }

        #endregion

        #region MouseOverBorderBrush

        [PublicAPI]
        public static DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(MouseOverBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetMouseOverBorderBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetMouseOverBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        #endregion



        #region PressedBorderBrush

        [PublicAPI]
        public static DependencyProperty PressedBorderBrushProperty =
            DependencyProperty.RegisterAttached("PressedBorderBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(PressedBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetPressedBorderBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(PressedBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetPressedBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(PressedBorderBrushProperty, value);
        }

        #endregion

        #region PressedBackgroundBrush

        [PublicAPI]
        public static DependencyProperty PressedBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(PressedBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetPressedBackgroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(PressedBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetPressedBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(PressedBackgroundBrushProperty, value);
        }

        #endregion

        #region PressedForegroundBrush

        [PublicAPI]
        public static DependencyProperty PressedForegroundBrushProperty =
            DependencyProperty.RegisterAttached("PressedForegroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(PressedForegroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetPressedForegroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(PressedForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetPressedForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(PressedForegroundBrushProperty, value);
        }

        #endregion



        #region DisableForegroundBrush

        [PublicAPI]
        public static DependencyProperty DisableForegroundBrushProperty =
            DependencyProperty.RegisterAttached("DisableForegroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(DisableForegroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableForegroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(DisableForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(DisableForegroundBrushProperty, value);
        }

        #endregion

        #region DisableBackgroundBrush

        [PublicAPI]
        public static DependencyProperty DisableBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("DisableBackgroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(DisableBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBackgroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(DisableBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(DisableBackgroundBrushProperty, value);
        }

        #endregion

        #region DisableBorderBrush

        [PublicAPI]
        public static DependencyProperty DisableBorderBrushProperty =
            DependencyProperty.RegisterAttached("DisableBorderBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(DisableBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBorderBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(DisableBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(DisableBorderBrushProperty, value);
        }

        #endregion


        #region SelectedIndicateBrush

        [PublicAPI]
        public static DependencyProperty SelectedIndicateBrushProperty =
            DependencyProperty.RegisterAttached("SelectedIndicateBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(SelectedIndicateBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetSelectedIndicateBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(SelectedIndicateBrushProperty);
        }

        [PublicAPI]
        public static void SetSelectedIndicateBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(SelectedIndicateBrushProperty, value);
        }

        #endregion

        #region UnSelectedIndicateBrush

        [PublicAPI]
        public static DependencyProperty UnSelectedIndicateBrushProperty =
            DependencyProperty.RegisterAttached("UnSelectedIndicateBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(UnSelectedIndicateBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetUnSelectedIndicateBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(UnSelectedIndicateBrushProperty);
        }

        [PublicAPI]
        public static void SetUnSelectedIndicateBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(UnSelectedIndicateBrushProperty, value);
        }

        #endregion


        #region NormalTabForegroundBrush

        [PublicAPI]
        public static DependencyProperty NormalTabForegroundBrushProperty =
            DependencyProperty.RegisterAttached("NormalTabForegroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(NormalTabForegroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalTabForegroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(NormalTabForegroundBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalTabForegroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(NormalTabForegroundBrushProperty, value);
        }

        #endregion

        #region NormalTabBackgroundBrush

        [PublicAPI]
        public static DependencyProperty NormalTabBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("NormalTabBackgroundBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(NormalTabBackgroundBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalTabBackgroundBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(NormalTabBackgroundBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalTabBackgroundBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {

            obj.SetValue(NormalTabBackgroundBrushProperty, value);
        }

        #endregion

        #region NormalTabBorderBrush

        [PublicAPI]
        public static DependencyProperty NormalTabBorderBrushProperty =
            DependencyProperty.RegisterAttached("NormalTabBorderBrush", typeof(SolidColorBrush), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(NormalTabBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetNormalTabBorderBrush([NotNull] DependencyObject obj)
        {

            return (SolidColorBrush)obj.GetValue(NormalTabBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetNormalTabBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            //ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(NormalTabBorderBrushProperty, value);
        }

        #endregion


        #region BorderThickness

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI]
        public static readonly DependencyProperty TabBorderThicknessProperty =
            DependencyProperty.RegisterAttached("TabBorderThickness", typeof(Thickness), typeof(RadPanelAttriXaml),
                                    new FrameworkPropertyMetadata(TabBorderThickness,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsArrange |
                                                                  FrameworkPropertyMetadataOptions.Inherits,
                                                                  null));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetTabBorderThickness([NotNull] DependencyObject obj)
        {
            //ValidationHelper.NotNull(obj, "obj");
            //ThicknessUtil.EnsureNonNegative();
            return (Thickness)obj.GetValue(TabBorderThicknessProperty);
        }

        [PublicAPI]
        public static void SetTabBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {
            //ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(TabBorderThicknessProperty, value);
        }

        #endregion


        #endregion
    }
}
