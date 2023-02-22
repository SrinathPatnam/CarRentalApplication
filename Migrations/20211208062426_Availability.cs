using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalApplication.Migrations
{
    public partial class Availability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avail_end_date",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Avail_start_date",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Car");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Avail_end_date",
                table: "Car",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Avail_start_date",
                table: "Car",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Car",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
