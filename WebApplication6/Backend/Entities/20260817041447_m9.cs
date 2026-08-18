#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication6.Backend.Entities
{
    /// <inheritdoc />
    public partial class m9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LikelyFileLocation",
                table: "UntrackedFiles",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "UntrackedFiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "UntrackedFiles");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "UntrackedFiles",
                newName: "LikelyFileLocation");
        }
    }
}
