using Microsoft.Extensions.DependencyInjection;

namespace Fujitsu.CvQc.Console
{
    public static class ServiceInjector
    {
        public static ServiceProvider? serviceProvider { get; set; }

        public static T GetService<T>()
        {
            if (serviceProvider == null)
            {
                throw new Exception("ServiceInjector.serviceProvider has not been created in Program.ConfigServices");
            }

            var service = serviceProvider.GetService<T>();

            if (service == null)
            {
                throw new Exception("Service has not been registered in Program.ConfigServices");
            }

            return service;
        }

    }
}

