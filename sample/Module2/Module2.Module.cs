using System;
using Microsoft.Extensions.DependencyInjection;
using Module1.Services;
using ModuleLoader.Core;

namespace Module2
{
    public class Module2Module: ModuleLoader.Core.FeatureModule
    {
        public Module2Module()
        {
            Console.WriteLine("Module2Module Constructor!");
        }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Module2Module ConfigureServices");
            serviceCollection.AddTransient<IServiceInitialization, InitializeService>();
        }
    }
}
