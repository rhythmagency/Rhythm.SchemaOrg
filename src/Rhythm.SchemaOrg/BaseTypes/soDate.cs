using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rhythm.SchemaOrg.BaseTypes
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class soDate : SchemaSimpleType<DateTime> {
        // sample 2017-07-03 13:35:00.0

		public static readonly string[] InputFormats = new string[] {
            "yyyy-MM-dd",
            "MM-dd-yyyy",
            "yyyy/MM/dd",
            "MM/dd/yyyy"
        };
        public const string OutputFormat = "yyyy-MM-dd";

        private static readonly CultureInfo DateCulture = new CultureInfo("en-US");

        public soDate() : base() { }
        public soDate(DateTime value) : base(value) { }
        public soDate(DateTime? value) : base(value) { }

        protected override string GetText(DateTime? source) {
            if (source.HasValue) {
                return source.Value.ToString(OutputFormat);
            }
            else return null;
        }

        protected override DateTime? Parse(string source) {
            DateTime result;
            if (source != null && DateTime.TryParseExact(source, InputFormats, DateCulture, DateTimeStyles.None, out result))
                return result;
            else return null;
        }

        public static explicit operator soDate(DateTime v) {
            return new soDate(v);
        }

        public static explicit operator soDate(DateTime? v) {
            return new soDate(v);
        }

        public override string ToString() {
            return this.HasValue ? this._value.Value.ToString(OutputFormat) : null;
        }

        public string ToString(string format) {
            return this.HasValue ? this._value.Value.ToString(format) : null;
        }

    }

}
