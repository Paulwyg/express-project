using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Elysium.Extensions
{
    [DebuggerNonUserCode]
    internal static class FreezableExtensions
    {
        [Pure]
        [JetBrains.Annotations.Pure]
        internal static T TryFreeze<T>(T freezable)
            where T : Freezable
        {
            ValidationHelper.NotNull(freezable, "freezable");
            Contract.Ensures(Contract.Result<T>() != null);

            return !freezable.IsFrozen && freezable.CanFreeze ? (T)freezable.GetAsFrozen() : freezable;
        }

        internal static void TryFreeze(this Freezable freezable)
        {
            ValidationHelper.NotNull(freezable, "freezable");

            if (!freezable.IsFrozen && freezable.CanFreeze)
            {
                freezable.Freeze();
            }
        }
    }
}