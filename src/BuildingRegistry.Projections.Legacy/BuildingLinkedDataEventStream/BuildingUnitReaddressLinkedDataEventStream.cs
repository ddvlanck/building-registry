namespace BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream
{
    using BuildingRegistry.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NodaTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BuildingUnitReaddressLinkedDataEventStreamItem
    {
        public long Position { get; set; }
        public Guid BuildingUnitId { get; set; }
        public Guid OldAddressId { get; set; }
        public Guid NewAddressId { get; set; }
        public DateTime ReaddressBeginDateAsDateTimeOffset { get; set; }

        public LocalDate ReaddressBeginDate
        {
            get => LocalDate.FromDateTime(ReaddressBeginDateAsDateTimeOffset);
            set => ReaddressBeginDateAsDateTimeOffset = value.ToDateTimeUnspecified();
        }

        public BuildingUnitReaddressLinkedDataEventStreamItem CloneAndApplyEventInfo(long position)
        {
            var newItem = new BuildingUnitReaddressLinkedDataEventStreamItem
            {
                Position = position,
                BuildingUnitId = BuildingUnitId,
                OldAddressId = OldAddressId,
                NewAddressId = NewAddressId,
                ReaddressBeginDate = ReaddressBeginDate
            };

            return newItem;
        }
    }

    public class BuildingUnitReaddressLinkedDataEventStreamItemConfiguration : IEntityTypeConfiguration<BuildingUnitReaddressLinkedDataEventStreamItem>
    {
        private const string TableName = "BuildingUnitReaddressLinkedDataEventStream";

        public void Configure(EntityTypeBuilder<BuildingUnitReaddressLinkedDataEventStreamItem> b)
        {
            b.ToTable(TableName, Schema.Legacy)
                .HasKey(p => new { p.Position, p.BuildingUnitId, p.OldAddressId })
                .IsClustered(false);

            b.Property(x => x.NewAddressId);
            b.Property(x => x.ReaddressBeginDateAsDateTimeOffset).HasColumnName("ReaddressDate");
            b.Ignore(x => x.ReaddressBeginDate);
        }
    }
}
