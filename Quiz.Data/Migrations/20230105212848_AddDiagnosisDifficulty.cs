using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Data.Migrations
{
    public partial class AddDiagnosisDifficulty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "SkalaTrudnosciId",
                table: "Diagnozy",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnozy_SkalaTrudnosciId",
                table: "Diagnozy",
                column: "SkalaTrudnosciId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnozy_SkaleTrudnosci_SkalaTrudnosciId",
                table: "Diagnozy",
                column: "SkalaTrudnosciId",
                principalTable: "SkaleTrudnosci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnozy_SkaleTrudnosci_SkalaTrudnosciId",
                table: "Diagnozy");

            migrationBuilder.DropIndex(
                name: "IX_Diagnozy_SkalaTrudnosciId",
                table: "Diagnozy");

            migrationBuilder.DropColumn(
                name: "SkalaTrudnosciId",
                table: "Diagnozy");
        }
    }
}
