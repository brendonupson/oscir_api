using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public class ConfigItemTests
    {
        public static Guid OwnerPlatformId = new Guid("b9eea287-184e-440f-9a43-2f354ddde2a8");
        public static Guid OwnerCustomerId = new Guid("0f86b9a9-142e-4076-a377-d6a4ea434bd6");
        public static Guid SourceClassId = new Guid("1f86b9a9-142e-4076-a377-e6a4ea434bd8");
        public static Guid TargetClassId = new Guid("2f86b9a9-142e-4076-a377-f6a4ea434bd9");
        public static Guid TargetClassId2 = new Guid("3f86b9a9-142e-4076-a377-a6a4ea434bd0");

        public Guid CISourceId = new Guid("3f86b9a9-0001-4076-a377-a6a4ea434bd0");
        public Guid CITargetId1 = new Guid("3f86b9a9-0002-4076-a377-a6a4ea434bd0");
        public Guid CITargetId1a = new Guid("3f86b9a9-0003-4076-a377-a6a4ea434bd0");
        public Guid CITargetId2 = new Guid("3f86b9a9-0004-4076-a377-a6a4ea434bd0");
        public Guid CITargetId2a = new Guid("3f86b9a9-0005-4076-a377-a6a4ea434bd0");


        private BlueprintManager _blueprintManager;
        private ConfigItemManager _configItemManager;
        private DbContext _dbContext;



        public ConfigItemTests()
        {
            var options = new DbContextOptionsBuilder<CMDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDBConfigItemTests")
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

            _dbContext.Add(new OwnerEntity() { Id = OwnerPlatformId, OwnerName = "PLATFORM", OwnerCode="P" });
            _dbContext.Add(new OwnerEntity() { Id = OwnerCustomerId, OwnerName = "Customer 1", OwnerCode="CUST1" });
            _dbContext.SaveChanges();


            _blueprintManager.CreateClass(new ClassEntity() { Id = SourceClassId, ClassName = "SourceClass1", IsInstantiable = true });
            _blueprintManager.CreateClass(new ClassEntity() { Id = TargetClassId, ClassName = "TargetClass", IsInstantiable = true });
            _blueprintManager.CreateClass(new ClassEntity() { Id = TargetClassId2, ClassName = "TargetClass2", IsInstantiable = false });


            //_blueprintManager.CreateClassRelationship(new ClassRelationshipEntity() { Id = Guid.NewGuid(), SourceClassEntityId = SourceClassId, TargetClassEntityId = TargetClassId, RelationshipDescription = "Rel1", IsUnique = true });
            //_blueprintManager.CreateClassRelationship(new ClassRelationshipEntity() { Id = Guid.NewGuid(), SourceClassEntityId = SourceClassId, TargetClassEntityId = TargetClassId2, RelationshipDescription = "Rel2" });

            var c1 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CISourceId, ClassEntityId = SourceClassId, Name = "VM-SOURCE", OwnerId = OwnerPlatformId });
            Assert.NotNull(c1);
            var c2 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CITargetId1, ClassEntityId = TargetClassId, Name = "VM-TARGET-1", OwnerId = OwnerPlatformId });
            Assert.NotNull(c2);
            var c3 = _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = CITargetId1a, ClassEntityId = TargetClassId, Name = "VM-TARGET-1a", OwnerId = OwnerPlatformId });
            Assert.NotNull(c3);

            //CI should not be able to be created
            Exception ex = Assert.Throws<DataWriteException>(() => _configItemManager.CreateConfigItem(new ConfigItemEntity() { Id = Guid.NewGuid(), ClassEntityId = TargetClassId2, Name = "VM-TARGET-2", OwnerId = OwnerPlatformId }));
            Assert.Equal("DataWriteException", ex.GetType().Name);



        }

        [Fact, Priority(2)]
        public void TestDelete()
        {
            //check there is only 1
            var ci = _configItemManager.ReadConfigItems(new Guid[] { CITargetId1a });
            Assert.Single(ci);

            //delete it
            _configItemManager.DeleteConfigItem(CITargetId1a, "TestUser");

            //check there is none
            var ci2 = _configItemManager.ReadConfigItems(new Guid[] { CITargetId1a });
            Assert.Empty(ci2);
        }


        }//class


    }
