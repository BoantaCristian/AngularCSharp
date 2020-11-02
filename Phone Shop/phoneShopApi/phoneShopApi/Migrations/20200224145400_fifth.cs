using Microsoft.EntityFrameworkCore.Migrations;

namespace phoneShopApi.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historics_Telephones_TelephoneId",
                table: "Historics");

            migrationBuilder.DropIndex(
                name: "IX_Historics_TelephoneId",
                table: "Historics");

            migrationBuilder.DropColumn(
                name: "TelephoneId",
                table: "Historics");

            migrationBuilder.AddColumn<string>(
                name: "Order",
                table: "Historics",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Historics");

            migrationBuilder.AddColumn<int>(
                name: "TelephoneId",
                table: "Historics",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Historics_TelephoneId",
                table: "Historics",
                column: "TelephoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historics_Telephones_TelephoneId",
                table: "Historics",
                column: "TelephoneId",
                principalTable: "Telephones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
