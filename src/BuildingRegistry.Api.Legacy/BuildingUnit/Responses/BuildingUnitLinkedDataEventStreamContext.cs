namespace BuildingRegistry.Api.Legacy.BuildingUnit.Responses
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "BuildingUnitContext")]
    public class BuildingUnitLinkedDataEventStreamContext
    {
        [DataMember(Name = "tree")]
        public readonly Uri HypermediaSpecificationUri = new Uri("https://w3id.org/tree#");

        [DataMember(Name = "skos")]
        public readonly Uri CodelistsUri = new Uri("http://www.w3.org/2004/02/skos/core#");

        [DataMember(Name = "xsd")]
        public readonly Uri XmlSchemaUri = new Uri("http://www.w3.org/2001/XMLSchema#");

        [DataMember(Name = "prov")]
        public readonly Uri ProvenanceUri = new Uri("http://www.w3.org/ns/prov#");

        [DataMember(Name = "dct")]
        public readonly Uri MetadataTermsUri = new Uri("http://purl.org/dc/terms/");

        [DataMember(Name = "adms")]
        public readonly Uri AssetDescription = new Uri("http://www.w3.org/ns/adms#");

        [DataMember(Name = "geo")]
        public readonly Uri GeoSparql = new Uri("http://www.opengis.net/ont/geosparql#");

        [DataMember(Name = "gebouw")]
        public readonly Uri OsloAddressUri = new Uri("https://data.vlaanderen.be/ns/gebouw#");

        [DataMember(Name = "br")]
        public readonly Uri OsloBuildingRegistryUri = new Uri("https://basisregisters.vlaanderen.be/ns/gebouw#");

        [DataMember(Name = "items")]
        public readonly string ItemsDefinitionUri = "tree:member";

        [DataMember(Name = "viewOf")]
        public readonly TreeCollectionContext ViewOf = new TreeCollectionContext();

        [DataMember(Name = "generatedAtTime")]
        public readonly ProvenanceContext Provenance = new ProvenanceContext();

        [DataMember(Name = "eventName")]
        public readonly string EventNameUri = "adms:versionNotes";

        [DataMember(Name = "isVersionOf")]
        public readonly ParentInformationContext IsVersionOf = new ParentInformationContext();

        [DataMember(Name = "tree:node")]
        public readonly PropertyOverride TreePath = new PropertyOverride
        {
            Type = "@id"
        };

        [DataMember(Name = "tree:shape")]
        public readonly PropertyOverride TreeShape = new PropertyOverride
        {
            Type = "@id"
        };

        [DataMember(Name = "Gebouweenheid")]
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public readonly string BuildingUnitType = "gebouw:Gebouweenheid";

        [DataMember(Name = "adres")]
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public readonly string BuildingUnitAddresses = "gebouw:Gebouweenheid.adres";

        [DataMember(Name = "functie")]
        public readonly string BuildingType = "gebouw:functie";

        [DataMember(Name = "status")]
        public readonly string BuildingUnitStatus = "gebouw:Gebouweenheid.status";

        [DataMember(Name = "behoordTot")]
        public readonly PropertyOverride AssociatedBuilding = new PropertyOverride
        {
            Id = "br:Gebouweenheid.behoordTot",
            Type = "@id"
        };

        [DataMember(Name = "geometrie")]
        public readonly PropertyOverride BuildingGeometry = new PropertyOverride
        {
            Id = "gebouw:Gebouweenheid.geometrie",
            Type = "gebouw:2DGebouwgeometrie"
        };

        [DataMember(Name = "gml")]
        public readonly PropertyOverride BuildingGml = new PropertyOverride
        {
            Id = "geo:asGML",
            Type = "geo:gmlLiteral"
        };

        [DataMember(Name = "methode")]
        public readonly string GeometryMethod = "gebouw:methode";
    }

    public class TreeCollectionContext
    {
        [JsonProperty("@reverse")]
        public readonly string ReverseRelation = "tree:view";

        [JsonProperty("@type")]
        public readonly string Type = "@id";
    }

    public class ProvenanceContext
    {
        [JsonProperty("@id")]
        public readonly Uri Id = new Uri("prov:generatedAtTime");

        [JsonProperty("@type")]
        public readonly string Type = "xsd:dateTime";
    }

    public class ParentInformationContext
    {
        [JsonProperty("@id")]
        public readonly Uri Id = new Uri("dct:isVersionOf");

        [JsonProperty("@type")]
        public readonly string Type = "@id";
    }

    public class PropertyOverride
    {
        [JsonProperty("@id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
    }

    public class BuildingUnitShaclContext
    {
        [JsonProperty("sh")]
        public readonly Uri ShaclUri = new Uri("https://www.w3.org/ns/shacl#");

        [JsonProperty("rdf")]
        public readonly Uri RdfUri = new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#");

        [JsonProperty("xsd")]
        public readonly Uri XmlSchemaUri = new Uri("http://www.w3.org/2001/XMLSchema#");

        [JsonProperty("skos")]
        public readonly Uri CodelistUri = new Uri("http://www.w3.org/2004/02/skos/core#");

        [JsonProperty("prov")]
        public readonly Uri ProvenanceUri = new Uri("http://www.w3.org/ns/prov#");

        [JsonProperty("dct")]
        public readonly Uri MetadataTermsUri = new Uri("http://purl.org/dc/terms/");

        [JsonProperty("adms")]
        public readonly Uri AssetDescription = new Uri("http://www.w3.org/ns/adms#");

        [JsonProperty("gebouw")]
        public readonly Uri OsloBuildingUri = new Uri("https://data.vlaanderen.be/ns/gebouw#");

        [JsonProperty("br")]
        public readonly Uri OsloBuildingRegistryUri = new Uri("https://basisregisters.vlaanderen.be/ns/gebouw#");

        [JsonProperty("geo")]
        public readonly Uri GeoSparql = new Uri("http://www.opengis.net/ont/geosparql#");

        [JsonProperty("sh:datatype")]
        public readonly PropertyOverride DataTypeExtended = new PropertyOverride
        {
            Type = "@id"
        };

        [JsonProperty("sh:nodeKind")]
        public readonly PropertyOverride NodeKindExtended = new PropertyOverride
        {
            Type = "@id"
        };

        [JsonProperty("sh:path")]
        public readonly PropertyOverride PathExtended = new PropertyOverride
        {
            Type = "@id"
        };
    }
}
