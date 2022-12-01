using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Data.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adresy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Panstwo = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Miejscowosc = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NumerDomu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumerMieszkania = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    KodPocztowy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etaty",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etaty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObszaryZestawowPytan",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazwaRozszerzona = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObszaryZestawowPytan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Przedmioty",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkroconaNazwa = table.Column<string>(type: "char(3)", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przedmioty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkaleTrudnosci",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkaleTrudnosci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stanowiska",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanowiska", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HashHasla = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RolaId = table.Column<byte>(type: "tinyint", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uzytkownicy_Role_RolaId",
                        column: x => x.RolaId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZestawyPytan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpisUmiejetnosci = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ObszarZestawuPytanId = table.Column<byte>(type: "tinyint", nullable: false),
                    SkalaTrudnosciId = table.Column<byte>(type: "tinyint", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZestawyPytan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZestawyPytan_ObszaryZestawowPytan_ObszarZestawuPytanId",
                        column: x => x.ObszarZestawuPytanId,
                        principalTable: "ObszaryZestawowPytan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZestawyPytan_SkaleTrudnosci_SkalaTrudnosciId",
                        column: x => x.SkalaTrudnosciId,
                        principalTable: "SkaleTrudnosci",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pracownicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pensja = table.Column<decimal>(type: "money", precision: 7, scale: 2, nullable: false),
                    DniUrlopu = table.Column<int>(type: "int", nullable: true),
                    WymiarGodzinowy = table.Column<double>(type: "float", nullable: true),
                    Nadgodziny = table.Column<double>(type: "float", nullable: true),
                    NrTelefonu = table.Column<string>(type: "varchar(11)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EtatId = table.Column<byte>(type: "tinyint", nullable: false),
                    StanowiskoId = table.Column<byte>(type: "tinyint", nullable: false),
                    DataZatrudnienia = table.Column<DateTime>(type: "date", nullable: false),
                    DataKoncaZatrudnienia = table.Column<DateTime>(type: "date", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Imie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DrugieImie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataUrodzenia = table.Column<DateTime>(type: "date", maxLength: 10, nullable: true),
                    MiejsceUrodzenia = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Pesel = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pracownicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pracownicy_Etaty_EtatId",
                        column: x => x.EtatId,
                        principalTable: "Etaty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pracownicy_Stanowiska_StanowiskoId",
                        column: x => x.StanowiskoId,
                        principalTable: "Stanowiska",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Migawki",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataZmiany = table.Column<DateTime>(type: "datetime", nullable: false, computedColumnSql: "getdate()"),
                    Szczegoly = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UzytkownikId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Migawki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Migawki_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KartyPracy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zawartosc = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RodzajZawartosci = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Rozmiar = table.Column<long>(type: "bigint", nullable: false),
                    ZestawPytanId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KartyPracy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KartyPracy_ZestawyPytan_ZestawPytanId",
                        column: x => x.ZestawPytanId,
                        principalTable: "ZestawyPytan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OcenyZestawowPytan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpisOceny = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ZestawPytanId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcenyZestawowPytan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OcenyZestawowPytan_ZestawyPytan_ZestawPytanId",
                        column: x => x.ZestawPytanId,
                        principalTable: "ZestawyPytan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pytania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ZestawPytanId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pytania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pytania_ZestawyPytan_ZestawPytanId",
                        column: x => x.ZestawPytanId,
                        principalTable: "ZestawyPytan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Oddzialy",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracownikId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oddzialy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oddzialy_Pracownicy_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracownicyAdresy",
                columns: table => new
                {
                    AdresId = table.Column<int>(type: "int", nullable: false),
                    PracownikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracownicyAdresy", x => new { x.PracownikId, x.AdresId });
                    table.ForeignKey(
                        name: "FK_PracownicyAdresy_Adresy_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adresy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PracownicyAdresy_Pracownicy_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrzedmiotyPracownicy",
                columns: table => new
                {
                    PrzedmiotId = table.Column<byte>(type: "tinyint", nullable: false),
                    PracownikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrzedmiotyPracownicy", x => new { x.PracownikId, x.PrzedmiotId });
                    table.ForeignKey(
                        name: "FK_PrzedmiotyPracownicy_Pracownicy_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrzedmiotyPracownicy_Przedmioty_PrzedmiotId",
                        column: x => x.PrzedmiotId,
                        principalTable: "Przedmioty",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Uczniowie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WychowawcaId = table.Column<int>(type: "int", nullable: false),
                    OddzialId = table.Column<byte>(type: "tinyint", nullable: false),
                    AdresId = table.Column<int>(type: "int", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    Imie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DrugieImie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataUrodzenia = table.Column<DateTime>(type: "date", maxLength: 10, nullable: true),
                    MiejsceUrodzenia = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Pesel = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uczniowie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uczniowie_Adresy_AdresId",
                        column: x => x.AdresId,
                        principalTable: "Adresy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Uczniowie_Oddzialy_OddzialId",
                        column: x => x.OddzialId,
                        principalTable: "Oddzialy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Uczniowie_Pracownicy_WychowawcaId",
                        column: x => x.WychowawcaId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Diagnozy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RokSzkolny = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    UczenId = table.Column<int>(type: "int", nullable: false),
                    PracownikId = table.Column<int>(type: "int", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnozy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnozy_Pracownicy_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diagnozy_Uczniowie_UczenId",
                        column: x => x.UczenId,
                        principalTable: "Uczniowie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Oceny",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WystawionaOcena = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    UczenId = table.Column<int>(type: "int", nullable: false),
                    PrzedmiotId = table.Column<byte>(type: "tinyint", nullable: false),
                    PracownikId = table.Column<int>(type: "int", nullable: false),
                    DataWystawienia = table.Column<DateTime>(type: "datetime", nullable: false, computedColumnSql: "getdate()"),
                    PoprzedniaOcena = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true),
                    DataPoprawienia = table.Column<DateTime>(type: "datetime", nullable: true),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oceny", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oceny_Pracownicy_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Oceny_Przedmioty_PrzedmiotId",
                        column: x => x.PrzedmiotId,
                        principalTable: "Przedmioty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Oceny_Uczniowie_UczenId",
                        column: x => x.UczenId,
                        principalTable: "Uczniowie",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wyniki",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OcenaZestawuPytanId = table.Column<int>(type: "int", nullable: false),
                    PoziomOceny = table.Column<byte>(type: "tinyint", nullable: false),
                    DiagnozaId = table.Column<int>(type: "int", nullable: false),
                    Notatki = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    DataCzasWpisu = table.Column<DateTime>(type: "datetime", nullable: false, computedColumnSql: "getdate()"),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wyniki", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wyniki_Diagnozy_DiagnozaId",
                        column: x => x.DiagnozaId,
                        principalTable: "Diagnozy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wyniki_OcenyZestawowPytan_OcenaZestawuPytanId",
                        column: x => x.OcenaZestawuPytanId,
                        principalTable: "OcenyZestawowPytan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diagnozy_PracownikId",
                table: "Diagnozy",
                column: "PracownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnozy_UczenId",
                table: "Diagnozy",
                column: "UczenId");

            migrationBuilder.CreateIndex(
                name: "IX_KartyPracy_ZestawPytanId",
                table: "KartyPracy",
                column: "ZestawPytanId");

            migrationBuilder.CreateIndex(
                name: "IX_Migawki_UzytkownikId",
                table: "Migawki",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Oceny_PracownikId",
                table: "Oceny",
                column: "PracownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Oceny_PrzedmiotId",
                table: "Oceny",
                column: "PrzedmiotId");

            migrationBuilder.CreateIndex(
                name: "IX_Oceny_UczenId",
                table: "Oceny",
                column: "UczenId");

            migrationBuilder.CreateIndex(
                name: "IX_OcenyZestawowPytan_ZestawPytanId",
                table: "OcenyZestawowPytan",
                column: "ZestawPytanId");

            migrationBuilder.CreateIndex(
                name: "IX_Oddzialy_PracownikId",
                table: "Oddzialy",
                column: "PracownikId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pracownicy_EtatId",
                table: "Pracownicy",
                column: "EtatId");

            migrationBuilder.CreateIndex(
                name: "IX_Pracownicy_StanowiskoId",
                table: "Pracownicy",
                column: "StanowiskoId");

            migrationBuilder.CreateIndex(
                name: "IX_PracownicyAdresy_AdresId",
                table: "PracownicyAdresy",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_PrzedmiotyPracownicy_PrzedmiotId",
                table: "PrzedmiotyPracownicy",
                column: "PrzedmiotId");

            migrationBuilder.CreateIndex(
                name: "IX_Pytania_ZestawPytanId",
                table: "Pytania",
                column: "ZestawPytanId");

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_AdresId",
                table: "Uczniowie",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_OddzialId",
                table: "Uczniowie",
                column: "OddzialId");

            migrationBuilder.CreateIndex(
                name: "IX_Uczniowie_WychowawcaId",
                table: "Uczniowie",
                column: "WychowawcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_RolaId",
                table: "Uzytkownicy",
                column: "RolaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wyniki_DiagnozaId",
                table: "Wyniki",
                column: "DiagnozaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wyniki_OcenaZestawuPytanId",
                table: "Wyniki",
                column: "OcenaZestawuPytanId");

            migrationBuilder.CreateIndex(
                name: "IX_ZestawyPytan_ObszarZestawuPytanId",
                table: "ZestawyPytan",
                column: "ObszarZestawuPytanId");

            migrationBuilder.CreateIndex(
                name: "IX_ZestawyPytan_SkalaTrudnosciId",
                table: "ZestawyPytan",
                column: "SkalaTrudnosciId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KartyPracy");

            migrationBuilder.DropTable(
                name: "Migawki");

            migrationBuilder.DropTable(
                name: "Oceny");

            migrationBuilder.DropTable(
                name: "PracownicyAdresy");

            migrationBuilder.DropTable(
                name: "PrzedmiotyPracownicy");

            migrationBuilder.DropTable(
                name: "Pytania");

            migrationBuilder.DropTable(
                name: "Wyniki");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "Przedmioty");

            migrationBuilder.DropTable(
                name: "Diagnozy");

            migrationBuilder.DropTable(
                name: "OcenyZestawowPytan");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Uczniowie");

            migrationBuilder.DropTable(
                name: "ZestawyPytan");

            migrationBuilder.DropTable(
                name: "Adresy");

            migrationBuilder.DropTable(
                name: "Oddzialy");

            migrationBuilder.DropTable(
                name: "ObszaryZestawowPytan");

            migrationBuilder.DropTable(
                name: "SkaleTrudnosci");

            migrationBuilder.DropTable(
                name: "Pracownicy");

            migrationBuilder.DropTable(
                name: "Etaty");

            migrationBuilder.DropTable(
                name: "Stanowiska");
        }
    }
}
