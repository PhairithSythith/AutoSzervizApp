using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoSzervizApp.Migrations
{
    /// <inheritdoc />
    public partial class SzervizTipusJavaslatok : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SzervizTipusJavaslatok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nev = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Leiras = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    JavasoltAlapar = table.Column<decimal>(type: "TEXT", nullable: false),
                    JavasloFelhasznaloId = table.Column<string>(type: "TEXT", nullable: false),
                    Allapot = table.Column<int>(type: "INTEGER", nullable: false),
                    Letrehozva = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Elbiralva = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ElbiraloFelhasznaloId = table.Column<string>(type: "TEXT", nullable: true),
                    AdminMegjegyzes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SzervizTipusJavaslatok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SzervizTipusJavaslatok_AspNetUsers_ElbiraloFelhasznaloId",
                        column: x => x.ElbiraloFelhasznaloId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SzervizTipusJavaslatok_AspNetUsers_JavasloFelhasznaloId",
                        column: x => x.JavasloFelhasznaloId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SzervizTipusJavaslatok_ElbiraloFelhasznaloId",
                table: "SzervizTipusJavaslatok",
                column: "ElbiraloFelhasznaloId");

            migrationBuilder.CreateIndex(
                name: "IX_SzervizTipusJavaslatok_JavasloFelhasznaloId",
                table: "SzervizTipusJavaslatok",
                column: "JavasloFelhasznaloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SzervizTipusJavaslatok");
        }
    }
}
