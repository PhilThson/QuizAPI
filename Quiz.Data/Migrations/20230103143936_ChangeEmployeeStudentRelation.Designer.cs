﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quiz.Data.DataAccess;

#nullable disable

namespace Quiz.Data.Migrations
{
    [DbContext(typeof(QuizDbContext))]
    [Migration("20230103143936_ChangeEmployeeStudentRelation")]
    partial class ChangeEmployeeStudentRelation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Quiz.Data.Models.Adres", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("KodPocztowy")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Miejscowosc")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("NumerDomu")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("NumerMieszkania")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Panstwo")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Ulica")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Adresy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Diagnoza", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<int>("PracownikId")
                        .HasColumnType("int");

                    b.Property<string>("RokSzkolny")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<int>("UczenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PracownikId");

                    b.HasIndex("UczenId");

                    b.ToTable("Diagnozy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Etat", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("Etaty");
                });

            modelBuilder.Entity("Quiz.Data.Models.KartaPracy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("RodzajZawartosci")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<long>("Rozmiar")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Zawartosc")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("ZestawPytanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZestawPytanId");

                    b.ToTable("KartyPracy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Migawka", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataZmiany")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasComputedColumnSql("getdate()");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Szczegoly")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UzytkownikId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UzytkownikId");

                    b.ToTable("Migawki");
                });

            modelBuilder.Entity("Quiz.Data.Models.ObszarZestawuPytan", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("NazwaRozszerzona")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("ObszaryZestawowPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Ocena", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DataPoprawienia")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DataWystawienia")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasComputedColumnSql("getdate()");

                    b.Property<decimal?>("PoprzedniaOcena")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.Property<int>("PracownikId")
                        .HasColumnType("int");

                    b.Property<byte>("PrzedmiotId")
                        .HasColumnType("tinyint");

                    b.Property<int>("UczenId")
                        .HasColumnType("int");

                    b.Property<decimal>("WystawionaOcena")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.HasKey("Id");

                    b.HasIndex("PracownikId");

                    b.HasIndex("PrzedmiotId");

                    b.HasIndex("UczenId");

                    b.ToTable("Oceny");
                });

            modelBuilder.Entity("Quiz.Data.Models.OcenaZestawuPytan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("OpisOceny")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("ZestawPytanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZestawPytanId");

                    b.ToTable("OcenyZestawowPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Oddzial", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("PracownikId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PracownikId")
                        .IsUnique();

                    b.ToTable("Oddzialy");
                });

            modelBuilder.Entity("Quiz.Data.Models.PracownicyAdresy", b =>
                {
                    b.Property<int>("PracownikId")
                        .HasColumnType("int");

                    b.Property<int>("AdresId")
                        .HasColumnType("int");

                    b.HasKey("PracownikId", "AdresId");

                    b.HasIndex("AdresId");

                    b.ToTable("PracownicyAdresy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Pracownik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DataKoncaZatrudnienia")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DataUrodzenia")
                        .HasMaxLength(10)
                        .HasColumnType("date");

                    b.Property<DateTime>("DataZatrudnienia")
                        .HasColumnType("date");

                    b.Property<int?>("DniUrlopu")
                        .HasColumnType("int");

                    b.Property<string>("DrugieImie")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("EtatId")
                        .IsRequired()
                        .HasColumnType("tinyint");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MiejsceUrodzenia")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<double?>("Nadgodziny")
                        .HasColumnType("float");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NrTelefonu")
                        .HasColumnType("varchar(11)");

                    b.Property<decimal>("Pensja")
                        .HasPrecision(7, 2)
                        .HasColumnType("money");

                    b.Property<string>("Pesel")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<byte?>("StanowiskoId")
                        .IsRequired()
                        .HasColumnType("tinyint");

                    b.Property<double?>("WymiarGodzinowy")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("EtatId");

                    b.HasIndex("StanowiskoId");

                    b.ToTable("Pracownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Przedmiot", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("SkroconaNazwa")
                        .HasColumnType("char(3)");

                    b.HasKey("Id");

                    b.ToTable("Przedmioty");
                });

            modelBuilder.Entity("Quiz.Data.Models.PrzedmiotyPracownicy", b =>
                {
                    b.Property<int>("PracownikId")
                        .HasColumnType("int");

                    b.Property<byte>("PrzedmiotId")
                        .HasColumnType("tinyint");

                    b.HasKey("PracownikId", "PrzedmiotId");

                    b.HasIndex("PrzedmiotId");

                    b.ToTable("PrzedmiotyPracownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Pytanie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Tresc")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("ZestawPytanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZestawPytanId");

                    b.ToTable("Pytania");
                });

            modelBuilder.Entity("Quiz.Data.Models.Rola", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Quiz.Data.Models.SkalaTrudnosci", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("SkaleTrudnosci");
                });

            modelBuilder.Entity("Quiz.Data.Models.Stanowisko", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Opis")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("Stanowiska");
                });

            modelBuilder.Entity("Quiz.Data.Models.Uczen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AdresId")
                        .HasColumnType("int");

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DataUrodzenia")
                        .HasMaxLength(10)
                        .HasColumnType("date");

                    b.Property<string>("DrugieImie")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MiejsceUrodzenia")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("OddzialId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Pesel")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("AdresId");

                    b.HasIndex("OddzialId");

                    b.ToTable("Uczniowie");
                });

            modelBuilder.Entity("Quiz.Data.Models.Uzytkownik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HashHasla")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte>("RolaId")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("RolaId");

                    b.ToTable("Uzytkownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Wynik", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataCzasWpisu")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime")
                        .HasComputedColumnSql("getdate()");

                    b.Property<int>("DiagnozaId")
                        .HasColumnType("int");

                    b.Property<string>("Notatki")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("OcenaZestawuPytanId")
                        .HasColumnType("int");

                    b.Property<byte>("PoziomOceny")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("DiagnozaId");

                    b.HasIndex("OcenaZestawuPytanId");

                    b.ToTable("Wyniki");
                });

            modelBuilder.Entity("Quiz.Data.Models.ZestawPytan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CzyAktywny")
                        .HasColumnType("bit");

                    b.Property<byte>("ObszarZestawuPytanId")
                        .HasColumnType("tinyint");

                    b.Property<string>("OpisUmiejetnosci")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<byte>("SkalaTrudnosciId")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("ObszarZestawuPytanId");

                    b.HasIndex("SkalaTrudnosciId");

                    b.ToTable("ZestawyPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Diagnoza", b =>
                {
                    b.HasOne("Quiz.Data.Models.Pracownik", "Pracownik")
                        .WithMany("PracownikDiagnozy")
                        .HasForeignKey("PracownikId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.Uczen", "Uczen")
                        .WithMany("UczenDiagnozy")
                        .HasForeignKey("UczenId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Pracownik");

                    b.Navigation("Uczen");
                });

            modelBuilder.Entity("Quiz.Data.Models.KartaPracy", b =>
                {
                    b.HasOne("Quiz.Data.Models.ZestawPytan", "ZestawPytan")
                        .WithMany("ZestawPytanKartyPracy")
                        .HasForeignKey("ZestawPytanId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ZestawPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Migawka", b =>
                {
                    b.HasOne("Quiz.Data.Models.Uzytkownik", "Uzytkownik")
                        .WithMany("UzytkownikMigawki")
                        .HasForeignKey("UzytkownikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Uzytkownik");
                });

            modelBuilder.Entity("Quiz.Data.Models.Ocena", b =>
                {
                    b.HasOne("Quiz.Data.Models.Pracownik", "Pracownik")
                        .WithMany("PracownikOceny")
                        .HasForeignKey("PracownikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.Przedmiot", "Przedmiot")
                        .WithMany()
                        .HasForeignKey("PrzedmiotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.Uczen", "Uczen")
                        .WithMany("UczenOceny")
                        .HasForeignKey("UczenId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Pracownik");

                    b.Navigation("Przedmiot");

                    b.Navigation("Uczen");
                });

            modelBuilder.Entity("Quiz.Data.Models.OcenaZestawuPytan", b =>
                {
                    b.HasOne("Quiz.Data.Models.ZestawPytan", "ZestawPytan")
                        .WithMany("ZestawPytanOceny")
                        .HasForeignKey("ZestawPytanId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ZestawPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Oddzial", b =>
                {
                    b.HasOne("Quiz.Data.Models.Pracownik", "Pracownik")
                        .WithOne("PracownikOddzial")
                        .HasForeignKey("Quiz.Data.Models.Oddzial", "PracownikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pracownik");
                });

            modelBuilder.Entity("Quiz.Data.Models.PracownicyAdresy", b =>
                {
                    b.HasOne("Quiz.Data.Models.Adres", "Adres")
                        .WithMany("AdresPracownicyAdresy")
                        .HasForeignKey("AdresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.Pracownik", "Pracownik")
                        .WithMany("PracownikPracownicyAdresy")
                        .HasForeignKey("PracownikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adres");

                    b.Navigation("Pracownik");
                });

            modelBuilder.Entity("Quiz.Data.Models.Pracownik", b =>
                {
                    b.HasOne("Quiz.Data.Models.Etat", "Etat")
                        .WithMany("EtatPracownicy")
                        .HasForeignKey("EtatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.Stanowisko", "Stanowisko")
                        .WithMany("StanowiskoPracownicy")
                        .HasForeignKey("StanowiskoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etat");

                    b.Navigation("Stanowisko");
                });

            modelBuilder.Entity("Quiz.Data.Models.PrzedmiotyPracownicy", b =>
                {
                    b.HasOne("Quiz.Data.Models.Pracownik", "Pracownik")
                        .WithMany("PracownikPrzedmiotyPracownicy")
                        .HasForeignKey("PracownikId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.Przedmiot", "Przedmiot")
                        .WithMany("PrzedmiotPrzedmiotyPracownicy")
                        .HasForeignKey("PrzedmiotId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Pracownik");

                    b.Navigation("Przedmiot");
                });

            modelBuilder.Entity("Quiz.Data.Models.Pytanie", b =>
                {
                    b.HasOne("Quiz.Data.Models.ZestawPytan", "ZestawPytan")
                        .WithMany("ZestawPytanPytania")
                        .HasForeignKey("ZestawPytanId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ZestawPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Uczen", b =>
                {
                    b.HasOne("Quiz.Data.Models.Adres", null)
                        .WithMany("AdresUczniowie")
                        .HasForeignKey("AdresId");

                    b.HasOne("Quiz.Data.Models.Oddzial", "Oddzial")
                        .WithMany("OddzialUczniowie")
                        .HasForeignKey("OddzialId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Oddzial");
                });

            modelBuilder.Entity("Quiz.Data.Models.Uzytkownik", b =>
                {
                    b.HasOne("Quiz.Data.Models.Rola", "Rola")
                        .WithMany("RolaUzytkownicy")
                        .HasForeignKey("RolaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rola");
                });

            modelBuilder.Entity("Quiz.Data.Models.Wynik", b =>
                {
                    b.HasOne("Quiz.Data.Models.Diagnoza", "Diagnoza")
                        .WithMany("DiagnozaWyniki")
                        .HasForeignKey("DiagnozaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.OcenaZestawuPytan", "OcenaZestawuPytan")
                        .WithMany("OcenaZestawuPytanWyniki")
                        .HasForeignKey("OcenaZestawuPytanId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Diagnoza");

                    b.Navigation("OcenaZestawuPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.ZestawPytan", b =>
                {
                    b.HasOne("Quiz.Data.Models.ObszarZestawuPytan", "ObszarZestawuPytan")
                        .WithMany("ObszarZestawuPytanZestawyPytan")
                        .HasForeignKey("ObszarZestawuPytanId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Quiz.Data.Models.SkalaTrudnosci", "SkalaTrudnosci")
                        .WithMany("SkalaTrudnosciZestawyPytan")
                        .HasForeignKey("SkalaTrudnosciId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ObszarZestawuPytan");

                    b.Navigation("SkalaTrudnosci");
                });

            modelBuilder.Entity("Quiz.Data.Models.Adres", b =>
                {
                    b.Navigation("AdresPracownicyAdresy");

                    b.Navigation("AdresUczniowie");
                });

            modelBuilder.Entity("Quiz.Data.Models.Diagnoza", b =>
                {
                    b.Navigation("DiagnozaWyniki");
                });

            modelBuilder.Entity("Quiz.Data.Models.Etat", b =>
                {
                    b.Navigation("EtatPracownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.ObszarZestawuPytan", b =>
                {
                    b.Navigation("ObszarZestawuPytanZestawyPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.OcenaZestawuPytan", b =>
                {
                    b.Navigation("OcenaZestawuPytanWyniki");
                });

            modelBuilder.Entity("Quiz.Data.Models.Oddzial", b =>
                {
                    b.Navigation("OddzialUczniowie");
                });

            modelBuilder.Entity("Quiz.Data.Models.Pracownik", b =>
                {
                    b.Navigation("PracownikDiagnozy");

                    b.Navigation("PracownikOceny");

                    b.Navigation("PracownikOddzial")
                        .IsRequired();

                    b.Navigation("PracownikPracownicyAdresy");

                    b.Navigation("PracownikPrzedmiotyPracownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Przedmiot", b =>
                {
                    b.Navigation("PrzedmiotPrzedmiotyPracownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Rola", b =>
                {
                    b.Navigation("RolaUzytkownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.SkalaTrudnosci", b =>
                {
                    b.Navigation("SkalaTrudnosciZestawyPytan");
                });

            modelBuilder.Entity("Quiz.Data.Models.Stanowisko", b =>
                {
                    b.Navigation("StanowiskoPracownicy");
                });

            modelBuilder.Entity("Quiz.Data.Models.Uczen", b =>
                {
                    b.Navigation("UczenDiagnozy");

                    b.Navigation("UczenOceny");
                });

            modelBuilder.Entity("Quiz.Data.Models.Uzytkownik", b =>
                {
                    b.Navigation("UzytkownikMigawki");
                });

            modelBuilder.Entity("Quiz.Data.Models.ZestawPytan", b =>
                {
                    b.Navigation("ZestawPytanKartyPracy");

                    b.Navigation("ZestawPytanOceny");

                    b.Navigation("ZestawPytanPytania");
                });
#pragma warning restore 612, 618
        }
    }
}
