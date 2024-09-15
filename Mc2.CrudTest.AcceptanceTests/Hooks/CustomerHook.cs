using Testcontainers.PostgreSql;

namespace ReqnrollProject1.Hooks
{
    [Binding]
    public sealed class CustomerHook
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();

        [BeforeScenario("@tag1")]
        public async Task BeforeScenarioWithTag()
        {
            await _postgres.StartAsync();
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            // Example of ordering the execution of hooks
            // See https://go.reqnroll.net/doc-hooks#hook-execution-order

            //TODO: implement logic that has to run before executing each scenario
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _postgres.StopAsync();
        }
    }
}