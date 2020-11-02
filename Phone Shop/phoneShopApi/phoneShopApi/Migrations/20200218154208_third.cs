using Microsoft.EntityFrameworkCore.Migrations;

namespace phoneShopApi.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Telephones_TelephoneId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_TelephoneId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TelephoneId",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Telephones",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Telephones_CompanyId",
                table: "Telephones",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Telephones_Companies_CompanyId",
                table: "Telephones",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telephones_Companies_CompanyId",
                table: "Telephones");

            migrationBuilder.DropIndex(
                name: "IX_Telephones_CompanyId",
                table: "Telephones");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Telephones");

            migrationBuilder.AddColumn<int>(
                name: "TelephoneId",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TelephoneId",
                table: "Companies",
                column: "TelephoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Telephones_TelephoneId",
                table: "Companies",
                column: "TelephoneId",
                principalTable: "Telephones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
