using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivateLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AttributeFixNameAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Publishers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Publishers",
                newName: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Publishers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Publishers",
                newName: "Bio");
        }
    }
}
