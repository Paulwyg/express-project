using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
#if NETFX45
using System.Windows.Shell;
#endif
using System.Windows.Threading;

using Elysium.Extensions;
using Elysium.Native;

using JetBrains.Annotations;

#if NETFX4
using Microsoft.Windows.Shell;
#endif
using Microsoft.Windows.Shell;
using Monitor = Elysium.Native.Monitor;

namespace Elysium.Controls
{
    [PublicAPI]
    [TemplatePart(Name = LayoutRootName, Type = typeof(Panel))]
    [TemplatePart(Name = CaptionName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TitleName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ProgressBarName, Type = typeof(ProgressBar))]
    [TemplatePart(Name = MinimizeName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MaximizeName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = RestoreName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = CloseName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = GripName, Type = typeof(ResizeGrip))]
    public class Window : System.Windows.Window
    {
        private const string LayoutRootName = "PART_LayoutRoot";
        private const string TitleName = "PART_Title";
        private const string CaptionName = "PART_Caption";
        private const string ProgressBarName = "PART_ProgressBar";
        private const string MinimizeName = "PART_Minimize";
        private const string MaximizeName = "PART_Maximize";
        private const string RestoreName = "PART_Restore";
        private const string CloseName = "PART_Close";
        private const string GripName = "PART_Grip";

        private FrameworkElement _caption;
        private Panel _layoutRoot;

        private Monitor _monitor;
        private Native.Window _window;

        [SecurityCritical]
        private WindowChrome _chrome;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "We need to use static constructor for custom actions during dependency properties initialization")]
        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
            WindowStateProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(OnWindowStateChanged));
        }

        [SecuritySafeCritical]
        public Window()
        {
            Initialize();

            CommandBindings.Add(new CommandBinding(WindowCommands.Minimize, (sender, e) => WindowState = WindowState.Minimized));
            CommandBindings.Add(new CommandBinding(WindowCommands.Maximize, (sender, e) => WindowState = WindowState.Maximized));
            CommandBindings.Add(new CommandBinding(WindowCommands.Restore, (sender, e) => WindowState = WindowState.Normal));
            CommandBindings.Add(new CommandBinding(WindowCommands.Close, (sender, e) => Close()));
        }

#if NETFX4
        [SuppressMessage("Microsoft.Security", "CA2141:TransparentMethodsMustNotSatisfyLinkDemandsFxCopRule",
            Justification = "We need to use Microsoft.Windows.Shell.dll version 3.5")]
#endif
        [SecurityCritical]
        private void Initialize()
        {
#if NETFX4
            // NOTE: Lack of contracts: SystemParameters2.Current must ensure non-null value
            Contract.Assume(SystemParameters2.Current != null);
#endif

            _chrome = new WindowChrome
                          {
#if NETFX4
                              CaptionHeight = SystemParameters2.Current.WindowCaptionHeight,
#elif NETFX45
                              CaptionHeight = SystemParameters.WindowCaptionHeight,
#endif
                              CornerRadius = new CornerRadius(0d),
                              GlassFrameThickness = new Thickness(0d),
                              NonClientFrameEdges = NonClientFrameEdges.None,
                              ResizeBorderThickness = Parameters.Window.GetResizeBorderThickness(this),
                              UseAeroCaptionButtons = false
                          };
            _chrome.TryFreeze();
            if (WindowChrome.GetWindowChrome(this) == null)
            {
                WindowChrome.SetWindowChrome(this, _chrome);
            }

            Initialized += OnInitializedInternal;
            Loaded += OnLoadedInternal;

            var resizeBorderThicknessPropertyDescriptor =
                DependencyPropertyDescriptor.FromProperty(Parameters.Window.ResizeBorderThicknessProperty, typeof(Window));
            if (resizeBorderThicknessPropertyDescriptor != null)
            {
                resizeBorderThicknessPropertyDescriptor.AddValueChanged(this, OnResizeBorderThicknessChanged);
            }
        }

        private SizeToContent _previousSizeToContent = SizeToContent.Manual;

        [SecurityCritical]
        private void OnInitializedInternal(object sender, EventArgs e)
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome) && SizeToContent != SizeToContent.Manual)
            {
                _previousSizeToContent = SizeToContent;
                SizeToContent = SizeToContent.Manual;
            }
        }

        [SecurityCritical]
        private void OnLoadedInternal(object sender, RoutedEventArgs e)
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome) && _previousSizeToContent != SizeToContent.Manual)
            {
                SizeToContent = _previousSizeToContent;
                _previousSizeToContent = SizeToContent.Manual;

                if (WindowStartupLocation == WindowStartupLocation.CenterScreen)
                {
                    Left = SystemParameters.VirtualScreenLeft + SystemParameters.PrimaryScreenWidth / 2 - ActualWidth / 2;
                    Top = SystemParameters.VirtualScreenTop + SystemParameters.PrimaryScreenHeight / 2 - ActualHeight / 2;
                }
                if (WindowStartupLocation == WindowStartupLocation.CenterOwner)
                {
                    if (Owner != null)
                    {
                        if (Owner.WindowState == WindowState.Maximized)
                        {
                            var source = PresentationSource.FromVisual(Owner);
                            if (source != null && source.CompositionTarget != null)
                            {
                                var ownerHandle = new WindowInteropHelper(Owner).EnsureHandle();
                                var ownerWindow = new Native.Window(ownerHandle);
                                ownerWindow.Invalidate();
                                Left = -ownerWindow.NonClientBorderWidth * source.CompositionTarget.TransformFromDevice.M11;
                                Top = -ownerWindow.NonClientBorderHeight * source.CompositionTarget.TransformFromDevice.M22;
                            }
                            else
                            {
                                Left = 0;
                                Top = 0;
                            }
                        }
                        else
                        {
                            Left = Owner.Left;
                            Top = Owner.Top;
                        }
                        Left += Owner.ActualWidth / 2 - ActualWidth / 2;
                        Top += Owner.ActualHeight / 2 - ActualHeight / 2;
                    }
                }
                
                UpdateNonClientBorder();

                if (_dispatcherFrame != null)
                {
                    _dispatcherFrame.Continue = false;
                }
            }
        }

        [SecuritySafeCritical]
        protected override void OnSourceInitialized(EventArgs e)
        {
            Hook();
            base.OnSourceInitialized(e);
        }

        [SecurityCritical]
        private void Hook()
        {
            var handle = new WindowInteropHelper(this).EnsureHandle();
            _monitor = new Monitor(handle);
            _window = new Native.Window(handle);
            UpdateNonClientBorder();

            var source = HwndSource.FromHwnd(handle);
            if (source != null)
            {
                source.AddHook(WndProc);
            }
        }

        [SecurityCritical]
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Interop.WM_GETMINMAXINFO:
                    if (Equals(WindowChrome.GetWindowChrome(this), _chrome))
                    {
                        GetMinMaxInfo(lParam);
                        handled = true;
                    }
                    break;
            }

            return (IntPtr)0;
        }

        [SecurityCritical]
        private void GetMinMaxInfo(IntPtr lParam)
        {
            var info = (Interop.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Interop.MINMAXINFO));

            _monitor.Invalidate();
            Taskbar.Invalidate();

            var bounds = _monitor.Bounds;
            var workArea = _monitor.WorkArea;
            info.ptMaxPosition.x = Math.Abs(bounds.left) + Taskbar.Position == TaskbarPosition.Left && Taskbar.AutoHide ? 1 : 0;
            info.ptMaxPosition.y = Math.Abs(bounds.top) + Taskbar.Position == TaskbarPosition.Top && Taskbar.AutoHide ? 1 : 0;
            info.ptMaxSize.x = info.ptMaxTrackSize.x =
                Math.Abs(workArea.right - workArea.left) - (Taskbar.Position == TaskbarPosition.Right && Taskbar.AutoHide ? 1 : 0);
            info.ptMaxSize.y = info.ptMaxTrackSize.y =
                Math.Abs(workArea.bottom - workArea.top) - (Taskbar.Position == TaskbarPosition.Bottom && Taskbar.AutoHide ? 1 : 0);

            var source = PresentationSource.FromVisual(this);
            if (source != null && source.CompositionTarget != null)
            {
                if (DoubleUtil.IsNonNegative(MinWidth))
                {
                    info.ptMinTrackSize.x = (int)Math.Ceiling(MinWidth * source.CompositionTarget.TransformFromDevice.M11);
                }
                if (DoubleUtil.IsNonNegative(MinHeight))
                {
                    info.ptMinTrackSize.y = (int)Math.Ceiling(MinHeight * source.CompositionTarget.TransformFromDevice.M22);
                }
                if (DoubleUtil.IsNonNegative(MaxWidth))
                {
                    info.ptMaxSize.x = info.ptMaxTrackSize.x =
                                       Math.Min(info.ptMaxSize.x, (int)Math.Ceiling(MaxWidth * source.CompositionTarget.TransformFromDevice.M11));
                }
                if (DoubleUtil.IsNonNegative(MaxHeight))
                {
                    info.ptMaxSize.y = info.ptMaxTrackSize.y =
                                       Math.Min(info.ptMaxSize.y, (int)Math.Ceiling(MaxHeight * source.CompositionTarget.TransformFromDevice.M22));
                }
            }

            Marshal.StructureToPtr(info, lParam, true);
        }

        [PublicAPI]
        public static readonly DependencyProperty IsMainWindowProperty =
            DependencyProperty.RegisterAttached("IsMainWindow", typeof(bool), typeof(Window),
                                                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnIsMainWindowChanged));

        [PublicAPI]
        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Window))]
        public static bool GetIsMainWindow([NotNull] System.Windows.Window obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return BooleanBoxingHelper.Unbox(obj.GetValue(IsMainWindowProperty));
        }

        [PublicAPI]
        public static void SetIsMainWindow([NotNull] System.Windows.Window obj, bool value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(IsMainWindowProperty, BooleanBoxingHelper.Box(value));
        }

        private static void OnIsMainWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (System.Windows.Window)d;
            if (instance != null && e.NewValue == BooleanBoxingHelper.TrueBox)
            {
                Action setMainWindow =
                    () =>
                    {
                        foreach (var window in Application.Current.Windows.AsParallel().Cast<System.Windows.Window>().Where(window => !Equals(window, instance)))
                        {
                            SetIsMainWindow(window, false);
                        }
                    };
                Application.Current.Dispatcher.BeginInvoke(setMainWindow, DispatcherPriority.Render);
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(Window),
                                        new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.None, OnProgressChanged));

        [PublicAPI]
        [Category("Appearance")]
        [Description("The current magnitude of the progress bar that placed in the top of window.")]
        public double Progress
        {
            get { return BoxingHelper<double>.Unbox(GetValue(ProgressProperty)); }
            set { SetValue(ProgressProperty, value); }
        }

        private static void OnProgressChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Window)obj;
            instance.OnProgressChanged((double)e.OldValue, (double)e.NewValue);
        }

        [PublicAPI]
// ReSharper disable VirtualMemberNeverOverriden.Global
        protected virtual void OnProgressChanged(double oldProgress, double newProgress)
// ReSharper restore VirtualMemberNeverOverriden.Global
        {
        }

        [PublicAPI]
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(Window),
                                        new FrameworkPropertyMetadata(BooleanBoxingHelper.FalseBox, FrameworkPropertyMetadataOptions.None, OnIsBusyChanged));

        [PublicAPI]
        [Category("Appearance")]
        [Description("Indicates busy state of window.")]
        public bool IsBusy
        {
            get { return BooleanBoxingHelper.Unbox(GetValue(IsBusyProperty)); }
            set { SetValue(IsBusyProperty, BooleanBoxingHelper.Box(value)); }
        }

        private static void OnIsBusyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Window)obj;
            instance.OnIsBusyChanged(BooleanBoxingHelper.Unbox(e.OldValue), BooleanBoxingHelper.Unbox(e.NewValue));
        }

        [PublicAPI]
// ReSharper disable VirtualMemberNeverOverriden.Global
        protected virtual void OnIsBusyChanged(bool oldIsBusy, bool newIsBusy)
// ReSharper restore VirtualMemberNeverOverriden.Global
        {
        }

        [PublicAPI]
        public static readonly DependencyProperty HasDropShadowProperty =
            DependencyProperty.Register("HasDropShadow", typeof(bool), typeof(Window),
                                        new FrameworkPropertyMetadata(BooleanBoxingHelper.FalseBox, FrameworkPropertyMetadataOptions.None,
                                                                      OnHasDropShadowChanged, CoerceHasDropShadow));

        [PublicAPI]
        [Category("Appearance")]
        [Description("Indicates has window drop shadow or not.")]
        public bool HasDropShadow
        {
            get { return BooleanBoxingHelper.Unbox(GetValue(HasDropShadowProperty)); }
            set { SetValue(HasDropShadowProperty, BooleanBoxingHelper.Box(value)); }
        }

        private static void OnHasDropShadowChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Window)obj;
            instance.OnHasDropShadowChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        [PublicAPI]
        [SecuritySafeCritical]
// ReSharper disable VirtualMemberNeverOverriden.Global
        protected virtual void OnHasDropShadowChanged(bool oldHasDropShadow, bool newHasDropShadow)
// ReSharper restore VirtualMemberNeverOverriden.Global
        {
            OnHasDropShadowChangedInternal(newHasDropShadow);
        }

#if NETFX4
        [SuppressMessage("Microsoft.Security", "CA2141:TransparentMethodsMustNotSatisfyLinkDemandsFxCopRule",
            Justification = "We need to use Microsoft.Windows.Shell.dll version 3.5")]
#endif
        [SuppressMessage("Microsoft.Contracts", "Nonnull-36-0",
            Justification = "Bug in Code Contracts static checker: We should ignore value of _chrome field because it is overwritten.")]
        [SecurityCritical]
        private void OnHasDropShadowChangedInternal(bool newHasDropShadow)
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome))
            {
                _chrome = new WindowChrome
                              {
                                  CaptionHeight = _chrome.CaptionHeight,
                                  CornerRadius = _chrome.CornerRadius,
                                  GlassFrameThickness = !newHasDropShadow ? new Thickness(0d) : new Thickness(0d, 0d, 0d, 1d),
                                  NonClientFrameEdges = _chrome.NonClientFrameEdges,
                                  ResizeBorderThickness = _chrome.ResizeBorderThickness,
                                  UseAeroCaptionButtons = _chrome.UseAeroCaptionButtons
                              };
                _chrome.TryFreeze();
                WindowChrome.SetWindowChrome(this, _chrome);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Coerce- methods shouldn't throw exceptions")]
        [SuppressMessage("Microsoft.Contracts", "Nonnull-29-0", Justification = "Lack of contracts")]
        private static object CoerceHasDropShadow(DependencyObject obj, object basevalue)
        {
            ValidationHelper.NotNull(obj, "obj");

            try
            {
                // NOTE: Ignore Code Contracts warnings
                return BooleanBoxingHelper.Unbox(basevalue) && SystemParameters.DropShadow && Windows.IsWindowsVistaOrHigher;
            }
            catch
            {
                return basevalue;
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty ApplicationBarProperty =
            DependencyProperty.RegisterAttached("ApplicationBar", typeof(ApplicationBar), typeof(Window),
                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnApplicationBarChanged));

        [PublicAPI]
        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Window))]
        public static ApplicationBar GetApplicationBar([NotNull] System.Windows.Window obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (ApplicationBar)obj.GetValue(ApplicationBarProperty);
        }

        [PublicAPI]
        public static void SetApplicationBar([NotNull] System.Windows.Window obj, ApplicationBar value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ApplicationBarProperty, value);
        }

        private static void OnApplicationBarChanged([NotNull] DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ValidationHelper.NotNull(obj, "obj");
            var instance = obj as System.Windows.Window;
            if (instance != null && e.OldValue != null)
            {
                instance.MouseRightButtonUp -= OnApplicationBarVisibilityChanged;
            }
            if (instance != null && e.NewValue != null)
            {
                instance.MouseRightButtonUp += OnApplicationBarVisibilityChanged;
            }
        }

        private static void OnApplicationBarVisibilityChanged(object sender, MouseButtonEventArgs e)
        {
            var window = sender as System.Windows.Window;
            var source = e.OriginalSource as UIElement;
            if (window != null && source != null && !ApplicationBar.GetPreventsOpen(source))
            {
                var applicationBar = GetApplicationBar(window);
                if (applicationBar != null)
                {
                    if (applicationBar.IsOpening || applicationBar.IsClosing)
                    {
                        if (applicationBar.IsOpening)
                        {
                            applicationBar.Opened += ChangeVisibilityAfterOpened;
                        }
                        else if (applicationBar.IsClosing)
                        {
                            applicationBar.Closed += ChangeVisibilityAfterClosed;
                        }
                    }
                    else
                    {
                        applicationBar.IsOpen = !applicationBar.StaysOpen || !applicationBar.IsOpen;
                    }
                }
            }
        }

        private static void ChangeVisibilityAfterOpened(object sender, EventArgs e)
        {
            var applicationBar = sender as ApplicationBar;
            if (applicationBar != null)
            {
                applicationBar.IsOpen = !applicationBar.StaysOpen || !applicationBar.IsOpen;
                applicationBar.Opened -= ChangeVisibilityAfterOpened;
            }
        }

        private static void ChangeVisibilityAfterClosed(object sender, EventArgs e)
        {
            var applicationBar = sender as ApplicationBar;
            if (applicationBar != null)
            {
                applicationBar.IsOpen = !applicationBar.StaysOpen || !applicationBar.IsOpen;
                applicationBar.Closed -= ChangeVisibilityAfterClosed;
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty IsApplicationBarHostProperty =
            DependencyProperty.RegisterAttached("IsApplicationBarHost", typeof(bool), typeof(Window),
                                                new FrameworkPropertyMetadata(BooleanBoxingHelper.FalseBox, FrameworkPropertyMetadataOptions.None,
                                                                              OnIsApplicationBarHostChanged));

        [PublicAPI]
        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        [AttachedPropertyBrowsableForType(typeof(Decorator))]
        public static bool GetIsApplicationBarHost(Decorator obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return BooleanBoxingHelper.Unbox(obj.GetValue(IsApplicationBarHostProperty));
        }

        [PublicAPI]
        public static void SetIsApplicationBarHost(Decorator obj, bool value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(IsApplicationBarHostProperty, BooleanBoxingHelper.Box(value));
        }

        private static void OnIsApplicationBarHostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var host = obj as Decorator;
            if (host != null)
            {
                var window = VisualTreeHelperExtensions.FindParent<System.Windows.Window>(host);
                if (window != null)
                {
                    var applicationBar = GetApplicationBar(window);
                    host.Child = BooleanBoxingHelper.Unbox(e.NewValue) ? applicationBar : null;
                }
            }
        }

        [PublicAPI]
        [SecurityCritical] 
        public new bool? ShowDialog()
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome) && SizeToContent != SizeToContent.Manual)
            {
                _dispatcherFrame = new DispatcherFrame();

                Show();

                Dispatcher.PushFrame(_dispatcherFrame);
                _dispatcherFrame = null;
                return base.ShowDialog();
            }
            return base.ShowDialog();
        }

        private DispatcherFrame _dispatcherFrame;

        [SecuritySafeCritical]
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OnApplyTemplateInternal();
        }

        [SecurityCritical]
#if NETFX4
        [SuppressMessage("Microsoft.Security", "CA2141:TransparentMethodsMustNotSatisfyLinkDemandsFxCopRule",
            Justification = "We need to use Microsoft.Windows.Shell.dll version 3.5")]
#endif
        private void OnApplyTemplateInternal()
        {
            if (Template != null)
            {
                _caption = Template.FindName(CaptionName, this) as FrameworkElement;
                if (_caption == null)
                {
                    Trace.TraceError(CaptionName + " not found.");
                }
                else
                {
                    _caption.SizeChanged += OnCaptionSizeChanged;
                }

                // NOTE: Lack of contracts: FindName must be marked as pure method
                Contract.Assume(Template != null);
                _layoutRoot = Template.FindName(LayoutRootName, this) as Panel;
                if (_layoutRoot == null)
                {
                    Trace.TraceError(LayoutRootName + " not found.");
                }
            }
        }

        [SuppressMessage("Microsoft.Contracts", "Nonnull-67-0",
            Justification = "Bug in Code Contracts static checker: We should ignore value of _chrome field because it is overwritten.")]
        [SecurityCritical]
        private void OnCaptionSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged && Equals(WindowChrome.GetWindowChrome(this), _chrome))
            {
                _chrome = new WindowChrome
                              {
                                  CaptionHeight = e.NewSize.Height,
                                  CornerRadius = _chrome.CornerRadius,
                                  GlassFrameThickness = _chrome.GlassFrameThickness,
                                  NonClientFrameEdges = _chrome.NonClientFrameEdges,
                                  ResizeBorderThickness = _chrome.ResizeBorderThickness,
                                  UseAeroCaptionButtons = _chrome.UseAeroCaptionButtons
                              };
                _chrome.TryFreeze();
                WindowChrome.SetWindowChrome(this, _chrome);
            }
        }

        [SuppressMessage("Microsoft.Contracts", "Nonnull-36-0",
            Justification = "Bug in Code Contracts static checker: We should ignore value of _chrome field because it is overwritten.")]
        [SecurityCritical]
        private void OnResizeBorderThicknessChanged(object sender, EventArgs e)
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome))
            {
                _chrome = new WindowChrome
                              {
                                  CaptionHeight = _chrome.CaptionHeight,
                                  CornerRadius = _chrome.CornerRadius,
                                  GlassFrameThickness = _chrome.GlassFrameThickness,
                                  NonClientFrameEdges = _chrome.NonClientFrameEdges,
                                  ResizeBorderThickness = Parameters.Window.GetResizeBorderThickness(this),
                                  UseAeroCaptionButtons = _chrome.UseAeroCaptionButtons
                              };
                _chrome.TryFreeze();
                WindowChrome.SetWindowChrome(this, _chrome);
            }
        }

        [SecuritySafeCritical]
        private static void OnWindowStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ValidationHelper.NotNull(obj, "obj");
            var instance = (Window)obj;
            instance.OnWindowStateChanged();
        }

        [SecurityCritical]
        private void OnWindowStateChanged()
        {
            UpdateNonClientBorder();
        }

        [SecurityCritical]
        private void UpdateNonClientBorder()
        {
            if (!Equals(WindowChrome.GetWindowChrome(this), _chrome) || _layoutRoot == null || _window == null)
            {
                return;
            }
            
            Taskbar.Invalidate();
            if (WindowState == WindowState.Maximized && !Taskbar.AutoHide && SizeToContent == SizeToContent.Manual)
            {
                _window.Invalidate();
                _layoutRoot.Margin = new Thickness(_window.NonClientBorderWidth,
                                                   _window.NonClientBorderHeight,
                                                   _window.NonClientBorderWidth,
                                                   _window.NonClientBorderHeight);
            }
            else
            {
                _layoutRoot.Margin = new Thickness();
            }
        }
    }
}