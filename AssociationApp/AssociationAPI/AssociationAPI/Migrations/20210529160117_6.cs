using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociationAPI.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MonthsDelay",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MonthsDelay",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
