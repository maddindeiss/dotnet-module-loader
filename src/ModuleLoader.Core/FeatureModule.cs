using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public abstract class FeatureModule : IFeatureModule
    {
        protected internal IServiceCollection ServiceCollection { get; set; }

        public void PreConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void OnApplicationStartup(IServiceProvider serviceProvider)
        {

        }

        public void OnPostApplicationStartup(IServiceProvider serviceProvider)
        {

        }

        public virtual void ConfigureApplicationInitialization(IApplicationBuilder app, IServiceProvider serviceProvider)
        {

        }
    }
}
