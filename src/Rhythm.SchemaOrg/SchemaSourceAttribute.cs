using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface)]
    public class SchemaSourceAttribute : Attribute
    {
        public string Path { get; private set; }

        public SchemaSourceAttribute(string path)
        {
            Path = path;
        }
    }
}
