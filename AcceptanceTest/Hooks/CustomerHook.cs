using Mc2.CrudTest.Presentation;
using Reqnroll;
using Testcontainers.PostgreSql;

namespace AcceptanceTest.Hooks
{
    [Binding]
    public sealed class CustomerHook
    {
        // For additional details on Reqnroll hooks see https://go.reqnroll.net/doc-hooks
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();
        
        [BeforeScenario("@tag1")]
        public async Task BeforeScenarioWithTag()
        {
           
        }

        [BeforeScenario(Order = 1)]
        public async Task FirstBeforeScenario()
        {
            await _postgres.StartAsync();

            _host = Mc2.CrudTest.Presentation.Program.CreateHostBuilder(null).UseContentRoot(Path.Combine(Environment.CurrentDirectory, "../../../../SpecFlowCalculator")).Build();

            _host.Start();

        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _host.stopAsync();
            await _postgres.StopAsync();
        }
    }
}