using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisualVibes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedAndIsModeratedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isModerated",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isModerated",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isModerated",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "isModerated",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "isBlocked",
                table: "AspNetUsers");
        }
    }
}
