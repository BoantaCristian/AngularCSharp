using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssociationAPI.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColdWaterLiterPrice",
                table: "Associations");

            migrationBuilder.DropColumn(
                name: "Electricity",
                table: "Associations");

            migrationBuilder.DropColumn(
                name: "GasPrice",
                table: "Associations");

            migrationBuilder.DropColumn(
                name: "HotWaterLiterPrice",
                table: "Associations");

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Program = table.Column<string>(nullable: true),
                    HotWaterLiterPrice = table.Column<double>(nullable: false),
                    ColdWaterLiterPrice = table.Column<double>(nullable: false),
                    GasPrice = table.Column<double>(nullable: false),
                    ElectricityPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<string>(nullable: true),
                    WaterFk = table.Column<int>(nullable: false),
                    GasFk = table.Column<int>(nullable: false),
                    ElectricityFk = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProviders_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientProviders_Providers_ElectricityFk",
                        column: x => x.ElectricityFk,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientProviders_Providers_GasFk",
                        column: x => x.GasFk,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientProviders_Providers_WaterFk",
                        column: x => x.WaterFk,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviders_ClientId",
                table: "ClientProviders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviders_ElectricityFk",
                table: "ClientProviders",
                column: "ElectricityFk");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviders_GasFk",
                table: "ClientProviders",
                column: "GasFk");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviders_WaterFk",
                table: "ClientProviders",
                column: "WaterFk");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProviders");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterLiterPrice",
                table: "Associations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Electricity",
                table: "Associations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GasPrice",
                table: "Associations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterLiterPrice",
                table: "Associations",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
