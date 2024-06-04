using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kaufer_comex.Migrations
{
    public partial class UpdateM13Documentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEnvioSeguro",
                table: "Documentos");

            migrationBuilder.AddColumn<int>(
                name: "Courier",
                table: "Documentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Courier",
                table: "Documentos");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEnvioSeguro",
                table: "Documentos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
