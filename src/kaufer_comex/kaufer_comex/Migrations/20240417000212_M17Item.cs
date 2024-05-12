using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M17Item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoProduto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescricaoProduto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Familia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Largura = table.Column<float>(type: "real", nullable: false),
                    Comprimento = table.Column<float>(type: "real", nullable: false),
                    Expressura = table.Column<float>(type: "real", nullable: false),
                    AreaM2 = table.Column<float>(type: "real", nullable: false),
					DiametroAltura = table.Column<float>(type: "real", nullable: false),
					DiametroComprimento = table.Column<float>(type: "real", nullable: false),
					LarguraAparente = table.Column<float>(type: "real", nullable: false),
                    VolumeM2 = table.Column<float>(type: "real", nullable: false),
                    PesoLiquido = table.Column<float>(type: "real", nullable: false),
                    PesoBruto = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itens");
        }
    }
}
