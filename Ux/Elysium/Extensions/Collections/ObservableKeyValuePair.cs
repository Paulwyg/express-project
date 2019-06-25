// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableKeyValuePair.cs" company="Alex F. Sherman & Codeplex community">
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
//   Defines the ObservableKeyValuePair type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

using Elysium.Extensions;

using JetBrains.Annotations;

// ReSharper disable CheckNamespace
namespace System.Collections.ObjectModel
{
    [PublicAPI]
    [Serializable]
    public sealed class ObservableKeyValuePair<TKey, TValue> : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private fields

        private readonly IEqualityComparer<TKey> _comparer;

        private readonly TKey _key;
        private TValue _value;

        internal TValue PreviewValue;

        #endregion

        #region Constructor

        [PublicAPI]
        public ObservableKeyValuePair(TKey key, TValue value, IEqualityComparer<TKey> comparer)
        {
            ValidationHelper.NotNull(key, "key");

            _key = key;
            _value = value;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        [PublicAPI]
// ReSharper disable IntroduceOptionalParameters.Global
        public ObservableKeyValuePair(TKey key, TValue value) : this(key, value, null)
// ReSharper restore IntroduceOptionalParameters.Global
        {
            ValidationHelper.NotNull(key, "key");
        }

        #endregion

        #region Properties

        [PublicAPI]
        public TKey Key
        {
            get
            {
                Contract.Ensures(Contract.Result<TKey>() != null);
                return _key;
            }
        }

        [PublicAPI]
        public TValue Value
        {
            get { return _value; }
            set
            {
                if (!Equals(_value, value))
                {
                    var observableChangingValue = _value as INotifyPropertyChanging;
                    if (observableChangingValue != null)
                    {
                        observableChangingValue.PropertyChanging -= OnValueChanging;
                    }
                    var observableChangedValue = _value as INotifyPropertyChanged;
                    if (observableChangedValue != null)
                    {
                        observableChangedValue.PropertyChanged -= OnValueChanged;
                    }
                    observableChangingValue = value as INotifyPropertyChanging;
                    if (observableChangingValue != null)
                    {
                        observableChangingValue.PropertyChanging += OnValueChanging;
                    }
                    observableChangedValue = value as INotifyPropertyChanged;
                    if (observableChangedValue != null)
                    {
                        observableChangedValue.PropertyChanged += OnValueChanged;
                    }

                    PreviewValue = value;

                    RaisePropertyChanging("Value");
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        private void OnValueChanging(object sender, PropertyChangingEventArgs e)
        {
            RaisePropertyChanging("Value");
        }

        private void OnValueChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Value");
        }

        #endregion

        #region Implementation of INotifyPropertyChanging

        [PublicAPI]
        public event PropertyChangingEventHandler PropertyChanging;

        private void RaisePropertyChanging(string propertyName)
        {
            var handler = PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged

        [PublicAPI]
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Overrides

        [SuppressMessage("Microsoft.Contracts", "Requires-1-34", Justification = "Unknown requires")]
        [JetBrains.Annotations.Pure]
        [Diagnostics.Contracts.Pure]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var pair = obj as ObservableKeyValuePair<TKey, TValue>;
            return pair != null && Equals(pair);
        }

        [JetBrains.Annotations.Pure]
        [Diagnostics.Contracts.Pure]
        private bool Equals(ObservableKeyValuePair<TKey, TValue> other)
        {
            return _comparer.Equals(_key, other._key) && EqualityComparer<TValue>.Default.Equals(_value, other._value);
        }

        [JetBrains.Annotations.Pure]
        [Diagnostics.Contracts.Pure]
        public override int GetHashCode()
        {
            unchecked
            {
// ReSharper disable NonReadonlyFieldInGetHashCode
                return (_comparer.GetHashCode(_key) * 397) ^ EqualityComparer<TValue>.Default.GetHashCode(_value);
// ReSharper restore NonReadonlyFieldInGetHashCode
            }
        }
        
        [JetBrains.Annotations.Pure]
        [Diagnostics.Contracts.Pure]
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            stringBuilder.Append(Key);
            stringBuilder.Append(", ");
            stringBuilder.Append(Value);
            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        #endregion

        #region Contracts

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(_key != null);
        }

        #endregion
    }
}
// ReSharper restore CheckNamespace