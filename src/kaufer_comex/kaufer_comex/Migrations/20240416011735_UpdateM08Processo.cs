using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateM08Processo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processo_AgenteDeCargas_AgenteDeCargaId",
                table: "Processo");

            migrationBuilder.DropIndex(
                name: "IX_Processo_AgenteDeCargaId",
                table: "Processo");

            migrationBuilder.RenameColumn(
                name: "AgenteDeCargaId",
                table: "Processo",
                newName: "UsuarioResponsavel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioResponsavel",
                table: "Processo",
                newName: "AgenteDeCargaId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_AgenteDeCargaId",
                table: "Processo",
                column: "AgenteDeCargaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_AgenteDeCargas_AgenteDeCargaId",
                table: "Processo",
                column: "AgenteDeCargaId",
                principalTable: "AgenteDeCargas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
