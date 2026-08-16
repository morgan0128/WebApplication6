using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Entities
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentCreatedInYear",
                table: "Photos",
                newName: "YearContentCreated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearContentCreated",
                table: "Photos",
                newName: "ContentCreatedInYear");
        }
    }
}
