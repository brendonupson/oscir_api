using System;
using OSCiR.Model;
using Microsoft.EntityFrameworkCore;

namespace OSCiR.Areas.Shared
{
    /*
     * dotnet ef migrations add InitialCreate   
     * Project started 12 January 2019    
     */
    public class CMDbContext : DbContext
    {
        public DbSet<OwnerEntity> Owner { get; set; }

        public DbSet<ConfigItemEntity> ConfigItem { get; set; }
        public DbSet<ConfigItemRelationshipEntity> ConfigItemRelationship { get; set; }
        //public DbSet<ConfigItemArchiveEntity> ConfigItemArchive { get; set; }

        public DbSet<ClassEntity> Class { get; set; }
        public DbSet<ClassPropertyEntity> ClassProperty { get; set; }
        public DbSet<ClassExtendEntity> ClassExtend { get; set; }
        public DbSet<ClassRelationshipEntity> ClassRelationship { get; set; }

        public DbSet<UserEntity> User { get; set; }


        public CMDbContext(DbContextOptions<CMDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //FIXME

           

            modelBuilder.Entity<ClassEntity>().HasIndex(e => e.ClassName).IsUnique();
            modelBuilder.Entity<UserEntity>().HasIndex(e => e.Username).IsUnique();

            // https://stackoverflow.com/questions/38520695/multiple-relationships-to-the-same-table-in-ef7core
            /*
            modelBuilder.Entity<ClassEntity>()
                .HasMany(ce => ce.Relationships)
                .WithOne(r => r.SourceClassEntity)
                .HasForeignKey("FK_ClassSourceRelationships")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                */


            modelBuilder.Entity<ConfigItemEntity>()
                .HasOne(b => b.ParentClassEntity)
                .WithMany(a => a.ConfigItemEntities)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConfigItemEntity>()
                .HasOne(b => b.Owner)
                .WithMany(a => a.ConfigItems)
                .OnDelete(DeleteBehavior.Restrict);



            //modelBuilder.Entity<School>().HasMany(s => s.Students).WithOne(s => s.School);
            modelBuilder.Entity<ConfigItemEntity>().HasMany(ci => ci.SourceRelationships).WithOne(r => r.SourceConfigItem);
            modelBuilder.Entity<ConfigItemEntity>().HasMany(ci => ci.TargetRelationships).WithOne(r => r.TargetConfigItem);


            modelBuilder.Entity<ClassEntity>().HasMany(c => c.Extends).WithOne(ex => ex.ParentClass);
            modelBuilder.Entity<ClassEntity>().HasMany(c => c.Properties).WithOne(ex => ex.ParentClass);
            modelBuilder.Entity<ClassEntity>().HasMany(c => c.SourceRelationships).WithOne(r => r.SourceClassEntity);
            modelBuilder.Entity<ClassEntity>().HasMany(c => c.TargetRelationships).WithOne(r => r.TargetClassEntity);


            modelBuilder.Entity<ClassRelationshipEntity>()
                .HasIndex(p => new { p.DeletedOn, p.SourceClassEntityId, p.TargetClassEntityId, p.RelationshipDescription }).IsUnique();
            modelBuilder.Entity<ClassRelationshipEntity>()
                .HasIndex(p => new { p.DeletedOn, p.TargetClassEntityId }); //for lookup speed


            modelBuilder.Entity<ConfigItemRelationshipEntity>()
                .HasIndex(p => new { p.DeletedOn, p.SourceConfigItemEntityId, p.TargetConfigItemEntityId, p.RelationshipDescription }).IsUnique();
            modelBuilder.Entity<ConfigItemRelationshipEntity>()
                .HasIndex(p => new { p.DeletedOn, p.TargetConfigItemEntityId }); //for lookup speed

            modelBuilder.Entity<ConfigItemEntity>()
                .HasIndex(p => new { p.DeletedOn, p.ClassEntityId, p.OwnerId, p.Name }).IsUnique();

            modelBuilder.Entity<ClassExtendEntity>()
               .HasIndex(p => new { p.DeletedOn, p.ClassEntityId, p.ExtendsClassEntityId }).IsUnique();

            modelBuilder.Entity<ClassPropertyEntity>()
                .HasIndex(p => new { p.DeletedOn, p.ClassEntityId, p.InternalName }).IsUnique();

            modelBuilder.Entity<ClassEntity>()
                .HasIndex(p => new { p.DeletedOn, p.Id });


            //allow multiple relationships between two CIs, but only one of each type
            /*modelBuilder.Entity<ConfigItemRelationshipEntity>()
                .HasIndex(p => new { p.SourceConfigItemEntityId, p.TargetConfigItemEntityId, p.ClassRelationshipEntityId }).IsUnique(); //unique and fast lookup by source
                */
            modelBuilder.Entity<ConfigItemRelationshipEntity>().HasIndex(e => e.TargetConfigItemEntityId); //fast lookups by target
            modelBuilder.Entity<ClassRelationshipEntity>().HasIndex(e => e.TargetClassEntityId); //fast lookups by target


            //enable default queries for soft deletes
            modelBuilder.Entity<ClassEntity>()
            .HasQueryFilter(rel => EF.Property<DateTime?>(rel, "DeletedOn") == null);

            modelBuilder.Entity<ClassRelationshipEntity>()
            .HasQueryFilter(rel => EF.Property<DateTime?>(rel, "DeletedOn") == null);

            modelBuilder.Entity<ConfigItemEntity>()
            .HasQueryFilter(rel => EF.Property<DateTime?>(rel, "DeletedOn") == null);

            modelBuilder.Entity<ConfigItemRelationshipEntity>()
            .HasQueryFilter(rel => EF.Property<DateTime?>(rel, "DeletedOn") == null);


        }
    }
}
