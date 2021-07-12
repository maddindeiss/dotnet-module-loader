using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace ModuleLoader.Core
{
    public class ModuleInfo : IModuleInfo
    {
        public Type Type { get; }
        public IAbstractModule Instance { get; }
        public Assembly Assembly { get; }
        public string Name { get; }

        private readonly IList<ModuleInfo> _dependencies;
        public IReadOnlyList<ModuleInfo> Dependencies
        {
            get
            {
                return _dependencies.ToImmutableList();
            }
        }

        public ModuleInfo(Type type, IAbstractModule instance)
        {
            _dependencies = new List<ModuleInfo>();
            Type = type;
            Instance = instance;
            Assembly = type.Assembly;
        }

        public ModuleInfo(Assembly assembly, Type type, string name, IAbstractModule instance)
        {
            _dependencies = new List<ModuleInfo>();
            Type = type;
            Instance = instance;
            Assembly = assembly;
            Name = name;
        }

        public void AddDependency(ModuleInfo moduleInfo)
        {
            if(!_dependencies.Contains(moduleInfo))
                _dependencies.Add(moduleInfo);
        }
    }
}
