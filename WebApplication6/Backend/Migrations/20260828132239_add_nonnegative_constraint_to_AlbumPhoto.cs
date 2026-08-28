using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Backend.Migrations
{
    /// <inheritdoc />
    public partial class add_nonnegative_constraint_to_AlbumPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_AlbumPhoto_Order_NonNegative",
                table: "AlbumPhoto",
                sql: "\"Order\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_AlbumPhoto_Order_NonNegative",
                table: "AlbumPhoto");
        }
    }
}
