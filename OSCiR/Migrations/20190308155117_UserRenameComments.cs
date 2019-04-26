using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class UserRenameComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "User",
                newName: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "User",
                newName: "Comment");
        }
    }
}
