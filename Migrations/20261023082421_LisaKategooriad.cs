using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Timur_Tooded.Migrations
{
    /// <inheritdoc />
    public partial class LisaKategooriad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Kategooriatabel",
                columns: new[] { "Id", "Kategooria_nimetus", "Kirjeldus" },
                values: new object[,]
                {
                    { 4, "Piimatooted ja munad", null },
                    { 5, "Liha", null },
                    { 6, "Pagaritooted", null },
                    { 7, "Muu", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Kategooriatabel",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Kategooriatabel",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Kategooriatabel",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Kategooriatabel",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
