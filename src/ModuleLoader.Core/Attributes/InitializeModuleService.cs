using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InitializeModuleAttribute: Attribute
    {
        public Type InitializeService { get; }

        public InitializeModuleAttribute(Type initializeService)
        {
            InitializeService = initializeService;
        }
    }
}
