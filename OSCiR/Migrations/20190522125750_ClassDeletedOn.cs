using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class ClassDeletedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassProperty_ClassEntityId_InternalName",
                table: "ClassProperty");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_ClassEntityId",
                table: "ClassProperty",
                column: "ClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_DeletedOn_ClassEntityId_InternalName",
                table: "ClassProperty",
                columns: new[] { "DeletedOn", "ClassEntityId", "InternalName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassProperty_ClassEntityId",
                table: "ClassProperty");

            migrationBuilder.DropIndex(
                name: "IX_ClassProperty_DeletedOn_ClassEntityId_InternalName",
                table: "ClassProperty");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_ClassEntityId_InternalName",
                table: "ClassProperty",
                columns: new[] { "ClassEntityId", "InternalName" },
                unique: true);
        }
    }
}
