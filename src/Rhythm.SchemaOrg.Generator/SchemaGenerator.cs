using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg.Generator
{
    public class SchemaGenerator
    {
        public string Namespace { get; set; }
        public string OutputPath { get; set; }
        public SchemaDataModel DataModel { get; set; }

        public DirectoryInfo EnumerationDirectory { get; private set; }
        public DirectoryInfo ModelDirectory { get; private set; }

        public void Initialize()
        {
            EnumerationDirectory = new DirectoryInfo(OutputPath + "\\Enumerations");
            if (!EnumerationDirectory.Exists)
                EnumerationDirectory.Create();

            ModelDirectory = new DirectoryInfo(OutputPath + "\\Models");
            if (!ModelDirectory.Exists)
                ModelDirectory.Create();

            // TODO: clean output directories
            //var existingFiles = ModelDirectory.EnumerateFiles();
            //foreach (var existingFile in existingFiles)
            //    existingFile.Delete();
        }

        public void GenerateModels()
        {
            foreach (var item in DataModel.Models)
            {
                GenerateModel(item as JObject);
            }
        }

        public void GenerateModel(JObject modelData)
        {
            var modelName = modelData["rdfs:label"]?.ToObject<string>();

            if (String.IsNullOrEmpty(modelName) || TypeBlacklist.Contains(modelName.ToLower()))
                return;

            modelName = CaptalizeFirstLetter(modelName);
            Console.Out.WriteLine($"Creating models for {modelName}");

            var schemaSourceUrl = modelData["@id"]?.ToObject<string>();

            var modelDescription = CleanDescription(modelData?["rdfs:comment"]?.ToObject<string>());

            var parentName = GetNameFromId(DataModel.GetItemParentFor(modelData));
            if (String.IsNullOrWhiteSpace(parentName))
            {
                parentName = "SchemaModelBase";
            }
            else if (IsGeneratedEnum(parentName))
            {
                parentName = $"TypeEquivalent<{parentName}>";
            }

            var interfaceProperties = DataModel.GetModelPropertiesFor(modelData);
            Console.Out.WriteLine($"  found {interfaceProperties.Count()} properties");

            Console.Out.WriteLine($"  creating class");
            using (var classFile = new StreamWriter(File.Create($"{ModelDirectory.FullName}\\{modelName}.cs"), Encoding.UTF8))
            {
                classFile.WriteLine($"namespace {Namespace}.Models");
                classFile.WriteLine( "{");
                classFile.WriteLine($"    using System;");
                classFile.WriteLine($"    using Newtonsoft.Json;");
                classFile.WriteLine($"    using {Namespace}.BaseTypes;");
                classFile.WriteLine($"    using {Namespace}.Enumerations;");
                classFile.WriteLine();
                classFile.WriteLine($"    /// <summary>{modelDescription}</summary>");
                classFile.WriteLine($"    [SchemaSource(\"{schemaSourceUrl}\")]");
                classFile.WriteLine($"    public class {modelName} : {parentName}");
                classFile.WriteLine( "    {");

                foreach (var item in interfaceProperties)
                {
                    var propertyDescription = CleanDescription(item?["rdfs:comment"]?.ToObject<string>());
                    var propertyName = item?["rdfs:label"]?.ToObject<string>();
                    string originalName = null;
                    if (PropertyNameBlacklist.Contains(propertyName))
                    {
                        originalName = propertyName;
                        propertyName = "@" + propertyName;
                    }
                    var propertyType = GetPropertyDataType(item);

                    classFile.WriteLine();
                    classFile.WriteLine($"        /// <summary>{propertyDescription}</summary>");
                    if (!String.IsNullOrWhiteSpace(originalName))
                    {
                        classFile.WriteLine($"        [JsonProperty(\"{originalName}\")]");
                    }
                    classFile.WriteLine($"        public {propertyType} {propertyName} {{ get; set; }}");
                }

                classFile.WriteLine();
                classFile.WriteLine( "    }");
                classFile.WriteLine( "}");
            }
        }

        public void GenerateEnumerations()
        {
            foreach (var item in DataModel.Enumerations)
            {
                GenerateEnumeration(item as JObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumData"></param>
        public void GenerateEnumeration(JObject enumData)
        {
            var enumName = enumData["rdfs:label"]?.ToObject<string>();

            if (String.IsNullOrEmpty(enumName) || TypeBlacklist.Contains(enumName.ToLower()))
                return;

            enumName = CaptalizeFirstLetter(enumName);
            Console.Out.WriteLine($"Creating enumeration for {enumName}");

            var schemaSourceUrl = enumData["@id"]?.ToObject<string>();
            var enumDescription = CleanDescription(enumData["rdfs:comment"]?.ToObject<string>());

            var parentName = GetNameFromId(DataModel.GetItemParentFor(enumData));
            if (parentName != "Enumeration")
            {
                parentName = $"TypeEquivalent<{parentName}>";
            }


            var members = DataModel.GetEnumerationMembersFor(enumData);
            Console.Out.WriteLine($"  found {members.Count()} members");

            using (var enumFile = new StreamWriter(File.Create($"{EnumerationDirectory.FullName}\\{enumName}.cs"), Encoding.UTF8))
            {
                enumFile.WriteLine($"namespace {Namespace}.Enumerations");
                enumFile.WriteLine( "{");
                enumFile.WriteLine( "    /// <summary>");
                enumFile.WriteLine($"    /// {enumDescription}");
                enumFile.WriteLine( "    /// </summary>");
                enumFile.WriteLine($"    [SchemaSource(\"{schemaSourceUrl}\")]");
                if (parentName == "Enumeration")
                {
                    enumFile.WriteLine($"    public enum {enumName}");
                }
                else
                {
                    enumFile.WriteLine($"    public class {enumName} : {parentName}");
                }
                enumFile.WriteLine( "    {");

                foreach (var item in members)
                {
                    var memberDescription = CleanDescription(item?["rdfs:comment"]?.ToObject<string>());
                    var memberName = item?["rdfs:label"]?.ToObject<string>();

                    string originalName = null;
                    if (PropertyNameBlacklist.Contains(memberName))
                    {
                        originalName = memberName;
                        memberName = "@" + memberName;
                    }

                    enumFile.WriteLine();
                    enumFile.WriteLine($"        /// <summary>{memberDescription}</summary>");
                    //if (!String.IsNullOrWhiteSpace(originalName))
                    //{
                    //    enumFile.WriteLine($"        [JsonProperty(\"{originalName}\")]");
                    //}
                    enumFile.WriteLine($"        {memberName},");
                }

                enumFile.WriteLine();
                enumFile.WriteLine( "    }");
                enumFile.WriteLine( "}");
            }
        }

        private static string CaptalizeFirstLetter(string source)
        {
            if (String.IsNullOrWhiteSpace(source))
                return null;
            return source.Insert(0, source.Substring(0, 1).ToUpper()).Remove(1, 1);
        }

        private static string GetNameFromId(string source)
        {
            if (String.IsNullOrWhiteSpace(source))
            {
                return null;
            }
            return Regex.Replace(source, "^https?://schema.org/", "");
        }

        private static string CleanDescription(string source)
        {
            if (String.IsNullOrWhiteSpace(source))
            {
                return null;
            }
            return Regex.Replace(source, "[\r\n]", "");
        }

        private string GetPropertyDataType(JObject source)
        {
            var typeIds = DataModel.GetRangeIdsFor(source);
            if (typeIds == null || typeIds.Count() < 1)
                return "object";
            if (typeIds.Count() == 1)
                return ConvertIdToTypeName(typeIds.First());
            return $"Junction<{typeIds.Select(i => ConvertIdToTypeName(i)).Aggregate((c, n) => c + "," + n)}>";
        }

        private string ConvertIdToTypeName(string source)
        {
            if (String.IsNullOrWhiteSpace(source))
                return null;
            switch (source)
            {
                case "http://schema.org/Boolean":
                    return "soBoolean";

                case "http://schema.org/Date":
                    return "soDate";

                case "http://schema.org/DateTime":
                    return "soDateTime";

                case "http://schema.org/Number":
                    return "soNumber";

                case "http://schema.org/Float":
                    return "soFloat";

                case "http://schema.org/Integer":
                    return "soInteger";

                case "http://schema.org/Text":
                    return "string";

                case "http://schema.org/URL":
                    return "Uri";

                case "http://schema.org/Time":
                    return "soTime";

                case "http://schema.org/DayOfWeek":
                    return $"{Namespace}.Enumerations.DayOfWeek";

                default:
                    return CaptalizeFirstLetter(GetNameFromId(source));
            }
        }

        private static readonly string[] TypeBlacklist = new string[]
        {
            "boolean",
            "date",
            "datatype",
            "datetime",
            "enumeration",
            "float",
            "integer",
            "number",
            "text",
            "time",
            "url",
        };

        private static readonly string[] PropertyNameBlacklist = new string[]
        {
            "object",
            "event",
        };

        private bool IsGeneratedEnum(string name)
        {
            return DataModel.Enumerations
                .Select(i => CaptalizeFirstLetter(GetNameFromId(i?["rdfs:label"]?.ToObject<string>())))
                .Any(n => n == name);
        }

    }
}
