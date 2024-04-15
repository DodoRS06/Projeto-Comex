using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M15EmbarqueRodoviario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmbarqueRodoviario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Transportadora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEmbarque = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransitTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChegadaDestino = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AgenteDeCargaId = table.Column<int>(type: "int", nullable: false),
                    IdProcesso = table.Column<int>(type: "int", nullable: false),
                    ProcessoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmbarqueRodoviario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmbarqueRodoviario_AgenteDeCargas_AgenteDeCargaId",
                        column: x => x.AgenteDeCargaId,
                        principalTable: "AgenteDeCargas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmbarqueRodoviario_Processo_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmbarqueRodoviario_AgenteDeCargaId",
                table: "EmbarqueRodoviario",
                column: "AgenteDeCargaId");

            migrationBuilder.CreateIndex(
                name: "IX_EmbarqueRodoviario_ProcessoId",
                table: "EmbarqueRodoviario",
                column: "ProcessoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmbarqueRodoviario");
        }
    }
}
