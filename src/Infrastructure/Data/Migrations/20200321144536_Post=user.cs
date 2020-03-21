using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Nnn.Infrastructure.Data.Migrations
{
    public partial class Postuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Users_UserId1",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_UserId1",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Post");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Post",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Users_UserId",
                table: "Post",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Users_UserId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_UserId",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Post",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId1",
                table: "Post",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Users_UserId1",
                table: "Post",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
