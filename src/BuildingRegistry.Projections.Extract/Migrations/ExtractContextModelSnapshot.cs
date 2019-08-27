﻿// <auto-generated />
using System;
using BuildingRegistry.Projections.Extract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingRegistry.Projections.Extract.Migrations
{
    [DbContext(typeof(ExtractContext))]
    partial class ExtractContextModelSnapshot : ModelSnapshot
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

                    b.ToTable("ProjectionStates","BuildingRegistryExtract");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Extract.BuildingExtract.BuildingExtractItem", b =>
                {
                    b.Property<Guid>("BuildingId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("DbaseRecord");

                    b.Property<bool>("IsComplete");

                    b.Property<double>("MaximumX");

                    b.Property<double>("MaximumY");

                    b.Property<double>("MinimumX");

                    b.Property<double>("MinimumY");

                    b.Property<int?>("PersistentLocalId");

                    b.Property<byte[]>("ShapeRecordContent");

                    b.Property<int>("ShapeRecordContentLength");

                    b.HasKey("BuildingId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("PersistentLocalId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("Building","BuildingRegistryExtract");
                });

            modelBuilder.Entity("BuildingRegistry.Projections.Extract.BuildingUnitExtract.BuildingUnitExtractItem", b =>
                {
                    b.Property<Guid>("BuildingUnitId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("BuildingId");

                    b.Property<byte[]>("DbaseRecord");

                    b.Property<bool>("IsComplete");

                    b.Property<double>("MaximumX");

                    b.Property<double>("MaximumY");

                    b.Property<double>("MinimumX");

                    b.Property<double>("MinimumY");

                    b.Property<int?>("PersistentLocalId");

                    b.Property<byte[]>("ShapeRecordContent");

                    b.Property<int>("ShapeRecordContentLength");

                    b.HasKey("BuildingUnitId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("BuildingId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("BuildingUnit","BuildingRegistryExtract");
                });
#pragma warning restore 612, 618
        }
    }
}
