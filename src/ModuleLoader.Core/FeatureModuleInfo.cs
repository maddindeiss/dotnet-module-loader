using System;

namespace ModuleLoader.Core
{
    public class FeatureModuleInfo
    {
        public Type Type { get; }

        public IFeatureModule Instance { get; }

        /*
        private readonly IList<FeatureModuleInfo> _dependencies;
        public IReadOnlyList<FeatureModuleInfo> Dependencies
        {
            get
            {
                return _dependencies.ToImmutableList();
            }
        }
        */

        public FeatureModuleInfo(Type type, IFeatureModule instance)
        {
            // _dependencies = new List<FeatureModuleInfo>();
            Type = type;
            Instance = instance;
        }
    }
}
