using Module1;
using ModuleLoader.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Extensions;
using ModuleWeb.Services;

namespace ModuleWeb
{
    [Module("web_module")]
    [DependingOnModuleByName("module1")]

    [DependingOnModule(typeof(Module1Module))]
    [InitializeModule(typeof(InitializeService))]
    public class WebModule: ModuleLoader.Core.AbstractModule
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var config = serviceCollection.GetConfiguration();
        }
    }
}
