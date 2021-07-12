using System;
using System.Threading.Tasks;
using ModuleLoader.Core;

namespace Module2.Services
{
    public class InitializeService: IModuleInitialization
    {
        public InitializeService()
        {
            Console.WriteLine("Module2: InitializeService");
        }

        public async Task Initialize()
        {
            Console.WriteLine("Module2: InitializeService Initialize");
        }
    }
}
