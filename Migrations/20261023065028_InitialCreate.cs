using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Timur_Tooded.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategooriatabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kategooria_nimetus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kirjeldus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategooriatabel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Toodetabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Toodenimetus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kogus = table.Column<int>(type: "int", nullable: false),
                    Hind = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Pilt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategooriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toodetabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toodetabel_Kategooriatabel_KategooriaId",
                        column: x => x.KategooriaId,
                        principalTable: "Kategooriatabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kategooriatabel",
                columns: new[] { "Id", "Kategooria_nimetus", "Kirjeldus" },
                values: new object[,]
                {
                    { 1, "Joogid", null },
                    { 2, "Puu- ja köögiviljad", null },
                    { 3, "Maiustused", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Toodetabel_KategooriaId",
                table: "Toodetabel",
                column: "KategooriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Toodetabel");

            migrationBuilder.DropTable(
                name: "Kategooriatabel");
        }
    }
}
