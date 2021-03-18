namespace BuildingRegistry.Api.Legacy.Building.Responses
{
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using Be.Vlaanderen.Basisregisters.GrAr.Legacy.SpatialTools;
    using BuildingRegistry.Api.Legacy.Infrastructure;
    using BuildingRegistry.ValueObjects;
    using Newtonsoft.Json;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract(Name = "BuildingLinkedDataEventStream", Namespace = "")]
    public class BuildingLinkedDataEventStreamResponse
    {
        /*[DataMember(Name = "@context")]
        public readonly object Context = */

        [DataMember(Name = "@id")]
        public Uri Id { get; set; }

        [DataMember(Name = "@type")]
        public readonly string Type = "tree:Node";

        [DataMember(Name = "viewOf")]
        public Uri CollectionLink { get; set; }

        /*[DataMember(Name = "tree:shape")]
        public Uri BuildingShape { get; set; }*/

        /*[DataMember(Name = "tree:relation")]
        [JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<HypermediaControl>? HypermediaControls { get; set; }*/

        [DataMember(Name = "items")]
        public List<BuildingVersionObject> Buildings { get; set; }
    }

    [DataContract(Name= "GebouwVersieObject", Namespace = "")]
    public class BuildingVersionObject
    {
        [DataMember(Name = "@id", Order = 1)]
        public Uri Id { get; set; }

        [DataMember(Name = "@type", Order = 2)]
        public readonly string Type = "Gebouw";

        [DataMember(Name = "isVersionOf", Order = 3)]
        public Uri IsVersionOf { get; set; }

        [DataMember(Name = "generatedAtTime", Order = 4)]
        public DateTimeOffset GeneratedAtTime { get; set; }

        [DataMember(Name = "eventName", Order = 5)]
        public string ChangeType { get; set; }

        [DataMember(Name = "geometrie", Order = 6)]
        public BuildingGeometry Geometry { get; set; }

        [DataMember(Name = "status", Order = 7)]
        public Uri BuildingStatusUri { get; set; }

        [DataMember(Name = "bestaatUit", Order = 8)]
        public List<Uri> BuildingUnits { get; set; }

        [DataMember(Name = "ligtOp", Order = 9)]
        public List<string>? OccupiedParcels { get; set; }  //TODO

        [IgnoreDataMember]
        public LinkedDataEventStreamConfiguration _configuration { get; set; }

        public BuildingVersionObject(
            LinkedDataEventStreamConfiguration configuration,
            string objectIdentifier,
            int persistentLocalId,
            string changeType,
            Instant generatedAtTime,
            byte[] geometry,
            BuildingGeometryMethod geometryMethod,
            BuildingStatus status,
            List<int> buildingUnitsIds)
        {
            _configuration = configuration;

            ChangeType = changeType;
            GeneratedAtTime = generatedAtTime.ToBelgianDateTimeOffset();

            Id = CreateVersionUri(objectIdentifier);
            IsVersionOf = GetPersistentUri(persistentLocalId);

            Geometry = GetBuildingGeometry(geometry, geometryMethod);
            BuildingStatusUri = GetBuildingStatus(status);

            BuildingUnits = GetBuildingUnitsPersistentUris(buildingUnitsIds);
            //TODO: parcels
        }

        private Uri CreateVersionUri(string identifier)
            => new Uri($"{_configuration.ApiEndpoint}#{identifier}");

        private Uri GetPersistentUri(int persistentLocalId)
            => new Uri($"{_configuration.DataVlaanderenNamespace}/{persistentLocalId}");

        private BuildingGeometry GetBuildingGeometry(byte[] geometry, BuildingGeometryMethod geometryMethod)
            => new BuildingGeometry(geometry, geometryMethod);

        private Uri GetBuildingStatus(BuildingStatus status)
        {
            switch (status)
            {
                case BuildingStatus.Planned:
                    return new Uri("https://data.vlaanderen.be/id/concept/gebouwstatus/gepland");

                case BuildingStatus.NotRealized:
                    return new Uri("https://data.vlaanderen.be/id/concept/gebouwstatus/nietGerealiseerd");

                case BuildingStatus.Realized:
                    return new Uri("https://data.vlaanderen.be/id/concept/gebouwstatus/gerealiseerd");

                case BuildingStatus.Retired:
                    return new Uri("https://data.vlaanderen.be/id/concept/gebouwstatus/gehistoreerd");

                case BuildingStatus.UnderConstruction:
                    return new Uri("https://data.vlaanderen.be/id/concept/gebouwstatus/inAanbouw");

                default:
                    throw new Exception("Building should have a status");
            }
        }

        private List<Uri> GetBuildingUnitsPersistentUris(List<int> buildingUnitsPersistentLocalIds)
        {
            List<Uri> buildingUnitPersistentUris = new List<Uri>();

            foreach (var id in buildingUnitsPersistentLocalIds)
                buildingUnitPersistentUris.Add(new Uri($"{_configuration.DataVlaanderenBuildingUnitNamespace}/{id}"));

            return buildingUnitPersistentUris;
        }
    }

    public class BuildingGeometry
    {
        public GmlPolygon GmlString { get; set; }

        public Uri GeometryMethodUri {get; set;}

        public BuildingGeometry(byte[] geometry, BuildingGeometryMethod geometryMethod)
        {
            GmlString = GetGmlPolygon(geometry);
            GeometryMethodUri = GetGeometryMethodUri(geometryMethod);
        }

        private GmlPolygon GetGmlPolygon(byte[] polygon)
        {
            var geometry = WKBReaderFactory.Create().Read(polygon) as NetTopologySuite.Geometries.Polygon;

            if (geometry == null) //some buildings have multi polygons (imported) which are incorrect.
                return null;

            return BuildingController.MapGmlPolygon(geometry);
        }

        private Uri GetGeometryMethodUri(BuildingGeometryMethod method)
        {
            switch (method)
            {
                case BuildingGeometryMethod.MeasuredByGrb:
                    return new Uri("https://data.vlaanderen.be/id/concept/2Dgeometriemethode/ingemetenGRB");

                case BuildingGeometryMethod.Outlined:
                    return new Uri("https://data.vlaanderen.be/id/concept/2Dgeometriemethode/ingemeten");

                default:
                    throw new Exception("Building should have a geometry method");
            }
        }
    }
}
