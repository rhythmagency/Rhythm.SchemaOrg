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
    public class soTime : SchemaSimpleType<TimeSpan> {
        // sample 2017-07-03 13:35:00.0

        public const string DateFormat = "hh\\:mm\\:ss";
        private static readonly CultureInfo DateCulture = new CultureInfo("en-US");

        public soTime() : base() { }
        public soTime(TimeSpan value) : base(value) { }
        public soTime(TimeSpan? value) : base(value) { }

        protected override string GetText(TimeSpan? source) {
            if (source.HasValue) {
                return source.Value.ToString(DateFormat);
            }
            else return null;
        }

        protected override TimeSpan? Parse(string source) {
            DateTime result;
            if (Text != null && DateTime.TryParseExact(Text, DateFormat, DateCulture, DateTimeStyles.None, out result))
                return result - result.Date;
            else return null;
        }

        public static implicit operator soTime(TimeSpan v) {
            return new soTime(v);
        }

        public static implicit operator soTime(TimeSpan? v) {
            return new soTime(v);
        }

        public static implicit operator TimeSpan? (soTime v) {
            return v._value;
        }

        public override string ToString() {
            return this._value.HasValue ?
                this._value.Value.ToString("c") :
                !string.IsNullOrEmpty(this.Text) ?
                    this.Text : "";
        }

        public string ToString(string format) {
            return this.HasValue ?
                this._value.Value.ToString(format) :
                !string.IsNullOrEmpty(this.Text) ?
                    this.Text : "";
        }

    }

}
