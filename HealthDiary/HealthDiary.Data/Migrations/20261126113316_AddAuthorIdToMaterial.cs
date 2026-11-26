using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthDiary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorIdToMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Materials");
        }
    }
}
