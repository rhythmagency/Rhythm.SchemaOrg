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
    public abstract class SchemaEnumeration
    {
        public string Value { get; private set; }

        protected SchemaEnumeration(string value)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        #region Nullable<T> imitation

        public bool HasValue { get { return String.IsNullOrWhiteSpace(Value); } }

        #endregion

        #region IEquatable

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            else if (obj is SchemaEnumeration) return Equals(this, obj);
            else return false;
        }

        public bool Equals(SchemaEnumeration obj1, SchemaEnumeration obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            else if (obj1 == null || obj2 == null) return false;
            else if (obj1.GetType() != obj2.GetType()) return false;
            else return String.Equals(obj1.Value, obj2.Value);
        }

        #endregion
    }
}
