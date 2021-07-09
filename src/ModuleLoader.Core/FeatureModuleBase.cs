using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModuleLoader.Core.Attributes;

namespace ModuleLoader.Core
{
    public class FeatureModuleBase : IFeatureModuleBase
    {
        public Type MainFeatureModule { get; }
        public IServiceProvider ServiceProvider { get; set; }
        public IServiceCollection ServiceCollection { get; }
        public IApplicationBuilder ApplicationBuilder { get; set; }
        public IReadOnlyList<FeatureModuleInfo> FeatureModuleInfos { get; }

        public FeatureModuleBase(Type mainFeatureModule, IServiceCollection serviceCollection)
        {
            MainFeatureModule = mainFeatureModule;
            ServiceCollection = serviceCollection;

            serviceCollection.AddSingleton<IFeatureModuleBase>(this);

            FeatureModuleInfos = LoadFeatureModules(serviceCollection, mainFeatureModule);
            AddServiceCollection();
            AddInitializeServices();
        }

        public void Initialize(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            ApplicationBuilder = app;
            ServiceProvider = serviceProvider;

            LogModuleInfo(serviceProvider);
            AddServiceProvider();
        }

        private void AddServiceCollection()
        {
            foreach (var featureModuleInfo in FeatureModuleInfos)
            {
                ((FeatureModule) featureModuleInfo.Instance).ServiceCollection = ServiceCollection;
            }

            foreach (var featureModuleInfo in FeatureModuleInfos)
            {
                featureModuleInfo.Instance.PreConfigureServices(ServiceCollection);
            }

            foreach (var featureModuleInfo in FeatureModuleInfos)
            {
                featureModuleInfo.Instance.ConfigureServices(ServiceCollection);
            }
        }

        private void AddInitializeServices()
        {
            foreach (var featureModuleInfo in FeatureModuleInfos)
            {
                var initializeAttributes = featureModuleInfo.Type.GetCustomAttributes<InitializeModule>();

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
                foreach (var featureModuleInfo in FeatureModuleInfos)
                {
                    featureModuleInfo.Instance.ConfigureApplication(ApplicationBuilder, scope.ServiceProvider);
                }

                foreach (var featureModuleInfo in FeatureModuleInfos)
                {
                    featureModuleInfo.Instance.OnApplicationStartup(scope.ServiceProvider);
                }

                var services = scope.ServiceProvider.GetServices<IInitializeService>();
                foreach (var service in services)
                {
                    service.Initialize();
                }
            }
        }

        private void LogModuleInfo(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<FeatureModuleBase>>();

            var modulesString = $"Loaded the following {FeatureModuleInfos.Count} modules: \n";

            foreach (var featureModuleInfo in FeatureModuleInfos)
            {
                modulesString += "   -> " + featureModuleInfo.Type.FullName + "\n";
            }
            logger.LogInformation(modulesString);
        }

        private IReadOnlyList<FeatureModuleInfo> LoadFeatureModules(IServiceCollection serviceCollection, Type mainFeatureModule)
        {
            var featureModuleInfos = new List<FeatureModuleInfo>();

            var featureModuleTypes = new List<Type>();
            FindModulesRecursive(featureModuleTypes, mainFeatureModule);

            foreach (var moduleType in featureModuleTypes)
            {
                var moduleInstance = (IFeatureModule)Activator.CreateInstance(moduleType);

                if (moduleInstance == null)
                    throw new InvalidOperationException($"Found module '{moduleType.AssemblyQualifiedName}' can not be instantiated");

                var featureModuleInfo = new FeatureModuleInfo(moduleType, moduleInstance);
                featureModuleInfos.Add(featureModuleInfo);

                serviceCollection.AddSingleton(moduleType, moduleInstance);
            }

            return featureModuleInfos;
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
