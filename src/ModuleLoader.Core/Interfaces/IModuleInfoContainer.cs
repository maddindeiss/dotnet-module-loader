using System.Collections.Generic;

namespace ModuleLoader.Core
{
    public interface IModuleInfoContainer
    {
        IList<ModuleInfo> ModulesInfo { get; }
    }
}
