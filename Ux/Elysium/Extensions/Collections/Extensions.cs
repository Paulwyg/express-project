using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Elysium.Extensions;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace
namespace System.Linq
{
    [DebuggerStepThrough]
    internal static class Extensions
    {
        internal static bool SafeGet<TKey, TValue>([NotNull] this IDictionary dictionary, [NotNull] TKey key, out TValue value)
        {
            ValidationHelper.NotNull(dictionary, "dictionary");
            ValidationHelper.NotNull(key, "key");

            value = default(TValue);
            if (!dictionary.Contains(key))
            {
                return false;
            }
            var temp = dictionary[key];
            if (!(temp is TValue))
            {
                return false;
            }
            value = (TValue)temp;
            return true;
        }

        internal static void SafeSet<TKey, TValue>([NotNull] this IDictionary dictionary, [NotNull] TKey key, TValue value)
        {
            ValidationHelper.NotNull(dictionary, "dictionary");
            ValidationHelper.NotNull(key, "key");

            if (dictionary.Contains(key))
            {
                // Set value, if key exist
                dictionary[key] = value;
            }
            else
            {
                // Add key and value, if key doesn't exist
                dictionary.Add(key, value);
            }
        }

        internal static void SafeSet([NotNull] this IDictionary destinationDictionary, [NotNull] IDictionary sourceDictionary)
        {
            ValidationHelper.NotNull(destinationDictionary, "destinationDictionary");
            ValidationHelper.NotNull(sourceDictionary, "sourceDictionary");

            foreach (var key in sourceDictionary.Keys)
            {
                destinationDictionary.SafeSet(key, sourceDictionary[key]);
            }
        }

        internal static void SafeRemove<TKey>([NotNull] this IDictionary dictionary, [NotNull] TKey key)
        {
            ValidationHelper.NotNull(dictionary, "dictionary");
            ValidationHelper.NotNull(key, "key");

            if (dictionary.Contains(key))
            {
                dictionary.Remove(key);
            }
        }

        internal static void SafeRemove([NotNull] this IDictionary destinationDictionary, [NotNull] IDictionary sourceDictionary)
        {
            ValidationHelper.NotNull(destinationDictionary, "destinationDictionary");
            ValidationHelper.NotNull(sourceDictionary, "sourceDictionary");

            foreach (var key in sourceDictionary.Keys)
            {
                destinationDictionary.SafeRemove(key);
            }
        }

        internal static void SafeSet<T>([NotNull] this ICollection<WeakReference> collection, T value)
        {
            ValidationHelper.NotNull(collection, "collection");

            if (!collection.Any(reference => Equals(reference.Target, value)))
            {
                collection.Add(new WeakReference(value));
            }
        }

        internal static void SafeRemove<T>([NotNull] this ICollection<WeakReference> collection, T value)
        {
            ValidationHelper.NotNull(collection, "collection");

            var values = collection.Where(reference => Equals(reference.Target, value));
            foreach (var reference in values)
            {
                collection.Remove(reference);
            }
        }

        internal static void Collect([NotNull] this ICollection<WeakReference> collection)
        {
            ValidationHelper.NotNull(collection, "collection");

            foreach (var reference in collection.Where(reference => reference.Target == null))
            {
                collection.Remove(reference);
            }
        }
    }
}
// ReSharper restore CheckNamespace