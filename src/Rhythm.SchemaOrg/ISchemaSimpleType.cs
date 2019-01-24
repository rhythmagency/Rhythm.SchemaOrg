using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg
{
    public interface ISchemaSimpleType
    {
        bool HasValue { get; }
        object Value { get; }
        object GetValueOrDefault();
        object GetValueOrDefault(object defaultValue);
    }
}