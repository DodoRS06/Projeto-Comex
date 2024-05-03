using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateNotasRemoveItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Itens_ItemId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_ItemId",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Notas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Notas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_ItemId",
                table: "Notas",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Itens_ItemId",
                table: "Notas",
                column: "ItemId",
                principalTable: "Itens",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
