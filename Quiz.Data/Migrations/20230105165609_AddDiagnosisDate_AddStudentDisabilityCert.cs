using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Data.Migrations
{
    public partial class AddDiagnosisDate_AddStudentDisabilityCert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NrOrzeczenia",
                table: "Uczniowie",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPrzeprowadzenia",
                table: "Diagnozy",
                type: "datetime",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_Email",
                table: "Uzytkownicy",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Uzytkownicy_Email",
                table: "Uzytkownicy");

            migrationBuilder.DropColumn(
                name: "DataPrzeprowadzenia",
                table: "Diagnozy");

            migrationBuilder.DropColumn(
                name: "NrOrzeczenia",
                table: "Uczniowie");
        }
    }
}
