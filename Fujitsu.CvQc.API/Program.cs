using Fujitsu.CvQc.API;
using Fujitsu.CvQc.API.Startup;
using Fujitsu.CvQc.Business;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = createBuilder(args);
        ConfigServices(args, builder);
        LaunchStartup(args, builder);
    }

    private static WebApplicationBuilder createBuilder(string[] args)
    {
        return WebApplication.CreateBuilder(args);
    }

    private static void ConfigServices(string[] args, WebApplicationBuilder builder)
    {
        var serviceCollection = new ServiceCollection()
            .AddSingleton<IStartupService, StartupService>()
            .AddSingleton<IBusinessLogic, BusinessLogic>()
            .AddSingleton<IProjectService, ProjectService>()
            .AddSingleton<ILogService, LogService>()
            .AddSingleton<IJobService, JobService>()
            .AddSingleton<IDocumentService, DocumentService>();
        ServiceInjector.serviceProvider = serviceCollection.BuildServiceProvider();

        var businessLogic = ServiceInjector.GetService<IBusinessLogic>();
        businessLogic.ConfigServices(serviceCollection, builder.Configuration);

        ServiceInjector.serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private static void LaunchStartup(string[] args, WebApplicationBuilder builder)
    {
        var startup = ServiceInjector.GetService<IStartupService>();
        startup.Launch(args, builder);
    }
}
