using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Backend.Migrations
{
    /// <inheritdoc />
    public partial class migration_albumphoto_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumPhoto_Albums_CollectionsId",
                table: "AlbumPhoto");

            migrationBuilder.RenameColumn(
                name: "CollectionsId",
                table: "AlbumPhoto",
                newName: "AlbumsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumPhoto_Albums_AlbumsId",
                table: "AlbumPhoto",
                column: "AlbumsId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumPhoto_Albums_AlbumsId",
                table: "AlbumPhoto");

            migrationBuilder.RenameColumn(
                name: "AlbumsId",
                table: "AlbumPhoto",
                newName: "CollectionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumPhoto_Albums_CollectionsId",
                table: "AlbumPhoto",
                column: "CollectionsId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
