using Microsoft.EntityFrameworkCore.Migrations;

namespace AccidentAPI.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_People_PeopleId",
                table: "Accidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_People_PeopleId1",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_PeopleId",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_PeopleId1",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Accidents");

            migrationBuilder.DropColumn(
                name: "PeopleId1",
                table: "Accidents");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_Guilty",
                table: "Accidents",
                column: "Guilty");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_Innocent",
                table: "Accidents",
                column: "Innocent");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_People_Guilty",
                table: "Accidents",
                column: "Guilty",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_People_Innocent",
                table: "Accidents",
                column: "Innocent",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_People_Guilty",
                table: "Accidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Accidents_People_Innocent",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_Guilty",
                table: "Accidents");

            migrationBuilder.DropIndex(
                name: "IX_Accidents_Innocent",
                table: "Accidents");

            migrationBuilder.AddColumn<int>(
                name: "PeopleId",
                table: "Accidents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeopleId1",
                table: "Accidents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_PeopleId",
                table: "Accidents",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Accidents_PeopleId1",
                table: "Accidents",
                column: "PeopleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_People_PeopleId",
                table: "Accidents",
                column: "PeopleId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accidents_People_PeopleId1",
                table: "Accidents",
                column: "PeopleId1",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
