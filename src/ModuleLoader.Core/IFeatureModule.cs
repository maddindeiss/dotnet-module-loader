using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public interface IFeatureModule
    {
        void PreConfigureServices(IServiceCollection serviceCollection);
        void ConfigureServices(IServiceCollection serviceCollection);
        void OnApplicationInitialization(IServiceProvider serviceProvider);
    }
}
