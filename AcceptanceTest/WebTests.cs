using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest
{
    using Aspire.Hosting.Testing;
    using Projects;
    using System.Net;

    public class WebTests
    {
        [Fact]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange
            var appHost =
                await DistributedApplicationTestingBuilder.CreateAsync<Mc2_CrudTest_AppHost>();

            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("mc2-crudtest-presentation-server");
            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
