using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class Update2M08Processo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioResponsavel",
                table: "Processo",
                newName: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Processo_UsuarioId",
                table: "Processo",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processo_Usuarios_UsuarioId",
                table: "Processo",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processo_Usuarios_UsuarioId",
                table: "Processo");

            migrationBuilder.DropIndex(
                name: "IX_Processo_UsuarioId",
                table: "Processo");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Processo",
                newName: "UsuarioResponsavel");
        }
    }
}
