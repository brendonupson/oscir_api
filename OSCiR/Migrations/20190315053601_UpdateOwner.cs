using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class UpdateOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ALternateName1",
                table: "Owner",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Owner",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ALternateName1",
                table: "Owner");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Owner");
        }
    }
}
