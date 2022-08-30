using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelRequisitionSystem.Migrations
{
    public partial class AddedDateUpdatedToRequisitionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Requisitions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Requisitions");
        }
    }
}
