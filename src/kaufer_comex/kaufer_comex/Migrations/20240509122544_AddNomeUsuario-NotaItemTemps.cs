using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class AddNomeUsuarioNotaItemTemps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "NomeUsuario",
                table: "NotaItemTemps",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
     
            migrationBuilder.DropColumn(
                name: "NomeUsuario",
                table: "NotaItemTemps");

        

         
        }
    }
}
