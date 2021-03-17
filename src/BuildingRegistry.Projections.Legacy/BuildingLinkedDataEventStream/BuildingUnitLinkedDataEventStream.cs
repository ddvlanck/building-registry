namespace BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.MigrationExtensions;
    using BuildingRegistry.Infrastructure;
    using BuildingRegistry.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BuildingUnitLinkedDataEventStreamItem
    {
        public long Position { get; set; }
        public string ChangeType { get; set; }
        public Guid BuildingUnitId { get; set; }
        public Guid BuildingId { get; set; }
        public int? PersistentLocalId { get; set; }
        public byte[]? PointPosition { get; set; }

        public BuildingUnitFunction? Function
        {
            get => BuildingUnitFunction.Parse(FunctionAsString);
            set => FunctionAsString = value.HasValue ? value.Value.Function : string.Empty;
        }

        public string? FunctionAsString { get; set; }

        public BuildingUnitPositionGeometryMethod? PositionMethod
        {
            get => string.IsNullOrEmpty(PositionMethodAsString) ? null : (BuildingUnitPositionGeometryMethod?)BuildingUnitPositionGeometryMethod.Parse(PositionMethodAsString);
            set => PositionMethodAsString = value?.GeometryMethod;
        }
        public string? PositionMethodAsString { get; set; }

        public BuildingUnitStatus? Status
        {
            get => BuildingUnitStatus.Parse(StatusAsString);
            set => StatusAsString = value.HasValue ? value.Value.Status : string.Empty;
        }
        public string? StatusAsString { get; set; }

        public DateTimeOffset EventGeneratedAtTimeAsDateTimeOffset { get; set; }

        public Instant EventGeneratedAtTime
        {
            get => Instant.FromDateTimeOffset(EventGeneratedAtTimeAsDateTimeOffset);
            set => EventGeneratedAtTimeAsDateTimeOffset = value.ToDateTimeOffset();
        }

        public string? ObjectHash { get; set; }
        public bool? RecordCanBePublished { get; set; }

        public Collection<BuildingUnitAddressLinkedDataEventStreamItem> Addresses { get; set; }
        public Collection<BuildingUnitReaddressLinkedDataEventStreamItem> Readdresses { get; set; }

        public BuildingUnitLinkedDataEventStreamItem()
        {
            Addresses = new Collection<BuildingUnitAddressLinkedDataEventStreamItem>();
            Readdresses = new Collection<BuildingUnitReaddressLinkedDataEventStreamItem>();
        }

        public BuildingUnitLinkedDataEventStreamItem CloneAndApplyEventInfo(
            long position,
            Guid buildingId)
        {
            var newItem = new BuildingUnitLinkedDataEventStreamItem
            {
                Position = position,
                BuildingId = buildingId,

                BuildingUnitId = BuildingUnitId,
                PersistentLocalId = PersistentLocalId,
                PointPosition = PointPosition,
                Function = Function,
                PositionMethod = PositionMethod,
                Status = Status,
                EventGeneratedAtTime = EventGeneratedAtTime,
                ChangeType = ChangeType,
                Addresses = new Collection<BuildingUnitAddressLinkedDataEventStreamItem>(Addresses.Select(x => x.CloneAndApplyEventInfo(position)).ToList()),
                Readdresses = new Collection<BuildingUnitReaddressLinkedDataEventStreamItem>(Readdresses.Select(x => x.CloneAndApplyEventInfo(position)).ToList()),
            };

            return newItem;
        }
    }

    public class BuildingUnitLinkedDataEventStreamConfiguration : IEntityTypeConfiguration<BuildingUnitLinkedDataEventStreamItem>
    {
        private const string TableName = "BuildingUnitLinkedDataEventStream";

        public void Configure(EntityTypeBuilder<BuildingUnitLinkedDataEventStreamItem> builder)
        {
            builder.ToTable(TableName, Schema.Legacy)
                .HasKey(p => new { p.Position, p.BuildingUnitId })
                .IsClustered(false);

            builder.Property(p => p.Position);
            builder.Property(p => p.BuildingUnitId);

            builder.HasIndex(p => new { p.Position, p.BuildingUnitId }).IsColumnStore($"CI_{TableName}_Position_BuildingUnitId");

            builder.Property(p => p.ChangeType);
            builder.Property(p => p.BuildingId);

            builder.Property(p => p.PersistentLocalId);
            builder.Property(p => p.PointPosition);

            builder.Property(p => p.EventGeneratedAtTimeAsDateTimeOffset).HasColumnName("EventGeneratedAtTime");
            builder.Property(p => p.ObjectHash).HasColumnName("ObjectIdentifier");

            builder.Property(p => p.RecordCanBePublished);

            builder.Ignore(p => p.EventGeneratedAtTime);

            builder.Ignore(p => p.Status);
            builder.Property(p => p.StatusAsString)
                .HasColumnName("Status");

            builder.Ignore(p => p.PositionMethod);
            builder.Property(p => p.PositionMethodAsString)
                .HasColumnName("PositionMethod");

            builder.Ignore(p => p.Function);
            builder.Property(p => p.FunctionAsString)
                .HasColumnName("Function");

            builder.HasMany(p => p.Addresses)
                .WithOne()
                .IsRequired()
                .HasForeignKey(p => new { p.Position, p.BuildingUnitId });

            builder.HasMany(p => p.Readdresses)
                .WithOne()
                .IsRequired()
                .HasForeignKey(p => new { p.Position, p.BuildingUnitId });
        }
    }
}
