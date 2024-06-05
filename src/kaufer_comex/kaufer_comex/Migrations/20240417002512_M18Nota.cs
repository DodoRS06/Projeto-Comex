using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M18Nota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroNf = table.Column<int>(type: "int", nullable: false),
                    Emissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BaseNota = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorFob = table.Column<float>(type: "real", nullable: false),
                    ValorFrete = table.Column<float>(type: "real", nullable: false),
                    ValorSeguro = table.Column<float>(type: "real", nullable: false),
                    ValorCif = table.Column<float>(type: "real", nullable: false),
                    PesoLiq = table.Column<float>(type: "real", nullable: false),
                    PesoBruto = table.Column<float>(type: "real", nullable: false),
                    TaxaCambial = table.Column<float>(type: "real", nullable: false),
                    CertificadoQualidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbarqueRodoviarioId = table.Column<int>(type: "int", nullable: false),
                    VeiculoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notas_EmbarqueRodoviario_EmbarqueRodoviarioId",
                        column: x => x.EmbarqueRodoviarioId,
                        principalTable: "EmbarqueRodoviario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notas_Veiculo_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_EmbarqueRodoviarioId",
                table: "Notas",
                column: "EmbarqueRodoviarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_VeiculoId",
                table: "Notas",
                column: "VeiculoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notas");
        }
    }
}
