using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Nnn.Infrastructure.Data.Migrations
{
    public partial class category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Communities",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_CategoryId",
                table: "Communities",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Category_CategoryId",
                table: "Communities",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Category_CategoryId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_CategoryId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Communities");
        }
    }
}
