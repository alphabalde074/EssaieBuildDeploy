using Fujitsu.CvQc.Console;
using Fujitsu.CvQc.Console.App;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        ConfigServices();
        LaunchStartup(args);
    }

    private static void ConfigServices()
    {
        var configuration = GetConfiguration();
        ServiceInjector.serviceProvider = new ServiceCollection()
            .AddSingleton<IStartupService, StartupService>()
            .AddSingleton<IDataService, DataService>()
            .AddSingleton<IExportationService, ExportationService>()
            .AddSingleton<IImportationService, ImportationService>()
            .AddSingleton<IJobDataService, JobService>()
            .AddSingleton<IDocumentService, DocumentService>()
            .AddSingleton<ITransformationService, TransformationService>()
            .AddSingleton(configuration)
            .BuildServiceProvider();
    }

    private static IConfiguration GetConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONEMENT");

        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();
    }

    private static void LaunchStartup(string[] args)
    {
        var startup = ServiceInjector.GetService<IStartupService>();
        startup.Launch(args);
    }
}
