using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public interface IFeatureModuleBase
    {
        Type MainFeatureModule { get; }
        IServiceProvider ServiceProvider { get; set; }
        IServiceCollection ServiceCollection { get; }
        void Initialize(IServiceProvider serviceProvider);
    }
}
