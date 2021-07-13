using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module2.Services;
using ModuleLoader.Core;
using ModuleLoader.Core.Attributes;

namespace Module2
{
    [Module("module2")]
    [Tag("module")]
    [InitializeModule(typeof(InitializeService))]
    public class Module2Module: AbstractModule
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Module2Module ConfigureServices");
        }

        public override void PreConfigureModules(IList<ModuleInfo> modulesInfo, IConfiguration configuration)
        {
            var myName = this.GetType().GetCustomAttribute<ModuleAttribute>()?.Name;
            var myTags = this.GetType().GetCustomAttribute<TagAttribute>()?.Tags;

            var moduleToRemove = modulesInfo
                .Where(module => module.Tags.Any(moduleTag => myTags.Any(myTag => myTag == moduleTag)))
                //.Where(module => module.Tags.Contains("module"))
                .Single(module => module.Name == "module1");

            Console.WriteLine($"Module {myName} has same tag as {moduleToRemove.Name} -> removing module {moduleToRemove.Name}");

            modulesInfo.Remove(moduleToRemove);
        }
    }
}
