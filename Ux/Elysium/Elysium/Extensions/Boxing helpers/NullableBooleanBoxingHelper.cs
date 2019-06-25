using System.Diagnostics;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    internal static class NullableBooleanBoxingHelper
    {
        [Pure]
        internal static object Box(bool? value)
        {
            return value.HasValue ? BooleanBoxingHelper.Box(value.Value) : null;
        }

        [Pure]
        internal static bool? Unbox(object value)
        {
            return value == null ? new bool?() : BooleanBoxingHelper.Unbox(value);
        }
    }
}