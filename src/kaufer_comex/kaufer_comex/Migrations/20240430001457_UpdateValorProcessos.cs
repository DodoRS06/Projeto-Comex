using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateValorProcessos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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


        }
    }
}
