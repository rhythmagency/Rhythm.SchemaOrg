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
    public class soFloat : SchemaSimpleType<double> {

        public soFloat() : base() { }
        public soFloat(double value) : base(value) { }
        public soFloat(double? value) : base(value) { }

        protected override string GetText(double? source) {
            if (source.HasValue)
                return source.Value.ToString();
            else return null;
        }

        protected override double? Parse(string source) {
            double result;
            if (double.TryParse(Text, out result))
                return result;
            else return null;
        }

        public static implicit operator soFloat(double v) {
            return new soFloat(v);
        }

        public static implicit operator soFloat(double? v) {
            return new soFloat(v);
        }

        public static implicit operator double? (soFloat v) {
            return v._value;
        }

    }

}
