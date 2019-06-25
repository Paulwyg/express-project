using System.Diagnostics;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    internal static class NullableBoxingHelper<T>
        where T : struct
    {
        [Pure]
        internal static object Box(T? value)
        {
            return value.HasValue ? (object)value.Value : null;
        }

        [Pure]
        internal static T? Unbox(object value)
        {
            return value == null ? new T?() : BoxingHelper<T>.Unbox(value);
        }
    }
}