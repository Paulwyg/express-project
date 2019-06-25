// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableDictionary.cs" company="Alex F. Sherman & Codeplex community">
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
//   Defines the ObservableDictionary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

using Elysium.Extensions;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace
namespace System.Collections.ObjectModel
{
    [PublicAPI]
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "ObservableDictionary don't need to be disposable")]
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
    public class ObservableDictionary<TKey, TValue> :
// ReSharper restore ClassWithVirtualMembersNeverInherited.Global
        IObservableDictionary<TKey, TValue>,
        IDictionary<TKey, TValue>,
        IDictionary,
        ISerializable,
        IDeserializationCallback
    {
        #region Private fields

        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Monitor _monitor = new Monitor();

        #endregion

        #region Monitor

        private sealed class Monitor : IDisposable
        {
            private int _busyCount;

            public bool IsBusy
            {
                get { return _busyCount > 0; }
            }

            public void Enter()
            {
                _busyCount++;
            }

            public void Dispose()
            {
                _busyCount--;
            }
        }

        #endregion

        #region Constructors

        [PublicAPI]
        public ObservableDictionary()
            : this(0, null)
        {
        }

        [PublicAPI]
        public ObservableDictionary(int capacity)
            : this(capacity, null)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            Contract.Ensures(Count == 0);
            Contract.EndContractBlock();
        }

        [PublicAPI]
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer)
        {
            Contract.Ensures(Count == 0);
        }

        [PublicAPI]
        [SecuritySafeCritical]
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            Contract.Ensures(Count == 0);
            Contract.EndContractBlock();

            _cache = new Dictionary<TKey, TValue>(capacity, comparer);

            Contract.Assume(Count == 0);
        }

        [PublicAPI]
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) :
            this(dictionary, null)
        {
            ValidationHelper.NotNull(dictionary, "dictionary");
            Contract.Ensures(Count == dictionary.Count);
        }

        [PublicAPI]
        [SecuritySafeCritical]
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            ValidationHelper.NotNull(dictionary, "dictionary");
            Contract.Ensures(Count == dictionary.Count);

            _cache = new Dictionary<TKey, TValue>(dictionary, comparer);

            Contract.Assume(Count == dictionary.Count);
        }

        [PublicAPI]
        [SecuritySafeCritical]
        protected ObservableDictionary(SerializationInfo info, StreamingContext context)
        {
            Contract.Ensures(Count == 0);

            Ctor(info, context);

            Contract.Assume(Count == 0);
        }

        [SecurityCritical]
        private void Ctor(SerializationInfo info, StreamingContext context)
        {
            var constructor = typeof(Dictionary<TKey, TValue>).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null,
                                                                              new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);
            if (constructor != null)
            {
                constructor.Invoke(_cache, new object[] { info, context });
            }
        }

        #endregion

        #region Public methods

        [PublicAPI]
        public void Add(TKey key, TValue value)
        {
            ValidationHelper.NotNull(key, "key");
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);

            var oldCount = Count;

            CheckReentrancy();

            _cache.Add(key, value);
            RaiseAdded(key, value, IndexOf(key));

            // NOTE: Lack of contracts
            Contract.Assume(Count == oldCount + 1);
        }

        [SecuritySafeCritical]
        private int IndexOf(TKey key)
        {
            ValidationHelper.NotNull(key, "key");

            return FindEntry(key);
        }

        [SecuritySafeCritical]
        private int IndexOf(object key)
        {
            ValidationHelper.NotNull(key, "key");
            ValidationHelper.OfType(key, "key", typeof(TKey));

            return FindEntry((TKey)key);
        }

        [SecurityCritical]
        private int FindEntry(TKey key)
        {
            var findEntry = typeof(Dictionary<TKey, TValue>).GetMethod("FindEntry", BindingFlags.Instance | BindingFlags.NonPublic);

            if (findEntry != null)
            {
                return (int)findEntry.Invoke(_cache, new object[] { key });
            }
            return -1;
        }

        [PublicAPI]
        public bool ContainsKey(TKey key)
        {
            ValidationHelper.NotNull(key, "key");
            Contract.Ensures(!Contract.Result<bool>() || Count > 0);

            var result = _cache.ContainsKey(key);

            // NOTE: Lack of contracts
            Contract.Assume(!result || Count > 0);
            return result;
        }

        [PublicAPI]
        public bool ContainsValue(TValue value)
        {
            Contract.Ensures(!Contract.Result<bool>() || Count > 0);

            var result = _cache.ContainsValue(value);

            // NOTE: Lack of contracts
            Contract.Assume(!result || Count > 0);
            return result;
        }

        [PublicAPI]
        public void Clear()
        {
            Contract.Ensures(Count == 0);

            CheckReentrancy();

            var items = new ObservableKeyValuePair<TKey, TValue>[Count];
            ((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).CopyTo(items, 0);
            _cache.Clear();
            RaiseCleared(items);

            Contract.Assume(Count == 0);
        }

        [PublicAPI]
        public bool Remove(TKey key)
        {
            ValidationHelper.NotNull(key, "key");
            Contract.Ensures(!Contract.Result<bool>() || Count == Contract.OldValue(Count) - 1);

            var oldCount = Count;

            CheckReentrancy();

            var value = _cache[key];
            var result = _cache.Remove(key);
            RaiseRemoved(key, value, IndexOf(key));

            // NOTE: Lack of contracts
            Contract.Assume(!result || Count == oldCount - 1);
            return result;
        }

        [PublicAPI]
        public bool TryGetValue(TKey key, out TValue value)
        {
            ValidationHelper.NotNull(key, "key");
            Contract.Ensures(Contract.Result<bool>() == ContainsKey(key));

            var result = _cache.TryGetValue(key, out value);

            // NOTE: Lack of contracts
            Contract.Assume(result == ContainsKey(key));
            return result;
        }

        [PublicAPI]
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Public properties

        [PublicAPI]
        public IEqualityComparer<TKey> Comparer
        {
            get
            {
                Contract.Ensures(Contract.Result<IEqualityComparer<TKey>>() != null);
                return _cache.Comparer;
            }
        }

        [PublicAPI]
        public int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return _cache.Count;
            }
        }

        [PublicAPI]
        public TValue this[TKey key]
        {
            get
            {
                ValidationHelper.NotNull(key, "key");

                return _cache[key];
            }
            set
            {
                ValidationHelper.NotNull(key, "key");

                var oldValue = _cache[key];

                CheckReentrancy();

                _cache[key] = value;
                RaiseReplaced(key, oldValue, value, IndexOf(key));
            }
        }

        [PublicAPI]
        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                Contract.Ensures(Contract.Result<Dictionary<TKey, TValue>.KeyCollection>() != null);
                return _cache.Keys;
            }
        }

        [PublicAPI]
        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                Contract.Ensures(Contract.Result<Dictionary<TKey, TValue>.ValueCollection>() != null);
                return _cache.Values;
            }
        }

        #endregion

        #region Implementation of IDictionary

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary)_cache)[key];
            }

            [SuppressMessage("Microsoft.Contracts", "Nonnull-69-0", Justification = "Can't be proven.")]
            [SuppressMessage("Microsoft.Contracts", "Requires-14-87", Justification = "Can't be proven.")]
            set
            {
                ValidationHelper.NotNull(key, "key");
                ValidationHelper.OfType(key, "key", typeof(TKey));

                var cache = (IDictionary)_cache;

                var oldValue = cache[key];

                CheckReentrancy();

                cache[key] = value;

                RaiseReplaced((TKey)key, (TValue)oldValue, (TValue)value, IndexOf(key));
            }
        }

        ICollection<TKey> IObservableDictionary<TKey, TValue>.Keys
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<TKey>>() != null);
                return ((IDictionary<TKey, TValue>)_cache).Keys;
            }
        }

        ICollection<TValue> IObservableDictionary<TKey, TValue>.Values
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<TValue>>() != null);
                return ((IDictionary<TKey, TValue>)_cache).Values;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<TKey>>() != null);
                return ((IDictionary<TKey, TValue>)_cache).Keys;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<TValue>>() != null);
                return ((IDictionary<TKey, TValue>)_cache).Values;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection>() != null);
                return ((IDictionary)_cache).Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection>() != null);
                return ((IDictionary)_cache).Values;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>());
                return false;
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>());
                return false;
            }
        }

        bool IDictionary.Contains(object key)
        {
            ValidationHelper.NotNull(key, "key");
            ValidationHelper.OfType(key, "key", typeof(TKey));
            Contract.Ensures(!Contract.Result<bool>() || ((IDictionary)this).Count > 0);

            var result = ((IDictionary)_cache).Contains(key);

            // NOTE: Lack of contracts
            Contract.Assume(!result || ((IDictionary)this).Count > 0);
            return result;
        }

        [SuppressMessage("Microsoft.Contracts", "Requires-14-106", Justification = "Can't be proven.")]
        void IDictionary.Add(object key, object value)
        {
            ValidationHelper.NotNull(key, "key");
            ValidationHelper.OfType(key, "key", typeof(TKey));
            Contract.Ensures(((IDictionary)this).Count == Contract.OldValue(((IDictionary)this).Count) + 1);

            var @this = (IDictionary)this;

            var oldCount = @this.Count;

            CheckReentrancy();

            ((IDictionary)_cache).Add(key, value);

            RaiseAdded((TKey)key, (TValue)value, IndexOf(key));

            // NOTE: Lack of contracts
            Contract.Assume(@this.Count == oldCount + 1);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new Enumerator(this);
        }

        [SuppressMessage("Microsoft.Contracts", "Nonnull-103-0", Justification = "Can't be proven.")]
        [SuppressMessage("Microsoft.Contracts", "Requires-14-115", Justification = "Can't be proven.")]
        void IDictionary.Remove(object key)
        {
            ValidationHelper.NotNull(key, "key");
            ValidationHelper.OfType(key, "key", typeof(TKey));
            Contract.Ensures(((IDictionary)this).Count == Contract.OldValue(((IDictionary)this).Count) - 1);

            var @this = (IDictionary)this;

            var oldCount = @this.Count;

            CheckReentrancy();

            var cache = (IDictionary)_cache;
            var value = cache[key];
            cache.Remove(key);

            RaiseRemoved((TKey)key, (TValue)value, IndexOf(key));

            // NOTE: Lack of contracts
            Contract.Assume(@this.Count == oldCount - 1);
        }

        #endregion

        #region Implementation of ICollection

        bool ICollection<ObservableKeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>());
                return false;
            }
        }

        void ICollection<ObservableKeyValuePair<TKey, TValue>>.Add(ObservableKeyValuePair<TKey, TValue> item)
        {
            ValidationHelper.NotNull(item, "item");
            ValidationHelper.NotNull(item.Key, "item");
            Contract.Ensures(((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).Count ==
                             Contract.OldValue(((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).Count) + 1);

            var @this = (ICollection<ObservableKeyValuePair<TKey, TValue>>)this;

            var oldCount = @this.Count;

            CheckReentrancy();

            Add(item.Key, item.Value);
            RaiseAdded(item, IndexOf(item.Key));

            // NOTE: Lack of contracts
            Contract.Assume(@this.Count == oldCount + 1);
        }

        bool ICollection<ObservableKeyValuePair<TKey, TValue>>.Contains(ObservableKeyValuePair<TKey, TValue> item)
        {
            ValidationHelper.NotNull(item, "item");
            ValidationHelper.NotNull(item.Key, "item");
            Contract.Ensures(!Contract.Result<bool>() || ((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).Count > 0);

            var result = ContainsKey(item.Key) && ContainsValue(item.Value);

            // NOTE: Lack of contracts
            Contract.Assume(!result || ((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).Count > 0);
            return result;
        }

        bool ICollection<ObservableKeyValuePair<TKey, TValue>>.Remove(ObservableKeyValuePair<TKey, TValue> item)
        {
            ValidationHelper.NotNull(item, "item");
            ValidationHelper.NotNull(item.Key, "item");
            Contract.Ensures(!Contract.Result<bool>() ||
                             ((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).Count ==
                             Contract.OldValue(((ICollection<ObservableKeyValuePair<TKey, TValue>>)this).Count) - 1);

            var @this = (ICollection<ObservableKeyValuePair<TKey, TValue>>)this;

            var oldCount = @this.Count;

            CheckReentrancy();

            var result = @this.Contains(item) && Remove(item.Key);
            RaiseRemoved(item, IndexOf(item.Key));

            // NOTE: Lack of contracts
            Contract.Assume(!result || @this.Count == oldCount - 1);
            return result;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>());
                return false;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ValidationHelper.NotNull(item.Key, "item");
            Contract.Ensures(((ICollection<KeyValuePair<TKey, TValue>>)this).Count ==
                             Contract.OldValue(((ICollection<KeyValuePair<TKey, TValue>>)this).Count) + 1);

            var @this = (ICollection<KeyValuePair<TKey, TValue>>)this;

            var oldCount = @this.Count;

            CheckReentrancy();

            ((ICollection<KeyValuePair<TKey, TValue>>)_cache).Add(item);
            RaiseAdded(item.Key, item.Value, IndexOf(item.Key));

            // NOTE: Lack of contracts
            Contract.Assume(@this.Count == oldCount + 1);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            ValidationHelper.NotNull(item.Key, "item");
            Contract.Ensures(!Contract.Result<bool>() || ((ICollection<KeyValuePair<TKey, TValue>>)this).Count > 0);

            var result = ((ICollection<KeyValuePair<TKey, TValue>>)_cache).Contains(item);

            // NOTE: Lack of contracts
            Contract.Assume(!result || ((ICollection<KeyValuePair<TKey, TValue>>)this).Count > 0);
            return result;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            ValidationHelper.NotNull(item.Key, "item");
            Contract.Ensures(!Contract.Result<bool>() ||
                             ((ICollection<KeyValuePair<TKey, TValue>>)this).Count ==
                             Contract.OldValue(((ICollection<KeyValuePair<TKey, TValue>>)this).Count) - 1);

            var @this = (ICollection<KeyValuePair<TKey, TValue>>)this;

            var oldCount = @this.Count;

            CheckReentrancy();

            var result = ((ICollection<KeyValuePair<TKey, TValue>>)_cache).Remove(item);
            RaiseRemoved(item.Key, item.Value, IndexOf(item.Key));

            // NOTE: Lack of contracts
            Contract.Assume(!result || @this.Count == oldCount - 1);
            return result;
        }

        object ICollection.SyncRoot
        {
            get
            {
                return ((ICollection)_cache).SyncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>());
                return false;
            }
        }

        [SuppressMessage("Microsoft.Contracts", "ArrayUpperBound-146-0", Justification = "Bug in Code Contracts static checker")]
        void ICollection<ObservableKeyValuePair<TKey, TValue>>.CopyTo(ObservableKeyValuePair<TKey, TValue>[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            if (array.Rank != 1)
            {
                throw new ArgumentException("Multi-demensional array is not supported", "array");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Array must have non-zero lower bound", "array");
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (array.Length - index < Count)
            {
                throw new ArgumentException("Array length too small", "array");
            }
            Contract.EndContractBlock();

            var enumerator = GetEnumerator();
            enumerator.MoveNext();
            for (var i = 0; i < Count; i++)
            {
                array[i + index] = enumerator.Current;
                enumerator.MoveNext();
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            if (array.Rank != 1)
            {
                throw new ArgumentException("Multi-demensional array is not supported", "array");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Array must have non-zero lower bound", "array");
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (array.Length - index < Count)
            {
                throw new ArgumentException("Array length too small", "array");
            }
            Contract.EndContractBlock();

            var enumerator = ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
            enumerator.MoveNext();
            for (var i = 0; i < Count; i++)
            {
                array[i + index] = enumerator.Current;
                enumerator.MoveNext();
            }
        }

        [SuppressMessage("Microsoft.Contracts", "ArrayUpperBound-162-0", Justification = "Bug in Code Contracts static checker")]
        [SuppressMessage("Microsoft.Contracts", "ArrayUpperBound-221-0", Justification = "Bug in Code Contracts static checker")]
        [SuppressMessage("Microsoft.Contracts", "ArrayUpperBound-302-0", Justification = "Bug in Code Contracts static checker")]
        [SuppressMessage("Microsoft.Contracts", "ArrayUpperBound-386-0", Justification = "Bug in Code Contracts static checker")]
        [SuppressMessage("Microsoft.Contracts", "Nonnull-386-0", Justification = "Variable is used in try...catch block")]
        void ICollection.CopyTo(Array array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            if (array.Rank != 1)
            {
                throw new ArgumentException("Multi-demensional array is not supported", "array");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Array must have non-zero lower bound", "array");
            }
            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (array.Length - index < Count)
            {
                throw new ArgumentException("Array length too small", "array");
            }
            Contract.EndContractBlock();

            var observableKeyValuePairArray = array as ObservableKeyValuePair<TKey, TValue>[];
            if (observableKeyValuePairArray != null)
            {
                var enumerator = GetEnumerator();
                enumerator.MoveNext();
                for (var i = 0; i < Count; i++)
                {
                    observableKeyValuePairArray[i + index] = enumerator.Current;
                    enumerator.MoveNext();
                }
            }
            else
            {
                var keyValuePairArray = array as KeyValuePair<TKey, TValue>[];
                if (keyValuePairArray != null)
                {
                    var enumerator = ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
                    enumerator.MoveNext();
                    for (var i = 0; i < Count; i++)
                    {
                        keyValuePairArray[i + index] = enumerator.Current;
                        enumerator.MoveNext();
                    }
                }
                else
                {
                    var dictionaryEntryArray = array as DictionaryEntry[];
                    if (dictionaryEntryArray != null)
                    {
                        var enumerator = ((IDictionary)this).GetEnumerator();
                        enumerator.MoveNext();
                        for (var i = 0; i < Count; i++)
                        {
                            dictionaryEntryArray[i + index] = enumerator.Entry;
                            enumerator.MoveNext();
                        }
                    }
                    else
                    {
                        var objectArray = array as object[];
                        try
                        {
                            var enumerator = GetEnumerator();
                            enumerator.MoveNext();
                            for (var i = 0; i < Count; i++)
                            {
// ReSharper disable PossibleNullReferenceException
                                objectArray[i + index] = enumerator.Current;
// ReSharper restore PossibleNullReferenceException
                                enumerator.MoveNext();
                            }
                        }
                        catch (NullReferenceException)
                        {
                            throw new ArgumentException("Invalid array type", "array");
                        }
                        catch (InvalidCastException)
                        {
                            throw new ArgumentException("Invalid array type", "array");
                        }
                    }
                }
            }
        }

        #endregion

        #region Implementation of IEnumerable

        IEnumerator<ObservableKeyValuePair<TKey, TValue>> IEnumerable<ObservableKeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Enumerator

        [PublicAPI]
        [Serializable]
        public struct Enumerator : IEnumerator<ObservableKeyValuePair<TKey, TValue>>, IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
        {
            #region Private fields

            private readonly ObservableDictionary<TKey, TValue> _dictionary;
            private Dictionary<TKey, TValue>.Enumerator _cacheEnumerator;

            #endregion

            #region Constructors

            [SuppressMessage("Microsoft.Contracts", "Nonnull-14-0", Justification = "Bug in Code Contracts static checker: _cache can't be null")]
            internal Enumerator(ObservableDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _cacheEnumerator = dictionary._cache.GetEnumerator();
            }

            #endregion

            #region Implementation of IDictionaryEnumerator

            object IDictionaryEnumerator.Key
            {
                get
                {
                    return ((IDictionaryEnumerator)_cacheEnumerator).Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    return ((IDictionaryEnumerator)_cacheEnumerator).Value;
                }
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    return ((IDictionaryEnumerator)_cacheEnumerator).Entry;
                }
            }

            #endregion

            #region Implementation of IEnumerator

            [PublicAPI]
            public ObservableKeyValuePair<TKey, TValue> Current
            {
                get
                {
                    var current = _cacheEnumerator.Current;
                    Contract.Assume(current.Key != null);
                    var item = new ObservableKeyValuePair<TKey, TValue>(current.Key, current.Value);
                    item.PropertyChanging += _dictionary.RaiseItemChanging;
                    return item;
                }
            }

            KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current
            {
                get
                {
                    return _cacheEnumerator.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    var current = ((IEnumerator)_cacheEnumerator).Current;
                    if (current is DictionaryEntry)
                    {
                        return current;
                    }
                    var pair = BoxingHelper<KeyValuePair<TKey, TValue>>.Unbox(current);
                    Contract.Assume(pair.Key != null);
                    var item = new ObservableKeyValuePair<TKey, TValue>(pair.Key, pair.Value);
                    item.PropertyChanging += _dictionary.RaiseItemChanging;
                    return item;
                }
            }

            [PublicAPI]
            public bool MoveNext()
            {
                return _cacheEnumerator.MoveNext();
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator)_cacheEnumerator).Reset();
            }

            #endregion

            #region Implementation of IDisposable

            [PublicAPI]
            public void Dispose()
            {
            }

            #endregion
        }

        #endregion

        #region Implementation of ISerializable

        [PublicAPI]
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _cache.GetObjectData(info, context);
        }

        #endregion

        #region Implementation of IDeserializationCallback

        [PublicAPI]
        public virtual void OnDeserialization(object sender)
        {
            _cache.OnDeserialization(sender);
        }

        #endregion

        #region Private methods

        [PublicAPI]
        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        [PublicAPI]
        [SuppressMessage("Microsoft.Contracts", "Nonnull-32-0", Justification = "Lack of contracts: GetInvokationList must ensures non-null return value")]
        protected void CheckReentrancy()
        {
            if (_monitor.IsBusy && CollectionChanged != null && CollectionChanged.GetInvocationList().Length > 1)
            {
                throw new InvalidOperationException("Observable dictionary reentrancy is not allowed");
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        [PublicAPI]
        public event PropertyChangedEventHandler PropertyChanged;

        [PublicAPI]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Raise is correct prefix")]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        [PublicAPI]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [PublicAPI]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Raise is correct prefix")]
        [SuppressMessage("Microsoft.Contracts", "Nonnull-23-0",
            Justification = "Bug in Code Contracts static checker: CollectionChanged can't be null at this line")]
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                using (BlockReentrancy())
                {
                    CollectionChanged(this, e);
                }
            }
        }

        #endregion

        #region Raise events

        private void RaiseAdded(TKey key, TValue value, int index)
        {
            ValidationHelper.NotNull(key, "key");

            RaiseAdded(new ObservableKeyValuePair<TKey, TValue>(key, value), index);
        }

        private void RaiseAdded(ObservableKeyValuePair<TKey, TValue> item, int index)
        {
            RaiseCollectionChanged();
            item.PropertyChanging += RaiseItemChanging;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        private void RaiseReplaced(TKey key, TValue oldValue, TValue newValue, int index)
        {
            ValidationHelper.NotNull(key, "key");

            RaiseItemChanged();
            var oldItem = new ObservableKeyValuePair<TKey, TValue>(key, oldValue);
            var newItem = new ObservableKeyValuePair<TKey, TValue>(key, newValue);
            newItem.PropertyChanging += RaiseItemChanging;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        private void RaiseRemoved(TKey key, TValue value, int index)
        {
            ValidationHelper.NotNull(key, "key");

            RaiseRemoved(new ObservableKeyValuePair<TKey, TValue>(key, value), index);
        }

        private void RaiseRemoved(ObservableKeyValuePair<TKey, TValue> item, int index)
        {
            RaiseCollectionChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        private void RaiseCleared(ObservableKeyValuePair<TKey, TValue>[] items)
        {
            RaiseCollectionChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, items));
        }

        private void RaiseItemChanging(object sender, PropertyChangingEventArgs e)
        {
            var item = (ObservableKeyValuePair<TKey, TValue>)sender;
            this[item.Key] = item.PreviewValue;
        }

        private void RaiseCollectionChanged()
        {
            OnPropertyChanged("Count");
            OnPropertyChanged("Item[]");
            OnPropertyChanged("Keys");
            OnPropertyChanged("Values");
        }

        private void RaiseItemChanged()
        {
            OnPropertyChanged("Item[]");
            OnPropertyChanged("Values");
        }

        #endregion
    }
}
// ReSharper restore CheckNamespace