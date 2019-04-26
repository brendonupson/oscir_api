using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class TweakRelationships3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId_TargetClassEntityId_R~",
                table: "ClassRelationship",
                columns: new[] { "SourceClassEntityId", "TargetClassEntityId", "RelationshipDescription" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_SourceClassEntityId_TargetClassEntityId_R~",
                table: "ClassRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship",
                column: "SourceClassEntityId");
        }
    }
}
