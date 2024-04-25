using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateM10DCE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CadastroDespesa_DCE_DCEId",
                table: "CadastroDespesa");

            migrationBuilder.DropForeignKey(
                name: "FK_FornecedorServico_DCE_DCEId",
                table: "FornecedorServico");

            migrationBuilder.DropIndex(
                name: "IX_FornecedorServico_DCEId",
                table: "FornecedorServico");

            migrationBuilder.DropIndex(
                name: "IX_CadastroDespesa_DCEId",
                table: "CadastroDespesa");

            migrationBuilder.DropColumn(
                name: "DCEId",
                table: "FornecedorServico");

            migrationBuilder.DropColumn(
                name: "DCEId",
                table: "CadastroDespesa");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "ExpImp",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TransitTime",
                table: "EmbarqueRodoviario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                table: "DCE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "FornecedorServicoId",
                table: "DCE",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Despesa-DCE",
                columns: table => new
                {
                    CadastroDespesaId = table.Column<int>(type: "int", nullable: false),
                    DCEId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesa-DCE", x => new { x.CadastroDespesaId, x.DCEId });
                    table.ForeignKey(
                        name: "FK_Despesa-DCE_CadastroDespesa_CadastroDespesaId",
                        column: x => x.CadastroDespesaId,
                        principalTable: "CadastroDespesa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Despesa-DCE_DCE_DCEId",
                        column: x => x.DCEId,
                        principalTable: "DCE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedor-DCE",
                columns: table => new
                {
                    FornecedorServicoId = table.Column<int>(type: "int", nullable: false),
                    DCEId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor-DCE", x => new { x.FornecedorServicoId, x.DCEId });
                    table.ForeignKey(
                        name: "FK_Fornecedor-DCE_DCE_DCEId",
                        column: x => x.DCEId,
                        principalTable: "DCE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fornecedor-DCE_FornecedorServico_FornecedorServicoId",
                        column: x => x.FornecedorServicoId,
                        principalTable: "FornecedorServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Despesa-DCE_DCEId",
                table: "Despesa-DCE",
                column: "DCEId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor-DCE_DCEId",
                table: "Fornecedor-DCE",
                column: "DCEId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Despesa-DCE");

            migrationBuilder.DropTable(
                name: "Fornecedor-DCE");

            migrationBuilder.DropColumn(
                name: "FornecedorServicoId",
                table: "DCE");

            migrationBuilder.AddColumn<int>(
                name: "DCEId",
                table: "FornecedorServico",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "ExpImp",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransitTime",
                table: "EmbarqueRodoviario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Observacao",
                table: "DCE",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DCEId",
                table: "CadastroDespesa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FornecedorServico_DCEId",
                table: "FornecedorServico",
                column: "DCEId");

            migrationBuilder.CreateIndex(
                name: "IX_CadastroDespesa_DCEId",
                table: "CadastroDespesa",
                column: "DCEId");

            migrationBuilder.AddForeignKey(
                name: "FK_CadastroDespesa_DCE_DCEId",
                table: "CadastroDespesa",
                column: "DCEId",
                principalTable: "DCE",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FornecedorServico_DCE_DCEId",
                table: "FornecedorServico",
                column: "DCEId",
                principalTable: "DCE",
                principalColumn: "Id");
        }
    }
}
