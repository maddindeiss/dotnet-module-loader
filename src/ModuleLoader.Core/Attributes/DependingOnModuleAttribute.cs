using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependingOnModuleAttribute: Attribute
    {
        public Type DependingModule { get; }

        public DependingOnModuleAttribute(Type dependingModule)
        {
            DependingModule = dependingModule;
        }
    }
}
