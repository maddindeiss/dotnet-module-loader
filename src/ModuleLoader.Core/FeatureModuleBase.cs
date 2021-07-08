﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Attributes;

namespace ModuleLoader.Core
{
    public class FeatureModuleBase : IFeatureModuleBase
    {
        public Type MainFeatureModule { get; }
        public IServiceProvider ServiceProvider { get; set; }
        public IServiceCollection ServiceCollection { get; }
        public IApplicationBuilder ApplicationBuilder { get; set; }
        public IReadOnlyList<IFeatureModule> FeatureModules { get; }

        public FeatureModuleBase(Type mainFeatureModule, IServiceCollection serviceCollection)
        {
            MainFeatureModule = mainFeatureModule;
            ServiceCollection = serviceCollection;

            serviceCollection.AddSingleton<IFeatureModuleBase>(this);

            FeatureModules = LoadFeatureModules(serviceCollection, mainFeatureModule);
            AddServiceCollection();
            AddInitializeServices();
        }

        public void Initialize(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            Console.WriteLine("Initialize");
            ApplicationBuilder = app;
            ServiceProvider = serviceProvider;

            AddServiceProvider();
        }

        private void AddServiceCollection()
        {
            foreach (var featureModule in FeatureModules)
            {
                ((FeatureModule) featureModule).ServiceCollection = ServiceCollection;
            }

            foreach (var featureModule in FeatureModules)
            {
                featureModule.PreConfigureServices(ServiceCollection);
            }

            foreach (var featureModule in FeatureModules)
            {
                featureModule.ConfigureServices(ServiceCollection);
            }
        }

        private void AddInitializeServices()
        {
            foreach (var featureModule in FeatureModules)
            {
                var initializeAttributes = featureModule.GetType().GetCustomAttributes<InitializeModule>();

                foreach (var initializeAttribute in initializeAttributes)
                {
                    var serviceType = initializeAttribute.InitializeService;
                    ServiceCollection.AddTransient(typeof(IInitializeService), serviceType);
                }
            }
        }

        private void AddServiceProvider()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var featureModule in FeatureModules)
                {
                    featureModule.ConfigureApplication(ApplicationBuilder, scope.ServiceProvider);
                }

                foreach (var featureModule in FeatureModules)
                {
                    featureModule.OnApplicationStartup(scope.ServiceProvider);
                }

                var services = scope.ServiceProvider.GetServices<IInitializeService>();
                foreach (var service in services)
                {
                    service.Initialize();
                }
            }
        }

        private IReadOnlyList<IFeatureModule> LoadFeatureModules(IServiceCollection serviceCollection, Type mainFeatureModule)
        {
            Console.WriteLine("Try to load FeatureModules...");

            var featureModuleInfos = new List<FeatureModuleInfo>();
            var featureModuleInstances = new List<IFeatureModule>();

            var featureModuleTypes = new List<Type>();
            FindModulesRecursive(featureModuleTypes, mainFeatureModule);

            Console.WriteLine($"Found '{featureModuleTypes.Count}' modules to load");
            Console.WriteLine($"modules found:");
            foreach (var moduleType in featureModuleTypes)
            {
                Console.WriteLine($"* {moduleType.AssemblyQualifiedName}");
                var moduleInstance = (IFeatureModule)Activator.CreateInstance(moduleType);

                if (moduleInstance == null)
                    throw new InvalidOperationException($"Found module '{moduleType.AssemblyQualifiedName}' can not be instantiated");

                featureModuleInstances.Add(moduleInstance);
                serviceCollection.AddSingleton(moduleType, moduleInstance);
            }

            return featureModuleInstances;
        }

        private void FindModulesRecursive(IList<Type> featureModuleTypes, Type featureModule)
        {
            if (!typeof(IFeatureModule).GetTypeInfo().IsAssignableFrom(featureModule))
                throw new ArgumentException($"Module '{featureModule.AssemblyQualifiedName}' can not be loaded!");

            if(featureModuleTypes.Contains(featureModule))
                return;

            featureModuleTypes.Add(featureModule);

            foreach (var dependedModuleType in FindModulesByAttribute(featureModule))
            {
                FindModulesRecursive(featureModuleTypes, dependedModuleType);
            }
        }

        private IList<Type> FindModulesByAttribute(Type featureModule)
        {
            var featureModuleTypes = new List<Type>();

            var dependingModules = featureModule
                .GetCustomAttributes()
                .OfType<IsDependingOnModule>();

            foreach (var dependingModule in dependingModules)
            {
                if (!featureModuleTypes.Contains(dependingModule.DependingModule))
                    featureModuleTypes.Add(dependingModule.DependingModule);
            }

            return featureModuleTypes;
        }
    }
}
