using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    internal static class ValidationHelper
    {
        [ContractAnnotation("halt <= argument: null")]
        [ContractArgumentValidator]
        internal static void NotNull<T>([ValidatedNotNull] T argument, [NotNull] Expression<Func<T>> parameterExpression)
        {
            NotNull(argument, ((MemberExpression)parameterExpression.Body).Member.Name);
        }

        [ContractAnnotation("halt <= argument: null")]
        [ContractArgumentValidator]
        internal static void NotNull<T>([ValidatedNotNull] T argument, [NotNull] string parameterName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            Contract.EndContractBlock();
        }

        [ContractAnnotation("halt <= argument: null")]
        [ContractArgumentValidator]
        internal static void NotNullOrWhitespace([ValidatedNotNull] string argument, [NotNull] Expression<Func<string>> parameterExpression)
        {
            NotNullOrWhitespace(argument, ((MemberExpression)parameterExpression.Body).Member.Name);
        }

        [ContractAnnotation("halt <= argument: null")]
        [ContractArgumentValidator]
        internal static void NotNullOrWhitespace([ValidatedNotNull] string argument, [NotNull] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException(parameterName + " can't be null, empty or contains only whitespaces", parameterName);
            }
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        internal static void OfType<T>(T argument, [NotNull] Expression<Func<T>> parameterExpression, [NotNull] Type type)
        {
            OfType(argument, ((MemberExpression)parameterExpression.Body).Member.Name, type);
        }

        [ContractArgumentValidator]
        internal static void OfType<T>(T argument, [NotNull] string parameterName, [NotNull] Type type)
        {
            if (!type.IsInstanceOfType(argument))
            {
                throw new ArgumentException(parameterName + " must be of type: " + type.Name, parameterName);
            }
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        internal static void OfTypes<T>(T argument, [NotNull] Expression<Func<T>> parameterExpression, [NotNull] Type firstType, [NotNull] Type secondType)
        {
            OfTypes(argument, ((MemberExpression)parameterExpression.Body).Member.Name, firstType, secondType);
        }

        [ContractArgumentValidator]
        internal static void OfTypes<T>(T argument, [NotNull] string parameterName, [NotNull] Type firstType, [NotNull] Type secondType)
        {
            if (!(firstType.IsInstanceOfType(argument) || secondType.IsInstanceOfType(argument)))
            {
                throw new ArgumentException(parameterName + " must belong to one of the types: " + firstType.Name + ", " + secondType.Name, parameterName);
            }
            Contract.EndContractBlock();
        }
    }
}