using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OSCiR.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    IsInstantiable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigItemRelationship",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItemRelationship", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true),
                    OwnerCode = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassExtend",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ExtendsClassEntityId = table.Column<Guid>(nullable: false),
                    ClassEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassExtend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassExtend_Class_ClassEntityId",
                        column: x => x.ClassEntityId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DisplayLabel = table.Column<string>(nullable: true),
                    DisplayGroup = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    InternalName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    EnumChoices = table.Column<string[]>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    ClassEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassProperty_Class_ClassEntityId",
                        column: x => x.ClassEntityId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassRelationship",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    SourceClassEntityId = table.Column<Guid>(nullable: false),
                    TargetClassEntityId = table.Column<Guid>(nullable: false),
                    RelationshipDescription = table.Column<string>(nullable: true),
                    InverseRelationshipDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassRelationship_Class_SourceClassEntityId",
                        column: x => x.SourceClassEntityId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassRelationship_Class_TargetClassEntityId",
                        column: x => x.TargetClassEntityId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    ConcreteReference = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(type: "json", nullable: true),
                    ClassEntityId = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigItem_Class_ClassEntityId",
                        column: x => x.ClassEntityId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigItem_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Class_ClassName",
                table: "Class",
                column: "ClassName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtend_ClassEntityId",
                table: "ClassExtend",
                column: "ClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_ClassEntityId",
                table: "ClassProperty",
                column: "ClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship",
                column: "SourceClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_TargetClassEntityId",
                table: "ClassRelationship",
                column: "TargetClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_ClassEntityId",
                table: "ConfigItem",
                column: "ClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_OwnerId",
                table: "ConfigItem",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassExtend");

            migrationBuilder.DropTable(
                name: "ClassProperty");

            migrationBuilder.DropTable(
                name: "ClassRelationship");

            migrationBuilder.DropTable(
                name: "ConfigItem");

            migrationBuilder.DropTable(
                name: "ConfigItemRelationship");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Owner");
        }
    }
}
