using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FisishDate",
                table: "Historics");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Historics",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "Historics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdMedic",
                table: "Historics",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdPacient",
                table: "Historics",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Minute",
                table: "Historics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Historics");

            migrationBuilder.DropColumn(
                name: "IdMedic",
                table: "Historics");

            migrationBuilder.DropColumn(
                name: "IdPacient",
                table: "Historics");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "Historics");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Historics",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FisishDate",
                table: "Historics",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
