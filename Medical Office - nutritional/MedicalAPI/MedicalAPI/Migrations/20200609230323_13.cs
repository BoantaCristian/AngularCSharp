using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasalMetabolism",
                table: "Details",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Breakfast",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Dinner",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Glucid",
                table: "Details",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "GlucidGrams",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Launch",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Lipid",
                table: "Details",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "LipidGrams",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Protein",
                table: "Details",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ProteindGrams",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack1",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack2",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasalMetabolism",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Breakfast",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Dinner",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Glucid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "GlucidGrams",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Launch",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Lipid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "LipidGrams",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "ProteindGrams",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack1",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack2",
                table: "Details");
        }
    }
}
