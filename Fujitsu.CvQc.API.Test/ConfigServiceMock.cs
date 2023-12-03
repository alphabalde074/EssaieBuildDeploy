using AutoMapper;
using Fujitsu.CvQc.Business;
using Fujitsu.CvQc.Business.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Fujitsu.CvQc.API.Test
{
    internal class ConfigServiceMock
    {
        public void mockServiceProvider()
        {
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DocumentMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            var serviceCollection = new ServiceCollection()
                .AddSingleton<IProjectService, ProjectServiceMock>()
                .AddSingleton<IDocumentService, DocumentServiceMock>()
                .AddSingleton<IJobService, JobServiceMock>()
                .AddSingleton(mapper)
                ;


            ServiceInjector.serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
