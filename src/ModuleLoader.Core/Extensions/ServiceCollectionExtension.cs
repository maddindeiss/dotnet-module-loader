using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ModuleLoader.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var hostBuilderContext = (HostBuilderContext)services
                .FirstOrDefault(d => d.ServiceType == typeof(HostBuilderContext))?.ImplementationInstance;

            if (hostBuilderContext?.Configuration != null)
            {
                return hostBuilderContext.Configuration as IConfigurationRoot;
            }

            var configuration = (IConfiguration)services
                .FirstOrDefault(d => d.ServiceType == typeof(IConfiguration))?.ImplementationInstance;

            return configuration;
        }
    }
}
