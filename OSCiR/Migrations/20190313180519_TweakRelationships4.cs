using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class TweakRelationships4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship",
                columns: new[] { "SourceConfigItemEntityId", "TargetConfigItemEntityId", "RelationshipDescription" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "SourceConfigItemEntityId");
        }
    }
}
