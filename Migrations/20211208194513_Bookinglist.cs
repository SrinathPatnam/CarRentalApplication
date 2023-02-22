using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRentalApplication.Migrations
{
    public partial class Bookinglist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingId1",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_BookingId1",
                table: "Booking",
                column: "BookingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Booking_BookingId1",
                table: "Booking",
                column: "BookingId1",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Booking_BookingId1",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_BookingId1",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Booking");
        }
    }
}
