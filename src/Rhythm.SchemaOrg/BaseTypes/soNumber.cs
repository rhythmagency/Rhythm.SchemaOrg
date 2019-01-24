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
    public class soNumber : SchemaSimpleType<decimal> {

        public soNumber() : base() { }
        public soNumber(decimal value) : base(value) { }
        public soNumber(decimal? value) : base(value) { }

        protected override string GetText(decimal? source) {
            if (source.HasValue)
                return source.Value.ToString();
            else return null;
        }

        protected override decimal? Parse(string source) {
            decimal result;
            if (decimal.TryParse(Text, out result))
                return result;
            else return null;
        }

        public static implicit operator soNumber(decimal v) {
            return new soNumber(v);
        }

        public static implicit operator soNumber(decimal? v) {
            return new soNumber(v);
        }

        public static implicit operator decimal? (soNumber v) {
            return v._value;
        }

    }

}
