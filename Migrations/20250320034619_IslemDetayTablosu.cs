using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TamirhaneMVC.Migrations
{
    /// <inheritdoc />
    public partial class IslemDetayTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IslemDetaylar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IslemId = table.Column<int>(type: "int", nullable: false),
                    HazirIslemId = table.Column<int>(type: "int", nullable: false),
                    IslemAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslemDetaylar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IslemDetaylar_HazirIslemler_HazirIslemId",
                        column: x => x.HazirIslemId,
                        principalTable: "HazirIslemler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IslemDetaylar_Islemler_IslemId",
                        column: x => x.IslemId,
                        principalTable: "Islemler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IslemDetaylar_HazirIslemId",
                table: "IslemDetaylar",
                column: "HazirIslemId");

            migrationBuilder.CreateIndex(
                name: "IX_IslemDetaylar_IslemId",
                table: "IslemDetaylar",
                column: "IslemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IslemDetaylar");
        }
    }
}
