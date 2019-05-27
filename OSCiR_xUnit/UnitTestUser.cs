using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using App;
using Application.Interfaces;
using DomainLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using OSCiR.Areas.Shared;
using OSCiR.Datastore;
using OSCiR.Model;
using Xunit;
using Xunit.Priority;

namespace OSCiR_xUnit
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [ExcludeFromCodeCoverage]
    public class UserTests
    {
        private String _secret = System.Convert.ToBase64String(Encoding.UTF8.GetBytes("THIS IS AN EXAMPLE SECRET"));
        private string _adminUserName = "admin";
        private string _adminPassword = "$%fr23FF";
        private UserManager _userManager;

        public UserTests()
        {
            var options = new DbContextOptionsBuilder<CMDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDBUserTests")
                    .Options;


            _userManager = new UserManager(new UserRepository(new CMDbContext(options)), _secret);

        }

        [Fact, Priority(1)]
        public void TestBadInput()
        {
            //UserManager um = new UserManager(new UserTestData(), Secret);
            Exception ex = Assert.Throws<DataReadException>(() => _userManager.GetUser(Guid.Empty));
            Assert.Equal("DataReadException", ex.GetType().Name);

        }

        [Fact, Priority(2)]
        public void TestCreateAdminUser()
        {
            var user = _userManager.GenerateDefaultUser(_adminPassword);
            Assert.Equal(_adminUserName, user.Username);

            var x = _userManager.GetUserCountAsync();
            Assert.Equal(1, x.Result);

            //not allowed to create same key twice
            Exception ex = Assert.Throws<DataWriteException>(() => _userManager.Create(user));
            Assert.Equal("DataWriteException", ex.GetType().Name);
        }



        [Fact, Priority(3)]
        public void TestAuthentication()
        {
            var user = _userManager.Authenticate(_adminUserName, _adminPassword);
            Assert.Equal(_adminUserName, user.Username);

            var user2 = _userManager.Authenticate(_adminUserName, "The wrong Password");
            Assert.Null(user2);
        }

        [Fact, Priority(3)]
        public void TestOtherUser()
        {
            UserEntity user = new UserEntity()
            {
                Username = "bob",
                FirstName = "Robert",
                LastName = "Smith",
                IsAdmin = false
            };
            user.SetPassword("password");

            var user2 = _userManager.Create(user);
            Assert.Equal("Robert", user2.FirstName);

            var user3 = _userManager.Authenticate("bob", "passworda");
            Assert.Null(user3);
        }
    }


}
