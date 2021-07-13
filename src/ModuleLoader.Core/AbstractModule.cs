using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public abstract class AbstractModule : IAbstractModule
    {
        public void PreConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {

        }

        public virtual void ApplicationStartup(IServiceProvider serviceProvider)
        {

        }

        public virtual void PreConfigureModules(IList<ModuleInfo> modulesInfo, IConfiguration configuration)
        {

        }
    }
}
