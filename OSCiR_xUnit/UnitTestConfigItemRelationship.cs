using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App;
using Application.Interfaces;
using DomainLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using OSCiR.Areas.Admin.Class.Model;
using OSCiR.Areas.Shared;
using OSCiR.Datastore;
using OSCiR.Model;
using Xunit;
using Xunit.Priority;

namespace OSCiR_xUnit
{

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [ExcludeFromCodeCoverage]
    public class ConfigItemRelationshipTests
    {
        public static Guid OwnerPlatformId = new Guid("b9eea287-184e-440f-9a43-2f354ddde2a8");
        public static Guid OwnerCustomerId = new Guid("0f86b9a9-142e-4076-a377-d6a4ea434bd6");
        public static Guid SourceClassId = new Guid("1f86b9a9-142e-4076-a377-e6a4ea434bd8");
        public static Guid TargetClassId = new Guid("2f86b9a9-142e-4076-a377-f6a4ea434bd9");
        public static Guid TargetClassId2 = new Guid("3f86b9a9-142e-4076-a377-a6a4ea434bd0");
        public static Guid TargetClassId3 = new Guid("3f86b9a9-142e-4076-a377-a6a4ea434bd1");

        public Guid CISourceId = new Guid("3f86b9a9-0001-4076-a377-a6a4ea434bd0");
        public Guid CITargetId1 = new Guid("3f86b9a9-0002-4076-a377-a6a4ea434bd0");
        public Guid CITargetId1a = new Guid("3f86b9a9-0003-4076-a377-a6a4ea434bd0");
        public Guid CITargetId2 = new Guid("3f86b9a9-0004-4076-a377-a6a4ea434bd0");
        public Guid CITargetId2a = new Guid("3f86b9a9-0005-4076-a377-a6a4ea434bd0");


        private BlueprintManager _blueprintManager;
        private ConfigItemManager _configItemManager;
        private DbContext _dbContext;



        public ConfigItemRelationshipTests()
        {
            var options = new DbContextOptionsBuilder<CMDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDBRelationshipTests")
                .Options;
            //TODO seed data

            _dbContext = new CMDbContext(options);

            _blueprintManager = new BlueprintManager(new BlueprintRepository(_dbContext), new ConfigItemRepository(_dbContext));
            _configItemManager = new ConfigItemManager(new ConfigItemRepository(_dbContext), new BlueprintRepository(_dbContext));


        }



        [Fact, Priority(1)]
        public void TestSeedData()
        {
            UserEntity userEntity = new UserEntity() { Id = Guid.NewGuid(), Username = "admin" };
            userEntity.SetPassword("password");
            _dbContext.Add(userEntity);

            _dbContext.Add(new OwnerEntity() { Id = OwnerPlatformId, OwnerName = "PLATFORM", OwnerCode = "P" });
            _dbContext.Add(new OwnerEntity() { Id = OwnerCustomerId, OwnerName = "Customer 1", OwnerCode = "CUST1" });
            _dbContext.SaveChanges();


            _blueprintManager.CreateClass(new ClassEntity() { Id = SourceClassId, ClassName = "SourceClass1", IsInstantiable = true });
            _blueprintManager.CreateClass(new ClassEntity() { Id = TargetClassId, ClassName = "TargetClass1", IsInstantiable = true });
            _blueprintManager.CreateClass(new ClassEntity() { Id = TargetClassId2, ClassName = "TargetClass2", IsInstantiable = true });
            _blueprintManager.CreateClass(new ClassEntity() { Id = TargetClassId3, ClassName = "_TargetClass3", IsInstantiable = false });


            _blueprintManager.CreateClassRelationship(new ClassRelationshipEntity() { Id = Guid.NewGuid(), SourceClassEntityId = SourceClassId, TargetClassEntityId = TargetClassId, RelationshipDescription = "Rel1", IsUnique = true });

            //test trying to make a relationship to a non-instantiable class
            Exception ex = Assert.Throws<DataWriteException>(() => _blueprintManager.CreateClassRelationship(new ClassRelationshipEntity() { Id = Guid.NewGuid(), SourceClassEntityId = SourceClassId, TargetClassEntityId = TargetClassId3, RelationshipDescription = "This should fail" }));
            Assert.Equal("DataWriteException", ex.GetType().Name);



            var c1 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CISourceId, ClassEntityId = SourceClassId, Name = "VM-SOURCE", OwnerId = OwnerPlatformId });
            Assert.NotNull(c1);
            var c2 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CITargetId1, ClassEntityId = TargetClassId, Name = "VM-TARGET-1", OwnerId = OwnerPlatformId });
            Assert.NotNull(c2);
            var c3 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CITargetId1a, ClassEntityId = TargetClassId, Name = "VM-TARGET-1a", OwnerId = OwnerPlatformId });
            Assert.NotNull(c3);
            var c4 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CITargetId2, ClassEntityId = TargetClassId2, Name = "VM-TARGET-2", OwnerId = OwnerPlatformId });
            Assert.NotNull(c4);
            var c5 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CITargetId2a, ClassEntityId = TargetClassId2, Name = "VM-TARGET-2a", OwnerId = OwnerPlatformId });
            Assert.NotNull(c5);

        }

        [Fact, Priority(2)]
        public void TestRelationships()
        {
            var rel1 = _configItemManager.CreateConfigItemRelationship(CISourceId, CITargetId1, null, false, "USER1");
            Assert.NotNull(rel1);
            Assert.Equal(CITargetId1.ToString(), rel1.TargetConfigItemEntityId.ToString());
            Assert.Equal("Rel1", rel1.RelationshipDescription);

            //this should switch relationship to new CI
            _configItemManager.CreateConfigItemRelationship(CISourceId, CITargetId1a, null, false, "USER1");
            var ci = _configItemManager.ReadConfigItems(new Guid[] { CISourceId }).First();
            Console.WriteLine("count=" + ci.SourceRelationships.Count());
            Assert.Single(ci.SourceRelationships); //should be only 1 relationship
            //check relationship has switched to second CI
            Assert.Equal(CITargetId1a, ci.SourceRelationships.First().TargetConfigItemEntityId);



            Exception ex = Assert.Throws<DataWriteException>(() => _configItemManager.CreateConfigItemRelationship(CISourceId, Guid.NewGuid(), "This should fail", false, "USER1"));
            Assert.Equal("DataWriteException", ex.GetType().Name);



        }


    }//class


}
