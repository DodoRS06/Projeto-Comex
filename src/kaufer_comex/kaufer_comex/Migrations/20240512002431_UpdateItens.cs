using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateItens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expressura",
                table: "Itens",
                newName: "Espessura");

            migrationBuilder.RenameColumn(
                name: "Diametro",
                table: "Itens",
                newName: "DiametroComprimento");

            migrationBuilder.AddColumn<float>(
                name: "DiametroAltura",
                table: "Itens",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiametroAltura",
                table: "Itens");

            migrationBuilder.RenameColumn(
                name: "Espessura",
                table: "Itens",
                newName: "Expressura");

            migrationBuilder.RenameColumn(
                name: "DiametroComprimento",
                table: "Itens",
                newName: "Diametro");
        }
    }
}
