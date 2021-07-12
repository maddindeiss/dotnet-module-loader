using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace ModuleLoader.Core.Extensions
{
    public static class ModuleBaseExtensions
    {
        public static ModuleBaseApp SetRootModule<MainFeatureModule>(this IServiceCollection services)
            where MainFeatureModule : IAbstractModule
        {
            return new ModuleBaseApp(typeof(MainFeatureModule), services);
        }

        public static void Initialize(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var application = app.ApplicationServices.GetRequiredService<IModuleBaseApp>();
            application.Initialize(app, serviceProvider);
        }
    }
}
