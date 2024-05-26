using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisualVibes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserProfileImageRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_ImageId",
                table: "UserProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ImageId",
                table: "UserProfiles",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_ImageId",
                table: "UserProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ImageId",
                table: "UserProfiles",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Images_ImageId",
                table: "UserProfiles",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
