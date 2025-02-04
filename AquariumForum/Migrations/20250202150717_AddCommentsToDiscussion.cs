using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquariumForum.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsToDiscussion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageFilename",
                table: "Discussion",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_DiscussionId",
                table: "Comment",
                column: "DiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Discussion_DiscussionId",
                table: "Comment",
                column: "DiscussionId",
                principalTable: "Discussion",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Discussion_DiscussionId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_DiscussionId",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "ImageFilename",
                table: "Discussion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
