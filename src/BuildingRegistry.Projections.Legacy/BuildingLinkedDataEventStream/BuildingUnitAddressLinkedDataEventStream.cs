namespace BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream
{
    using BuildingRegistry.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BuildingUnitAddressLinkedDataEventStreamItem
    {
        public long Position { get; set; }
        public Guid BuildingUnitId { get; set; }
        public Guid? AddressId { get; set; }
        public int Count { get; set; }

        public BuildingUnitAddressLinkedDataEventStreamItem CloneAndApplyEventInfo(long position)
        {
            var newItem = new BuildingUnitAddressLinkedDataEventStreamItem
            {
                Position = position,
                BuildingUnitId = BuildingUnitId,
                AddressId = AddressId,
                Count = Count
            };

            return newItem;
        }
    }

    public class BuildingUnitAddressLinkedDataEventStreamItemItemConfiguration : IEntityTypeConfiguration<BuildingUnitAddressLinkedDataEventStreamItem>
    {
        private const string TableName = "BuildingUnitAddressLinkedDataEventStream";

        public void Configure(EntityTypeBuilder<BuildingUnitAddressLinkedDataEventStreamItem> b)
        {
            b.ToTable(TableName, Schema.Legacy)
                .HasKey(p => new { p.Position, p.BuildingUnitId, p.AddressId })
                .IsClustered(false);

            b.Property(x => x.Count);
        }
    }
}
