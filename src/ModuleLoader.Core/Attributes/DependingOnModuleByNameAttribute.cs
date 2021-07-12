using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependingOnModuleByNameAttribute: Attribute
    {
        public string DependingModule { get; }

        public DependingOnModuleByNameAttribute(string dependingModule)
        {
            DependingModule = dependingModule;
        }
    }
}
