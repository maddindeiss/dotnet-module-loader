using Module1;
using ModuleLoader.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Extensions;
using ModuleWeb.Services;

namespace ModuleWeb
{
    [IsDependingOnModule(typeof(Module1Module))]
    [InitializeModule(typeof(InitializeService))]
    public class WebModule: ModuleLoader.Core.FeatureModule
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var config = serviceCollection.GetConfiguration();
        }
    }
}
