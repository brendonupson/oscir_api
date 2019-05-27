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
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    IsInstantiable = table.Column<bool>(nullable: false),
                    IsPromiscuous = table.Column<bool>(nullable: false),
                    AllowAnyData = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
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
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true),
                    OwnerCode = table.Column<string>(nullable: true),
                    AlternateName1 = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
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
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_ClassExtend_Class_ExtendsClassEntityId",
                        column: x => x.ExtendsClassEntityId,
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
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    DisplayLabel = table.Column<string>(nullable: true),
                    DisplayGroup = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    InternalName = table.Column<string>(nullable: true),
                    ControlType = table.Column<string>(nullable: true),
                    TypeDefinition = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    IsMandatory = table.Column<bool>(nullable: false),
                    HideWhen = table.Column<string>(nullable: true),
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
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    SourceClassEntityId = table.Column<Guid>(nullable: false),
                    TargetClassEntityId = table.Column<Guid>(nullable: false),
                    RelationshipDescription = table.Column<string>(nullable: true),
                    IsUnique = table.Column<bool>(nullable: false)
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
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfigItem_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfigItemRelationship",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    SourceConfigItemEntityId = table.Column<Guid>(nullable: false),
                    RelationshipDescription = table.Column<string>(nullable: true),
                    TargetConfigItemEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigItemRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigItemRelationship_ConfigItem_SourceConfigItemEntityId",
                        column: x => x.SourceConfigItemEntityId,
                        principalTable: "ConfigItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigItemRelationship_ConfigItem_TargetConfigItemEntityId",
                        column: x => x.TargetConfigItemEntityId,
                        principalTable: "ConfigItem",
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
                name: "IX_ClassExtend_ExtendsClassEntityId",
                table: "ClassExtend",
                column: "ExtendsClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtend_DeletedOn_ClassEntityId_ExtendsClassEntityId",
                table: "ClassExtend",
                columns: new[] { "DeletedOn", "ClassEntityId", "ExtendsClassEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassProperty_ClassEntityId_InternalName",
                table: "ClassProperty",
                columns: new[] { "ClassEntityId", "InternalName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_SourceClassEntityId",
                table: "ClassRelationship",
                column: "SourceClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_TargetClassEntityId",
                table: "ClassRelationship",
                column: "TargetClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_DeletedOn_TargetClassEntityId",
                table: "ClassRelationship",
                columns: new[] { "DeletedOn", "TargetClassEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassRelationship_DeletedOn_SourceClassEntityId_TargetClass~",
                table: "ClassRelationship",
                columns: new[] { "DeletedOn", "SourceClassEntityId", "TargetClassEntityId", "RelationshipDescription" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_ClassEntityId",
                table: "ConfigItem",
                column: "ClassEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_OwnerId",
                table: "ConfigItem",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItem_DeletedOn_ClassEntityId_OwnerId_Name",
                table: "ConfigItem",
                columns: new[] { "DeletedOn", "ClassEntityId", "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_SourceConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "SourceConfigItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_TargetConfigItemEntityId",
                table: "ConfigItemRelationship",
                column: "TargetConfigItemEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_DeletedOn_TargetConfigItemEntityId",
                table: "ConfigItemRelationship",
                columns: new[] { "DeletedOn", "TargetConfigItemEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigItemRelationship_DeletedOn_SourceConfigItemEntityId_T~",
                table: "ConfigItemRelationship",
                columns: new[] { "DeletedOn", "SourceConfigItemEntityId", "TargetConfigItemEntityId", "RelationshipDescription" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
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
                name: "ConfigItemRelationship");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ConfigItem");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Owner");
        }
    }
}
