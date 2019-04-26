using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class CI3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship",
                columns: new[] { "SourceConfigItemEntityId", "TargetConfigItemEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId_TargetClassEntityId",
                table: "ClassRelationship",
                columns: new[] { "SourceClassEntityId", "TargetClassEntityId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_SourceClassEntityId_TargetClassEntityId",
                table: "ClassRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "SourceConfigItemEntityId");
        }
    }
}
