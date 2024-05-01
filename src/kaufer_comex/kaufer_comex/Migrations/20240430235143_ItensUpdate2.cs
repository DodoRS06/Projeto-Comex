using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class ItensUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_EmbarqueRodoviario_Embarque RodoviarioId",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Itens_Itens",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_Embarque RodoviarioId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_Itens",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "Embarque RodoviarioId",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "Itens",
                table: "Notas");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_EmbarqueRodoviarioId",
                table: "Notas",
                column: "EmbarqueRodoviarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_ItemId",
                table: "Notas",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_EmbarqueRodoviario_EmbarqueRodoviarioId",
                table: "Notas",
                column: "EmbarqueRodoviarioId",
                principalTable: "EmbarqueRodoviario",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Itens_ItemId",
                table: "Notas",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_EmbarqueRodoviario_EmbarqueRodoviarioId",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Itens_ItemId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_EmbarqueRodoviarioId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_ItemId",
                table: "Notas");

            migrationBuilder.AddColumn<int>(
                name: "Embarque RodoviarioId",
                table: "Notas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Itens",
                table: "Notas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_Embarque RodoviarioId",
                table: "Notas",
                column: "Embarque RodoviarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_Itens",
                table: "Notas",
                column: "Itens");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_EmbarqueRodoviario_Embarque RodoviarioId",
                table: "Notas",
                column: "Embarque RodoviarioId",
                principalTable: "EmbarqueRodoviario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Itens_Itens",
                table: "Notas",
                column: "Itens",
                principalTable: "Itens",
                principalColumn: "Id");
        }
    }
}
