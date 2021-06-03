using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociationAPI.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthsDelay",
                table: "Payments",
                newName: "DaysDelay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DaysDelay",
                table: "Payments",
                newName: "MonthsDelay");
        }
    }
}
