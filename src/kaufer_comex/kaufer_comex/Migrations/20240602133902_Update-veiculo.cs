using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class Updateveiculo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessoId",
                table: "Veiculo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_ProcessoId",
                table: "Veiculo",
                column: "ProcessoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculo_Processo_ProcessoId",
                table: "Veiculo",
                column: "ProcessoId",
                principalTable: "Processo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculo_Processo_ProcessoId",
                table: "Veiculo");

            migrationBuilder.DropIndex(
                name: "IX_Veiculo_ProcessoId",
                table: "Veiculo");

            migrationBuilder.DropColumn(
                name: "ProcessoId",
                table: "Veiculo");
        }
    }
}
