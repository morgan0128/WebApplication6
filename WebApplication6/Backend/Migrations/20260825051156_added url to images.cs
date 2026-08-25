using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Backend.Migrations
{
    /// <inheritdoc />
    public partial class addedurltoimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");
        }
    }
}
