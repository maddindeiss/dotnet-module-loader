using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public interface IAbstractModule
    {
        void PreConfigureServices(IServiceCollection serviceCollection);
        void ConfigureServices(IServiceCollection serviceCollection);
        void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider);


        void OnApplicationStartup(IServiceProvider serviceProvider);
    }
}
