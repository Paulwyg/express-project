// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicalTreeHelperExtensions.cs" company="Alex F. Sherman & Codeplex community">
//   Copyright (c) 2011 Alex F. Sherman
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//   documentation files (the "Software"), to deal in the Software without restriction, including without limitation
//   the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
//   to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//   THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//   CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//   DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the LogicalTreeHelperExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Windows;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    public static class LogicalTreeHelperExtensions
    {
        [Pure]
        public static T FindParent<T>(DependencyObject reference)
            where T : DependencyObject
        {
            ValidationHelper.NotNull(reference, "reference");

            var currentParent = LogicalTreeHelper.GetParent(reference);
            var parent = currentParent as T;
            return parent ?? (currentParent != null ? FindParent<T>(currentParent) : null);
        }

        [Pure]
        public static DependencyObject FindTopLevelParent(DependencyObject reference)
        {
            ValidationHelper.NotNull(reference, "reference");

            var parent = LogicalTreeHelper.GetParent(reference);
            if (parent != null)
            {
                var nextParent = LogicalTreeHelper.GetParent(parent);
                return nextParent != null ? FindTopLevelParent(parent) : parent;
            }
            return null;
        }
    }
}
