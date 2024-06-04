using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateM15EmbarqueRodoviario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ValorProcessos_ProcessoId",
                table: "ValorProcessos");

            migrationBuilder.DropIndex(
                name: "IX_EmbarqueRodoviario_ProcessoId",
                table: "EmbarqueRodoviario");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_ProcessoId",
                table: "Documentos");

            migrationBuilder.DropIndex(
                name: "IX_Despacho_ProcessoId",
                table: "Despacho");

            migrationBuilder.AddColumn<string>(
                name: "Booking",
                table: "EmbarqueRodoviario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeadlineCarga",
                table: "EmbarqueRodoviario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeadlineDraft",
                table: "EmbarqueRodoviario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeadlineVgm",
                table: "EmbarqueRodoviario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ValorProcessos_ProcessoId",
                table: "ValorProcessos",
                column: "ProcessoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmbarqueRodoviario_ProcessoId",
                table: "EmbarqueRodoviario",
                column: "ProcessoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ProcessoId",
                table: "Documentos",
                column: "ProcessoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Despacho_ProcessoId",
                table: "Despacho",
                column: "ProcessoId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ValorProcessos_ProcessoId",
                table: "ValorProcessos");

            migrationBuilder.DropIndex(
                name: "IX_EmbarqueRodoviario_ProcessoId",
                table: "EmbarqueRodoviario");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_ProcessoId",
                table: "Documentos");

            migrationBuilder.DropIndex(
                name: "IX_Despacho_ProcessoId",
                table: "Despacho");

            migrationBuilder.DropColumn(
                name: "Booking",
                table: "EmbarqueRodoviario");

            migrationBuilder.DropColumn(
                name: "DeadlineCarga",
                table: "EmbarqueRodoviario");

            migrationBuilder.DropColumn(
                name: "DeadlineDraft",
                table: "EmbarqueRodoviario");

            migrationBuilder.DropColumn(
                name: "DeadlineVgm",
                table: "EmbarqueRodoviario");

            migrationBuilder.CreateIndex(
                name: "IX_ValorProcessos_ProcessoId",
                table: "ValorProcessos",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmbarqueRodoviario_ProcessoId",
                table: "EmbarqueRodoviario",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ProcessoId",
                table: "Documentos",
                column: "ProcessoId");

            migrationBuilder.CreateIndex(
                name: "IX_Despacho_ProcessoId",
                table: "Despacho",
                column: "ProcessoId");
        }
    }
}
