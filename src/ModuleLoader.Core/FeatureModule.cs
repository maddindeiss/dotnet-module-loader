﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace ModuleLoader.Core
{
    public abstract class FeatureModule : IFeatureModule
    {
        protected internal IServiceCollection ServiceCollection { get; set; }

        public void PreConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void ConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public virtual void OnApplicationInitialization(IServiceProvider serviceProvider)
        {

        }
    }
}
