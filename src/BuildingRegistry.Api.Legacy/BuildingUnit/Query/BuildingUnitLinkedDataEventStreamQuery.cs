namespace BuildingRegistry.Api.Legacy.BuildingUnit.Query
{
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
    using Be.Vlaanderen.Basisregisters.GrAr.Common;
    using BuildingRegistry.Projections.Legacy;
    using BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream;
    using BuildingRegistry.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class BuildingUnitLinkedDataEventStreamQueryResult
    {
        public int PersistentLocalId { get; }
        public Guid BuildingId { get; }
        public string ChangeType { get; }

        public byte[] Geometry { get; }
        public BuildingUnitPositionGeometryMethod GeometryMethod { get; }

        public BuildingUnitStatus Status { get; }

        public BuildingUnitFunction Function { get; }

        public Instant EventGeneratedAtTime { get; }
        public string ObjectIdentifier { get; }

        public IEnumerable<Guid> AddressIds { get; set; }

        public BuildingUnitLinkedDataEventStreamQueryResult(
            string objectIdentifier,
            int persistentLocalId,
            Guid buildingId,
            string changeType,
            Instant eventGeneratedAtTime,
            byte[] geometry,
            BuildingUnitPositionGeometryMethod geometryMethod,
            BuildingUnitStatus status,
            BuildingUnitFunction function,
            Collection<BuildingUnitAddressLinkedDataEventStreamItem> addresses,
            Collection<BuildingUnitReaddressLinkedDataEventStreamItem> readdresses)
        {
            ObjectIdentifier = objectIdentifier;
            PersistentLocalId = persistentLocalId;
            BuildingId = buildingId;
            ChangeType = changeType;
            EventGeneratedAtTime = eventGeneratedAtTime;
            Geometry = geometry;
            GeometryMethod = geometryMethod;
            Status = status;
            Function = function;

            var datetimeLastChangedOn = eventGeneratedAtTime.ToBelgianDateTimeOffset();
            var relevantReaddresses = readdresses.Where(x => x.ReaddressBeginDate >= LocalDate.FromDateTime(datetimeLastChangedOn.DateTime)).ToList();

            AddressIds = addresses
                .Where(x => x.AddressId.HasValue)
                .Select(address => relevantReaddresses.Any(x => x.OldAddressId == address.AddressId)
                    ? relevantReaddresses.First(x => x.OldAddressId == address.AddressId).NewAddressId
                    : address.AddressId.Value)
                .Distinct()
                .ToList();
        }
    }

    public class BuildingUnitLinkedDataEventStreamQuery : Query<BuildingUnitLinkedDataEventStreamItem, BuildingUnitLinkedDataEventStreamFilter, BuildingUnitLinkedDataEventStreamQueryResult>
    {
        private readonly LegacyContext _context;

        public BuildingUnitLinkedDataEventStreamQuery(LegacyContext context)
            => _context = context;

        protected override ISorting Sorting => new BuildingUnitLinkedDataEventStreamSorting();

        protected override Expression<Func<BuildingUnitLinkedDataEventStreamItem, BuildingUnitLinkedDataEventStreamQueryResult>> Transformation
        {
            get => linkedDataEventStreamItem => new BuildingUnitLinkedDataEventStreamQueryResult(
                linkedDataEventStreamItem.ObjectHash,
                (int)linkedDataEventStreamItem.PersistentLocalId,
                linkedDataEventStreamItem.BuildingId,
                linkedDataEventStreamItem.ChangeType,
                linkedDataEventStreamItem.EventGeneratedAtTime,
                linkedDataEventStreamItem.PointPosition,
                (BuildingUnitPositionGeometryMethod)linkedDataEventStreamItem.PositionMethod,
                (BuildingUnitStatus)linkedDataEventStreamItem.Status,
                (BuildingUnitFunction)linkedDataEventStreamItem.Function,
                linkedDataEventStreamItem.Addresses,
                linkedDataEventStreamItem.Readdresses);
        }

        protected override IQueryable<BuildingUnitLinkedDataEventStreamItem> Filter(FilteringHeader<BuildingUnitLinkedDataEventStreamFilter> filtering)
        {
            var buildingUnits = _context
                .BuildingUnitLinkedDataEventStream
                .Include(x => x.Addresses)
                .Include(x => x.Readdresses)
                .OrderBy(x => x.Position)
                .AsSplitQuery()
                .AsNoTracking();

            buildingUnits = buildingUnits.Where(x => x.RecordCanBePublished == true);

            return buildingUnits;
        }
    }

    internal class BuildingUnitLinkedDataEventStreamSorting : ISorting
    {
        public IEnumerable<string> SortableFields { get; } = new[]
        {
            nameof(BuildingUnitLinkedDataEventStreamItem.Position)
        };

        public SortingHeader DefaultSortingHeader { get; } = new SortingHeader(nameof(BuildingUnitLinkedDataEventStreamItem.Position), SortOrder.Ascending);
    }

    public class BuildingUnitLinkedDataEventStreamFilter
    {
        public int PageNumber { get; set; }
    }
}
