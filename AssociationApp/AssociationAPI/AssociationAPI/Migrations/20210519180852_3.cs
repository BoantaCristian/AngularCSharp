using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociationAPI.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RepresentativeIdId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RepresentativeIdId",
                table: "AspNetUsers",
                newName: "RepresentativeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_RepresentativeIdId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_RepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RepresentativeId",
                table: "AspNetUsers",
                column: "RepresentativeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RepresentativeId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RepresentativeId",
                table: "AspNetUsers",
                newName: "RepresentativeIdId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_RepresentativeId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_RepresentativeIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RepresentativeIdId",
                table: "AspNetUsers",
                column: "RepresentativeIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
