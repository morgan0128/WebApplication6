using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Backend.Migrations
{
    /// <inheritdoc />
    public partial class renamePPtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioPage_Albums_AlbumId",
                table: "PortfolioPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioPage",
                table: "PortfolioPage");

            migrationBuilder.RenameTable(
                name: "PortfolioPage",
                newName: "PortfolioPages");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioPage_AlbumId",
                table: "PortfolioPages",
                newName: "IX_PortfolioPages_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioPages",
                table: "PortfolioPages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioPages_Albums_AlbumId",
                table: "PortfolioPages",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioPages_Albums_AlbumId",
                table: "PortfolioPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioPages",
                table: "PortfolioPages");

            migrationBuilder.RenameTable(
                name: "PortfolioPages",
                newName: "PortfolioPage");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioPages_AlbumId",
                table: "PortfolioPage",
                newName: "IX_PortfolioPage_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioPage",
                table: "PortfolioPage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioPage_Albums_AlbumId",
                table: "PortfolioPage",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
