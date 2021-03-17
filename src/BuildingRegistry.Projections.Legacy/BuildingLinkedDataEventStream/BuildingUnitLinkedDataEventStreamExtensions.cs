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

    public static class BuildingUnitLinkedDataEventStreamExtensions
    {
        public static async Task UpdateBuildingUnitPersistentLocalIdentifier(
            this LegacyContext context,
            Guid buildingUnitId,
            int persistentLocalId,
            CancellationToken ct)
        {
            var buildingUnitItems = context
                    .BuildingUnitLinkedDataEventStream
                    .Local
                    .Where(x => x.BuildingUnitId == buildingUnitId).ToList()
                ?? await context
                    .BuildingUnitLinkedDataEventStream
                    .Where(x => x.BuildingUnitId == buildingUnitId)
                    .ToListAsync(ct);

            foreach (var item in buildingUnitItems)
                item.PersistentLocalId = persistentLocalId;

            await context.SaveChangesAsync();
        }

        public static void CheckIfRecordCanBePublished(this BuildingUnitLinkedDataEventStreamItem buildingUnitLinkedDataEventStreamItem)
        {
            var recordCanBePublished = true;

            if (buildingUnitLinkedDataEventStreamItem.Status == null)
                recordCanBePublished = false;

            if (buildingUnitLinkedDataEventStreamItem.PointPosition == null | buildingUnitLinkedDataEventStreamItem.PositionMethod == null)
                recordCanBePublished = false;

            if (buildingUnitLinkedDataEventStreamItem.Addresses.Count == 0 && buildingUnitLinkedDataEventStreamItem.Readdresses.Count == 0)
                recordCanBePublished = false;

            buildingUnitLinkedDataEventStreamItem.RecordCanBePublished = recordCanBePublished;
        }

        public static void SetObjectHash(this BuildingUnitLinkedDataEventStreamItem buildingUnitLinkedDataEventStreamItem)
        {
            var objectString = JsonConvert.SerializeObject(buildingUnitLinkedDataEventStreamItem);

            using var md5Hash = MD5.Create();
            var hashBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(objectString));
            buildingUnitLinkedDataEventStreamItem.ObjectHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        private static ProjectionItemNotFoundException<BuildingUnitLinkedDataEventStreamItem> DatabaseItemNotFound(Guid buildingUnitId)
            => new ProjectionItemNotFoundException<BuildingUnitLinkedDataEventStreamItem>(buildingUnitId.ToString("D"));
    }
}
