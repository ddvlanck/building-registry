﻿// <auto-generated />
using System;
using BuildingRegistry.Projections.Legacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingRegistry.Projections.Legacy.Migrations
{
    [DbContext(typeof(LegacyContext))]
    partial class LegacyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DesiredState");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt");

                    b.Property<long>("Position");

                    b.HasKey("Name")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ProjectionStates","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingDetail.BuildingDetailItem", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Geometry");

                    b.Property<int?>("GeometryMethod");

                    b.Property<bool>("IsComplete");

                    b.Property<bool>("IsRemoved");

                    b.Property<int?>("PersistentLocalId");

                    b.Property<int?>("Status");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("PersistentLocalId")
                        .IsUnique()
                        .HasFilter("[PersistentLocalId] IS NOT NULL");

                    b.ToTable("BuildingDetails","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingSyndicationItem", b =>
                {
                    b.Property<long>("Position");

                    b.Property<int?>("Application");

                    b.Property<Guid?>("BuildingId")
                        .IsRequired();

                    b.Property<string>("ChangeType");

                    b.Property<string>("EventDataAsXml");

                    b.Property<byte[]>("Geometry");

                    b.Property<int?>("GeometryMethod");

                    b.Property<bool>("IsComplete");

                    b.Property<DateTimeOffset>("LastChangedOnAsDateTimeOffset")
                        .HasColumnName("LastChangedOn");

                    b.Property<int?>("Modification");

                    b.Property<string>("Operator");

                    b.Property<int?>("Organisation");

                    b.Property<int?>("PersistentLocalId");

                    b.Property<string>("Reason");

                    b.Property<DateTimeOffset>("RecordCreatedAtAsDateTimeOffset")
                        .HasColumnName("RecordCreatedAt");

                    b.Property<int?>("Status");

                    b.HasKey("Position")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("BuildingId");

                    b.HasIndex("PersistentLocalId");

                    b.ToTable("BuildingSyndication","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitAddressSyndicationItem", b =>
                {
                    b.Property<long>("Position");

                    b.Property<Guid>("BuildingUnitId");

                    b.Property<Guid?>("AddressId");

                    b.Property<int>("Count");

                    b.HasKey("Position", "BuildingUnitId", "AddressId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("BuildingUnitAddressSyndication","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitReaddressSyndicationItem", b =>
                {
                    b.Property<long>("Position");

                    b.Property<Guid>("BuildingUnitId");

                    b.Property<Guid>("OldAddressId");

                    b.Property<Guid>("NewAddressId");

                    b.Property<DateTime>("ReaddressBeginDateAsDateTimeOffset")
                        .HasColumnName("ReaddressDate");

                    b.HasKey("Position", "BuildingUnitId", "OldAddressId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("BuildingUnitReaddressSyndication","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", b =>
                {
                    b.Property<long>("Position");

                    b.Property<Guid>("BuildingUnitId");

                    b.Property<string>("FunctionAsString")
                        .HasColumnName("Function");

                    b.Property<bool>("IsComplete");

                    b.Property<int?>("PersistentLocalId");

                    b.Property<byte[]>("PointPosition");

                    b.Property<string>("PositionMethodAsString")
                        .HasColumnName("PositionMethod");

                    b.Property<string>("StatusAsString")
                        .HasColumnName("Status");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("Version");

                    b.HasKey("Position", "BuildingUnitId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("BuildingUnitSyndication","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitBuildingItem", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BuildingPersistentLocalId");

                    b.Property<int?>("BuildingRetiredStatus");

                    b.Property<bool?>("IsComplete");

                    b.Property<bool>("IsRemoved");

                    b.HasKey("BuildingId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("BuildingPersistentLocalId");

                    b.ToTable("BuildingUnit_Buildings","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailAddressItem", b =>
                {
                    b.Property<Guid>("BuildingUnitId");

                    b.Property<Guid>("AddressId");

                    b.Property<int>("Count");

                    b.HasKey("BuildingUnitId", "AddressId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("BuildingUnitAddresses","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailItem", b =>
                {
                    b.Property<Guid>("BuildingUnitId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BuildingId");

                    b.Property<int?>("BuildingPersistentLocalId");

                    b.Property<string>("FunctionAsString")
                        .HasColumnName("Function");

                    b.Property<bool>("IsBuildingComplete");

                    b.Property<bool>("IsComplete");

                    b.Property<bool>("IsRemoved");

                    b.Property<int?>("PersistentLocalId");

                    b.Property<byte[]>("Position");

                    b.Property<string>("PositionMethodAsString")
                        .HasColumnName("PositionMethod");

                    b.Property<string>("StatusAsString")
                        .HasColumnName("Status");

                    b.Property<DateTimeOffset>("VersionTimestampAsDateTimeOffset")
                        .HasColumnName("Version");

                    b.HasKey("BuildingUnitId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("BuildingPersistentLocalId");

                    b.HasIndex("PersistentLocalId")
                        .IsUnique()
                        .HasFilter("[PersistentLocalId] IS NOT NULL");

                    b.ToTable("BuildingUnitDetails","BuildingRegistryLegacy");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitAddressSyndicationItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem")
                        .WithMany("Addresses")
                        .HasForeignKey("Position", "BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitReaddressSyndicationItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem")
                        .WithMany("Readdresses")
                        .HasForeignKey("Position", "BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingUnitSyndicationItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingSyndication.BuildingSyndicationItem")
                        .WithMany("BuildingUnits")
                        .HasForeignKey("Position")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailAddressItem", b =>
                {
                    b.HasOne("BuildingRegistry.Projections.Legacy.BuildingUnitDetail.BuildingUnitDetailItem")
                        .WithMany("Addresses")
                        .HasForeignKey("BuildingUnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
