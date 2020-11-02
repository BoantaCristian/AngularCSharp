using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalAPI.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPacient",
                table: "Appointments",
                newName: "IdPacientId");

            migrationBuilder.AddColumn<string>(
                name: "Appointments",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentsPactient",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdPacientId",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_IdPacientId",
                table: "Appointments",
                column: "IdPacientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_IdPacientId",
                table: "Appointments",
                column: "IdPacientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_IdPacientId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_IdPacientId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Appointments",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppointmentsPactient",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IdPacientId",
                table: "Appointments",
                newName: "IdPacient");

            migrationBuilder.AlterColumn<string>(
                name: "IdPacient",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
