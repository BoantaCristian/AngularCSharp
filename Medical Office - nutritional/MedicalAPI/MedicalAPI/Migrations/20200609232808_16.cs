using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BreakfastGlucid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BreakfastLipid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BreakfastProtein",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DinnerGlucid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DinnerLipid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DinnerProtein",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaunchGlucid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaunchLipid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaunchProtein",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack1Glucid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack1Lipid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack1Protein",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack2Glucid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack2Lipid",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Snack2Protein",
                table: "Details",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakfastGlucid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "BreakfastLipid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "BreakfastProtein",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "DinnerGlucid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "DinnerLipid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "DinnerProtein",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "LaunchGlucid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "LaunchLipid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "LaunchProtein",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack1Glucid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack1Lipid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack1Protein",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack2Glucid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack2Lipid",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Snack2Protein",
                table: "Details");
        }
    }
}
