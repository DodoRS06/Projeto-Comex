using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M10DCE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DCE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CadastroDespesaId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<float>(type: "real", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCE_Processo_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CadastroDespesa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeDespesa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DCEId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CadastroDespesa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CadastroDespesa_DCE_DCEId",
                        column: x => x.DCEId,
                        principalTable: "DCE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FornecedorServico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoServico = table.Column<int>(type: "int", nullable: false),
                    DCEId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FornecedorServico_DCE_DCEId",
                        column: x => x.DCEId,
                        principalTable: "DCE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CadastroDespesa_DCEId",
                table: "CadastroDespesa",
                column: "DCEId");

            migrationBuilder.CreateIndex(
                name: "IX_DCE_ProcessoId",
                table: "DCE",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_FornecedorServico_DCEId",
                table: "FornecedorServico",
                column: "DCEId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CadastroDespesa");

            migrationBuilder.DropTable(
                name: "FornecedorServico");

            migrationBuilder.DropTable(
                name: "DCE");
        }
    }
}
