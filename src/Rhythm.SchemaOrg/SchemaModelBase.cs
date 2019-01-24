using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rhythm.SchemaOrg
{
    public abstract class SchemaModelBase
    {
        [JsonProperty("@id")]
        public virtual string SchemaId { get; }

        [JsonProperty("@type")]
        public virtual string SchemaType { get; }

        public IHtmlString ToScriptTag()
        {
            var json = JsonConvert.SerializeObject(this);
            return new HtmlString($"<script type='application/ld+json'>{json}</script>");
        }
    }
}
