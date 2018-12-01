using Microsoft.EntityFrameworkCore.Migrations;

namespace TourServer.Migrations
{
    public partial class Sdjfhsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Orders");
        }
    }
}
