using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class Users1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "User",
                nullable: true);
        }
    }
}
