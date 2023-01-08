using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Data.Migrations
{
    public partial class AddReportEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Raporty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zawartosc = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Rozmiar = table.Column<long>(type: "bigint", nullable: false),
                    DiagnozaId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raporty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Raporty_Diagnozy_DiagnozaId",
                        column: x => x.DiagnozaId,
                        principalTable: "Diagnozy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Raporty_DiagnozaId",
                table: "Raporty",
                column: "DiagnozaId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Raporty");
        }
    }
}
