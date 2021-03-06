using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg
{

    class SchemaSimpleTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType is ISchemaSimpleType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ISchemaSimpleType)
            {
                var typedValue = value as ISchemaSimpleType;
                if (typedValue.HasValue)
                {
                    JToken token = JToken.FromObject(typedValue.Value, serializer);
                    token.WriteTo(writer);
                    return;
                }
            }

            writer.WriteNull();
        }
    }
}
