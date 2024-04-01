using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class M07ValorProcesso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValorProcessos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorExw = table.Column<float>(type: "real", nullable: false),
                    ValorFobFca = table.Column<float>(type: "real", nullable: false),
                    FreteInternacional = table.Column<float>(type: "real", nullable: false),
                    SeguroInternaciona = table.Column<float>(type: "real", nullable: false),
                    ValorTotalCif = table.Column<float>(type: "real", nullable: false),
                    Moeda = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorProcessos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValorProcessos");
        }
    }
}
