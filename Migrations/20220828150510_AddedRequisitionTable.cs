using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelRequisitionSystem.Migrations
{
    public partial class AddedRequisitionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requisitions",
                columns: table => new
                {
                    RequisitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequisitionNumber = table.Column<string>(maxLength: 50, nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    SourceLocation = table.Column<string>(maxLength: 100, nullable: false),
                    SourceCountry = table.Column<int>(nullable: false),
                    DestinationLocation = table.Column<string>(maxLength: 100, nullable: false),
                    DestinationCountry = table.Column<int>(nullable: false),
                    ProposedDepartureDateAndTime = table.Column<DateTime>(nullable: false),
                    TravelClass = table.Column<int>(nullable: false),
                    TripType = table.Column<int>(nullable: false),
                    ChargeCode = table.Column<string>(maxLength: 20, nullable: false),
                    TravelerName = table.Column<string>(maxLength: 100, nullable: false),
                    RequisitionStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisitions", x => x.RequisitionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requisitions");
        }
    }
}
