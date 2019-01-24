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
    public class soBoolean : SchemaSimpleType<bool> {

        protected const string True = "true";
        protected const string False = "false";

        public soBoolean() : base() { }
        public soBoolean(bool value) : base(value) { }
        public soBoolean(bool? value) : base(value) { }

        protected override string GetText(bool? source) {
            if (source.HasValue) {
                return source.Value ? True : False;
            }
            else return null;
        }

        protected override bool? Parse(string source) {
            switch (source?.ToLower()) {
                case False: return false;
                case True: return true;
                default: return null;
            }
        }

        public static implicit operator soBoolean(bool v) {
            return new soBoolean(v);
        }

        public static implicit operator soBoolean(bool? v) {
            return new soBoolean(v);
        }

        public static implicit operator bool?(soBoolean v) {
            return v._value;
        }

        public static implicit operator bool (soBoolean v) {
            return v._value.GetValueOrDefault();
        }

    }

}
