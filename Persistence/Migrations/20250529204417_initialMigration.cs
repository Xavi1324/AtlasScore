using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Macroindicadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EsMejorMasAlto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Macroindicadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoIso = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TasasRetorno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TasaMinima = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TasaMaxima = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasasRetorno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimulacionesMacroindicadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MacroindicadorId = table.Column<int>(type: "int", nullable: false),
                    PesoSimulacion = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulacionesMacroindicadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimulacionesMacroindicadores_Macroindicadores_MacroindicadorId",
                        column: x => x.MacroindicadorId,
                        principalTable: "Macroindicadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicadoresPorPais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaisId = table.Column<int>(type: "int", nullable: false),
                    MacroindicadorId = table.Column<int>(type: "int", nullable: false),
                    Año = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresPorPais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicadoresPorPais_Macroindicadores_MacroindicadorId",
                        column: x => x.MacroindicadorId,
                        principalTable: "Macroindicadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndicadoresPorPais_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresPorPais_MacroindicadorId",
                table: "IndicadoresPorPais",
                column: "MacroindicadorId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresPorPais_PaisId_MacroindicadorId_Año",
                table: "IndicadoresPorPais",
                columns: new[] { "PaisId", "MacroindicadorId", "Año" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SimulacionesMacroindicadores_MacroindicadorId",
                table: "SimulacionesMacroindicadores",
                column: "MacroindicadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicadoresPorPais");

            migrationBuilder.DropTable(
                name: "SimulacionesMacroindicadores");

            migrationBuilder.DropTable(
                name: "TasasRetorno");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "Macroindicadores");
        }
    }
}
