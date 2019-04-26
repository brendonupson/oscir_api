using System;
using OSCiR.Areas.Shared;
using OSCiR.Model;

namespace Web.Api.IntegrationTests
{
    public static class SeedData
    {
        public static Guid ownerPlatformId = new Guid("b9eea287-184e-440f-9a43-2f354ddde2a8");
        public static Guid ownerCustomerId = new Guid("0f86b9a9-142e-4076-a377-d6a4ea434bd6");
        public static string AccessUsername = "admin";
        public static string AccessPassword = "admin";

        public static void PopulateTestData(CMDbContext dbContext)
        {

            UserEntity userEntity = new UserEntity() { Id = Guid.NewGuid(), Username = AccessUsername };
            userEntity.SetPassword(AccessPassword);

            dbContext.Add(userEntity);

            dbContext.Add(new OwnerEntity() { Id = ownerPlatformId, OwnerName = "PLATFORM" });
            dbContext.Add(new OwnerEntity() { Id = ownerCustomerId, OwnerName = "Customer 1" });



            //dbContext.Players.Add(new Player("Wayne", "Gretzky", 183, 84, new DateTime(1961,1,26)) { Id = 1, Created = DateTime.UtcNow });
            //dbContext.Players.Add(new Player("Mario", "Lemieux", 193, 91, new DateTime(1965, 11, 5)) { Id = 2, Created = DateTime.UtcNow });
            dbContext.SaveChanges();
        }
    }
}
