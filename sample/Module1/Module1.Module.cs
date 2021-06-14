﻿using System;
using ModuleLoader.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Module1.Services;
using Module2;

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
            serviceCollection.AddSingleton<ISayHelloService, SayHelloService>();
            serviceCollection.AddSingleton<ISayService, SayService>();
        }

        public override void OnApplicationInitialization(IServiceProvider serviceProvider)
        {
            var sayService = serviceProvider.GetRequiredService<ISayService>();
            sayService.SayHello();
        }
    }
}
