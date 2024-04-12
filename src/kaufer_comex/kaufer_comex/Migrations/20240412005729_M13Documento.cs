using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M13Documento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificadoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificadoSeguro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEnvioOrigem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEnvioSeguro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrackinCourier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Processo_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ProcessoId",
                table: "Documentos",
                column: "ProcessoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documentos");
        }
    }
}
