using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingRegistry.Projections.Legacy.Migrations
{
    public partial class AddTableForLinkedDataEventStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuildingLinkedDataEventStream",
                schema: "BuildingRegistryLegacy",
                columns: table => new
                {
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersistentLocalId = table.Column<int>(type: "int", nullable: true),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Geometry = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    GeometryMethod = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    EventGeneratedAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ObjectIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordCanBePublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingLinkedDataEventStream", x => x.Position)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "BuildingUnitLinkedDataEventStream",
                schema: "BuildingRegistryLegacy",
                columns: table => new
                {
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    BuildingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersistentLocalId = table.Column<int>(type: "int", nullable: true),
                    PointPosition = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventGeneratedAtTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ObjectIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordCanBePublished = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingUnitLinkedDataEventStream", x => new { x.Position, x.BuildingUnitId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BuildingUnitLinkedDataEventStream_BuildingLinkedDataEventStream_Position",
                        column: x => x.Position,
                        principalSchema: "BuildingRegistryLegacy",
                        principalTable: "BuildingLinkedDataEventStream",
                        principalColumn: "Position",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingUnitAddressLinkedDataEventStream",
                schema: "BuildingRegistryLegacy",
                columns: table => new
                {
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    BuildingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingUnitAddressLinkedDataEventStream", x => new { x.Position, x.BuildingUnitId, x.AddressId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BuildingUnitAddressLinkedDataEventStream_BuildingUnitLinkedDataEventStream_Position_BuildingUnitId",
                        columns: x => new { x.Position, x.BuildingUnitId },
                        principalSchema: "BuildingRegistryLegacy",
                        principalTable: "BuildingUnitLinkedDataEventStream",
                        principalColumns: new[] { "Position", "BuildingUnitId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingUnitReaddressLinkedDataEventStream",
                schema: "BuildingRegistryLegacy",
                columns: table => new
                {
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    BuildingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NewAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReaddressDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingUnitReaddressLinkedDataEventStream", x => new { x.Position, x.BuildingUnitId, x.OldAddressId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BuildingUnitReaddressLinkedDataEventStream_BuildingUnitLinkedDataEventStream_Position_BuildingUnitId",
                        columns: x => new { x.Position, x.BuildingUnitId },
                        principalSchema: "BuildingRegistryLegacy",
                        principalTable: "BuildingUnitLinkedDataEventStream",
                        principalColumns: new[] { "Position", "BuildingUnitId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "CI_BuildingLinkedDataEventStream_Position",
                schema: "BuildingRegistryLegacy",
                table: "BuildingLinkedDataEventStream",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLinkedDataEventStream_BuildingId",
                schema: "BuildingRegistryLegacy",
                table: "BuildingLinkedDataEventStream",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingLinkedDataEventStream_PersistentLocalId",
                schema: "BuildingRegistryLegacy",
                table: "BuildingLinkedDataEventStream",
                column: "PersistentLocalId");

            migrationBuilder.CreateIndex(
                name: "CI_BuildingUnitLinkedDataEventStream_Position_BuildingUnitId",
                schema: "BuildingRegistryLegacy",
                table: "BuildingUnitLinkedDataEventStream",
                columns: new[] { "Position", "BuildingUnitId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingUnitAddressLinkedDataEventStream",
                schema: "BuildingRegistryLegacy");

            migrationBuilder.DropTable(
                name: "BuildingUnitReaddressLinkedDataEventStream",
                schema: "BuildingRegistryLegacy");

            migrationBuilder.DropTable(
                name: "BuildingUnitLinkedDataEventStream",
                schema: "BuildingRegistryLegacy");

            migrationBuilder.DropTable(
                name: "BuildingLinkedDataEventStream",
                schema: "BuildingRegistryLegacy");
        }
    }
}
