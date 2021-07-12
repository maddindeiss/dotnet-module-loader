using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public static class AspNetCoreModule
    {
        public static void AddMvcBuilder(IServiceCollection serviceCollection)
        {
            var featureModulesInfoContainer = (IModuleInfoContainer)serviceCollection
                .FirstOrDefault(d => d.ServiceType == typeof(IModuleInfoContainer))
                ?.ImplementationInstance;

            if (featureModulesInfoContainer == null)
                return;

            var assemblies = featureModulesInfoContainer.ModulesInfo
                .Select(info => info.Assembly)
                .Distinct();

            var mvcBuilder = serviceCollection.AddMvc();

            foreach (var assembly in assemblies)
            {
                mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            mvcBuilder.AddControllersAsServices();
        }
    }
}
