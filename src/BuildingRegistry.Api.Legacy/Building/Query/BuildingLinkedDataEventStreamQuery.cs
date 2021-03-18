namespace BuildingRegistry.Api.Legacy.Building.Query
{
    using Be.Vlaanderen.Basisregisters.Api.Search;
    using Be.Vlaanderen.Basisregisters.Api.Search.Filtering;
    using Be.Vlaanderen.Basisregisters.Api.Search.Sorting;
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

    public class BuildingLinkedDataEventStreamQueryResult
    {
        public int PersistentLocalId { get; }
        public string ChangeType { get; }

        public byte[] Geometry { get; }
        public BuildingGeometryMethod GeometryMethod { get; }

        public BuildingStatus Status { get; }

        public Instant EventGeneratedAtTime { get; }

        public virtual List<int> BuildingUnitsIds { get; }

        public string ObjectIdentifier { get; }

        public BuildingLinkedDataEventStreamQueryResult(
            string objectIdentifier,
            int persistentLocalId,
            string changeType,
            Instant eventGeneratedAtTime,
            byte[] geometry,
            BuildingGeometryMethod geometryMethod,
            BuildingStatus status,
            List<int> buildingUnitsIds)
        {
            ObjectIdentifier = objectIdentifier;
            PersistentLocalId = persistentLocalId;
            ChangeType = changeType;
            EventGeneratedAtTime = eventGeneratedAtTime;

            Geometry = geometry;
            GeometryMethod = geometryMethod;
            Status = status;
            BuildingUnitsIds = buildingUnitsIds;
        }
    }

    public class BuildingLinkedDataEventStreamQuery : Query<BuildingLinkedDataEventStreamItem, BuildingLinkedDataEventStreamFilter, BuildingLinkedDataEventStreamQueryResult>
    {
        private readonly LegacyContext _context;

        public BuildingLinkedDataEventStreamQuery(LegacyContext context)
            => _context = context;

        protected override ISorting Sorting => new BuildingLinkedDataEventStreamSorting();

        protected override Expression<Func<BuildingLinkedDataEventStreamItem, BuildingLinkedDataEventStreamQueryResult>> Transformation
        {
            get => linkedDataEventStreamItem => new BuildingLinkedDataEventStreamQueryResult(
                linkedDataEventStreamItem.ObjectHash,
                (int)linkedDataEventStreamItem.PersistentLocalId,
                linkedDataEventStreamItem.ChangeType,
                linkedDataEventStreamItem.EventGeneratedAtTime,
                linkedDataEventStreamItem.Geometry,
                (BuildingGeometryMethod)linkedDataEventStreamItem.GeometryMethod,
                (BuildingStatus)linkedDataEventStreamItem.Status,
                linkedDataEventStreamItem.BuildingUnits.Select(x => (int) x.PersistentLocalId).ToList());
        }

        protected override IQueryable<BuildingLinkedDataEventStreamItem> Filter(FilteringHeader<BuildingLinkedDataEventStreamFilter> filtering)
        {
            var buildings = _context
                .BuildingLinkedDataEventStream
                .Include(x => x.BuildingUnits)
                .OrderBy(x => x.Position)
                .AsSplitQuery()
                .AsNoTracking();

            buildings = buildings.Where(x => x.RecordCanBePublished == true);

            return buildings;
        }
    }

    internal class BuildingLinkedDataEventStreamSorting : ISorting
    {
        public IEnumerable<string> SortableFields { get; } = new[]
        {
            nameof(BuildingLinkedDataEventStreamItem.Position)
        };

        public SortingHeader DefaultSortingHeader { get; } = new SortingHeader(nameof(BuildingLinkedDataEventStreamItem.Position), SortOrder.Ascending);
    }

    public class BuildingLinkedDataEventStreamFilter
    {
        public int PageNumber { get; set; }
    }
}
