using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute: Attribute
    {
        public string Name { get; }

        public ModuleAttribute(string name)
        {
            Name = name.ToLowerInvariant();
        }
    }
}
