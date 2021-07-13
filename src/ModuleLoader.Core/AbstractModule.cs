﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public abstract class AbstractModule : IAbstractModule
    {
        protected internal IServiceCollection ServiceCollection { get; set; }

        public void PreConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {

        }

        public virtual void OnApplicationStartup(IServiceProvider serviceProvider)
        {

        }

        public virtual void ConfigureModules(IList<ModuleInfo> moduleContainer, IConfiguration configuration)
        {
            Console.WriteLine("ConfigureModules: no configuration happened");
        }
    }
}
