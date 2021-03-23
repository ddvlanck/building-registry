namespace BuildingRegistry.Api.Legacy.BuildingUnit.Responses
{
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using BuildingRegistry.Api.Legacy.Infrastructure;
    using BuildingRegistry.Projections.Legacy;
    using BuildingRegistry.Projections.Syndication;
    using BuildingRegistry.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using NodaTime;
    using Swashbuckle.AspNetCore.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract(Name = "BuildingUnitLinkedDataEventStream", Namespace = "")]
    public class BuildingUnitLinkedDataEventStreamResponse
    {
        [DataMember(Name = "@context")]
        public readonly BuildingUnitLinkedDataEventStreamContext Context = new BuildingUnitLinkedDataEventStreamContext();

        [DataMember(Name = "@id")]
        public Uri Id { get; set; }

        [DataMember(Name = "@type")]
        public readonly string Type = "tree:Node";

        [DataMember(Name = "viewOf")]
        public Uri CollectionLink { get; set; }

        [DataMember(Name = "tree:shape")]
        public Uri BuildingShape { get; set; }

        [DataMember(Name = "tree:relation")]
        [JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<HypermediaControl>? HypermediaControls { get; set; }

        [DataMember(Name = "items")]
        public List<BuildingUnitVersionObject> BuildingUnits { get; set; }
    }

    [DataContract(Name = "GebouweenheidVersieObject", Namespace = "")]
    public class BuildingUnitVersionObject
    {
        [DataMember(Name = "@id", Order = 1)]
        public Uri Id { get; set; }

        [DataMember(Name = "@type", Order = 2)]
        public readonly string Type = "Gebouweenheid";

        [DataMember(Name = "isVersionOf", Order = 3)]
        public Uri IsVersionOf { get; set; }

        [DataMember(Name = "generatedAtTime", Order = 4)]
        public DateTimeOffset EventGeneratedAtTime { get; set; }

        [DataMember(Name = "eventName", Order = 5)]
        public string ChangeType { get; set; }

        [DataMember(Name = "adres", Order = 6)]
        public List<Uri> BuildingUnitAddresses { get; set; }

        [DataMember(Name = "functie", Order = 7)]
        public Uri Function { get; set; }

        [DataMember(Name = "status", Order = 8)]
        public Uri Status { get; set; }

        [DataMember(Name = "geometrie", Order = 9)]
        public BuildingUnitGeometry Geometry { get; set; }

        [DataMember(Name = "behoordTot", Order = 9)]
        public Uri AssociatedBuilding { get; set; }

        [IgnoreDataMember]
        public LinkedDataEventStreamConfiguration _configuration { get; set; }

        public BuildingUnitVersionObject(
            LinkedDataEventStreamConfiguration configuration,
            string objectIdentifier,
            int persistentLocalId,
            string changeType,
            Instant generatedAtTime,
            byte[] pointPosition,
            BuildingUnitPositionGeometryMethod geometryMethod,
            BuildingUnitStatus status,
            BuildingUnitFunction function,
            int buildingPersistentId,
            IEnumerable<string?> addressesPersistentIds)
        {
            _configuration = configuration;

            ChangeType = changeType;
            EventGeneratedAtTime = generatedAtTime.ToBelgianDateTimeOffset();

            Id = CreateVersionUri(objectIdentifier);
            IsVersionOf = GetPersistentUri(persistentLocalId);
            Geometry = GetBuildingUnitGeometry(pointPosition, geometryMethod);
            Status = GetBuildingUnitStatus(status);
            Function = GetBuildingUnitFunction(function);
            AssociatedBuilding = GetBuildingPersistentUri(buildingPersistentId);
            BuildingUnitAddresses = GetBuildingUnitAddressesPersistentUris(addressesPersistentIds);
        }

        private Uri CreateVersionUri(string identifier)
            => new Uri($"{_configuration.ApiEndpoint}#{identifier}");

        private Uri GetPersistentUri(int persistentLocalId)
            => new Uri($"{_configuration.DataVlaanderenBuildingUnitNamespace}/{persistentLocalId}");

        private BuildingUnitGeometry GetBuildingUnitGeometry(byte[] pointPosition, BuildingUnitPositionGeometryMethod geometryMethod)
            => new BuildingUnitGeometry(pointPosition, geometryMethod);

        private Uri GetBuildingUnitStatus(BuildingUnitStatus status)
        {
            if (BuildingUnitStatus.Planned == status)
                return new Uri($"https://data.vlaanderen.be/id/concept/gebouweenheidstatus/gepland");

            if (BuildingUnitStatus.NotRealized == status)
                return new Uri($"https://data.vlaanderen.be/id/concept/gebouweenheidstatus/nietGerealiseerd");

            if (BuildingUnitStatus.Realized == status)
                return new Uri($"https://data.vlaanderen.be/id/concept/gebouweenheidstatus/gerealiseerd");

            if (BuildingUnitStatus.Retired == status)
                return new Uri($"https://data.vlaanderen.be/id/concept/gebouweenheidstatus/gehistoreerd");

            throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }

        private static Uri GetBuildingUnitFunction(BuildingUnitFunction function)
        {
            if (BuildingUnitFunction.Common == function)
                return new Uri("https://data.vlaanderen.be/id/concept/gebouweenheidfunctie/gemeenschappelijkDeel");

            if (BuildingUnitFunction.Unknown == function)
                return new Uri("https://data.vlaanderen.be/id/concept/gebouweenheidfunctie/nietGekend");

            throw new ArgumentOutOfRangeException(nameof(function), function, null);
        }

        private Uri GetBuildingPersistentUri(int buildingPersistentId)
            => new Uri($"{_configuration.DataVlaanderenNamespace}/{buildingPersistentId}");

        private List<Uri> GetBuildingUnitAddressesPersistentUris(IEnumerable<string?> addressesPersistentIds)
            => addressesPersistentIds
            .Select(addressPersistentId => new Uri($"{_configuration.DataVlaanderenAddressNamespace}/{addressPersistentId}"))
            .ToList();
    }

    public class BuildingUnitGeometry
    {
        [JsonProperty("gml")]
        public string GmlString { get; set; }

        [JsonProperty("methode")]
        public Uri BuilintUnitGeometryMethodUri { get; set; }

        public BuildingUnitGeometry(byte[] pointPosition, BuildingUnitPositionGeometryMethod geometryMethod)
        {
            GmlString = GetPointAsGmlString(pointPosition);
            BuilintUnitGeometryMethodUri = GetGeometryMethodUri(geometryMethod);
        }

        private string GetPointAsGmlString(byte[] pointPosition)
        {
            var xmlPoint = BuildingUnitController.GetBuildingUnitPoint(pointPosition).XmlPoint;

            return $"<gml:Point><gml:pos>{xmlPoint.Pos}</gml:pos></gml:Point>";
        }

        private static Uri GetGeometryMethodUri(BuildingUnitPositionGeometryMethod method)
        {
            if (BuildingUnitPositionGeometryMethod.AppointedByAdministrator == method)
                return new Uri($"https://data.vlaanderen.be/id/concept/geometriemethode/aangeduidDoorBeheerder");

            if (BuildingUnitPositionGeometryMethod.DerivedFromObject == method)
                return new Uri($"https://data.vlaanderen.be/id/concept/geometriemethode/afgeleidVanObject");

            throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }

    public class BuildingUnitLinkedDataEventStreamResponseExamples : IExamplesProvider<BuildingUnitLinkedDataEventStreamResponse>
    {
        private readonly LinkedDataEventStreamConfiguration _configuration;

        public BuildingUnitLinkedDataEventStreamResponseExamples(LinkedDataEventStreamConfiguration configuration)
            => _configuration = configuration;

        public BuildingUnitLinkedDataEventStreamResponse GetExamples()
        {
            //TODO : array of bytes makes this difficult
            return null;
        }
    }
}
