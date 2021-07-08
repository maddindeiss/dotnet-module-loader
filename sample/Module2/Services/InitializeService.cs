using System;
using ModuleLoader.Core;

namespace Module2.Services
{
    public class InitializeService: IInitializeService
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
