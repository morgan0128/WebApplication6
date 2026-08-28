using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AlbumPhotoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisplaysDescription",
                table: "AlbumPhoto",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisplaysName",
                table: "AlbumPhoto",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisplaysYearContentCreated",
                table: "AlbumPhoto",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "AlbumPhoto",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_AlbumPhoto_AlbumsId_Order",
                table: "AlbumPhoto",
                columns: new[] { "AlbumsId", "Order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_AlbumPhoto_AlbumsId_Order",
                table: "AlbumPhoto");

            migrationBuilder.DropColumn(
                name: "DisplaysDescription",
                table: "AlbumPhoto");

            migrationBuilder.DropColumn(
                name: "DisplaysName",
                table: "AlbumPhoto");

            migrationBuilder.DropColumn(
                name: "DisplaysYearContentCreated",
                table: "AlbumPhoto");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "AlbumPhoto");
        }
    }
}
