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
    public class BlueprintTests
    {
        private BlueprintManager _blueprintManager;

        public BlueprintTests()
        {
            var options = new DbContextOptionsBuilder<CMDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDBBlueprintTests")
                .Options;
            //TODO seed data

            var dbContext = new CMDbContext(options);
            _blueprintManager = new BlueprintManager(new BlueprintRepository(dbContext), new ConfigItemRepository(dbContext));
        }


        [Fact, Priority(1)]
        public void TestBadInput()
        {
            Exception ex = Assert.Throws<DataReadException>(() => _blueprintManager.GetConfigItemExample(Guid.Empty));
            Assert.Equal("DataReadException", ex.GetType().Name);
        }


        [Fact, Priority(2)]
        public void TestSeedData()
        {

        }
    }


}
