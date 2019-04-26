using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class CI2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRelationship_Class_SourceClassEntityId",
                table: "ClassRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship");

            migrationBuilder.AddColumn<Guid>(
                name: "FK_ClassSourceRelationships",
                table: "ClassRelationship",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_FK_ClassSourceRelationships",
                table: "ClassRelationship",
                column: "FK_ClassSourceRelationships");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRelationship_Class_FK_ClassSourceRelationships",
                table: "ClassRelationship",
                column: "FK_ClassSourceRelationships",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRelationship_Class_FK_ClassSourceRelationships",
                table: "ClassRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ClassRelationship_FK_ClassSourceRelationships",
                table: "ClassRelationship");

            migrationBuilder.DropColumn(
                name: "FK_ClassSourceRelationships",
                table: "ClassRelationship");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship",
                column: "SourceClassEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRelationship_Class_SourceClassEntityId",
                table: "ClassRelationship",
                column: "SourceClassEntityId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
