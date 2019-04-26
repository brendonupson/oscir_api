using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class TweakRelationships2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItem_Owner_OwnerId",
                table: "ConfigItem");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItem_Owner_OwnerId",
                table: "ConfigItem",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigItem_Owner_OwnerId",
                table: "ConfigItem");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigItem_Owner_OwnerId",
                table: "ConfigItem",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
