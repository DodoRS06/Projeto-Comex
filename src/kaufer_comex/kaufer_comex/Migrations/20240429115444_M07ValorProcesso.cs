using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M07ValorProcesso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValorProcessoProcessos");

            migrationBuilder.AddColumn<int>(
                name: "ProcessoId",
                table: "ValorProcessos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ValorProcessos_ProcessoId",
                table: "ValorProcessos",
                column: "ProcessoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ValorProcessos_Processo_ProcessoId",
                table: "ValorProcessos",
                column: "ProcessoId",
                principalTable: "Processo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValorProcessos_Processo_ProcessoId",
                table: "ValorProcessos");

            migrationBuilder.DropIndex(
                name: "IX_ValorProcessos_ProcessoId",
                table: "ValorProcessos");

            migrationBuilder.DropColumn(
                name: "ProcessoId",
                table: "ValorProcessos");

            migrationBuilder.CreateTable(
                name: "ValorProcessoProcessos",
                columns: table => new
                {
                    ValorProcessoId = table.Column<int>(type: "int", nullable: false),
                    ProcessoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorProcessoProcessos", x => new { x.ValorProcessoId, x.ProcessoId });
                    table.ForeignKey(
                        name: "FK_ValorProcessoProcessos_Processo_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValorProcessoProcessos_ValorProcessos_ValorProcessoId",
                        column: x => x.ValorProcessoId,
                        principalTable: "ValorProcessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValorProcessoProcessos_ProcessoId",
                table: "ValorProcessoProcessos",
                column: "ProcessoId");
        }
    }
}
