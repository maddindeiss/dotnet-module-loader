using System;
using System.Reflection;

namespace ModuleLoader.Core
{
    public interface IModuleInfo
    {
        Type Type { get; }
        IAbstractModule Instance { get; }
        Assembly Assembly { get; }
        string Name { get; }
    }
}
