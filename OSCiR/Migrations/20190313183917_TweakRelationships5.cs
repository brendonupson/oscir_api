using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class TweakRelationships5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItem_ClassEntityId",
                table: "ConfigItem");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_ClassEntityId_OwnerId_Name",
                table: "ConfigItem",
                columns: new[] { "ClassEntityId", "OwnerId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItem_ClassEntityId_OwnerId_Name",
                table: "ConfigItem");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_ClassEntityId",
                table: "ConfigItem",
                column: "ClassEntityId");
        }
    }
}
