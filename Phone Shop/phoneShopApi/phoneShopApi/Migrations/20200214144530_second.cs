using Microsoft.EntityFrameworkCore.Migrations;

namespace phoneShopApi.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_Descriptions_DescriptionsId",
                table: "Descriptions");

            migrationBuilder.DropIndex(
                name: "IX_Descriptions_DescriptionsId",
                table: "Descriptions");

            migrationBuilder.DropColumn(
                name: "DescriptionsId",
                table: "Descriptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescriptionsId",
                table: "Descriptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_DescriptionsId",
                table: "Descriptions",
                column: "DescriptionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_Descriptions_DescriptionsId",
                table: "Descriptions",
                column: "DescriptionsId",
                principalTable: "Descriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
