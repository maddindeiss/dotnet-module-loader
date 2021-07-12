using System;
using Microsoft.Extensions.DependencyInjection;
using Module2.Services;
using ModuleLoader.Core.Attributes;

namespace Module2
{
    [InitializeModule(typeof(InitializeService))]
    public class Module2Module: ModuleLoader.Core.AbstractModule
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
