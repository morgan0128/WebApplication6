using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Entities
{
    /// <inheritdoc />
    public partial class m6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoragePath",
                table: "Images",
                newName: "StorageFileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorageFileName",
                table: "Images",
                newName: "StoragePath");
        }
    }
}
