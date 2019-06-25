using System.Windows;
using System.Windows.Controls;

namespace Login.Services
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
                                                typeof(string), typeof(PasswordHelper),
                                                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
                                                typeof(bool), typeof(PasswordHelper),
                                                new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
                                                typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
                                                      DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,
                                   DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }

    //public static class PasswordBoxBindingHelper
    //{
    //    public static bool GetIsPasswordBindingEnabled(DependencyObject obj)
    //    {
    //        return (bool)obj.GetValue(IsPasswordBindingEnabledProperty);
    //    }

    //    public static void SetIsPasswordBindingEnabled(DependencyObject obj, bool value)
    //    {
    //        obj.SetValue(IsPasswordBindingEnabledProperty, value);
    //    }

    //    public static readonly DependencyProperty IsPasswordBindingEnabledProperty =
    //        DependencyProperty.RegisterAttached("IsPasswordBindingEnabled", typeof(bool),
    //        typeof(PasswordBoxBindingHelper),
    //        new UIPropertyMetadata(false, OnIsPasswordBindingEnabledChanged));

    //    private static void OnIsPasswordBindingEnabledChanged(DependencyObject obj,
    //                                                          DependencyPropertyChangedEventArgs e)
    //    {
    //        var passwordBox = obj as PasswordBox;

    //        if (passwordBox != null)
    //        {
    //            passwordBox.PasswordChanged -= PasswordBoxPasswordChanged;

    //            if ((bool)e.NewValue)
    //            {
    //                passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
    //            }

    //        }
    //    }

    //    //when the passwordBox's password changed, update the buffer
    //    static void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
    //    {
    //        var passwordBox = (PasswordBox)sender;

    //        if (!String.Equals(GetBindedPassword(passwordBox), passwordBox.Password))
    //        {
    //            SetBindedPassword(passwordBox, passwordBox.Password);
    //        }
    //    }


    //    public static string GetBindedPassword(DependencyObject obj)
    //    {
    //        return (string)obj.GetValue(BindedPasswordProperty);
    //    }


    //    public static void SetBindedPassword(DependencyObject obj, string value)
    //    {
    //        obj.SetValue(BindedPasswordProperty, value);
    //    }

    //    public static readonly DependencyProperty BindedPasswordProperty =
    //        DependencyProperty.RegisterAttached("BindedPassword", typeof(string),
    //        typeof(PasswordBoxBindingHelper),
    //        new UIPropertyMetadata(string.Empty, OnBindedPasswordChanged));

    //    //when the buffer changed, upate the passwordBox's password
    //    private static void OnBindedPasswordChanged(DependencyObject obj,
    //                                                DependencyPropertyChangedEventArgs e)
    //    {
    //        var passwordBox = obj as PasswordBox;
    //        if (passwordBox != null)
    //        {

    //            passwordBox.Password = e.NewValue == null ? string.Empty : e.NewValue.ToString();
    //        }
    //    }

    //    private static void SetPasswordBoxSelection(PasswordBox passwordBox, int start, int length)
    //    {
    //        var select = passwordBox.GetType().GetMethod("Select",
    //                        BindingFlags.Instance | BindingFlags.NonPublic);

    //        select.Invoke(passwordBox, new object[] { start, length });
    //    }

    //}
}