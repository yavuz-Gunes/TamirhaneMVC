using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TamirhaneMVC.Migrations
{
    /// <inheritdoc />
    public partial class IslemParcaVeStokTakibi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StokMiktari",
                table: "Parcalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IslemDetayi",
                table: "Islemler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "IslemTarihi",
                table: "Islemler",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "IslemParcalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IslemId = table.Column<int>(type: "int", nullable: false),
                    ParcaId = table.Column<int>(type: "int", nullable: false),
                    KullanilanMiktar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslemParcalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IslemParcalar_Islemler_IslemId",
                        column: x => x.IslemId,
                        principalTable: "Islemler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IslemParcalar_Parcalar_ParcaId",
                        column: x => x.ParcaId,
                        principalTable: "Parcalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IslemParcalar_IslemId",
                table: "IslemParcalar",
                column: "IslemId");

            migrationBuilder.CreateIndex(
                name: "IX_IslemParcalar_ParcaId",
                table: "IslemParcalar",
                column: "ParcaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IslemParcalar");

            migrationBuilder.DropColumn(
                name: "StokMiktari",
                table: "Parcalar");

            migrationBuilder.DropColumn(
                name: "IslemDetayi",
                table: "Islemler");

            migrationBuilder.DropColumn(
                name: "IslemTarihi",
                table: "Islemler");
        }
    }
}
