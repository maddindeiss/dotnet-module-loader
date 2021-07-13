using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public interface IAbstractModule
    {
        void PreConfigureServices(IServiceCollection serviceCollection);
        void ConfigureServices(IServiceCollection serviceCollection);
        void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider);
        void ConfigureModules(IList<ModuleInfo> moduleContainer, IConfiguration configuration);


        void OnApplicationStartup(IServiceProvider serviceProvider);
    }
}
