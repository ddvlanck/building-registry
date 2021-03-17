namespace BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Connector;
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.SqlStreamStore;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    public static class BuildingLinkedDataEventStreamExtensions
    {
        public static async Task CreateNewBuildingLinkedDataEventStreamItem<T>(
            this LegacyContext context,
            Guid buildingId,
            Envelope<T> message,
            Action<BuildingLinkedDataEventStreamItem> applyEventInfoOn,
            bool recordIsAllowedInBuildingFeed,
            CancellationToken ct)
        {
            var dummyApplyEventInfoOn = new Action<BuildingLinkedDataEventStreamItem, BuildingLinkedDataEventStreamItem>(
                (oldSyndicationItem, newSyndicationItem) => applyEventInfoOn(newSyndicationItem));

            await context.CreateNewBuildingLinkedDataEventStreamItem(
                buildingId,
                message,
                dummyApplyEventInfoOn,
                recordIsAllowedInBuildingFeed,
                ct);
        }

        public static async Task CreateNewBuildingLinkedDataEventStreamItem<T>(
            this LegacyContext context,
            Guid buildingId,
            Envelope<T> message,
            Action<BuildingLinkedDataEventStreamItem, BuildingLinkedDataEventStreamItem> applyEventInfoOn,
            bool recordIsAllowedInBuildingFeed,
            CancellationToken ct)
        {
            context.Database.SetCommandTimeout(300);

            var buildingLinkedDataEventStreamItem = await context.LatestPosition(buildingId, ct);

            if (buildingLinkedDataEventStreamItem == null)
                throw DatabaseItemNotFound(buildingId);

            if (buildingLinkedDataEventStreamItem.Position >= message.Position)
                return;

            var dummyApplyEventInfoOn = new Action<BuildingLinkedDataEventStreamItem>(
                newLinkedDataEventStreamItem => applyEventInfoOn(buildingLinkedDataEventStreamItem, newLinkedDataEventStreamItem));

            var newBuildingLinkedDataEventStreamItem = buildingLinkedDataEventStreamItem.CloneAndApplyEventInfo(
                message.Position,
                message.EventName,
                message.Message is IHasProvenance x ? x.Provenance.Timestamp : Instant.FromDateTimeOffset(DateTimeOffset.MinValue),
                dummyApplyEventInfoOn);

            newBuildingLinkedDataEventStreamItem.SetObjectHash();

            // In the Linked Data Event Stream about buildings we only want a specific group of events that talk about building units
            // When the event is allowed, it still needs to comply with the data model in order to be published
            if (recordIsAllowedInBuildingFeed)
                newBuildingLinkedDataEventStreamItem.CheckIfRecordCanBePublished();
            else
                newBuildingLinkedDataEventStreamItem.RecordCanBePublished = false;

            await context
                .BuildingLinkedDataEventStream
                .AddAsync(newBuildingLinkedDataEventStreamItem, ct);
        }

        public static async Task<BuildingLinkedDataEventStreamItem> LatestPosition(
            this LegacyContext context,
            Guid buildingId,
            CancellationToken ct)
            => context
                   .BuildingLinkedDataEventStream
                   .Local
                   .Where(x => x.BuildingId == buildingId)
                   .OrderByDescending(x => x.Position)
                   .FirstOrDefault()
               ?? await context
                   .BuildingLinkedDataEventStream
                   .AsNoTracking()
                   .Where(x => x.BuildingId == buildingId)
                   .Include(x => x.BuildingUnits).ThenInclude(y => y.Addresses)
                   .Include(x => x.BuildingUnits).ThenInclude(y => y.Readdresses)
                   .OrderByDescending(x => x.Position)
                   .FirstOrDefaultAsync(ct);

        public static async Task UpdateBuildingPersistentLocalIdentifier(
            this LegacyContext context,
            Guid buildingId,
            int persistentLocalId,
            CancellationToken ct)
        {
            var buildingItems = context
                    .BuildingLinkedDataEventStream
                    .Local
                    .Where(x => x.BuildingId == buildingId).ToList()
                ?? await context
                    .BuildingLinkedDataEventStream
                    .Where(x => x.BuildingId == buildingId)
                    .ToListAsync(ct);

            foreach (var item in buildingItems)
                item.PersistentLocalId = persistentLocalId;

            await context.SaveChangesAsync();
        }

        public static async Task BuildingWasRemoved(
            this LegacyContext context,
            Guid buildingId,
            CancellationToken ct)
        {
            var buildingItems = context
                    .BuildingLinkedDataEventStream
                    .Local
                    .Where(x => x.BuildingId == buildingId).ToList()
                ?? await context
                    .BuildingLinkedDataEventStream
                    .Where(x => x.BuildingId == buildingId)
                    .ToListAsync(ct);

            foreach (var item in buildingItems)
                item.RecordCanBePublished = false;

            await context.SaveChangesAsync();
        }

        public static void CheckIfRecordCanBePublished(this BuildingLinkedDataEventStreamItem buildingLinkedDataEventStreamItem)
        {
            var recordCanBePublished = true;

            if (buildingLinkedDataEventStreamItem.Status == null || string.IsNullOrEmpty(buildingLinkedDataEventStreamItem.Status.ToString()))
                recordCanBePublished = false;

            if (buildingLinkedDataEventStreamItem.Geometry == null | buildingLinkedDataEventStreamItem.GeometryMethod == null)
                recordCanBePublished = false;

            buildingLinkedDataEventStreamItem.RecordCanBePublished = recordCanBePublished;
        }

        public static void SetObjectHash(this BuildingLinkedDataEventStreamItem buildingLinkedDataEventStreamItem)
        {
            var objectString = JsonConvert.SerializeObject(buildingLinkedDataEventStreamItem);

            using var md5Hash = MD5.Create();
            var hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(objectString));
            buildingLinkedDataEventStreamItem.ObjectHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        private static ProjectionItemNotFoundException<BuildingLinkedDataEventStreamItem> DatabaseItemNotFound(Guid buildingId)
            => new ProjectionItemNotFoundException<BuildingLinkedDataEventStreamItem>(buildingId.ToString("D"));
    }
}
