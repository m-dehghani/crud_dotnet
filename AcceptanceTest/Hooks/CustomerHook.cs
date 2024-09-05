using Mc2.CrudTest.Presentation;
using SpecFlow;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace AcceptanceTest.Hooks
{
    [Binding]
    public sealed class CustomerHook
    {
        private IHost _host;
        // For additional details on Reqnroll hooks see https://go.reqnroll.net/doc-hooks
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();
      
        [BeforeScenario(Order = 1)]
        public async Task FirstBeforeScenario()
        {
            await _postgres.StartAsync();

            _host = Mc2.CrudTest.Presentation.Program.CreateHostBuilder([])
                .UseContentRoot(Path.Combine(Environment.CurrentDirectory, "../../../../SpecFlowCalculator"))
                .Build();

            _host.Start();

        }
        
        

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _host.StopAsync();
            await _postgres.StopAsync();
        }
    }
}