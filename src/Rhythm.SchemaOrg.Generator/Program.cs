using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Beginning automatic schema generation");
            Console.Out.WriteLine($"Source: {ConfigurationManager.AppSettings["SchemaSource"]}");
            Console.Out.WriteLine($"Destination: {ConfigurationManager.AppSettings["SchemaOutputPath"]}");
            Console.Out.WriteLine($"Destination Namespace: {ConfigurationManager.AppSettings["SchemaNamespace"]}");
            Console.Out.WriteLine();

            var client = new HttpClient();
            var schemaTask = client.GetStringAsync(ConfigurationManager.AppSettings["SchemaSource"]);
            schemaTask.Wait();
            if (schemaTask.IsFaulted)
            {
                Console.Error.WriteLine("Schema retrieval failed | {0}", schemaTask.Exception.Message);
            }
            else
            {
                Console.Out.WriteLine("Schema retrival succeeded");

                var schemaRaw = schemaTask.Result;
                var schema = JsonConvert.DeserializeObject(schemaRaw) as JObject;

                var dataModel = new SchemaDataModel(schema["@graph"]);
                Console.Out.WriteLine($"Found {dataModel.Enumerations.Count()} enumerations");
                Console.Out.WriteLine($"Found {dataModel.EnumerationMembers.Count()} enumeration members");
                Console.Out.WriteLine($"Found {dataModel.Models.Count()} models");
                Console.Out.WriteLine($"Found {dataModel.ModelProperties.Count()} model properties");

                var generator = new SchemaGenerator()
                {
                    Namespace = ConfigurationManager.AppSettings["SchemaNamespace"],
                    OutputPath = ConfigurationManager.AppSettings["SchemaOutputPath"],
                    DataModel = dataModel
                };

                generator.Initialize();
                generator.GenerateEnumerations();
                generator.GenerateModels();
            }
        }
    }
}
