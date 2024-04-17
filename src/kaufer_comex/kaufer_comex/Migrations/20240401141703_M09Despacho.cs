using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M09Despacho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Despacho",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroDue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExportacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConhecimentoEmbarque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataConhecimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataAverbacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodPais = table.Column<int>(type: "int", nullable: false),
                    Parametrizacao = table.Column<int>(type: "int", nullable: false),
                    ProcessoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despacho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Despacho_Processo_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Despacho_ProcessoId",
                table: "Despacho",
                column: "ProcessoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Despacho");
        }
    }
}
