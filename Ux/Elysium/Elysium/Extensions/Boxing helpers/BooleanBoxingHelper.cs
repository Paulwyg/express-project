using System.Diagnostics;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    internal static class BooleanBoxingHelper
    {
        internal static readonly object FalseBox = false;
        internal static readonly object TrueBox = true;

        [JetBrains.Annotations.Pure]
        internal static object Box(bool value)
        {
            Contract.Ensures(Contract.Result<object>() != null);
            return value ? TrueBox : FalseBox;
        }

        [JetBrains.Annotations.Pure]
        internal static bool Unbox(object value)
        {
            Contract.Assume(value is bool);
            return BoxingHelper<bool>.Unbox(value);
        }
    }
}
