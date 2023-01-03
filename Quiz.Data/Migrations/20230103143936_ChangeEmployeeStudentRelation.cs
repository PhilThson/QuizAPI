using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Data.Migrations
{
    public partial class ChangeEmployeeStudentRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uczniowie_Pracownicy_WychowawcaId",
                table: "Uczniowie");

            migrationBuilder.DropIndex(
                name: "IX_Uczniowie_WychowawcaId",
                table: "Uczniowie");

            migrationBuilder.DropColumn(
                name: "WychowawcaId",
                table: "Uczniowie");

            migrationBuilder.AlterColumn<string>(
                name: "Notatki",
                table: "Wyniki",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notatki",
                table: "Wyniki",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WychowawcaId",
                table: "Uczniowie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_WychowawcaId",
                table: "Uczniowie",
                column: "WychowawcaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uczniowie_Pracownicy_WychowawcaId",
                table: "Uczniowie",
                column: "WychowawcaId",
                principalTable: "Pracownicy",
                principalColumn: "Id");
        }
    }
}
