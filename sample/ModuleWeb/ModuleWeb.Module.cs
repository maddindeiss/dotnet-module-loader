using Module1;
using ModuleLoader.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleWeb
{
    [IsDependingOnModule(typeof(Module1Module))]
    public class WebModule: ModuleLoader.Core.FeatureModule
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {

        }
    }
}
