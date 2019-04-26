using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class ClassExtends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassExtend_ClassEntityId",
                table: "ClassExtend");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtend_ExtendsClassEntityId",
                table: "ClassExtend",
                column: "ExtendsClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtend_ClassEntityId_ExtendsClassEntityId",
                table: "ClassExtend",
                columns: new[] { "ClassEntityId", "ExtendsClassEntityId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassExtend_Class_ExtendsClassEntityId",
                table: "ClassExtend",
                column: "ExtendsClassEntityId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassExtend_Class_ExtendsClassEntityId",
                table: "ClassExtend");

            migrationBuilder.DropIndex(
                name: "IX_ClassExtend_ExtendsClassEntityId",
                table: "ClassExtend");

            migrationBuilder.DropIndex(
                name: "IX_ClassExtend_ClassEntityId_ExtendsClassEntityId",
                table: "ClassExtend");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtend_ClassEntityId",
                table: "ClassExtend",
                column: "ClassEntityId");
        }
    }
}
