namespace BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.MigrationExtensions;
    using BuildingRegistry.Infrastructure;
    using BuildingRegistry.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Newtonsoft.Json;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BuildingLinkedDataEventStreamItem
    {
        public long Position { get; set; }

        public Guid BuildingId { get; set; }
        public int? PersistentLocalId { get; set; }
        public string? ChangeType { get; set; }

        public byte[]? Geometry { get; set; }
        public BuildingGeometryMethod? GeometryMethod { get; set; }

        public BuildingStatus? Status { get; set; }

        public DateTimeOffset EventGeneratedAtTimeAsDateTimeOffset { get; set; }

        public Instant EventGeneratedAtTime
        {
            get => Instant.FromDateTimeOffset(EventGeneratedAtTimeAsDateTimeOffset);
            set => EventGeneratedAtTimeAsDateTimeOffset = value.ToDateTimeOffset();
        }

        //public List<Guid> BuildingUnits { get; set; }

        //public string? BuildingUnitsAsJson { get; set; }

        public virtual Collection<BuildingUnitLinkedDataEventStreamItem> BuildingUnits { get; set; }

        public string ObjectHash { get; set; }

        public bool RecordCanBePublished { get; set; }

        public BuildingLinkedDataEventStreamItem()
        {
            BuildingUnits = new Collection<BuildingUnitLinkedDataEventStreamItem>();
        }

        public BuildingLinkedDataEventStreamItem CloneAndApplyEventInfo(
            long position,
            string changeType,
            Instant lastChangedOn,
            Action<BuildingLinkedDataEventStreamItem> editFunc)
        {
            var buildingUnits = BuildingUnits.Select(x => x.CloneAndApplyEventInfo(position, BuildingId));

            var newItem = new BuildingLinkedDataEventStreamItem
            {
                ChangeType = changeType,
                Position = position,
                EventGeneratedAtTime = lastChangedOn,

                BuildingId = BuildingId,
                PersistentLocalId = PersistentLocalId,
                GeometryMethod = GeometryMethod,
                Geometry = Geometry,
                Status = Status,
                //BuildingUnitsAsJson = BuildingUnitsAsJson,
                BuildingUnits = new Collection<BuildingUnitLinkedDataEventStreamItem>(buildingUnits.ToList()),
                RecordCanBePublished = RecordCanBePublished
            };

            editFunc(newItem);

            return newItem;
        }

        /*public void AddBuildingUnit(Guid buildingUnitId)
        {
            var newList = GetBuildingUnitsAsList();
            newList.Add(buildingUnitId);
            BuildingUnitsAsJson = JsonConvert.SerializeObject(newList);
        }

        public void RemoveBuildingUnit(Guid buildingUnitId)
        {
            var newList = GetBuildingUnitsAsList();
            newList.Remove(buildingUnitId);
            BuildingUnitsAsJson = JsonConvert.SerializeObject(newList);
        }

        private List<Guid> GetBuildingUnitsAsList()
            => string.IsNullOrEmpty(BuildingUnitsAsJson)
                ? new List<Guid>()
                : JsonConvert.DeserializeObject<List<Guid>>(BuildingUnitsAsJson);*/
    }

    public class BuildingLinkedDataEventStreamConfiguration : IEntityTypeConfiguration<BuildingLinkedDataEventStreamItem>
    {
        private const string TableName = "BuildingLinkedDataEventStream";

        public void Configure(EntityTypeBuilder<BuildingLinkedDataEventStreamItem> builder)
        {
            builder.ToTable(TableName, Schema.Legacy)
                .HasKey(x => x.Position)
                .IsClustered();

            builder.Property(x => x.Position).ValueGeneratedNever();
            builder.HasIndex(x => x.Position).IsColumnStore($"CI_{TableName}_Position");

            builder.Property(x => x.BuildingId).IsRequired();
            builder.Property(x => x.ChangeType);

            builder.Property(x => x.Geometry);
            builder.Property(x => x.GeometryMethod);

            builder.Property(x => x.Status);
            //builder.Property(x => x.BuildingUnitsAsJson).HasColumnName("BuildingUnits");

            builder.Property(x => x.RecordCanBePublished);

            builder.Property(x => x.EventGeneratedAtTimeAsDateTimeOffset).HasColumnName("EventGeneratedAtTime");
            builder.Property(x => x.ObjectHash).HasColumnName("ObjectIdentifier");

            builder.Ignore(x => x.EventGeneratedAtTime);
            //builder.Ignore(x => x.BuildingUnits);

            builder.HasMany(x => x.BuildingUnits)
                .WithOne()
                .HasForeignKey(x => x.Position)
                .IsRequired();

            builder.HasIndex(x => x.BuildingId);
            builder.HasIndex(x => x.PersistentLocalId);
        }
    }
}
