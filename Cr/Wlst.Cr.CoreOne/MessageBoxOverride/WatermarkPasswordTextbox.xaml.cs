using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wlst.Cr.CoreOne.MessageBoxOverride
{
    /// <summary>
    /// WatermarkPasswordTextbox.xaml 的交互逻辑
    /// </summary>
    public partial class WatermarkPasswordTextbox : UserControl
    {
        public static readonly DependencyProperty PropertyWatermarkContent =
            DependencyProperty.Register("WatermarkContent", typeof (string),
                                        typeof (WatermarkPasswordTextbox), new PropertyMetadata(""));

        public string WatermarkContent
        {
            get { return (string) GetValue(PropertyWatermarkContent); }
            set { SetValue(PropertyWatermarkContent, value); }
        }

        public static readonly DependencyProperty PropertyCurrentText =
            DependencyProperty.Register("CurrentText", typeof (string),
                                        typeof (WatermarkPasswordTextbox), new PropertyMetadata(null));

        public string CurrentText
        {
            get { return (string) GetValue(PropertyCurrentText); }
            set { SetValue(PropertyCurrentText, value); }
        }

        public static readonly DependencyProperty PropertyIsWatermarkVisible =
            DependencyProperty.Register("IsWatermarkVisible", typeof (System.Windows.Visibility),
                                        typeof (WatermarkPasswordTextbox),
                                        new PropertyMetadata(System.Windows.Visibility.Visible));

        public System.Windows.Visibility IsWatermarkVisible
        {
            get { return (System.Windows.Visibility) GetValue(PropertyIsWatermarkVisible); }
            set { SetValue(PropertyIsWatermarkVisible, value); }
        }

        public static readonly DependencyProperty PropertyWatermarkForeground =
            DependencyProperty.Register("WatermarkForeground", typeof (Brush),
                                        typeof (WatermarkPasswordTextbox),
                                        new PropertyMetadata(
                                            new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x4C, 0x00, 0x00,
                                                                                                    0x00))));

        // [Category("Common Properties")]
        public Brush WatermarkForeground
        {
            get { return (Brush) GetValue(PropertyWatermarkForeground); }
            set { SetValue(PropertyWatermarkForeground, value); }
        }

        public static readonly DependencyProperty PropertyBackground =
            DependencyProperty.Register("Fill", typeof (Brush),
                                        typeof (WatermarkPasswordTextbox),
                                        new PropertyMetadata(
                                            new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0xFF,
                                                                                                    0xFF))));

        public Brush Fill
        {
            get { return (Brush) GetValue(PropertyBackground); }
            set { SetValue(PropertyBackground, value); }
        }

        public static readonly DependencyProperty PropertyTextForeground =
            DependencyProperty.Register("TextForeground", typeof (Brush),
                                        typeof (WatermarkPasswordTextbox),
                                        new PropertyMetadata(
                                            new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x00, 0x00,
                                                                                                    0x00))));

        public Brush TextForeground
        {
            get { return (Brush) GetValue(PropertyTextForeground); }
            set { SetValue(PropertyTextForeground, value); }
        }

        public static readonly DependencyProperty PropertyFontFamily =
            DependencyProperty.Register("FontFamily", typeof (FontFamily),
                                        typeof (WatermarkPasswordTextbox), new PropertyMetadata(new FontFamily("Arial")));

        public new FontFamily FontFamily
        {
            get { return (FontFamily) GetValue(PropertyFontFamily); }
            set { SetValue(PropertyFontFamily, value); }
        }

        public static readonly DependencyProperty PropertyFontSize =
            DependencyProperty.Register("FontSize", typeof (double),
                                        typeof (WatermarkPasswordTextbox), new PropertyMetadata((double) 16));

        public new double FontSize
        {
            get { return ((double) GetValue(PropertyFontSize) == 0) ? 16 : (double) GetValue(PropertyFontSize); }
            set { SetValue(PropertyFontSize, value); }
        }

        public static readonly DependencyProperty PropertyRemoverTooltip =
            DependencyProperty.Register("RemoverTooltip", typeof (string),
                                        typeof (WatermarkPasswordTextbox), new PropertyMetadata("Clear"));

        public string RemoverTooltip
        {
            get { return (string) GetValue(PropertyRemoverTooltip); }
            set { SetValue(PropertyRemoverTooltip, value); }
        }

        public WatermarkPasswordTextbox()
        {
            this.InitializeComponent();
        }

        private void EvtRemoveInputText(object sender, System.Windows.RoutedEventArgs e)
        {
            this.InputText.Password = string.Empty;
            this.InputText.Focus();
        }

        private void EvtInputTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (this.InputText.Password == string.Empty)
            {
                if (IsWatermarkVisible != System.Windows.Visibility.Hidden)
                {
                    this.Watermark.Visibility = System.Windows.Visibility.Visible;
                }
                this.Remover.Visibility = System.Windows.Visibility.Hidden;
                this.InputText.Margin = new Thickness(0);
            }
            else
            {
                if (IsWatermarkVisible != System.Windows.Visibility.Hidden)
                {
                    this.Watermark.Visibility = System.Windows.Visibility.Hidden;
                }
                this.Remover.Visibility = System.Windows.Visibility.Visible;
                this.InputText.Margin = new Thickness(0, 0, 24, 0);
            }
        }

        private void SetBaseBackgroundStrokeColor(Color color)
        {
            SolidColorBrush scb = new SolidColorBrush();
            scb.Color = color;
            this.BaseBackground.Stroke = scb;
        }

        private void EvtInputTextMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!this.InputText.IsFocused)
            {
                VisualStateManager.GoToElementState(this.UCBase, "StateInputTextOnMouseEnter", true);
                // SetBaseBackgroundStrokeColor(Colors.DarkGray);
            }
        }

        private void EvtInputTextMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!this.InputText.IsFocused)
            {
                VisualStateManager.GoToElementState(this.UCBase, "StateInputTextOnMouseLeave", true);
                // SetBaseBackgroundStrokeColor(Colors.Transparent);
            }
        }

        private void EvtInputTextLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(this.UCBase, "StateInputTextOnMouseLeave", true);
            // SetBaseBackgroundStrokeColor(Colors.Transparent);
        }

        private void EvtInputTextGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(this.UCBase, "StateInputTextOnFocused", true);
            // SetBaseBackgroundStrokeColor(Color.FromArgb(255,0x33,0x33,0x33));
        }

        private void InputText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.InputText.Password == string.Empty)
            {
                if (IsWatermarkVisible != System.Windows.Visibility.Hidden)
                {
                    this.Watermark.Visibility = System.Windows.Visibility.Visible;
                }
                this.Remover.Visibility = System.Windows.Visibility.Hidden;
                this.InputText.Margin = new Thickness(0);
            }
            else
            {
                if (IsWatermarkVisible != System.Windows.Visibility.Hidden)
                {
                    this.Watermark.Visibility = System.Windows.Visibility.Hidden;
                }
                this.Remover.Visibility = System.Windows.Visibility.Visible;
                this.InputText.Margin = new Thickness(0, 0, 24, 0);
            }
        }
    }
}