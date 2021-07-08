using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InitializeModuleService: Attribute
    {
        public Type InitializeService { get; }

        public InitializeModuleService(Type initializeService)
        {
            InitializeService = initializeService;
        }
    }
}
