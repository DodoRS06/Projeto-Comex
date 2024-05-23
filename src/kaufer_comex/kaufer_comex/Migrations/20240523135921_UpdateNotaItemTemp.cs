using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateNotaItemTemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PesoBruto",
                table: "NotaItemTemps",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PesoLiquido",
                table: "NotaItemTemps",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PesoBruto",
                table: "NotaItemTemps");

            migrationBuilder.DropColumn(
                name: "PesoLiquido",
                table: "NotaItemTemps");
        }
    }
}
