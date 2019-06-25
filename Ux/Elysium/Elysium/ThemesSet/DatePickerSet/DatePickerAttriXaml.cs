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

namespace Elysium.ThemesSet.DatePickerSet
{

    [PublicAPI]
    public static class DatePickerAttriXaml
    {
        /// <summary>
        /// 按钮样式存放目录文件名称
        /// </summary>
        private const string ScrollSetXmlFileName = "DatePickerAttriXaml";

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

        #region  内容面板及其边框
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
                return new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));

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

        public static SolidColorBrush DisableBackgrounBrush
        {
            get
            {
                if (Info.ContainsKey("DisableBackgrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DisableBackgrounBrush"]);
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
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DisableBackgrounBrush")) Info["DisableBackgrounBrush"] = value.Color.ToString();
                    else Info.Add("DisableBackgrounBrush", value.Color.ToString());
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
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

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
        public static SolidColorBrush DisableForeground
        {
            get
            {
                if (Info.ContainsKey("DisableForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DisableForeground"]);
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
                return new SolidColorBrush(Color.FromArgb(0xFF, 0xB8, 0xB8, 0xB8));

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DisableForeground")) Info["v"] = value.Color.ToString();
                    else Info.Add("DisableForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        

        #endregion

        #region Item项
        #region ItemNormalBackground
        public static SolidColorBrush ItemNormalBackground
        {
            get
            {
                if (Info.ContainsKey("ItemNormalBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemNormalBackground"]);
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
                    if (Info.ContainsKey("ItemNormalBackground")) Info["ItemNormalBackground"] = value.Color.ToString();
                    else Info.Add("ItemNormalBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region NormalForeground
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
                    if (Info.ContainsKey("NormalForeground")) Info["NormalForeground"] = value.Color.ToString();
                    else Info.Add("NormalForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region ItemMouseOverBorderBrush
        public static SolidColorBrush ItemMouseOverBorderBrush
        {
            get
            {
                if (Info.ContainsKey("ItemMouseOverBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemMouseOverBorderBrush"]);
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
                    if (Info.ContainsKey("ItemMouseOverBorderBrush")) Info["ItemMouseOverBorderBrush"] = value.Color.ToString();
                    else Info.Add("ItemMouseOverBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region ItemBorderThickness
        public static Thickness ItemBorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("ItemBorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["ItemBorderThickness"]);
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
                    if (Info.ContainsKey("ItemBorderThickness")) Info["ItemBorderThickness"] = value.Left.ToString();
                    else Info.Add("ItemBorderThickness", value.Left.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion


        #region ItemMouseOverBackground
        public static SolidColorBrush ItemMouseOverBackground
        {
            get
            {
                if (Info.ContainsKey("ItemMouseOverBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemMouseOverBackground"]);
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
                    if (Info.ContainsKey("ItemMouseOverBackground")) Info["ItemMouseOverBackground"] = value.Color.ToString();
                    else Info.Add("ItemMouseOverBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region ItemNormalForeground
        public static SolidColorBrush ItemNormalForeground
        {
            get
            {
                if (Info.ContainsKey("ItemNormalForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemNormalForeground"]);
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
                    if (Info.ContainsKey("ItemNormalForeground")) Info["ItemNormalForeground"] = value.Color.ToString();
                    else Info.Add("ItemNormalForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region ItemNormalBorderBrush
        public static SolidColorBrush ItemNormalBorderBrush
        {
            get
            {
                if (Info.ContainsKey("ItemNormalBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemNormalBorderBrush"]);
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
                    if (Info.ContainsKey("ItemNormalBorderBrush")) Info["ItemNormalBorderBrush"] = value.Color.ToString();
                    else Info.Add("ItemNormalBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region ItemSelectedForeground
        public static SolidColorBrush ItemSelectedForeground
        {
            get
            {
                if (Info.ContainsKey("ItemSelectedForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemSelectedForeground"]);
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
                    if (Info.ContainsKey("ItemSelectedForeground")) Info["ItemSelectedForeground"] = value.Color.ToString();
                    else Info.Add("ItemSelectedForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #endregion

        #region Header项
        #region ItemExpandForeground
        public static SolidColorBrush ItemExpandForeground
        {
            get
            {
                if (Info.ContainsKey("ItemExpandForeground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemExpandForeground"]);
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
                    if (Info.ContainsKey("ItemExpandForeground")) Info["ItemExpandForeground"] = value.Color.ToString();
                    else Info.Add("ItemExpandForeground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion
        #region ItemExpandBackground
        public static SolidColorBrush ItemExpandBackground
        {
            get
            {
                if (Info.ContainsKey("ItemExpandBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["ItemExpandBackground"]);
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
                    if (Info.ContainsKey("ItemExpandBackground")) Info["ItemExpandBackground"] = value.Color.ToString();
                    else Info.Add("ItemExpandBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region HeaderNormalBackground
        public static SolidColorBrush HeaderNormalBackground
        {
            get
            {
                if (Info.ContainsKey("HeaderNormalBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["HeaderNormalBackground"]);
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
                    if (Info.ContainsKey("HeaderNormalBackground")) Info["HeaderNormalBackground"] = value.Color.ToString();
                    else Info.Add("HeaderNormalBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion
        #region HeaderMouseOverBackground
        public static SolidColorBrush HeaderMouseOverBackground
        {
            get
            {
                if (Info.ContainsKey("HeaderMouseOverBackground"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["HeaderMouseOverBackground"]);
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
                    if (Info.ContainsKey("HeaderMouseOverBackground")) Info["HeaderMouseOverBackground"] = value.Color.ToString();
                    else Info.Add("HeaderMouseOverBackground", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion
        #endregion

        #region Calendar

        #region CalendarBackgrounBrush
        public static SolidColorBrush CalendarBackgrounBrush
        {
            get
            {
                if (Info.ContainsKey("CalendarBackgrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["CalendarBackgrounBrush"]);
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
                    if (Info.ContainsKey("CalendarBackgrounBrush")) Info["CalendarBackgrounBrush"] = value.Color.ToString();
                    else Info.Add("CalendarBackgrounBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region CalendarBorderBrush
        public static SolidColorBrush CalendarBorderBrush
        {
            get
            {
                if (Info.ContainsKey("CalendarBorderBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["CalendarBorderBrush"]);
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
                    if (Info.ContainsKey("CalendarBorderBrush")) Info["CalendarBorderBrush"] = value.Color.ToString();
                    else Info.Add("CalendarBorderBrush", value.Color.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region CalendarBorderThickness
        public static Thickness CalendarBorderThickness //= new Thickness(1d);
        {
            get
            {
                if (Info.ContainsKey("CalendarBorderThickness"))
                {
                    try
                    {
                        var convertFromString = Double.Parse(Info["CalendarBorderThickness"]);
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
                    if (Info.ContainsKey("CalendarBorderThickness")) Info["CalendarBorderThickness"] = value.Left.ToString();
                    else Info.Add("CalendarBorderThickness", value.Left.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #endregion

        #region DatePickerButton
        
       #region DatePickerButtonBackgrounBrush
        public static Color DatePickerButtonBackgrounBrush
        {
            get
            {
                if (Info.ContainsKey("DatePickerButtonBackgrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DatePickerButtonBackgrounBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return cl;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return Color.FromArgb(0x0, 0xFF, 0xFF, 0xFF);

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DatePickerButtonBackgrounBrush")) Info["DatePickerButtonBackgrounBrush"] = value.ToString();
                    else Info.Add("DatePickerButtonBackgrounBrush", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion
            
       #region DatePickerButtonInUpBackgrounBrush
        public static Color DatePickerButtonInUpBackgrounBrush
        {
            get
            {
                if (Info.ContainsKey("DatePickerButtonInUpBackgrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DatePickerButtonInUpBackgrounBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return cl;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return Color.FromArgb(0x0, 0xFF, 0xFF, 0xFF);

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DatePickerButtonInUpBackgrounBrush")) Info["DatePickerButtonInUpBackgrounBrush"] = value.ToString();
                    else Info.Add("DatePickerButtonInUpBackgrounBrush", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        #region DatePickerButtonForegrounBrush
        public static Color DatePickerButtonForegrounBrush
        {
            get
            {
                if (Info.ContainsKey("DatePickerButtonForegrounBrush"))
                {
                    try
                    {
                        var convertFromString = ColorConverter.ConvertFromString(Info["DatePickerButtonForegrounBrush"]);
                        if (convertFromString != null)
                        {
                            var cl = (Color)convertFromString;
                            return cl;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return Color.FromArgb(0x0, 0xFF, 0xFF, 0xFF);

            }
            set
            {
                try
                {
                    if (Info.ContainsKey("DatePickerButtonForegrounBrush")) Info["DatePickerButtonForegrounBrush"] = value.ToString();
                    else Info.Add("DatePickerButtonForegrounBrush", value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion
        #endregion

        #endregion

        #region Attach Propory

        #region 内容面板及其边框

        #region BorderThickness

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI]
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(DatePickerAttriXaml),
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

        #region NormalBackgrounBrush

        [PublicAPI]
        public static DependencyProperty NormalBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("NormalBackgrounBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
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
            DependencyProperty.RegisterAttached("NormalBorderBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
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

        #region NormalForeground
        [PublicAPI]
        public static DependencyProperty NormalForegroundProperty =
            DependencyProperty.RegisterAttached("NormalForeground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
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

        #region DisableBackgrounBrush

        [PublicAPI]
        public static DependencyProperty DisableBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("DisableBackgrounBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBackgrounBrush,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(DisableBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetDisableBackgrounBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableBackgrounBrushProperty, value);
        }

        #endregion

        #region DisableBorderBrush

        [PublicAPI]
        public static DependencyProperty DisableBorderBrushProperty =
            DependencyProperty.RegisterAttached("DisableBorderBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(NormalBorderBrush,
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

        #region DisableForeground

        [PublicAPI]
        public static DependencyProperty DisableForegroundProperty =
            DependencyProperty.RegisterAttached("DisableForeground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(DisableForeground,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsRender |
                                                                    FrameworkPropertyMetadataOptions.
                                                                        Inherits));

        [PublicAPI]
        public static SolidColorBrush GetDisableForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(DisableForegroundProperty);
        }

        [PublicAPI]
        public static void SetDisableForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DisableForegroundProperty, value);
        }

        #endregion
        

        #endregion

        #region Item项

        #region ItemNormalBackground
        [PublicAPI]
        public static DependencyProperty ItemNormalBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemNormalBackground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemNormalBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemNormalBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemNormalBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemNormalBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemNormalBackgroundProperty, value);
        }
        #endregion



        #region ItemMouseOverBorderBrush
        [PublicAPI]
        public static DependencyProperty ItemMouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("ItemMouseOverBorderBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemMouseOverBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemMouseOverBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemMouseOverBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetItemMouseOverBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemMouseOverBorderBrushProperty, value);
        }
        #endregion

        #region ItemBorderThickness

        [PublicAPI]
        public static readonly DependencyProperty ItemBorderThicknessProperty =
            DependencyProperty.RegisterAttached("ItemBorderThickness", typeof(Thickness), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemBorderThickness,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsArrange |
                                                                  FrameworkPropertyMetadataOptions.Inherits,
                                                                  null, ThicknessUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetItemBorderThickness([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<Thickness>.Unbox(obj.GetValue(ItemBorderThicknessProperty));
        }

        [PublicAPI]
        public static void SetItemBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemBorderThicknessProperty, value);
        }


        #endregion


        #region ItemMouseOverBackground
        [PublicAPI]
        public static DependencyProperty ItemMouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemMouseOverBackground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemMouseOverBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemMouseOverBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemMouseOverBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemMouseOverBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemMouseOverBackgroundProperty, value);
        }
        #endregion

        #region ItemNormalForeground
        [PublicAPI]
        public static DependencyProperty ItemNormalForegroundProperty =
            DependencyProperty.RegisterAttached("ItemNormalForeground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemNormalForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemNormalForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemNormalForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemNormalForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemNormalForegroundProperty, value);
        }
        #endregion

        #region ItemSelectedBackground
        [PublicAPI]
        public static DependencyProperty ItemNormalBorderBrushProperty =
            DependencyProperty.RegisterAttached("ItemNormalBorderBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemNormalBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemNormalBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemNormalBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetItemNormalBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemNormalBorderBrushProperty, value);
        }
        #endregion

        #region ItemSelectedForeground
        [PublicAPI]
        public static DependencyProperty ItemSelectedForegroundProperty =
            DependencyProperty.RegisterAttached("ItemSelectedForeground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemSelectedForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemSelectedForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemSelectedForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemSelectedForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemSelectedForegroundProperty, value);
        }
        #endregion

        #endregion

        #region Header项
        #region ItemExpandBackground
        [PublicAPI]
        public static DependencyProperty ItemExpandBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemExpandBackground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemExpandBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemExpandBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemExpandBackgroundProperty);
        }

        [PublicAPI]
        public static void SetItemExpandBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemExpandBackgroundProperty, value);
        }
        #endregion
        #region ItemExpandForeground
        [PublicAPI]
        public static DependencyProperty ItemExpandForegroundProperty =
            DependencyProperty.RegisterAttached("ItemExpandForeground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(ItemExpandForeground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetItemExpandForeground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ItemExpandForegroundProperty);
        }

        [PublicAPI]
        public static void SetItemExpandForeground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ItemExpandForegroundProperty, value);
        }
        #endregion

        #region HeaderNormalBackground
        [PublicAPI]
        public static DependencyProperty HeaderNormalBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderNormalBackground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderNormalBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderNormalBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderNormalBackgroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderNormalBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderNormalBackgroundProperty, value);
        }
        #endregion
        #region HeaderMouseOverBackground
        [PublicAPI]
        public static DependencyProperty HeaderMouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderMouseOverBackground", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(HeaderMouseOverBackground,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetHeaderMouseOverBackground([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(HeaderMouseOverBackgroundProperty);
        }

        [PublicAPI]
        public static void SetHeaderMouseOverBackground([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderMouseOverBackgroundProperty, value);
        }
        #endregion
        #endregion

        #region Calendar

        #region CalendarBackgrounBrush
        [PublicAPI]
        public static DependencyProperty CalendarBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("CalendarBackgrounBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(CalendarBackgrounBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetCalendarBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(CalendarBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetCalendarBackgrounBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(CalendarBackgrounBrushProperty, value);
        }
        #endregion


        #region CalendarBorderBrush
        [PublicAPI]
        public static DependencyProperty CalendarBorderBrushProperty =
            DependencyProperty.RegisterAttached("CalendarBorderBrush", typeof(SolidColorBrush), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(CalendarBorderBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static SolidColorBrush GetCalendarBorderBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(CalendarBorderBrushProperty);
        }

        [PublicAPI]
        public static void SetCalendarBorderBrush([NotNull] DependencyObject obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(CalendarBorderBrushProperty, value);
        }
        #endregion

        #region CalendarBorderThickness

        /// <summary>
        /// Button边框厚度
        /// </summary>
        [PublicAPI]
        public static readonly DependencyProperty CalendarBorderThicknessProperty =
            DependencyProperty.RegisterAttached("CalendarBorderThickness", typeof(Thickness), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(CalendarBorderThickness,
                                                                    FrameworkPropertyMetadataOptions.
                                                                        AffectsArrange |
                                                                    FrameworkPropertyMetadataOptions.Inherits,
                                                                    null, ThicknessUtil.CoerceNonNegative));

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static Thickness GetCalendarBorderThickness([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ThicknessUtil.EnsureNonNegative();
            return BoxingHelper<Thickness>.Unbox(obj.GetValue(CalendarBorderThicknessProperty));
        }

        [PublicAPI]
        public static void SetCalendarBorderThickness([NotNull] DependencyObject obj, Thickness value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(CalendarBorderThicknessProperty, value);
        }

        #endregion

        #endregion

        #region DatePickerButton

        #region DatePickerButtonBackgrounBrush
        [PublicAPI]
        public static DependencyProperty DatePickerButtonBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("DatePickerButtonBackgrounBrush", typeof(Color), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(DatePickerButtonBackgrounBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static Color GetDatePickerButtonBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (Color)obj.GetValue(DatePickerButtonBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetDatePickerButtonBackgrounBrush([NotNull] DependencyObject obj, Color value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DatePickerButtonBackgrounBrushProperty, value);
        }
        #endregion

        #region DatePickerButtonInUpBackgrounBrush
        [PublicAPI]
        public static DependencyProperty DatePickerButtonInUpBackgrounBrushProperty =
            DependencyProperty.RegisterAttached("DatePickerButtonInUpBackgrounBrush", typeof(Color), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(DatePickerButtonInUpBackgrounBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static Color GetDatePickerButtonInUpBackgrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (Color)obj.GetValue(DatePickerButtonInUpBackgrounBrushProperty);
        }

        [PublicAPI]
        public static void SetDatePickerButtonInUpBackgrounBrush([NotNull] DependencyObject obj, Color value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DatePickerButtonInUpBackgrounBrushProperty, value);
        }
        #endregion

        #region DatePickerButtonForegrounBrush
        [PublicAPI]
        public static DependencyProperty DatePickerButtonForegrounBrushProperty =
            DependencyProperty.RegisterAttached("DatePickerButtonForegrounBrush", typeof(Color), typeof(DatePickerAttriXaml),
                                    new FrameworkPropertyMetadata(DatePickerButtonForegrounBrush,
                                                                  FrameworkPropertyMetadataOptions.
                                                                      AffectsRender |
                                                                  FrameworkPropertyMetadataOptions.
                                                                      Inherits));

        [PublicAPI]
        public static Color GetDatePickerButtonForegrounBrush([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (Color)obj.GetValue(DatePickerButtonForegrounBrushProperty);
        }

        [PublicAPI]
        public static void SetDatePickerButtonForegrounBrush([NotNull] DependencyObject obj, Color value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(DatePickerButtonForegrounBrushProperty, value);
        }
        #endregion

        #endregion

        #endregion
    }
}
