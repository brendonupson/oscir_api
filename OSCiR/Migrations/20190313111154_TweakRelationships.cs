using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class TweakRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItem_Class_ClassEntityId",
                table: "ConfigItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItemRelationship_ClassRelationship_ClassRelationshipE~",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_ClassRelationshipEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId_TargetConfi~",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_SourceClassEntityId_TargetClassEntityId",
                table: "ClassRelationship");

            migrationBuilder.DropColumn(
                name: "ClassRelationshipEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.AddColumn<string>(
                name: "InverseRelationshipDescription",
                table: "ConfigItemRelationship",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelationshipDescription",
                table: "ConfigItemRelationship",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromiscuous",
                table: "Class",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "SourceConfigItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship",
                column: "SourceClassEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItem_Class_ClassEntityId",
                table: "ConfigItem",
                column: "ClassEntityId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItem_Class_ClassEntityId",
                table: "ConfigItem");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship");

            migrationBuilder.DropColumn(
                name: "InverseRelationshipDescription",
                table: "ConfigItemRelationship");

            migrationBuilder.DropColumn(
                name: "RelationshipDescription",
                table: "ConfigItemRelationship");

            migrationBuilder.DropColumn(
                name: "IsPromiscuous",
                table: "Class");

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

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId_TargetClassEntityId",
                table: "ClassRelationship",
                columns: new[] { "SourceClassEntityId", "TargetClassEntityId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItem_Class_ClassEntityId",
                table: "ConfigItem",
                column: "ClassEntityId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItemRelationship_ClassRelationship_ClassRelationshipE~",
                table: "ConfigItemRelationship",
                column: "ClassRelationshipEntityId",
                principalTable: "ClassRelationship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
