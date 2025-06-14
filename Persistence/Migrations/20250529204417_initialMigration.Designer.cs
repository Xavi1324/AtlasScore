﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.DBContext;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(AtlasScoreDbContext))]
    [Migration("20250529204417_initialMigration")]
    partial class initialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Persistence.Entities.IndicadorPorPais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Año")
                        .HasColumnType("int");

                    b.Property<int>("MacroindicadorId")
                        .HasColumnType("int");

                    b.Property<int>("PaisId")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MacroindicadorId");

                    b.HasIndex("PaisId", "MacroindicadorId", "Año")
                        .IsUnique();

                    b.ToTable("IndicadoresPorPais", (string)null);
                });

            modelBuilder.Entity("Persistence.Entities.Macroindicador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("EsMejorMasAlto")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Peso")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Id");

                    b.ToTable("Macroindicadores", (string)null);
                });

            modelBuilder.Entity("Persistence.Entities.Pais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodigoIso")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Paises", (string)null);
                });

            modelBuilder.Entity("Persistence.Entities.SimulacionMacroindicador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MacroindicadorId")
                        .HasColumnType("int");

                    b.Property<decimal>("PesoSimulacion")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Id");

                    b.HasIndex("MacroindicadorId");

                    b.ToTable("SimulacionesMacroindicadores", (string)null);
                });

            modelBuilder.Entity("Persistence.Entities.TasaRetorno", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("TasaMaxima")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("TasaMinima")
                        .HasColumnType("decimal(5,2)");

                    b.HasKey("Id");

                    b.ToTable("TasasRetorno", (string)null);
                });

            modelBuilder.Entity("Persistence.Entities.IndicadorPorPais", b =>
                {
                    b.HasOne("Persistence.Entities.Macroindicador", "Macroindicador")
                        .WithMany("Indicadores")
                        .HasForeignKey("MacroindicadorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Persistence.Entities.Pais", "Pais")
                        .WithMany("Indicadores")
                        .HasForeignKey("PaisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Macroindicador");

                    b.Navigation("Pais");
                });

            modelBuilder.Entity("Persistence.Entities.SimulacionMacroindicador", b =>
                {
                    b.HasOne("Persistence.Entities.Macroindicador", "Macroindicador")
                        .WithMany("Simulaciones")
                        .HasForeignKey("MacroindicadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Macroindicador");
                });

            modelBuilder.Entity("Persistence.Entities.Macroindicador", b =>
                {
                    b.Navigation("Indicadores");

                    b.Navigation("Simulaciones");
                });

            modelBuilder.Entity("Persistence.Entities.Pais", b =>
                {
                    b.Navigation("Indicadores");
                });
#pragma warning restore 612, 618
        }
    }
}
