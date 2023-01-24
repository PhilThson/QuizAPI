using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Data.Migrations
{
    public partial class AddDiagnosisInstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uczniowie_Adresy_AdresId",
                table: "Uczniowie");

            migrationBuilder.DropIndex(
                name: "IX_Uczniowie_AdresId",
                table: "Uczniowie");

            migrationBuilder.DropColumn(
                name: "AdresId",
                table: "Uczniowie");

            migrationBuilder.AddColumn<string>(
                name: "PlacowkaOswiatowa",
                table: "Diagnozy",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlacowkaOswiatowa",
                table: "Diagnozy");

            migrationBuilder.AddColumn<int>(
                name: "AdresId",
                table: "Uczniowie",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_AdresId",
                table: "Uczniowie",
                column: "AdresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uczniowie_Adresy_AdresId",
                table: "Uczniowie",
                column: "AdresId",
                principalTable: "Adresy",
                principalColumn: "Id");
        }
    }
}
