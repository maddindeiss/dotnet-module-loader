using Module1;
using ModuleLoader.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Extensions;
using ModuleWeb.Services;

namespace ModuleWeb
{
    [Module("web_module")]
    [Tag("web_module")]
    [InitializeModule(typeof(InitializeService))]

    // DependingOnModule is just used for module loading by reference
    [DependingOnModule(typeof(Module1Module))]
    public class WebModule: ModuleLoader.Core.AbstractModule
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var config = serviceCollection.GetConfiguration();
        }
    }
}
