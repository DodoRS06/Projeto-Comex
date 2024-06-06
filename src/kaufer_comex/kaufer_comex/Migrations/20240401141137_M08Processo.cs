using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M08Processo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodProcessoExportacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExportadorId = table.Column<int>(type: "int", nullable: false),
                    ImportadorId = table.Column<int>(type: "int", nullable: false),
                    Modal = table.Column<int>(type: "int", nullable: false),
                    Incoterm = table.Column<int>(type: "int", nullable: false),
                    DestinoId = table.Column<int>(type: "int", nullable: false),
                    AgenteDeCargaId = table.Column<int>(type: "int", nullable: false),
                    FronteiraId = table.Column<int>(type: "int", nullable: false),
                    DespachanteId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Proforma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataInicioProcesso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevisaoProducao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevisaoPagamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevisaoColeta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevisaoCruze = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevisaoEntrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PedidosRelacionados = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processo_AgenteDeCargas_AgenteDeCargaId",
                        column: x => x.AgenteDeCargaId,
                        principalTable: "AgenteDeCargas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Processo_Despachantes_DespachanteId",
                        column: x => x.DespachanteId,
                        principalTable: "Despachantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Processo_Destino_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Destino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Processo_Fronteiras_FronteiraId",
                        column: x => x.FronteiraId,
                        principalTable: "Fronteiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Processo_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Processo_Vendedores_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processo_AgenteDeCargaId",
                table: "Processo",
                column: "AgenteDeCargaId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_DespachanteId",
                table: "Processo",
                column: "DespachanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_DestinoId",
                table: "Processo",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_FronteiraId",
                table: "Processo",
                column: "FronteiraId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_StatusId",
                table: "Processo",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_VendedorId",
                table: "Processo",
                column: "VendedorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processo");
        }
    }
}
