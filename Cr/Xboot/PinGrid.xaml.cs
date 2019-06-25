using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Xboot
{
   

    /// <summary>
    /// PinGrid.xaml 的交互逻辑
    /// </summary>
    public partial class PinGrid : Grid
    {

        public PinGrid()
        {
            InitializeComponent();
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(PinOverride_DataContextChanged);

        }


        protected  const int zFontSize = 30;
        private Button _btnButton = null;
        private string _btnName = "btnName";

        private void PinOverride_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _btnButton = null;
            try
            {
                bool find = false;
                for (int i = 0; i < this.RowDefinitions.Count; i++)
                {
                    // 这个方法是循环Grid下所有控件的方法
                    //FindName 方法只能查询在XAML中定义的组件，后台动态添加的需要手动写循环来处理
                    for (int j = 0; j < this.Children.Count; j++)
                    {
                        var border = this.Children[i] as Button;
                        if (border != null && border.Name == _btnName)
                        {

                            this.Children.Remove(border);
                            //border.Click -= new RoutedEventHandler(_btnButton_Click);
                            find = true;
                            break;
                        }
                    }
                    if (find) break;
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                _btnButton = new Button();
                _btnButton.Name = _btnName;
                _btnButton.Content = "Pin";
                _btnButton.FontSize = zFontSize ;
                _btnButton.FontWeight = FontWeights.Bold;
                
                _btnButton.Foreground=new SolidColorBrush(Colors.Red);
               // _btnButton.Background = new SolidColorBrush(Colors.Transparent);
                _btnButton.Width = 35;
                _btnButton.Height = 22;

                _btnButton.Click += new RoutedEventHandler(_btnButton_Click);
                _btnButton.MouseEnter += _btnButton_Enter;
                _btnButton.MouseLeave += _btnButton_Leave;


                if (this.PinHorizontalAlignment == HorizontalAlignment.Left)
                {
                    _btnButton.SetValue(Grid.ColumnProperty, 0);
                    _btnButton.HorizontalAlignment = HorizontalAlignment.Left;

                }
                else
                {
                    _btnButton.SetValue(Grid.RowProperty,
                                    ColumnDefinitions.Count - 1 < 0 ? 0 : ColumnDefinitions.Count - 1);

                    _btnButton.HorizontalAlignment = HorizontalAlignment.Right;
                }
                if (this.PinVerticalAlignment == VerticalAlignment.Bottom)
                {
                    _btnButton.SetValue(Grid.RowProperty,
                                            RowDefinitions.Count - 1 < 0 ? 0 : RowDefinitions.Count - 1);

                    _btnButton.VerticalAlignment = VerticalAlignment.Bottom;
                }
                else
                {
                    _btnButton.SetValue(Grid.RowProperty, 0);

                    _btnButton.VerticalAlignment = VerticalAlignment.Top;
                }
                _btnButton.Margin = this.PinMargin;
                if (_initvisi == Visibility.Visible)
                {
                    _btnButton.Content = this.PinHideContent;
                }
                else
                {
                    _btnButton.Content = this.PinShowContent;
                }
                _btnButton.BorderThickness = new Thickness(0);
                _btnButton.Background = new SolidColorBrush(Colors.Transparent ); 

                this.Children.Add(_btnButton);
            }
            catch (Exception ex)
            {

            }
        }

        private Visibility _initvisi = Visibility.Visible;
        private Brush _backGroundBrush = null;
        private HorizontalAlignment _hor;
        private VerticalAlignment _ver;
    //    private bool _isfirst = true;
        private void _btnButton_Click(object sender, RoutedEventArgs e)
        {
           
            //if (_initvisi == Visibility.Visible)
            //{
            //    _backGroundBrush = this.Background;
            //    _initvisi = Visibility.Collapsed;
            //    _btnButton.Content = this.PinShowContent;
            //    this.Background = new SolidColorBrush(Colors.Transparent);

            //    _hor = this.HorizontalAlignment;
            //    _ver = this.VerticalAlignment;
            //    this.HorizontalAlignment =  PinHorizontalAlignment;
            //    this.VerticalAlignment = PinVerticalAlignment;
            //}
            //else
            //{
            //    _initvisi = Visibility.Visible;
            //    _btnButton.Content = this.PinHideContent;
            //    this.Background = _backGroundBrush;
            //    this.HorizontalAlignment = _hor;
            //    this.VerticalAlignment = _ver;
            //}
            //for (int i = 0; i < this.RowDefinitions.Count; i++)
            //{
            // 这个方法是循环Grid下所有控件的方法
            //FindName 方法只能查询在XAML中定义的组件，后台动态添加的需要手动写循环来处理
            //for (int j = 0; j < this.Children.Count; j++)
            //{
            //    var border = this.Children[j] as ContentControl;
            //    if (border != null && border.Name == _btnName)
            //    {
            //        //border.Visibility = Visibility.Collapsed;
            //    }
            //    else if (this.Children[j] != null)
            //        this.Children[j].Visibility = _initvisi;

            //}
            this.PinVisibility = Visibility.Collapsed;


        }

        private void _btnButton_Enter(object sender,RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            btn.Content = "Ｘ";
            btn.FontSize = zFontSize;

        }

        private void _btnButton_Leave(object sender,RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            btn.Content = "ｘ";
            btn.FontSize = zFontSize;
          // btn.Background = new SolidColorBrush(Colors.Tomato);
        }


        #region PinHorizontalAlignment
        public static readonly DependencyProperty PropertyPinHorizontalAlignment =
           DependencyProperty.Register("PinHorizontalAlignment", typeof(HorizontalAlignment),
                                       typeof(PinGrid), new PropertyMetadata(HorizontalAlignment.Right, new PropertyChangedCallback(PinHorizontalAlignmentPropertyChangedCallback)));

        public HorizontalAlignment PinHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(PropertyPinHorizontalAlignment); }
            set { SetValue(PropertyPinHorizontalAlignment, value); }
        }


        static void PinHorizontalAlignmentPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tmp = (sender as PinGrid);
            if (tmp == null) return;
            //border.Opacity =1 - Convert.ToDouble(e.NewValue) / 255;
            try
            {
                if (tmp._btnButton == null) return;
                var xxx = (HorizontalAlignment)e.NewValue;
                //  if (xxx == null) return;
                if (xxx == HorizontalAlignment.Left)
                {
                    tmp._btnButton.SetValue(Grid.ColumnProperty, 0);
                    tmp._btnButton.HorizontalAlignment = HorizontalAlignment.Left;

                }
                else
                {
                    tmp._btnButton.SetValue(Grid.RowProperty,
                                    tmp.ColumnDefinitions.Count - 1 < 0 ? 0 : tmp.ColumnDefinitions.Count - 1);

                    tmp._btnButton.HorizontalAlignment = HorizontalAlignment.Right;
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion


        #region PinVerticalAlignment
        public static readonly DependencyProperty PropertyPinVerticalAlignment =
           DependencyProperty.Register("PinVerticalAlignment", typeof(VerticalAlignment),
                                       typeof(PinGrid), new PropertyMetadata(VerticalAlignment.Top, new PropertyChangedCallback(PinVerticalAlignmentPropertyChangedCallback)));

        public VerticalAlignment PinVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(PropertyPinVerticalAlignment); }
            set { SetValue(PropertyPinVerticalAlignment, value); }
        }

        static void PinVerticalAlignmentPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tmp = (sender as PinGrid);
            if (tmp == null) return;
            //border.Opacity =1 - Convert.ToDouble(e.NewValue) / 255;
            try
            {
                if (tmp._btnButton == null) return;
                var xxx = (VerticalAlignment)e.NewValue;
                //  if (xxx == null) return;
                if (xxx == VerticalAlignment.Bottom)
                {
                    tmp._btnButton.SetValue(Grid.RowProperty,
                                            tmp.RowDefinitions.Count - 1 < 0 ? 0 : tmp.RowDefinitions.Count - 1);

                    tmp._btnButton.VerticalAlignment = VerticalAlignment.Bottom;
                }
                else
                {
                    tmp._btnButton.SetValue(Grid.RowProperty, 0);

                    tmp._btnButton.VerticalAlignment = VerticalAlignment.Top;
                }
            }
            catch (Exception ex)
            {
            }

        }

        #endregion



        #region PinMargin
        public static readonly DependencyProperty PropertyPinMargin =
           DependencyProperty.Register("PinMargin", typeof(Thickness),
                                       typeof(PinGrid), new PropertyMetadata(new Thickness(0, 0, 0, 0), new PropertyChangedCallback(PinMarginPropertyChangedCallback)));

        public Thickness PinMargin
        {
            get { return (Thickness)GetValue(PropertyPinMargin); }
            set { SetValue(PropertyPinMargin, value); }
        }

        static void PinMarginPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tmp = (sender as PinGrid);
            if (tmp == null) return;
            //border.Opacity =1 - Convert.ToDouble(e.NewValue) / 255;
            try
            {
                if (tmp._btnButton == null) return;
                var xxx = (Thickness)e.NewValue;
                tmp.Margin = xxx;

            }
            catch (Exception ex)
            {
            }

        }

        #endregion


        #region PinHideContent
        public static readonly DependencyProperty PropertyPinHideContent =
           DependencyProperty.Register("PinHideContent", typeof(object),
                                       typeof(PinGrid), new PropertyMetadata("Hide", new PropertyChangedCallback(PinPropertyPinHideContentPropertyChangedCallback)));

        public object PinHideContent
        {
            get { return (object)GetValue(PropertyPinHideContent); }
            set { SetValue(PropertyPinHideContent, value); }
        }

        static void PinPropertyPinHideContentPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tmp = (sender as PinGrid);
            if (tmp == null) return;
            //border.Opacity =1 - Convert.ToDouble(e.NewValue) / 255;
            try
            {
                if (tmp._btnButton == null) return;
                var xxx = (object)e.NewValue;
                if (tmp._initvisi == Visibility.Collapsed)
                {
                    tmp._btnButton.Content = xxx;
                }


            }
            catch (Exception ex)
            {
            }

        }

        #endregion



        #region PinShowContent
        public static readonly DependencyProperty PropertyPinShowContent =
           DependencyProperty.Register("PinShowContent", typeof(object),
                                       typeof(PinGrid), new PropertyMetadata("Show", new PropertyChangedCallback(PinPropertyPinShowContentPropertyChangedCallback)));

        public object PinShowContent
        {
            get { return (object)GetValue(PropertyPinShowContent); }
            set { SetValue(PropertyPinShowContent, value); }
        }

        static void PinPropertyPinShowContentPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tmp = (sender as PinGrid);
            if (tmp == null) return;
            //border.Opacity =1 - Convert.ToDouble(e.NewValue) / 255;
            try
            {
                if (tmp._btnButton == null) return;
                var xxx = (object)e.NewValue;
                if (tmp._initvisi == Visibility.Visible)
                {
                    tmp._btnButton.Content = xxx;
                }


            }
            catch (Exception ex)
            {
            }

        }

        #endregion

        #region PinVisibility
        public static readonly DependencyProperty PropertyPinVisibility =
   DependencyProperty.Register("PinVisibility", typeof(object),
                               typeof(PinGrid), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(PinPropertyPropertyPinVisibilityPropertyChangedCallback)));

        public object PinVisibility
        {
            get { return (object)GetValue(PropertyPinVisibility); }
            set { SetValue(PropertyPinVisibility, value); }
        }

        static void PinPropertyPropertyPinVisibilityPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tmp = (sender as PinGrid);
            if (tmp == null) return;
            //border.Opacity =1 - Convert.ToDouble(e.NewValue) / 255;
            try
            {
                if (tmp._btnButton == null) return;
                var xxx = (object)e.NewValue;
                tmp.Visibility = (Visibility)xxx;


            }
            catch (Exception ex)
            {
            }

        }
        #endregion

    }

}
