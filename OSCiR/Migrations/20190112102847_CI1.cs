using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class CI1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TargetConfigItemEntityId",
                table: "ConfigItemRelationship",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "SourceConfigItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_TargetConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "TargetConfigItemEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItemRelationship_ConfigItem_SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "SourceConfigItemEntityId",
                principalTable: "ConfigItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItemRelationship_ConfigItem_TargetConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "TargetConfigItemEntityId",
                principalTable: "ConfigItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItemRelationship_ConfigItem_SourceConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItemRelationship_ConfigItem_TargetConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ConfigItemRelationship_TargetConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropColumn(
                name: "SourceConfigItemEntityId",
                table: "ConfigItemRelationship");

            migrationBuilder.DropColumn(
                name: "TargetConfigItemEntityId",
                table: "ConfigItemRelationship");
        }
    }
}
