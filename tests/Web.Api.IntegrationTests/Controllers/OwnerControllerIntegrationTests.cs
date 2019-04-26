using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OSCiR.Model;
using Newtonsoft.Json;
using OSCiR;
using Xunit;

namespace Web.Api.IntegrationTests.Controllers
{
    public class PlayersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public PlayersControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetOwners()
        {
            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/api/owner");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var owners = JsonConvert.DeserializeObject<IEnumerable<OwnerEntity>>(stringResponse);
            Assert.Contains(owners, p => p.OwnerName=="PLATFORM");
            Assert.Contains(owners, p => p.OwnerName == "Customer 1");
        }


        [Fact]
        public async Task CanGetOwnerById()
        {
            var testId = SeedData.ownerPlatformId;

            // The endpoint or route of the controller action.
            var httpResponse = await _client.GetAsync("/api/owner/" + testId.ToString());

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var owner = JsonConvert.DeserializeObject<OwnerEntity>(stringResponse);
            Assert.Equal(testId.ToString(), owner.Id.ToString());
            Assert.Equal("PLATFORM", owner.OwnerName);
        }
    }
}
