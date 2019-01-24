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
    public class soDateTime : SchemaSimpleType<DateTime> {
        // sample 2017-07-03 13:35:00.0

        public static readonly string[] InputFormats = new string[] {
            "yyyy-MM-dd hh:mm:ss.f tt",
            "yyyy-MM-dd hh:mm:ss tt",
            "yyyy-MM-dd HH:mm:ss.f",
            "yyyy-MM-dd HH:mm:ss",

            "MM-dd-yyyy hh:mm:ss.f tt",
            "MM-dd-yyyy hh:mm:ss tt",
            "MM-dd-yyyy HH:mm:ss.f",
            "MM-dd-yyyy HH:mm:ss",

            "yyyy/MM/dd hh:mm:ss.f tt",
            "yyyy/MM/dd hh:mm:ss tt",
            "yyyy/MM/dd HH:mm:ss.f",
            "yyyy/MM/dd HH:mm:ss",

            "MM/dd/yyyy hh:mm:ss.f tt",
            "MM/dd/yyyy hh:mm:ss tt",
            "MM/dd/yyyy HH:mm:ss.f",
            "MM/dd/yyyy HH:mm:ss"
        };
        public const string OutputFormat = "yyyy-MM-dd HH:mm:ss.f";
        private static readonly CultureInfo DateCulture = new CultureInfo("en-US");

        public soDateTime() : base() { }
        public soDateTime(DateTime value) : base(value) { }
        public soDateTime(DateTime? value) : base(value) { }

        protected override string GetText(DateTime? source) {
            if (source.HasValue) {
                return source.Value.ToString(OutputFormat);
            }
            else return null;
        }

        protected override DateTime? Parse(string source) {
            DateTime result;
            if (Text != null && DateTime.TryParseExact(Text, InputFormats, DateCulture, DateTimeStyles.None, out result))
                return result;
            else return null;
        }

        public static implicit operator soDateTime(DateTime v) {
            return new soDateTime(v);
        }

        public static implicit operator soDateTime(DateTime? v) {
            return new soDateTime(v);
        }

        public static implicit operator DateTime? (soDateTime v) {
            return v._value;
        }

        public override string ToString() {
            return this.HasValue ? this._value.Value.ToString(OutputFormat) : null;
        }

        public string ToString(string format) {
            return this.HasValue ? this._value.Value.ToString(format) : null;
        }

    }

}
