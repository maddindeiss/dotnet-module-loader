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
    public class ModuleBaseApp : IModuleBaseApp
    {
        public Type RootModule { get; }
        public IServiceProvider ServiceProvider { get; set; }
        public IServiceCollection ServiceCollection { get; }
        public IApplicationBuilder ApplicationBuilder { get; set; }
        public IList<ModuleInfo> ModulesInfo { get; }

        public ModuleBaseApp(Type rootModule, IServiceCollection serviceCollection)
        {
            RootModule = rootModule;
            ServiceCollection = serviceCollection;

            serviceCollection.AddSingleton<IModuleBaseApp>(this);
            serviceCollection.AddSingleton<IModuleInfoContainer>(this);

            // Load modules by type reference
            //ModulesInfo = LoadModules(serviceCollection, rootModule);

            // Load modules by dll
            var moduleLoader = new ModuleLoader();
            ModulesInfo = moduleLoader.LoadModules(serviceCollection, rootModule);

            AddServiceCollection();

            AspNetCoreModule.AddMvcBuilder(serviceCollection);

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
            foreach (var featureModuleInfo in ModulesInfo)
            {
                ((AbstractModule) featureModuleInfo.Instance).ServiceCollection = ServiceCollection;
            }

            foreach (var featureModuleInfo in ModulesInfo)
            {
                featureModuleInfo.Instance.PreConfigureServices(ServiceCollection);
            }

            foreach (var featureModuleInfo in ModulesInfo)
            {
                featureModuleInfo.Instance.ConfigureServices(ServiceCollection);
            }
        }

        private void AddInitializeServices()
        {
            foreach (var featureModuleInfo in ModulesInfo)
            {
                var initializeAttributes = featureModuleInfo.Type.GetCustomAttributes<InitializeModuleAttribute>();

                foreach (var initializeAttribute in initializeAttributes)
                {
                    var serviceType = initializeAttribute.InitializeService;
                    ServiceCollection.AddTransient(typeof(IModuleInitialization), serviceType);
                }
            }
        }

        private void AddServiceProvider()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var featureModuleInfo in ModulesInfo)
                {
                    featureModuleInfo.Instance.ConfigureApplication(ApplicationBuilder, scope.ServiceProvider);
                }

                foreach (var featureModuleInfo in ModulesInfo)
                {
                    featureModuleInfo.Instance.OnApplicationStartup(scope.ServiceProvider);
                }

                var services = scope.ServiceProvider.GetServices<IModuleInitialization>();
                foreach (var service in services)
                {
                    service.Initialize().GetAwaiter();
                }
            }
        }

        private void LogModuleInfo(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<IModuleBaseApp>>();

            var modulesString = $"Loaded the following {ModulesInfo.Count} modules:";

            foreach (var featureModuleInfo in ModulesInfo)
            {
                modulesString += $"{Environment.NewLine} -> {featureModuleInfo.Type.FullName}";
            }
            logger.LogInformation(modulesString);
        }

        private IList<ModuleInfo> LoadModules(IServiceCollection serviceCollection, Type rootModule)
        {
            var featureModuleInfos = new List<ModuleInfo>();

            var featureModuleTypes = new List<Type>();
            FindModulesRecursive(featureModuleTypes, rootModule);

            foreach (var moduleType in featureModuleTypes)
            {
                var moduleInstance = (IAbstractModule)Activator.CreateInstance(moduleType);

                if (moduleInstance == null)
                    throw new InvalidOperationException($"Found module '{moduleType.AssemblyQualifiedName}' can not be instantiated");

                var featureModuleInfo = new ModuleInfo(moduleType, moduleInstance);
                featureModuleInfos.Add(featureModuleInfo);

                serviceCollection.AddSingleton(moduleType, moduleInstance);
            }

            return featureModuleInfos;
        }

        private void FindModulesRecursive(IList<Type> featureModuleTypes, Type featureModule)
        {
            if (!typeof(IAbstractModule).GetTypeInfo().IsAssignableFrom(featureModule))
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
                .OfType<DependingOnModuleAttribute>();

            foreach (var dependingModule in dependingModules)
            {
                if (!featureModuleTypes.Contains(dependingModule.DependingModule))
                    featureModuleTypes.Add(dependingModule.DependingModule);
            }

            return featureModuleTypes;
        }
    }
}
