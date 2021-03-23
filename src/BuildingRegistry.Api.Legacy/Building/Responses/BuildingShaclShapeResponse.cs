namespace BuildingRegistry.Api.Legacy.Building.Responses
{
    using BuildingRegistry.Api.Legacy.Infrastructure;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "BuildingShaclShape", Namespace = "")]
    public class BuildingShaclShapeResponse
    {
        [DataMember(Name = "@context", Order = 1)]
        [JsonProperty(Required = Required.Always)]
        public readonly BuildingShaclContext Context = new BuildingShaclContext();

        [DataMember(Name = "@id", Order = 2)]
        [JsonProperty(Required = Required.Always)]
        public Uri Id { get; set; }

        [DataMember(Name = "@type", Order = 3)]
        [JsonProperty(Required = Required.Always)]
        public readonly string Type = "sh:NodeShape";

        [DataMember(Name = "sh:property", Order = 4)]
        [JsonProperty(Required = Required.Always)]
        public readonly List<BuildingShaclProperty> Shape = BuildingShaclShape.GetShape();
    }

    public class BuildingShaclShape
    {
        public static List<BuildingShaclProperty> GetShape()
            => new List<BuildingShaclProperty>()
            {
                new BuildingShaclProperty
                {
                    PropertyPath = "dct:isVersionOf",
                    NodeKind = "sh:IRI",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingShaclProperty
                {
                    PropertyPath = "prov:generatedAtTime",
                    DataType = "xsd:dateTime",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingShaclProperty
                {
                    PropertyPath = "adms:versionNotes",
                    DataType = "xsd:string",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingShaclProperty
                {
                    PropertyPath = "gebouw:Gebouw.geometrie",
                    Node = new BuildingNodeProperty
                    {
                        Nodes = new List<BuildingShaclProperty>
                        {
                            new BuildingShaclProperty
                            {
                                PropertyPath = "geo:asGML",
                                DataType = "geo:gmlLiteral",
                                MinimumCount = 1,
                                MaximumCount = 1
                            },
                            new BuildingShaclProperty
                            {
                                PropertyPath = "gebouw:methode",
                                DataType = "skos:Concept",
                                MinimumCount = 1,
                                MaximumCount = 1
                            }
                        }
                    }
                },
                new BuildingShaclProperty
                {
                    PropertyPath = "gebouw:Gebouw.status",
                    DataType = "skos:Concept",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingShaclProperty
                {
                    PropertyPath = "gebouw:bestaatUit",
                    NodeKind = "sh:IRI"
                }
            };
    }

    public class BuildingShaclProperty
    {
        [JsonProperty("sh:path")]
        public string PropertyPath { get; set; }

        [JsonProperty("sh:datatype", NullValueHandling = NullValueHandling.Ignore)]
        public string? DataType { get; set; }

        [JsonProperty("sh:nodeKind", NullValueHandling = NullValueHandling.Ignore)]
        public string? NodeKind { get; set; }

        [JsonProperty(PropertyName = "sh:minCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinimumCount { get; set; }

        [JsonProperty(PropertyName = "sh:maxCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaximumCount { get; set; }

        [JsonProperty("sh:node", NullValueHandling = NullValueHandling.Ignore)]
        public BuildingNodeProperty? Node { get; set; }
    }

    public class BuildingNodeProperty
    {
        [JsonProperty("sh:properties")]
        public List<BuildingShaclProperty> Nodes { get; set; }
    }

    public class BuildingShaclShapeResponseExamples : IExamplesProvider<BuildingShaclShapeResponse>
    {
        private readonly LinkedDataEventStreamConfiguration _configuration;

        public BuildingShaclShapeResponseExamples(LinkedDataEventStreamConfiguration configuration)
            => _configuration = configuration;

        public BuildingShaclShapeResponse GetExamples()
            => new BuildingShaclShapeResponse
            {
                Id = new Uri($"{_configuration.ApiEndpoint}/shape")
            };
    }
}
