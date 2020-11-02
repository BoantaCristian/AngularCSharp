using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Illnesses");

            migrationBuilder.RenameColumn(
                name: "Symptoms",
                table: "Illnesses",
                newName: "Risk");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Risk",
                table: "Illnesses",
                newName: "Symptoms");

            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "Illnesses",
                nullable: true);
        }
    }
}
