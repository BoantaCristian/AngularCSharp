using Microsoft.EntityFrameworkCore.Migrations;

namespace AccidentAPI.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSupervisorId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserSupervisorId",
                table: "AspNetUsers",
                column: "UserSupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UserSupervisorId",
                table: "AspNetUsers",
                column: "UserSupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UserSupervisorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserSupervisorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserSupervisorId",
                table: "AspNetUsers");
        }
    }
}
