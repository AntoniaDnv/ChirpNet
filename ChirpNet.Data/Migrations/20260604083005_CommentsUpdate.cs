using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChirpNet.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommentsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isIsDeleted",
                table: "Comments",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Comments",
                newName: "isIsDeleted");
        }
    }
}
