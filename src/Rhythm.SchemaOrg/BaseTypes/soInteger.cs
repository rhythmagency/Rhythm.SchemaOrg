using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rhythm.SchemaOrg.BaseTypes
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class soInteger : SchemaSimpleType<long> {

        public soInteger() : base() { }
        public soInteger(int value) : base(value) { }
        public soInteger(int? value) : base(value) { }
        public soInteger(long value) : base(value) { }
        public soInteger(long? value) : base(value) { }

        protected override string GetText(long? source) {
            if (source.HasValue)
                return source.Value.ToString();
            else return null;
        }

        protected override long? Parse(string source) {
            long result;
            if (long.TryParse(Text, out result))
                return result;
            else return null;
        }

        public static implicit operator soInteger(int v)
        {
            return new soInteger(v);
        }

        public static implicit operator soInteger(int? v)
        {
            return new soInteger(v);
        }

        public static implicit operator int? (soInteger v)
        {
            return v._value.HasValue ? (int?)v._value.Value : (int?)null;
        }

        public static implicit operator soInteger(long v) {
            return new soInteger(v);
        }

        public static implicit operator soInteger(long? v) {
            return new soInteger(v);
        }

        public static implicit operator long? (soInteger v) {
            return v._value;
        }

    }

}
