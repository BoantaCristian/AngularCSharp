using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatinetIllIdId",
                table: "Illnesses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Illnesses_PatinetIllIdId",
                table: "Illnesses",
                column: "PatinetIllIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Illnesses_AspNetUsers_PatinetIllIdId",
                table: "Illnesses",
                column: "PatinetIllIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Illnesses_AspNetUsers_PatinetIllIdId",
                table: "Illnesses");

            migrationBuilder.DropIndex(
                name: "IX_Illnesses_PatinetIllIdId",
                table: "Illnesses");

            migrationBuilder.DropColumn(
                name: "PatinetIllIdId",
                table: "Illnesses");
        }
    }
}
