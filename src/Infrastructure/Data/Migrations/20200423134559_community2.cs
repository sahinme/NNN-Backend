using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Nnn.Infrastructure.Data.Migrations
{
    public partial class community2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "CommunityUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "CommunityUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
