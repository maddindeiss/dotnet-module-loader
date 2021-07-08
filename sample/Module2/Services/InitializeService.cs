using System;
using ModuleLoader.Core;

namespace Module1.Services
{
    public class InitializeService: IServiceInitialization
    {
        public InitializeService()
        {
            Console.WriteLine("Module2: InitializeService");
        }

        public void Initialize()
        {
            Console.WriteLine("Module2: InitializeService Initialize");
        }
    }
}
