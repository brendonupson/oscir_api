using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class RemoveInverseRelDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InverseRelationshipDescription",
                table: "ConfigItemRelationship");

            migrationBuilder.DropColumn(
                name: "InverseRelationshipDescription",
                table: "ClassRelationship");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InverseRelationshipDescription",
                table: "ConfigItemRelationship",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InverseRelationshipDescription",
                table: "ClassRelationship",
                nullable: true);
        }
    }
}
