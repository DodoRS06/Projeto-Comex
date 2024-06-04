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

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.RenameColumn(
                name: "Espessura",
                table: "Itens",
                newName: "Expressura");


        }
    }
}
