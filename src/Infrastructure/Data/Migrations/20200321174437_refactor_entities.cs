using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.Nnn.Infrastructure.Data.Migrations
{
    public partial class refactor_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Users",
                newName: "ProfileImagePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "Users",
                newName: "Surname");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                nullable: true);
        }
    }
}
