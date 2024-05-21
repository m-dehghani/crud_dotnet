using System.Runtime.CompilerServices;

namespace Mc2.CrudTest.Presentation.Infrastructure;

public static class MyModuleInitializer
{
    // Required for pg datetime type
    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}