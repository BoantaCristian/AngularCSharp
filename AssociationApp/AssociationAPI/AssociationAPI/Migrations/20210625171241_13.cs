using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociationAPI.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sanitation",
                table: "Receipts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorkingCapital",
                table: "Receipts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sanitation",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "WorkingCapital",
                table: "Receipts");
        }
    }
}
