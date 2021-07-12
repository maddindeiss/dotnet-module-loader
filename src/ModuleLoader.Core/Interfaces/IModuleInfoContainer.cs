using System.Collections.Generic;

namespace ModuleLoader.Core
{
    public interface IModuleInfoContainer
    {
        IReadOnlyList<IModuleInfo> ModulesInfo { get; }
    }
}
