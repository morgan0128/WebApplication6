#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication6.Backend.Entities
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentCreatedInYear",
                table: "Photos",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentCreatedInYear",
                table: "Photos");
        }
    }
}
