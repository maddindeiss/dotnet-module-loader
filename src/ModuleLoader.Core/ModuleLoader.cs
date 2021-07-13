using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Attributes;
using ModuleLoader.Core.Extensions;

namespace ModuleLoader.Core
{
    public class ModuleLoader
    {
        public IList<ModuleInfo> LoadModulesByReference(IServiceCollection serviceCollection, Type rootModule)
        {
            var featureModuleInfos = new List<ModuleInfo>();

            var featureModuleTypes = new List<Type>();
            FindModulesRecursive(featureModuleTypes, rootModule);

            foreach (var moduleType in featureModuleTypes)
            {
                var moduleInstance = (IAbstractModule)Activator.CreateInstance(moduleType);

                if (moduleInstance == null)
                    throw new InvalidOperationException(
                        $"Found module '{moduleType.AssemblyQualifiedName}' can not be instantiated");

                var moduleName = moduleType.GetCustomAttribute<ModuleAttribute>()?.Name;
                var moduleTags = moduleType.GetCustomAttribute<TagAttribute>()?.Tags;

                var featureModuleInfo = new ModuleInfo(moduleType, moduleName, moduleInstance, moduleTags);
                featureModuleInfos.Add(featureModuleInfo);

                serviceCollection.AddSingleton(moduleType, moduleInstance);
            }

            return featureModuleInfos;
        }

        private void FindModulesRecursive(IList<Type> featureModuleTypes, Type featureModule)
        {
            if (!typeof(IAbstractModule).GetTypeInfo().IsAssignableFrom(featureModule))
                throw new ArgumentException($"Not possible to load module '{featureModule.FullName}'. Is not type of AbstractModule!");

            if (featureModuleTypes.Contains(featureModule))
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

        public IList<ModuleInfo> LoadModulesByDll(IServiceCollection serviceCollection, Type rootModule)
        {
            var moduleInfoList = GetListOfModules();

            var configuration = serviceCollection.GetConfiguration();

            foreach (var moduleInfo in moduleInfoList.ToList())
            {
                var moduleInstance = (IAbstractModule)Activator.CreateInstance(moduleInfo.Type);

                if (moduleInstance == null)
                    throw new InvalidOperationException(
                        $"Found module '{moduleInfo.Type.AssemblyQualifiedName}' can not be instantiated");

                moduleInfo.Instance = moduleInstance;
                moduleInstance.PreConfigureModules(moduleInfoList, configuration);
            }

            foreach (var moduleInfo in moduleInfoList)
            {
                serviceCollection.AddSingleton(moduleInfo.Type, moduleInfo.Instance);
            }

            return moduleInfoList;
        }

        private IList<ModuleInfo> GetListOfModules()
        {
            var moduleInfos = new List<ModuleInfo>();
            var assemblies = GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var modules = GetModulesFromAssembly(assembly);
                if (modules.Count <= 0)
                    continue;

                foreach (var module in modules)
                {
                    var moduleName = module.GetCustomAttribute<ModuleAttribute>()?.Name;
                    var tags = module.GetCustomAttribute<TagAttribute>()?.Tags.ToList();

                    moduleInfos.Add(new ModuleInfo(module, moduleName, null, tags));
                }
            }

            return moduleInfos;
        }

        private IList<Assembly> GetAssemblies()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return ModuleAssemblyFinder.LoadAssembliesFromPath(path, SearchOption.TopDirectoryOnly, "");
        }

        private IList<Type> GetModulesFromAssembly(Assembly assembly)
        {
            var modules = assembly.GetTypes()
                .Where(type => type.GetCustomAttribute<ModuleAttribute>() != null)
                .ToList();

            foreach (var module in modules.Where(module =>
                !typeof(IAbstractModule).GetTypeInfo().IsAssignableFrom(module)))
            {
                throw new ArgumentException($"Not possible to load module '{module.FullName}'. Is not type of AbstractModule!");
            }

            return modules;
        }
    }
}
