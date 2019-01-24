using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Rhythm.SchemaOrg.Generator
{
    public class SchemaDataModel
    {
        private JArray _data;

        public IEnumerable<JObject> Enumerations { get; private set; }
        public IEnumerable<JObject> EnumerationMembers { get; private set; }
        public IEnumerable<JObject> Models { get; private set; }
        public IEnumerable<JObject> ModelProperties { get; private set; }

        public SchemaDataModel(JToken data)
        {
            this._data = data as JArray;

            Enumerations = _data
                .Where(i => GetItemParentFor(i) == "http://schema.org/Enumeration")
                .Cast<JObject>()
                .ToList();

            var enumerationIds = Enumerations
                .Select(i => i["@id"]?.ToObject<string>())
                .Where(i => !String.IsNullOrWhiteSpace(i))
                .ToList();

            EnumerationMembers = _data
                .Where(i => GetItemTypes(i).Any(t => enumerationIds.Contains(t)))
                .Cast<JObject>()
                .ToList();

            Models = _data
                .Where(i => GetItemTypes(i).Contains("rdfs:Class"))
                .Cast<JObject>()
                .Except(Enumerations)
                .ToList();

            ModelProperties = _data
                .Where(i => GetItemTypes(i).Contains("rdf:Property"))
                .Cast<JObject>()
                .ToList();

        }

        public IEnumerable<JObject> GetEnumerationMembersFor(JObject enumeration)
        {
            var enumId = GetItemId(enumeration);
            return EnumerationMembers.Where(i => GetItemTypes(i).Contains(enumId));
        }

        public IEnumerable<JObject> GetModelPropertiesFor(JObject model)
        {
            var modelId = GetItemId(model);
            return ModelProperties.Where(i => GetDomainIdsFor(i).Contains(modelId));
        }

        /// <summary>
        /// Returns a property type's domain IDs, i.e. the object types which use it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<string> GetDomainIdsFor(JObject model)
        {
            return GetIdList(model, "http://schema.org/domainIncludes");
        }

        /// <summary>
        /// Returns a property type's range IDs, i.e. the object types it is allowed to contain.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<string> GetRangeIdsFor(JObject model)
        {
            return GetIdList(model, "http://schema.org/rangeIncludes");
        }

        private static string GetItemId(JToken source)
        {
            return (source as JObject)?["@id"]?.ToObject<string>();
        }

        public string GetItemParentFor(JToken source)
        {
            return ((source as JObject)?["rdfs:subClassOf"] as JObject)?["@id"]?.ToObject<string>();
        }

        private static IEnumerable<string> GetIdList(JObject source, string property)
        {
            var value = source[property];
            if (value == null)
            {
                return Enumerable.Empty<string>();
            }
            else if (value is JArray)
            {
                var typedObject = source[property] as JArray;
                return typedObject.Select(i => (i as JObject)["@id"].ToObject<string>());
            }
            else if (value is JObject)
            {
                var typedObject = source[property] as JObject;
                return new[] { typedObject["@id"].ToObject<string>() };
            }
            else return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> GetItemTypes(JToken source)
        {
            var value = (source as JObject)?["@type"];
            if (value == null)
            {
                return Enumerable.Empty<string>();
            }
            if (value is JArray)
            {
                return (value as JArray).Select(i => i.ToObject<string>());
            }
            else if (value.Type == JTokenType.String)
            {
                return new string[] { value.ToObject<string>() };
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

    }
}
