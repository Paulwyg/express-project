using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Elysium.ControlsOverride.TextBoxOverride
{

    public class TextBoxWaterMarked : TextBox
    {
        private TextBlock _waterMarkLable;

        // private ScrollViewer wateMarkScrollViewer;

        static TextBoxWaterMarked()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxWaterMarked),
                                                     new FrameworkPropertyMetadata(typeof(TextBoxWaterMarked)));
        }


        public TextBoxWaterMarked()
        {

            this.Loaded += new RoutedEventHandler(PerfectWateMarkTextBoxLoaded);
            this.TextChanged += new TextChangedEventHandler(PerfectWateMarkTextBoxTextChanged);
        }

       

        private void PerfectWateMarkTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            if (this._waterMarkLable != null)
            {
                this._waterMarkLable.Text  = WaterMark;
                this._waterMarkLable.Foreground = WaterMarkForeground;
                this._waterMarkLable.Visibility = Visibility.Hidden;
            }
           
        }

        private void PerfectWateMarkTextBoxTextChanged(object sender, TextChangedEventArgs e)
        { 
            if (this._waterMarkLable == null) return;
            if (this.Text == null)
            {
                this._waterMarkLable.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrEmpty(this.Text))
            {
                this._waterMarkLable.Visibility = Visibility.Visible;
                return;
            }
          this._waterMarkLable.Visibility = Visibility.Hidden;
        }


        public static readonly DependencyProperty PropertyWaterMarkForeground =
         DependencyProperty.Register("WaterMarkForeground", typeof(SolidColorBrush), //#FF707070
         typeof(TextBoxWaterMarked), new PropertyMetadata(new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xc0, 0xc0, 0xc0))));

        public SolidColorBrush WaterMarkForeground
        {
            get { return (SolidColorBrush)GetValue(PropertyWaterMarkForeground); }
            set { SetValue(PropertyWaterMarkForeground, value); }
        }


        public string WaterMark
        {
            get { return (string) GetValue(WaterMarkProperty); }

            set { SetValue(WaterMarkProperty, value); }
        }

        public static DependencyProperty WaterMarkProperty =
            DependencyProperty.Register("WaterMark", typeof(string), typeof(TextBoxWaterMarked),
                                        new UIPropertyMetadata("需要输入哟"));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._waterMarkLable = this.GetTemplateChild("TextPrompt") as TextBlock;

            //   this.wateMarkScrollViewer = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
        }
    }
}
