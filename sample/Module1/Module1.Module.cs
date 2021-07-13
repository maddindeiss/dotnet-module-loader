using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Attributes;
using Module1.Services;
using Module2;

namespace Module1
{
    [Module("module1")]
    [Tag("module")]
    [InitializeModule(typeof(InitializeService))]
    [InitializeModule(typeof(InitializeService2))]

    // DependingOnModule is just used for module loading by reference
    [DependingOnModule(typeof(Module2Module))]
    public class Module1Module: ModuleLoader.Core.AbstractModule
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Module1Module ConfigureServices");
            serviceCollection.AddSingleton<ISayHelloService, SayHelloService>();
            serviceCollection.AddSingleton<ISayService, SayService>();
        }

        public override void ApplicationStartup(IServiceProvider serviceProvider)
        {
            // var sayService = serviceProvider.GetRequiredService<ISayService>();
            // sayService.SayHello();
        }

        public override void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseStaticFiles();
        }

    }
}
