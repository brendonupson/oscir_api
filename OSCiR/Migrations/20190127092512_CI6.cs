using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class CI6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassRelationshipEntityId",
                table: "ConfigItemRelationship",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_ClassRelationshipEntityId",
                table: "ConfigItemRelationship",
                column: "ClassRelationshipEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship",
                columns: new[] { "SourceConfigItemEntityId", "TargetConfigItemEntityId", "ClassRelationshipEntityId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItemRelationship_ClassRelationship_ClassRelationshipE~",
                table: "ConfigItemRelationship",
                column: "ClassRelationshipEntityId",
                principalTable: "ClassRelationship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItemRelationship_ClassRelationship_ClassRelationshipE~",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_ClassRelationshipEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship");

            migrationBuilder.DropColumn(
                name: "ClassRelationshipEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship",
                columns: new[] { "SourceConfigItemEntityId", "TargetConfigItemEntityId" },
                unique: true);
        }
    }
}
