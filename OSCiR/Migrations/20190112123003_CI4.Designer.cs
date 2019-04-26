﻿// <auto-generated />
using System;
using OSCiR.Areas.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OSCiR.Migrations
{
    [DbContext(typeof(CMDbContext))]
    [Migration("20190112123003_CI4")]
    partial class CI4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("ClassName");

                    b.Property<string>("Comments");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsInstantiable");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("ClassName")
                        .IsUnique();

                    b.ToTable("Class");
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassExtendEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ClassEntityId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("ExtendsClassEntityId");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("ClassEntityId");

                    b.ToTable("ClassExtend");
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassPropertyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ClassEntityId");

                    b.Property<string>("Comments");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DefaultValue");

                    b.Property<string>("DisplayGroup");

                    b.Property<string>("DisplayLabel");

                    b.Property<int>("DisplayOrder");

                    b.Property<string[]>("EnumChoices");

                    b.Property<string>("InternalName");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ClassEntityId");

                    b.ToTable("ClassProperty");
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassRelationshipEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("InverseRelationshipDescription");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("RelationshipDescription");

                    b.Property<Guid>("SourceClassEntityId");

                    b.Property<Guid>("TargetClassEntityId");

                    b.HasKey("Id");

                    b.HasIndex("TargetClassEntityId");

                    b.HasIndex("SourceClassEntityId", "TargetClassEntityId")
                        .IsUnique();

                    b.ToTable("ClassRelationship");
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Owner.Model.OwnerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("OwnerCode");

                    b.Property<string>("OwnerName");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Owner");
                });

            modelBuilder.Entity("CMDB.Areas.CI.Model.ConfigItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ClassEntityId");

                    b.Property<string>("Comments");

                    b.Property<string>("ConcreteReference");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<Guid>("OwnerId");

                    b.Property<string>("PropertiesInternal")
                        .HasColumnName("Properties")
                        .HasColumnType("json");

                    b.HasKey("Id");

                    b.HasIndex("ClassEntityId");

                    b.HasIndex("OwnerId");

                    b.ToTable("ConfigItem");
                });

            modelBuilder.Entity("CMDB.Areas.CI.Model.ConfigItemRelationshipEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<Guid>("SourceConfigItemEntityId");

                    b.Property<Guid>("TargetConfigItemEntityId");

                    b.HasKey("Id");

                    b.HasIndex("TargetConfigItemEntityId");

                    b.HasIndex("SourceConfigItemEntityId", "TargetConfigItemEntityId")
                        .IsUnique();

                    b.ToTable("ConfigItemRelationship");
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassExtendEntity", b =>
                {
                    b.HasOne("CMDB.Areas.Admin.Class.Model.ClassEntity", "ParentClass")
                        .WithMany("Extends")
                        .HasForeignKey("ClassEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassPropertyEntity", b =>
                {
                    b.HasOne("CMDB.Areas.Admin.Class.Model.ClassEntity", "ParentClass")
                        .WithMany("Properties")
                        .HasForeignKey("ClassEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CMDB.Areas.Admin.Class.Model.ClassRelationshipEntity", b =>
                {
                    b.HasOne("CMDB.Areas.Admin.Class.Model.ClassEntity", "SourceClassEntity")
                        .WithMany()
                        .HasForeignKey("SourceClassEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CMDB.Areas.Admin.Class.Model.ClassEntity", "TargetClassEntity")
                        .WithMany()
                        .HasForeignKey("TargetClassEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CMDB.Areas.CI.Model.ConfigItemEntity", b =>
                {
                    b.HasOne("CMDB.Areas.Admin.Class.Model.ClassEntity", "ClassEnt")
                        .WithMany()
                        .HasForeignKey("ClassEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CMDB.Areas.Admin.Owner.Model.OwnerEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CMDB.Areas.CI.Model.ConfigItemRelationshipEntity", b =>
                {
                    b.HasOne("CMDB.Areas.CI.Model.ConfigItemEntity", "SourceConfigItemEntity")
                        .WithMany()
                        .HasForeignKey("SourceConfigItemEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CMDB.Areas.CI.Model.ConfigItemEntity", "TargetConfigItemEntity")
                        .WithMany()
                        .HasForeignKey("TargetConfigItemEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
