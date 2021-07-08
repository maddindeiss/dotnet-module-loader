using System;
using Microsoft.AspNetCore.Builder;
using ModuleLoader.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Module1.Services;
using Module2;
using ModuleLoader.Core;

namespace Module1
{
    [IsDependingOnModule(typeof(Module2Module))]
    public class Module1Module: ModuleLoader.Core.FeatureModule
    {
        public Module1Module()
        {
            Console.WriteLine("Module1Module Constructor!");
        }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Module1Module ConfigureServices");
            serviceCollection.AddTransient<IServiceInitialization, InitializeService>();
            serviceCollection.AddSingleton<ISayHelloService, SayHelloService>();
            serviceCollection.AddSingleton<ISayService, SayService>();
        }

        public override void OnApplicationStartup(IServiceProvider serviceProvider)
        {
            // var sayService = serviceProvider.GetRequiredService<ISayService>();
            // sayService.SayHello();
        }

        public override void ConfigureApplicationInitialization(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseStaticFiles();
        }
    }
}
