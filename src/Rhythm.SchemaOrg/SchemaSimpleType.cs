using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rhythm.SchemaOrg
{
    [JsonConverter(typeof(SchemaSimpleTypeJsonConverter))]
    public abstract class SchemaSimpleType<T> : ISchemaSimpleType, IEquatable<T> where T : struct
    {
        public SchemaSimpleType() { _value = null; }
        public SchemaSimpleType(T value) { _value = value; }
        public SchemaSimpleType(T? value) { _value = value; }

        protected T? _value;

        protected abstract T? Parse(string source);

        protected abstract string GetText(T? source);

        public string Text
        {
            get { return GetText(_value); }
            set { _value = Parse(value); }
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return Text;
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

        #region ISchemaSimpleType implementation

        bool ISchemaSimpleType.HasValue => this.HasValue;

        object ISchemaSimpleType.Value => this.Value;

        object ISchemaSimpleType.GetValueOrDefault()
        {
            return this.GetValueOrDefault();
        }

        object ISchemaSimpleType.GetValueOrDefault(object defaultValue)
        {
            return this.GetValueOrDefault((T)defaultValue);
        }

        #endregion

        #region IEquatable

        public override bool Equals(object obj)
        {
            if (obj is SchemaSimpleType<T>)
                return Equals((SchemaSimpleType<T>)obj);
            if (obj is T)
                return Equals((T)obj);
            else if (obj is T?)
                return Equals((T?)obj);
            else
                return Object.Equals(this, obj);
        }

        public bool Equals(SchemaSimpleType<T> other)
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
    }
}
