using System;
using Microsoft.Extensions.DependencyInjection;
using Module2.Services;
using ModuleLoader.Core.Attributes;

namespace Module2
{
    [InitializeModuleService(typeof(InitializeService))]
    public class Module2Module: ModuleLoader.Core.FeatureModule
    {
        public Module2Module()
        {
            Console.WriteLine("Module2Module Constructor!");
        }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Module2Module ConfigureServices");
        }
    }
}
