using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IsDependingOnModule: Attribute
    {
        public Type DependingModule { get; }

        public IsDependingOnModule(Type dependingModule)
        {
            DependingModule = dependingModule;
        }
    }
}
