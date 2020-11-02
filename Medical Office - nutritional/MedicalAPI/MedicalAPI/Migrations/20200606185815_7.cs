using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "IllnessesId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IllnessesId",
                table: "AspNetUsers",
                column: "IllnessesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Illnesses_IllnessesId",
                table: "AspNetUsers",
                column: "IllnessesId",
                principalTable: "Illnesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Illnesses_IllnessesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IllnessesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IllnessesId",
                table: "AspNetUsers");

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
    }
}
