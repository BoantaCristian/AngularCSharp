using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociationAPI.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Archives");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Archives",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Archives");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Archives",
                nullable: false,
                defaultValue: 0);
        }
    }
}
