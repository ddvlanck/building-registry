﻿// <auto-generated />
using System;
using BuildingRegistry.Projections.Legacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingRegistry.Projections.Legacy.Migrations
{
    [DbContext(typeof(LegacyContext))]
    [Migration("20210316155849_AddTableForLinkedDataEventStream")]
    partial class AddTableForLinkedDataEventStream
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DesiredState")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name")
                        .IsClustered();

                    b.ToTable("ProjectionStates", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingDetail.BuildingDetailItem", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Geometry")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("GeometryMethod")
                        .HasColumnType("int");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingId")
                        .IsClustered(false);

                    b.HasIndex("PersistentLocalId")
                        .IsClustered();

                    b.HasIndex("Status");

                    b.HasIndex("IsComplete", "IsRemoved", "PersistentLocalId");

                    b.ToTable("BuildingDetails", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingDetail.BuildingDetailListCountView", b =>
                {
                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.ToView("vw_BuildingDetailListCountView", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingLinkedDataEventStreamItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("BuildingId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChangeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("EventGeneratedAtTimeAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("EventGeneratedAtTime");

                    b.Property<byte[]>("Geometry")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("GeometryMethod")
                        .HasColumnType("int");

                    b.Property<string>("ObjectHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ObjectIdentifier");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<bool>("RecordCanBePublished")
                        .HasColumnType("bit");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("Position")
                        .IsClustered();

                    b.HasIndex("BuildingId");

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Position")
                        .HasDatabaseName("CI_BuildingLinkedDataEventStream_Position")
                        .HasAnnotation("SqlServer:ColumnStoreIndex", "");

                    b.ToTable("BuildingLinkedDataEventStream", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitAddressLinkedDataEventStreamItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("Position", "BuildingUnitId", "AddressId")
                        .IsClustered(false);

                    b.ToTable("BuildingUnitAddressLinkedDataEventStream", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitLinkedDataEventStreamItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("EventGeneratedAtTimeAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("EventGeneratedAtTime");

                    b.Property<string>("FunctionAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Function");

                    b.Property<string>("ObjectHash")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ObjectIdentifier");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PointPosition")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PositionMethodAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PositionMethod");

                    b.Property<bool?>("RecordCanBePublished")
                        .HasColumnType("bit");

                    b.Property<string>("StatusAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Status");

                    b.HasKey("Position", "BuildingUnitId")
                        .IsClustered(false);

                    b.HasIndex("Position", "BuildingUnitId")
                        .HasDatabaseName("CI_BuildingUnitLinkedDataEventStream_Position_BuildingUnitId")
                        .HasAnnotation("SqlServer:ColumnStoreIndex", "");

                    b.ToTable("BuildingUnitLinkedDataEventStream", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitReaddressLinkedDataEventStreamItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OldAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NewAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ReaddressBeginDateAsDateTimeOffset")
                        .HasColumnType("datetime2")
                        .HasColumnName("ReaddressDate");

                    b.HasKey("Position", "BuildingUnitId", "OldAddressId")
                        .IsClustered(false);

                    b.ToTable("BuildingUnitReaddressLinkedDataEventStream", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingPersistentIdCrabIdMapping.BuildingPersistentLocalIdCrabIdMapping", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CrabIdentifierTerrainObject")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CrabTerrainObjectId")
                        .HasColumnType("int");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.HasKey("BuildingId")
                        .IsClustered(false);

                    b.HasIndex("CrabIdentifierTerrainObject");

                    b.HasIndex("CrabTerrainObjectId");

                    b.HasIndex("PersistentLocalId")
                        .IsClustered();

                    b.ToTable("BuildingPersistentIdCrabIdMappings", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingSyndicationItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<int?>("Application")
                        .HasColumnType("int");

                    b.Property<Guid?>("BuildingId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ChangeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventDataAsXml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Geometry")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("GeometryMethod")
                        .HasColumnType("int");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastChangedOnAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("LastChangedOn");

                    b.Property<int?>("Modification")
                        .HasColumnType("int");

                    b.Property<string>("Operator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Organisation")
                        .HasColumnType("int");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("RecordCreatedAtAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("RecordCreatedAt");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("SyndicationItemCreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Position")
                        .IsClustered();

                    b.HasIndex("BuildingId");

                    b.HasIndex("PersistentLocalId");

                    b.HasIndex("Position")
                        .HasDatabaseName("CI_BuildingSyndication_Position")
                        .HasAnnotation("SqlServer:ColumnStoreIndex", "");

                    b.ToTable("BuildingSyndication", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitAddressSyndicationItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("Position", "BuildingUnitId", "AddressId")
                        .IsClustered(false);

                    b.ToTable("BuildingUnitAddressSyndication", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitReaddressSyndicationItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OldAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NewAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ReaddressBeginDateAsDateTimeOffset")
                        .HasColumnType("datetime2")
                        .HasColumnName("ReaddressDate");

                    b.HasKey("Position", "BuildingUnitId", "OldAddressId")
                        .IsClustered(false);

                    b.ToTable("BuildingUnitReaddressSyndication", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", b =>
                {
                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FunctionAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Function");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PointPosition")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PositionMethodAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PositionMethod");

                    b.Property<string>("StatusAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Status");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("Position", "BuildingUnitId")
                        .IsClustered(false);

                    b.HasIndex("Position", "BuildingUnitId")
                        .HasDatabaseName("CI_BuildingUnitSyndication_Position_BuildingUnitId")
                        .HasAnnotation("SqlServer:ColumnStoreIndex", "");

                    b.ToTable("BuildingUnitSyndication", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitBuildingItem", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BuildingPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("BuildingRetiredStatus")
                        .HasColumnType("int");

                    b.Property<bool?>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.HasKey("BuildingId")
                        .IsClustered(false);

                    b.HasIndex("BuildingPersistentLocalId");

                    b.ToTable("BuildingUnit_Buildings", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailAddressItem", b =>
                {
                    b.Property<Guid>("BuildingUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("BuildingUnitId", "AddressId")
                        .IsClustered(false);

                    b.HasIndex("AddressId")
                        .IsClustered(false);

                    b.ToTable("BuildingUnitAddresses", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailItem", b =>
                {
                    b.Property<Guid>("BuildingUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BuildingPersistentLocalId")
                        .HasColumnType("int");

                    b.Property<string>("FunctionAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Function");

                    b.Property<bool>("IsBuildingComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<int?>("PersistentLocalId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Position")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PositionMethodAsString")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PositionMethod");

                    b.Property<string>("StatusAsString")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Status");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnType("datetimeoffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingUnitId")
                        .IsClustered(false);

                    b.HasIndex("BuildingId");

                    b.HasIndex("BuildingPersistentLocalId");

                    b.HasIndex("PersistentLocalId")
                        .IsClustered();

                    b.HasIndex("StatusAsString");

                    b.HasIndex("IsComplete", "IsRemoved", "PersistentLocalId", "IsBuildingComplete", "BuildingPersistentLocalId");

                    b.ToTable("BuildingUnitDetails", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailListCountView", b =>
                {
                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.ToView("vw_BuildingUnitDetailListCountView", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.PersistentLocalIdMigration.DuplicatedPersistentLocalId", b =>
                {
                    b.Property<int>("DuplicatePersistentLocalId")
                        .HasColumnType("int");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OriginalPersistentLocalId")
                        .HasColumnType("int");

                    b.HasKey("DuplicatePersistentLocalId")
                        .IsClustered(false);

                    b.ToTable("DuplicatedPersistentLocalIds", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.PersistentLocalIdMigration.RemovedPersistentLocalId", b =>
                {
                    b.Property<string>("PersistentLocalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersistentLocalId")
                        .IsClustered(false);

                    b.ToTable("RemovedPersistentLocalIds", "BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitAddressLinkedDataEventStreamItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitLinkedDataEventStreamItem", null)
                        .WithMany("Addresses")
                        .HasForeignKey("Position", "BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitLinkedDataEventStreamItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingLinkedDataEventStreamItem", null)
                        .WithMany("BuildingUnits")
                        .HasForeignKey("Position")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitReaddressLinkedDataEventStreamItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitLinkedDataEventStreamItem", null)
                        .WithMany("Readdresses")
                        .HasForeignKey("Position", "BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitAddressSyndicationItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", null)
                        .WithMany("Addresses")
                        .HasForeignKey("Position", "BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitReaddressSyndicationItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", null)
                        .WithMany("Readdresses")
                        .HasForeignKey("Position", "BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingSyndicationItem", null)
                        .WithMany("BuildingUnits")
                        .HasForeignKey("Position")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailAddressItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailItem", null)
                        .WithMany("Addresses")
                        .HasForeignKey("BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingLinkedDataEventStreamItem", b =>
                {
                    b.Navigation("BuildingUnits");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingLinkedDataEventStream.BuildingUnitLinkedDataEventStreamItem", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Readdresses");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingSyndicationItem", b =>
                {
                    b.Navigation("BuildingUnits");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Readdresses");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailItem", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}
