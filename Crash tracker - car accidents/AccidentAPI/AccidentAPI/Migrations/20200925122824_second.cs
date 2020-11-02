using Microsoft.EntityFrameworkCore.Migrations;

namespace AccidentAPI.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeverityId",
                table: "Accidents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_SeverityId",
                table: "Accidents",
                column: "SeverityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_Severity_SeverityId",
                table: "Accidents",
                column: "SeverityId",
                principalTable: "Severity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_Severity_SeverityId",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_SeverityId",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "SeverityId",
                table: "Accidents");
        }
    }
}
