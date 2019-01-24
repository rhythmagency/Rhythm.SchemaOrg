using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg
{
    public class TypeEquivalent<T> : IEquatable<T> where T : struct
    {

        public TypeEquivalent() { _value = null; }
        public TypeEquivalent(T value) { _value = value; }
        public TypeEquivalent(T? value) { _value = value; }

        protected T? _value;

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        #region Nullable<T> imitation

        public bool HasValue { get { return _value.HasValue; } }

        public T Value
        {
            get { return _value.Value; }
        }

        public T GetValueOrDefault()
        {
            return _value.GetValueOrDefault();
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return _value.GetValueOrDefault(defaultValue);
        }

        #endregion

        #region IEquatable

        public override bool Equals(object obj)
        {
            if (obj is TypeEquivalent<T>)
                return Equals((TypeEquivalent<T>)obj);
            if (obj is T)
                return Equals((T)obj);
            else if (obj is T?)
                return Equals((T?)obj);
            else
                return Object.Equals(this, obj);
        }

        public bool Equals(TypeEquivalent<T> other)
        {
            if (_value.HasValue && other != null)
                return _value.Equals(other._value);
            else
                return Object.Equals(this, other);
        }

        public bool Equals(T? other)
        {
            if (_value.HasValue)
                return _value.Equals(other);
            else
                return Object.Equals(_value, other);
        }

        public bool Equals(T other)
        {
            if (_value.HasValue)
                return _value.Value.Equals(other);
            else
                return Object.Equals(_value, other);
        }

        #endregion

        public static implicit operator TypeEquivalent<T>(T source)
        {
            return new TypeEquivalent<T>(source);
        }

        public static implicit operator TypeEquivalent<T>(T? source)
        {
            return new TypeEquivalent<T>(source);
        }

        public static implicit operator T?(TypeEquivalent<T> source)
        {
            return source._value;
        }

    }
}
