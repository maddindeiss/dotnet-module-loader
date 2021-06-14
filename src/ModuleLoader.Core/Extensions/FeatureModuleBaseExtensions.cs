using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace ModuleLoader.Core.Extensions
{
    public static class FeatureModuleBaseExtensions
    {
        public static FeatureModuleBase AddMainModule<MainFeatureModule>(this IServiceCollection services)
            where MainFeatureModule : IFeatureModule
        {
            return new FeatureModuleBase(typeof(MainFeatureModule), services);
        }

        public static void Initialize(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var application = app.ApplicationServices.GetRequiredService<IFeatureModuleBase>();
            application.Initialize(serviceProvider);
        }
    }
}
