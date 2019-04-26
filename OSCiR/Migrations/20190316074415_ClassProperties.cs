using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class ClassProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassProperty_ClassEntityId",
                table: "ClassProperty");

            migrationBuilder.DropColumn(
                name: "EnumChoices",
                table: "ClassProperty");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ClassProperty",
                newName: "TypeDefinition");

            migrationBuilder.AddColumn<string>(
                name: "ControlType",
                table: "ClassProperty",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HideWhen",
                table: "ClassProperty",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "ClassProperty",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_ClassEntityId_InternalName",
                table: "ClassProperty",
                columns: new[] { "ClassEntityId", "InternalName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassProperty_ClassEntityId_InternalName",
                table: "ClassProperty");

            migrationBuilder.DropColumn(
                name: "ControlType",
                table: "ClassProperty");

            migrationBuilder.DropColumn(
                name: "HideWhen",
                table: "ClassProperty");

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "ClassProperty");

            migrationBuilder.RenameColumn(
                name: "TypeDefinition",
                table: "ClassProperty",
                newName: "Type");

            migrationBuilder.AddColumn<string[]>(
                name: "EnumChoices",
                table: "ClassProperty",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_ClassEntityId",
                table: "ClassProperty",
                column: "ClassEntityId");
        }
    }
}
