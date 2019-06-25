using System.Diagnostics;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    internal static class BoxingHelper<T>
        where T : struct
    {
        [JetBrains.Annotations.Pure]
        internal static T Unbox(object value)
        {
            Contract.Assume(value is T);
            return (T)value;
        }
    }
}
