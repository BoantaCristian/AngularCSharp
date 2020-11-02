using Microsoft.EntityFrameworkCore.Migrations;

namespace phoneShopApi.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Historics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Historics",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Historics");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Historics");
        }
    }
}
