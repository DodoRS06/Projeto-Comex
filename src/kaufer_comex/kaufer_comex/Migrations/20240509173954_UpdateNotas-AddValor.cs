using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateNotasAddValor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "NotaItens");

            migrationBuilder.AddColumn<double>(
                name: "QuantidadeTotal",
                table: "Notas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorTotal",
                table: "Notas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeTotal",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "ValorTotal",
                table: "Notas");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "NotaItens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
