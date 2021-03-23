namespace BuildingRegistry.Api.Legacy.BuildingUnit.Responses
{
    using BuildingRegistry.Api.Legacy.Infrastructure;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "BuildingUnitShaclShape", Namespace = "")]
    public class BuildingUnitShaclShapeResponse
    {
        [DataMember(Name = "@context", Order = 1)]
        [JsonProperty(Required = Required.Always)]
        public readonly BuildingUnitShaclContext Context = new BuildingUnitShaclContext();

        [DataMember(Name = "@id", Order = 2)]
        [JsonProperty(Required = Required.Always)]
        public Uri Id { get; set; }

        [DataMember(Name = "@type", Order = 3)]
        [JsonProperty(Required = Required.Always)]
        public readonly string Type = "sh:NodeShape";

        [DataMember(Name = "sh:property", Order = 4)]
        [JsonProperty(Required = Required.Always)]
        public readonly List<BuildingUnitShaclProperty> Shape = BuildingUnitShaclShape.GetShape();
    }

    public class BuildingUnitShaclShape
    {
        public static List<BuildingUnitShaclProperty> GetShape()
            => new List<BuildingUnitShaclProperty>()
            {
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "dct:isVersionOf",
                    NodeKind = "sh:IRI",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "prov:generatedAtTime",
                    DataType = "xsd:dateTime",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "adms:versionNotes",
                    DataType = "xsd:string",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "gebouw:Gebouweenheid.geometrie",
                    Node = new BuildingUnitNodeProperty
                    {
                        Nodes = new List<BuildingUnitShaclProperty>
                        {
                            new BuildingUnitShaclProperty
                            {
                                PropertyPath = "geo:asGML",
                                DataType = "geo:gmlLiteral",
                                MinimumCount = 1,
                                MaximumCount = 1
                            },
                            new BuildingUnitShaclProperty
                            {
                                PropertyPath = "gebouw:methode",
                                DataType = "skos:Concept",
                                MinimumCount = 1,
                                MaximumCount = 1
                            }
                        }
                    }
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "gebouw:Gebouweenheid.adres",
                    NodeKind = "sh:IRI",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "gebouw:functie",
                    DataType = "skos:Concept",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "gebouw:Gebouweenheid.status",
                    DataType = "skos:Concept",
                    MinimumCount = 1,
                    MaximumCount = 1
                },
                new BuildingUnitShaclProperty
                {
                    PropertyPath = "br:Gebouweenheid.behoordTot",
                    NodeKind = "sh:IRI",
                    MinimumCount = 1,
                    MaximumCount = 1
                }
            };
    }

    public class BuildingUnitShaclProperty
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
        public BuildingUnitNodeProperty? Node { get; set; }
    }

    public class BuildingUnitNodeProperty
    {
        [JsonProperty("sh:properties")]
        public List<BuildingUnitShaclProperty> Nodes { get; set; }
    }

    public class BuildingUnitShaclShapeResponseExamples : IExamplesProvider<BuildingUnitShaclShapeResponse>
    {
        private readonly LinkedDataEventStreamConfiguration _configuration;

        public BuildingUnitShaclShapeResponseExamples(LinkedDataEventStreamConfiguration configuration)
            => _configuration = configuration;

        public BuildingUnitShaclShapeResponse GetExamples()
            => new BuildingUnitShaclShapeResponse
            {
                Id = new Uri($"{_configuration.BuildingUnitApiEndpoint}/shape")
            };
    }
}
