using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fujitsu.CvQc.Data;
public interface IDataAccess
{
    public void ConfigDataContext(IServiceCollection serviceCollection, IConfiguration configuration);
}
