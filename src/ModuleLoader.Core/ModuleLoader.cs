using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using ModuleLoader.Core.Attributes;
using ModuleLoader.Core.Extensions;

namespace ModuleLoader.Core
{
    public class ModuleLoader
    {
        public IList<ModuleInfo> LoadModules(IServiceCollection serviceCollection, Type rootModule)
        {
            var moduleInfoList = GetListOfModules();

            var configuration = serviceCollection.GetConfiguration();

            foreach (var moduleInfo in moduleInfoList.ToList()) {
                var moduleInstance = (IAbstractModule)Activator.CreateInstance(moduleInfo.Type);

                if (moduleInstance == null)
                    throw new InvalidOperationException($"Found module '{moduleInfo.Type.AssemblyQualifiedName}' can not be instantiated");

                moduleInfo.Instance = moduleInstance;
                moduleInstance.ConfigureModules(moduleInfoList, configuration);
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
                if(modules.Count <= 0)
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

            return ModuleFinder.LoadAssemblies(path, SearchOption.TopDirectoryOnly, "");
        }

        private IList<Type> GetModulesFromAssembly(Assembly assembly)
        {
            var modules = assembly.GetTypes()
                .Where(type => type.GetCustomAttribute<ModuleAttribute>() != null)
                .ToList();

            foreach (var module in modules.Where(module => !typeof(IAbstractModule).GetTypeInfo().IsAssignableFrom(module)))
            {
                throw new ArgumentException($"Module '{module.AssemblyQualifiedName}' can not be loaded!");
            }

            return modules;
        }
    }
}
