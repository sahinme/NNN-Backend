using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Nnn.Infrastructure.Data.Migrations
{
    public partial class likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Unlikes_Posts_PostId",
                table: "Unlikes");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Unlikes",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "CommentId",
                table: "Unlikes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntityId",
                table: "Unlikes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "Unlikes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ReplyId",
                table: "Unlikes",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Likes",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "CommentId",
                table: "Likes",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EntityId",
                table: "Likes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "Likes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ReplyId",
                table: "Likes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unlikes_CommentId",
                table: "Unlikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Unlikes_ReplyId",
                table: "Unlikes",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CommentId",
                table: "Likes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ReplyId",
                table: "Likes",
                column: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Comments_CommentId",
                table: "Likes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Reply_ReplyId",
                table: "Likes",
                column: "ReplyId",
                principalTable: "Reply",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Unlikes_Comments_CommentId",
                table: "Unlikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Unlikes_Posts_PostId",
                table: "Unlikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Unlikes_Reply_ReplyId",
                table: "Unlikes",
                column: "ReplyId",
                principalTable: "Reply",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Comments_CommentId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Reply_ReplyId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Unlikes_Comments_CommentId",
                table: "Unlikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Unlikes_Posts_PostId",
                table: "Unlikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Unlikes_Reply_ReplyId",
                table: "Unlikes");

            migrationBuilder.DropIndex(
                name: "IX_Unlikes_CommentId",
                table: "Unlikes");

            migrationBuilder.DropIndex(
                name: "IX_Unlikes_ReplyId",
                table: "Unlikes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_CommentId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ReplyId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Unlikes");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Unlikes");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Unlikes");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Unlikes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Likes");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Unlikes",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Likes",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unlikes_Posts_PostId",
                table: "Unlikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
