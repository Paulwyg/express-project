using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    internal static class DoubleUtil
    {
        [JetBrains.Annotations.Pure]
        internal static bool IsNonNegative(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;
        }

        [Conditional("CONTRACTS_FULL")]
        [ContractAbbreviator]
        internal static void EnsureNonNegative()
        {
            Contract.Ensures(IsNonNegative(Contract.Result<double>()));
        }

        [JetBrains.Annotations.Pure]
        internal static object CoerceNonNegative(DependencyObject obj, object basevalue)
        {
            ValidationHelper.NotNull(obj, "obj");
            var value = BoxingHelper<double>.Unbox(basevalue);
            return IsNonNegative(value) ? value : 0d;
        }
    }
}
