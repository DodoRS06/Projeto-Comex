using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateM13Documento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Processo_ProcessoId",
                table: "Documentos");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessoId",
                table: "Documentos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Processo_ProcessoId",
                table: "Documentos",
                column: "ProcessoId",
                principalTable: "Processo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Processo_ProcessoId",
                table: "Documentos");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessoId",
                table: "Documentos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Processo_ProcessoId",
                table: "Documentos",
                column: "ProcessoId",
                principalTable: "Processo",
                principalColumn: "Id");
        }
    }
}
