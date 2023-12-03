using Fujitsu.CvQc.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fujitsu.CvQc.Business;
public class BusinessLogic : IBusinessLogic
{
    public void ConfigServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection
            .AddSingleton<IProjectDataService, ProjectDataService>()
            .AddSingleton<IJobDataService, JobDataService>()
            .AddSingleton<ILogDataService, LogDataService>()
            .AddSingleton<IDocumentDataService, DocumentDataService>()
            .AddSingleton<IDataAccess, DataAccess>();

        var dataAccess = serviceCollection.BuildServiceProvider().GetService<IDataAccess>()!;
        dataAccess.ConfigDataContext(serviceCollection, configuration);
    }
}
