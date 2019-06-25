using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Wlst.Cr.CoreOne.OtherHelper
{
    public static class ReturnKey
    {
        /// <summary>
        /// Command to execute on return key event.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof (ICommand),
            typeof (ReturnKey),
            new PropertyMetadata(OnSetCommandCallback));


        private static readonly DependencyProperty ReturnCommandBehaviorProperty = DependencyProperty.RegisterAttached(
            "ReturnCommandBehavior",
            typeof (ReturnCommandBehavior),
            typeof (ReturnKey),
            null);


        /// <summary>
        /// Sets the <see cref="ICommand"/> to execute on the return key event.
        /// </summary>
        /// <param name="textBox">TextBox dependency object to attach command</param>
        /// <param name="command">Command to attach</param>
        public static void SetCommand(Control textBox, ICommand command)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            textBox.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="textBox">TextBox containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        public static ICommand GetCommand(Control textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            return textBox.GetValue(CommandProperty) as ICommand;
        }


        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control textBox = dependencyObject as Control;
            if (textBox != null)
            {
                ReturnCommandBehavior behavior = GetOrCreateBehavior(textBox);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static ReturnCommandBehavior GetOrCreateBehavior(Control textBox)
        {
            ReturnCommandBehavior behavior = textBox.GetValue(ReturnCommandBehaviorProperty) as ReturnCommandBehavior;
            if (behavior == null)
            {
                behavior = new ReturnCommandBehavior(textBox);
                textBox.SetValue(ReturnCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }

    /// <summary>
    /// Defines a behavior that executes a <see cref="ICommand"/> when the Return key is pressed inside a <see cref="TextBox"/>.
    /// </summary>
    /// <remarks>This behavior also supports setting a basic watermark on the <see cref="TextBox"/>.</remarks>
    public class ReturnCommandBehavior : CommandBehaviorBase<Control>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ReturnCommandBehavior"/>.
        /// </summary>
        /// <param name="textBox">The <see cref="TextBox"/> over which the <see cref="ICommand"/> will work.</param>
        public ReturnCommandBehavior(Control textBox)
            : base(textBox)
        {
            if (textBox == null)
            {
                throw new ArgumentNullException("textBox");
            }

            var tmps = textBox as TextBox;
            if (tmps != null)
            {
                tmps.AcceptsReturn = false;
            }

            textBox.KeyDown += (s, e) => this.KeyPressed(e.Key);
        }

        /// <summary>
        /// Gets or Sets the text which is set as water mark on the <see cref="TextBox"/>.
        /// </summary>
        //  public string DefaultTextAfterCommandExecution { get; set; }

        /// <summary>
        /// Executes the <see cref="ICommand"/> when <paramref name="key"/> is <see cref="Key.Enter"/>.
        /// </summary>
        /// <param name="key">The key pressed on the <see cref="TextBox"/>.</param>
        protected void KeyPressed(Key key)
        {
            if (key == Key.Enter && TargetObject != null)
            {
                //  this.CommandParameter = TargetObject.Text;
                ExecuteCommand();
            }
        }


    }


}
