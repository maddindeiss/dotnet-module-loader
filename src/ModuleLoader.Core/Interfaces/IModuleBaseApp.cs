using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public interface IModuleBaseApp: IModuleInfoContainer
    {
        Type RootModule { get; }
        IServiceProvider ServiceProvider { get; set; }
        IServiceCollection ServiceCollection { get; }
        void Initialize(IApplicationBuilder app, IServiceProvider serviceProvider);
    }
}
