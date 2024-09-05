using AcceptanceTest.Pages;
using AcceptanceTest.Services;
using AcceptanceTest.Settings;
using AcceptanceTest.StepDefinitions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Autofac;
using Microsoft.Playwright;
using Reqnroll.Autofac;

namespace AcceptanceTest
{
    public static class TestStartup
    {
        [ScenarioDependencies]
        public static void CreateServices(ContainerBuilder builder)
        {
            builder.RegisterConfiguration();
            builder.RegisterPlaywright();
            builder.RegisterAppSettings();
            builder.RegisterPages();
            builder.RegisterPagesHandler();
            builder.RegisterPageDependencyService();
            builder.RegisterSteps();
        }

        private static void RegisterSteps(this ContainerBuilder builder)
        {
            builder.RegisterType<CustomerPageStepDefinitions>().InstancePerDependency();
        }

        private static void RegisterConfiguration(this ContainerBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("Settings/appsettings.json", false, true)
                .Build();

            builder.RegisterInstance(configuration)
                .As<IConfiguration>()
                .SingleInstance();
        }

        private static void RegisterAppSettings(this ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                IConfiguration configuration = c.Resolve<IConfiguration>();
                AppSettings appSettings = new AppSettings();
                configuration.Bind(appSettings);
                return Options.Create(appSettings);
            }).As<IOptions<AppSettings>>();
        }

        private static void RegisterPlaywright(this ContainerBuilder builder)
        {
            builder.Register(async _ =>
            {
                IPlaywright? playwright = await Playwright.CreateAsync().ConfigureAwait(false);
                IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                    SlowMo = 200
                }).ConfigureAwait(false);
                return await browser.NewPageAsync().ConfigureAwait(false);
            }).As<Task<IPage>>().InstancePerDependency();
        }

        private static void RegisterPages(this ContainerBuilder builder)
        {
            builder.RegisterType<CustomerPage>().AsSelf().InstancePerDependency();
        }

        private static void RegisterPagesHandler(this ContainerBuilder builder)
        {
            builder.RegisterType<PagesService>().As<IPageService>().InstancePerLifetimeScope();
        }

        private static void RegisterPageDependencyService(this ContainerBuilder builder)
        {
            builder.RegisterType<PageDependencyService>().As<IPageDependencyService>().InstancePerLifetimeScope();
        }
    }
}
