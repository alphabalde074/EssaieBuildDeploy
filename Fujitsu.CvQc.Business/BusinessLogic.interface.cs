using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fujitsu.CvQc.Business;
public interface IBusinessLogic
{
    public void ConfigServices(IServiceCollection serviceCollection, IConfiguration configuration);
}