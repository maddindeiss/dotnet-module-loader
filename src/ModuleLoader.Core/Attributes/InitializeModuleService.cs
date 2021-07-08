using System;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InitializeModule: Attribute
    {
        public Type InitializeService { get; }

        public InitializeModule(Type initializeService)
        {
            InitializeService = initializeService;
        }
    }
}
