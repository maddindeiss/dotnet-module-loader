using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModuleLoader.Core.Attributes;

namespace ModuleLoader.Core
{
    public enum LoadingType
    {
        ByReference,
        ByDll
    }

    public class ModuleBaseApp : IModuleBaseApp
    {
        public Type RootModule { get; }
        public IServiceProvider ServiceProvider { get; set; }
        public IServiceCollection ServiceCollection { get; }
        public IApplicationBuilder ApplicationBuilder { get; set; }
        public IList<ModuleInfo> ModulesInfo { get; }

        public ModuleBaseApp(Type rootModule, IServiceCollection serviceCollection, LoadingType loadingType)
        {
            RootModule = rootModule;
            ServiceCollection = serviceCollection;

            serviceCollection.AddSingleton<IModuleBaseApp>(this);
            serviceCollection.AddSingleton<IModuleInfoContainer>(this);

            ModulesInfo = LoadModules(serviceCollection, rootModule, loadingType);

            AddServiceCollection();

            AspNetCoreModule.AddMvcBuilder(serviceCollection);

            AddInitializeServices();
        }

        private IList<ModuleInfo> LoadModules(IServiceCollection serviceCollection, Type rootModule, LoadingType loadingType)
        {
            var moduleLoader = new ModuleLoader();

            if(loadingType == LoadingType.ByReference)
                return moduleLoader.LoadModulesByReference(serviceCollection, rootModule);

            return moduleLoader.LoadModulesByDll(serviceCollection, rootModule);
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
                    featureModuleInfo.Instance.ApplicationStartup(scope.ServiceProvider);
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
    }
}
