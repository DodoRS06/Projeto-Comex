using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M12ProcessoExpImp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processo-Exportador-Importador",
                columns: table => new
                {
                    ProcessoId = table.Column<int>(type: "int", nullable: false),
                    ExpImpId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processo-Exportador-Importador", x => new { x.ProcessoId, x.ExpImpId });
                    table.ForeignKey(
                        name: "FK_Processo-Exportador-Importador_ExpImp_ExpImpId",
                        column: x => x.ExpImpId,
                        principalTable: "ExpImp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processo-Exportador-Importador_Processo_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processo-Exportador-Importador_ExpImpId",
                table: "Processo-Exportador-Importador",
                column: "ExpImpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processo-Exportador-Importador");
        }
    }
}
